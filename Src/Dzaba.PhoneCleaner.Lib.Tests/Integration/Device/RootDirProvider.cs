using Dzaba.PhoneCleaner.Utils;
using System;

namespace Dzaba.PhoneCleaner.Lib.Tests.Integration.Device
{
    public interface IRootDirProvider
    {
        string GetRootDir();
    }

    internal sealed class RootDirProvider : IRootDirProvider
    {
        private readonly Func<string> rootDir;

        public RootDirProvider(Func<string> rootDir)
        {
            Require.NotNull(rootDir, nameof(rootDir));

            this.rootDir = rootDir;
        }

        public string GetRootDir()
        {
            return rootDir();
        }
    }
}
