using System.IO;
using System.Linq;

namespace Dzaba.PhoneCleaner.Lib
{
    public static class DeviceIOUtils
    {
        public static string GetFileOrDirectoryName(string path)
        {
            Require.NotWhiteSpace(path, nameof(path));

            var split = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrWhiteSpace(p));
            return split.Last();
        }

        public static string GetNewTargetFileName(string targetFilePath)
        {
            Require.NotWhiteSpace(targetFilePath, nameof(targetFilePath));

            var fileInfo = new FileInfo(targetFilePath);
            var nameWithoutExt = Path.GetFileNameWithoutExtension(targetFilePath);

            var i = 0;
            while (true)
            {
                i++;
                var newName = $"{nameWithoutExt}_({i})";
                var full = Path.Combine(fileInfo.DirectoryName, newName + fileInfo.Extension);
                if (!File.Exists(full))
                {
                    return full;
                }
            }
        }
    }
}
