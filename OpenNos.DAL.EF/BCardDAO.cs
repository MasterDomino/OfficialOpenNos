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
using OpenNos.DAL.EF.Helpers;
using OpenNos.DAL.Interface;
using OpenNos.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenNos.DAL.EF
{
    public class BCardDAO : MappingBaseDAO<BCard, BCardDTO>, IBCardDAO
    {
        #region Methods

        public BCardDTO Insert(ref BCardDTO cardObject)
        {
            try
            {
                using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    BCard entity = _mapper.Map<BCard>(cardObject);
                    context.BCard.Add(entity);
                    context.SaveChanges();
                    return _mapper.Map<BCardDTO>(entity);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public void Insert(List<BCardDTO> cards)
        {
            try
            {
                using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    foreach (BCardDTO card in cards)
                    {
                        BCard entity = _mapper.Map<BCard>(card);
                        context.BCard.Add(entity);
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

        public IEnumerable<BCardDTO> LoadAll()
        {
            using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
            {
                foreach (BCard card in context.BCard)
                {
                    yield return _mapper.Map<BCardDTO>(card);
                }
            }
        }

        public IEnumerable<BCardDTO> LoadByCardId(short cardId)
        {
            using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
            {
                foreach (BCard card in context.BCard.Where(s => s.CardId == cardId))
                {
                    yield return _mapper.Map<BCardDTO>(card);
                }
            }
        }

        public BCardDTO LoadById(short cardId)
        {
            try
            {
                using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    return _mapper.Map<BCardDTO>(context.BCard.FirstOrDefault(s => s.BCardId.Equals(cardId)));
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public IEnumerable<BCardDTO> LoadByItemVNum(short vNum)
        {
            using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
            {
                foreach (BCard card in context.BCard.Where(s => s.ItemVNum == vNum))
                {
                    yield return _mapper.Map<BCardDTO>(card);
                }
            }
        }

        public IEnumerable<BCardDTO> LoadByNpcMonsterVNum(short vNum)
        {
            using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
            {
                foreach (BCard card in context.BCard.Where(s => s.NpcMonsterVNum == vNum))
                {
                    yield return _mapper.Map<BCardDTO>(card);
                }
            }
        }

        public IEnumerable<BCardDTO> LoadBySkillVNum(short vNum)
        {
            using (DB.OpenNosContext context = DataAccessHelper.CreateContext())
            {
                foreach (BCard card in context.BCard.Where(s => s.SkillVNum == vNum))
                {
                    yield return _mapper.Map<BCardDTO>(card);
                }
            }
        }

        #endregion
    }
}