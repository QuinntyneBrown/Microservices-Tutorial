using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Orders_API.Adapters.Configuration;
using Orders_Core;
using Topshelf;

namespace Orders_API.Adapters.Service
{
    class OrderService : ServiceControl
    {
        private IDisposable _app;
        public bool Start(HostControl hostControl)
        {
            var configuration = OrderServerConfiguration.GetConfiguration();
            var uri = configuration.Address.Uri;
            Globals.HostName = uri.Host + ":" + uri.Port;
            _app = WebApp.Start<StartUp>(configuration.Address.Uri.AbsoluteUri);
            return true;
        }


        public bool Stop(HostControl hostControl)
        {
            _app.Dispose();
            return true;
        }

        public void Shutdown(HostControl hostcontrol)
        {
            return;
        }
    }
}
