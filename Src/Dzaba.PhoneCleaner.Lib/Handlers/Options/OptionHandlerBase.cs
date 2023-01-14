using Dzaba.PhoneCleaner.Lib.Config.Options;
using Dzaba.PhoneCleaner.Lib.Device;
using System;

namespace Dzaba.PhoneCleaner.Lib.Handlers.Options
{
    internal abstract class OptionHandlerBase<T> : IOptionHandler
        where T : Option
    {
        public Type ModelType => typeof(T);

        public bool IsOk(Option model, IDeviceConnection deviceConnection, IDeviceSystemInfo systemInfo)
        {
            return IsOk((T)model, deviceConnection, systemInfo);
        }

        protected abstract bool IsOk(T model, IDeviceConnection deviceConnection, IDeviceSystemInfo systemInfo);
    }
}
