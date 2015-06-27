using System;
using System.Configuration;

namespace Product_API.Adapters.Configuration
{
    public class ProductServerConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("address")]
        public ProductUriSpecification Address
        {
            get { return this["address"] as ProductUriSpecification; }
            set { this["address"] = value; }
        }

        public static ProductServerConfiguration GetConfiguration()
        {
            var configuration =
                ConfigurationManager.GetSection("productServer") as ProductServerConfiguration;

            if (configuration != null)
                return configuration;

            return new ProductServerConfiguration();
        }
    }

    public class ProductUriSpecification : ConfigurationElement
    {
        [ConfigurationProperty("uri", DefaultValue = "http://localhost:3416/", IsRequired = true)]
        public Uri Uri
        {
            get { return (Uri)this["uri"]; }
            set { this["uri"] = value; }
        }
    }
}
