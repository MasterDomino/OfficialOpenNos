using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class CharacterRelationMapper
    {
        public CharacterRelationMapper()
        {

        }

        public void ToCharacterRelationDTO(CharacterRelation input, CharacterRelationDTO output)
        {
            output.CharacterId = input.CharacterId;
            output.CharacterRelationId = input.CharacterRelationId;
            output.RelatedCharacterId = input.RelatedCharacterId;
            output.RelationType = input.RelationType;
        }

        public void ToCharacterRelation(CharacterRelationDTO input, CharacterRelation output)
        {
            output.CharacterId = input.CharacterId;
            output.CharacterRelationId = input.CharacterRelationId;
            output.RelatedCharacterId = input.RelatedCharacterId;
            output.RelationType = input.RelationType;
        }
    }
}
