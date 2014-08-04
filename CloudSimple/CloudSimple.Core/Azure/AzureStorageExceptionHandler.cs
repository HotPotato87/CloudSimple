using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudSimple.Core.AlertManager;
using CloudSimple.Core.ExceptionHandling;
using CloudSimple.Core.General;

namespace CloudSimple.Core.Azure
{
    public class AzureStorageExceptionHandler : IExceptionHandler
    {
        private readonly AzureStorageConfiguration _config;
        private readonly IAlertManager _alertManager;

        public AzureStorageExceptionHandler(
            AzureStorageConfiguration config,
            IAlertManager alertManager)
        {
            _config = config;
            _alertManager = alertManager;
        }

        public Task HandleExceptionAsync(Exception e, bool alert = false, Severity severity = Severity.None, dynamic extra = null)
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
}
