using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Lib.Handlers;
using Dzaba.PhoneCleaner.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Dzaba.PhoneCleaner.Lib
{
    public interface ICleaner
    {
        int Clean(string configFilepath, string deviceName, CleanData cleanData);
    }

    internal sealed class Cleaner : ICleaner
    {
        private readonly IConfigReader configReader;
        private readonly IDeviceConnectionFactory deviceConnectionFactory;
        private readonly IHandlerFactory handlerFactory;
        private readonly ILogger<Cleaner> logger;

        public Cleaner(IConfigReader configReader,
            IDeviceConnectionFactory deviceConnectionFactory,
            IHandlerFactory handlerFactory,
            ILogger<Cleaner> logger)
        {
            Require.NotNull(configReader, nameof(configReader));
            Require.NotNull(deviceConnectionFactory, nameof(deviceConnectionFactory));
            Require.NotNull(handlerFactory, nameof(handlerFactory));
            Require.NotNull(logger, nameof(logger));

            this.configReader = configReader;
            this.deviceConnectionFactory = deviceConnectionFactory;
            this.handlerFactory = handlerFactory;
            this.logger = logger;
        }

        public int Clean(string configFilepath, string deviceName, CleanData cleanData)
        {
            Require.NotWhiteSpace(configFilepath, nameof(configFilepath));
            Require.NotWhiteSpace(deviceName, nameof(deviceName));
            Require.NotNull(cleanData, nameof(cleanData));

            var config = configReader.Read(configFilepath);

            var affected = 0;

            using (var device = deviceConnectionFactory.Create(deviceName, cleanData.TestOnly))
            {
                foreach (var action in config.Actions)
                {
                    affected += HandleAction(action, device, cleanData);
                }
            }

            return affected;
        }

        private int HandleAction(Config.Action action, IDeviceConnection device, CleanData cleanData)
        {
            try
            {
                var perfWatch = Stopwatch.StartNew();

                logger.LogInformation("Start action {Action}.", action.GetType());

                var handler = handlerFactory.CreateHandler(action);
                var result = handler.Handle(action, device, cleanData);

                perfWatch.Stop();
                logger.LogInformation("Action {Action} finished. Took {Elapsed}.", action.GetType(), perfWatch.Elapsed);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error action {Action}.", action.GetType());
                return 0;
            }
        }
    }
}
