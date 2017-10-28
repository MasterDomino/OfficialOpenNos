using System;
using System.Xml.Serialization;

namespace OpenNos.XMLModel.Objects
{
    [Serializable]
    public class StartPosition
    {
        #region Properties

        [XmlAttribute]
        public int Value { get; set; }

        #endregion
    }
}