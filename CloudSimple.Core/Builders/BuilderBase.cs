namespace CloudSimple.Core.Builders
{
    public class BuilderBase
    {
        protected internal CloudSimpleContainer Container { get; private set; }

        public BuilderBase(CloudSimpleContainer container)
        {
            this.Container = container;
        }
    }
}