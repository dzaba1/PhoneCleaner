using Dzaba.PhoneCleaner.Utils;
using System.IO;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    public interface IConfigReader
    {
        Config Read(string filepath);
    }

    internal sealed class ConfigReader : IConfigReader
    {
        public Config Read(string filepath)
        {
            Require.NotWhiteSpace(filepath, nameof(filepath));

            var serlializer = new XmlSerializer(typeof(Config));
            using var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return (Config)serlializer.Deserialize(fs);
        }
    }
}
