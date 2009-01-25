using NHibernate.Cfg;
using NUnit.Framework;

namespace NHibernate.Search.Tests.Optimizer
{
    [TestFixture]
    public class IncrementalOptimizerStrategyTest : OptimizerTestCase
    {
        protected override string BaseIndexDirName
        {
            get { return base.BaseIndexDirName + "inc"; } // Use another dir because running both tests on the same dir fails
        }

        protected override void Configure(Configuration configuration)
        {
            base.Configure(configuration);
            configuration.SetProperty("hibernate.search.default.optimizer.transaction_limit.max", "10");
        }
    }
}