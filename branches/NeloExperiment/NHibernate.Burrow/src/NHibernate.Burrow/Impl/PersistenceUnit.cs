using System;
using NHibernate.Burrow.Util;

namespace NHibernate.Burrow.Impl
{
    /// <summary>
    /// A persistant Unit is a unit of a ORM management
    /// </summary>
    /// <remarks>
    /// it consists of a Database, a NHibernate SessionManager, a Nhibernate SessionFactory.
    /// It can be shared by multiple domain layer assemblies 
    /// It's heavy weight
    /// </remarks>
    internal class PersistenceUnit
    {
        private readonly IPersistenceUnitCfg configuration;
        private readonly Cfg.Configuration nHConfiguration;
        private ISessionFactory sessionFactory;

        internal PersistenceUnit(IPersistenceUnitCfg cfg)
        {
            configuration = cfg;
            nHConfiguration = CreateNHConfiguration();
            ReBuildSessionfactory();
            //Temporarily removed auditLog before we decided whether it should stay in Burrow
            //if (Configuration.EnableAuditLog)
            //    interceptorFactory = AuditLogInterceptorFactory.Instance;
        }

        /// <summary>
        /// the name of the PU
        /// </summary>
        /// <remarks>
        /// Set at the configuration File
        /// </remarks>
        public string Name
        {
            get { return configuration.Name; }
        }

        /// <summary>
        /// The configuration section that sets this Persistence Unit in the configuration file
        /// </summary>
        /// <remarks>
        /// This class stored the setting information associated with this PU
        /// </remarks>
        public IPersistenceUnitCfg Configuration
        {
            get { return configuration; }
        }

        internal ISessionFactory SessionFactory
        {
            get { return sessionFactory; }
        }

        /// <summary>
        /// The nhibernate configuration of this session Manager
        /// </summary>
        public Cfg.Configuration NHConfiguration
        {
            get { return nHConfiguration; }
        }

        /// <summary>
        /// Rebuild the Session factory
        /// </summary>
        /// <remarks>
        /// in case you need to change the NHConfiguration on the fly
        /// </remarks>
        public void ReBuildSessionfactory()
        {
            sessionFactory = nHConfiguration.BuildSessionFactory();
        }

        public static PersistenceUnit Instance(System.Type t)
        {
            return PersistenceUnitRepo.Instance.GetPU(t);
        }

        ///<summary>
        /// Create a NHibernate Configuration
        ///</summary>
        ///<returns></returns>
        private Cfg.Configuration CreateNHConfiguration()
        {
            Cfg.Configuration retVal = new Cfg.Configuration();
            string configFile = configuration.NHConfigFile.Replace("~", AppDomain.CurrentDomain.BaseDirectory);
            retVal.Configure(configFile);
            //retVal.Properties = Configuration.ORMFrameworkSettingsDict;
            //foreach (Assembly assembly in domainLayerAssemblies)
            //    retVal.AddAssembly(assembly);
            return retVal;
        }

        public IInterceptor CreateInterceptor()
        {
            if (string.IsNullOrEmpty(Configuration.InterceptorFactory))
            {
                return null;
            }
            return InstanceLoader.Load<IInterceptorFactory>(Configuration.InterceptorFactory).Create(NHConfiguration);
        }
    }
}