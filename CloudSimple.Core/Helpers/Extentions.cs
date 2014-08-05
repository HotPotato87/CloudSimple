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

        public static ExceptionHandlerBuilder ConfigureExceptionHandlers(this CloudSimpleContainer source)
        {
            return new ExceptionHandlerBuilder(source);
        }

        public static ExceptionHandlerBuilder WithFlushThreshold(this ExceptionHandlerBuilder source, int thresholdValue)
        {
            source.Container.ExceptionHandlers.ForEach(x => x.Configuration.FlushThreshold = thresholdValue);

            return source;
        }

        public static ExceptionHandlerBuilder WithFlushTimer(this ExceptionHandlerBuilder source, TimeSpan interval)
        {
            source.Container.ExceptionHandlers.ForEach(x => x.Configuration.FlushTimer = interval);

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
