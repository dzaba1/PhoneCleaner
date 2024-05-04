using System;
using System.Collections.Generic;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib.Device
{
    public interface IDeviceConnection : IDisposable
    {
        void DeleteFile(string path);
        void DeleteDirectory(string path, bool recursive);
        void CopyFile(string source, Stream dest);
        IEnumerable<IDeviceFileInfo> EnumerateFiles(string path, SearchOption searchOption);
        IEnumerable<IDeviceDirectoryInfo> EnumerateDirectories(string path, SearchOption searchOption);
        IEnumerable<string> EnumerableDrives();
        bool DirectoryExists(string path);
        bool FileExists(string path);
        IDeviceFileInfo GetFileInfo(string path);
        IDeviceDirectoryInfo GetDirectoryInfo(string path);
        void Reconnect();
    }
}
