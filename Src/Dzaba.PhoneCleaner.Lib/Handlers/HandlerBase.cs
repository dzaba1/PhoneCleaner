using Dzaba.PhoneCleaner.Lib.Config;

namespace Dzaba.PhoneCleaner.Lib.Handlers
{
    internal abstract class HandlerBase<T> : IHandler
        where T : Action
    {
        public int Handle(Action model, IDeviceConnection deviceConnection)
        {
            return Handle((T)model, deviceConnection);
        }

        protected abstract int Handle(T model, IDeviceConnection deviceConnection);
    }
}
