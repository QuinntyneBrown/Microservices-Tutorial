using System;
using Microsoft.Practices.Unity;
using Orders_Core.Adapters.DataAccess;
using Orders_Core.Ports.Commands;
using Orders_Core.Ports.Handlers;
using Orders_Service.Ports.Mappers;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using paramore.brighter.commandprocessor.messaginggateway.rmq;
using paramore.brighter.serviceactivator;
using Polly;
using Topshelf;

namespace Orders_Service.Adapters.ServiceHost
{
    class OrderService: ServiceControl
    {
        private Dispatcher _dispatcher;

        public OrderService()
        {
            log4net.Config.XmlConfigurator.Configure();

            var logger = LogProvider.For<OrderService>();

            var container = new UnityContainer();
            container.RegisterInstance(typeof(ILog), LogProvider.For<OrderService>(), new ContainerControlledLifetimeManager());
            container.RegisterType<AddOrderCommandMessageMapper>();
            container.RegisterType<CompleteOrderCommandMessageMapper>();
            container.RegisterType<EditOrderCommandMessageMapper>();
            container.RegisterType<OrderUpdateCommandMessageMapper>();
            container.RegisterType<AddOrderCommandHandler>();
            container.RegisterType<CompleteOrderCommandHandler>();
            container.RegisterType<EditOrderCommandHandler>();
            container.RegisterType<OrderUpdateCommandHandler>();
            container.RegisterType<OrdersDAO>();

            var handlerFactory = new UnityHandlerFactory(container);
            var messageMapperFactory = new UnityMessageMapperFactory(container);

            var subscriberRegistry = new SubscriberRegistry();
            subscriberRegistry.Register<AddOrderCommand, AddOrderCommandHandler>();
            subscriberRegistry.Register<CompleteOrderCommand, CompleteOrderCommandHandler>();
            subscriberRegistry.Register<EditOrderCommand, EditOrderCommandHandler>();
            subscriberRegistry.Register<OrderUpdateCommand, OrderUpdateCommandHandler>();

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
                .Logger(logger)
                .NoTaskQueues()
                .RequestContextFactory(new InMemoryRequestContextFactory())
                .Build();

            //create message mappers
            var messageMapperRegistry = new MessageMapperRegistry(messageMapperFactory)
            {
            };

            var rmqMessageConsumerFactory = new RmqMessageConsumerFactory(logger);

            _dispatcher = DispatchBuilder.With()
                .Logger(logger)
                .CommandProcessor(commandProcessor)
                .MessageMappers(messageMapperRegistry)
                .ChannelFactory(new InputChannelFactory(rmqMessageConsumerFactory))
                .ConnectionsFromConfiguration()
                .Build();
        }

        public bool Start(HostControl hostControl)
        {
            _dispatcher.Receive();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _dispatcher.End().Wait();
            _dispatcher = null;
            return true;
        }

        public void Shutdown(HostControl hostcontrol)
        {
            if (_dispatcher != null)
                _dispatcher.End().Wait();
        }
    }
}
