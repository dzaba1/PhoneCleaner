using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Config.Options;
using FluentAssertions;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace Dzaba.PhoneCleaner.Lib.Tests.Integration.Handlers
{
    [TestFixture]
    public class MoveHandlerTests : HandlerTestFixture
    {
        [Test]
        public void Handle_WhenNotRecursive_ThenOnlyFilesAreCopiedAndRemoved()
        {
            var model = new Move()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Destination = Path.Combine(WorkingDir, "Dir1"),
                OnConflict = OnFileConflict.RaiseError,
                Recursive = false
            };

            MakeConfig(model);
            SetupSomeDeviceFiles();

            var cleaner = CreateCleaner();
            var result = cleaner.Clean(GetCleanData());

            result.Should().Be(2);
            Directory.Exists(model.Destination).Should().BeTrue();
            Directory.EnumerateFiles(model.Destination).Should().HaveCount(2);
            Directory.EnumerateDirectories(model.Destination).Should().BeEmpty();

            Directory.Exists(model.Path).Should().BeTrue();
            Directory.EnumerateFiles(model.Path).Should().BeEmpty();
            Directory.EnumerateDirectories(model.Path).Should().HaveCount(2);

            var nextDeviceDir = Directory.EnumerateDirectories(model.Path).First();
            Directory.EnumerateFiles(nextDeviceDir).Should().HaveCount(2);
            Directory.EnumerateDirectories(nextDeviceDir).Should().HaveCount(2);
        }

        [Test]
        public void Handle_WhenDirectoryDoesntExists_ThenNothing()
        {
            var model = new Move()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Destination = Path.Combine(WorkingDir, "Dir1"),
                OnConflict = OnFileConflict.RaiseError,
                Recursive = false
            };

            MakeConfig(model);

            var cleaner = CreateCleaner();
            var result = cleaner.Clean(GetCleanData());

            result.Should().Be(0);

            Directory.Exists(model.Destination).Should().BeFalse();
        }

        [Test]
        public void Handle_WhenRecursive_ThenDirectorisAreCopied()
        {
            var model = new Move()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Destination = Path.Combine(WorkingDir, "Dir1"),
                OnConflict = OnFileConflict.RaiseError,
                Recursive = true
            };

            MakeConfig(model);
            SetupSomeDeviceFiles();

            var cleaner = CreateCleaner();
            var result = cleaner.Clean(GetCleanData());

            result.Should().Be(14);
            Directory.Exists(model.Destination).Should().BeTrue();
            Directory.EnumerateFiles(model.Destination).Should().HaveCount(2);
            Directory.EnumerateDirectories(model.Destination).Should().HaveCount(2);

            var nextDir = Directory.EnumerateDirectories(model.Destination).First();
            Directory.EnumerateFiles(nextDir).Should().HaveCount(2);
            Directory.EnumerateDirectories(nextDir).Should().HaveCount(2);

            Directory.Exists(model.Path).Should().BeTrue();
            Directory.EnumerateFiles(model.Path).Should().BeEmpty();
            Directory.EnumerateDirectories(model.Path).Should().BeEmpty();
        }

        [Test]
        public void Handle_WhenNotOk_ThenNothing()
        {
            var model = new Copy()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Destination = Path.Combine(WorkingDir, "Dir1"),
                OnConflict = OnFileConflict.RaiseError,
                Recursive = false,
                Options = new Option[]
                {
                    new Regex
                    {
                        Pattern = "invalid",
                        RegexOptions = System.Text.RegularExpressions.RegexOptions.IgnoreCase
                    }
                }
            };

            MakeConfig(model);
            SetupSomeDeviceFiles();

            var cleaner = CreateCleaner();
            var result = cleaner.Clean(GetCleanData());

            result.Should().Be(0);
            Directory.Exists(model.Destination).Should().BeTrue();
            Directory.EnumerateFiles(model.Destination).Should().BeEmpty();
            Directory.EnumerateDirectories(model.Destination).Should().BeEmpty();

            Directory.Exists(model.Path).Should().BeTrue();
            Directory.EnumerateFiles(model.Path).Should().HaveCount(2);
            Directory.EnumerateDirectories(model.Path).Should().HaveCount(2);
        }
    }
}
