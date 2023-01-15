using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Lib.Tests.Integration.Device;
using Dzaba.PhoneCleaner.Serilog;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace Dzaba.PhoneCleaner.Lib.Tests.Integration
{
    public class IocTestFixture : TempFileTestFixture
    {
        private ServiceProvider serviceProvider;

        protected IServiceProvider Container => serviceProvider;

        [SetUp]
        public void SetupContainer()
        {
            var services = new ServiceCollection();
            services.RegisterPhoneCleanerLib();

            var loggerConfig = Serilog.Extensions.GetLoggerConfiguration();
            loggerConfig.AddConsole();
            services.RegisterSerilog(loggerConfig.CreateLogger());

            services.AddTransient<IDeviceConnectionFactory, DeviceConnectionFactory>();

            OnSetupContainer(services);

            serviceProvider = services.BuildServiceProvider();
        }

        protected virtual void OnSetupContainer(IServiceCollection services)
        {

        }

        [TearDown]
        public void CleanupContainer()
        {
            serviceProvider?.Dispose();
        }
    }
}
