using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Device;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Handlers
{
    internal sealed class CopyHandler : HandlerBase<Copy>
    {
        private readonly ILogger<CopyHandler> logger;
        private readonly IIOHelper ioHelper;

        public CopyHandler(ILogger<CopyHandler> logger,
            IIOHelper ioHelper)
        {
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            ArgumentNullException.ThrowIfNull(ioHelper, nameof(ioHelper));

            this.logger = logger;
            this.ioHelper = ioHelper;
        }

        protected override int Handle(Copy model, IDeviceConnection deviceConnection, CleanData cleanData)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            ArgumentNullException.ThrowIfNull(deviceConnection, nameof(deviceConnection));
            ArgumentNullException.ThrowIfNull(cleanData, nameof(cleanData));

            var root = deviceConnection.GetRootOrThrow(model.DriveIndex);
            var path = Path.Combine(root, model.Path);

            var fullDest = Path.Combine(cleanData.WorkingDir, model.Destination);

            logger.LogInformation("Invoking the copy action from '{Path}' to '{Destination}'. Recursive: {Recursive}. On file conflict: {OnFileConflict}",
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

            var files = ioHelper.EnumerateFiles(deviceConnection, sourceDir, model.Options);

            foreach (var file in files)
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);

                if (ioHelper.TryCopyFile(file, targetFilePath, deviceConnection, model.OnConflict))
                {
                    affected++;
                }
            }

            if (model.Recursive)
            {
                var subDirs = ioHelper.EnumerateDirectories(deviceConnection, sourceDir, model.Options);

                foreach (var subDir in subDirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    var subDirAffected = CopyDirectory(deviceConnection, subDir, newDestinationDir, model);
                    affected += subDirAffected;
                }
            }

            return affected;
        }
    }
}
