using AutoFixture.AutoMoq;
using AutoFixture;

namespace Dzaba.PhoneCleaner.Lib.Tests
{
    public static class TestFixture
    {
        public static IFixture Create()
        {
            return new Fixture().Customize(new AutoMoqCustomization());
        }
    }
}
