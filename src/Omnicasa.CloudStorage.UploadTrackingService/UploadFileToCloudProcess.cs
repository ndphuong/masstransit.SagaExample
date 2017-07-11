using Automatonymous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicasa.CloudStorage.UploadTrackingService
{
    public class UploadFileToCloudProcess : SagaStateMachineInstance
    {
        public int UploadStatus { get; set; }
        public string CurrentState { get; set; }
        public string FileName { get; set; }
        public Guid CorrelationId { set; get; }
        public string ServerId { set; get; }
    }
}
