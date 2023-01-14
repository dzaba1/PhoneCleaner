using System;
using Dzaba.PhoneCleaner.Lib.Device;

namespace Dzaba.PhoneCleaner.Lib.Handlers
{
    public interface IHandler
    {
        Type ModelType { get; }
        int Handle(Config.Action model, IDeviceConnection deviceConnection, CleanData cleanData);
    }
}
