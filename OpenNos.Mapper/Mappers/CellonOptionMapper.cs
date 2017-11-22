
using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class CellonOptionMapper
    {
        public CellonOptionMapper()
        {

        }

        public void ToCellonOptionDTO(CellonOption input, CellonOptionDTO output)
        {
            output.CellonOptionId = input.CellonOptionId;
            output.EquipmentSerialId = input.EquipmentSerialId;
            output.Level = input.Level;
            output.Type = input.Type;
            output.Value = input.Value;
        }

        public void ToCellonOption(CellonOptionDTO input, CellonOption output)
        {
            output.CellonOptionId = input.CellonOptionId;
            output.EquipmentSerialId = input.EquipmentSerialId;
            output.Level = input.Level;
            output.Type = input.Type;
            output.Value = input.Value;
        }
    }
}
