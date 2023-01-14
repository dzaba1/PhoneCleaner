using Dzaba.PhoneCleaner.Lib.Device;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests.Device
{
    internal sealed class DirectoryInfoWrap : FileSystemInfoWrap<DirectoryInfo>, IDeviceDirectoryInfo
    {
        public DirectoryInfoWrap(DirectoryInfo info)
            : base(info)
        {

        }
    }
}
