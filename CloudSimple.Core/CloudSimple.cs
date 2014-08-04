using CloudSimple.Core.ExceptionHandling;

namespace CloudSimple.Core
{
    public class CloudSimple
    {
        private CloudSimple _instance;
        private IExceptionHandler _exceptionHandler;

        public CloudSimple Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        public IExceptionHandler ExceptionHandler
        {
            get { return _exceptionHandler; }
        }
    }
}
