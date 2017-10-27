using OpenNos.DAL.Interface;
using OpenNos.Data;
using System;

namespace OpenNos.DAL.Mock
{
    public class RecipeListDAO : BaseDAO<RecipeListDTO>, IRecipeListDAO
    {
        #region Methods

        public RecipeListDTO LoadById(int recipeListId) => throw new NotImplementedException();

        #endregion
    }
}