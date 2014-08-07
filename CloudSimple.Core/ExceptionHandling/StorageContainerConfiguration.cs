using System;

namespace CloudSimple.Core
{
    public class StorageContainerConfiguration
    {
        /// <summary>
        /// The number of items that be held in the local memory before it syncs to storage. This significantly improves the performance of your application, 
        /// but there may be a lag in the results that you see
        /// </summary>
        public int FlushThreshold { get; set; }

        /// <summary>
        /// If there are any items in memory when this timer is invoked, the container will flush to cloud storage
        /// </summary>
        public TimeSpan FlushTimer { get; set; }

        /// <summary>
        /// Indicates a timeout will be applied to any messages in the queue. Once the timer elapses, any queue items get flushed
        /// </summary>
        public bool UseFlushTimer { get; set; }

        public StorageContainerConfiguration()
        {
            //defaults
            FlushThreshold = 20;
            FlushTimer = TimeSpan.FromMinutes(1);
        }
    }
}