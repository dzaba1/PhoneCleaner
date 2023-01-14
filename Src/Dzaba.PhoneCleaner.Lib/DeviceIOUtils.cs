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
    }
}
