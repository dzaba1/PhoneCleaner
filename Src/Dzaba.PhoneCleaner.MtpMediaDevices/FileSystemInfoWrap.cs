using Dzaba.PhoneCleaner.Lib.Device;
using System;
using MediaDevices;
using Dzaba.PhoneCleaner.Utils;

namespace Dzaba.PhoneCleaner.MtpMediaDevices
{
    internal class FileSystemInfoWrap<T> : IDeviceSystemInfo
        where T : MediaFileSystemInfo
    {
        private readonly Lazy<T> info;

        public FileSystemInfoWrap(string path,
            Func<string, T> fileInfoProvider)
        {
            Require.NotNull(fileInfoProvider, nameof(fileInfoProvider));
            Require.NotWhiteSpace(path, nameof(path));

            info = new Lazy<T>(() => fileInfoProvider(path));
            FullName = path;
        }

        protected T Info => info.Value;

        public string Name => Info.Name;

        public string FullName { get; }

        public DateTime? CreationTime => Info.CreationTime;

        public DateTime? ModificationTime => Info.LastWriteTime;
    }
}
