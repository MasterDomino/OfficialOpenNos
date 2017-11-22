using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class QuicklistEntryMapper
    {
        public QuicklistEntryMapper()
        {
        }

        public void ToQuicklistEntryDTO(QuicklistEntry input, QuicklistEntryDTO output)
        {
            output.CharacterId = input.CharacterId;
            output.Id = input.Id;
            output.Morph = input.Morph;
            output.Pos = input.Pos;
            output.Q1 = input.Q1;
            output.Q2 = input.Q2;
            output.Slot = input.Slot;
            output.Type = input.Type;
        }

        public void ToQuicklistEntry(QuicklistEntryDTO input, QuicklistEntry output)
        {
            output.CharacterId = input.CharacterId;
            output.Id = input.Id;
            output.Morph = input.Morph;
            output.Pos = input.Pos;
            output.Q1 = input.Q1;
            output.Q2 = input.Q2;
            output.Slot = input.Slot;
            output.Type = input.Type;
        }
    }
}
