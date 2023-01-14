using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config.Options
{
    [Serializable]
    public sealed class Skip : Option
    {
        [XmlAttribute(AttributeName = "NewerThan")]
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
