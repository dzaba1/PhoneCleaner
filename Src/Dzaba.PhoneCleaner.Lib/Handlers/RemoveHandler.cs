using Dzaba.PhoneCleaner.Lib.Config;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;

namespace Dzaba.PhoneCleaner.Lib.Handlers
{
    internal sealed class RemoveHandler : HandlerBase<Remove>
    {
        private readonly ILogger<RemoveHandler> logger;

        public RemoveHandler(ILogger<RemoveHandler> logger)
        {
            Require.NotNull(logger, nameof(logger));

            this.logger = logger;
        }

        protected override int Handle(Remove model, IDeviceConnection deviceConnection, CleanData cleanData)
        {
            Require.NotNull(model, nameof(model));
            Require.NotNull(deviceConnection, nameof(deviceConnection));
            Require.NotNull(cleanData, nameof(cleanData));

            var root = deviceConnection.GetRootOrThrow(cleanData.DriveIndex);
            var path = Path.Combine(root, model.Path);

            logger.LogInformation("Invoking the remove action for path '{Path}'. Content flag: {Content}. Recursive: {Recursive}",
                path, model.Content, model.Recursive);

            if (model.Content)
            {
                var affected = 0;

                var files = deviceConnection.EnumerateFiles(path, SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    deviceConnection.DeleteFile(file);
                    affected++;
                }

                var directories = deviceConnection.EnumerateDirectories(path, SearchOption.TopDirectoryOnly);
                foreach (var dir in directories)
                {
                    deviceConnection.DeleteDirectory(dir, model.Recursive);
                    affected++;
                }

                return affected;
            }
            else
            {
                deviceConnection.DeleteDirectory(path, model.Recursive);
                return 1;
            }
        }
    }
}
