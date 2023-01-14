namespace Dzaba.PhoneCleaner.Lib
{
    public interface IDeviceConnectionFactory
    {
        IDeviceConnection Create(string deviceName);
    }
}
