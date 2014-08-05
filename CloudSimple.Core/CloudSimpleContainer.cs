using System.Collections.Generic;

namespace CloudSimple.Core
{
    public class CloudSimpleContainer
    {
        private static CloudSimpleContainer _instance;
        public static CloudSimpleContainer Instance
        {
            get { return _instance ?? new CloudSimpleContainer(); }
            protected set { _instance = value; }
        }

        protected internal List<IExceptionHandler> ExceptionHandlers { get; set; }
        protected internal List<ILogHandler> LogHandlers { get; set; }
        protected internal List<IAlertManager> AlertHandlers { get; set; }

        public CloudSimpleContainer()
        {
            ExceptionHandlers = new List<IExceptionHandler>();
            LogHandlers = new List<ILogHandler>();
            AlertHandlers = new List<IAlertManager>();
        }
    }
}
