using Dzaba.PhoneCleaner.Lib.Device;
using Moq;
using System;

namespace Dzaba.PhoneCleaner.Lib.Tests
{
    internal static class TestUtils
    {
        public static T CreateSystemInfo<T>(string fullName = null, DateTime? modifiedTime = null)
            where T : class, IDeviceSystemInfo
        {
            var mock = new Mock<T>();
            mock.Setup(x => x.ModificationTime)
                .Returns(modifiedTime);
            mock.Setup(x => x.FullName)
                .Returns(fullName);
            return mock.Object;
        }
    }
}
