using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;

namespace Dzaba.PhoneCleaner.Lib.Tests
{
    [TestFixture]
    public class DeviceExtensionsTests
    {
        [Test]
        public void GetRootOrThrow_WhenDriveExists_ThenItIsReturned()
        {
            var expected = "Drive1";

            var device = new Mock<IDeviceConnection>();
            device.Setup(x => x.EnumerableDrives())
                .Returns(new[] { expected });

            var result = device.Object.GetRootOrThrow(0);
            result.Should().Be(expected);
        }

        [Test]
        public void GetRootOrThrow_WhenDriveDoesntExist_ThenError()
        {
            var device = new Mock<IDeviceConnection>();
            device.Setup(x => x.EnumerableDrives())
                .Returns(new[] { "Drive1" });

            this.Invoking(s => device.Object.GetRootOrThrow(1))
                .Should().Throw<InvalidOperationException>();
        }
    }
}
