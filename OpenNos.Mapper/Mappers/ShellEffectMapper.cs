using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class ShellEffectMapper
    {
        public ShellEffectMapper()
        {
        }

        public void ToShellEffectDTO(ShellEffect input, ShellEffectDTO output)
        {
            output.Effect = input.Effect;
            output.EffectLevel = input.EffectLevel;
            output.EquipmentSerialId = input.EquipmentSerialId;
            output.ShellEffectId = input.ShellEffectId;
            output.Value = input.Value;
        }

        public void ToShellEffect(ShellEffectDTO input, ShellEffect output)
        {
            output.Effect = input.Effect;
            output.EffectLevel = input.EffectLevel;
            output.EquipmentSerialId = input.EquipmentSerialId;
            output.ShellEffectId = input.ShellEffectId;
            output.Value = input.Value;
        }
    }
}
