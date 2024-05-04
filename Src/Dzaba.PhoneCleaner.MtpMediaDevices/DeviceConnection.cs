using Dzaba.PhoneCleaner.Lib.Device;
using MediaDevices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dzaba.PhoneCleaner.MtpMediaDevices
{
    internal sealed class DeviceConnection : IDeviceConnection
    {
        private MediaDevice mediaDevice;
        private readonly ILogger<DeviceConnection> logger;
        private readonly bool testOnly;

        public DeviceConnection(string friendlyName,
            ILogger<DeviceConnection> logger,
            bool testOnly)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(friendlyName, nameof(friendlyName));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));

            this.logger = logger;
            this.testOnly = testOnly;

            FriendlyName = friendlyName;
            Connect();
        }

        private void Connect()
        {
            mediaDevice = MediaDevice.GetDevices()
                .FirstOrDefault(x => x.FriendlyName == FriendlyName);

            if (mediaDevice == null)
            {
                throw new InvalidOperationException($"Could't find any devices with name '{FriendlyName}'.");
            }

            logger.LogInformation("Connecting to '{DeviceName}'", FriendlyName);
            mediaDevice.Connect();
        }

        public string FriendlyName { get; }

        public void CopyFile(string source, Stream dest)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(source, nameof(source));
            ArgumentNullException.ThrowIfNull(dest, nameof(dest));

            logger.LogInformation("Copy file from '{DeviceName}' '{Path}' to destination stream.", FriendlyName, source);

            if (!testOnly)
            {
                mediaDevice.DownloadFile(source, dest);
            }
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));

            logger.LogInformation("Deleting directory at '{DeviceName}' '{Path}'. Recursive: {Recursive}.", FriendlyName, path, recursive);

            if (!testOnly)
            {
                mediaDevice.DeleteDirectory(path, recursive);
            }
        }

        public void DeleteFile(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));

            logger.LogInformation("Deleting file at '{DeviceName}' '{Path}'.", FriendlyName, path);

            if (!testOnly)
            {
                mediaDevice.DeleteFile(path);
            }
        }

        public bool DirectoryExists(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));

            return mediaDevice.DirectoryExists(path);
        }

        public void Dispose()
        {
            logger.LogInformation("Disposing '{DeviceName}'", FriendlyName);
            mediaDevice?.Dispose();
            mediaDevice = null;
        }

        public IEnumerable<string> EnumerableDrives()
        {
            logger.LogInformation("Getting all drives from '{DeviceName}'", FriendlyName);

            return mediaDevice.GetDrives().Select(d => d.RootDirectory.FullName);
        }

        public IEnumerable<IDeviceDirectoryInfo> EnumerateDirectories(string path, SearchOption searchOption)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));

            logger.LogInformation("Getting all directories from '{DeviceName}' '{Path}'. Search options: {SearchOption}.", FriendlyName, path, searchOption);
            return mediaDevice.EnumerateDirectories(path, "*", searchOption)
                .Select(GetDirectoryInfo);
        }

        public IEnumerable<IDeviceFileInfo> EnumerateFiles(string path, SearchOption searchOption)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));

            logger.LogInformation("Getting all files from '{DeviceName}' '{Path}'. Search options: {SearchOption}.", FriendlyName, path, searchOption);
            return mediaDevice.EnumerateFiles(path, "*.*", searchOption)
                .Select(GetFileInfo);
        }

        public bool FileExists(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));

            return mediaDevice.FileExists(path);
        }

        private MediaDirectoryInfo GetMediaDirectoryInfo(string path)
        {
            logger.LogInformation("Getting directory info '{DeviceName}' '{Path}'.", FriendlyName, path);
            return mediaDevice.GetDirectoryInfo(path);
        }

        public IDeviceDirectoryInfo GetDirectoryInfo(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));

            return new DirectoryInfoWrap(path, GetMediaDirectoryInfo);
        }

        private MediaFileInfo GetMediaFileInfo(string path)
        {
            logger.LogInformation("Getting file info '{DeviceName}' '{Path}'.", FriendlyName, path);
            return mediaDevice.GetFileInfo(path);
        }

        public IDeviceFileInfo GetFileInfo(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));

            return new FileInfoWrap(path, GetMediaFileInfo);
        }

        public void Reconnect()
        {
            if (mediaDevice != null)
            {
                Dispose();
            }

            Connect();
        }
    }
}
