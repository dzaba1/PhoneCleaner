using Dzaba.PhoneCleaner.Lib;
using Dzaba.PhoneCleaner.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Dzaba.PhoneCleaner
{
    internal interface IApp
    {
        int Run(CleanData data);
    }

    internal sealed class App : IApp
    {
        private readonly ICleaner cleaner;
        private readonly ILogger<App> logger;

        public App(ICleaner cleaner,
            ILogger<App> logger)
        {
            Require.NotNull(cleaner, nameof(cleaner));
            Require.NotNull(logger, nameof(logger));

            this.cleaner = cleaner;
            this.logger = logger;
        }

        public int Run(CleanData data)
        {
            Require.NotNull(data, nameof(data));

            try
            {
                ValidateInput(data);

                var affected = cleaner.Clean(data);
                logger.LogInformation("Clean finished. Affected {Affected} files or directories.", affected);
                return 0;
            }
            catch (ExitCodeException ex)
            {
                logger.LogError(ex, "Unhandled error.");
                return (int)ex.ExitCode;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled error.");
                return 1;
            }
        }

        private void ValidateInput(CleanData data)
        {
            if (string.IsNullOrWhiteSpace(data.DeviceName))
            {
                throw new ExitCodeException("The provided device name is empty.", ExitCode.EmptyDeviceName);
            }

            if (string.IsNullOrWhiteSpace(data.WorkingDir))
            {
                throw new ExitCodeException("The provided working directory is empty.", ExitCode.EmptyWorkingDir);
            }

            if (string.IsNullOrWhiteSpace(data.ConfigFilepath))
            {
                throw new ExitCodeException("The provided configuration file path is empty.", ExitCode.EmptyConfigFile);
            }

            if (!Path.IsPathRooted(data.ConfigFilepath))
            {
                data.ConfigFilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, data.ConfigFilepath);
            }

            if (!File.Exists(data.ConfigFilepath))
            {
                throw new ExitCodeException($"The provided configuration file '{data.ConfigFilepath}' doesn't exist.", ExitCode.MissingConfigFile);
            }
        }
    }
}
