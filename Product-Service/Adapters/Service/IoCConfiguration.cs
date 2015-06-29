using System;
using System.IO;
using Grean.AtomEventStore;
using Microsoft.Practices.Unity;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Polly;
using Products_Core.Adapters.Atom;
using Products_Core.Adapters.DataAccess;
using Products_Core.Ports.Commands;
using Products_Core.Ports.Events;
using Products_Core.Ports.Handlers;
using Product_API.Adapters.Configuration;
using Product_API.Adapters.Controllers;
using Product_Service;

namespace Product_API.Adapters.Service
{
    internal static class IoCConfiguration
    {
        public static void Run(UnityContainer container)
        {
            container.RegisterType<FeedController>();
            container.RegisterType<ProductsController>();
            container.RegisterInstance(typeof(ILog), LogProvider.For<ProductService>(), new ContainerControlledLifetimeManager());
            container.RegisterType<IProductsDAO, ProductsDAO>();
            container.RegisterType<AddProductCommandHandler>();
            container.RegisterType<ChangeProductCommandHandler>();
            container.RegisterType<ProductAddedEventHandler>();
            container.RegisterType<ProductChangedEventHandler>();
            container.RegisterType<ProductRemovedEventHandler>();
            container.RegisterType<RemoveProductCommandHandler>();

            var storage = new AtomEventsInFiles(new DirectoryInfo(Globals.StoragePath));
            var serializer = new DataContractContentSerializer(
                DataContractContentSerializer
                    .CreateTypeResolver(typeof (ProductEntry).Assembly)
                );

            var events = new FifoEvents<ProductEntry>(
                Globals.EventStreamId, 
                storage,       
                serializer);

            container.RegisterInstance(typeof (IObserver<ProductEntry>), events, new TransientLifetimeManager());

            var handlerFactory = new UnityHandlerFactory(container);

            var subscriberRegistry = new SubscriberRegistry
            {
                {typeof(AddProductCommand), typeof(AddProductCommandHandler)},
                {typeof(ChangeProductCommand), typeof(ChangeProductCommandHandler)},
                {typeof(RemoveProductCommand), typeof(RemoveProductCommandHandler)},
                {typeof(ProductAddedEvent), typeof(ProductAddedEventHandler)},
                {typeof(ProductRemovedEvent), typeof(ProductRemovedEventHandler)},
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
