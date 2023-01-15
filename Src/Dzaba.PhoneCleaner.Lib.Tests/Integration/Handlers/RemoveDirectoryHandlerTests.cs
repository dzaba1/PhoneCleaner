using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Config.Options;
using FluentAssertions;
using NUnit.Framework;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests.Integration.Handlers
{
    [TestFixture]
    public class RemoveDirectoryHandlerTests : HandlerTestFixture
    {
        [Test]
        public void Handle_WhenOk_ThenDirectoryIsDeleted()
        {
            var model = new RemoveDirectory()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1")
            };

            MakeConfig(model);
            SetupSomeDeviceFiles();

            var cleaner = CreateCleaner();
            var result = cleaner.Clean(GetCleanData());

            result.Should().Be(1);
            Directory.Exists(model.Path).Should().BeFalse();
        }

        [Test]
        public void Handle_WhenNotOk_ThenDirectoryIsNotDeleted()
        {
            var model = new RemoveDirectory()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
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
        }

        [Test]
        public void Handle_WhenDirectoryDoesntExist_ThenNothing()
        {
            var model = new RemoveDirectory()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1")
            };

            MakeConfig(model);

            var cleaner = CreateCleaner();
            var result = cleaner.Clean(GetCleanData());

            result.Should().Be(0);
        }
    }
}
