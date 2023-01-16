using System;
using System.Xml.Serialization;
using Dzaba.PhoneCleaner.Lib.Config.Options;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    public class Action
    {
        [XmlElement(Type = typeof(Skip))]
        [XmlElement(Type = typeof(Take))]
        [XmlElement(Type = typeof(Regex))]
        public Option[] Options { get; set; }

        [XmlAttribute]
        public int DriveIndex { get; set; } = 0;
    }
}
