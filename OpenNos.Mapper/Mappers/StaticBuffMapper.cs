using OpenNos.DAL.EF;
using OpenNos.Data;
using System;

namespace OpenNos.Mapper.Mappers
{
    public class StaticBuffMapper
    {
        public StaticBuffMapper()
        {

        }

        public void ToStaticBuffDTO(StaticBuff input, StaticBuffDTO output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.CardId = input.CardId;
            output.CharacterId = input.CharacterId;
            output.RemainingTime = input.RemainingTime;
            output.StaticBuffId = input.StaticBuffId;
        }

        public void ToStaticBuff(StaticBuffDTO input, StaticBuff output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.CardId = input.CardId;
            output.CharacterId = input.CharacterId;
            output.RemainingTime = input.RemainingTime;
            output.StaticBuffId = input.StaticBuffId;
        }
    }
}
