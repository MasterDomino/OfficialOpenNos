using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenNos.XMLModel.Events.Quest
{
    [Serializable]
    public class TeleportTo
    {
        [XmlAttribute]
        public short MapId { get; set; }

        [XmlAttribute]
        public short MapX { get; set; }

        [XmlAttribute]
        public short MapY { get; set; }

        [XmlAttribute]
        public bool AskTeleport { get; set; }
    }
}
