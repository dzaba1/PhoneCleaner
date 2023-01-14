using System;
using System.Xml.Serialization;
using Dzaba.PhoneCleaner.Lib.Config.Options;

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
