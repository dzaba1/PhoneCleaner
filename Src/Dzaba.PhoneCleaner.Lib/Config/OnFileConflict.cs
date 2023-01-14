using System;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    public enum OnFileConflict
    {
        RaiseError,
        DoNothing,
        KeepBoth,
        Overwrite
    }
}
