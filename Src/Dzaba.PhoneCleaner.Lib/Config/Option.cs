using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    public class Option
    {
        [XmlAttribute]
        public IOItemType ItemType { get; set; } = IOItemType.Both;
    }
}
