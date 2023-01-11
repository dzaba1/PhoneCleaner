using AutoFixture;
using Moq;

namespace Dzaba.PhoneCleaner.Lib.Tests
{
    public static class Extensions
    {
        public static Mock<T> FreezeMock<T>(this IFixture fixture)
                    where T : class
        {
            Require.NotNull(fixture, nameof(fixture));

            return fixture.Freeze<Mock<T>>();
        }
    }
}
