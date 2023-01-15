using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Config.Options;
using FluentAssertions;
using NUnit.Framework;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests.Integration.Handlers
{
    [TestFixture]
    public class RemoveHandlerTests : HandlerTestFixture
    {
        [Test]
        public void Handle_WhenDirectoryDoesntExists_ThenNothing()
        {
            var model = new Remove()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Recursive = false
            };

            MakeConfig(model);

            var cleaner = CreateCleaner();
            var result = cleaner.Clean(GetCleanData());

            result.Should().Be(0);
        }

        [Test]
        public void Handle_WhenRecursiveFlagIsSpecified_ThenDirectoryIsEmpty()
        {
            var model = new Remove()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Recursive = true
            };

            MakeConfig(model);
            SetupSomeDeviceFiles();

            var cleaner = CreateCleaner();
            var result = cleaner.Clean(GetCleanData());

            result.Should().Be(14);
            Directory.Exists(model.Path).Should().BeTrue();
            Directory.EnumerateFiles(model.Path).Should().BeEmpty();
            Directory.EnumerateDirectories(model.Path).Should().BeEmpty();
        }

        [Test]
        public void Handle_WhenNotRecursive_ThenDirectoryDoesntHaveFiles()
        {
            var model = new Remove()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Recursive = false
            };

            MakeConfig(model);
            SetupSomeDeviceFiles();

            var cleaner = CreateCleaner();
            var result = cleaner.Clean(GetCleanData());

            result.Should().Be(2);
            Directory.Exists(model.Path).Should().BeTrue();
            Directory.EnumerateFiles(model.Path).Should().BeEmpty();
            Directory.EnumerateDirectories(model.Path).Should().HaveCount(2);
        }

        [Test]
        public void Handle_WhenNotOk_ThenNothing()
        {
            var model = new Remove()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
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
            Directory.Exists(model.Path).Should().BeTrue();
            Directory.EnumerateFiles(model.Path).Should().HaveCount(2);
            Directory.EnumerateDirectories(model.Path).Should().HaveCount(2);
        }
    }
}
