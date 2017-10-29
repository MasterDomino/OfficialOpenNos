using System;
using System.Xml.Serialization;

namespace OpenNos.XMLModel.Events
{
    [Serializable]
    public class SummonMonster
    {
        #region Properties

        [XmlAttribute]
        public bool IsBoss { get; set; }

        [XmlAttribute]
        public bool IsHostile { get; set; }

        [XmlAttribute]
        public bool IsTarget { get; set; }

        [XmlAttribute]
        public bool Move { get; set; }

        [XmlElement]
        public OnDeath OnDeath { get; set; }

        [XmlAttribute]
        public short PositionX { get; set; }

        [XmlAttribute]
        public short PositionY { get; set; }

        [XmlElement]
        public Roam Roam { get; set; }

        [XmlAttribute]
        public int VNum { get; set; }

        #endregion
    }
}