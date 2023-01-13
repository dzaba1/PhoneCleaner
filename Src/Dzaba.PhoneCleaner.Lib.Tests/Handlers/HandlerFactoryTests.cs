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
            var expected = CreateHandler<Model2>();
            SetupHandlers(CreateHandler<Model1>(), expected, CreateHandler<Model3>());

            var sut = CreateSut();

            var result = sut.CreateHandler(new Model2());
            result.Should().Be(expected);
        }

        [Test]
        public void CreateHandler_WhenHandlerIsNotRegistered_ThenError()
        {
            SetupHandlers(CreateHandler<Model1>(), CreateHandler<Model3>());

            var sut = CreateSut();

            this.Invoking(s => sut.CreateHandler(new Model2()))
                .Should().Throw<System.InvalidOperationException>();
        }

        private class Model1 : Action
        {

        }

        private class Model2 : Action
        {

        }

        private class Model3 : Action
        {

        }
    }
}
