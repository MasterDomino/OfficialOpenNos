using System;
using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class MinigameLogMapper
    {
        public MinigameLogMapper()
        {
        }

        public bool ToMinigameLogDTO(MinigameLog input, MinigameLogDTO output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }

            output.CharacterId = input.CharacterId;
            output.EndTime = input.EndTime;
            output.Minigame = input.Minigame;
            output.MinigameLogId = input.MinigameLogId;
            output.Score = input.Score;
            output.StartTime = input.StartTime;
            return true;
        }

        public bool ToMinigameLog(MinigameLogDTO input, MinigameLog output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }

            output.CharacterId = input.CharacterId;
            output.EndTime = input.EndTime;
            output.Minigame = input.Minigame;
            output.MinigameLogId = input.MinigameLogId;
            output.Score = input.Score;
            output.StartTime = input.StartTime;
            return true;
        }
    }
}
