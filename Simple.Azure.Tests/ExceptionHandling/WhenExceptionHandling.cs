using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudSimple.Azure.Tests.General;
using CloudSimple.Core;
using NUnit.Framework;

namespace CloudSimple.Azure.Tests.ExceptionHandling
{
    [TestFixture]
    public class WhenExceptionHandling : AzureStorageTestBase
    {
        [Test]
        public async Task ExceptionsLoggedToStorage()
        {
            AzureSimpleContainer.Configure(base.StorageAccount, base.StorageKey)
                .ConfigureExceptionHandlers()
                    .WithFlushThreshold(1);

            var exception = new Exception("something went wrong");

            await CloudSimpleContainer.Instance.HandleExceptionAsync(exception);
        }
    }
}
