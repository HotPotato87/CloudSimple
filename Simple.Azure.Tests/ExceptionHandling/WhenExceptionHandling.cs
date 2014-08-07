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
            AzureSimpleContainer.Configure(base.StorageConnectionString)
                .ConfigureExceptionHandlers()
                    .WithFlushThreshold(1);

            var exception = new Exception("something went wrong");

            await CloudSimpleContainer.Instance.HandleExceptionAsync(exception);

            var itemsFromStorage = base.GetAllFromStorage<LoggedExceptionEntity>("exceptions");
            Assert.IsTrue(itemsFromStorage.Count == 1);
        }

        [Test]
        public async Task FullDetailsOfExceptionIsLogged()
        {
            AzureSimpleContainer.Configure(base.StorageConnectionString)
                .ConfigureExceptionHandlers()
                    .WithFlushThreshold(1);

            Func<int> getZero = () => 0;
            Exception loggedException = null;

            try
            {
                var wontwork = 0 / getZero();
                Console.Write(wontwork);
            }
            catch (Exception eX)
            {
                CloudSimpleContainer.Instance.HandleException(eX);
                loggedException = eX;
            }

            var itemsFromStorage = base.GetAllFromStorage<LoggedExceptionEntity>(ExceptionTableName);
            var logged = itemsFromStorage.First();
            Assert.IsTrue(logged.StackTrace == loggedException.StackTrace);
            Assert.IsTrue(logged.Source == loggedException.Source);
            Assert.IsTrue(logged.Message == loggedException.Message);
            Assert.IsTrue(logged.Severity == Severity.None.ToString());
        }

        [Test]
        public async Task IfThresholdSetToTwo_ExceptionNotFlushedToStorage()
        {
            AzureSimpleContainer.Configure(base.StorageConnectionString)
                .ConfigureExceptionHandlers()
                    .WithFlushThreshold(2);

            Func<int> getZero = () => 0;
            Exception loggedException = null;

            try
            {
                var wontwork = 0 / getZero();
                Console.Write(wontwork);
            }
            catch (Exception eX)
            {
                CloudSimpleContainer.Instance.HandleException(eX);
                loggedException = eX;
            }

            var itemsFromStorage = base.GetAllFromStorage<LoggedExceptionEntity>(base.ExceptionTableName);
            Assert.IsFalse(itemsFromStorage.Any());
        }

        [Test]
        public async Task IfThresholdSetToTwo_AfterTwoBothExceptionsFlushed()
        {
            AzureSimpleContainer.Configure(base.StorageConnectionString)
                .ConfigureExceptionHandlers()
                    .WithFlushThreshold(2);

            Func<int> getZero = () => 0;

            try
            {
                var wontwork = 0 / getZero();
                Console.Write(wontwork);
            }
            catch (Exception eX)
            {
                CloudSimpleContainer.Instance.HandleException(eX);
                CloudSimpleContainer.Instance.HandleException(eX);
            }

            var itemsFromStorage = base.GetAllFromStorage<LoggedExceptionEntity>(base.ExceptionTableName);
            Assert.IsTrue(itemsFromStorage.Count() == 2);
        }

        [Test]
        public async Task IfTimerSetAndBelowThreshold_BeforeTimerNoExceptionsFlushed()
        {
            AzureSimpleContainer.Configure(base.StorageConnectionString)
                .ConfigureExceptionHandlers()
                    .WithFlushThreshold(2)
                    .WithFlushTimer(TimeSpan.FromMilliseconds(5));

            Func<int> getZero = () => 0;

            try
            {
                var wontwork = 0 / getZero();
                Console.Write(wontwork);
            }
            catch (Exception eX)
            {
                CloudSimpleContainer.Instance.HandleException(eX);
            }

            var itemsFromStorage = base.GetAllFromStorage<LoggedExceptionEntity>(base.ExceptionTableName);
            Assert.IsFalse(itemsFromStorage.Any());
            await Task.Delay(TimeSpan.FromMilliseconds(2));
            itemsFromStorage = base.GetAllFromStorage<LoggedExceptionEntity>(base.ExceptionTableName);
            Assert.IsFalse(itemsFromStorage.Any());
        }

        [Test]
        public async Task IfTimerSetAndBelowThreshold_AfterTimerAllExceptionsFlushed()
        {
            AzureSimpleContainer.Configure(base.StorageConnectionString)
                .ConfigureExceptionHandlers()
                    .WithFlushThreshold(2)
                    .WithFlushTimer(TimeSpan.FromMilliseconds(150));

            Func<int> getZero = () => 0;

            try
            {
                var wontwork = 0 / getZero();
                Console.Write(wontwork);
            }
            catch (Exception eX)
            {
                CloudSimpleContainer.Instance.HandleException(eX);
            }

            var itemsFromStorage = base.GetAllFromStorage<LoggedExceptionEntity>(base.ExceptionTableName);
            Assert.IsFalse(itemsFromStorage.Any());
            await Task.Delay(TimeSpan.FromMilliseconds(200));
            itemsFromStorage = base.GetAllFromStorage<LoggedExceptionEntity>(base.ExceptionTableName);
            Assert.IsFalse(itemsFromStorage.Any());
        }
    }
}
