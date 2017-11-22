using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class BazaarItemMapper
    {
        public BazaarItemMapper()
        {

        }

        public void ToBazaarItemDTO(BazaarItem input, BazaarItemDTO output)
        {
            output.Amount = input.Amount;
            output.BazaarItemId = input.BazaarItemId;
            output.DateStart = input.DateStart;
            output.Duration = input.Duration;
            output.IsPackage = input.IsPackage;
            output.ItemInstanceId = input.ItemInstanceId;
            output.MedalUsed = input.MedalUsed;
            output.Price = input.Price;
            output.SellerId = input.SellerId;
        }

        public void ToBazaarItem(BazaarItemDTO input, BazaarItem output)
        {
            output.Amount = input.Amount;
            output.BazaarItemId = input.BazaarItemId;
            output.DateStart = input.DateStart;
            output.Duration = input.Duration;
            output.IsPackage = input.IsPackage;
            output.ItemInstanceId = input.ItemInstanceId;
            output.MedalUsed = input.MedalUsed;
            output.Price = input.Price;
            output.SellerId = input.SellerId;
        }
    }
}
