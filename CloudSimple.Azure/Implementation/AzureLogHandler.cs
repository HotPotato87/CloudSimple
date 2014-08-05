using System;
using CloudSimple.Core;

namespace CloudSimple.Azure
{
        public class AzureLogHandler : ILogHandler
        {
            private AzureStorageConfiguration _config;

            public AzureLogHandler(AzureStorageConfiguration config)
            {
                _config = config;
            }

            public void LogMessageAsync(string message)
            {
                throw new NotImplementedException();
            }
        }
}
