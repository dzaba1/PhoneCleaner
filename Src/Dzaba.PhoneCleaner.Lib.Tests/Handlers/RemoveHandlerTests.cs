using AutoFixture;
using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Lib.Handlers;
using Dzaba.PhoneCleaner.Lib.Handlers.Options;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests.Handlers
{
    [TestFixture]
    public class RemoveHandlerTests : HandlerTestFixture
    {
        private RemoveHandler CreateSut()
        {
            return Fixture.Create<RemoveHandler>();
        }

        [Test]
        public void Handle_WhenDirectoryDoesntExists_ThenNothing()
        {
            var model = new Remove()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Recursive = false
            };

            var device = GetTempPathDevice();

            var sut = CreateSut();

            var result = sut.Handle(model, device, GetCleanData());

            result.Should().Be(0);
        }

        [Test]
        public void Handle_WhenRecursiveFlagIsSpecified_ThenDirectoryIsEmpty()
        {
            var model = new Remove()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Recursive = true
            };

            var device = GetTempPathDevice();
            SetupSomeDeviceFiles();

            Fixture.FreezeMock<IOptionsEvaluator>()
                .Setup(x => x.IsOk(null, device, It.IsAny<IDeviceSystemInfo>()))
                .Returns(true);

            var sut = CreateSut();

            var result = sut.Handle(model, device, GetCleanData());

            result.Should().Be(4);
            Directory.Exists(model.Path).Should().BeTrue();
            Directory.EnumerateFiles(model.Path).Should().BeEmpty();
            Directory.EnumerateDirectories(model.Path).Should().BeEmpty();
        }

        [Test]
        public void Handle_WhenNotRecursive_ThenDirectoryDoesntHaveFiles()
        {
            var model = new Remove()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Recursive = false
            };

            var device = GetTempPathDevice();
            SetupSomeDeviceFiles();

            Fixture.FreezeMock<IOptionsEvaluator>()
                .Setup(x => x.IsOk(null, device, It.IsAny<IDeviceSystemInfo>()))
                .Returns(true);

            var sut = CreateSut();

            var result = sut.Handle(model, device, GetCleanData());

            result.Should().Be(2);
            Directory.Exists(model.Path).Should().BeTrue();
            Directory.EnumerateFiles(model.Path).Should().BeEmpty();
            Directory.EnumerateDirectories(model.Path).Should().HaveCount(2);
        }

        [Test]
        public void Handle_WhenNotOk_ThenNothing()
        {
            var model = new Remove()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Recursive = false
            };

            var device = GetTempPathDevice();
            SetupSomeDeviceFiles();

            Fixture.FreezeMock<IOptionsEvaluator>()
                .Setup(x => x.IsOk(null, device, It.IsAny<IDeviceSystemInfo>()))
                .Returns(false);

            var sut = CreateSut();

            var result = sut.Handle(model, device, GetCleanData());

            result.Should().Be(0);
            Directory.Exists(model.Path).Should().BeTrue();
            Directory.EnumerateFiles(model.Path).Should().HaveCount(2);
            Directory.EnumerateDirectories(model.Path).Should().HaveCount(2);
        }
    }
}
