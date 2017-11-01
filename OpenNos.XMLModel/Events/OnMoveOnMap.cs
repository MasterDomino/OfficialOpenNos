using OpenNos.XMLModel.Objects;
using System;
using System.Xml.Serialization;

namespace OpenNos.XMLModel.Events
{
    [Serializable]
    public class OnMoveOnMap
    {
        #region Properties

        [XmlElement(Order = 7)]
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

        [XmlElement(Order = 5)]
        public StartClock StartClock { get; set; }

        [XmlElement(Order = 6)]
        public StartClock StartMapClock { get; set; }

        [XmlElement(Order = 8)]
        public Wave[] Wave { get; set; }

        #endregion
    }
}