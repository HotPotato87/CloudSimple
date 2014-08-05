using System;
using System.Runtime.InteropServices;
using CloudSimple.Azure;
using CloudSimple.Core;

namespace CloudSimple.Azure
{
        public class AzureLogHandler : AzureStorageBase, ILogHandler
        {
            private static string _tableName = "logs";

            public AzureLogHandler(AzureStorageConfiguration config) : base(_tableName, config)
            {
            }

            public void LogMessageAsync(string message)
            {
                throw new NotImplementedException();
            }
        }
}
