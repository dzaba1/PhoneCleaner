using AutoFixture;
using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Config.Options;
using FluentAssertions;
using NUnit.Framework;
using System;
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
  <Copy Path=""Path1"" Destination=""Path2"" Recursive=""true"" OnConflict=""KeepBoth""></Copy>
  <Remove Path=""Path3""></Remove>
  <Remove Path=""Path4"" Content=""true"" ContentRecursive=""true"">
    <Skip ItemType=""File"" NewerThan=""30.00:00:00"" />
  </Remove>
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

            AssertCopyAction(copy1, "Path1", "Path2", false, OnFileConflict.RaiseError);
            AssertCopyAction(copy2, "Path1", "Path2", true, OnFileConflict.KeepBoth);

            AssertRemoveAction(remove1, "Path3", false, false);
            AssertRemoveAction(remove2, "Path4", true, true);

            remove2.Options.Should().HaveCount(1);
            var skip1 = (Skip)remove2.Options[0];
            skip1.NewerThan.Should().Be(TimeSpan.FromDays(30));
        }

        private void AssertRemoveAction(Remove result, string path, bool content, bool contentRecursive)
        {
            result.Path.Should().Be(path);
            result.Content.Should().Be(content);
            result.ContentRecursive.Should().Be(contentRecursive);
        }

        private void AssertCopyAction(Copy result, string path, string dest, bool recursive, OnFileConflict onConflict)
        {
            result.Path.Should().Be(path);
            result.Destination.Should().Be(dest);
            result.Recursive.Should().Be(recursive);
            result.OnConflict.Should().Be(onConflict);
        }
    }
}
