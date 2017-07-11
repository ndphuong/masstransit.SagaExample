using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicasa.CloudStorage.Messages
{
    public interface FileUploadedToOvhEvent
    {
        string FileName { get; set; }
        DateTime? TimeStamp { get; set; }
        string LocalFilePath { set; get; }
        string GlobalFilePath { set; get; }
        Guid CorrelationId { get; set; }
        string ServerId { set; get; }
    }
}
