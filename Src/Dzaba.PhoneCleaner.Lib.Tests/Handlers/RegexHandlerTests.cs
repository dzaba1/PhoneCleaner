using AutoFixture;
using Dzaba.PhoneCleaner.Lib.Handlers.Options;
using FluentAssertions;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace Dzaba.PhoneCleaner.Lib.Tests.Handlers
{
    [TestFixture]
    public class RegexHandlerTests
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = TestFixture.Create();
        }

        private RegexHandler CreateSut()
        {
            return fixture.Create<RegexHandler>();
        }

        [TestCase(@"C:\\Test\test.txt", @"test\.txt$", RegexOptions.IgnoreCase, true)]
        [TestCase(@"C:\\Test\test.txt", @"test\.jpg$", RegexOptions.IgnoreCase, false)]
        public void IsOk_WhenRegexProvided_ThenCorrectResult(string fullName, string pattern, RegexOptions regexOptions, bool expected)
        {
            var model = new Config.Options.Regex
            {
                Pattern = pattern,
                RegexOptions = regexOptions
            };

            var systemInfo = TestUtils.CreateSystemInfo(fullName);

            var sut = CreateSut();

            var result = sut.IsOk(model, null, systemInfo);
            result.Should().Be(expected);
        }
    }
}
