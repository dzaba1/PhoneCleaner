using System;
using System.IO;
using System.Linq;

namespace Dzaba.PhoneCleaner.Lib
{
    public static class DeviceIOUtils
    {
        public static string GetFileOrDirectoryName(string path)
        {
            Require.NotWhiteSpace(path, nameof(path));

            var split = path.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return split.Last();
        }
    }
}
