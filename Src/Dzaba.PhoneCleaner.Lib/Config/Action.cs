using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    public class Action
    {
        [XmlElement]
        public Action[] Children { get; set; }
    }
}
