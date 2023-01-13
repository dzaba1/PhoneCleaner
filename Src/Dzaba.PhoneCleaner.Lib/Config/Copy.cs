using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    public sealed class Copy : DirectoryAction
    {
        [XmlAttribute]
        public string Destination { get; set; }
    }
}
