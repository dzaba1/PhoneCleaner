using System;
using System.Collections.Generic;
using System.IO;

namespace Dzaba.PhoneCleaner.Lib
{
    public interface IDeviceConnection : IDisposable
    {
        void DeleteFile(string path);
        void DeleteDirectory(string path, bool recursive);
        void CopyFile(string source, Stream dest);
        IEnumerable<string> EnumerateFiles(string path, SearchOption searchOption);
        IEnumerable<string> EnumerateDirectories(string path, SearchOption searchOption);
    }
}
