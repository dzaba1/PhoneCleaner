using Dzaba.PhoneCleaner.Lib;
using Dzaba.PhoneCleaner.Lib.Device;
using MediaDevices;

namespace Dzaba.PhoneCleaner.MtpMediaDevices
{
    internal sealed class FileInfoWrap : IDeviceFileInfo
    {
        private readonly MediaFileInfo fileInfo;

        public FileInfoWrap(MediaFileInfo fileInfo)
        {
            Require.NotNull(fileInfo, nameof(fileInfo));

            this.fileInfo = fileInfo;
        }

        public string Name => fileInfo.Name;

        public string FullName => fileInfo.FullName;
    }
}
