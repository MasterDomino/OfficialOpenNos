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
            if (input == null)
            {
                output = null;
                return;
            }
            output.QuestData = input.QuestData;
            output.QuestId = input.QuestId;
        }

        public void ToQuest(QuestDTO input, Quest output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.QuestData = input.QuestData;
            output.QuestId = input.QuestId;
        }
    }
}

