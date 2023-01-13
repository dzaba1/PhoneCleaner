using Dzaba.PhoneCleaner.Lib.Config;

namespace Dzaba.PhoneCleaner.Lib.Handlers
{
    internal abstract class HandlerBase<T> : IHandler
        where T : Action
    {
        public System.Type ModelType => typeof(T);

        public int Handle(Action model, IDeviceConnection deviceConnection, CleanData cleanData)
        {
            return Handle((T)model, deviceConnection, cleanData);
        }

        protected abstract int Handle(T model, IDeviceConnection deviceConnection, CleanData cleanData);
    }
}
