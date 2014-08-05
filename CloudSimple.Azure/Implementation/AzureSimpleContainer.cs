using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudSimple.Core;

namespace CloudSimple.Azure
{
    public class AzureSimpleContainer : CloudSimpleContainer
    {
        protected new List<IExceptionHandler> ExceptionHandlers { get; set; }
        protected new List<ILogHandler> LogHandlers { get; set; }
        protected new List<IAlertManager> AlertHandlers { get; set; }

        public static AzureSimpleContainer Configure(string storageKey, string storageAccount)
        {
            var config = new AzureStorageConfiguration(storageAccount, storageKey);
            var instance = new AzureSimpleContainer();
            instance.ExceptionHandlers.Add(new AzureStorageExceptionHandler(config));
            instance.LogHandlers.Add(new AzureLogHandler(config));

            CloudSimpleContainer.Instance = instance;

            return instance;
        }
    }
}
