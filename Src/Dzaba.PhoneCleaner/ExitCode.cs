namespace Dzaba.PhoneCleaner
{
    public enum ExitCode
    {
        Ok = 0,
        Unknown = 1,
        EmptyWorkingDir,
        MissingConfigFile,
        EmptyDeviceName
    }
}
