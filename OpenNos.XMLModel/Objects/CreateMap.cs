﻿using OpenNos.XMLModel.Events;
using System;
using System.Xml.Serialization;

namespace OpenNos.XMLModel.Objects
{
    [Serializable]
    public class CreateMap
    {
        #region Properties

        [XmlAttribute]
        public byte IndexX { get; set; }

        [XmlAttribute]
        public byte IndexY { get; set; }

        [XmlAttribute]
        public int Map { get; set; }

        [XmlElement]
        public OnMoveOnMap[] OnMoveOnMap { get; set; }

        [XmlElement]
        public SummonMonster[] SummonMonster { get; set; }

        [XmlAttribute]
        public short VNum { get; set; }

        #endregion
    }
}