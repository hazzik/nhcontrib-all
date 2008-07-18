using Lucene.Net.Analysis;
using Lucene.Net.Store;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Search.Event;
using NHibernate.Search.Impl;
using NHibernate.Search.Store;
using NUnit.Framework;
using TestCase=NHibernate.Test.TestCase;

namespace NHibernate.Search.Tests
{
    [TestFixture]
    public abstract class SearchTestCase : TestCase
    {
        protected Directory GetDirectory(System.Type clazz)
        {
            return SearchFactoryImpl.GetSearchFactory(cfg).GetDirectoryProvider(clazz).Directory;
        }

        protected override void Configure(Configuration configuration)
        {
            cfg.SetProperty("hibernate.search.default.directory_provider",
                            typeof(RAMDirectoryProvider).AssemblyQualifiedName);
            cfg.SetProperty(Environment.AnalyzerClass, typeof(StopAnalyzer).AssemblyQualifiedName);
            SetListener(cfg);
            cfg.Configure();
        }

        public static void SetListener(Configuration configure)
        {
            configure.SetListener(ListenerType.PostUpdate, new FullTextIndexEventListener());
            configure.SetListener(ListenerType.PostInsert, new FullTextIndexEventListener());
            configure.SetListener(ListenerType.PostDelete, new FullTextIndexEventListener());
        }

        protected override string MappingsAssembly
        {
            get { return "NHibernate.Search.Tests"; }
        }
    }
}