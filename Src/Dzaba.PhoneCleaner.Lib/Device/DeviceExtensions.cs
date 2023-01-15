using Dzaba.PhoneCleaner.Utils;
using System;
using System.IO;
using System.Linq;

namespace Dzaba.PhoneCleaner.Lib.Device
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

        public static void CopyFile(this IDeviceConnection deviceConnection, string source, string dest, bool overwrite)
        {
            Require.NotNull(deviceConnection, nameof(deviceConnection));
            Require.NotWhiteSpace(source, nameof(source));
            Require.NotWhiteSpace(dest, nameof(dest));

            var mode = overwrite ? FileMode.Create : FileMode.CreateNew;

            using (var fs = new FileStream(dest, mode, FileAccess.Write, FileShare.None))
            {
                deviceConnection.CopyFile(source, fs);
            }
        }
    }
}
