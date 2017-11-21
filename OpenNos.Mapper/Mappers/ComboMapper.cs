using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class ComboMapper
    {
        public ComboMapper()
        {

        }

        public void ToComboDTO(Combo input, ComboDTO output)
        {
            output.Animation = input.Animation;
            output.ComboId = input.ComboId;
            output.Effect = input.Effect;
            output.Hit = input.Hit;
            output.SkillVNum = input.SkillVNum;
        }

        public void ToCombo(ComboDTO input, Combo output)
        {
            output.Animation = input.Animation;
            output.ComboId = input.ComboId;
            output.Effect = input.Effect;
            output.Hit = input.Hit;
            output.SkillVNum = input.SkillVNum;
        }
    }
}
