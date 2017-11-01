using System;

namespace OpenNos.Data
{
    [Serializable]
    public class QuestDTO : MappingBaseDTO
    {
        public long QuestId { get; set; }

        public string QuestData { get; set; }
    }
}
