using AutoFixture;
using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Handlers;
using FluentAssertions;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace Dzaba.PhoneCleaner.Lib.Tests.Handlers
{
    [TestFixture]
    public class CopyHandlerTests : HandlerTestFixture
    {
        private CopyHandler CreateSut()
        {
            return Fixture.Create<CopyHandler>();
        }

        [Test]
        public void Handle_WhenNotRecursive_ThenOnlyFilesAreCopied()
        {
            var model = new Copy()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Destination = Path.Combine(WorkingDir, "Dir1"),
                OnConflict = OnFileConflict.RaiseError,
                Recursive = false
            };

            var device = GetTempPathDevice();
            SetupSomeDeviceFiles();

            var sut = CreateSut();

            var result = sut.Handle(model, device, GetCleanData());

            result.Should().Be(2);
            Directory.Exists(model.Destination).Should().BeTrue();
            Directory.EnumerateFiles(model.Destination).Should().HaveCount(2);
            Directory.EnumerateDirectories(model.Destination).Should().BeEmpty();
        }

        [Test]
        public void Handle_WhenDirectoryDoesntExists_ThenNothing()
        {
            var model = new Copy()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Destination = Path.Combine(WorkingDir, "Dir1"),
                OnConflict = OnFileConflict.RaiseError,
                Recursive = false
            };

            var device = GetTempPathDevice();

            var sut = CreateSut();

            var result = sut.Handle(model, device, GetCleanData());

            result.Should().Be(0);
            Directory.Exists(model.Destination).Should().BeFalse();
        }

        [Test]
        public void Handle_WhenFileExistButOnConflictDoNothing_ThenFileIsNotCopied()
        {
            var expected = "Not touched";

            var model = new Copy()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Destination = Path.Combine(WorkingDir, "Dir1"),
                OnConflict = OnFileConflict.DoNothing,
                Recursive = false
            };

            var device = GetTempPathDevice();
            SetupSomeDeviceFiles();

            var fileToCheck = Path.Combine(model.Destination, "file1.txt");
            Directory.CreateDirectory(model.Destination);
            File.WriteAllText(fileToCheck, expected);

            var sut = CreateSut();

            var result = sut.Handle(model, device, GetCleanData());

            result.Should().Be(1);
            Directory.EnumerateFiles(model.Destination).Should().HaveCount(2);
            File.ReadAllText(fileToCheck).Should().Be(expected);
        }

        [Test]
        public void Handle_WhenFileExistAndOverride_ThenFileIsCopied()
        {
            var expected = "Test";

            var model = new Copy()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Destination = Path.Combine(WorkingDir, "Dir1"),
                OnConflict = OnFileConflict.Overwrite,
                Recursive = false
            };

            var device = GetTempPathDevice();
            SetupSomeDeviceFiles();

            var fileToCheck = Path.Combine(model.Destination, "file1.txt");
            Directory.CreateDirectory(model.Destination);
            File.WriteAllText(fileToCheck, "Not touched");

            var sut = CreateSut();

            var result = sut.Handle(model, device, GetCleanData());

            result.Should().Be(2);
            Directory.EnumerateFiles(model.Destination).Should().HaveCount(2);
            File.ReadAllText(fileToCheck).Should().Be(expected);
        }

        [Test]
        public void Handle_WhenFileExistAndRaiseError_ThenError()
        {
            var model = new Copy()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Destination = Path.Combine(WorkingDir, "Dir1"),
                OnConflict = OnFileConflict.RaiseError,
                Recursive = false
            };

            var device = GetTempPathDevice();
            SetupSomeDeviceFiles();

            var fileToCheck = Path.Combine(model.Destination, "file1.txt");
            Directory.CreateDirectory(model.Destination);
            File.WriteAllText(fileToCheck, "Not touched");

            var sut = CreateSut();

            this.Invoking(s => sut.Handle(model, device, GetCleanData()))
                .Should().Throw<IOException>();
        }

        [Test]
        public void Handle_WhenRecursive_ThenDirectorisAreCopied()
        {
            var model = new Copy()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Destination = Path.Combine(WorkingDir, "Dir1"),
                OnConflict = OnFileConflict.RaiseError,
                Recursive = true
            };

            var device = GetTempPathDevice();
            SetupSomeDeviceFiles();

            var sut = CreateSut();

            var result = sut.Handle(model, device, GetCleanData());

            result.Should().Be(14);
            Directory.Exists(model.Destination).Should().BeTrue();
            Directory.EnumerateFiles(model.Destination).Should().HaveCount(2);
            Directory.EnumerateDirectories(model.Destination).Should().HaveCount(2);

            var nextDir = Directory.EnumerateDirectories(model.Destination).First();
            Directory.EnumerateFiles(nextDir).Should().HaveCount(2);
            Directory.EnumerateDirectories(nextDir).Should().HaveCount(2);
        }
    }
}
