using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Lib.Handlers.Options;
using Microsoft.Extensions.Logging;
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
            Require.NotNull(logger, nameof(logger));
            Require.NotNull(optionsEvaluator, nameof(optionsEvaluator));

            this.logger = logger;
            this.optionsEvaluator = optionsEvaluator;
        }

        protected override int Handle(RemoveDirectory model, IDeviceConnection deviceConnection, CleanData cleanData)
        {
            Require.NotNull(model, nameof(model));
            Require.NotNull(deviceConnection, nameof(deviceConnection));
            Require.NotNull(cleanData, nameof(cleanData));

            var root = deviceConnection.GetRootOrThrow(cleanData.DriveIndex);
            var path = Path.Combine(root, model.Path);

            logger.LogInformation("Invoking the remove directory action for path '{Path}'.", path);

            if (!deviceConnection.DirectoryExists(path))
            {
                logger.LogInformation("The directory '{Path}' doesn't exist.", path);
                return 0;
            }

            if (!optionsEvaluator.IsOk(model.Options, deviceConnection, path, true))
            {
                logger.LogInformation("The directory '{Path}' doesn't match options.", path);
                return 0;
            }

            deviceConnection.DeleteDirectory(path, true);
            return 1;
        }
    }
}
