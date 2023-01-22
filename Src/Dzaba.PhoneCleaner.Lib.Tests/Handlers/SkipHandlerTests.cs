using AutoFixture;
using Dzaba.PhoneCleaner.Lib.Config.Options;
using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Lib.Handlers.Options;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Dzaba.PhoneCleaner.Lib.Tests.Handlers
{
    [TestFixture]
    public class SkipHandlerTests
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = TestFixture.Create();
        }

        private SkipHandler CreateSut()
        {
            return fixture.Create<SkipHandler>();
        }

        public static IEnumerable<TestCaseData> GetOlderThanData()
        {
            var now = new DateTime(2020, 1, 10);

            yield return new TestCaseData(now, new DateTime(2018, 1, 10), TimeSpan.FromDays(14), false);
            yield return new TestCaseData(now, new DateTime(2020, 1, 9), TimeSpan.FromDays(14), true);
            yield return new TestCaseData(now, new DateTime(2020, 1, 9), TimeSpan.FromDays(1), false);
            yield return new TestCaseData(now, new DateTime(2020, 1, 8), TimeSpan.FromDays(1), false);
        }

        public static IEnumerable<TestCaseData> GetNewerThanData()
        {
            var now = new DateTime(2020, 1, 10);

            yield return new TestCaseData(now, new DateTime(2018, 1, 10), TimeSpan.FromDays(14), true);
            yield return new TestCaseData(now, new DateTime(2020, 1, 9), TimeSpan.FromDays(14), false);
            yield return new TestCaseData(now, new DateTime(2020, 1, 9), TimeSpan.FromDays(1), false);
            yield return new TestCaseData(now, new DateTime(2020, 1, 9), TimeSpan.FromDays(2), false);
        }

        [TestCaseSource(nameof(GetNewerThanData))]
        public void IsOk_WhenNewerThanSpecified_ThenCorrectCheck(DateTime now, DateTime modificationTime, TimeSpan newerThan, bool expected)
        {
            fixture.FreezeMock<IDateTimeProvider>()
                .Setup(x => x.Now())
                .Returns(now);

            var systemInfo = TestUtils.CreateSystemInfo<IDeviceSystemInfo>(modifiedTime: modificationTime);

            var skip = new Skip
            {
                NewerThan = newerThan
            };

            var sut = CreateSut();

            var result = sut.IsOk(skip, null, systemInfo);
            result.Should().Be(expected);
        }

        [TestCaseSource(nameof(GetOlderThanData))]
        public void IsOk_WhenOlderThanSpecified_ThenCorrectCheck(DateTime now, DateTime modificationTime, TimeSpan olderThan, bool expected)
        {
            fixture.FreezeMock<IDateTimeProvider>()
                .Setup(x => x.Now())
                .Returns(now);

            var systemInfo = TestUtils.CreateSystemInfo<IDeviceSystemInfo>(modifiedTime: modificationTime);

            var skip = new Skip
            {
                OlderThan = olderThan
            };

            var sut = CreateSut();

            var result = sut.IsOk(skip, null, systemInfo);
            result.Should().Be(expected);
        }
    }
}
