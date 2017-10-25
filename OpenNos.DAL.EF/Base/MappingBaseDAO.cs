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
using OpenNos.DAL.Interface;
using OpenNos.Data;
using System;
using System.Collections.Generic;

namespace OpenNos.DAL.EF
{
    public class MappingBaseDAO<TEntity, TDTO> : IMappingBaseDAO where TDTO : MappingBaseDTO
    {
        #region Members

        protected readonly IDictionary<Type, Type> _mappings = new Dictionary<Type, Type>();

        protected IMapper _mapper;

        #endregion

        #region Methods

        public virtual void InitializeMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                foreach (KeyValuePair<Type, Type> entry in _mappings)
                {
                    // GameObject -> Entity
                    cfg.CreateMap(typeof(TDTO), entry.Value);

                    // Entity -> GameObject
                    cfg.CreateMap(entry.Value, typeof(TDTO)).AfterMap((src, dest) => ((MappingBaseDTO)dest).Initialize()).As(entry.Key);
                }
            });
            _mapper = config.CreateMapper();
        }

        public virtual IMappingBaseDAO RegisterMapping(Type gameObjectType)
        {
            try
            {
                Type targetType = typeof(TEntity);
                _mappings.Add(gameObjectType, targetType);
                return this;
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