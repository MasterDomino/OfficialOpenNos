using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class RecipeMapper
    {
        public RecipeMapper()
        {
        }

        public void ToRecipeDTO(Recipe input, RecipeDTO output)
        {
            output.Amount = input.Amount;
            output.ItemVNum = input.ItemVNum;
            output.RecipeId = input.RecipeId;
        }

        public void ToRecipe(RecipeDTO input, Recipe output)
        {
            output.Amount = input.Amount;
            output.ItemVNum = input.ItemVNum;
            output.RecipeId = input.RecipeId;
        }
    }
}
