using System.Linq;
using System.Threading.Tasks;
using Cloud.Simple.Azure.Implementation;
using CloudSimple.Azure.Tests.General;
using CloudSimple.Core;
using Microsoft.WindowsAzure.Storage.File;
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
            AzureSimpleContainer.Instance.LogMessage(message, category:category);

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

        [Test]
        public async Task WhenNewPartitionField_FirstPartitionFieldIsStored()
        {
            AzureSimpleContainer.Configure(base.StorageConnectionString)
               .ConfigureLogHandlers()
                   .PartitionBy<User>(x => x.Email)
                   .WithFlushThreshold(1);

            string emailAddress = "myemail@email.com";
            string message = "This is a log message";
            string category = "Categorized Messages";
            AzureSimpleContainer.Instance.LogMessage(message, meta: new User() { Email = emailAddress, Firstname = "first", Lastname = "last" });
            AzureSimpleContainer.Instance.LogMessage(message, meta: new User() { Email = emailAddress, Firstname = "first", Lastname = "last" });

            var items = base.GetAllFromStorage<PartitionValueEntity>(LogPartitionTableName);

            Assert.IsTrue(items.Count(x => x.PartitionKey == emailAddress) == 1);
        }

        [Test]
        public async Task WhenNewPartitionField_FirstPartitionIsOnlyStoredOnce()
        {
            AzureSimpleContainer.Configure(base.StorageConnectionString)
               .ConfigureLogHandlers()
                   .PartitionBy<User>(x => x.Email)
                   .WithFlushThreshold(1);

            string emailAddress = "myemail@email.com";
            string message = "This is a log message";
            string category = "Categorized Messages";
            AzureSimpleContainer.Instance.LogMessage(message, meta: new User() { Email = emailAddress, Firstname = "first", Lastname = "last" });

            var items = base.GetAllFromStorage<PartitionValueEntity>(LogPartitionTableName);

            Assert.IsTrue(items.Count(x => x.PartitionKey == emailAddress)==1);
        }

        [Test]
        public async Task WhenNewPartitionField_SecondPartitionIsStored()
        {
            AzureSimpleContainer.Configure(base.StorageConnectionString)
               .ConfigureLogHandlers()
                   .PartitionBy<User>(x => x.Email)
                   .WithFlushThreshold(1);

            string emailAddress = "myemail@email.com";
            string message = "This is a log message";
            var secondEmail = "secondEmail";
            string category = "Categorized Messages";
            AzureSimpleContainer.Instance.LogMessage(message, meta: new User() { Email = emailAddress, Firstname = "first", Lastname = "last" });
            AzureSimpleContainer.Instance.LogMessage(message, meta: new User() { Email = secondEmail, Firstname = "first", Lastname = "last" });

            var items = base.GetAllFromStorage<PartitionValueEntity>(LogPartitionTableName);

            Assert.IsTrue(items.Count() == 2);
            Assert.IsTrue(items.Any(x => x.PartitionKey == secondEmail));
            Assert.IsTrue(items.Any(x => x.PartitionKey == emailAddress));
        }

        [Test]
        public async Task WhenNewPartitionField_SecondPartitionIsStoredOnce()
        {
            AzureSimpleContainer.Configure(base.StorageConnectionString)
               .ConfigureLogHandlers()
                   .PartitionBy<User>(x => x.Email)
                   .WithFlushThreshold(1);

            string emailAddress = "myemail@email.com";
            string message = "This is a log message";
            var secondEmail = "secondEmail";
            string category = "Categorized Messages";
            AzureSimpleContainer.Instance.LogMessage(message, meta: new User() { Email = emailAddress, Firstname = "first", Lastname = "last" });
            AzureSimpleContainer.Instance.LogMessage(message, meta: new User() { Email = secondEmail, Firstname = "first", Lastname = "last" });
            AzureSimpleContainer.Instance.LogMessage(message, meta: new User() { Email = secondEmail, Firstname = "first", Lastname = "last" });
            AzureSimpleContainer.Instance.LogMessage(message, meta: new User() { Email = secondEmail, Firstname = "first", Lastname = "last" });
            AzureSimpleContainer.Instance.LogMessage(message, meta: new User() { Email = secondEmail, Firstname = "first", Lastname = "last" });

            var items = base.GetAllFromStorage<PartitionValueEntity>(LogPartitionTableName);

            Assert.IsTrue(items.Count() == 2);
            Assert.IsTrue(items.Any(x => x.PartitionKey == secondEmail));
            Assert.IsTrue(items.Any(x => x.PartitionKey == emailAddress));
        }

        public class User
        {
            public string Email { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
        }
    }
}