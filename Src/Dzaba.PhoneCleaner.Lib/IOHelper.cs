using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Config.Options;
using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Lib.Handlers.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dzaba.PhoneCleaner.Lib
{
    public interface IIOHelper
    {
        bool TryCopyFile(IDeviceFileInfo file, string targetFilePath,
            IDeviceConnection deviceConnection, OnFileConflict onFileConflict);

        IReadOnlyList<IDeviceFileInfo> EnumerateFiles(IDeviceConnection deviceConnection, IDeviceDirectoryInfo dir,
            IEnumerable<Option> options);

        IReadOnlyList<IDeviceDirectoryInfo> EnumerateDirectories(IDeviceConnection deviceConnection, IDeviceDirectoryInfo dir,
            IEnumerable<Option> options);
    }

    internal sealed class IOHelper : IIOHelper
    {
        private readonly ILogger<IOHelper> logger;
        private readonly IOptionsEvaluator optionsEvaluator;

        public IOHelper(ILogger<IOHelper> logger,
            IOptionsEvaluator optionsEvaluator)
        {
            Require.NotNull(logger, nameof(logger));
            Require.NotNull(optionsEvaluator, nameof(optionsEvaluator));

            this.logger = logger;
            this.optionsEvaluator = optionsEvaluator;
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

        public IReadOnlyList<IDeviceFileInfo> EnumerateFiles(IDeviceConnection deviceConnection, IDeviceDirectoryInfo dir,
            IEnumerable<Option> options)
        {
            Require.NotNull(deviceConnection, nameof(deviceConnection));
            Require.NotNull(dir, nameof(dir));

            return deviceConnection.EnumerateFiles(dir.FullName, SearchOption.TopDirectoryOnly)
                .Where(f => optionsEvaluator.IsOk(options, deviceConnection, f))
                .ToArray();
        }

        public IReadOnlyList<IDeviceDirectoryInfo> EnumerateDirectories(IDeviceConnection deviceConnection, IDeviceDirectoryInfo dir,
            IEnumerable<Option> options)
        {
            Require.NotNull(deviceConnection, nameof(deviceConnection));
            Require.NotNull(dir, nameof(dir));

            return deviceConnection.EnumerateDirectories(dir.FullName, SearchOption.TopDirectoryOnly)
                    .Where(d => optionsEvaluator.IsOk(options, deviceConnection, d))
                    .ToArray();
        }
    }
}
