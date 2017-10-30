using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.XMLModel.Objects.Quest
{
    [Serializable]
    public class WalkObjective
    {
        public short MapId { get; set; }

        public short MapX { get; set; }

        public short MapY { get; set; }

        public bool Finished { get; set; }

        public bool HiddenGoal { get; set; }
    }
}
