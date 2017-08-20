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
using OpenNos.DAL.EF.DB;
using OpenNos.DAL.EF.Helpers;
using OpenNos.DAL.Interface;
using OpenNos.Data;
using OpenNos.Data.Enums;
using OpenNos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenNos.DAL.EF
{
    public class ShellEffectDAO : MappingBaseDAO<ShellEffect, ShellEffectDTO>, IShellEffectDAO
    {
        #region Methods

        public DeleteResult DeleteByItemInstanceId(Guid id)
        {
            try
            {
                using (var context = DataAccessHelper.CreateContext())
                {
                    List<ShellEffect> deleteentities = context.ShellEffect.Where(s => s.ItemInstanceId == id).ToList();
                    if (deleteentities.Count != 0)
                    {
                        context.ShellEffect.RemoveRange(deleteentities);
                    }

                    return DeleteResult.Deleted;
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(string.Format(Language.Instance.GetMessageFromKey("DELETE_ERROR"), id, e.Message), e);
                return DeleteResult.Error;
            }
        }

        public ShellEffectDTO InsertOrUpdate(ShellEffectDTO shelleffect)
        {
            try
            {
                using (var context = DataAccessHelper.CreateContext())
                {
                    long shelleffectId = shelleffect.ShellEffectId;
                    ShellEffect entity = context.ShellEffect.FirstOrDefault(c => c.ShellEffectId.Equals(shelleffectId));

                    if (entity == null)
                    {
                        return Insert(shelleffect, context);
                    }
                    return Update(entity, shelleffect, context);
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(string.Format(Language.Instance.GetMessageFromKey("INSERT_ERROR"), shelleffect, e.Message), e);
                return shelleffect;
            }
        }

        public IEnumerable<ShellEffectDTO> LoadByItemInstanceId(Guid id)
        {
            using (var context = DataAccessHelper.CreateContext())
            {
                return context.ShellEffect.Where(c => c.ItemInstanceId == id).ToList().Select(c => _mapper.Map<ShellEffectDTO>(c)).ToList();
            }
        }

        private ShellEffectDTO Insert(ShellEffectDTO shelleffect, OpenNosContext context)
        {
            ShellEffect entity = _mapper.Map<ShellEffect>(shelleffect);
            context.ShellEffect.Add(entity);
            context.SaveChanges();
            return _mapper.Map<ShellEffectDTO>(entity);
        }

        private ShellEffectDTO Update(ShellEffect entity, ShellEffectDTO shelleffect, OpenNosContext context)
        {
            if (entity != null)
            {
                _mapper.Map(shelleffect, entity);
                context.SaveChanges();
            }

            return _mapper.Map<ShellEffectDTO>(entity);
        }

        #endregion
    }
}