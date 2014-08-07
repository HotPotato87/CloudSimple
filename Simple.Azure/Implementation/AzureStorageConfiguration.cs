using System;

namespace CloudSimple.Azure
{
    public class AzureStorageConfiguration
    {
        public AzureStorageConfiguration(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        public string ConnectionString { get; set; }
    }
}