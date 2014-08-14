using System;

namespace CloudSimple.Core
{
    public interface IStorageContainer
    {
        StorageContainerConfiguration Configuration { get; set; }
        Func<object, string> PartitionSelector { get; set; }
    }
}