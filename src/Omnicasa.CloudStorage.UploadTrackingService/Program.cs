using Automatonymous;
using MassTransit;
using MassTransit.EntityFrameworkIntegration;
using MassTransit.EntityFrameworkIntegration.Saga;
using MassTransit.Saga;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicasa.CloudStorage.UploadTrackingService
{
    class Program
    {
        private static IBusControl _bus;
        static UploadFileToCloudProcessStateMachine _machine;
        static Lazy<ISagaRepository<UploadFileToCloudProcess>> _repository;

        public static IBus Bus
        {
            get { return _bus; }
        }
        static void Main(string[] args)
        {
            _machine = new UploadFileToCloudProcessStateMachine();


            SagaDbContextFactory sagaDbContextFactory =
                () => new SagaDbContext<UploadFileToCloudProcess, UploadFileToCloudProcessMap>(@"data source=hosting11p.omnicasa.com;initial catalog=SagaManagement;persist security info=True;Integrated Security=SSPI;");
            _repository = new Lazy<ISagaRepository<UploadFileToCloudProcess>>(
               () => new EntityFrameworkSagaRepository<UploadFileToCloudProcess>(sagaDbContextFactory));

            _bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var host = x.Host(new Uri(ConfigurationManager.AppSettings["RabbitMQHost"]), h =>
                {
                    h.Username(ConfigurationManager.AppSettings["RabbitMQUsername"]);
                    h.Password(ConfigurationManager.AppSettings["RabbitMQPassword"]);
                });

                x.ReceiveEndpoint(host, "cloud_storage_upload_tracking_test", e =>
                {
                    e.PrefetchCount = 10;
                    e.StateMachineSaga(_machine, _repository.Value);
                });
            });

            _bus.Start();
        }
    }
}
