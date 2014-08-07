using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudSimple.Core.Builders
{
    public class StorageContainerHandlerBuilder : BuilderBase
    {
        public StorageContainerHandlerBuilder(CloudSimpleContainer source, Func<CloudSimpleContainer,IEnumerable<IStorageContainer>> handlerSelector) : base(source, handlerSelector)
        {
        }
    }
}
