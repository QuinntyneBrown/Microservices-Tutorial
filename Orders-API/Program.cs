using Orders_API.Adapters.Service;
using Topshelf;

namespace Orders_API
{
    class Program
    {
        static void Main(string[] args)
        {
           HostFactory.Run(x => x.Service<OrderService>(sc =>
           {
               sc.ConstructUsing(() => new OrderService());

               // the start and stop methods for the service
               sc.WhenStarted((s, hostcontrol) => s.Start(hostcontrol));
               sc.WhenStopped((s, hostcontrol) => s.Stop(hostcontrol));

               // optional, when shutdown is supported
               sc.WhenShutdown((s, hostcontrol) => s.Shutdown(hostcontrol));
           }));
        }
    }
}
