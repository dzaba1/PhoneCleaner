using AutoFixture;
using Moq;
using System;

namespace Dzaba.PhoneCleaner.Lib.Tests
{
    public static class Extensions
    {
        public static Mock<T> FreezeMock<T>(this IFixture fixture)
                    where T : class
        {
            ArgumentNullException.ThrowIfNull(fixture, nameof(fixture));

            return fixture.Freeze<Mock<T>>();
        }
    }
}
