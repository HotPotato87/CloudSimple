using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudSimple.Azure;
using Microsoft.WindowsAzure.Storage.Table;

namespace Cloud.Simple.Azure.Implementation
{
    public class AzureLogPartitionHandler : AzureStorageBase
    {
        private static State _currentState = State.Initializing;
        private static List<string> _partitionBuffer = new List<string>();
        private static List<string> _existingPartitions = new List<string>();

        public AzureLogPartitionHandler(AzureStorageConfiguration config) : base("logPartitions", config)
        {
            RetreivePartitionValues();
        }

        private void RetreivePartitionValues()
        {
            //get the partition fields
            _existingPartitions = base.RetrieveValues<PartitionValueEntity>().Select(x => x.RowKey).ToList();

            //update state
            _currentState = State.Ready;

            //flush the buffer
            _existingPartitions.ForEach(HandlePartition);
        }

        public void HandlePartition(string partition)
        {
            if (_currentState == State.Initializing)
            {
                _partitionBuffer.Add(partition);
            }
            else
            {
                if (!_existingPartitions.Contains(partition))
                {
                    this.AddToQueue(new PartitionValueEntity(partition));
                    _existingPartitions.Add(partition);
                }
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
        public PartitionValueEntity() { }
        public PartitionValueEntity(string partitionName)
        {
            this.PartitionKey = partitionName;
            RowKey = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);
        }
    }
}
