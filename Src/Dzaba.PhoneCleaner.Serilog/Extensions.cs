using Dzaba.PhoneCleaner.Utils;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Dzaba.PhoneCleaner.Serilog
{
    public static class Extensions
    {
        public static void RegisterSerilog(this IServiceCollection services, ILogger logger)
        {
            Require.NotNull(services, nameof(services));
            Require.NotNull(logger, nameof(logger));

            services.AddLogging(c => c.AddSerilog(logger, true));
        }
    }
}
