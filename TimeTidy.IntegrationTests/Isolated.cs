using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Interfaces;
using System.Transactions;

namespace TimeTidy.IntegrationTests
{
    public class Isolated : Attribute, ITestAction
    {
        private TransactionScope _transactionScope;

        public ActionTargets Targets
        {
            get
            {
                return ActionTargets.Test;
            }
        }

        public void BeforeTest(ITest test)
        {
            // Async flow option needs to be enabled so awaits won't hang.
            _transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        public void AfterTest(ITest test)
        {
            _transactionScope.Dispose();
        }

    }
}
