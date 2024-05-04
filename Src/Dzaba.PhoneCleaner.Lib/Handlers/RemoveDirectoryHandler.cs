using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Lib.Handlers.Options;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Handlers
{
    internal sealed class RemoveDirectoryHandler : HandlerBase<RemoveDirectory>
    {
        private readonly ILogger<RemoveDirectoryHandler> logger;
        private readonly IOptionsEvaluator optionsEvaluator;

        public RemoveDirectoryHandler(ILogger<RemoveDirectoryHandler> logger,
            IOptionsEvaluator optionsEvaluator)
        {
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            ArgumentNullException.ThrowIfNull(optionsEvaluator, nameof(optionsEvaluator));

            this.logger = logger;
            this.optionsEvaluator = optionsEvaluator;
        }

        protected override int Handle(RemoveDirectory model, IDeviceConnection deviceConnection, CleanData cleanData)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            ArgumentNullException.ThrowIfNull(deviceConnection, nameof(deviceConnection));
            ArgumentNullException.ThrowIfNull(cleanData, nameof(cleanData));

            var root = deviceConnection.GetRootOrThrow(model.DriveIndex);
            var path = Path.Combine(root, model.Path);

            logger.LogInformation("Invoking the remove directory action for path '{Path}'.", path);

            if (!deviceConnection.DirectoryExists(path))
            {
                logger.LogInformation("The directory '{Path}' doesn't exist.", path);
                return 0;
            }

            var pathInfo = deviceConnection.GetDirectoryInfo(path);

            if (!optionsEvaluator.IsOk(model.Options, deviceConnection, pathInfo))
            {
                logger.LogInformation("The directory '{Path}' doesn't match options.", path);
                return 0;
            }

            deviceConnection.DeleteDirectory(path, true);
            return 1;
        }
    }
}
