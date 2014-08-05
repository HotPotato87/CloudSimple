using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudSimple.Core
{
    public interface ILogHandler
    {
        void LogMessageAsync(string message);
    }
}
