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
            output.CharacterId = input.CharacterId;
            output.Id = input.Id;
            output.SkillVNum = input.SkillVNum;
        }

        public void ToCharacterSkill(CharacterSkillDTO input, CharacterSkill output)
        {
            output.CharacterId = input.CharacterId;
            output.Id = input.Id;
            output.SkillVNum = input.SkillVNum;
        }
    }
}
