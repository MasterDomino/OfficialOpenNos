using System;
using System.Xml.Serialization;

namespace OpenNos.XMLModel.Events
{
    [Serializable]
    public class OnNoticing
    {
        #region Properties

        // We might want to try using XmlAnyElement as Move and Effect does this stupid loopty loop -_-

        [XmlElement]
        public Effect Effect { get; set; }

        [XmlElement]
        public Move Move { get; set; }

        [XmlAttribute]
        public byte Range { get; set; }

        #endregion
    }
}