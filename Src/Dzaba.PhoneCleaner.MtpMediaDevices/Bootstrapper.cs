using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Dzaba.PhoneCleaner.MtpMediaDevices
{
    public static class Bootstrapper
    {
        public static void RegisterMtpMediaDevices(this IServiceCollection services)
        {
            Require.NotNull(services, nameof(services));

            services.AddTransient<IDeviceConnectionFactory, DeviceConnectionFactory>();
        }
    }
}
