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
using OpenNos.DAL.EF;
using OpenNos.DAL.EF.Helpers;
using OpenNos.DAL.Interface;
using OpenNos.Data;
using OpenNos.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenNos.DAL.DAO
{
    public class ShellEffectDAO : MappingBaseDAO<ShellEffect, ShellEffectDTO>, IShellEffectDAO
    {
        #region Methods

        public DeleteResult DeleteByEquipmentSerialId(Guid id)
        {
            try
            {
                using (OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    List<ShellEffect> deleteentities = context.ShellEffect.Where(s => s.EquipmentSerialId == id).ToList();
                    if (deleteentities.Count != 0)
                    {
                        context.ShellEffect.RemoveRange(deleteentities);
                        context.SaveChanges();
                    }

                    return DeleteResult.Deleted;
                }
            }
            catch (Exception e)
            {
                Logger.Error(string.Format(Language.Instance.GetMessageFromKey("DELETE_ERROR"), id, e.Message), e);
                return DeleteResult.Error;
            }
        }

        public ShellEffectDTO InsertOrUpdate(ShellEffectDTO shelleffect)
        {
            try
            {
                using (OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    long shelleffectId = shelleffect.ShellEffectId;
                    ShellEffect entity = context.ShellEffect.FirstOrDefault(c => c.ShellEffectId.Equals(shelleffectId));

                    if (entity == null)
                    {
                        return insert(shelleffect, context);
                    }
                    return update(entity, shelleffect, context);
                }
            }
            catch (Exception e)
            {
                Logger.Error(string.Format(Language.Instance.GetMessageFromKey("INSERT_ERROR"), shelleffect, e.Message), e);
                return shelleffect;
            }
        }

        public void InsertOrUpdateFromList(List<ShellEffectDTO> shellEffects)
        {
            try
            {
                using (OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    void insert(ShellEffectDTO shelleffect)
                    {
                        ShellEffect _entity = _mapper.Map<ShellEffect>(shelleffect);
                        context.ShellEffect.Add(_entity);
                    }

                    void update(ShellEffect _entity, ShellEffectDTO shelleffect)
                    {
                        if (_entity != null)
                        {
                            _mapper.Map(shelleffect, _entity);
                            context.SaveChanges();
                        }
                    }

                    foreach (ShellEffectDTO item in shellEffects)
                    {
                        ShellEffect entity = context.ShellEffect.FirstOrDefault(c => c.ShellEffectId == item.ShellEffectId);

                        if (entity == null)
                        {
                            insert(item);
                        }
                        update(entity, item);
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }


        public IEnumerable<ShellEffectDTO> LoadByEquipmentSerialId(Guid id)
        {
            using (OpenNosContext context = DataAccessHelper.CreateContext())
            {
                return context.ShellEffect.Where(c => c.EquipmentSerialId == id).ToList().Select(c => _mapper.Map<ShellEffectDTO>(c)).ToList();
            }
        }

        private ShellEffectDTO insert(ShellEffectDTO shelleffect, OpenNosContext context)
        {
            ShellEffect entity = _mapper.Map<ShellEffect>(shelleffect);
            context.ShellEffect.Add(entity);
            context.SaveChanges();
            return _mapper.Map<ShellEffectDTO>(entity);
        }

        private ShellEffectDTO update(ShellEffect entity, ShellEffectDTO shelleffect, OpenNosContext context)
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