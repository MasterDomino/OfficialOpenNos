using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class RollGeneratedItemMapper
    {
        public RollGeneratedItemMapper()
        {
        }

        public void ToRollGeneratedItemDTO(RollGeneratedItem input, RollGeneratedItemDTO output)
        {
            output.IsRareRandom = input.IsRareRandom;
            output.ItemGeneratedAmount = input.ItemGeneratedAmount;
            output.ItemGeneratedVNum = input.ItemGeneratedVNum;
            output.MaximumOriginalItemRare = input.MaximumOriginalItemRare;
            output.MinimumOriginalItemRare = input.MinimumOriginalItemRare;
            output.OriginalItemDesign = input.OriginalItemDesign;
            output.OriginalItemVNum = input.OriginalItemVNum;
            output.Probability = input.Probability;
            output.RollGeneratedItemId = input.RollGeneratedItemId;
        }

        public void ToRollGeneratedItem(RollGeneratedItemDTO input, RollGeneratedItem output)
        {
            output.IsRareRandom = input.IsRareRandom;
            output.ItemGeneratedAmount = input.ItemGeneratedAmount;
            output.ItemGeneratedVNum = input.ItemGeneratedVNum;
            output.MaximumOriginalItemRare = input.MaximumOriginalItemRare;
            output.MinimumOriginalItemRare = input.MinimumOriginalItemRare;
            output.OriginalItemDesign = input.OriginalItemDesign;
            output.OriginalItemVNum = input.OriginalItemVNum;
            output.Probability = input.Probability;
            output.RollGeneratedItemId = input.RollGeneratedItemId;
        }
    }
}

