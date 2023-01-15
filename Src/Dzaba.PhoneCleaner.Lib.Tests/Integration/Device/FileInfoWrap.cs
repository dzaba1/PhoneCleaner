using Dzaba.PhoneCleaner.Lib.Device;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests.Integration.Device
{
    internal sealed class FileInfoWrap : FileSystemInfoWrap<FileInfo>, IDeviceFileInfo
    {
        public FileInfoWrap(FileInfo fileInfo)
            : base(fileInfo)
        {
            
        }
    }
}
