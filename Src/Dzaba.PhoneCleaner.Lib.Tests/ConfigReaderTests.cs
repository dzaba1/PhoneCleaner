using AutoFixture;
using Dzaba.PhoneCleaner.Lib.Config;
using FluentAssertions;
using NUnit.Framework;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests
{

    [TestFixture]
    public class ConfigReaderTests : TempFileTestFixture
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = TestFixture.Create();
        }

        private ConfigReader CreateSut()
        {
            return fixture.Create<ConfigReader>();
        }

        [Test]
        public void Read_WhenXml_ThenConfig()
        {
            var xml = @"<Config>
  <Copy Path=""Path1"" Destination=""Path2""></Copy>
  <Copy Path=""Path1"" Destination=""Path2"" Content=""true"" ContentRecursive=""true""></Copy>
  <Remove Path=""Path3""></Remove>
  <Remove Path=""Path4"" Content=""true"" ContentRecursive=""true""></Remove>
</Config>";

            var filepath = Path.Combine(TempPath, "config.xml");
            File.WriteAllText(filepath, xml);

            var sut = CreateSut();
            var result = sut.Read(filepath);

            result.Actions.Should().HaveCount(4);
            var copy1 = (Copy)result.Actions[0];
            var copy2 = (Copy)result.Actions[1];
            var remove1 = (Remove)result.Actions[2];
            var remove2 = (Remove)result.Actions[3];

            AssertDirectoryAction(copy1, "Path1", false, false);
            copy1.Destination.Should().Be("Path2");

            AssertDirectoryAction(copy2, "Path1", true, true);
            copy2.Destination.Should().Be("Path2");

            AssertDirectoryAction(remove1, "Path3", false, false);
            AssertDirectoryAction(remove2, "Path4", true, true);
        }

        private void AssertDirectoryAction(DirectoryAction result, string path, bool content, bool contentRecursive)
        {
            result.Path.Should().Be(path);
            result.Content.Should().Be(content);
            result.ContentRecursive.Should().Be(contentRecursive);
        }
    }
}
