using System;
using System.IO;
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
            Globals.StoragePath = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString("N"));
            Globals.PageSize = 25;
            Globals.EventStreamId = Guid.Parse("{028872FE-CAD5-492A-822A-1AF00AD80708}");
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
