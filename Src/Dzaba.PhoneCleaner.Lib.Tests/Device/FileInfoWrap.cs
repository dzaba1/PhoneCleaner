using Dzaba.PhoneCleaner.Lib.Device;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests.Device
{
    internal sealed class FileInfoWrap : FileSystemInfoWrap<FileInfo>, IDeviceFileInfo
    {
        public FileInfoWrap(FileInfo fileInfo)
            : base(fileInfo)
        {
            
        }
    }
}
