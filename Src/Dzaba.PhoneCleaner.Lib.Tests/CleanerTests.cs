using AutoFixture;
using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Lib.Handlers;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Dzaba.PhoneCleaner.Lib.Tests
{
    [TestFixture]
    public class CleanerTests
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = TestFixture.Create();
        }

        private Cleaner CreateSut()
        {
            return fixture.Create<Cleaner>();
        }

        private Mock<IDeviceConnection> SetupDevice(string deviceName)
        {
            var device = new Mock<IDeviceConnection>();
            fixture.FreezeMock<IDeviceConnectionFactory>()
                .Setup(x => x.Create(deviceName, false))
                .Returns(device.Object);
            return device;
        }

        private IHandler CreateHandler(Action model, IDeviceConnection deviceConnection, CleanData cleanData, int affected)
        {
            var mock = new Mock<IHandler>();
            mock.Setup(x => x.ModelType)
                .Returns(model.GetType());
            mock.Setup(x => x.Handle(model, deviceConnection, cleanData))
                .Returns(affected);
            return mock.Object;
        }

        [Test]
        public void Clean_WhenHandlerAndConfig_ThenCorrectAffectedSum()
        {
            var cleanData = new CleanData
            {
                ConfigFilepath = "config",
                DeviceName = "Device"
            };
            var action1 = new Model1();
            var action2 = new Model2();
            var config = new Config.Config
            {
                Actions = new Action[]
                {
                    action1,
                    action2
                }
            };

            fixture.FreezeMock<IConfigReader>()
                .Setup(x => x.Read(cleanData.ConfigFilepath))
                .Returns(config);

            var device = SetupDevice(cleanData.DeviceName);

            var handlerFactory = fixture.FreezeMock<IHandlerFactory>();
            var handler1 = CreateHandler(action1, device.Object, cleanData, 2);
            var handler2 = CreateHandler(action2, device.Object, cleanData, 2);

            handlerFactory.Setup(x => x.CreateHandler(action1))
                .Returns(handler1);
            handlerFactory.Setup(x => x.CreateHandler(action2))
                .Returns(handler2);

            var sut = CreateSut();

            var result = sut.Clean(cleanData);

            result.Should().Be(4);
        }

        [Test]
        public void Clean_WhenOneHandlerFailed_ThenProgramSurvives()
        {
            var cleanData = new CleanData
            {
                ConfigFilepath = "config",
                DeviceName = "Device"
            };
            var action1 = new Model1();
            var action2 = new Model2();
            var config = new Config.Config
            {
                Actions = new Action[]
                {
                    action1,
                    action2
                }
            };

            fixture.FreezeMock<IConfigReader>()
                .Setup(x => x.Read(cleanData.ConfigFilepath))
                .Returns(config);

            var device = SetupDevice(cleanData.DeviceName);

            var handlerFactory = fixture.FreezeMock<IHandlerFactory>();
            var handler1 = CreateHandler(action1, device.Object, cleanData, 2);

            var handler2 = new Mock<IHandler>();
            handler2.Setup(x => x.ModelType)
                .Returns(action2.GetType());
            handler2.Setup(x => x.Handle(It.IsAny<Action>(), It.IsAny<IDeviceConnection>(), It.IsAny<CleanData>()))
                .Throws(new System.Exception("Test"));

            handlerFactory.Setup(x => x.CreateHandler(action1))
                .Returns(handler1);
            handlerFactory.Setup(x => x.CreateHandler(action2))
                .Returns(handler2.Object);

            var sut = CreateSut();

            var result = sut.Clean(cleanData);

            result.Should().Be(2);
        }

        private class Model1 : Action
        {

        }

        private class Model2 : Action
        {

        }
    }
}
