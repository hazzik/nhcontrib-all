using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NHibernate.Linq.SqlClient;
using NHibernate.Linq.Visitors;
using NHibernate.Linq.Visitors.MethodTranslators;
using NUnit.Framework;
using NHibernate.Linq.Tests.Entities;

namespace NHibernate.Linq.Tests
{
	public class BaseTest
	{
		protected NorthwindContext db;
		protected TestContext nhib;
		protected NorthwindContext nwnd;
		protected ISession session;

		protected virtual string ConnectionStringName
		{
			get { return "Northwind"; }
		}

		static BaseTest()
		{
			new	GlobalSetup().SetupNHibernate();
		}

		[SetUp]
		public virtual void Setup()
		{
			session = CreateSession();
			nwnd = db = new NorthwindContext(session);
			nhib = new TestContext(session);
			MethodTranslatorRegistry.Current.RegisterTranslator<string, StringMethodTranslator>();
			MethodTranslatorRegistry.Current.RegisterTranslator(typeof(System.Linq.Enumerable),
																typeof(EnumerableMethodTranslator));
			MethodTranslatorRegistry.Current.RegisterTranslator(typeof(System.Linq.Queryable),
																typeof(QueryableMethodTranslator));
			MethodTranslatorRegistry.Current.RegisterTranslator(typeof(List<>),
																typeof(ListMethodTranslator));
			MethodTranslatorRegistry.Current.RegisterTranslator(typeof(SqlClientExtensions),
																	typeof(DBFunctionMethodTranslator));
			MethodTranslatorRegistry.Current.RegisterTranslator(typeof(SqlFunctionExtensions),
																	typeof(DBFunctionMethodTranslator));
			MethodTranslatorRegistry.Current.RegisterTranslator(typeof(Queryable),
														typeof(QueryableMethodTranslator));
			MethodTranslatorRegistry.Current.RegisterTranslator(typeof(ICollection<>),
											typeof(CollectionMethodTranslator));
		}

		protected virtual ISession CreateSession()
		{
			IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString);
			con.Open();
			return GlobalSetup.CreateSession(con);
		}

		[TearDown]
		public void TearDown()
		{
			session.Connection.Dispose();
			session.Dispose();
			session = null;
		}
	}
}