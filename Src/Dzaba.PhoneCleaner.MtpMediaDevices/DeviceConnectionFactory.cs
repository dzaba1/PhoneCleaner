using Dzaba.PhoneCleaner.Lib.Device;
using Microsoft.Extensions.Logging;
using System;

namespace Dzaba.PhoneCleaner.MtpMediaDevices
{
    internal sealed class DeviceConnectionFactory : IDeviceConnectionFactory
    {
        private readonly ILoggerFactory loggerFactory;

        public DeviceConnectionFactory(ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(loggerFactory, nameof(loggerFactory));

            this.loggerFactory = loggerFactory;
        }

        public IDeviceConnection Create(string deviceName, bool testOnly)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(deviceName, nameof(deviceName));

            return new DeviceConnection(deviceName, loggerFactory.CreateLogger<DeviceConnection>(), testOnly);
        }
    }
}
