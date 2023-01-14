using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    public class RemoveDirectory : Action
    {
        [XmlAttribute]
        public string Path { get; set; }
    }
}
