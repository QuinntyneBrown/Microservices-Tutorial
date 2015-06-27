using Product_API.Adapters.Service;
using Topshelf;

namespace Product_API
{
    class Program
    {
        static void Main(string[] args)
        {
           HostFactory.Run(x => x.Service<ProductService>(sc =>
           {
               sc.ConstructUsing(() => new ProductService());

                    // the start and stop methods for the service
                    sc.WhenStarted((s, hostcontrol) => s.Start(hostcontrol));
               sc.WhenStopped((s, hostcontrol) => s.Stop(hostcontrol));

                    // optional, when shutdown is supported
                    sc.WhenShutdown((s, hostcontrol) => s.Shutdown(hostcontrol));
           }));

        }
    }
}
