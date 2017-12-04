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
using OpenNos.Domain;
using System;
using System.Data.Entity;
using System.Linq;

namespace OpenNos.DAL.DAO
{
    public class MinigameLogDAO : IMinigameLogDAO
    {
        #region Methods

        public SaveResult InsertOrUpdate(ref MinigameLogDTO minigameLog)
        {
            try
            {
                using (OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    long minigameLogId = minigameLog.MinigameLogId;
                    MinigameLog entity = context.MinigameLog.FirstOrDefault(c => c.MinigameLogId.Equals(minigameLogId));

                    if (entity == null)
                    {
                        minigameLog = insert(minigameLog, context);
                        return SaveResult.Inserted;
                    }
                    minigameLog = update(entity, minigameLog, context);
                    return SaveResult.Updated;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return SaveResult.Error;
            }
        }

        public MinigameLogDTO LoadById(long minigameLogId)
        {
            try
            {
                using (OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    MinigameLog minigameLog = context.MinigameLog.FirstOrDefault(a => a.MinigameLogId.Equals(minigameLogId));
                    if (minigameLog != null)
                    {
                        MinigameLogDTO minigameLogDTO = new MinigameLogDTO();
                        if (Mapper.Mapper.Instance.MinigameLogMapper.ToMinigameLogDTO(minigameLog, minigameLogDTO))
                        {
                            return minigameLogDTO;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return null;
        }

        private MinigameLogDTO insert(MinigameLogDTO account, OpenNosContext context)
        {
            MinigameLog entity = new MinigameLog();
            Mapper.Mapper.Instance.MinigameLogMapper.ToMinigameLog(account, entity);
            context.MinigameLog.Add(entity);
            context.SaveChanges();
            Mapper.Mapper.Instance.MinigameLogMapper.ToMinigameLogDTO(entity, account);
            return account;
        }

        private MinigameLogDTO update(MinigameLog entity, MinigameLogDTO account, OpenNosContext context)
        {
            if (entity != null)
            {
                Mapper.Mapper.Instance.MinigameLogMapper.ToMinigameLog(account, entity);
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
            if (Mapper.Mapper.Instance.MinigameLogMapper.ToMinigameLogDTO(entity, account))
            {
                return account;
            }

            return null;
        }

        #endregion
    }
}