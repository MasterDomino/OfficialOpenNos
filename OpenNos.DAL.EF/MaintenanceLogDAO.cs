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
    public class MaintenanceLogDAO : MappingBaseDAO<MaintenanceLog, MaintenanceLogDTO>, IMaintenanceLogDAO
    {
        #region Methods

        public MaintenanceLogDTO Insert(MaintenanceLogDTO maintenanceLog)
        {
            try
            {
                using (var context = DataAccessHelper.CreateContext())
                {
                    MaintenanceLog entity = _mapper.Map<MaintenanceLog>(maintenanceLog);
                    context.MaintenanceLog.Add(entity);
                    context.SaveChanges();
                    return _mapper.Map<MaintenanceLogDTO>(maintenanceLog);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public IEnumerable<MaintenanceLogDTO> LoadAll()
        {
            using (var context = DataAccessHelper.CreateContext())
            {
                foreach (MaintenanceLog maintenanceLog in context.MaintenanceLog)
                {
                    yield return _mapper.Map<MaintenanceLogDTO>(maintenanceLog);
                }
            }
        }

        public MaintenanceLogDTO LoadFirst()
        {
            try
            {
                using (var context = DataAccessHelper.CreateContext())
                {
                    return _mapper.Map<MaintenanceLogDTO>(context.MaintenanceLog.FirstOrDefault(m => m.DateEnd > DateTime.Now && m.DateStart <= DateTime.Now));
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