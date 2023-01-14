using Dzaba.PhoneCleaner.Lib;
using Dzaba.PhoneCleaner.Lib.Device;
using MediaDevices;

namespace Dzaba.PhoneCleaner.MtpMediaDevices
{
    internal sealed class DirectoryInfoWrap : IDeviceDirectoryInfo
    {
        private readonly MediaDirectoryInfo dirInfo;

        public DirectoryInfoWrap(MediaDirectoryInfo dirInfo)
        {
            Require.NotNull(dirInfo, nameof(dirInfo));

            this.dirInfo = dirInfo;
        }

        public string Name => dirInfo.Name;

        public string FullName => dirInfo.FullName;
    }
}
