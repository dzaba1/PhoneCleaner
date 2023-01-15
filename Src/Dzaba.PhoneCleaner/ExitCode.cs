namespace Dzaba.PhoneCleaner
{
    internal enum ExitCode
    {
        Ok = 0,
        Unknown = 1,
        EmptyWorkingDir,
        MissingConfigFile,
        EmptyDeviceName,
        EmptyConfigFile
    }
}
