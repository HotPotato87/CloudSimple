using System;
using CloudSimple.Core;

namespace CloudSimple.Azure
{
    public class AzureStorageExceptionHandler : AzureStorageBase, IExceptionHandler
    {
        public StorageContainerConfiguration QueueConfiguration { get; private set; }
        public AzureStorageConfiguration StorageConfiguration { get; private set; }

        private readonly IAlertManager _alertManager;

        public AzureStorageExceptionHandler(
            AzureStorageConfiguration config)
        {
            this.StorageConfiguration = config;
            this.QueueConfiguration = new StorageContainerConfiguration();
        }

        public void HandleExceptionAsync(Exception e, bool alert = false, Severity severity = Severity.None, dynamic extra = null)
        {
            if (_alertManager != null && alert)
            {
                ContactAlertManager(e, severity, extra);
            }
        }

        private void ContactAlertManager(Exception exception, Severity severity, object extra)
        {

        }
    }

    public class LoggedException
    {
        
    }
}
