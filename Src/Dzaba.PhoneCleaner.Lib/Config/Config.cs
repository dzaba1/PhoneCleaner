using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    [XmlRoot]
    public sealed class Config
    {
        [XmlElement]
        public Action[] Actions { get; set; }
    }
}
