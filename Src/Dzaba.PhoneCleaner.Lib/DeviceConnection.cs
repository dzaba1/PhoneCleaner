using MediaDevices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dzaba.PhoneCleaner.Lib
{
    internal sealed class DeviceConnection : IDeviceConnection
    {
        private readonly MediaDevice mediaDevice;
        private readonly ILogger<DeviceConnection> logger;

        public DeviceConnection(string friendlyName,
            ILogger<DeviceConnection> logger)
        {
            Require.NotNull(logger, nameof(logger));

            this.logger = logger;

            mediaDevice = MediaDevice.GetDevices()
                .FirstOrDefault(x => x.FriendlyName == friendlyName);

            if (mediaDevice == null)
            {
                throw new InvalidOperationException($"Could't find any devices with name '{friendlyName}'.");
            }

            logger.LogInformation("Connecting to '{DeviceName}'", friendlyName);
            mediaDevice.Connect();
        }

        public string FriendlyName => mediaDevice.FriendlyName;

        public void CopyFile(string source, Stream dest)
        {
            Require.NotWhiteSpace(source, nameof(source));
            Require.NotNull(dest, nameof(dest));

            logger.LogInformation("Copy file from '{DeviceName}' '{Path}' to destination stream.", FriendlyName, source);
            mediaDevice.DownloadFile(source, dest);
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            Require.NotWhiteSpace(path, nameof(path));

            logger.LogInformation("Deleting directory at '{DeviceName}' '{Path}'. Recursive: {Recursive}.", FriendlyName, path, recursive);
            mediaDevice.DeleteDirectory(path, recursive);
        }

        public void DeleteFile(string path)
        {
            Require.NotWhiteSpace(path, nameof(path));

            logger.LogInformation("Deleting file at '{DeviceName}' '{Path}'.", FriendlyName, path);
            mediaDevice.DeleteFile(path);
        }

        public void Dispose()
        {
            logger.LogInformation("Disposing '{DeviceName}'", FriendlyName);
            mediaDevice?.Dispose();
        }

        public IEnumerable<string> EnumerateDirectories(string path, SearchOption searchOption)
        {
            Require.NotWhiteSpace(path, nameof(path));

            logger.LogInformation("Getting all directories from '{DeviceName}' '{Path}'. Search options: {SearchOption}.", FriendlyName, path, searchOption);
            return mediaDevice.EnumerateDirectories(path, "*", searchOption);
        }

        public IEnumerable<string> EnumerateFiles(string path, SearchOption searchOption)
        {
            Require.NotWhiteSpace(path, nameof(path));

            logger.LogInformation("Getting all files from '{DeviceName}' '{Path}'. Search options: {SearchOption}.", FriendlyName, path, searchOption);
            return mediaDevice.EnumerateFiles(path, "*.*", searchOption);
        }
    }
}
