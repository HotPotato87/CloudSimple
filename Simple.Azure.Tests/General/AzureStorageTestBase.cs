using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CloudSimple.Azure.Tests.General
{
    [TestFixture]
    public class AzureStorageTestBase
    {
        protected string StorageAccount { get; set; }
        protected string StorageKey { get; set; }

        [TestFixtureSetUp]
        public virtual void OnSetup()
        {
            this.StorageKey = "";
            this.StorageAccount = "";
        }
    }
}
