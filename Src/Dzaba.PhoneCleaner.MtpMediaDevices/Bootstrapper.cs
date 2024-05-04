using Dzaba.PhoneCleaner.Lib.Device;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dzaba.PhoneCleaner.MtpMediaDevices
{
    public static class Bootstrapper
    {
        public static void RegisterMtpMediaDevices(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            services.AddTransient<IDeviceConnectionFactory, DeviceConnectionFactory>();
        }
    }
}
