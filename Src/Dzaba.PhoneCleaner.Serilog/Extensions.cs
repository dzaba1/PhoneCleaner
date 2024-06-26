﻿using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;

namespace Dzaba.PhoneCleaner.Serilog
{
    public static class Extensions
    {
        public static readonly string Format = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3} [{ThreadId}] {SourceContext} - {Message:lj}{NewLine}{Exception}";

        public static void RegisterSerilog(this IServiceCollection services, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));

            services.AddLogging(c => c.AddSerilog(logger, true));
        }

        public static LoggerConfiguration GetLoggerConfiguration()
        {
            return new LoggerConfiguration()
                .Enrich.WithThreadId()
                .MinimumLevel.Debug();
        }

        public static LoggerConfiguration AddConsole(this LoggerConfiguration loggerConfiguration, LogEventLevel level = LogEventLevel.Debug)
        {
            ArgumentNullException.ThrowIfNull(loggerConfiguration, nameof(loggerConfiguration));

            return loggerConfiguration
                .WriteTo.Console(level, Format);
        }

        public static LoggerConfiguration AddFile(this LoggerConfiguration loggerConfiguration, string path, LogEventLevel level = LogEventLevel.Debug)
        {
            ArgumentNullException.ThrowIfNull(loggerConfiguration, nameof(loggerConfiguration));

            return loggerConfiguration
                .WriteTo.File(path, level, fileSizeLimitBytes: 15728640, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, retainedFileCountLimit: 10);
        }
    }
}
