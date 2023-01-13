using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests
{
    [TestFixture]
    public class DeviceExtensionsTests : TempFileTestFixture
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

        [Test]
        public void CopyFile_WhenFileExistAndOverwrite_ThenNewFile()
        {
            var source = "source";
            var expected = "OK";

            var dest = Path.Combine(TempPath, "test.txt");
            File.WriteAllText(dest, "invalid");

            var device = new Mock<IDeviceConnection>();
            device.Setup(x => x.CopyFile(source, It.IsNotNull<Stream>()))
                .Callback<string, Stream>((s, e) =>
                {
                    var writer = new StreamWriter(e);
                    writer.Write(expected);
                    writer.Flush();
                });

            device.Object.CopyFile(source, dest, true);
            File.ReadAllText(dest).Should().Be(expected);
        }

        [Test]
        public void CopyFile_WhenFileExistButNotOverwrite_ThenError()
        {
            var source = "source";
            var expected = "OK";

            var dest = Path.Combine(TempPath, "test.txt");
            File.WriteAllText(dest, "invalid");

            var device = new Mock<IDeviceConnection>();
            device.Setup(x => x.CopyFile(source, It.IsNotNull<Stream>()))
                .Callback<string, Stream>((s, e) =>
                {
                    var writer = new StreamWriter(e);
                    writer.Write(expected);
                    writer.Flush();
                });

            this.Invoking(s => device.Object.CopyFile(source, dest, false))
                .Should().Throw<IOException>();
        }
    }
}
