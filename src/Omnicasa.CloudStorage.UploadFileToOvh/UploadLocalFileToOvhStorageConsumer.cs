using MassTransit;
using Omnicasa.CloudStorage.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Omnicasa.CloudStorage.UploadFileToOvh
{
    public class UploadLocalFileToOvhStorageConsumer : IConsumer<UploadLocalFileToOvhStorageCommand>
    {
        public Task Consume(ConsumeContext<UploadLocalFileToOvhStorageCommand> context)
        {
            return Task.Factory.StartNew(()=> {
                Console.WriteLine($@"Uploading local file {context.Message.LocalFilePath} to Ovh");
                //Thread.Sleep(1000);
                Console.WriteLine($@"Send event completed");
                var endpoint = Program.Bus.GetSendEndpoint(new Uri("rabbitmq://172.16.3.178/cloudstorage/cloud_storage_upload_tracking_test")).Result;
                endpoint.Send<FileUploadedToOvhEvent>(new {
                    CorrelationId = context.Message.CorrelationId,
                    FileName = context.Message.FileName,
                    LocalFilePath = context.Message.LocalFilePath
                });
            });
        }
    }
}
