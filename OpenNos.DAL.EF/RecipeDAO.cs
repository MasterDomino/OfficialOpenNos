/*
 * This file is part of the OpenNos Emulator Project. See AUTHORS file for Copyright information
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 */

using OpenNos.Core;
using OpenNos.DAL.EF.Helpers;
using OpenNos.DAL.Interface;
using OpenNos.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenNos.DAL.EF
{
    public class RecipeDAO : MappingBaseDAO<Recipe, RecipeDTO>, IRecipeDAO
    {
        #region Methods

        public RecipeDTO Insert(RecipeDTO recipe)
        {
            try
            {
                using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    Recipe entity = _mapper.Map<Recipe>(recipe);
                    context.Recipe.Add(entity);
                    context.SaveChanges();
                    return _mapper.Map<RecipeDTO>(entity);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public IEnumerable<RecipeDTO> LoadAll()
        {
            using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
            {
                foreach (Recipe Recipe in context.Recipe)
                {
                    yield return _mapper.Map<RecipeDTO>(Recipe);
                }
            }
        }

        public IEnumerable<RecipeDTO> LoadByItemVNum(short itemVNum)
        {
            using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
            {
                foreach (Recipe recipe in context.Recipe.Where(s => s.ItemVNum.Equals(itemVNum)))
                {
                    yield return _mapper.Map<RecipeDTO>(recipe);
                }
            }
        }

        public RecipeDTO LoadById(short recipeId)
        {
            try
            {
                using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    return _mapper.Map<RecipeDTO>(context.Recipe.SingleOrDefault(s => s.RecipeId.Equals(recipeId)));
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        #endregion
    }
}