using Dzaba.PhoneCleaner.Lib.Config;
using Dzaba.PhoneCleaner.Lib.Config.Options;
using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Lib.Handlers.Options;
using Dzaba.PhoneCleaner.Utils;
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
            string tempCopyFilepath = null;

            if (File.Exists(currentTargetFilePath))
            {
                switch (onFileConflict)
                {
                    default: throw new ArgumentOutOfRangeException($"Unknown {nameof(OnFileConflict)} value: {onFileConflict}");
                    case OnFileConflict.RaiseError:
                        throw new IOException($"The file '{currentTargetFilePath}' already exists.");
                    case OnFileConflict.DoNothing:
                        logger.LogInformation("The file '{Path}' already exists. Skipping copy.", currentTargetFilePath);
                        return false;
                    case OnFileConflict.Overwrite:
                        overwrite = true;
                        tempCopyFilepath = MakeTempCopy(currentTargetFilePath);
                        break;
                    case OnFileConflict.KeepBoth:
                        currentTargetFilePath = DeviceIOUtils.GetNewTargetFileName(currentTargetFilePath);
                        break;
                }
            }

            logger.LogInformation("Copy file '{Path}' to '{Destination}'.", file.FullName, currentTargetFilePath);

            try
            {
                deviceConnection.CopyFile(file.FullName, currentTargetFilePath, overwrite);
                return true;
            }
            catch
            {
                RestoreOrCleanFile(currentTargetFilePath, tempCopyFilepath);
                throw;
            }
            finally
            {
                ClearTempCopy(tempCopyFilepath);
            }
        }

        private void RestoreOrCleanFile(string targetFilePath, string tempCopyFilepath)
        {
            if (File.Exists(targetFilePath))
            {
                File.Delete(targetFilePath);
            }

            if (!string.IsNullOrWhiteSpace(tempCopyFilepath))
            {
                using (var fsSource = new FileStream(tempCopyFilepath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var fsTarget = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        fsSource.CopyTo(fsTarget);
                    }
                }
            }
        }

        private void ClearTempCopy(string tempCopyFilepath)
        {
            if (!string.IsNullOrWhiteSpace(tempCopyFilepath) && File.Exists(tempCopyFilepath))
            {
                File.Delete(tempCopyFilepath);
            }
        }

        private string MakeTempCopy(string filepath)
        {
            var dir = Path.Combine(Path.GetTempPath(), "DzabaPhoneCleaner");
            var filename = Path.GetFileName(filepath);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var tempFilepath = Path.Combine(dir, filename);

            using (var fsSource = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var fsTarget = new FileStream(tempFilepath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    fsSource.CopyTo(fsTarget);
                }
            }

            return tempFilepath;
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
