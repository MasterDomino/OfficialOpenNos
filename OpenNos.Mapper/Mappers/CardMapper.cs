using OpenNos.DAL.EF;
using OpenNos.Data;
using OpenNos.Domain;

namespace OpenNos.Mapper.Mappers
{
    public class CardMapper
    {
        public CardMapper()
        {

        }

        public void ToCardDTO(Card input, CardDTO output)
        {
            output.BuffType = input.BuffType;
            output.CardId = input.CardId;
            output.Delay = input.Delay;
            output.Duration = input.Duration;
            output.EffectId = input.EffectId;
            output.Level = input.Level;
            output.Name = input.Name;
            output.Propability = input.Propability;
            output.TimeoutBuff = input.TimeoutBuff;
            output.TimeoutBuffChance = input.TimeoutBuffChance;
        }

        public void ToCard(CardDTO input, Card output)
        {
            output.BuffType = input.BuffType;
            output.CardId = input.CardId;
            output.Delay = input.Delay;
            output.Duration = input.Duration;
            output.EffectId = input.EffectId;
            output.Level = input.Level;
            output.Name = input.Name;
            output.Propability = input.Propability;
            output.TimeoutBuff = input.TimeoutBuff;
            output.TimeoutBuffChance = input.TimeoutBuffChance;
        }
    }
}
