using AutoFixture;
using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Device;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests
{
    [TestFixture]
    public class IOHelperTests : TempFileTestFixture
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = TestFixture.Create();
        }

        private IOHelper CreateSut()
        {
            return fixture.Create<IOHelper>();
        }

        private string GetTargetFileName(bool shouldBeCreated)
        {
            var target = Path.Combine(TempPath, "test.txt");
            if (shouldBeCreated)
            {
                File.WriteAllText(target, "test");
            }
            return target;
        }

        private void AssertCopy(Mock<IDeviceConnection> device, string source, string dest)
        {
            device.Verify(x => x.CopyFile(source, It.Is<Stream>(s => ((FileStream)s).Name == dest)), Times.Once());
        }

        [Test]
        public void TryCopyFile_WhenRaiseErrorAndTargetDoesnExist_ThenCopy()
        {
            var source = "Path";
            var target = GetTargetFileName(false);
            var fileInfo = TestUtils.CreateSystemInfo<IDeviceFileInfo>(source);
            var device = new Mock<IDeviceConnection>();

            var sut = CreateSut();

            var result = sut.TryCopyFile(fileInfo, target, device.Object, OnFileConflict.RaiseError);

            result.Should().BeTrue();
            AssertCopy(device, source, target);
        }

        [Test]
        public void TryCopyFile_WhenRaiseErrorAndTargetExists_ThenError()
        {
            var source = "Path";
            var target = GetTargetFileName(true);
            var fileInfo = TestUtils.CreateSystemInfo<IDeviceFileInfo>(source);
            var device = new Mock<IDeviceConnection>();

            var sut = CreateSut();

            this.Invoking(s => sut.TryCopyFile(fileInfo, target, device.Object, OnFileConflict.RaiseError))
                .Should().Throw<IOException>();
        }

        [Test]
        public void TryCopyFile_WhenDoNothingAndTargetDoesnExist_ThenCopy()
        {
            var source = "Path";
            var target = GetTargetFileName(false);
            var fileInfo = TestUtils.CreateSystemInfo<IDeviceFileInfo>(source);
            var device = new Mock<IDeviceConnection>();

            var sut = CreateSut();

            var result = sut.TryCopyFile(fileInfo, target, device.Object, OnFileConflict.DoNothing);

            result.Should().BeTrue();
            AssertCopy(device, source, target);
        }

        [Test]
        public void TryCopyFile_WhenDoNothingAndTargetExist_ThenDontCopy()
        {
            var source = "Path";
            var target = GetTargetFileName(true);
            var fileInfo = TestUtils.CreateSystemInfo<IDeviceFileInfo>(source);
            var device = new Mock<IDeviceConnection>();

            var sut = CreateSut();

            var result = sut.TryCopyFile(fileInfo, target, device.Object, OnFileConflict.DoNothing);

            result.Should().BeFalse();
            device.Verify(x => x.CopyFile(It.IsAny<string>(), It.IsAny<Stream>()), Times.Never());
        }

        [Test]
        [Combinatorial]
        public void TryCopyFile_WhenOverwrite_ThenCopyAlways([Values] bool shouldTargetExist)
        {
            var source = "Path";
            var target = GetTargetFileName(shouldTargetExist);
            var fileInfo = TestUtils.CreateSystemInfo<IDeviceFileInfo>(source);
            var device = new Mock<IDeviceConnection>();

            var sut = CreateSut();

            var result = sut.TryCopyFile(fileInfo, target, device.Object, OnFileConflict.Overwrite);

            result.Should().BeTrue();
            AssertCopy(device, source, target);
        }

        [Test]
        public void TryCopyFile_WhenKeepBothAndTargetDoesnExist_ThenCopy()
        {
            var source = "Path";
            var target = GetTargetFileName(false);
            var fileInfo = TestUtils.CreateSystemInfo<IDeviceFileInfo>(source);
            var device = new Mock<IDeviceConnection>();

            var sut = CreateSut();

            var result = sut.TryCopyFile(fileInfo, target, device.Object, OnFileConflict.KeepBoth);

            result.Should().BeTrue();
            AssertCopy(device, source, target);
        }

        [Test]
        public void TryCopyFile_WhenKeepBothAndTargetExists_ThenCopyNewFile()
        {
            var source = "Path";
            var target = GetTargetFileName(true);
            var fileInfo = TestUtils.CreateSystemInfo<IDeviceFileInfo>(source);
            var device = new Mock<IDeviceConnection>();
            var expected = Path.Combine(TempPath, "test_(1).txt");

            var sut = CreateSut();

            var result = sut.TryCopyFile(fileInfo, target, device.Object, OnFileConflict.KeepBoth);

            result.Should().BeTrue();
            AssertCopy(device, source, expected);
        }
    }
}
