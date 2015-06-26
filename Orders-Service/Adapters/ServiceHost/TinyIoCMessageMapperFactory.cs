using System;
using paramore.brighter.commandprocessor;
using TinyIoC;

namespace Orders_Service.Adapters.ServiceHost
{
    internal class TinyIoCMessageMapperFactory : IAmAMessageMapperFactory
    {
        private readonly TinyIoCContainer _container;

        public TinyIoCMessageMapperFactory(TinyIoCContainer container)
        {
            _container = container;
        }

        public IAmAMessageMapper Create(Type messageMapperType)
        {
            return (IAmAMessageMapper)_container.Resolve(messageMapperType);
        }
    }
}
