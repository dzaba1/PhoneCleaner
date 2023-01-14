using AutoFixture;
using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Handlers;
using Dzaba.PhoneCleaner.Lib.Handlers.Options;
using FluentAssertions;
using NUnit.Framework;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests.Handlers
{
    [TestFixture]
    public class RemoveDirectoryHandlerTests : HandlerTestFixture
    {
        private RemoveDirectoryHandler CreateSut()
        {
            return Fixture.Create<RemoveDirectoryHandler>();
        }

        [Test]
        public void Handle_WhenOk_ThenDirectoryIsDeleted()
        {
            var model = new RemoveDirectory()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1")
            };

            var device = GetTempPathDevice();
            SetupSomeDeviceFiles();

            Fixture.FreezeMock<IOptionsEvaluator>()
                .Setup(x => x.IsOk(null, device, model.Path, true))
                .Returns(true);

            var sut = CreateSut();

            var result = sut.Handle(model, device, GetCleanData());

            result.Should().Be(1);
            Directory.Exists(model.Path).Should().BeFalse();
        }

        [Test]
        public void Handle_WhenNotOk_ThenDirectoryIsNotDeleted()
        {
            var model = new RemoveDirectory()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1")
            };

            var device = GetTempPathDevice();
            SetupSomeDeviceFiles();

            Fixture.FreezeMock<IOptionsEvaluator>()
                .Setup(x => x.IsOk(null, device, model.Path, true))
                .Returns(false);

            var sut = CreateSut();

            var result = sut.Handle(model, device, GetCleanData());

            result.Should().Be(0);
            Directory.Exists(model.Path).Should().BeTrue();
        }

        [Test]
        public void Handle_WhenDirectoryDoesntExist_ThenNothing()
        {
            var model = new RemoveDirectory()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1")
            };

            var device = GetTempPathDevice();

            Fixture.FreezeMock<IOptionsEvaluator>()
                .Setup(x => x.IsOk(null, device, model.Path, true))
                .Returns(true);

            var sut = CreateSut();

            var result = sut.Handle(model, device, GetCleanData());

            result.Should().Be(0);
        }
    }
}
