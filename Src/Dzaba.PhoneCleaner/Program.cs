using Dzaba.PhoneCleaner.Lib;
using Dzaba.PhoneCleaner.MtpMediaDevices;
using Dzaba.PhoneCleaner.Serilog;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;
using System;
using System.IO;

namespace Dzaba.PhoneCleaner
{
    internal static class Program
    {
        public static int Main(string workingDir,
            string deviceName,
            int driveIndex = 0,
            string configFilepath = null,
            bool testOnly = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(configFilepath))
                {
                    configFilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.xml");
                }

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


