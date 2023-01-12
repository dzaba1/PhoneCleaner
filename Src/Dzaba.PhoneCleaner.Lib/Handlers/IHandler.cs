using Dzaba.PhoneCleaner.Lib.Config;

namespace Dzaba.PhoneCleaner.Lib.Handlers
{
    public interface IHandler
    {
        int Handle(Action model, IDeviceConnection deviceConnection);
    }
}
