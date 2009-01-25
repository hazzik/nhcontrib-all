using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using log4net;
using NHibernate.Search.Backend.Impl.Lucene;
using NHibernate.Search.Engine;
using NHibernate.Search.Impl;
using NHibernate.Util;

namespace NHibernate.Search.Backend.Impl
{
    /// <summary>
    ///  Batch work until <c>ExecuteQueue</c> is called.
    ///  The work is then executed synchronously or asynchronously
    /// </summary>
    public class BatchedQueueingProcessor : IQueueingProcessor
    {
        private readonly ILog log = LogManager.GetLogger(typeof (BatchedQueueingProcessor));
        private readonly IBackendQueueProcessorFactory backendQueueProcessorFactory;
        private readonly int batchSize;
        private readonly ISearchFactoryImplementor searchFactory;
        private readonly bool sync;
        private bool continueProcessing;

        public BatchedQueueingProcessor(ISearchFactoryImplementor searchFactory, IDictionary<string,string> properties)
        {
            this.searchFactory = searchFactory;
            //default to sync if none defined
            sync = !"async".Equals((string) properties[Environment.WorkerExecution],StringComparison.InvariantCultureIgnoreCase);

            int.TryParse(properties[Environment.WorkerBatchSize], out batchSize);

            string backend = (string)properties[Environment.WorkerBackend];
            if (StringHelper.IsEmpty(backend) || "lucene".Equals(backend, StringComparison.InvariantCultureIgnoreCase))
            {
                backendQueueProcessorFactory = new LuceneBackendQueueProcessorFactory();
            }
            else
            {
                try
                {
                    System.Type processorFactoryClass = ReflectHelper.ClassForName(backend);
                    backendQueueProcessorFactory =
                        (IBackendQueueProcessorFactory) Activator.CreateInstance((System.Type) processorFactoryClass);
                }
                catch (Exception e)
                {
                    throw new SearchException("Unable to find/create processor class: " + backend, e);
                }
            }
            backendQueueProcessorFactory.Initialize(properties, searchFactory);
            searchFactory.BackendQueueProcessorFactory = backendQueueProcessorFactory;
        }

        //TODO implements parallel batchWorkers (one per Directory)

        #region IQueueingProcessor Members

        public void Add(Work work, WorkQueue workQueue)
        {
            workQueue.Add(work);
            if (batchSize > 0 && workQueue.Count >= batchSize)
            {
                WorkQueue subQueue = workQueue.SplitQueue();
                PrepareWorks(subQueue);
                PerformWorks(subQueue);
            }
        }

        public void PerformWorks(WorkQueue workQueue)
        {
            WaitCallback processor = backendQueueProcessorFactory.GetProcessor(workQueue.GetSealedQueue());
            if (sync)
            {
                processor(null);
            }
            else
            {
                ThreadPool.QueueUserWorkItem(delegate(object state)
                {
                    if (continueProcessing)
                        processor(state);
                });
            }
        }

        public void CancelWorks(WorkQueue workQueue)
        {
            workQueue.Clear();
        }

        public void PrepareWorks(WorkQueue workQueue)
        {
            List<Work> queue = workQueue.GetQueue();
            int initialSize = queue.Count;
            List<LuceneWork> luceneQueue = new List<LuceneWork>(initialSize); //TODO load factor for containedIn
            /**
			 * Collection work type are processed second, so if the owner entity has already been processed for whatever reason
			 * the work will be ignored.
			 * However if the owner entity has not been processed, an "UPDATE" work is executed
			 *
			 * Processing collection works last is mandatory to avoid reindexing a object to be deleted
			 */
            ProcessWorkByLayer(queue, initialSize, luceneQueue, Layer.First);
            ProcessWorkByLayer(queue, initialSize, luceneQueue, Layer.Second);
            workQueue.SetSealedQueue(luceneQueue);
        }

        #endregion

        private void ProcessWorkByLayer(IList<Work> queue, int initialSize, List<LuceneWork> luceneQueue, Layer layer)
        {
            /* By Kailuo Wang
             * This sequence of the queue is reversed which is different from the Java version
             * By reversing the sequence here, it ensures that the work that is added to the queue later has higher priority.
             * I did this to solve the following problem I encountered:
             * If you update an entity before deleting it in the same transaction,
             * There will be two Works generated by the event listener: Update Work and Delete Work.
             * However, the update Work will prevent the Delete Work from being added to the queue and thus 
             * fail purging the index for that entity. 
             * I am not sure if the Java version has the same problem.
             */
            for (int i = initialSize - 1; i >= 0; i--)
            {
                Work work = queue[i];
                if (work == null || !layer.IsRightLayer(work.WorkType)) 
                    continue;
                queue[i] = null; // help GC and avoid 2 loaded queues in memory
                System.Type entityClass = NHibernateUtil.GetClass(work.Entity);
                DocumentBuilder builder = searchFactory.DocumentBuilders[entityClass];
                if (builder == null) continue; //or exception?
                builder.AddToWorkQueue(entityClass, work.Entity, work.Id, work.WorkType, luceneQueue, searchFactory);
            }
        }

        public void Close()
        {
            // hibernate search stops the thread pool here.
            // we just set the stop flag
            continueProcessing = false;
        }

        #region Nested type: Layer

        private class Layer
        {
            public static readonly Layer First = new Layer(delegate(WorkType type)
            {
                return type != WorkType.Collection;
            });
            public static readonly Layer Second = new Layer(delegate(WorkType type)
            {
                return type == WorkType.Collection;
            });

            public delegate bool IsRightLayerDelegate(WorkType type);

            private readonly IsRightLayerDelegate isRightLayer;

            protected Layer(IsRightLayerDelegate isRightLayer)
            {
                this.isRightLayer = isRightLayer;
            }

            public bool IsRightLayer(WorkType type)
            {
                return isRightLayer(type);
            }
        }

        #endregion
    }
}