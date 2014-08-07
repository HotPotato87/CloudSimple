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
        }

        [Test]
        public async Task WhenLogMessage_CategorizedMessageIsStored()
        {
            AzureSimpleContainer.Configure(base.StorageConnectionString)
                .ConfigureLogHandlers()
                    .WithFlushThreshold(1);

            AzureSimpleContainer.Instance.LogMessage("Categorized Messages", "This is a log message");
        }
    }
}