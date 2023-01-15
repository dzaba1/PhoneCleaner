using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Utils;
using System;

namespace Dzaba.PhoneCleaner.Lib.Tests.Integration.Device
{
    internal sealed class DeviceConnectionFactory : IDeviceConnectionFactory
    {
        private readonly Func<string> rootDirProvider;

        public DeviceConnectionFactory(Func<string> rootDirProvider)
        {
            Require.NotNull(rootDirProvider, nameof(rootDirProvider));

            this.rootDirProvider = rootDirProvider;
        }

        public IDeviceConnection Create(string deviceName, bool testOnly)
        {
            return new TempPathDevice(rootDirProvider());
        }
    }
}
