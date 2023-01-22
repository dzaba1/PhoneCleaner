using Dzaba.PhoneCleaner.Lib.Config.Options;
using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Utils;
using Microsoft.Extensions.Logging;

namespace Dzaba.PhoneCleaner.Lib.Handlers.Options
{
    internal sealed class TakeHandler : OptionHandlerBase<Take>
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ILogger<TakeHandler> logger;

        public TakeHandler(IDateTimeProvider dateTimeProvider,
            ILogger<TakeHandler> logger)
        {
            Require.NotNull(dateTimeProvider, nameof(dateTimeProvider));
            Require.NotNull(logger, nameof(logger));

            this.dateTimeProvider = dateTimeProvider;
            this.logger = logger;
        }

        protected override bool IsOk(Take model, IDeviceConnection deviceConnection, IDeviceSystemInfo systemInfo)
        {
            Require.NotNull(model, nameof(model));
            Require.NotNull(systemInfo, nameof(systemInfo));

            if (systemInfo.ModificationTime.HasValue)
            {
                if (model.NewerThan != null)
                {
                    var span = dateTimeProvider.Now() - systemInfo.ModificationTime.Value;
                    var result = span < model.NewerThan.Value;
                    if (!result)
                    {
                        logger.LogDebug("Modification time: {ModificationTime}. Newer than: {NewerThan}.", systemInfo.ModificationTime, model.NewerThan);
                    }
                    return result;
                }

                if (model.OlderThan != null)
                {
                    var span = dateTimeProvider.Now() - systemInfo.ModificationTime.Value;
                    var result = span > model.OlderThan.Value;
                    if (!result)
                    {
                        logger.LogDebug("Modification time: {ModificationTime}. Older than: {OlderThan}.", systemInfo.ModificationTime, model.OlderThan);
                    }
                    return result;
                }
            }

            return true;
        }
    }
}
