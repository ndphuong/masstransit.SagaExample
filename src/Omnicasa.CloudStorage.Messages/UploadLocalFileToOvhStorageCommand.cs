using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicasa.CloudStorage.Messages
{
    public interface UploadLocalFileToOvhStorageCommand
    {
        Guid CorrelationId { set; get; }
        string FileName { set; get; }
        string LocalFilePath { set; get; }
    }
}
