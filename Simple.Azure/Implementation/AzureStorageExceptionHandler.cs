using System;
using System.Threading.Tasks;
using CloudSimple.Core;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace CloudSimple.Azure
{
    public class AzureStorageExceptionHandler : AzureStorageBase, IExceptionHandler
    {
        public AzureStorageConfiguration StorageConfiguration { get; private set; }
        private static string _exceptionTableName = "exceptions";

        private readonly IAlertManager _alertManager;

        public AzureStorageExceptionHandler(AzureStorageConfiguration config) : base(_exceptionTableName, config)
        {
            this.StorageConfiguration = config;
        }

        public async Task HandleExceptionAsync(Exception e, bool alert = false, Severity severity = Severity.None, dynamic extra = null)
        {
            await Task.Run(() =>
            {
                HandleException(e, alert, severity, extra);
            });
        }

        public void HandleException(Exception e, bool alert = false, Severity severity = Severity.None, dynamic extra = null)
        {
            if (_alertManager != null && alert)
            {
                ContactAlertManager(e, severity, extra);
            }

            base.AddToQueue(new LoggedExceptionEntity(e, severity, extra));
        }

        private void ContactAlertManager(Exception exception, Severity severity, object extra)
        {

        }

        public Func<object, object> PartitionSelector { get; set; }
    }

    public class LoggedExceptionEntity : TableEntity
    {
        public string Message { get; set; }

        public string StackTrace { get; set; }

        public string InnerExceptionMessage { get; set; }

        public string InnerExceptionStackTrace { get; set; }

        public string Severity { get; set; }

        public string Extra { get; set; }

        public string Source { get; set; }

        public LoggedExceptionEntity() {  }
        public LoggedExceptionEntity(Exception exception, Severity severity, object extra)
        {
            PartitionKey = exception.Message;
            RowKey = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);
            this.Message = exception.Message;
            this.StackTrace = exception.StackTrace;
            this.Source = exception.Source;
            this.InnerExceptionMessage = exception.InnerException != null ? exception.InnerException.Message : null;
            this.InnerExceptionStackTrace = exception.InnerException != null ? exception.InnerException.StackTrace : null;
            this.Severity = severity.ToString();
            this.Extra = JsonConvert.SerializeObject(extra);
        }
    }
}
