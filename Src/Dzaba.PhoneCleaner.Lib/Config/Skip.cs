﻿using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    public class Skip : Option
    {
        [XmlElement(ElementName = "NewerThan")]
        public string NewerThanRaw { get; set; }

        [XmlIgnore]
        public TimeSpan? NewerThan
        {
            get
            {
                if (string.IsNullOrWhiteSpace(NewerThanRaw))
                {
                    return null;
                }

                return TimeSpan.Parse(NewerThanRaw);
            }
            set
            {
                if (value == null)
                {
                    NewerThanRaw = null;
                }

                NewerThanRaw = value.ToString();
            }
        }
    }
}
