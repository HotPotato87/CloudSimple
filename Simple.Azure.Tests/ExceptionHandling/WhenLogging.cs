using System.Threading.Tasks;
using CloudSimple.Azure.Tests.General;
using CloudSimple.Core;
using NUnit.Framework;

namespace CloudSimple.Azure.Tests.ExceptionHandling
{
    [TestFixture]
    public class WhenLogging : AzureStorageTestBase
    {
        [Test]
        public async Task WhenLogMessage_SimpleMessageIsStored()
        {
            AzureSimpleContainer.Configure(base.StorageConnectionString)
                .ConfigureLogHandlers()
                    .WithFlushThreshold(1);

            AzureSimpleContainer.Instance.LogMessage("This is a log message");

            var items = base.GetAllFromStorage<LogMessageEntity>(LogTableName);
            Assert.IsTrue(items.Count == 1);
        }

        [Test]
        public async Task WhenLogMessage_CategorizedMessageIsStored()
        {
            AzureSimpleContainer.Configure(base.StorageConnectionString)
                .ConfigureLogHandlers()
                    .WithFlushThreshold(1);

            string message = "This is a log message";
            string category = "Categorized Messages";
            AzureSimpleContainer.Instance.LogMessage(message, key:category);

            var items = base.GetAllFromStorage<LogMessageEntity>(LogTableName);
            var item = items[0];

            Assert.IsTrue(item.Category == category);
            Assert.IsTrue(item.Message == message);
        }
    }
}