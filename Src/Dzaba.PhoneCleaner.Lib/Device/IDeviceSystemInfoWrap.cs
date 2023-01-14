using System;

namespace Dzaba.PhoneCleaner.Lib.Device
{
    public interface IDeviceSystemInfoWrap
    {
        string Name { get; }
        string FullName { get; }
        DateTime? CreationTime { get; }
        DateTime? ModificationTime { get; }
    }
}
