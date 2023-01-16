using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Lib.Tests.Integration.Device;
using Dzaba.PhoneCleaner.Utils;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.IO;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Tests.Integration.Handlers
{
    public class HandlerTestFixture : IocTestFixture
    {
        protected string WorkingDir { get; private set; }

        protected string DeviceRootDir { get; private set; }

        protected string AppDir { get; private set; }

        protected string ConfigFilename { get; private set; }

        [SetUp]
        public void SetupHandler()
        {
            WorkingDir = Path.Combine(TempPath, "WorkingDir");
            Directory.CreateDirectory(WorkingDir);

            DeviceRootDir = Path.Combine(TempPath, "Device");
            Directory.CreateDirectory(DeviceRootDir);

            AppDir = Path.Combine(TempPath, "Application");
            Directory.CreateDirectory(AppDir);

            ConfigFilename = Path.Combine(AppDir, "config.xml");
        }

        protected override void OnSetupContainer(IServiceCollection services)
        {
            services.AddSingleton<IDeviceConnectionFactory>(new DeviceConnectionFactory(() => DeviceRootDir));
        }

        protected CleanData GetCleanData()
        {
            return new CleanData
            {
                WorkingDir = WorkingDir,
                TestOnly = true,
                DeviceName = "Test",
                ConfigFilepath = ConfigFilename
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

        protected ICleaner CreateCleaner()
        {
            return Container.GetRequiredService<ICleaner>();
        }

        protected void MakeConfig<T>(T model)
            where T : Action
        {
            Require.NotNull(model, nameof(model));

            var config = new Config.Config
            {
                Actions = new Action[]
                {
                    model
                }
            };

            var serliazer = new XmlSerializer(typeof(Config.Config));
            using (var fs = new FileStream(ConfigFilename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                serliazer.Serialize(fs, config);
            }
        }
    }
}
