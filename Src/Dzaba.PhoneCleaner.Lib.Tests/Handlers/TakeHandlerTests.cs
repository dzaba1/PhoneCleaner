﻿using AutoFixture;
using Dzaba.PhoneCleaner.Lib.Config.Options;
using Dzaba.PhoneCleaner.Lib.Handlers.Options;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Dzaba.PhoneCleaner.Lib.Tests.Handlers
{
    [TestFixture]
    public class TakeHandlerTests
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = TestFixture.Create();
        }

        private TakeHandler CreateSut()
        {
            return fixture.Create<TakeHandler>();
        }

        [TestCase(10, 10, false)]
        [TestCase(10, 9, true)]
        [TestCase(10, 11, false)]
        public void IsOk_WhenNewerThanSpecified_ThenCorrectCheck(int modificationTimeDays, int newerThanDays, bool expected)
        {
            var now = new DateTime(2020, 1, 1);
            fixture.FreezeMock<IDateTimeProvider>()
                .Setup(x => x.Now())
                .Returns(now);

            var modificationTime = now.AddDays(-modificationTimeDays);
            var systemInfo = TestUtils.CreateSystemInfo(modifiedTime: modificationTime);

            var take = new Take
            {
                NewerThan = TimeSpan.FromDays(newerThanDays)
            };

            var sut = CreateSut();

            var result = sut.IsOk(take, null, systemInfo);
            result.Should().Be(expected);
        }

        [TestCase(10, 10, false)]
        [TestCase(10, 9, false)]
        [TestCase(10, 11, true)]
        public void IsOk_WhenOlderThanSpecified_ThenCorrectCheck(int modificationTimeDays, int olderThanDays, bool expected)
        {
            var now = new DateTime(2020, 1, 1);
            fixture.FreezeMock<IDateTimeProvider>()
                .Setup(x => x.Now())
                .Returns(now);

            var modificationTime = now.AddDays(-modificationTimeDays);
            var systemInfo = TestUtils.CreateSystemInfo(modifiedTime: modificationTime);

            var take = new Take
            {
                OlderThan = TimeSpan.FromDays(olderThanDays)
            };

            var sut = CreateSut();

            var result = sut.IsOk(take, null, systemInfo);
            result.Should().Be(expected);
        }
    }
}