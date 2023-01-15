using Dzaba.PhoneCleaner.Lib;
using Dzaba.PhoneCleaner.Lib.Device;
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
