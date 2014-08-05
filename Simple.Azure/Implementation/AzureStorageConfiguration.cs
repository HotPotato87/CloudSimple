using System;

namespace CloudSimple.Azure
{
    public class AzureStorageConfiguration
    {
        public AzureStorageConfiguration(string storageAccount, string storageKey)
        {
            this.Account = storageAccount;
            this.Key = storageKey;
        }

        public string Account { get; set; }
        public string Key { get; set; }
        public string ConnectionString
        {
            get { return String.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", Account, Key); }
        }
    }
}