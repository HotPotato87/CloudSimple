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
    }
}