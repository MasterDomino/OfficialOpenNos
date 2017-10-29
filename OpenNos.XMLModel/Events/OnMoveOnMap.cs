using OpenNos.XMLModel.Objects;
using System;
using System.Xml.Serialization;

namespace OpenNos.XMLModel.Events
{
    [Serializable]
    public class OnMoveOnMap
    {
        #region Properties

        [XmlElement]
        public OnMapClean OnMapClean { get; set; }

        [XmlElement(Order = 4)]
        public object RefreshRaidGoals { get; set; }

        [XmlElement(Order = 0)]
        public object RemoveButtonLocker { get; set; }

        [XmlElement(Order = 1)]
        public object RemoveMonsterLocker { get; set; }

        [XmlElement(Order = 2)]
        public SetButtonLockers SetButtonLockers { get; set; }

        [XmlElement(Order = 3)]
        public SetMonsterLockers SetMonsterLockers { get; set; }

        [XmlElement]
        public Wave[] Wave { get; set; }

        #endregion
    }
}