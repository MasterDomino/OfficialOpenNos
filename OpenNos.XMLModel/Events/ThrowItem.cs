using System;
using System.Xml.Serialization;

namespace OpenNos.XMLModel.Events
{
    [Serializable]
    public class ThrowItem
    {
        #region Properties

        [XmlAttribute]
        public int MaxAmount { get; set; }

        [XmlAttribute]
        public int MinAmount { get; set; }

        [XmlAttribute]
        public int PackAmount { get; set; }

        [XmlAttribute]
        public int VNum { get; set; }

        #endregion
    }
}