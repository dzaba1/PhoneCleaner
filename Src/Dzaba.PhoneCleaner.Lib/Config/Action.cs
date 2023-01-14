﻿using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    public class Action
    {
        [XmlElement(Type = typeof(Skip))]
        public Option[] Options { get; set; }
    }
}
