using Dzaba.PhoneCleaner.Lib.Config.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dzaba.PhoneCleaner.Lib.Handlers.Options
{
    public interface IOptionHandlerFactory
    {
        IOptionHandler CreateOptionHandler(Option model);
    }

    internal sealed class OptionHandlerFactory : IOptionHandlerFactory
    {
        private readonly ILogger<OptionHandlerFactory> logger;
        private readonly IReadOnlyDictionary<Type, IOptionHandler> optionsHandlers;

        public OptionHandlerFactory(IEnumerable<IOptionHandler> optionsHandlers,
            ILogger<OptionHandlerFactory> logger)
        {
            ArgumentNullException.ThrowIfNull(optionsHandlers, nameof(optionsHandlers));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));

            this.optionsHandlers = optionsHandlers.ToDictionary(h => h.ModelType);
            this.logger = logger;
        }

        public IOptionHandler CreateOptionHandler(Option model)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            var type = model.GetType();

            logger.LogDebug("Getting option handler for option {Option}.", type);

            if (optionsHandlers.TryGetValue(model.GetType(), out IOptionHandler handler))
            {
                return handler;
            }

            throw new InvalidOperationException($"Couldn't find any option handler for option {type}.");
        }
    }
}
