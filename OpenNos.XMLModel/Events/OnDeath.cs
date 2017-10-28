using System;
using System.Xml.Serialization;

namespace OpenNos.XMLModel.Events
{
    [Serializable]
    public class OnDeath
    {
        #region Properties

        [XmlElement(IsNullable = false, Order = 2)]
        public End End { get; set; }

        [XmlElement(IsNullable = false, Order = 1)]
        public object RefreshRaidGoals { get; set; }

        [XmlElement(IsNullable = false, Order = 0)]
        public object RemoveMonsterLocker { get; set; }

        [XmlElement(IsNullable = false, Order = 3)]
        public ThrowItem[] ThrowItem { get; set; }

        #endregion
    }
}