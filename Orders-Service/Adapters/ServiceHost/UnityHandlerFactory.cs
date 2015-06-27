using System;
using Microsoft.Practices.Unity;
using paramore.brighter.commandprocessor;

namespace Orders_Service.Adapters.ServiceHost
{

   public class UnityHandlerFactory : IAmAHandlerFactory
    {
        private readonly UnityContainer _container;

        public UnityHandlerFactory(UnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Creates the specified handler type.
        /// </summary>
        /// <param name="handlerType">Type of the handler.</param>
        /// <returns>IHandleRequests.</returns>
        public IHandleRequests Create(Type handlerType)
        {
            return (IHandleRequests)_container.Resolve(handlerType);
        }

        /// <summary>
        /// Releases the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public void Release(IHandleRequests handler)
        {
            var disposable = handler as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }

}
