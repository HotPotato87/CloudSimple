using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudSimple.Core.Builders;

namespace CloudSimple.Core
{
    public static class Extentions
    {
        public static Task HandleExceptionAsync(this CloudSimpleContainer source, Exception e, bool alert = false, Severity severity = Severity.None, dynamic extra = null)
        {
            return Task.WhenAll(source.ExceptionHandlers.Select(exceptionHandler => exceptionHandler.HandleExceptionAsync(e, alert, severity, extra)).Cast<Task>());
        }

        public static void HandleException(this CloudSimpleContainer source, Exception e, bool alert = false, Severity severity = Severity.None, dynamic extra = null)
        {
            source.ExceptionHandlers.ForEach(exceptionHandler => exceptionHandler.HandleException(e, alert, severity, extra));
        }

        public static void LogMessage(this CloudSimpleContainer source, string messageText, string key = null, dynamic extra = null)
        {
            source.LogHandlers.ForEach(logHandler => logHandler.LogMessageAsync(messageText, key, extra));
        }

        public static StorageContainerHandlerBuilder ConfigureExceptionHandlers(this CloudSimpleContainer source)
        {
            return new StorageContainerHandlerBuilder(source, x=>x.ExceptionHandlers);
        }

        public static StorageContainerHandlerBuilder ConfigureLogHandlers(this CloudSimpleContainer source)
        {
            return new StorageContainerHandlerBuilder(source, x => x.LogHandlers);
        }

        public static StorageContainerHandlerBuilder WithFlushThreshold(this StorageContainerHandlerBuilder source, int thresholdValue)
        {
            source.Handlers.ForEach(x => x.Configuration.FlushThreshold = thresholdValue);

            return source;
        }

        public static StorageContainerHandlerBuilder WithFlushTimer(this StorageContainerHandlerBuilder source, TimeSpan interval)
        {
            source.Handlers.ForEach(x =>
            {
                x.Configuration.FlushTimer = interval;
                x.Configuration.UseFlushTimer = true;
            });

            return source;
        }

        public static void ForEach<T>(this List<T> source, Action<T> a)
        {
            foreach (var item in source)
            {
                a(item);
            }
        }
    }
}
