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
    public class QuestDAO : IQuestDAO
    {
        #region Methods


        public DeleteResult DeleteById(long id)
        {
            try
            {
                using (OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    Quest deleteEntity = context.Quest.Find(id);
                    if (deleteEntity != null)
                    {
                        context.Quest.Remove(deleteEntity);
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

        public QuestDTO InsertOrUpdate(QuestDTO quest)
        {
            try
            {
                using (OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    Quest entity = context.Quest.Find(quest.QuestId);

                    if (entity == null)
                    {
                        return insert(quest, context);
                    }
                    return update(entity, quest, context);
                }
            }
            catch (Exception e)
            {
                Logger.Error(string.Format(Language.Instance.GetMessageFromKey("INSERT_ERROR"), quest, e.Message), e);
                return quest;
            }
        }

        public void InsertOrUpdateFromList(List<QuestDTO> questList)
        {
            try
            {
                using (OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    void insert(QuestDTO quest)
                    {
                        Quest _entity = _mapper.Map<Quest>(quest);
                        context.Quest.Add(_entity);
                    }

                    void update(Quest _entity, QuestDTO quest)
                    {
                        if (_entity != null)
                        {
                            _mapper.Map(quest, _entity);
                            context.SaveChanges();
                        }
                    }

                    foreach (QuestDTO item in questList)
                    {
                        Quest entity = context.Quest.Find(item.QuestId);

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

        public IEnumerable<QuestDTO> LoadAll()
        {
            using (OpenNosContext context = DataAccessHelper.CreateContext())
            {
                return context.Quest.ToList().Select(c => _mapper.Map<QuestDTO>(c));
            }
        }

        public QuestDTO LoadById(long id)
        {
            using (OpenNosContext context = DataAccessHelper.CreateContext())
            {
                return _mapper.Map<QuestDTO>(context.Quest.Find(id));
            }
        }

        private QuestDTO insert(QuestDTO quest, OpenNosContext context)
        {
            Quest entity = _mapper.Map<Quest>(quest);
            context.Quest.Add(entity);
            context.SaveChanges();
            return _mapper.Map<QuestDTO>(entity);
        }

        private QuestDTO update(Quest entity, QuestDTO quest, OpenNosContext context)
        {
            if (entity != null)
            {
                _mapper.Map(quest, entity);
                context.SaveChanges();
            }

            return _mapper.Map<QuestDTO>(entity);
        }

        #endregion
    }
}