using System;
using Microsoft.Practices.Unity;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Polly;
using Product_API.Adapters.Configuration;

namespace Product_API.Adapters.Service
{
    internal static class IoCConfiguration
    {
        public static void Run(UnityContainer container)
        {
            //container.RegisterType<DomainController>();
            container.RegisterInstance(typeof(ILog), LogProvider.For<ProductService>(), new ContainerControlledLifetimeManager());
            //container.RegisterType<AddFeedCommandHandler>();

            var handlerFactory = new UnityHandlerFactory(container);

            var subscriberRegistry = new SubscriberRegistry
            {
                //{typeof(AddFeedCommand), typeof(AddFeedCommandHandler)},
            };

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
