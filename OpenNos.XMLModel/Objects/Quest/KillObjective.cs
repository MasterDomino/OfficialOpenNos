using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.XMLModel.Objects.Quest
{
    [Serializable]
    public class KillObjective
    {
        public int CurrentAmount { get; set; }

        public int GoalAmount { get; set; }

        public short MonsterVNum { get; set; }
    }
}
