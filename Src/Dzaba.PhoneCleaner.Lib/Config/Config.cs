using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Dzaba.PhoneCleaner.Lib.Config
{
    [Serializable]
    [XmlRoot]
    public sealed class Config : IReadOnlyList<Action>
    {
        [XmlElement(Type = typeof(Copy))]
        [XmlElement(Type = typeof(Remove))]
        public Action[] Actions { get; set; }

        [XmlIgnore]
        public Action this[int index] => Actions[index];

        [XmlIgnore]
        public int Count => Actions.Length;

        public IEnumerator<Action> GetEnumerator()
        {
            return ((IEnumerable<Action>)Actions).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Actions.GetEnumerator();
        }
    }
}
