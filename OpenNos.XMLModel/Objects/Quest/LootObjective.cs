using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.XMLModel.Objects.Quest
{
    [Serializable]
    public class LootObjective
    {
        public int CurrentAmount { get; set; }

        public int GoalAmount { get; set; }

        public short ItemVNum { get; set; }

        public short Chance { get; set; }

        public short[] DroppedByMonsterVNum { get; set; }
    }
}
