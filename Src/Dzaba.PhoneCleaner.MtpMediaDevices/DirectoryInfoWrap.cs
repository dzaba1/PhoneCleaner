using Dzaba.PhoneCleaner.Lib.Device;
using MediaDevices;
using System;
using System.Linq;

namespace Dzaba.PhoneCleaner.MtpMediaDevices
{
    internal sealed class DirectoryInfoWrap : FileSystemInfoWrap<MediaDirectoryInfo>, IDeviceDirectoryInfo
    {
        public DirectoryInfoWrap(string path,
            Func<string, MediaDirectoryInfo> dirProvider)
            : base(path, dirProvider)
        {

        }

        public bool IsEmpty()
        {
            return !Info.EnumerateFiles().Any() && !Info.EnumerateDirectories().Any();
        }
    }
}
