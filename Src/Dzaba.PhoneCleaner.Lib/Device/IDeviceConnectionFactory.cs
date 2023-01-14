namespace Dzaba.PhoneCleaner.Lib.Device
{
    public interface IDeviceConnectionFactory
    {
        IDeviceConnection Create(string deviceName, bool testOnly);
    }
}
