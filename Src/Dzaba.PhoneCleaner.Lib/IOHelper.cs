using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Device;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib
{
    public interface IIOHelper
    {
        bool TryCopyFile(IDeviceFileInfo file, string targetFilePath,
            IDeviceConnection deviceConnection, OnFileConflict onFileConflict);
    }

    internal sealed class IOHelper : IIOHelper
    {
        private readonly ILogger<IOHelper> logger;

        public IOHelper(ILogger<IOHelper> logger)
        {
            Require.NotNull(logger, nameof(logger));

            this.logger = logger;
        }

        public bool TryCopyFile(IDeviceFileInfo file, string targetFilePath, IDeviceConnection deviceConnection, OnFileConflict onFileConflict)
        {
            Require.NotNull(file, nameof(file));
            Require.NotWhiteSpace(targetFilePath, nameof(targetFilePath));
            Require.NotNull(deviceConnection, nameof(deviceConnection));

            var currentTargetFilePath = targetFilePath;
            var overwrite = false;

            if (File.Exists(currentTargetFilePath))
            {
                switch (onFileConflict)
                {
                    default: throw new ArgumentOutOfRangeException($"Unknown {nameof(OnFileConflict)} value: {onFileConflict}");
                    case OnFileConflict.RaiseError:
                        throw new IOException($"The file '{currentTargetFilePath}' already exists.");
                    case OnFileConflict.DoNothing:
                        return false;
                    case OnFileConflict.Overwrite:
                        overwrite = true;
                        break;
                    case OnFileConflict.KeepBoth:
                        currentTargetFilePath = DeviceIOUtils.GetNewTargetFileName(currentTargetFilePath);
                        break;
                }
            }

            logger.LogInformation("Copy file '{Path}' to '{Destination}'.", file, currentTargetFilePath);
            deviceConnection.CopyFile(file.FullName, currentTargetFilePath, overwrite);
            return true;
        }
    }
}
