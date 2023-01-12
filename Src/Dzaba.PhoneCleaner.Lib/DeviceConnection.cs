using MediaDevices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dzaba.PhoneCleaner.Lib
{
    internal sealed class DeviceConnection : IDeviceConnection
    {
        private readonly MediaDevice mediaDevice;

        public DeviceConnection(string friendlyName)
        {
            mediaDevice = MediaDevice.GetDevices()
                .FirstOrDefault(x => x.FriendlyName == friendlyName);

            if (mediaDevice == null)
            {
                throw new InvalidOperationException($"Could't find any devices with name '{friendlyName}'.");
            }

            mediaDevice.Connect();
        }

        public void CopyFile(string source, Stream dest)
        {
            Require.NotWhiteSpace(source, nameof(source));
            Require.NotNull(dest, nameof(dest));

            mediaDevice.DownloadFile(source, dest);
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            Require.NotWhiteSpace(path, nameof(path));

            mediaDevice.DeleteDirectory(path, recursive);
        }

        public void DeleteFile(string path)
        {
            Require.NotWhiteSpace(path, nameof(path));

            mediaDevice.DeleteFile(path);
        }

        public void Dispose()
        {
            mediaDevice?.Dispose();
        }

        public IEnumerable<string> EnumerateDirectories(string path, SearchOption searchOption)
        {
            Require.NotWhiteSpace(path, nameof(path));

            return mediaDevice.EnumerateDirectories(path, "*", searchOption);
        }

        public IEnumerable<string> EnumerateFiles(string path, SearchOption searchOption)
        {
            Require.NotWhiteSpace(path, nameof(path));

            return mediaDevice.EnumerateFiles(path, "*.*", searchOption);
        }
    }
}
