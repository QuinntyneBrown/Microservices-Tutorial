using System;
using Microsoft.Practices.Unity;
using Orders_API.Adapters.Configuration;
using Orders_API.Adapters.Controllers;
using Orders_Core.Adapters.DataAccess;
using Orders_Core.Ports.ViewModelRetrievers;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Polly;

namespace Orders_API.Adapters.Service
{
    internal static class IoCConfiguration
    {
        public static void Run(UnityContainer container)
        {
            container.RegisterType<OrdersController>();
            container.RegisterInstance(typeof(ILog), LogProvider.For<OrderService>(), new ContainerControlledLifetimeManager());
            container.RegisterType<OrdersDAO>();

            var handlerFactory = new UnityHandlerFactory(container);

            var subscriberRegistry = new SubscriberRegistry();
            //create policies

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(new[]
                    {
                        TimeSpan.FromMilliseconds(50),
                        TimeSpan.FromMilliseconds(100),
                        TimeSpan.FromMilliseconds(150)
                    });

            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreaker(1, TimeSpan.FromMilliseconds(500));

            var policyRegistry = new PolicyRegistry()
            {
                {CommandProcessor.RETRYPOLICY, retryPolicy},
                {CommandProcessor.CIRCUITBREAKER, circuitBreakerPolicy}
            };


            var commandProcessor = CommandProcessorBuilder.With()
                    .Handlers(new HandlerConfiguration(subscriberRegistry, handlerFactory))
                    .Policies(policyRegistry)
                    .Logger(container.Resolve<ILog>())
                    .NoTaskQueues()
                    .RequestContextFactory(new InMemoryRequestContextFactory())
                    .Build();

            container.RegisterInstance(typeof(IAmACommandProcessor), commandProcessor);
        }
    }
}
