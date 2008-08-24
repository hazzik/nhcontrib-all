using System.Collections.Generic;
using System.IO;
using Iesi.Collections.Generic;
using Lucene.Net.Index;
using Lucene.Net.Search;
using NHibernate.Search.Engine;
using NHibernate.Search.Store;
using Directory=Lucene.Net.Store.Directory;

namespace NHibernate.Search
{
    public class FullTextSearchHelper
    {
        public static Lucene.Net.Search.Query FilterQueryByClasses(ISet<System.Type> classesAndSubclasses,
                                                                   Lucene.Net.Search.Query luceneQuery)
        {
            //A query filter is more practical than a manual class filtering post query (esp on scrollable resultsets)
            //it also probably minimise the memory footprint
            if (classesAndSubclasses == null)
                return luceneQuery;
            else
            {
                BooleanQuery classFilter = new BooleanQuery();
                //annihilate the scoring impact of DocumentBuilder.CLASS_FIELDNAME
                classFilter.SetBoost(0);
                foreach (System.Type clazz in classesAndSubclasses)
                {
                    Term t = new Term(DocumentBuilder.CLASS_FIELDNAME, clazz.AssemblyQualifiedName);
                    TermQuery termQuery = new TermQuery(t);
                    classFilter.Add(termQuery, BooleanClause.Occur.SHOULD);
                }
                BooleanQuery filteredQuery = new BooleanQuery();
                filteredQuery.Add(luceneQuery, BooleanClause.Occur.MUST);
                filteredQuery.Add(classFilter, BooleanClause.Occur.MUST);
                return filteredQuery;
            }
        }

        public static Searcher BuildSearcher(ISearchFactoryImplementor searchFactory,
                                             out ISet<System.Type> classesAndSubclasses,
                                             params System.Type[] classes)
        {
            Dictionary<System.Type, DocumentBuilder> builders = searchFactory.DocumentBuilders;
            ISet<IDirectoryProvider> directories = new HashedSet<IDirectoryProvider>();
            if (classes == null || classes.Length == 0)
            {
                //no class means all classes
                foreach (DocumentBuilder builder in builders.Values)
                {
                    foreach (IDirectoryProvider provider in builder.DirectoryProvidersSelectionStrategy.GetDirectoryProvidersForAllShards())
                        directories.Add(provider);
                }
                classesAndSubclasses = null;
            }
            else
            {
                ISet<System.Type> involvedClasses = new HashedSet<System.Type>();
                involvedClasses.AddAll(classes);
                foreach (System.Type clazz in classes)
                {
                    DocumentBuilder builder;
                    builders.TryGetValue(clazz, out builder);
                    if (builder != null) involvedClasses.AddAll(builder.MappedSubclasses);
                }
                foreach (System.Type clazz in involvedClasses)
                {
                    DocumentBuilder builder;
                    builders.TryGetValue(clazz, out builder);
                    //TODO should we rather choose a polymorphic path and allow non mapped entities
                    if (builder == null) 
                        throw new HibernateException("Not a mapped entity: " + clazz);

                    foreach (IDirectoryProvider provider in builder.DirectoryProvidersSelectionStrategy.GetDirectoryProvidersForAllShards())
                        directories.Add(provider);
                }
                classesAndSubclasses = involvedClasses;
            }

            IDirectoryProvider[] directoryProviders = new List<IDirectoryProvider>(directories).ToArray();
            return new IndexSearcher(searchFactory.ReaderProvider.OpenReader(directoryProviders));
        }

        //public static Searcher GetSearcher(ISet<IDirectoryProvider> directories)
        //{
        //    if (directories.Count == 0)
        //        return null;
        //    //set up the searcher
        //    int dirNbr = directories.Count;
        //    int dirNbr = directories.Count;
        //    IndexSearcher[] searchers = new IndexSearcher[dirNbr];
        //    try
        //    {
        //        int index = 0;
        //        foreach (Directory directory in directories)
        //        {
        //            if (dirNbr == 1)
        //                return new IndexSearcher(directory);
        //            searchers[index] = new IndexSearcher(directory);
        //            index += 1;
        //        }
        //        return new MultiSearcher(searchers);
        //    }
        //    catch (IOException e)
        //    {
        //        throw new HibernateException("Unable to read Lucene directory", e);
        //    }
        //}
    }
}