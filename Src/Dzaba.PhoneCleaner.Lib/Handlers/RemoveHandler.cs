using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Lib.Handlers.Options;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace Dzaba.PhoneCleaner.Lib.Handlers
{
    internal sealed class RemoveHandler : HandlerBase<Remove>
    {
        private readonly ILogger<RemoveHandler> logger;
        private readonly IOptionsEvaluator optionsEvaluator;

        public RemoveHandler(ILogger<RemoveHandler> logger,
            IOptionsEvaluator optionsEvaluator)
        {
            Require.NotNull(logger, nameof(logger));
            Require.NotNull(optionsEvaluator, nameof(optionsEvaluator));

            this.logger = logger;
            this.optionsEvaluator = optionsEvaluator;
        }

        protected override int Handle(Remove model, IDeviceConnection deviceConnection, CleanData cleanData)
        {
            Require.NotNull(model, nameof(model));
            Require.NotNull(deviceConnection, nameof(deviceConnection));
            Require.NotNull(cleanData, nameof(cleanData));

            var root = deviceConnection.GetRootOrThrow(cleanData.DriveIndex);
            var path = Path.Combine(root, model.Path);

            logger.LogInformation("Invoking the remove action for path '{Path}'. Recursive: {Recursive}",
                path, model.Recursive);

            if (!deviceConnection.DirectoryExists(path))
            {
                logger.LogInformation("The directory '{Path}' doesn't exist.", path);
                return 0;
            }

            var affected = 0;

            var files = deviceConnection.EnumerateFiles(path, SearchOption.TopDirectoryOnly)
                .Where(f => optionsEvaluator.IsOk(model.Options, deviceConnection, f))
                .ToArray();

            foreach (var file in files)
            {
                deviceConnection.DeleteFile(file.FullName);
                affected++;
            }

            if (model.Recursive)
            {
                var directories = deviceConnection.EnumerateDirectories(path, SearchOption.TopDirectoryOnly)
                    .Where(d => optionsEvaluator.IsOk(model.Options, deviceConnection, d))
                    .ToArray();

                foreach (var dir in directories)
                {
                    deviceConnection.DeleteDirectory(dir.FullName, true);
                    affected++;
                }
            }

            return affected;
        }
    }
}
