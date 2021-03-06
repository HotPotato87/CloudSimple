﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;

namespace Simple.Azure.Helpers
{
    public class TableStorageWriter
    {
        private const int BatchSize = 100;
        private readonly ConcurrentQueue<Tuple<ITableEntity, TableOperation>> operations;
        private readonly CloudStorageAccount storageAccount;
        private readonly string tableName;

        public TableStorageWriter(string tableName, string connectionString)
        {
            this.tableName = tableName;

            var cs = connectionString;

            storageAccount = CloudStorageAccount.Parse(cs);

            var tableReference = MakeTableReference();

            tableReference.CreateIfNotExists();

            operations = new ConcurrentQueue<Tuple<ITableEntity, TableOperation>>();
        }

        private CloudTable MakeTableReference()
        {
            var tableClient = storageAccount.CreateCloudTableClient();
            var tableReference = tableClient.GetTableReference(tableName);
            return tableReference;
        }

        public decimal OutstandingOperations
        {
            get { return operations.Count; }
        }

        public void Insert<TEntity>(TEntity entity)
            where TEntity : ITableEntity
        {
            var e = new Tuple<ITableEntity, TableOperation>
                (entity,
                    TableOperation.Insert(entity));
            operations.Enqueue(e);
        }

        public void Delete<TEntity>(TEntity entity)
            where TEntity : ITableEntity
        {
            var e = new Tuple<ITableEntity, TableOperation>
                (entity,
                    TableOperation.Delete(entity));
            operations.Enqueue(e);
        }

        public void InsertOrMerge<TEntity>(TEntity entity)
            where TEntity : ITableEntity
        {
            var e = new Tuple<ITableEntity, TableOperation>
                (entity,
                    TableOperation.InsertOrMerge(entity));
            operations.Enqueue(e);
        }

        public void InsertOrReplace<TEntity>(TEntity entity)
            where TEntity : ITableEntity
        {
            var e = new Tuple<ITableEntity, TableOperation>
                (entity,
                    TableOperation.InsertOrReplace(entity));
            operations.Enqueue(e);
        }

        public void Merge<TEntity>(TEntity entity)
            where TEntity : ITableEntity
        {
            var e = new Tuple<ITableEntity, TableOperation>
                (entity,
                    TableOperation.Merge(entity));
            operations.Enqueue(e);
        }

        public void Replace<TEntity>(TEntity entity)
            where TEntity : ITableEntity
        {
            var e = new Tuple<ITableEntity, TableOperation>
                (entity,
                    TableOperation.Replace(entity));
            operations.Enqueue(e);
        }

        public void Execute()
        {
            var count = operations.Count;
            var toExecute = new List<Tuple<ITableEntity, TableOperation>>();
            for (var index = 0; index < count; index++)
            {
                Tuple<ITableEntity, TableOperation> operation;
                operations.TryDequeue(out operation);
                if (operation != null)
                    toExecute.Add(operation);
            }

            toExecute
                .GroupBy(tuple => tuple.Item1.PartitionKey)
                .ToList()
                .ForEach(g =>
                {
                    var opreations = g.ToList();

                    var batch = 0;
                    var operationBatch = GetOperations(opreations, batch);

                    while (operationBatch.Any())
                    {
                        var tableBatchOperation = MakeBatchOperation(operationBatch);

                        ExecuteBatchWithRetries(tableBatchOperation);

                        batch++;
                        operationBatch = GetOperations(opreations, batch);
                    }
                });
        }

        private void ExecuteBatchWithRetries(TableBatchOperation tableBatchOperation)
        {
            var tableRequestOptions = MakeTableRequestOptions();

            var tableReference = MakeTableReference();

            tableReference.ExecuteBatch(tableBatchOperation, tableRequestOptions);
        }

        private static TableRequestOptions MakeTableRequestOptions()
        {
            return new TableRequestOptions
            {
                RetryPolicy = new ExponentialRetry(TimeSpan.FromMilliseconds(2),
                    100)
            };
        }

        private static TableBatchOperation MakeBatchOperation(
            List<Tuple<ITableEntity, TableOperation>> operationsToExecute)
        {
            var tableBatchOperation = new TableBatchOperation();
            operationsToExecute.ForEach(tuple => tableBatchOperation.Add(tuple.Item2));
            return tableBatchOperation;
        }

        private static List<Tuple<ITableEntity, TableOperation>> GetOperations(
            IEnumerable<Tuple<ITableEntity, TableOperation>> opreations,
            int batch)
        {
            return opreations
                .Skip(batch * BatchSize)
                .Take(BatchSize)
                .ToList();
        }
    }
}
