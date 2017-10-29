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
using OpenNos.DAL.Interface;
using OpenNos.Data;
using OpenNos.Data.Enums;
using OpenNos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenNos.DAL.Mock
{
    public class ItemInstanceDAO : SynchronizableBaseDAO<ItemInstanceDTO>, IItemInstanceDAO
    {
        #region Members

        private readonly IDictionary<Type, Type> itemInstanceMappings = new Dictionary<Type, Type>();
        private Type _baseType;

        #endregion

        #region Methods

        public DeleteResult DeleteFromSlotAndType(long characterId, short slot, InventoryType type) => throw new NotImplementedException();

        public void InitializeMapper(Type baseType)
        {
            _baseType = baseType;
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(baseType, typeof(ItemInstanceDTO));
                cfg.CreateMap(typeof(ItemInstanceDTO), typeof(ItemInstanceDTO)).As(baseType);
                Type itemInstanceType = typeof(ItemInstanceDTO);
                foreach (KeyValuePair<Type, Type> entry in itemInstanceMappings)
                {
                    cfg.CreateMap(entry.Key, entry.Value).IncludeBase(baseType, typeof(ItemInstanceDTO));
                    cfg.CreateMap(entry.Value, entry.Key).IncludeBase(typeof(ItemInstanceDTO), baseType);
                    cfg.CreateMap(entry.Value, typeof(ItemInstanceDTO)).As(entry.Key);
                }
            });
            _mapper = config.CreateMapper();
        }

        public IEnumerable<ItemInstanceDTO> LoadByCharacterId(long characterId) => Container.Where(i => i.CharacterId == characterId);

        public ItemInstanceDTO LoadBySlotAndType(long characterId, short slot, InventoryType type) => MapEntity(Container.SingleOrDefault(i => i.CharacterId == characterId && i.Slot == slot && i.Type == type));

        public IEnumerable<ItemInstanceDTO> LoadByType(long characterId, InventoryType type) => Container.Where(i => i.CharacterId == characterId && i.Type == type);

        IList<Guid> IItemInstanceDAO.LoadSlotAndTypeByCharacterId(long characterId) => throw new NotImplementedException();

        public IEnumerable<Guid> LoadSlotAndTypeByCharacterId(long characterId) => Container.Where(i => i.CharacterId == characterId).Select(c => c.Id);

        public override IMappingBaseDAO RegisterMapping(Type gameObjectType)
        {
            Type itemInstanceType = typeof(ItemInstanceDTO);
            if (!itemInstanceMappings.ContainsKey(gameObjectType))
            {
                itemInstanceMappings.Add(gameObjectType, itemInstanceType);
            }

            return this;
        }

        public DeleteResult DeleteGuidList(IEnumerable<Guid> guids) => throw new NotImplementedException();

        public SaveResult InsertOrUpdateFromList(IEnumerable<ItemInstanceDTO> items) => throw new NotImplementedException();

        #endregion
    }
}