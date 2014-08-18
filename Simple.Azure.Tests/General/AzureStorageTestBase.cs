using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;

namespace CloudSimple.Azure.Tests.General
{
    public class AzureStorageTestBase
    {
        protected string StorageAccount { get; set; }
        protected string StorageKey { get; set; }
        protected string StorageEndpoint { get; set; }
        public string StorageConnectionString { get; set; }
        public string ExceptionTableName
        {
            get { return "exceptions"; }
        }
        public string LogTableName
        {
            get { return "logs"; }
        }

        public string LogPartitionTableName
        {
            get { return "logpartitions"; }
        }

        [TestFixtureSetUp]
        public virtual void OnFixtureSetup()
        {
            this.StorageKey = "devstoreaccount1";
            this.StorageAccount = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==+d9Q25nKo1LJ+ncOTFcoRPlJ/wlIV/q2IxiABTvEX2lRRAg/YucW6QOvjV+peY5Jsaw==";
            this.StorageEndpoint = "http://127.0.0.1:10000";
            this.StorageConnectionString = "UseDevelopmentStorage=true";

            ClearStorage(ExceptionTableName);
            ClearStorage(LogTableName);
        }

        [SetUp]
        public virtual void OnSetup()
        {
            ClearStorage(ExceptionTableName);
            ClearStorage(LogTableName);
        }

        protected void ClearStorage(string name)
        {
            var cloudBlob = CloudStorageAccount.Parse(this.StorageConnectionString);
            var tableClient = cloudBlob.CreateCloudTableClient();
            var tableReference = tableClient.GetTableReference(name);
            if (tableReference.Exists())
            {
                tableReference.Delete();    
            }

        }

        protected List<T> GetAllFromStorage<T>(string tableName) where T : ITableEntity,new()
        {
            var cloudBlob =  CloudStorageAccount.Parse(this.StorageConnectionString);
            var tableClient = cloudBlob.CreateCloudTableClient();
            var tableReference = tableClient.GetTableReference(tableName);
            if (!tableReference.Exists()) { return new List<T>(); }
            return tableReference.CreateQuery<T>().ToList();
        }
    }
}
