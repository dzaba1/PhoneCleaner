using System.Collections.Generic;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Tests
{
    public sealed class TempPathDevice : IDeviceConnection
    {
        private readonly string rootPath;

        public TempPathDevice(string rootPath)
        {
            Require.NotWhiteSpace(rootPath, nameof(rootPath));

            this.rootPath = rootPath;
        }

        public void CopyFile(string source, Stream dest)
        {
            using (var fs = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fs.CopyTo(dest);
            }
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            Directory.Delete(path, recursive);
        }

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public void Dispose()
        {
            
        }

        public IEnumerable<string> EnumerableDrives()
        {
            yield return rootPath;
        }

        public IEnumerable<string> EnumerateDirectories(string path, SearchOption searchOption)
        {
            return Directory.EnumerateDirectories(path, "*", searchOption);
        }

        public IEnumerable<string> EnumerateFiles(string path, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(path, "*.*", searchOption);
        }
    }
}
