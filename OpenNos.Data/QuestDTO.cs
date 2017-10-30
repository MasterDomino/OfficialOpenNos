using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.Data
{
    public class QuestDTO : MappingBaseDTO
    {
        public long QuestId { get; set; }

        public string QuestData { get; set; }
    }
}
