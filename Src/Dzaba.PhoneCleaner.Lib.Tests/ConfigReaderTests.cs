using AutoFixture;
using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Config.Options;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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

        private static TestCaseData CreateTestCaseData(string xml, Action<Config.Config> assert)
        {
            return new TestCaseData(xml, assert);
        }

        public static IEnumerable<TestCaseData> GetTestData()
        {
            yield return CreateTestCaseData(@"<Config>
  <Copy Path=""Path1"" Destination=""Path2"" />
</Config>", c =>
            {
                c.Actions.Should().HaveCount(1);
                var copy = (Copy)c.Actions[0];
                AssertCopyAction(copy, "Path1", "Path2", false, OnFileConflict.RaiseError);
            });

            yield return CreateTestCaseData(@"<Config>
  <Copy Path=""Path1"" Destination=""Path2"" Recursive=""true"" OnConflict=""KeepBoth"" />
</Config>", c =>
            {
                c.Actions.Should().HaveCount(1);
                var copy = (Copy)c.Actions[0];
                AssertCopyAction(copy, "Path1", "Path2", true, OnFileConflict.KeepBoth);
            });

            yield return CreateTestCaseData(@"<Config>
  <Remove Path=""Path3"" />
</Config>", c =>
            {
                c.Actions.Should().HaveCount(1);
                var remove = (Remove)c.Actions[0];
                AssertRemoveAction(remove, "Path3", false);
            });

            yield return CreateTestCaseData(@"<Config>
  <Remove Path=""Path4"" Recursive=""true"" />
</Config>", c =>
            {
                c.Actions.Should().HaveCount(1);
                var remove = (Remove)c.Actions[0];
                AssertRemoveAction(remove, "Path4", true);
            });

            yield return CreateTestCaseData(@"<Config>
  <RemoveDirectory Path=""Path5"" />
</Config>", c =>
            {
                c.Actions.Should().HaveCount(1);
                var remove = (RemoveDirectory)c.Actions[0];
                remove.Path.Should().Be("Path5");
            });

            yield return CreateTestCaseData(@"<Config>
  <Remove Path=""Path6"">
    <Regex Pattern="".*"" />
  </Remove>
</Config>", c =>
            {
                c.Actions.Should().HaveCount(1);
                var remove = (Remove)c.Actions[0];
                remove.Options.Should().HaveCount(1);
                var regex = (Regex)remove.Options[0];
                regex.ItemType.Should().Be(IOItemType.Both);
                regex.Pattern.Should().Be(".*");
                regex.RegexOptions.Should().Be(System.Text.RegularExpressions.RegexOptions.None);
            });

            yield return CreateTestCaseData(@"<Config>
  <Remove Path=""Path6"">
    <Regex Pattern="".*"" ItemType=""File"" RegexOptions=""IgnoreCase Multiline"" />
  </Remove>
</Config>", c =>
            {
                c.Actions.Should().HaveCount(1);
                var remove = (Remove)c.Actions[0];
                remove.Options.Should().HaveCount(1);
                var regex = (Regex)remove.Options[0];
                regex.ItemType.Should().Be(IOItemType.File);
                regex.Pattern.Should().Be(".*");
                regex.RegexOptions.Should().Be(System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);
            });

            yield return CreateTestCaseData(@"<Config>
  <Remove Path=""Path6"">
    <Skip NewerThan=""30.00:00:00"" OlderThan=""40.00:00:00"" />
  </Remove>
</Config>", c =>
            {
                c.Actions.Should().HaveCount(1);
                var remove = (Remove)c.Actions[0];
                remove.Options.Should().HaveCount(1);
                var skip = (Skip)remove.Options[0];
                skip.ItemType.Should().Be(IOItemType.Both);
                skip.NewerThan.Should().Be(TimeSpan.FromDays(30));
                skip.OlderThan.Should().Be(TimeSpan.FromDays(40));
            });

            yield return CreateTestCaseData(@"<Config>
  <Remove Path=""Path6"">
    <Take NewerThan=""30.00:00:00"" OlderThan=""40.00:00:00"" />
  </Remove>
</Config>", c =>
            {
                c.Actions.Should().HaveCount(1);
                var remove = (Remove)c.Actions[0];
                remove.Options.Should().HaveCount(1);
                var take = (Take)remove.Options[0];
                take.ItemType.Should().Be(IOItemType.Both);
                take.NewerThan.Should().Be(TimeSpan.FromDays(30));
                take.OlderThan.Should().Be(TimeSpan.FromDays(40));
            });

            yield return CreateTestCaseData(@"<Config>
  <Move Path=""Path7"" Destination=""Path8"" />
</Config>", c =>
            {
                c.Actions.Should().HaveCount(1);
                var move = (Move)c.Actions[0];
                AssertMoveAction(move, "Path7", "Path8", false, OnFileConflict.RaiseError);
            });

            yield return CreateTestCaseData(@"<Config>
  <Move Path=""Path7"" Destination=""Path8"" Recursive=""true"" OnConflict=""KeepBoth"" />
</Config>", c =>
            {
                c.Actions.Should().HaveCount(1);
                var move = (Move)c.Actions[0];
                AssertMoveAction(move, "Path7", "Path8", true, OnFileConflict.KeepBoth);
            });
        }

        [TestCaseSource(nameof(GetTestData))]
        public void Read_WhenXml_ThenConfig(string xml, Action<Config.Config> assert)
        {
            var filepath = Path.Combine(TempPath, "config.xml");
            File.WriteAllText(filepath, xml);

            var sut = CreateSut();
            var result = sut.Read(filepath);

            assert(result);
        }

        private static void AssertRemoveAction(Remove result, string path, bool recursive)
        {
            result.Path.Should().Be(path);
            result.Recursive.Should().Be(recursive);
        }

        private static void AssertCopyAction(Copy result, string path, string dest, bool recursive, OnFileConflict onConflict)
        {
            result.Path.Should().Be(path);
            result.Destination.Should().Be(dest);
            result.Recursive.Should().Be(recursive);
            result.OnConflict.Should().Be(onConflict);
        }

        private static void AssertMoveAction(Move result, string path, string dest, bool recursive, OnFileConflict onConflict)
        {
            result.Path.Should().Be(path);
            result.Destination.Should().Be(dest);
            result.Recursive.Should().Be(recursive);
            result.OnConflict.Should().Be(onConflict);
        }
    }
}
