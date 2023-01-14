using Dzaba.PhoneCleaner.Lib.Config;
using System;

namespace Dzaba.PhoneCleaner.Lib.Handlers
{
    internal abstract class OptionHandlerBase<T> : IOptionHandler
        where T : Option
    {
        public Type ModelType => typeof(T);

        public bool IsOk(Option model, IDeviceConnection deviceConnection, string path, bool isDirectory)
        {
            return IsOk((T)model, deviceConnection, path, isDirectory);
        }

        protected abstract bool IsOk(T model, IDeviceConnection deviceConnection, string path, bool isDirectory);
    }
}
