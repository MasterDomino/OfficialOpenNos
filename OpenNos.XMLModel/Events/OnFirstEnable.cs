using System;
using System.Xml.Serialization;

namespace OpenNos.XMLModel.Events
{
    [Serializable]
    public class OnFirstEnable
    {
        #region Properties

        [XmlElement(Order = 1)]
        public object RefreshRaidGoals { get; set; }

        [XmlElement(Order = 0)]
        public object RemoveButtonLocker { get; set; }

        [XmlElement]
        public SummonMonster[] SummonMonster { get; set; }

        [XmlElement]
        public Teleport Teleport { get; set; }

        #endregion
    }
}