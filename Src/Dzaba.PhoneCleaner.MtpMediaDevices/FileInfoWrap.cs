using Dzaba.PhoneCleaner.Lib.Device;
using MediaDevices;
using System;

namespace Dzaba.PhoneCleaner.MtpMediaDevices
{
    internal sealed class FileInfoWrap : FileSystemInfoWrap<MediaFileInfo>, IDeviceFileInfo
    {
        public FileInfoWrap(string path,
            Func<string, MediaFileInfo> fileProvider)
            : base(path, fileProvider)
        {

        }
    }
}
