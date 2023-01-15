using Dzaba.PhoneCleaner.Lib.Config.Options;
using Dzaba.PhoneCleaner.Lib.Handlers.Options;
using Dzaba.PhoneCleaner.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dzaba.PhoneCleaner.Lib.Handlers
{
    public interface IHandlerFactory
    {
        IHandler CreateHandler(Config.Action model);
        IOptionHandler CreateOptionHandler(Option model);
    }

    internal sealed class HandlerFactory : IHandlerFactory
    {
        private readonly IReadOnlyDictionary<Type, IHandler> handlers;
        private readonly IReadOnlyDictionary<Type, IOptionHandler> optionsHandlers;
        private readonly ILogger<HandlerFactory> logger;

        public HandlerFactory(IEnumerable<IHandler> handlers,
            IEnumerable<IOptionHandler> optionsHandlers,
            ILogger<HandlerFactory> logger)
        {
            Require.NotNull(handlers, nameof(handlers));
            Require.NotNull(optionsHandlers, nameof(optionsHandlers));
            Require.NotNull(logger, nameof(logger));

            this.handlers = handlers.ToDictionary(h => h.ModelType);
            this.optionsHandlers = optionsHandlers.ToDictionary(h => h.ModelType);
            this.logger = logger;
        }

        public IHandler CreateHandler(Config.Action model)
        {
            Require.NotNull(model, nameof(model));

            var type = model.GetType();

            logger.LogDebug("Getting handler for action {Action}.", type);

            if (handlers.TryGetValue(model.GetType(), out IHandler handler))
            {
                return handler;
            }

            throw new InvalidOperationException($"Couldn't find any handler for action {type}.");
        }

        public IOptionHandler CreateOptionHandler(Option model)
        {
            Require.NotNull(model, nameof(model));

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
