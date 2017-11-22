using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class ShopSkillMapper
    {
        public ShopSkillMapper()
        {
        }

        public void ToShopSkillDTO(ShopSkill input, ShopSkillDTO output)
        {
            output.ShopId = input.ShopId;
            output.ShopSkillId = input.ShopSkillId;
            output.SkillVNum = input.SkillVNum;
            output.Slot = input.Slot;
            output.Type = input.Type;
        }

        public void ToShopSkill(ShopSkillDTO input, ShopSkill output)
        {
            output.ShopId = input.ShopId;
            output.ShopSkillId = input.ShopSkillId;
            output.SkillVNum = input.SkillVNum;
            output.Slot = input.Slot;
            output.Type = input.Type;
        }
    }
}

