using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config.Options
{
    [Serializable]
    public class Option
    {
        [XmlAttribute]
        public IOItemType ItemType { get; set; } = IOItemType.Both;
    }
}
