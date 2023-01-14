using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    [XmlRoot]
    public sealed class Config
    {
        [XmlElement(Type = typeof(Copy))]
        [XmlElement(Type = typeof(Remove))]
        public Action[] Actions { get; set; }
    }
}
