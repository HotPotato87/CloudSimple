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

        [Test]
        public async Task WhenLogMessage_CanChooseADifferentPartitionField()
        {
            AzureSimpleContainer.Configure(base.StorageConnectionString)
                .ConfigureLogHandlers()
                    .PartitionBy<User>(x=>x.Email)
                    .WithFlushThreshold(1);

            string emailAddress = "myemail@email.com";
            string message = "This is a log message";
            string category = "Categorized Messages";
            AzureSimpleContainer.Instance.LogMessage(message, meta:new User() { Email = emailAddress});

            var items = base.GetAllFromStorage<LogMessageEntity>(LogTableName);
            var item = items[0];

            Assert.IsTrue(item.PartitionKey == emailAddress);
        }

        [Test]
        public async Task WhenLogMessage_MetaCanBeReconstructed()
        {
            AzureSimpleContainer.Configure(base.StorageConnectionString)
                .ConfigureLogHandlers()
                    .PartitionBy<User>(x => x.Email)
                    .WithFlushThreshold(1);

            string emailAddress = "myemail@email.com";
            string message = "This is a log message";
            string category = "Categorized Messages";
            AzureSimpleContainer.Instance.LogMessage(message, meta: new User() { Email = emailAddress, Firstname = "first", Lastname = "last"});

            var items = base.GetAllFromStorage<LogMessageEntity>(LogTableName);
            var item = items[0];

            Assert.IsTrue(item.GetMeta<User>().Firstname == "first");
            Assert.IsTrue(item.GetMeta<User>().Lastname == "last");
        }

        public class User
        {
            public string Email { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
        }
    }
}