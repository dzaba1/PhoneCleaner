namespace Dzaba.PhoneCleaner.Lib
{
    public sealed class CleanData
    {
        public string WorkingDir { get; set; }
        public bool TestOnly { get; set; }
        public string ConfigFilepath { get; set; }
        public string DeviceName { get; set; }
    }
}
