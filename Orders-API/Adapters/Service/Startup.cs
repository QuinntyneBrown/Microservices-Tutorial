using System.Web.Http;
using CacheCow.Server;
using Microsoft.Owin.Diagnostics;
using Microsoft.Practices.Unity;
using Owin;
using WebApiContrib.IoC.Unity;

namespace Orders_API.Adapters.Service
{
    internal class StartUp
    {
        private static UnityContainer s_container;

        public void Configuration(IAppBuilder builder)
        {
            var configuration = new HttpConfiguration();

            ConfigureDependencyInjection(configuration);

            MapRoutes(configuration);

            ConfigureCaching(configuration);

            ConfigureFormatting(configuration);

            ConfigureWelcomePage(builder);

            ConfigureDiagnostics(configuration);

            ConfigureErrorPage(builder);

            builder.UseWebApi(configuration);
        }

        private void ConfigureErrorPage(IAppBuilder builder)
        {
            builder.UseErrorPage(
                new ErrorPageOptions()
                {
                    ShowEnvironment = true,
                    ShowSourceCode = true,
                    ShowExceptionDetails = true,
                    ShowHeaders = true,
                    ShowCookies = true,
                    ShowQuery = true
                }
            );
        }

        private void ConfigureWelcomePage(IAppBuilder builder)
        {
            builder.UseWelcomePage("/status");
        }


        private void ConfigureCaching(HttpConfiguration configuration)
        {
            var cachingHandler = new CachingHandler(configuration);
            configuration.MessageHandlers.Add(cachingHandler);
            s_container.RegisterInstance<ICachingHandler>(cachingHandler);
        }

        private static void ConfigureDiagnostics(HttpConfiguration configuration)
        {
            configuration.EnableSystemDiagnosticsTracing();
        }

        private static void ConfigureFormatting(HttpConfiguration configuration)
        {
            var xml = configuration.Formatters.XmlFormatter;
            xml.UseXmlSerializer = true;
        }

        private static void MapRoutes(HttpConfiguration configuration)
        {
            configuration.MapHttpAttributeRoutes();
            configuration.Routes.MapHttpRoute(
                name: "OrdersAPI",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }

        private static void ConfigureDependencyInjection(HttpConfiguration configuration)
        {
            s_container = new UnityContainer();
            IoCConfiguration.Run(s_container);
            configuration.DependencyResolver = new UnityResolver(s_container);
        }
    }
}
