using System;
using System.Xml.Serialization;

namespace OpenNos.XMLModel.Events
{
    [Serializable]
    public class OnDeath
    {
        #region Properties

        [XmlElement(Order = 3)]
        public End End { get; set; }

        [XmlElement(Order = 2)]
        public object RefreshRaidGoals { get; set; }

        [XmlElement(Order = 1)]
        public object RemoveButtonLocker { get; set; }

        [XmlElement(Order = 0)]
        public object RemoveMonsterLocker { get; set; }

        [XmlElement(Order = 4)]
        public ThrowItem[] ThrowItem { get; set; }

        #endregion
    }
}