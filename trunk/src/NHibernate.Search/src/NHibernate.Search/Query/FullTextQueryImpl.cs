using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Iesi.Collections.Generic;
using log4net;
using Lucene.Net.Search;
using NHibernate.Engine.Query;
using NHibernate.Impl;
using NHibernate.Search.Engine;
using NHibernate.Search.Impl;
using NHibernate.Search.Util;

namespace NHibernate.Search.Query
{
    using Filter;

    using NHibernate.Util;

    using Transform;

    public class FullTextQueryImpl : AbstractQueryImpl, IFullTextQuery
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FullTextQueryImpl));
        private readonly Dictionary<string, FullTextFilterImpl> filterDefinitions;
        private readonly Lucene.Net.Search.Query luceneQuery;
        private System.Type[] classes;
        private ISet<System.Type> classesAndSubclasses;
        private int resultSize;
        private Sort sort;
        private Lucene.Net.Search.Filter filter;
        private ICriteria criteria;
        private string[] indexProjection;
        private IResultTransformer resultTransformer;        
        private ISearchFactoryImplementor searchFactoryImplementor;

        #region Constructors

        /// <summary>
        /// classes must be immutable
        /// </summary>
        public FullTextQueryImpl(Lucene.Net.Search.Query query, System.Type[] classes, ISession session,
                                 ParameterMetadata parameterMetadata)
            : base(query.ToString(), FlushMode.Unspecified, session.GetSessionImplementation(), parameterMetadata)
        {
            luceneQuery = query;
            resultSize = -1;
            this.classes = classes;
            this.filterDefinitions = new Dictionary<string, FullTextFilterImpl>();
        }

        #endregion

        public IFullTextQuery SetSort(Sort value)
        {
            this.sort = value;
            return this;
        }

        public IFullTextQuery SetFilter(Lucene.Net.Search.Filter value)
        {
            this.filter = value;
            return this;
        }

        /// <summary>
        /// Return an interator on the results.
        /// Retrieve the object one by one (initialize it during the next() operation)
        /// </summary>
        public override IEnumerable<T> Enumerable<T>()
        {
            //implement an interator which keep the id/class for each hit and get the object on demand
            //cause I can't keep the searcher and hence the hit opened. I dont have any hook to know when the
            //user stop using it
            //scrollable is better in this area

            //find the directories
            IndexSearcher searcher = BuildSearcher();
            if (searcher == null)
            {
                return new IteratorImpl<T>(new List<EntityInfo>(), noLoader).Iterate();
            }

            try
            {
                Hits hits = GetHits(searcher);
                SetResultSize(hits);
                int first = First();
                int max = Max(first, hits);

                int size = max - first + 1 < 0 ? 0 : max - first + 1;
                IList<EntityInfo> infos = new List<EntityInfo>(size);
                DocumentExtractor extractor = new DocumentExtractor(SearchFactory, indexProjection);
                for (int index = first; index <= max; index++)
                {
                    //TODO use indexSearcher.getIndexReader().document( hits.id(index), FieldSelector(indexProjection) );
                    infos.Add(extractor.Extract(hits, index));
                }
                return new IteratorImpl<T>(infos, this.GetLoader(Session.GetSession())).Iterate();
            }
            catch (IOException e)
            {
                throw new HibernateException("Unable to query Lucene index", e);
            }
            finally
            {
                CloseSearcher(searcher);
            }
        }

        public override IEnumerable Enumerable()
        {
            return Enumerable<object>();
        }

        private ILoader GetLoader(ISession session)
        {
            if (indexProjection != null)
            {
                ProjectionLoader loader = new ProjectionLoader();
                loader.Init(session, SearchFactory, resultTransformer, indexProjection);
                return loader;
            }

            if (criteria != null)
            {
                if (classes.GetLength(0) > 1)
                    throw new SearchException("Cannot mix criteria and multiple entity types");

                if (criteria is CriteriaImpl)
                {
                    string targetEntity = ((CriteriaImpl)criteria).EntityOrClassName;
                    if (classes.GetLength(0) == 1 && classes[0].Name != targetEntity)
                    {
                        throw new SearchException("Criteria query entity should match query entity");
                    }

                    try
                    {
                        System.Type entityType = ReflectHelper.ClassForName(targetEntity);
                        classes = new System.Type[] {entityType};
                    }
                    catch (Exception e)
                    {
                        throw new SearchException("Unable to load indexed class: " + targetEntity, e);
                    }
                }

                QueryLoader loader = new QueryLoader();
                loader.Init(session, searchFactoryImplementor);
                loader.EntityType = classes[0];
                loader.Criteria = criteria;
                return loader;
            }

            if (classes.GetLength(0) == 1)
            {
                QueryLoader loader = new QueryLoader();
                loader.Init(session, searchFactoryImplementor);
                loader.EntityType = classes[0];
                return loader;
            }
            else
            {
                ObjectLoader loader = new ObjectLoader();
                loader.Init(session, searchFactoryImplementor);
                return loader;
            }
        }

		protected override IDictionary<string, LockMode> LockModes
        {
            get { return null; }
        }

        private ISearchFactoryImplementor SearchFactory
        {
            get
            {
                if (searchFactoryImplementor == null)
                    searchFactoryImplementor = ContextHelper.GetSearchFactoryBySFI(Session);
                return searchFactoryImplementor;
            }
        }

        public int ResultSize
        {
            get
            {
                if (resultSize < 0)
                {
                    //get result size without object initialization
                    IndexSearcher searcher = BuildSearcher();

                    if (searcher == null)
                        resultSize = 0;
                    else
                        try
                        {
                            resultSize = GetHits(searcher).Length();
                        }
                        catch (IOException e)
                        {
                            throw new HibernateException("Unable to query Lucene index", e);
                        }
                        finally
                        {
                            CloseSearcher(searcher);
                        }
                }
                return resultSize;
            }
        }

        public override IList<T> List<T>()
        {
            ArrayList arrayList = new ArrayList();
            List(arrayList);
            return (T[]) arrayList.ToArray(typeof(T));
        }

        public override IList List()
        {
            ArrayList arrayList = new ArrayList();
            List(arrayList);
            return arrayList;
        }

        public override void List(IList list)
        {
            //find the directories
            IndexSearcher searcher = BuildSearcher();
            if (searcher == null)
            {
                return;
            }
            
            try
            {
                Hits hits = GetHits(searcher);
                SetResultSize(hits);
                int first = First();
                int max = Max(first, hits);
                ISession sess = (ISession) Session;
                DocumentExtractor extractor = new DocumentExtractor(SearchFactory, indexProjection);
                ILoader loader = this.GetLoader(sess);                
                for (int index = first; index <= max; index++)
                {
                    list.Add(loader.Load(extractor.Extract(hits, index)));
                }

                if (resultTransformer == null || loader is ProjectionLoader)
                {
                    // stay consistent with transformTuple which can only be executed during a projection
                }
                else
                {
                    IList tempList = resultTransformer.TransformList(list);
                    list.Clear();
                    foreach (object entity in tempList)
                    {
                        list.Add(entity);
                    }
                }
                
            }
            catch (IOException e)
            {
                throw new HibernateException("Unable to query Lucene index", e);
            }
            finally
            {
                CloseSearcher(searcher);
            }
        }

        public override IQuery SetLockMode(string alias, LockMode lockMode)
        {
            return null;
        }

        public override int ExecuteUpdate()
        {
            throw new HibernateException("Not supported operation");
        }

        public IFullTextQuery SetCriteriaQuery(ICriteria value)
        {
            this.criteria = value;
            return this;
        }

        public IFullTextQuery SetProjection(params string[] fields)
        {
            if (fields == null || fields.Length == 0)
                this.indexProjection = null;
            else
                this.indexProjection = fields;

            return this;
        }

        public IFullTextFilter EnableFullTextFilter(string name)
        {
            FullTextFilterImpl filterDefinition;
            if (filterDefinitions.TryGetValue(name, out filterDefinition))
                return filterDefinition;

            filterDefinition = new FullTextFilterImpl();
            filterDefinition.Name = name;
            FilterDef filterDef = SearchFactory.GetFilterDefinition(name);
            if (filterDef == null)
                throw new SearchException("Unknown FullTextFilter: " + name);

            filterDefinitions[name] = filterDefinition;

            return filterDefinition;
        }

        public void DisableFullTextFilter(string name)
        {
            filterDefinitions.Remove(name);
        }

        public new IFullTextQuery SetFirstResult(int value)
        {
            // NB Base owns this value via Selection instance
            base.SetFirstResult(value);
            return this;
        }

        public new IFullTextQuery SetMaxResults(int value)
        {
            // NB Base owns this value via Selection instance
            base.SetMaxResults(value);
            return this;
        }

        public new IFullTextQuery SetFetchSize(int value)
        {
            // NB Base owns this value via Selection instance
            base.SetFetchSize(value);
            return this;
        }

        public new IFullTextQuery SetResultTransformer(IResultTransformer transformer)
        {
            base.SetResultTransformer(transformer);
            this.resultTransformer = resultTransformer;
            return this;
        }

        private Hits GetHits(Searcher searcher)
        {
            Lucene.Net.Search.Query query = FullTextSearchHelper.FilterQueryByClasses(classesAndSubclasses, luceneQuery);
            BuildFilters();
            Hits hits = searcher.Search(query, this.filter, this.sort);
            this.SetResultSize(hits);

            return hits;
        }

        private void BuildFilters()
        {
            if (filterDefinitions.Count > 0)
            {
                ChainedFilter chainedFilter = new ChainedFilter();
                foreach (FullTextFilterImpl filterDefinition in filterDefinitions.Values)
                {
                    FilterDef def = SearchFactory.GetFilterDefinition(filterDefinition.Name);
                    object instance;
                    try
                    {
                        instance = Activator.CreateInstance(def.Impl);
                    }
                    catch (Exception e)
                    {
                        throw new HibernateException("Unable to instantiate FullTextFilterDef: " + def.Impl.FullName, e);
                    }

                    foreach (KeyValuePair<string,object> entry in filterDefinition.Parameters)
                    {
                        def.Invoke(entry.Key, instance, entry.Value);
                    }

                    if (def.Cache && def.KeyMethod == null && filterDefinition.Parameters.Count > 0)
                        throw new SearchException("Filter with parameters and no Key method: " + filterDefinition.Name);

                    FilterKey key = null;
                    if (def.Cache)
                    {
                        if (def.KeyMethod == null)
                        {
                            key = new ImplFilterKey();
                        }
                        else
                        {
                            try
                            {
                                key = (FilterKey)def.KeyMethod.Invoke(null, new object[] { instance });
                            }
                            catch (InvalidCastException)
                            {
                                throw new SearchException("Key method does not return FilterKey: " + def.Impl.Name + "." + def.KeyMethod.Name);
                            }
                            // TODO: More specific exception filtering here
                            catch (Exception e)
                            {
                                throw new SearchException("Error accessing Key method: " + def.Impl.Name + "." + def.KeyMethod.Name, e);
                            }
                        }

                        key.Impl = def.Impl;
                    }

                    Lucene.Net.Search.Filter f = def.Cache
                                                 ? SearchFactory.GetFilterCachingStrategy().GetCachedFilter(key)
                                                 : null;

                    if (f == null)
                    {
                        if (def.FactoryMethod != null)
                        {
                            try
                            {
                                f = (Lucene.Net.Search.Filter)def.FactoryMethod.Invoke(null, new object[] { instance });
                            }
                            catch (InvalidCastException)
                            {
                                throw new SearchException("Factory method does not return Lucene.Net.Search.Filter: " + def.FactoryMethod.Name);
                            }
                            // TODO: More specific exception filtering here
                            catch (Exception e)
                            {
                                throw new SearchException("Error accessing Factory method: " + def.FactoryMethod.Name, e);
                            }
                        }
                        else
                        {
                            try
                            {
                                f = (Lucene.Net.Search.Filter) instance;
                            }
                            catch (InvalidCastException)
                            {
                                throw new SearchException("Class is not a Lucene.Net.Search.Filter: " + def.Impl.Name);
                            }
                        }
                        if (def.Cache)
                        {
                            SearchFactory.GetFilterCachingStrategy().AddCachedFilter(key, f);
                        }
                    }

                    chainedFilter.AddFilter(f);
                }

                if (filter != null)
                    chainedFilter.AddFilter(filter);

                filter = chainedFilter;
            }
        }

        private void CloseSearcher(IndexSearcher searcher)
        {
            if (searcher == null)
            {
                return;
            }

            try
            {
                //SearchFactory.GetReaderProvider().CloseReader(searcher.GetIndexReader());
                searcher.Close();
            }
            catch (IOException e)
            {
                log.Warn("Unable to properly close searcher during lucene query: " + QueryString, e);
            }
        }

        private IndexSearcher BuildSearcher()
        {
            // Java version is inline, but we need the same code in LuceneQueryExpression
            return FullTextSearchHelper.BuildSearcher(SearchFactory, out classesAndSubclasses, classes);
        }

        private int Max(int first, Hits hits)
        {
            if (Selection.MaxRows == NHibernate.Engine.RowSelection.NoValue)
                return hits.Length() - 1;

            if (Selection.MaxRows + first < hits.Length())
                return first + Selection.MaxRows - 1;

            return hits.Length() - 1;
        }

        private int First()
        {
            return Selection.FirstRow != NHibernate.Engine.RowSelection.NoValue ? Selection.FirstRow : 0;
        }

        private void SetResultSize(Hits hits)
        {
            resultSize = hits.Length();
        }

        #region Nested type: ImplFilterKey

        private class ImplFilterKey : FilterKey
        {
        }

        #endregion

        #region Nested type: NoLoader

        private static readonly ILoader noLoader = new NoLoader();

	    private class NoLoader : ILoader
        {
	        public void Init(ISession session, ISearchFactoryImplementor searchFactoryImplementor) {
		    }

		    public object Load(EntityInfo entityInfo) 
            {
			    throw new NotSupportedException( "noLoader should not be used" );
		    }

            public IList Load(params EntityInfo[] entityInfos)
            {
			    throw new NotSupportedException( "noLoader should not be used" );
		    }
        };

        #endregion

        #region Nested type: IteratorImpl

        private class IteratorImpl<T>
        {
            private readonly IList<EntityInfo> entityInfos;
            private readonly ILoader loader;

            public IteratorImpl(IList<EntityInfo> entityInfos, ILoader loader)
            {
                this.entityInfos = entityInfos;
                this.loader = loader;
            }

            public IEnumerable<T> Iterate()
            {
                foreach (EntityInfo entityInfo in entityInfos)
                    yield return (T) loader.Load(entityInfo);
            }
        }

        #endregion
    }
}