using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.Data
{
    [Serializable]
    public class QuestProgressDTO : MappingBaseDTO
    {
        public long QuestProgressId { get; set; }

        public long QuestId { get; set; }

        public string QuestData { get; set; }

        public long CharacterId { get; set; }

        public bool IsFinished { get; set; }

    }
}
