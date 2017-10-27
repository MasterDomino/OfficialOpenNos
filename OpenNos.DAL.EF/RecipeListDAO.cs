using OpenNos.Core;
using OpenNos.DAL.EF.Helpers;
using OpenNos.DAL.Interface;
using OpenNos.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenNos.DAL.EF
{
    public class RecipeListDAO : MappingBaseDAO<RecipeList, RecipeListDTO>, IRecipeListDAO
    {
        #region Methods

        public RecipeListDTO Insert(RecipeListDTO recipeList)
        {
            try
            {
                using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    RecipeList entity = _mapper.Map<RecipeList>(recipeList);
                    context.RecipeList.Add(entity);
                    context.SaveChanges();
                    return _mapper.Map<RecipeListDTO>(entity);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public IEnumerable<RecipeListDTO> LoadAll()
        {
            using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
            {
                foreach (RecipeList recipeList in context.RecipeList)
                {
                    yield return _mapper.Map<RecipeListDTO>(recipeList);
                }
            }
        }

        public RecipeListDTO LoadById(int recipeListId)
        {
            try
            {
                using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    return _mapper.Map<RecipeListDTO>(context.RecipeList.SingleOrDefault(s => s.RecipeListId.Equals(recipeListId)));
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public IEnumerable<RecipeListDTO> LoadByItemVNum(short itemVNum)
        {
            using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
            {
                foreach (RecipeList recipeList in context.RecipeList.Where(r => r.ItemVNum.Equals(itemVNum)))
                {
                    yield return _mapper.Map<RecipeListDTO>(recipeList);
                }
            }
        }

        public IEnumerable<RecipeListDTO> LoadByMapNpcId(int mapNpcId)
        {
            using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
            {
                foreach (RecipeList recipeList in context.RecipeList.Where(r => r.MapNpcId.Equals(mapNpcId)))
                {
                    yield return _mapper.Map<RecipeListDTO>(recipeList);
                }
            }
        }

        public IEnumerable<RecipeListDTO> LoadByRecipeId(short recipeId)
        {
            using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
            {
                foreach (RecipeList recipeList in context.RecipeList.Where(r => r.RecipeId.Equals(recipeId)))
                {
                    yield return _mapper.Map<RecipeListDTO>(recipeList);
                }
            }
        }

        public void Update(RecipeListDTO recipe)
        {
            try
            {
                using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    RecipeList result = context.RecipeList.FirstOrDefault(r => r.RecipeListId.Equals(recipe.RecipeListId));
                    if (result != null)
                    {
                        recipe.RecipeListId = result.RecipeListId;
                        _mapper.Map(recipe, result);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        #endregion
    }
}