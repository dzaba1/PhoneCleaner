using AutoFixture;
using Dzaba.PhoneCleaner.Lib.Config.Options;
using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Lib.Handlers;
using Dzaba.PhoneCleaner.Lib.Handlers.Options;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Dzaba.PhoneCleaner.Lib.Tests.Handlers
{
    [TestFixture]
    public class OptionsEvaluatorTests
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = TestFixture.Create();
        }

        private OptionsEvaluator CreateSut()
        {
            return fixture.Create<OptionsEvaluator>();
        }

        [Test]
        public void IsOk_WhenModelNull_ThenItIsOk()
        {
            var sut = CreateSut();

            var result = sut.IsOk(null, Mock.Of<IDeviceConnection>(), Mock.Of<IDeviceSystemInfo>());
            result.Should().BeTrue();
        }

        private IEnumerable<Option> GetSomeOptions()
        {
            yield return new Option
            {
                ItemType = IOItemType.File
            };
            yield return new Option
            {
                ItemType = IOItemType.Directory
            };
            yield return new Option
            {
                ItemType = IOItemType.Both
            };
        }

        private IDeviceSystemInfo GetSystemInfo(bool directory)
        {
            if (directory)
            {
                return Mock.Of<IDeviceDirectoryInfo>();
            }

            return Mock.Of<IDeviceFileInfo>();
        }

        [Test]
        public void IsOk_WhenFile_ThenHandlersForFile()
        {
            var options = GetSomeOptions().ToArray();
            var device = Mock.Of<IDeviceConnection>();
            var fileInfo = GetSystemInfo(false);

            var handler = new Mock<IOptionHandler>();
            handler.Setup(x => x.IsOk(It.IsIn<Option>(options), device, fileInfo))
                .Returns(true);

            fixture.FreezeMock<IHandlerFactory>()
                .Setup(x => x.CreateOptionHandler(It.IsIn<Option>(options)))
                .Returns(handler.Object);

            var sut = CreateSut();

            var result = sut.IsOk(options, device, fileInfo);

            result.Should().BeTrue();
            handler.Verify(x => x.IsOk(options[0], device, fileInfo), Times.Once());
            handler.Verify(x => x.IsOk(options[1], device, fileInfo), Times.Never());
            handler.Verify(x => x.IsOk(options[2], device, fileInfo), Times.Once());
        }

        [Test]
        public void IsOk_WhenDirectory_ThenHandlersForDirectory()
        {
            var options = GetSomeOptions().ToArray();
            var device = Mock.Of<IDeviceConnection>();
            var dirInfo = GetSystemInfo(true);

            var handler = new Mock<IOptionHandler>();
            handler.Setup(x => x.IsOk(It.IsIn<Option>(options), device, dirInfo))
                .Returns(true);

            fixture.FreezeMock<IHandlerFactory>()
                .Setup(x => x.CreateOptionHandler(It.IsIn<Option>(options)))
                .Returns(handler.Object);

            var sut = CreateSut();

            var result = sut.IsOk(options, device, dirInfo);

            result.Should().BeTrue();
            handler.Verify(x => x.IsOk(options[0], device, dirInfo), Times.Never());
            handler.Verify(x => x.IsOk(options[1], device, dirInfo), Times.Once());
            handler.Verify(x => x.IsOk(options[2], device, dirInfo), Times.Once());
        }

        [Test]
        public void IsOk_WhenFirstHandlerIsNotOk_ThenStop()
        {
            var options = GetSomeOptions().ToArray();
            var device = Mock.Of<IDeviceConnection>();
            var fileInfo = GetSystemInfo(false);

            var handler = new Mock<IOptionHandler>();
            handler.Setup(x => x.IsOk(options[0], device, fileInfo))
                .Returns(false);
            handler.Setup(x => x.IsOk(options[1], device, fileInfo))
                .Returns(true);
            handler.Setup(x => x.IsOk(options[2], device, fileInfo))
                .Returns(true);

            fixture.FreezeMock<IHandlerFactory>()
                .Setup(x => x.CreateOptionHandler(It.IsIn<Option>(options)))
                .Returns(handler.Object);

            var sut = CreateSut();

            var result = sut.IsOk(options, device, fileInfo);

            result.Should().BeFalse();
            handler.Verify(x => x.IsOk(options[0], device, fileInfo), Times.Once());
            handler.Verify(x => x.IsOk(options[1], device, fileInfo), Times.Never());
            handler.Verify(x => x.IsOk(options[2], device, fileInfo), Times.Never());
        }
    }
}
