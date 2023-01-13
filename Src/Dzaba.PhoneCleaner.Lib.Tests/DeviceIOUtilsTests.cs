using FluentAssertions;
using NUnit.Framework;

namespace Dzaba.PhoneCleaner.Lib.Tests
{
    [TestFixture]
    public class DeviceIOUtilsTests
    {
        [TestCase(@"\\Phone\Dir", "Dir")]
        [TestCase(@"\\Phone\Dir\", "Dir")]
        [TestCase(@"\\Phone\", "Phone")]
        [TestCase(@"\\Phone", "Phone")]
        [TestCase(@"Phone", "Phone")]
        [TestCase(@"Phone\", "Phone")]
        [TestCase(@"\\Phone\Dir\test.txt", "test.txt")]
        [TestCase(@"Phone\test.txt", "test.txt")]
        public void GetFileOrDirectoryName_WhenPathProvided_ThenCorrectResult(string path, string expected)
        {
            var result = DeviceIOUtils.GetFileOrDirectoryName(path);
            result.Should().Be(expected);
        }
    }
}
