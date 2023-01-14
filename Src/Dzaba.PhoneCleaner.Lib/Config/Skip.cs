using System;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    public class Skip : Option
    {
        public TimeSpan? NewerThan { get; set; }
    }
}
