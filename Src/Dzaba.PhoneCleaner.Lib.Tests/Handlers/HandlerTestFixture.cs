using AutoFixture;
using Dzaba.PhoneCleaner.Lib.Tests.Device;
using NUnit.Framework;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests.Handlers
{
    public class HandlerTestFixture : TempFileTestFixture
    {
        protected string WorkingDir { get; private set; }

        protected string DeviceRootDir { get; private set; }

        protected IFixture Fixture { get; private set; }

        [SetUp]
        public void SetupHandler()
        {
            Fixture = TestFixture.Create();

            WorkingDir = Path.Combine(TempPath, "WorkingDir");
            Directory.CreateDirectory(WorkingDir);

            DeviceRootDir = Path.Combine(TempPath, "Device");
            Directory.CreateDirectory(DeviceRootDir);
        }

        protected CleanData GetCleanData()
        {
            return new CleanData
            {
                DriveIndex = 0,
                WorkingDir = WorkingDir
            };
        }

        protected void SetupSomeDeviceFiles(int dirFilesCount = 2, int dirsCount = 2, int depth = 3)
        {
            SetupSomeDeviceFilesRecursive(dirFilesCount, dirsCount, depth, DeviceRootDir, 0);
        }

        private void SetupSomeDeviceFilesRecursive(int dirFilesCount, int dirsCount, int depth, string currentDir, int currentDepth)
        {
            for (int i = 0; i < dirFilesCount; i++)
            {
                File.WriteAllText(Path.Combine(currentDir, $"file{i + 1}.txt"), "Test");
            }

            if (currentDepth < depth)
            {
                for (int i = 0; i < dirsCount; i++)
                {
                    var newDir = Path.Combine(currentDir, $"Dir{i + 1}");
                    Directory.CreateDirectory(newDir);

                    SetupSomeDeviceFilesRecursive(dirFilesCount, dirsCount, depth, newDir, currentDepth + 1);
                }
            }
        }

        protected TempPathDevice GetTempPathDevice()
        {
            return new TempPathDevice(DeviceRootDir);
        }
    }
}
