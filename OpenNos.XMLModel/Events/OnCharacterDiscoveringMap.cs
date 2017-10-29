using System;
using System.Xml.Serialization;

namespace OpenNos.XMLModel.Events
{
    [Serializable]
    public class OnCharacterDiscoveringMap
    {
        #region Properties

        [XmlElement]
        public NpcDialog NpcDialog { get; set; }

        [XmlElement]
        public OnMoveOnMap OnMoveOnMap { get; set; }

        [XmlElement]
        public SendPacket SendPacket { get; set; }

        [XmlElement]
        public SummonNpc[] SummonNpc { get; set; }

        #endregion
    }
}