using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudSimple.Core.Builders
{
    public class BuilderBase
    {
        protected internal CloudSimpleContainer Container { get; private set; }
        protected internal List<IStorageContainer> Handlers
        {
            get { return _handlerSelector(Container).ToList(); }
        } 

        private readonly Func<CloudSimpleContainer, IEnumerable<IStorageContainer>> _handlerSelector;

        public BuilderBase(CloudSimpleContainer container, Func<CloudSimpleContainer, IEnumerable<IStorageContainer>> handlerSelector)
        {
            _handlerSelector = handlerSelector;
            this.Container = container;
        }
    }
}