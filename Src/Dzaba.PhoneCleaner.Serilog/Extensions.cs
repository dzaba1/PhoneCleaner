using Dzaba.PhoneCleaner.Utils;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Dzaba.PhoneCleaner.Serilog
{
    public static class Extensions
    {
        public static readonly string Format = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3} [{ThreadId}] {SourceContext} - {Message:lj}{NewLine}{Exception}";

        public static void RegisterSerilog(this IServiceCollection services, ILogger logger)
        {
            Require.NotNull(services, nameof(services));
            Require.NotNull(logger, nameof(logger));

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
            Require.NotNull(loggerConfiguration, nameof(loggerConfiguration));

            return loggerConfiguration
                .WriteTo.Console(level, Format);
        }
    }
}
