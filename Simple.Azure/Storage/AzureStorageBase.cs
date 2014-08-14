using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CloudSimple.Core;
using Microsoft.WindowsAzure.Storage.Table;
using Simple.Azure.Helpers;

namespace CloudSimple.Azure
{
    public class AzureStorageBase
    {
        public StorageContainerConfiguration Configuration { get; set; }
        public Func<object, string> PartitionSelector { get; set; }

        private readonly string _tableName;
        private readonly AzureStorageConfiguration _config;
        private readonly ConcurrentQueue<TableEntity> _queue = new ConcurrentQueue<TableEntity>();

        private bool _isInitialised = false;
        private Timer _flushTimer;

        protected AzureStorageBase(string tableName, AzureStorageConfiguration config)
        {
            Configuration = new StorageContainerConfiguration();
            _tableName = tableName;
            _config = config;
        }

        protected void AddToQueue(TableEntity tableEntity)
        {
            _queue.Enqueue(tableEntity);

            if (!_isInitialised)
            {
                Initialise();
            }

            if (_queue.Count >= Configuration.FlushThreshold)
            {
                FlushQueue();
            }
        }

        private void Initialise()
        {
            if (Configuration.UseFlushTimer)
            {
                _flushTimer = new Timer();
                _flushTimer.Interval = Configuration.FlushTimer.TotalMilliseconds;
                _flushTimer.Elapsed += (sender, args) => FlushQueue();
                _flushTimer.Start();
            }

            _isInitialised = true;
        }

        private void FlushQueue()
        {
            if (!_queue.Any()) return;

            lock (this)
            {
                var toSend = new List<TableEntity>();
                while (!_queue.IsEmpty)
                {
                    TableEntity outEntity;
                    if (_queue.TryDequeue(out outEntity))
                    {
                        toSend.Add(outEntity);    
                    }
                }

                var writer = new TableStorageWriter(_tableName, _config.ConnectionString);
                toSend.ForEach(writer.Insert);
                writer.Execute();
            }
        }
    }
}
