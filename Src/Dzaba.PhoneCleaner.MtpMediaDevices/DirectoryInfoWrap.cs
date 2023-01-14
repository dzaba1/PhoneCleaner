using Dzaba.PhoneCleaner.Lib.Device;
using MediaDevices;

namespace Dzaba.PhoneCleaner.MtpMediaDevices
{
    internal sealed class DirectoryInfoWrap : FileSystemInfoWrap<MediaDirectoryInfo>, IDeviceDirectoryInfo
    {
        public DirectoryInfoWrap(MediaDirectoryInfo info)
            : base(info)
        {

        }
    }
}
