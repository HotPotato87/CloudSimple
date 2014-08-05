using System;
using System.Collections.Generic;
using System.Linq;
using CloudSimple.Azure.Tests.General;
using CloudSimple.Core;
using NUnit.Framework;

namespace CloudSimple.Azure.Tests.ExceptionHandling
{
    [TestFixture]
    public class WhenExceptionHandling : AzureStorageTestBase
    {
        [Test]
        public void ExceptionsLoggedToStorage()
        {
            AzureSimpleContainer.Configure(base.StorageAccount, base.StorageKey)
                .ConfigureExceptionHandlers()
                    .WithFlushThreshold(1);

            var exception = new Exception("something went wrong");

            CloudSimpleContainer.Instance.HandleExceptionAsync(exception);
        }
    }
}
