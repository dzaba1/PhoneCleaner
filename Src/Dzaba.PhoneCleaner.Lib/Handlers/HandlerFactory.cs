using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dzaba.PhoneCleaner.Lib.Handlers
{
    public interface IHandlerFactory
    {
        IHandler CreateHandler(Config.Action model);
    }

    internal sealed class HandlerFactory : IHandlerFactory
    {
        private readonly IReadOnlyDictionary<Type, IHandler> handlers;
        private readonly ILogger<HandlerFactory> logger;

        public HandlerFactory(IEnumerable<IHandler> handlers,
            ILogger<HandlerFactory> logger)
        {
            ArgumentNullException.ThrowIfNull(handlers, nameof(handlers));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));

            this.handlers = handlers.ToDictionary(h => h.ModelType);
            this.logger = logger;
        }

        public IHandler CreateHandler(Config.Action model)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));

            var type = model.GetType();

            logger.LogDebug("Getting handler for action {Action}.", type);

            if (handlers.TryGetValue(model.GetType(), out IHandler handler))
            {
                return handler;
            }

            throw new InvalidOperationException($"Couldn't find any handler for action {type}.");
        }
    }
}
