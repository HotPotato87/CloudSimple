using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudSimple.Core.Azure;

namespace CloudSimple.Core
{
    public class AzurePack
    {
        private static AzureStorageConfiguration _azureStorageConfiguration;

        public static void Configure(string storageKey, string storageAccount)
        {
            _azureStorageConfiguration = new AzureStorageConfiguration()
            {
                Account = storageAccount,
                Key = storageKey
            };
        }


    }

    public class CloudSimple
    {
        private CloudSimple _instance;
        public CloudSimple Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }
    }
}
