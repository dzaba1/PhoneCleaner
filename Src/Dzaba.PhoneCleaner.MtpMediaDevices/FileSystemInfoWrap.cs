using Dzaba.PhoneCleaner.Lib.Device;
using System;
using MediaDevices;

namespace Dzaba.PhoneCleaner.MtpMediaDevices
{
    internal class FileSystemInfoWrap<T> : IDeviceSystemInfo
        where T : MediaFileSystemInfo
    {
        private readonly Lazy<T> info;

        public FileSystemInfoWrap(string path,
            Func<string, T> fileInfoProvider)
        {
            ArgumentNullException.ThrowIfNull(fileInfoProvider, nameof(fileInfoProvider));
            ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));

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
