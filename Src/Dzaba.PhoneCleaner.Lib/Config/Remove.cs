using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    public sealed class Remove : Action
    {
        [XmlAttribute]
        public string Path { get; set; }

        [XmlAttribute]
        public bool Content { get; set; }

        [XmlAttribute]
        public bool Recursive { get; set; } = true;
    }
}
