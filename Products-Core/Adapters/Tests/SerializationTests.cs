using System.IO;
using System.Xml.Serialization;
using Machine.Specifications;
using Products_Core.Model;
using Products_Core.Ports.Resources;
using Product_Service;

namespace Products_Core.Adapters.Tests
{
    public class SerializationTests
    {
        private static T SerializeToXml<T>(T value)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, value);
                var xml = textWriter.ToString();

                using (var textReader = new StringReader(xml))
                {
                    var obj = (T)serializer.Deserialize(textReader);
                    return obj;
                }
            }
        }

        public class When_serializing_a_product_to_xml
        {
            private static ProductModel s_product;
            private static ProductModel s_productModel;

            private Establish _context = () =>
            {
                Globals.HostName = "host.com";
                s_product = new ProductModel(new Product("Vanilla cake", "Sweet vanilla", 10.50), Globals.HostName);
            };

            private Because _of = () => s_productModel = SerializeToXml(s_product);

            private It _should_set_the_name = () => s_productModel.ProductName.ShouldEqual("Vanilla cake");
            private It _should_set_the_title = () => s_productModel.ProductDescription.ShouldEqual("Sweet vanilla");
            private It _should_set_the_profile_name = () => s_productModel.ProductPrice.ShouldEqual(10.50);
        }

        public class When_serializing_an_add_command_to_xml
        {
            private static AddProductModel s_product;
            private static AddProductModel s_productModel;

            private Establish _context = () =>
            {
                Globals.HostName = "host.com";
                s_product = new AddProductModel()
                {
                    ProductDescription = "Sweet, sweet, almond Cake",
                    ProductName = "Almond Cake",
                    ProductPrice = 10.50
                };
            };

            private Because _of = () => s_productModel = SerializeToXml(s_product);

            private It _should_set_the_name = () => s_productModel.ProductName.ShouldEqual("Almond Cake");
            private It _should_set_the_title = () => s_productModel.ProductDescription.ShouldEqual("Sweet, sweet, almond Cake");
            private It _should_set_the_profile_name = () => s_productModel.ProductPrice.ShouldEqual(10.50);
            
        }

    }
}
