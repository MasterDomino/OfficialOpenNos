using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class QuestProgressMapper
    {
        public QuestProgressMapper()
        {
        }

        public void ToQuestProgressDTO(QuestProgress input, QuestProgressDTO output)
        {
            output.CharacterId = input.CharacterId;
            output.IsFinished = input.IsFinished;
            output.QuestData = input.QuestData;
            output.QuestId = input.QuestId;
            output.QuestProgressId = input.QuestProgressId;
        }

        public void ToQuestProgress(QuestProgressDTO input, QuestProgress output)
        {
            output.CharacterId = input.CharacterId;
            output.IsFinished = input.IsFinished;
            output.QuestData = input.QuestData;
            output.QuestId = input.QuestId;
            output.QuestProgressId = input.QuestProgressId;
        }
    }
}

