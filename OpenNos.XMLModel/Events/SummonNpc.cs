﻿using System;
using System.Xml.Serialization;

namespace OpenNos.XMLModel.Events
{
    [Serializable]
    public class SummonNpc
    {
        #region Properties

        [XmlAttribute]
        public bool IsMate { get; set; }

        [XmlAttribute]
        public short IsProtected { get; set; }

        [XmlElement]
        public OnDeath OnDeath { get; set; }

        [XmlAttribute]
        public short PositionX { get; set; }

        [XmlAttribute]
        public short PositionY { get; set; }

        [XmlAttribute]
        public short VNum { get; set; }

        #endregion
    }
}