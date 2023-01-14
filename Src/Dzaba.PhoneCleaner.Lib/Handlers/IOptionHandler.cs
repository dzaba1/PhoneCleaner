using Dzaba.PhoneCleaner.Lib.Config;
using System;

namespace Dzaba.PhoneCleaner.Lib.Handlers
{
    public interface IOptionHandler
    {
        Type ModelType { get; }
        bool IsOk(Option model, IDeviceConnection deviceConnection, string path, bool isDirectory);
    }
}
