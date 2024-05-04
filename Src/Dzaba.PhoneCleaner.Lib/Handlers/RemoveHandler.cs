using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Device;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Handlers
{
    internal sealed class RemoveHandler : HandlerBase<Remove>
    {
        private readonly ILogger<RemoveHandler> logger;
        private readonly IIOHelper ioHelper;

        public RemoveHandler(ILogger<RemoveHandler> logger,
            IIOHelper ioHelper)
        {
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            ArgumentNullException.ThrowIfNull(ioHelper, nameof(ioHelper));

            this.logger = logger;
            this.ioHelper = ioHelper;
        }

        protected override int Handle(Remove model, IDeviceConnection deviceConnection, CleanData cleanData)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            ArgumentNullException.ThrowIfNull(deviceConnection, nameof(deviceConnection));
            ArgumentNullException.ThrowIfNull(cleanData, nameof(cleanData));

            var root = deviceConnection.GetRootOrThrow(model.DriveIndex);
            var path = Path.Combine(root, model.Path);

            logger.LogInformation("Invoking the remove action for path '{Path}'. Recursive: {Recursive}",
                path, model.Recursive);

            if (!deviceConnection.DirectoryExists(path))
            {
                logger.LogInformation("The directory '{Path}' doesn't exist.", path);
                return 0;
            }

            var pathInfo = deviceConnection.GetDirectoryInfo(path);

            return RemoveDirectory(deviceConnection, pathInfo, model);
        }

        private int RemoveDirectory(IDeviceConnection deviceConnection,
            IDeviceDirectoryInfo dir, Remove model)
        {
            var affected = 0;

            var files = ioHelper.EnumerateFiles(deviceConnection, dir, model.Options);

            foreach (var file in files)
            {
                deviceConnection.DeleteFile(file.FullName);
                affected++;
            }

            if (model.Recursive)
            {
                var subDirs = ioHelper.EnumerateDirectories(deviceConnection, dir, model.Options);

                foreach (var subDir in subDirs)
                {
                    var subDirAffected = RemoveDirectory(deviceConnection, subDir, model);
                    affected += subDirAffected;

                    if (subDir.IsEmpty())
                    {
                        deviceConnection.DeleteDirectory(subDir.FullName, false);
                    }
                }
            }

            return affected;
        }
    }
}
