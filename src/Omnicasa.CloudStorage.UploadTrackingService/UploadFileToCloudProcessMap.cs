namespace Omnicasa.CloudStorage.UploadTrackingService
{
    using MassTransit.EntityFrameworkIntegration;


    public class UploadFileToCloudProcessMap :
        SagaClassMapping<UploadFileToCloudProcess>
    {
        public UploadFileToCloudProcessMap()
        {
        }
    }
}