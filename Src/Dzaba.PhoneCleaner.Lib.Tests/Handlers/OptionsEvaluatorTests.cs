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

            var result = sut.IsOk(null, Mock.Of<IDeviceConnection>(), null, false);
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

        [Test]
        public void IsOk_WhenFile_ThenHandlersForFile()
        {
            var options = GetSomeOptions().ToArray();
            var device = Mock.Of<IDeviceConnection>();
            var path = "Path";
            var isDirectory = false;

            var handler = new Mock<IOptionHandler>();
            handler.Setup(x => x.IsOk(It.IsIn<Option>(options), device, path, isDirectory))
                .Returns(true);

            fixture.FreezeMock<IHandlerFactory>()
                .Setup(x => x.CreateOptionHandler(It.IsIn<Option>(options)))
                .Returns(handler.Object);

            var sut = CreateSut();

            var result = sut.IsOk(options, device, path, isDirectory);

            result.Should().BeTrue();
            handler.Verify(x => x.IsOk(options[0], device, path, isDirectory), Times.Once());
            handler.Verify(x => x.IsOk(options[1], device, path, isDirectory), Times.Never());
            handler.Verify(x => x.IsOk(options[2], device, path, isDirectory), Times.Once());
        }

        [Test]
        public void IsOk_WhenDirectory_ThenHandlersForDirectory()
        {
            var options = GetSomeOptions().ToArray();
            var device = Mock.Of<IDeviceConnection>();
            var path = "Path";
            var isDirectory = true;

            var handler = new Mock<IOptionHandler>();
            handler.Setup(x => x.IsOk(It.IsIn<Option>(options), device, path, isDirectory))
                .Returns(true);

            fixture.FreezeMock<IHandlerFactory>()
                .Setup(x => x.CreateOptionHandler(It.IsIn<Option>(options)))
                .Returns(handler.Object);

            var sut = CreateSut();

            var result = sut.IsOk(options, device, path, isDirectory);

            result.Should().BeTrue();
            handler.Verify(x => x.IsOk(options[0], device, path, isDirectory), Times.Never());
            handler.Verify(x => x.IsOk(options[1], device, path, isDirectory), Times.Once());
            handler.Verify(x => x.IsOk(options[2], device, path, isDirectory), Times.Once());
        }

        [Test]
        public void IsOk_WhenFirstHandlerIsNotOk_ThenStop()
        {
            var options = GetSomeOptions().ToArray();
            var device = Mock.Of<IDeviceConnection>();
            var path = "Path";
            var isDirectory = false;

            var handler = new Mock<IOptionHandler>();
            handler.Setup(x => x.IsOk(options[0], device, path, isDirectory))
                .Returns(false);
            handler.Setup(x => x.IsOk(options[1], device, path, isDirectory))
                .Returns(true);
            handler.Setup(x => x.IsOk(options[2], device, path, isDirectory))
                .Returns(true);

            fixture.FreezeMock<IHandlerFactory>()
                .Setup(x => x.CreateOptionHandler(It.IsIn<Option>(options)))
                .Returns(handler.Object);

            var sut = CreateSut();

            var result = sut.IsOk(options, device, path, isDirectory);

            result.Should().BeFalse();
            handler.Verify(x => x.IsOk(options[0], device, path, isDirectory), Times.Once());
            handler.Verify(x => x.IsOk(options[1], device, path, isDirectory), Times.Never());
            handler.Verify(x => x.IsOk(options[2], device, path, isDirectory), Times.Never());
        }
    }
}
