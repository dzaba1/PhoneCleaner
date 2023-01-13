using Microsoft.Extensions.Logging;

namespace Dzaba.PhoneCleaner.Lib
{
    public interface IDeviceConnectionFactory
    {
        IDeviceConnection Create(string deviceName);
    }

    internal sealed class DeviceConnectionFactory : IDeviceConnectionFactory
    {
        private readonly ILoggerFactory loggerFactory;

        public DeviceConnectionFactory(ILoggerFactory loggerFactory)
        {
            Require.NotNull(loggerFactory, nameof(loggerFactory));

            this.loggerFactory = loggerFactory;
        }

        public IDeviceConnection Create(string deviceName)
        {
            Require.NotWhiteSpace(deviceName, nameof(deviceName));

            return new DeviceConnection(deviceName, loggerFactory.CreateLogger<DeviceConnection>());
        }
    }
}
