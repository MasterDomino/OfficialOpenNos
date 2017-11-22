using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class RecipeItemMapper
    {
        public RecipeItemMapper()
        {
        }

        public void ToRecipeItemDTO(RecipeItem input, RecipeItemDTO output)
        {
            output.Amount = input.Amount;
            output.ItemVNum = input.ItemVNum;
            output.RecipeId = input.RecipeId;
            output.RecipeItemId = input.RecipeItemId;
        }

        public void ToRecipeItem(RecipeItemDTO input, RecipeItem output)
        {
            output.Amount = input.Amount;
            output.ItemVNum = input.ItemVNum;
            output.RecipeId = input.RecipeId;
            output.RecipeItemId = input.RecipeItemId;
        }
    }
}
