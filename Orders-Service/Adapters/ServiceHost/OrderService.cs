using System;
using Orders_Core.Ports.Commands;
using Orders_Core.Ports.Handlers;
using Orders_Service.Ports.Mappers;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using paramore.brighter.commandprocessor.messaginggateway.rmq;
using paramore.brighter.serviceactivator;
using Polly;
using TinyIoC;
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

            var container = new TinyIoCContainer();
            container.Register<ILog>(logger);
            container.Register<IAmAMessageMapper<AddOrderCommand>, AddOrderCommandMessageMapper>();
            container.Register<IAmAMessageMapper<CompleteOrderCommand>, CompleteOrderCommandMessageMapper>();
            container.Register<IAmAMessageMapper<EditOrderCommand>, EditOrderCommandMessageMapper>();
            container.Register<IAmAMessageMapper<OrderUpdateCommand>, OrderUpdateCommandMessageMapper>();
            container.Register<AddOrderCommandHandler, AddOrderCommandHandler>();
            container.Register<CompleteOrderCommandHandler, CompleteOrderCommandHandler>();
            container.Register<EditOrderCommandHandler, EditOrderCommandHandler>();
            container.Register<OrderUpdateCommandHandler, OrderUpdateCommandHandler>();

            var handlerFactory = new TinyIocHandlerFactory(container);
            var messageMapperFactory = new TinyIoCMessageMapperFactory(container);

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
