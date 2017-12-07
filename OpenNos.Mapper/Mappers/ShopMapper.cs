using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class ShopMapper
    {
        #region Methods

        public bool ToShop(ShopDTO input, Shop output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.MapNpcId = input.MapNpcId;
            output.MenuType = input.MenuType;
            output.Name = input.Name;
            output.ShopId = input.ShopId;
            output.ShopType = input.ShopType;
            return true;
        }

        public bool ToShopDTO(Shop input, ShopDTO output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.MapNpcId = input.MapNpcId;
            output.MenuType = input.MenuType;
            output.Name = input.Name;
            output.ShopId = input.ShopId;
            output.ShopType = input.ShopType;
            return true;
        }

        #endregion
    }
}