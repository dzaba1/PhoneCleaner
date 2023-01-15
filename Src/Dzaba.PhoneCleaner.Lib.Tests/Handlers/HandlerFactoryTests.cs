using AutoFixture;
using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Handlers;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Dzaba.PhoneCleaner.Lib.Tests.Handlers
{
    [TestFixture]
    public class HandlerFactoryTests
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = TestFixture.Create();
        }

        private HandlerFactory CreateSut()
        {
            return fixture.Create<HandlerFactory>();
        }

        private void SetupHandlers(params IHandler[] handlers)
        {
            fixture.Inject<IEnumerable<IHandler>>(handlers);
        }

        private IHandler CreateHandler<T>() where T : Action
        {
            var handler = new Mock<IHandler>();
            handler.Setup(x => x.ModelType)
                .Returns(typeof(T));
            return handler.Object;
        }

        [Test]
        public void CreateHandler_WhenHandlerRegistered_ThenItIsReturned()
        {
            var expected = CreateHandler<Action2>();
            SetupHandlers(CreateHandler<Action1>(), expected, CreateHandler<Action3>());

            var sut = CreateSut();

            var result = sut.CreateHandler(new Action2());
            result.Should().Be(expected);
        }

        [Test]
        public void CreateHandler_WhenHandlerIsNotRegistered_ThenError()
        {
            SetupHandlers(CreateHandler<Action1>(), CreateHandler<Action3>());

            var sut = CreateSut();

            this.Invoking(s => sut.CreateHandler(new Action2()))
                .Should().Throw<System.InvalidOperationException>();
        }

        private class Action1 : Action
        {

        }

        private class Action2 : Action
        {

        }

        private class Action3 : Action
        {

        }
    }
}
