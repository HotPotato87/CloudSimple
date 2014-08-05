using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudSimple.Azure.Implementation;
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
