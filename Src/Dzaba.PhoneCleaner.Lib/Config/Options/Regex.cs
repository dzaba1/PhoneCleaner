using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config.Options
{
    public class Regex : Option
    {
        [XmlAttribute]
        public string Pattern { get; set; }

        [XmlAttribute]
        public System.Text.RegularExpressions.RegexOptions RegexOptions { get; set; } = System.Text.RegularExpressions.RegexOptions.None;
    }
}
