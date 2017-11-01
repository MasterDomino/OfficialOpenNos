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

        [XmlElement(Order = 4)]
        public SummonMonster[] SummonMonster { get; set; }

        [XmlElement(Order = 3)]
        public Teleport Teleport { get; set; }

        [XmlElement(Order = 2)]
        public SendMessage SendMessage { get; set; }

        [XmlElement(Order = 5)]
        public OnMapClean OnMapClean { get; set; }

        #endregion
    }
}