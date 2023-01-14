using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    public sealed class Move : Action
    {
        [XmlAttribute]
        public string Path { get; set; }

        [XmlAttribute]
        public string Destination { get; set; }

        [XmlAttribute]
        public bool Recursive { get; set; }

        [XmlAttribute]
        public OnFileConflict OnConflict { get; set; } = OnFileConflict.RaiseError;
    }
}
