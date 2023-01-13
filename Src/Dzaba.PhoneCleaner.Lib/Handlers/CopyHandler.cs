using Dzaba.PhoneCleaner.Lib.Config;
using Microsoft.Extensions.Logging;
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

            logger.LogInformation("Invoking the copy action from '{Path}' to '{Destination}'. Recursive: {ContentRecursive}. Override: {Override}",
                path, fullDest, model.Recursive, model.Override);

            if (!deviceConnection.DirectoryExists(path))
            {
                logger.LogWarning("The directory '{Path}' doesn't exist.", path);
                return 0;
            }

            CopyDirectory(deviceConnection, path, fullDest, model.Recursive, model.Override);
            return 1;
        }

        private void CopyDirectory(IDeviceConnection deviceConnection,
            string sourceDir, string destinationDir,
            bool recursive, bool overwrite)
        {
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            foreach (var file in deviceConnection.EnumerateFiles(sourceDir, SearchOption.TopDirectoryOnly))
            {
                var fileName = DeviceIOUtils.GetFileOrDirectoryName(file);
                string targetFilePath = Path.Combine(destinationDir, fileName);

                if (overwrite || !File.Exists(targetFilePath))
                {
                    logger.LogInformation("Copy file '{Path}' to '{Destination}'.", file, targetFilePath);
                    deviceConnection.CopyFile(file, targetFilePath, overwrite);
                }
            }

            if (recursive)
            {
                foreach (var subDir in deviceConnection.EnumerateDirectories(sourceDir, SearchOption.TopDirectoryOnly))
                {
                    var subDirName = DeviceIOUtils.GetFileOrDirectoryName(subDir);
                    string newDestinationDir = Path.Combine(destinationDir, subDirName);
                    CopyDirectory(deviceConnection, subDir, newDestinationDir, true, overwrite);
                }
            }
        }
    }
}
