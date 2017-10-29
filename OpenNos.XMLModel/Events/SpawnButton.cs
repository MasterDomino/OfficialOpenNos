using System;
using System.Xml.Serialization;

namespace OpenNos.XMLModel.Events
{
    [Serializable]
    public class SpawnButton
    {
        #region Properties

        [XmlAttribute]
        public int Id { get; set; }

        [XmlElement(Order = 1, IsNullable = false)]
        public OnDisable OnDisable { get; set; }

        [XmlElement(Order = 0, IsNullable = false)]
        public OnEnable OnEnable { get; set; }

        [XmlAttribute]
        public short PositionX { get; set; }

        [XmlAttribute]
        public short PositionY { get; set; }

        [XmlAttribute]
        public short VNumDisabled { get; set; }

        [XmlAttribute]
        public short VNumEnabled { get; set; }

        #endregion
    }
}