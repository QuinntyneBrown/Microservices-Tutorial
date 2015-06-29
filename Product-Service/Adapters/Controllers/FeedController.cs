using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using Grean.AtomEventStore;
using Products_Core.Adapters.Atom;
using Product_Service;

namespace Product_API.Adapters.Controllers
{
    public class FeedController : ApiController
    {
        private FifoEvents<ProductEntry> _events;
        private static readonly AtomEventsInFiles s_storage;
        private static readonly DataContractContentSerializer s_serializer;

        static FeedController()
        {
            s_storage = new AtomEventsInFiles(new DirectoryInfo(Globals.StoragePath));
            s_serializer = new DataContractContentSerializer(
                DataContractContentSerializer
                    .CreateTypeResolver(typeof (ProductEntry).Assembly)
                );

        }

        public FeedController()
        {
            _events = new FifoEvents<ProductEntry>(
                Globals.EventStreamId, 
                s_storage,       
                s_serializer);   
        }

        [HttpGet]
        public HttpResponseMessage Recent()
        {
            var feed = _events.ReadFirst();
            if (feed != null)
            {
                var sb = new StringBuilder();
                using (var xmlWriter = XmlWriter.Create(sb))
                {
                    feed.WriteTo(xmlWriter,
                                 new DataContractContentSerializer(
                                     DataContractContentSerializer.CreateTypeResolver(typeof (ProductEntry).Assembly)));
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(sb.ToString())
                    };
                }
            }

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}
