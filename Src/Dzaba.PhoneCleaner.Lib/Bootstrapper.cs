using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Handlers;
using Dzaba.PhoneCleaner.Lib.Handlers.Options;
using Dzaba.PhoneCleaner.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Dzaba.PhoneCleaner.Lib
{
    public static class Bootstrapper
    {
        public static void RegisterPhoneCleanerLib(this IServiceCollection services)
        {
            Require.NotNull(services, nameof(services));

            services.AddTransient<ICleaner, Cleaner>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IIOHelper, IOHelper>();
            services.AddTransient<IHandlerFactory, HandlerFactory>();
            services.AddTransient<IOptionsEvaluator, OptionsEvaluator>();
            services.AddTransient<IConfigReader, ConfigReader>();

            services.AddTransient<IHandler, CopyHandler>();
            services.AddTransient<IHandler, MoveHandler>();
            services.AddTransient<IHandler, RemoveDirectoryHandler>();
            services.AddTransient<IHandler, RemoveHandler>();

            services.AddTransient<IOptionHandler, RegexHandler>();
            services.AddTransient<IOptionHandler, SkipHandler>();
            services.AddTransient<IOptionHandler, TakeHandler>();
        }
    }
}
