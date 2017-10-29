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

using AutoMapper;
using OpenNos.Core;
using OpenNos.DAL.EF;
using OpenNos.DAL.EF.Helpers;
using OpenNos.DAL.Interface;
using OpenNos.Data;
using OpenNos.Data.Enums;
using OpenNos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenNos.DAL.DAO
{
    public class ItemInstanceDAO : SynchronizableBaseDAO<ItemInstance, ItemInstanceDTO>, IItemInstanceDAO
    {
        #region Members

        private Type _baseType;

        #endregion

        #region Methods

        public DeleteResult DeleteFromSlotAndType(long characterId, short slot, InventoryType type)
        {
            try
            {
                ItemInstanceDTO dto = LoadBySlotAndType(characterId, slot, type);
                if (dto != null)
                {
                    return Delete(dto.Id);
                }

                return DeleteResult.Unknown;
            }
            catch (Exception e)
            {
                Logger.Error($"characterId: {characterId} slot: {slot} type: {type}", e);
                return DeleteResult.Error;
            }
        }

        public DeleteResult DeleteGuidList(IEnumerable<Guid> guids)
        {
            using (OpenNosContext context = DataAccessHelper.CreateContext())
            {
                try
                {
                    foreach (Guid id in guids)
                    {
                        ItemInstance entity = context.ItemInstance.FirstOrDefault(i => i.Id == id);
                        if (entity != null)
                        {
                            context.ItemInstance.Remove(entity);
                        }
                    }
                    context.SaveChanges();
                }
                catch
                {
                    foreach (Guid id in guids)
                    {
                        try
                        {
                            Delete(id);
                        }
                        catch (Exception ex)
                        {
                            // TODO: Work on: statement conflicted with the REFERENCE constraint
                            //       "FK_dbo.BazaarItem_dbo.ItemInstance_ItemInstanceId". The
                            //       conflict occurred in database "opennos", table
                            //       "dbo.BazaarItem", column 'ItemInstanceId'.
                            Logger.LogUserEventError("ONSAVEDELETION_EXCEPTION", "Saving Process", $"Detailed Item Information: Item ID = {id}", ex);
                        }
                    }
                }
                return DeleteResult.Deleted;
            }
        }

        public override void InitializeMapper()
        {
            // avoid override of mapping
        }

        public void InitializeMapper(Type baseType)
        {
            _baseType = baseType;
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(baseType, typeof(ItemInstance)).ForMember("Item", opts => opts.Ignore());

                cfg.CreateMap(typeof(ItemInstance), typeof(ItemInstanceDTO)).As(baseType);

                Type itemInstanceType = typeof(ItemInstance);
                foreach (KeyValuePair<Type, Type> entry in _mappings)
                {
                    // GameObject -> Entity
                    cfg.CreateMap(entry.Key, entry.Value).ForMember("Item", opts => opts.Ignore()).IncludeBase(baseType, typeof(ItemInstance));

                    // Entity -> GameObject
                    cfg.CreateMap(entry.Value, entry.Key).IncludeBase(typeof(ItemInstance), baseType);

                    // Entity -> GameObject
                    cfg.CreateMap(entry.Value, typeof(ItemInstanceDTO)).As(entry.Key);
                }
            });

            _mapper = config.CreateMapper();
        }

        public SaveResult InsertOrUpdateFromList(IEnumerable<ItemInstanceDTO> items)
        {
            try
            {
                using (OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    void insert(ItemInstanceDTO iteminstance)
                    {
                        ItemInstance _entity = _mapper.Map<ItemInstance>(iteminstance);
                        context.ItemInstance.Add(_entity);
                    }

                    void update(ItemInstance _entity, ItemInstanceDTO iteminstance)
                    {
                        if (_entity != null)
                        {
                            _mapper.Map(iteminstance, _entity);
                            context.SaveChanges();
                        }
                    }

                    foreach (ItemInstanceDTO item in items)
                    {
                        ItemInstance entity = context.ItemInstance.FirstOrDefault(c => c.Id == item.Id);

                        if (entity == null)
                        {
                            insert(item);
                        }
                        update(entity, item);
                    }

                    context.SaveChanges();
                    return SaveResult.Updated;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return SaveResult.Error;
            }
        }

        public IEnumerable<ItemInstanceDTO> LoadByCharacterId(long characterId)
        {
            using (OpenNosContext context = DataAccessHelper.CreateContext())
            {
                foreach (ItemInstance itemInstance in context.ItemInstance.Where(i => i.CharacterId.Equals(characterId)))
                {
                    yield return _mapper.Map<ItemInstanceDTO>(itemInstance);
                }
            }
        }

        public ItemInstanceDTO LoadBySlotAndType(long characterId, short slot, InventoryType type)
        {
            try
            {
                using (OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    ItemInstance entity = context.ItemInstance.FirstOrDefault(i => i.CharacterId == characterId && i.Slot == slot && i.Type == type);
                    return _mapper.Map<ItemInstanceDTO>(entity);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public IEnumerable<ItemInstanceDTO> LoadByType(long characterId, InventoryType type)
        {
            using (OpenNosContext context = DataAccessHelper.CreateContext())
            {
                foreach (ItemInstance itemInstance in context.ItemInstance.Where(i => i.CharacterId == characterId && i.Type == type))
                {
                    yield return _mapper.Map<ItemInstanceDTO>(itemInstance);
                }
            }
        }

        public IList<Guid> LoadSlotAndTypeByCharacterId(long characterId)
        {
            try
            {
                using (OpenNosContext context = DataAccessHelper.CreateContext())
                {
                    return context.ItemInstance.Where(i => i.CharacterId.Equals(characterId)).Select(i => i.Id).ToList();
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public override IMappingBaseDAO RegisterMapping(Type gameObjectType)
        {
            try
            {
                Type targetType = typeof(ItemInstance).Assembly.GetTypes().SingleOrDefault(t => t.Name.Equals(gameObjectType.Name));
                _mappings.Add(gameObjectType, targetType);
                return this;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        protected override ItemInstanceDTO InsertOrUpdate(OpenNosContext context, ItemInstanceDTO dto)
        {
            try
            {
                ItemInstance entity = context.ItemInstance.FirstOrDefault(c => c.Id == dto.Id);
                dto = entity == null ? Insert(dto, context) : Update(entity, dto, context);
                return dto;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        protected override ItemInstance MapEntity(ItemInstanceDTO dto)
        {
            try
            {
                ItemInstance entity = _mapper.Map<ItemInstance>(dto);
                KeyValuePair<Type, Type> targetMapping = _mappings.FirstOrDefault(k => k.Key.Equals(dto.GetType()));
                if (targetMapping.Key != null)
                {
                    entity = _mapper.Map(dto, targetMapping.Key, targetMapping.Value) as ItemInstance;
                }

                return entity;
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