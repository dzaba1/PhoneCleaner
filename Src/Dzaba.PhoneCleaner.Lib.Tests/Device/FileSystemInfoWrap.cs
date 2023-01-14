using Dzaba.PhoneCleaner.Lib.Device;
using System;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests.Device
{
    internal class FileSystemInfoWrap<T> : IDeviceSystemInfo
        where T : FileSystemInfo
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
