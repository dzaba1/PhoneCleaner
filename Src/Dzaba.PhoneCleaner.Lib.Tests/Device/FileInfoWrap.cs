using Dzaba.PhoneCleaner.Lib.Device;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests.Device
{
    internal sealed class FileInfoWrap : IDeviceFileInfo
    {
        private readonly FileInfo fileInfo;

        public FileInfoWrap(FileInfo fileInfo)
        {
            Require.NotNull(fileInfo, nameof(fileInfo));

            this.fileInfo = fileInfo;
        }

        public string Name => fileInfo.Name;

        public string FullName => fileInfo.FullName;
    }
}
