using Dzaba.PhoneCleaner.Lib.Config.Options;
using Dzaba.PhoneCleaner.Lib.Device;

namespace Dzaba.PhoneCleaner.Lib.Handlers.Options
{
    internal sealed class SkipHandler : OptionHandlerBase<Skip>
    {
        private readonly IDateTimeProvider dateTimeProvider;

        public SkipHandler(IDateTimeProvider dateTimeProvider)
        {
            Require.NotNull(dateTimeProvider, nameof(dateTimeProvider));

            this.dateTimeProvider = dateTimeProvider;
        }

        protected override bool IsOk(Skip model, IDeviceConnection deviceConnection, IDeviceSystemInfo systemInfo)
        {
            Require.NotNull(model, nameof(model));
            Require.NotNull(systemInfo, nameof(systemInfo));

            if (systemInfo.ModificationTime.HasValue)
            {
                if (model.NewerThan != null)
                {
                    var span = dateTimeProvider.Now() - systemInfo.ModificationTime.Value;
                    return span < model.NewerThan.Value;
                }
            }

            return true;
        }
    }
}
