using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Lib;
using System;
using MediaDevices;

namespace Dzaba.PhoneCleaner.MtpMediaDevices
{
    internal class FileSystemInfoWrap<T> : IDeviceSystemInfo
        where T : MediaFileSystemInfo
    {
        public FileSystemInfoWrap(T info)
        {
            Require.NotNull(info, nameof(info));

            Info = info;
        }

        protected T Info { get; private set; }

        public string Name => Info.Name;

        public string FullName => Info.FullName;

        public DateTime? CreationTime => Info.CreationTime;

        public DateTime? ModificationTime => Info.LastWriteTime;
    }
}
