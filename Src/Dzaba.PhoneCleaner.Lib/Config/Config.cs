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
        [XmlElement(Type = typeof(RemoveDirectory))]
        [XmlElement(Type = typeof(Move))]
        public Action[] Actions { get; set; }
    }
}
