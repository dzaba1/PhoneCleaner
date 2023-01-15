using Dzaba.PhoneCleaner.Lib;
using Dzaba.PhoneCleaner.MtpMediaDevices;
using Dzaba.PhoneCleaner.Serilog;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;
using System;

namespace Dzaba.PhoneCleaner
{
    internal static class Program
    {
        /// <summary>
        /// Main function.
        /// </summary>
        /// <param name="workingDir">The main directory where all files will be copied from the device.</param>
        /// <param name="deviceName">The friendly name of the device.</param>
        /// <param name="driveIndex">The ordinal index of the device storage.</param>
        /// <param name="configFilepath">The path to the confiuguration file.</param>
        /// <param name="testOnly">If set then any write operations like copy or delete won't be executed. Only log traces.</param>
        /// <returns></returns>
        public static int Main(string workingDir,
            string deviceName,
            int driveIndex = 0,
            string configFilepath = "config.xml",
            bool testOnly = false)
        {
            try
            {
                var data = new CleanData
                {
                    ConfigFilepath = configFilepath,
                    DeviceName = deviceName,
                    DriveIndex = driveIndex,
                    WorkingDir = workingDir,
                    TestOnly = testOnly
                };

                var services = new ServiceCollection();
                services.RegisterPhoneCleanerLib();
                services.RegisterMtpMediaDevices();

                var loggerConfig = Extensions.GetLoggerConfiguration();
                loggerConfig.AddConsole(LogEventLevel.Information);
                services.RegisterSerilog(loggerConfig.CreateLogger());

                services.AddTransient<IApp, App>();

                using var container = services.BuildServiceProvider();
                var app = container.GetRequiredService<IApp>();
                return app.Run(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }
    }
}


