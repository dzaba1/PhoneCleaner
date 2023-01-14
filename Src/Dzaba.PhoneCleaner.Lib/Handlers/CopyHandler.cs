using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Lib.Handlers.Options;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace Dzaba.PhoneCleaner.Lib.Handlers
{
    internal sealed class CopyHandler : HandlerBase<Copy>
    {
        private readonly ILogger<CopyHandler> logger;
        private readonly IOptionsEvaluator optionsEvaluator;

        public CopyHandler(ILogger<CopyHandler> logger,
            IOptionsEvaluator optionsEvaluator)
        {
            Require.NotNull(logger, nameof(logger));
            Require.NotNull(optionsEvaluator, nameof(optionsEvaluator));

            this.logger = logger;
            this.optionsEvaluator = optionsEvaluator;
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

            var pathInfo = deviceConnection.GetDirectoryInfo(path);

            return CopyDirectory(deviceConnection, pathInfo, fullDest, model);
        }

        private int CopyDirectory(IDeviceConnection deviceConnection,
            IDeviceDirectoryInfo sourceDir, string destinationDir,
            Copy model)
        {
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            var affected = 0;

            var files = deviceConnection.EnumerateFiles(sourceDir.FullName, SearchOption.TopDirectoryOnly)
                .Where(f => optionsEvaluator.IsOk(model.Options, deviceConnection, f))
                .ToArray();

            foreach (var file in files)
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);

                if (TryHandleFile(file, targetFilePath, deviceConnection, model.OnConflict))
                {
                    affected++;
                }
            }

            if (model.Recursive)
            {
                var subDirs = deviceConnection.EnumerateDirectories(sourceDir.FullName, SearchOption.TopDirectoryOnly)
                    .Where(d => optionsEvaluator.IsOk(model.Options, deviceConnection, d))
                    .ToArray();

                foreach (var subDir in subDirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    var subDirAffected = CopyDirectory(deviceConnection, subDir, newDestinationDir, model);
                    affected += subDirAffected;
                }
            }

            return affected;
        }

        private bool TryHandleFile(IDeviceFileInfo file, string targetFilePath, IDeviceConnection deviceConnection, OnFileConflict onFileConflict)
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
                        currentTargetFilePath = DeviceIOUtils.GetNewTargetFileName(currentTargetFilePath);
                        break;
                }
            }

            logger.LogInformation("Copy file '{Path}' to '{Destination}'.", file, targetFilePath);
            deviceConnection.CopyFile(file.FullName, targetFilePath, overwrite);
            return true;
        }
    }
}
