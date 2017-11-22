using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class CharacterSkillMapper
    {
        public CharacterSkillMapper()
        {

        }

        public void ToCharacterSkillDTO(CharacterSkill input, CharacterSkillDTO output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.CharacterId = input.CharacterId;
            output.Id = input.Id;
            output.SkillVNum = input.SkillVNum;
        }

        public void ToCharacterSkill(CharacterSkillDTO input, CharacterSkill output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.CharacterId = input.CharacterId;
            output.Id = input.Id;
            output.SkillVNum = input.SkillVNum;
        }
    }
}
