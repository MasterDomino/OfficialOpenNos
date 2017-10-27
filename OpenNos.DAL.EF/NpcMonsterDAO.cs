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
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenNos.DAL.EF
{
    public class NpcMonsterDAO : MappingBaseDAO<NpcMonster, NpcMonsterDTO>, INpcMonsterDAO
    {
        #region Methods

        public IEnumerable<NpcMonsterDTO> FindByName(string name)
        {
            using (OpenNosContext context = DataAccessHelper.CreateContext())
            {
                foreach (NpcMonster npcMonster in context.NpcMonster.Where(s => string.IsNullOrEmpty(name) ? s.Name.Equals(string.Empty) : s.Name.Contains(name)))
                {
                    yield return _mapper.Map<NpcMonsterDTO>(npcMonster);
                }
            }
        }

        public void Insert(List<NpcMonsterDTO> npcMonsters)
        {
            try
            {
                using (OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    foreach (NpcMonsterDTO Item in npcMonsters)
                    {
                        NpcMonster entity = _mapper.Map<NpcMonster>(Item);
                        context.NpcMonster.Add(entity);
                    }
                    context.Configuration.AutoDetectChangesEnabled = true;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public NpcMonsterDTO Insert(NpcMonsterDTO npc)
        {
            try
            {
                using (OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    NpcMonster entity = _mapper.Map<NpcMonster>(npc);
                    context.NpcMonster.Add(entity);
                    context.SaveChanges();
                    return _mapper.Map<NpcMonsterDTO>(entity);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public SaveResult InsertOrUpdate(ref NpcMonsterDTO npcMonster)
        {
            try
            {
                using (OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    short npcMonsterVNum = npcMonster.NpcMonsterVNum;
                    NpcMonster entity = context.NpcMonster.FirstOrDefault(c => c.NpcMonsterVNum.Equals(npcMonsterVNum));

                    if (entity == null)
                    {
                        npcMonster = insert(npcMonster, context);
                        return SaveResult.Inserted;
                    }

                    npcMonster = update(entity, npcMonster, context);
                    return SaveResult.Updated;
                }
            }
            catch (Exception e)
            {
                Logger.Error(string.Format(Language.Instance.GetMessageFromKey("UPDATE_NPCMONSTER_ERROR"), npcMonster.NpcMonsterVNum, e.Message), e);
                return SaveResult.Error;
            }
        }

        public IEnumerable<NpcMonsterDTO> LoadAll()
        {
            using (OpenNosContext context = DataAccessHelper.CreateContext())
            {
                foreach (NpcMonster NpcMonster in context.NpcMonster)
                {
                    yield return _mapper.Map<NpcMonsterDTO>(NpcMonster);
                }
            }
        }

        public NpcMonsterDTO LoadByVNum(short npcMonsterVNum)
        {
            try
            {
                using (OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    return _mapper.Map<NpcMonsterDTO>(context.NpcMonster.FirstOrDefault(i => i.NpcMonsterVNum.Equals(npcMonsterVNum)));
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        private NpcMonsterDTO insert(NpcMonsterDTO npcMonster, OpenNosContext context)
        {
            NpcMonster entity = _mapper.Map<NpcMonster>(npcMonster);
            context.NpcMonster.Add(entity);
            context.SaveChanges();
            return _mapper.Map<NpcMonsterDTO>(entity);
        }

        private NpcMonsterDTO update(NpcMonster entity, NpcMonsterDTO npcMonster, OpenNosContext context)
        {
            if (entity != null)
            {
                _mapper.Map(npcMonster, entity);
                context.SaveChanges();
            }
            return _mapper.Map<NpcMonsterDTO>(entity);
        }

        #endregion
    }
}