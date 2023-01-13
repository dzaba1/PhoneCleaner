﻿using AutoFixture;
using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Handlers;
using FluentAssertions;
using NUnit.Framework;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests.Handlers
{
    [TestFixture]
    public class RemoveHandlerTests : HandlerTestFixture
    {
        private RemoveHandler CreateSut()
        {
            return Fixture.Create<RemoveHandler>();
        }

        [Test]
        public void Handle_WhenContentFlagIsNotSpecified_ThenDirectoryIsDeleted()
        {
            var model = new Remove()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Content = false,
                ContentRecursive = false
            };

            var device = GetTempPathDevice();
            SetupSomeDeviceFiles();

            var sut = CreateSut();

            var result = sut.Handle(model, device, GetCleanData());

            result.Should().Be(1);
            Directory.Exists(model.Path).Should().BeFalse();
        }

        [Test]
        public void Handle_WhenDirectoryDoesntExists_ThenNothing()
        {
            var model = new Remove()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Content = false,
                ContentRecursive = false
            };

            var device = GetTempPathDevice();

            var sut = CreateSut();

            var result = sut.Handle(model, device, GetCleanData());

            result.Should().Be(0);
        }

        [Test]
        public void Handle_WhenContentAndRecursiveFlagIsSpecified_ThenDirectoryIsEmpty()
        {
            var model = new Remove()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Content = true,
                ContentRecursive = true
            };

            var device = GetTempPathDevice();
            SetupSomeDeviceFiles();

            var sut = CreateSut();

            var result = sut.Handle(model, device, GetCleanData());

            result.Should().Be(4);
            Directory.Exists(model.Path).Should().BeTrue();
            Directory.EnumerateFiles(model.Path).Should().BeEmpty();
            Directory.EnumerateDirectories(model.Path).Should().BeEmpty();
        }

        [Test]
        public void Handle_WhenContentFlagIsSpecified_ThenDirectoryDoesntHaveFiles()
        {
            var model = new Remove()
            {
                Path = Path.Combine(DeviceRootDir, "Dir1"),
                Content = true,
                ContentRecursive = false
            };

            var device = GetTempPathDevice();
            SetupSomeDeviceFiles();

            var sut = CreateSut();

            var result = sut.Handle(model, device, GetCleanData());

            result.Should().Be(2);
            Directory.Exists(model.Path).Should().BeTrue();
            Directory.EnumerateFiles(model.Path).Should().BeEmpty();
            Directory.EnumerateDirectories(model.Path).Should().HaveCount(2);
        }
    }
}
