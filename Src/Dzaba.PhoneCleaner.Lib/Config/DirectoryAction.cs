using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    public class DirectoryAction : Action
    {
        [XmlAttribute]
        public string Path { get; set; }

        [XmlAttribute]
        public bool Content { get; set; }

        [XmlAttribute]
        public bool ContentRecursive { get; set; }
    }
}
