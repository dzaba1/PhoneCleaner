using NUnit.Framework;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests
{
    public class TempFileTestFixture
    {
        protected string TempPath { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetupTemp()
        {
            TempPath = Path.Combine(Path.GetTempPath(), GetType().Name);
        }

        [SetUp]
        public void SetupTemp()
        {
            CleanupTemp();

            Directory.CreateDirectory(TempPath);
        }

        [TearDown]
        public void CleanupTemp()
        {
            if (!string.IsNullOrWhiteSpace(TempPath) && Directory.Exists(TempPath))
            {
                Directory.Delete(TempPath, true);
            }
        }
    }
}
