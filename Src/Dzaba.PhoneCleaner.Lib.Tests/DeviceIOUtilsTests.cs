using FluentAssertions;
using NUnit.Framework;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests
{
    [TestFixture]
    public class DeviceIOUtilsTests : TempFileTestFixture
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

        [Test]
        public void GetNewTargetFileName_WhenFileNameProvided_ThenItCreatesANewFilename()
        {
            var existingFile = Path.Combine(TempPath, "test.txt");
            File.WriteAllText(existingFile, "Test");

            var expected = Path.Combine(TempPath, "test_(1).txt");

            var result = DeviceIOUtils.GetNewTargetFileName(existingFile);

            result.Should().Be(expected);
        }
    }
}
