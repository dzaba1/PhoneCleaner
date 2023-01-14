using Dzaba.PhoneCleaner.Lib.Config.Options;
using Dzaba.PhoneCleaner.Lib.Device;
using Microsoft.Extensions.Logging;

namespace Dzaba.PhoneCleaner.Lib.Handlers.Options
{
    internal sealed class RegexHandler : OptionHandlerBase<Regex>
    {
        private readonly ILogger<RegexHandler> logger;

        public RegexHandler(ILogger<RegexHandler> logger)
        {
            Require.NotNull(logger, nameof(logger));

            this.logger = logger;
        }

        protected override bool IsOk(Regex model, IDeviceConnection deviceConnection, IDeviceSystemInfo systemInfo)
        {
            Require.NotNull(model, nameof(model));
            Require.NotNull(systemInfo, nameof(systemInfo));

            if (string.IsNullOrWhiteSpace(model.Pattern))
            {
                logger.LogWarning("The regex pattern is empty.");
                return true;
            }

            return model.RegexInstance.IsMatch(systemInfo.FullName);
        }
    }
}
