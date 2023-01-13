using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    public sealed class Copy : Action
    {
        [XmlAttribute]
        public string Path { get; set; }

        [XmlAttribute]
        public string Destination { get; set; }

        [XmlAttribute]
        public bool Recursive { get; set; }

        [XmlAttribute]
        public bool Override { get; set; }
    }
}
