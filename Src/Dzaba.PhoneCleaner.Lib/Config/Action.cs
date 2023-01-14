using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    //[XmlInclude(typeof(Skip))]
    public class Action
    {
        [XmlElement(Type = typeof(Skip))]
        [XmlElement(Type = typeof(Take))]
        //[XmlArray]
        public Option[] Options { get; set; }
    }
}
