using System.Collections;
using System.IO;
using System.Threading;
using Lucene.Net.Analysis;
using Lucene.Net.QueryParsers;
using NHibernate.Cfg;
using NHibernate.Search.Impl;
using NHibernate.Search.Store;
using NUnit.Framework;

namespace NHibernate.Search.Tests.Worker
{
    [TestFixture, Ignore("Only partially ported - can't figure out what this is testing.")]
    public class WorkerTestCase : SearchTestCase
    {
        protected override void Configure(Configuration configuration)
        {
            base.Configure(configuration);
            DirectoryInfo sub = BaseIndexDir;
            configuration.SetProperty("hibernate.search.default.indexBase", sub.FullName);
            configuration.SetProperty("hibernate.search.Clock.directory_provider", typeof(FSDirectoryProvider).AssemblyQualifiedName);

            configuration.SetProperty(Environment.AnalyzerClass, typeof(StopAnalyzer).AssemblyQualifiedName);
        }

        protected override void OnSetUp()
        {
            DirectoryInfo sub = BaseIndexDir;
            sub.Create();
            foreach (DirectoryInfo directoryInfo in sub.GetDirectories())
                directoryInfo.Delete(true);
            BuildSessionFactory(); //we need a fresh one per test
        }

        protected override void OnTearDown()
        {
            BaseIndexDir.Delete(true);
        }

        private DirectoryInfo BaseIndexDir
        {
            get { return new DirectoryInfo(Path.Combine(".", "indextemp")); }
        }

        protected override IList Mappings
        {
            get
            {
                return new string[]
                           {
                               "Worker.Employee.hbm.xml",
                               "Worker.Employer.hbm.xml",
                               "Worker.Food.hbm.xml",
                               "Worker.Drink.hbm.xml",
                           };
            }
        }

        protected class Work
        {
            private readonly ISessionFactory sf;
            public volatile int count;

            public Work(ISessionFactory sf)
            {
                this.sf = sf;
            }

            public void Run(object ignored)
            {
                ISession s = sf.OpenSession();
                ITransaction tx = s.BeginTransaction();
                Employee ee = new Employee();
                ee.Name = ("Emmanuel");
                s.Save(ee);
                Employer er = new Employer();
                er.Name = ("RH");
                s.Save(er);
                tx.Commit();
                s.Close();

                s = sf.OpenSession();
                tx = s.BeginTransaction();
                ee = (Employee) s.Get(typeof(Employee), ee.Id);
                ee.Name = ("Emmanuel2");
                er = (Employer) s.Get(typeof(Employer), er.Id);
                er.Name = ("RH2");
                tx.Commit();
                s.Close();

                s = sf.OpenSession();
                tx = s.BeginTransaction();
                IFullTextSession fts = new FullTextSessionImpl(s);
                QueryParser parser = new QueryParser("id", new StopAnalyzer());
                Lucene.Net.Search.Query query;
                query = parser.Parse("name:emmanuel2");

                bool results = fts.CreateFullTextQuery(query).List().Count > 0;
                //don't test because in case of async, it query happens before actual saving
                //if ( !results ) throw new RuntimeException( "No results!" );
                tx.Commit();
                s.Close();

                s = sf.OpenSession();
                tx = s.BeginTransaction();
                ee = (Employee) s.Get(typeof(Employee), ee.Id);
                s.Delete(ee);
                er = (Employer) s.Get(typeof(Employee), er.Id);
                s.Delete(er);
                tx.Commit();
                s.Close();
                count++;
            }
        }

        protected class ReverseWork
        {
            private readonly ISessionFactory sf;

            public ReverseWork(ISessionFactory sf)
            {
                this.sf = sf;
            }

            public void Run(object ignored)
            {
                ISession s = sf.OpenSession();
                ITransaction tx = s.BeginTransaction();
                Employer er = new Employer();
                er.Name = ("RH");
                s.Save(er);
                Employee ee = new Employee();
                ee.Name = ("Emmanuel");
                s.Save(ee);
                tx.Commit();
                s.Close();

                s = sf.OpenSession();
                tx = s.BeginTransaction();
                er = (Employer) s.Get(typeof(Employer), er.Id);
                er.Name = ("RH2");
                ee = (Employee) s.Get(typeof(Employee), ee.Id);
                ee.Name = ("Emmanuel2");
                tx.Commit();
                s.Close();

                s = sf.OpenSession();
                tx = s.BeginTransaction();
                er = (Employer) s.Get(typeof(Employer), er.Id);
                s.Delete(er);
                ee = (Employee) s.Get(typeof(Employee), ee.Id);
                s.Delete(ee);
                tx.Commit();
                s.Close();
            }
        }

        [Test]
        public void Concurrency()
        {
            Work work = new Work(sessions);
            ReverseWork reverseWork = new ReverseWork(sessions);
            int iteration = 100;
            for (int i = 0; i < iteration; i++)
            {
                ThreadPool.QueueUserWorkItem(work.Run);
                ThreadPool.QueueUserWorkItem(reverseWork.Run);
            }
            while (work.count < iteration - 1)
                Thread.Sleep(20);
        }
    }
}