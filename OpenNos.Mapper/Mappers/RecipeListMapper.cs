using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class RecipeListMapper
    {
        public RecipeListMapper()
        {
        }

        public void ToRecipeListDTO(RecipeList input, RecipeListDTO output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.ItemVNum = input.ItemVNum;
            output.MapNpcId = input.MapNpcId;
            output.RecipeId = input.RecipeId;
            output.RecipeListId = input.RecipeListId;
        }

        public void ToRecipeList(RecipeListDTO input, RecipeList output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.ItemVNum = input.ItemVNum;
            output.MapNpcId = input.MapNpcId;
            output.RecipeId = input.RecipeId;
            output.RecipeListId = input.RecipeListId;
        }
    }
}

