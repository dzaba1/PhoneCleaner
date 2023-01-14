using Dzaba.PhoneCleaner.Lib.Device;
using System.IO;
using System.Linq;

namespace Dzaba.PhoneCleaner.Lib.Tests.Device
{
    internal sealed class DirectoryInfoWrap : FileSystemInfoWrap<DirectoryInfo>, IDeviceDirectoryInfo
    {
        public DirectoryInfoWrap(DirectoryInfo info)
            : base(info)
        {

        }

        public bool IsEmpty()
        {
            return !Info.EnumerateFiles().Any() && !Info.EnumerateDirectories().Any();
        }
    }
}
