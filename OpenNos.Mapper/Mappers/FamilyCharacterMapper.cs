using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class FamilyCharacterMapper
    {
        public FamilyCharacterMapper()
        {

        }

        public void ToFamilyCharacterDTO(FamilyCharacter input, FamilyCharacterDTO output)
        {
            output.Authority = input.Authority;
            output.CharacterId = input.CharacterId;
            output.DailyMessage = input.DailyMessage;
            output.Experience = input.Experience;
            output.FamilyCharacterId = input.FamilyCharacterId;
            output.FamilyId = input.FamilyId;
            output.Rank = input.Rank;
        }

        public void ToFamilyCharacter(FamilyCharacterDTO input, FamilyCharacter output)
        {
            output.Authority = input.Authority;
            output.CharacterId = input.CharacterId;
            output.DailyMessage = input.DailyMessage;
            output.Experience = input.Experience;
            output.FamilyCharacterId = input.FamilyCharacterId;
            output.FamilyId = input.FamilyId;
            output.Rank = input.Rank;
        }
    }
}
