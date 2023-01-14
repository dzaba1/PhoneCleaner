using Dzaba.PhoneCleaner.Lib;
using Dzaba.PhoneCleaner.Lib.Device;
using Microsoft.Extensions.Logging;

namespace Dzaba.PhoneCleaner.MtpMediaDevices
{
    internal sealed class DeviceConnectionFactory : IDeviceConnectionFactory
    {
        private readonly ILoggerFactory loggerFactory;

        public DeviceConnectionFactory(ILoggerFactory loggerFactory)
        {
            Require.NotNull(loggerFactory, nameof(loggerFactory));

            this.loggerFactory = loggerFactory;
        }

        public IDeviceConnection Create(string deviceName, bool testOnly)
        {
            Require.NotWhiteSpace(deviceName, nameof(deviceName));

            return new DeviceConnection(deviceName, loggerFactory.CreateLogger<DeviceConnection>(), testOnly);
        }
    }
}
