using System;
using System.Linq;

namespace Dzaba.PhoneCleaner.Lib
{
    public static class DeviceExtensions
    {
        public static string GetRootOrThrow(this IDeviceConnection deviceConnection, int driveIndex)
        {
            Require.NotNull(deviceConnection, nameof(deviceConnection));

            var root = deviceConnection.EnumerableDrives().ElementAtOrDefault(driveIndex);
            if (string.IsNullOrWhiteSpace(root))
            {
                throw new InvalidOperationException($"Can't find the root directory and drive {driveIndex}.");
            }

            return root;
        }
    }
}
