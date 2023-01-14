using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config.Options
{
    [Serializable]
    public class Skip : Option
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
    }
}
