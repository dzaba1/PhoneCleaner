using Dzaba.PhoneCleaner.Lib.Device;
using Moq;
using System;

namespace Dzaba.PhoneCleaner.Lib.Tests
{
    internal static class TestUtils
    {
        public static IDeviceSystemInfo CreateSystemInfo(string fullName = null, DateTime? modifiedTime = null)
        {
            var mock = new Mock<IDeviceSystemInfo>();
            mock.Setup(x => x.ModificationTime)
                .Returns(modifiedTime);
            mock.Setup(x => x.FullName)
                .Returns(fullName);
            return mock.Object;
        }
    }
}
