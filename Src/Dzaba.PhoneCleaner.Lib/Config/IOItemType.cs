﻿using System;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Flags]
    [Serializable]
    public enum IOItemType
    {
        File = 1,
        Directory = 2,
        Both = File | Directory
    }
}
