using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class QuestMapper
    {
        public QuestMapper()
        {
        }

        public void ToQuestDTO(Quest input, QuestDTO output)
        {
            output.QuestData = input.QuestData;
            output.QuestId = input.QuestId;
        }

        public void ToQuest(QuestDTO input, Quest output)
        {
            output.QuestData = input.QuestData;
            output.QuestId = input.QuestId;
        }
    }
}

