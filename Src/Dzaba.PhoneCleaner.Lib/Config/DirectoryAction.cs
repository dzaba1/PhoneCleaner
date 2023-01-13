using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    public class DirectoryAction : Action
    {
        [XmlAttribute]
        public bool Content { get; set; }

        [XmlAttribute]
        public bool ContentRecursive { get; set; }
    }
}
