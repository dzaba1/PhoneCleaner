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
  <Copy Source=""Path1"" Destination=""Path2""></Copy>
  <Remove Path=""Path3""></Remove>
  <Remove Path=""Path4"" Content=""true"" ContentRecursive=""true""></Remove>
</Config>";

            var filepath = Path.Combine(TempPath, "config.xml");
            File.WriteAllText(filepath, xml);

            var sut = CreateSut();
            var result = sut.Read(filepath);

            result.Actions.Should().HaveCount(3);
            var copy = (Copy)result.Actions[0];
            var remove1 = (Remove)result.Actions[1];
            var remove2 = (Remove)result.Actions[2];

            copy.Destination.Should().Be("Path2");
            copy.Source.Should().Be("Path1");
            remove1.Path.Should().Be("Path3");
            remove1.Content.Should().BeFalse();
            remove1.ContentRecursive.Should().BeFalse();
            remove2.Path.Should().Be("Path4");
            remove2.Content.Should().BeTrue();
            remove2.ContentRecursive.Should().BeTrue();
        }
    }
}
