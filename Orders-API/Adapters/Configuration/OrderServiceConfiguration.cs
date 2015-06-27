using System;
using System.Configuration;

namespace Orders_API.Adapters.Configuration
{
    public class OrderServerConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("address")]
        public OrderUriSpecification Address
        {
            get { return this["address"] as OrderUriSpecification ; }
            set { this["address"] = value; }
        }

        public static OrderServerConfiguration  GetConfiguration()
        {
            var configuration =
                ConfigurationManager.GetSection("orderServer") as OrderServerConfiguration ;

            if (configuration != null)
                return configuration;

            return new OrderServerConfiguration ();
        }
    }

    public class OrderUriSpecification : ConfigurationElement
    {
        [ConfigurationProperty("uri", DefaultValue = "http://localhost:3416/", IsRequired = true)]
        public Uri Uri
        {
            get { return (Uri)this["uri"]; }
            set { this["uri"] = value; }
        }
    }
}
