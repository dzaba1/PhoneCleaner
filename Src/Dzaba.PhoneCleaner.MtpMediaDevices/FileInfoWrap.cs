using Dzaba.PhoneCleaner.Lib.Device;
using MediaDevices;

namespace Dzaba.PhoneCleaner.MtpMediaDevices
{
    internal sealed class FileInfoWrap : FileSystemInfoWrap<MediaFileInfo>, IDeviceFileInfo
    {
        public FileInfoWrap(MediaFileInfo info)
            : base(info)
        {

        }
    }
}
