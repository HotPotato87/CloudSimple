using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudSimple.Core.General;

namespace CloudSimple.Core.ExceptionHandling
{
    public interface IExceptionHandler
    {
        void HandleExceptionAsync(Exception e, bool alert = false, Severity severity = Severity.None, dynamic extra = null);
    }
}
