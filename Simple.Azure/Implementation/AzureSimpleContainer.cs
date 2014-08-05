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
        public static AzureSimpleContainer Configure(string storageKey, string storageAccount)
        {
            var config = new AzureStorageConfiguration(storageAccount, storageKey);
            var instance = new AzureSimpleContainer();
            instance.ExceptionHandlers.Add(new AzureStorageExceptionHandler(config));
            instance.LogHandlers.Add(new AzureLogHandler(config));

            CloudSimpleContainer.Instance = instance;

            return instance;
        }

        public AzureSimpleContainer()
        {
            ExceptionHandlers = new List<IExceptionHandler>();
            LogHandlers = new List<ILogHandler>();
            this.AlertHandlers = new List<IAlertManager>();
        }
    }
}
