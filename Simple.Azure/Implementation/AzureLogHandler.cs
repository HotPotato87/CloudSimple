using System;
using System.Collections.Generic;
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
        private static State _currentState = State.Initializing;
        private static List<Action> _initBuffer = new List<Action>();

        public AzureLogHandler(AzureStorageConfiguration config)
            : base(_tableName, config)
        {
            this.RetreivePartitionValues();
        }

        private void RetreivePartitionValues()
        {
            throw new NotImplementedException();

            _currentState = State.Ready;
        }

        public void LogMessageAsync(string message, string key = null, dynamic extra = null)
        {
            if (_currentState == State.Ready)
            {
                base.AddToQueue(new LogMessageEntity(message, base.PartitionSelector != null && extra != null ? base.PartitionSelector(extra) : key, extra));
            }
            else
            {
                
            }
            
        }

        private enum State
        {
            Initializing,
            Ready
        };
    }

    public class PartitionValueEntity : TableEntity
    {
        public PartitionValueEntity() {}
        public PartitionValueEntity(string partitionName)
        {
            this.PartitionKey = partitionName;
            RowKey = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);
        }
    }

    public class LogMessageEntity : TableEntity
    {
        public string Message { get; set; }

        public string Meta { get; set; }

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
            Meta = JsonConvert.SerializeObject(extra);
        }

        public T GetMeta<T>()
        {
            return JsonConvert.DeserializeObject<T>(Meta);
        }
    }
}
