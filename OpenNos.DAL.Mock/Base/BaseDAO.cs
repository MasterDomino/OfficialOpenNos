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

using OpenNos.DAL.Interface;
using OpenNos.Data;
using System;
using System.Collections.Generic;

namespace OpenNos.DAL.Mock
{
    public abstract class BaseDAO<TDTO> : IMappingBaseDAO
    {
        #region Members

        protected IMapper _mapper;
        protected Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>();

        #endregion

        #region Instantiation

        protected BaseDAO() => Container = new List<TDTO>();

        #endregion

        #region Properties

        public IList<TDTO> Container { get; set; }

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

        public void Insert(IEnumerable<TDTO> dtos)
        {
            foreach (TDTO dto in dtos)
            {
                Insert(dto);
            }
        }

        public virtual TDTO Insert(TDTO dto)
        {
            Container.Add(dto);
            return dto;
        }

        public IEnumerable<TDTO> LoadAll()
        {
            foreach (TDTO dto in Container)
            {
                yield return MapEntity(dto);
            }
        }

        public virtual IMappingBaseDAO RegisterMapping(Type gameObjectType)
        {
            try
            {
                Type targetType = typeof(TDTO);
                _mappings.Add(gameObjectType, targetType);
                return this;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Map a DTO to a GO
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected virtual TDTO MapEntity(TDTO dto) => _mapper.Map<TDTO>(dto);

        #endregion
    }
}