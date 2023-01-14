using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config.Options
{
    [Serializable]
    public class Take : Option
    {
        [XmlAttribute(AttributeName = "OlderThan")]
        public string OlderThanRaw { get; set; }

        [XmlIgnore]
        public TimeSpan? OlderThan
        {
            get
            {
                if (string.IsNullOrWhiteSpace(OlderThanRaw))
                {
                    return null;
                }

                return TimeSpan.Parse(OlderThanRaw);
            }
            set
            {
                if (value == null)
                {
                    OlderThanRaw = null;
                }

                OlderThanRaw = value.ToString();
            }
        }
    }
}
