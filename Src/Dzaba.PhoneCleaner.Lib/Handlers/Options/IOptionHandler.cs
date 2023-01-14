using Dzaba.PhoneCleaner.Lib.Config.Options;
using Dzaba.PhoneCleaner.Lib.Device;
using System;

namespace Dzaba.PhoneCleaner.Lib.Handlers.Options
{
    public interface IOptionHandler
    {
        Type ModelType { get; }
        bool IsOk(Option model, IDeviceConnection deviceConnection, string path, bool isDirectory);
    }
}
