using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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
            //get the partition fields
            throw new NotImplementedException();

            //update state
            _currentState = State.Ready;

            //flush the buffer
            _initBuffer.ForEach(x=>x());
        }

        public void LogMessageAsync(string message, string key = null, dynamic extra = null)
        {
            //add to queue (either buffer or now)
            Action addToQueueAction = () => this.HandleNewLogMessage(new LogMessageEntity(message, base.PartitionSelector != null && extra != null ? base.PartitionSelector(extra) : key, extra));
            
            switch (_currentState)
            {
                case State.Ready:
                    addToQueueAction();
                    break;
                default:
                    _initBuffer.Add(addToQueueAction);
                    break;
            }

            //handle any 
        }

        private void HandleNewLogMessage(LogMessageEntity logMessageEntity)
        {
            //handle new partitions 
            RIP this out should be it's own azure container

            //handle 
            this.AddToQueue(logMessageEntity);
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
