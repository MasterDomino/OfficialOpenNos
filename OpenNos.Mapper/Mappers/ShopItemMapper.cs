using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class ShopItemMapper
    {
        public ShopItemMapper()
        {
        }

        public void ToShopItemDTO(ShopItem input, ShopItemDTO output)
        {
            output.Color = input.Color;
            output.ItemVNum = input.ItemVNum;
            output.Rare = (sbyte)input.Rare;
            output.ShopId = input.ShopId;
            output.ShopItemId = input.ShopItemId;
            output.Slot = input.Slot;
            output.Type = input.Type;
            output.Upgrade = input.Upgrade;
        }

        public void ToShopItem(ShopItemDTO input, ShopItem output)
        {
            output.Color = input.Color;
            output.ItemVNum = input.ItemVNum;
            output.Rare = (sbyte)input.Rare;
            output.ShopId = input.ShopId;
            output.ShopItemId = input.ShopItemId;
            output.Slot = input.Slot;
            output.Type = input.Type;
            output.Upgrade = input.Upgrade;
        }
    }
}

