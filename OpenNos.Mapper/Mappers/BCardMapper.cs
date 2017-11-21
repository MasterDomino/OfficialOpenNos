using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class BCardMapper
    {
        public BCardMapper()
        {

        }

        public void ToBCardDTO(BCard input, BCardDTO output)
        {
            output.BCardId = input.BCardId;
            output.CardId = input.CardId;
            output.CastType = input.CastType;
            output.FirstData = input.FirstData;
            output.IsLevelDivided = input.IsLevelDivided;
            output.IsLevelScaled = input.IsLevelScaled;
            output.ItemVNum = input.ItemVNum;
            output.NpcMonsterVNum = input.NpcMonsterVNum;
            output.SecondData = input.SecondData;
            output.SkillVNum = input.SkillVNum;
            output.SubType = input.SubType;
            output.ThirdData = input.ThirdData;
            output.Type = input.Type;
        }

        public void ToBCard(BCardDTO input, BCard output)
        {
            output.BCardId = input.BCardId;
            output.CardId = input.CardId;
            output.CastType = input.CastType;
            output.FirstData = input.FirstData;
            output.IsLevelDivided = input.IsLevelDivided;
            output.IsLevelScaled = input.IsLevelScaled;
            output.ItemVNum = input.ItemVNum;
            output.NpcMonsterVNum = input.NpcMonsterVNum;
            output.SecondData = input.SecondData;
            output.SkillVNum = input.SkillVNum;
            output.SubType = input.SubType;
            output.ThirdData = input.ThirdData;
            output.Type = input.Type;
        }
    }
}
