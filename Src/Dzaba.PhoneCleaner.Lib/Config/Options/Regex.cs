using System;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config.Options
{
    [Serializable]
    public sealed class Regex : Option
    {
        [NonSerialized]
        [XmlIgnore]
        private System.Text.RegularExpressions.Regex _regexInstance;

        [NonSerialized]
        [XmlIgnore]
        private string _pattern;

        [NonSerialized]
        [XmlIgnore]
        private System.Text.RegularExpressions.RegexOptions _regexOptions = System.Text.RegularExpressions.RegexOptions.None;

        [XmlAttribute]
        public string Pattern
        {
            get => _pattern;
            set
            {
                _pattern = value;
                _regexInstance = null;
            }
        }

        [XmlAttribute]
        public System.Text.RegularExpressions.RegexOptions RegexOptions
        {
            get => _regexOptions;
            set
            {
                _regexOptions = value;
                _regexInstance = null;
            }
        }

        [XmlIgnore]
        public System.Text.RegularExpressions.Regex RegexInstance
        {
            get
            {
                if (_regexInstance == null)
                {
                    _regexInstance = new System.Text.RegularExpressions.Regex(Pattern, RegexOptions);
                }

                return _regexInstance;
            }
        }
    }
}
