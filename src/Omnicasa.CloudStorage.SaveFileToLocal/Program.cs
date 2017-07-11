using Automatonymous;
using MassTransit;
using MassTransit.EntityFrameworkIntegration;
using MassTransit.EntityFrameworkIntegration.Saga;
using MassTransit.Saga;
using Omnicasa.CloudStorage.Messages;
using System;
using System.Configuration;
using System.Threading;

namespace Omnicasa.CloudStorage.SaveFileToLocal
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
            });

            _bus.Start();

            var endpointGoogleQueue = _bus.GetSendEndpoint(new Uri("rabbitmq://172.16.3.178/cloudstorage/cloud_storage_upload_file_to_google_test")).Result;
            var endpointOvhQueue = _bus.GetSendEndpoint(new Uri("rabbitmq://172.16.3.178/cloudstorage/cloud_storage_upload_file_to_ovh_test")).Result;
            var endpointTrackingQueue = _bus.GetSendEndpoint(new Uri("rabbitmq://172.16.3.178/cloudstorage/cloud_storage_upload_tracking_test")).Result;

            while (true)
            {
                Console.WriteLine($@"Number of messages: ");
                string str = Console.ReadLine();
                int number = int.Parse(str);
                for (int i = 0; i < number; i++)
                {
                    

                    string fileName = $@"file name {i}";
                    Console.WriteLine($@"Save file to local: {fileName}");
                    Console.WriteLine($@"Send FileSavedToLocalEvent");
                    //Guid correlationId = new Guid("1a17f871-ff6f-4c42-b0af-f021e10ff659");
                    Guid correlationId = Guid.NewGuid();
                    endpointTrackingQueue.Send<FileSavedToLocalEvent>(new
                    {
                        FileName = fileName,
                        TimeStamp = DateTime.Now,
                        LocalFilePath = fileName,
                        ServerId = "ndphuong",
                        CorrelationId = correlationId
                    });

                    Console.WriteLine($@"Send UploadLocalFileToGoogleStorageCommand");
                    endpointGoogleQueue.Send<UploadLocalFileToGoogleStorageCommand>(new
                    {
                        FileName = fileName,
                        TimeStamp = DateTime.Now,
                        LocalFilePath = fileName,
                        ServerId = "ndphuong",
                        CorrelationId = correlationId
                    });

                    Console.WriteLine($@"Send UploadLocalFileToOvhStorageCommand");
                    endpointOvhQueue.Send<UploadLocalFileToOvhStorageCommand>(new
                    {
                        FileName = fileName,
                        TimeStamp = DateTime.Now,
                        LocalFilePath = fileName,
                        ServerId = "ndphuong",
                        CorrelationId = correlationId
                    });
                }
            }
        }
    }
}