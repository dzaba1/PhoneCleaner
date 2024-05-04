using Dzaba.PhoneCleaner.Lib.Config.Options;
using Dzaba.PhoneCleaner.Lib.Device;
using System;

namespace Dzaba.PhoneCleaner.Lib.Handlers.Options
{
    internal sealed class SkipHandler : OptionHandlerBase<Skip>
    {
        private readonly IDateTimeProvider dateTimeProvider;

        public SkipHandler(IDateTimeProvider dateTimeProvider)
        {
            ArgumentNullException.ThrowIfNull(dateTimeProvider, nameof(dateTimeProvider));

            this.dateTimeProvider = dateTimeProvider;
        }

        protected override bool IsOk(Skip model, IDeviceConnection deviceConnection, IDeviceSystemInfo systemInfo)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            ArgumentNullException.ThrowIfNull(systemInfo, nameof(systemInfo));

            if (systemInfo.ModificationTime.HasValue)
            {
                if (model.NewerThan != null)
                {
                    var span = dateTimeProvider.Now() - systemInfo.ModificationTime.Value;
                    return span > model.NewerThan.Value;
                }

                if (model.OlderThan != null)
                {
                    var span = dateTimeProvider.Now() - systemInfo.ModificationTime.Value;
                    return span < model.OlderThan.Value;
                }
            }

            return true;
        }
    }
}
