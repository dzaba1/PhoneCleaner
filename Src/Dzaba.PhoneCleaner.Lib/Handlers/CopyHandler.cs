using Dzaba.PhoneCleaner.Lib.Config;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Handlers
{
    internal sealed class CopyHandler : HandlerBase<Copy>
    {
        private readonly ILogger<RemoveHandler> logger;

        public CopyHandler(ILogger<RemoveHandler> logger)
        {
            Require.NotNull(logger, nameof(logger));

            this.logger = logger;
        }

        protected override int Handle(Copy model, IDeviceConnection deviceConnection, CleanData cleanData)
        {
            Require.NotNull(model, nameof(model));
            Require.NotNull(deviceConnection, nameof(deviceConnection));
            Require.NotNull(cleanData, nameof(cleanData));

            var root = deviceConnection.GetRootOrThrow(cleanData.DriveIndex);
            var path = Path.Combine(root, model.Path);

            var fullDest = Path.Combine(cleanData.WorkingDir, model.Destination);

            logger.LogInformation("Invoking the copy action from '{Path}' to '{Destination}'. Recursive: {ContentRecursive}. On file conflict: {OnFileConflict}",
                path, fullDest, model.Recursive, model.OnConflict);

            if (!deviceConnection.DirectoryExists(path))
            {
                logger.LogWarning("The directory '{Path}' doesn't exist.", path);
                return 0;
            }

            return CopyDirectory(deviceConnection, path, fullDest, model.Recursive, model.OnConflict);
        }

        private int CopyDirectory(IDeviceConnection deviceConnection,
            string sourceDir, string destinationDir,
            bool recursive, OnFileConflict onFileConflict)
        {
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            var affected = 0;

            foreach (var file in deviceConnection.EnumerateFiles(sourceDir, SearchOption.TopDirectoryOnly))
            {
                var fileName = DeviceIOUtils.GetFileOrDirectoryName(file);
                string targetFilePath = Path.Combine(destinationDir, fileName);

                if (TryHandleFile(file, targetFilePath, deviceConnection, onFileConflict))
                {
                    affected++;
                }
            }

            if (recursive)
            {
                foreach (var subDir in deviceConnection.EnumerateDirectories(sourceDir, SearchOption.TopDirectoryOnly))
                {
                    var subDirName = DeviceIOUtils.GetFileOrDirectoryName(subDir);
                    string newDestinationDir = Path.Combine(destinationDir, subDirName);
                    var subDirAffected = CopyDirectory(deviceConnection, subDir, newDestinationDir, true, onFileConflict);
                    affected += subDirAffected;
                }
            }

            return affected;
        }

        private bool TryHandleFile(string file, string targetFilePath, IDeviceConnection deviceConnection, OnFileConflict onFileConflict)
        {
            var currentTargetFilePath = targetFilePath;
            var overwrite = false;

            if (File.Exists(currentTargetFilePath))
            {
                switch (onFileConflict)
                {
                    default: throw new ArgumentOutOfRangeException($"Unknown {nameof(OnFileConflict)} value: {onFileConflict}");
                    case OnFileConflict.RaiseError:
                        throw new IOException($"The file '{targetFilePath}' already exists.");
                    case OnFileConflict.DoNothing:
                        return false;
                    case OnFileConflict.Overwrite:
                        overwrite = true;
                        break;
                    case OnFileConflict.KeepBoth:
                        currentTargetFilePath = GetNewTargetFileName(currentTargetFilePath);
                        break;
                }
            }

            logger.LogInformation("Copy file '{Path}' to '{Destination}'.", file, targetFilePath);
            deviceConnection.CopyFile(file, targetFilePath, overwrite);
            return true;
        }

        private string GetNewTargetFileName(string targetFilePath)
        {
            var fileInfo = new FileInfo(targetFilePath);
            var nameWithoutExt = Path.GetFileNameWithoutExtension(targetFilePath);

            var i = 0;
            while (true)
            {
                i++;
                var newName = $"{nameWithoutExt}_{i}";
                var full = Path.Combine(fileInfo.DirectoryName, newName + fileInfo.Extension);
                if (!File.Exists(full))
                {
                    return full;
                }
            }
        }
    }
}
