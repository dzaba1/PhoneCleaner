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
</Config>";

            var filepath = Path.Combine(TempPath, "config.xml");
            File.WriteAllText(filepath, xml);

            var sut = CreateSut();
            var result = sut.Read(filepath);

            result.Actions.Should().HaveCount(2);
            var copy = (Copy)result.Actions[0];
            var remove = (Remove)result.Actions[1];

            copy.Destination.Should().Be("Path2");
            copy.Source.Should().Be("Path1");
            remove.Path.Should().Be("Path3");
        }
    }
}
