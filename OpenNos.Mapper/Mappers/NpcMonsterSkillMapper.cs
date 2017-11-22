using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class NpcMonsterSkillMapper
    {
        public NpcMonsterSkillMapper()
        {
        }

        public void ToNpcMonsterSkillDTO(NpcMonsterSkill input, NpcMonsterSkillDTO output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.NpcMonsterSkillId = input.NpcMonsterSkillId;
            output.NpcMonsterVNum = input.NpcMonsterVNum;
            output.Rate = input.Rate;
            output.SkillVNum = input.SkillVNum;
        }

        public void ToNpcMonsterSkill(NpcMonsterSkillDTO input, NpcMonsterSkill output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.NpcMonsterSkillId = input.NpcMonsterSkillId;
            output.NpcMonsterVNum = input.NpcMonsterVNum;
            output.Rate = input.Rate;
            output.SkillVNum = input.SkillVNum;
        }
    }
}
