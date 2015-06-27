using System;
using Microsoft.Owin.Hosting;
using Product_API.Adapters.Configuration;
using Product_Service;
using Topshelf;

namespace Product_API.Adapters.Service
{
    internal class ProductService : ServiceControl
    {
        private IDisposable _app;
        public bool Start(HostControl hostControl)
        {
            var configuration = ProductServerConfiguration.GetConfiguration();
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
