using System;
using System.Runtime.InteropServices;
using CloudSimple.Azure;
using CloudSimple.Core;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace CloudSimple.Azure
{
    public class AzureLogHandler : AzureStorageBase, ILogHandler
    {
        private static string _tableName = "logs";

        public AzureLogHandler(AzureStorageConfiguration config)
            : base(_tableName, config)
        {
        }

        public void LogMessageAsync(string message, string key = null, dynamic extra = null)
        {
            base.AddToQueue(new LogMessageEntity(message, key, extra));
        }
    }

    public class LogMessageEntity : TableEntity
    {
        public string Message { get; set; }

        public object Extra { get; set; }

        public string Category
        {
            get { return this.PartitionKey; }
        }

        public LogMessageEntity() { }

        public LogMessageEntity(string message, string key, dynamic extra)
        {
            this.Message = message;
            PartitionKey = key ?? "";
            RowKey = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);
            Extra = JsonConvert.SerializeObject(extra);
        }
    }
}
