﻿using System;
using System.Xml.Serialization;

namespace OpenNos.XMLModel.Objects
{
    [Serializable]
    public class Id
    {
        #region Properties

        [XmlAttribute]
        public int Value { get; set; }

        #endregion
    }
}