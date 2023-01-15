using AutoFixture;
using Dzaba.PhoneCleaner.Lib.Config.Options;
using Dzaba.PhoneCleaner.Lib.Handlers.Options;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Dzaba.PhoneCleaner.Lib.Tests.Handlers
{
    [TestFixture]
    public class OptionHandlerFactoryTests
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = TestFixture.Create();
        }

        private OptionHandlerFactory CreateSut()
        {
            return fixture.Create<OptionHandlerFactory>();
        }

        private void SetupOptionHandlers(params IOptionHandler[] handlers)
        {
            fixture.Inject<IEnumerable<IOptionHandler>>(handlers);
        }

        private IOptionHandler CreateOptionHandler<T>() where T : Option
        {
            var handler = new Mock<IOptionHandler>();
            handler.Setup(x => x.ModelType)
                .Returns(typeof(T));
            return handler.Object;
        }

        [Test]
        public void CreateOptionHandler_WhenHandlerRegistered_ThenItIsReturned()
        {
            var expected = CreateOptionHandler<Option2>();
            SetupOptionHandlers(CreateOptionHandler<Option1>(), expected, CreateOptionHandler<Option3>());

            var sut = CreateSut();

            var result = sut.CreateOptionHandler(new Option2());
            result.Should().Be(expected);
        }

        [Test]
        public void CreateOptionHandler_WhenHandlerIsNotRegistered_ThenError()
        {
            SetupOptionHandlers(CreateOptionHandler<Option1>(), CreateOptionHandler<Option3>());

            var sut = CreateSut();

            this.Invoking(s => sut.CreateOptionHandler(new Option2()))
                .Should().Throw<System.InvalidOperationException>();
        }

        private class Option1 : Option
        {

        }

        private class Option2 : Option
        {

        }

        private class Option3 : Option
        {

        }
    }
}
