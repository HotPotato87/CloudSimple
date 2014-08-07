using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudSimple.Core
{
    public interface ILogHandler : IStorageContainer
    {
        void LogMessageAsync(string message, string category = null, dynamic extra = null)
    }
}
