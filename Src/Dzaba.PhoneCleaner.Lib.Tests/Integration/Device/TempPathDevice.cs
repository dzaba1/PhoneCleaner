using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dzaba.PhoneCleaner.Lib.Device;
using Dzaba.PhoneCleaner.Utils;

namespace Dzaba.PhoneCleaner.Lib.Tests.Integration.Device
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

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public void Dispose()
        {

        }

        public IEnumerable<string> EnumerableDrives()
        {
            yield return rootPath;
        }

        public IEnumerable<IDeviceDirectoryInfo> EnumerateDirectories(string path, SearchOption searchOption)
        {
            return Directory.EnumerateDirectories(path, "*", searchOption)
                .Select(GetDirectoryInfo);
        }

        public IEnumerable<IDeviceFileInfo> EnumerateFiles(string path, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(path, "*.*", searchOption)
                .Select(GetFileInfo);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public IDeviceDirectoryInfo GetDirectoryInfo(string path)
        {
            var info = new DirectoryInfo(path);
            return new DirectoryInfoWrap(info);
        }

        public IDeviceFileInfo GetFileInfo(string path)
        {
            var info = new FileInfo(path);
            return new FileInfoWrap(info);
        }
    }
}
