using Dzaba.PhoneCleaner.Lib.Device;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests.Device
{
    internal sealed class DirectoryInfoWrap : IDeviceDirectoryInfo
    {
        private readonly DirectoryInfo dirInfo;

        public DirectoryInfoWrap(DirectoryInfo dirInfo)
        {
            Require.NotNull(dirInfo, nameof(dirInfo));

            this.dirInfo = dirInfo;
        }

        public string Name => dirInfo.Name;

        public string FullName => dirInfo.FullName;
    }
}
