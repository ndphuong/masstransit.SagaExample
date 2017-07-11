using MassTransit;
using System;
using System.Configuration;

namespace Omnicasa.CloudStorage.UploadFileToOvh
{
    internal class Program
    {
        private static IBusControl _bus;
        private static BusHandle _busHandle;

        public static IBus Bus
        {
            get { return _bus; }
        }

        private static void Main(string[] args)
        {
            _bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var host = x.Host(new Uri(ConfigurationManager.AppSettings["RabbitMQHost"]), h =>
                {
                    h.Username(ConfigurationManager.AppSettings["RabbitMQUsername"]);
                    h.Password(ConfigurationManager.AppSettings["RabbitMQPassword"]);
                });

                x.ReceiveEndpoint(host, "cloud_storage_upload_file_to_ovh_test", e =>
                {
                    e.Consumer<UploadLocalFileToOvhStorageConsumer>();
                });
            });

            _bus.Start();
        }
    }
}