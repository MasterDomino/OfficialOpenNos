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
    public class ShellEffectGenerationDAO : MappingBaseDAO<ShellEffectGeneration, ShellEffectGenerationDTO>, IShellEffectGenerationDAO
    {
        #region Methods

        public ShellEffectGenerationDTO InsertOrUpdate(ShellEffectGenerationDTO shelleffect)
        {
            try
            {
                using (var context = DataAccessHelper.CreateContext())
                {
                    long shelleffectgenerationId = shelleffect.ShellEffectGenerationId;
                    ShellEffectGeneration entity = context.ShellEffectGeneration.FirstOrDefault(c => c.ShellEffectGenerationId.Equals(shelleffectgenerationId));

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

        public IEnumerable<ShellEffectGenerationDTO> LoadAll()
        {
            using (var context = DataAccessHelper.CreateContext())
            {
                return context.ShellEffectGeneration.ToList().Select(c => _mapper.Map<ShellEffectGenerationDTO>(c)).ToList();
            }
        }

        private ShellEffectGenerationDTO Insert(ShellEffectGenerationDTO shelleffect, OpenNosContext context)
        {
            ShellEffectGeneration entity = _mapper.Map<ShellEffectGeneration>(shelleffect);
            context.ShellEffectGeneration.Add(entity);
            context.SaveChanges();
            return _mapper.Map<ShellEffectGenerationDTO>(entity);
        }

        private ShellEffectGenerationDTO Update(ShellEffectGeneration entity, ShellEffectGenerationDTO shelleffect, OpenNosContext context)
        {
            if (entity != null)
            {
                _mapper.Map(shelleffect, entity);
                context.SaveChanges();
            }

            return _mapper.Map<ShellEffectGenerationDTO>(entity);
        }

        #endregion
    }
}