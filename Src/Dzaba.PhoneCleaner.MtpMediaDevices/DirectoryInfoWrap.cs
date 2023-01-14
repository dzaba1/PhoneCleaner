using Dzaba.PhoneCleaner.Lib.Device;
using MediaDevices;
using System.Linq;

namespace Dzaba.PhoneCleaner.MtpMediaDevices
{
    internal sealed class DirectoryInfoWrap : FileSystemInfoWrap<MediaDirectoryInfo>, IDeviceDirectoryInfo
    {
        public DirectoryInfoWrap(MediaDirectoryInfo info)
            : base(info)
        {

        }

        public bool IsEmpty()
        {
            return !Info.EnumerateFiles().Any() && !Info.EnumerateDirectories().Any();
        }
    }
}
