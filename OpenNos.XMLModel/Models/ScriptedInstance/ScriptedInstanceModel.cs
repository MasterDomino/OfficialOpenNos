using System.Xml.Serialization;
using OpenNos.XMLModel.Objects;
using System;

namespace OpenNos.XMLModel.Models.ScriptedInstance
{
    [XmlRoot("Definition"), Serializable]
    public class ScriptedInstanceModel
    {
        public Globals Globals { get; set; }

        public InstanceEvent InstanceEvent { get; set; }
    }
}
