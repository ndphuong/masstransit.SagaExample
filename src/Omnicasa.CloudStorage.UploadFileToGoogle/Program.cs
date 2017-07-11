using MassTransit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicasa.CloudStorage.UploadFileToGoogle
{
    class Program
    {
        private static IBusControl _bus;
        private static BusHandle _busHandle;

        public static IBus Bus
        {
            get { return _bus; }
        }
        static void Main(string[] args)
        {
            _bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var host = x.Host(new Uri(ConfigurationManager.AppSettings["RabbitMQHost"]), h =>
                {
                    h.Username(ConfigurationManager.AppSettings["RabbitMQUsername"]);
                    h.Password(ConfigurationManager.AppSettings["RabbitMQPassword"]);
                });

                x.ReceiveEndpoint(host, "cloud_storage_upload_file_to_google_test", e =>
                {
                    e.PrefetchCount = 8;
                    e.Consumer<UploadLocalFileToGoogleStorageConsumer>();
                });
            });

            _bus.Start();
        }
    }
}
