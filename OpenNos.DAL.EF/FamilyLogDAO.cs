﻿/*
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
    public class FamilyLogDAO : MappingBaseDAO<FamilyLog, FamilyLogDTO>, IFamilyLogDAO
    {
        #region Methods

        public DeleteResult Delete(long familyLogId)
        {
            try
            {
                using (var context = DataAccessHelper.CreateContext())
                {
                    FamilyLog famlog = context.FamilyLog.FirstOrDefault(c => c.FamilyLogId.Equals(familyLogId));

                    if (famlog != null)
                    {
                        context.FamilyLog.Remove(famlog);
                        context.SaveChanges();
                    }

                    return DeleteResult.Deleted;
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(string.Format(Language.Instance.GetMessageFromKey("DELETE_ERROR"), familyLogId, e.Message), e);
                return DeleteResult.Error;
            }
        }

        public SaveResult InsertOrUpdate(ref FamilyLogDTO familyLog)
        {
            try
            {
                using (var context = DataAccessHelper.CreateContext())
                {
                    long FamilyLog = familyLog.FamilyLogId;
                    FamilyLog entity = context.FamilyLog.FirstOrDefault(c => c.FamilyLogId.Equals(FamilyLog));

                    if (entity == null)
                    {
                        familyLog = Insert(familyLog, context);
                        return SaveResult.Inserted;
                    }

                    familyLog = Update(entity, familyLog, context);
                    return SaveResult.Updated;
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(string.Format(Language.Instance.GetMessageFromKey("UPDATE_FAMILYLOG_ERROR"), familyLog.FamilyLogId, e.Message), e);
                return SaveResult.Error;
            }
        }

        public IEnumerable<FamilyLogDTO> LoadByFamilyId(long familyId)
        {
            using (var context = DataAccessHelper.CreateContext())
            {
                foreach (FamilyLog familylog in context.FamilyLog.Where(fc => fc.FamilyId.Equals(familyId)))
                {
                    yield return _mapper.Map<FamilyLogDTO>(familylog);
                }
            }
        }

        private FamilyLogDTO Insert(FamilyLogDTO famlog, OpenNosContext context)
        {
            FamilyLog entity = _mapper.Map<FamilyLog>(famlog);
            context.FamilyLog.Add(entity);
            context.SaveChanges();
            return _mapper.Map<FamilyLogDTO>(entity);
        }

        private FamilyLogDTO Update(FamilyLog entity, FamilyLogDTO famlog, OpenNosContext context)
        {
            if (entity != null)
            {
                _mapper.Map(famlog, entity);
                context.SaveChanges();
            }

            return _mapper.Map<FamilyLogDTO>(entity);
        }

        #endregion
    }
}