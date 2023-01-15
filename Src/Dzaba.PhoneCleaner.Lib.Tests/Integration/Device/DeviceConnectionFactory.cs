using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Utils;

namespace Dzaba.PhoneCleaner.Lib.Tests.Integration.Device
{
    internal sealed class DeviceConnectionFactory : IDeviceConnectionFactory
    {
        private readonly IRootDirProvider rootDirProvider;

        public DeviceConnectionFactory(IRootDirProvider rootDirProvider)
        {
            Require.NotNull(rootDirProvider, nameof(rootDirProvider));

            this.rootDirProvider = rootDirProvider;
        }

        public IDeviceConnection Create(string deviceName, bool testOnly)
        {
            return new TempPathDevice(rootDirProvider.GetRootDir());
        }
    }
}
