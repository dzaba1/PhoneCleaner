using Dzaba.PhoneCleaner.Lib.Config.Options;
using Dzaba.PhoneCleaner.Lib.Device;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Dzaba.PhoneCleaner.Lib.Handlers.Options
{
    public interface IOptionsEvaluator
    {
        bool IsOk(IEnumerable<Option> model, IDeviceConnection deviceConnection, IDeviceSystemInfo systemInfo);
    }

    internal sealed class OptionsEvaluator : IOptionsEvaluator
    {
        private readonly IHandlerFactory handlerFactory;
        private readonly ILogger<OptionsEvaluator> logger;

        public OptionsEvaluator(IHandlerFactory handlerFactory,
            ILogger<OptionsEvaluator> logger)
        {
            Require.NotNull(handlerFactory, nameof(handlerFactory));
            Require.NotNull(logger, nameof(logger));

            this.handlerFactory = handlerFactory;
            this.logger = logger;
        }

        public bool IsOk(IEnumerable<Option> model, IDeviceConnection deviceConnection, IDeviceSystemInfo systemInfo)
        {
            if (model == null)
            {
                return true;
            }

            var validOptions = model
                .Where(o => IsOptionValid(o, systemInfo));

            foreach (var option in validOptions)
            {
                var handler = handlerFactory.CreateOptionHandler(option);

                logger.LogDebug("Invoking '{OptionHander}' option handler for '{Path}'. System info type: {SystemInfoType}", handler.GetType(), systemInfo.FullName, systemInfo.GetType());

                if (!handler.IsOk(option, deviceConnection, systemInfo))
                {
                    logger.LogInformation("The path '{Path}' is not OK by '{Option}' option.", systemInfo.FullName, option.GetType());
                    return false;
                }
            }

            return true;
        }

        private bool IsOptionValid(Option option, IDeviceSystemInfo systemInfo)
        {
            if (systemInfo is IDeviceDirectoryInfo)
            {
                return option.ItemType.HasFlag(IOItemType.Directory);
            }

            return option.ItemType.HasFlag(IOItemType.File);
        }
    }
}
