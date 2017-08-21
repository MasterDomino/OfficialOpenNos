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

using OpenNos.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenNos.DAL.EF
{
    public class ShellEffectGeneration : SynchronizableBaseEntity
    {
        #region Properties

        [Key]
        public long ShellEffectGenerationId { get; set; }

        public byte Rare { get; set; }

        public ShellEffectLevelType EffectLevel { get; set; }

        public byte Effect { get; set; }

        public byte MinimumValue { get; set; }

        public byte MaximumValue { get; set; }

        #endregion
    }
}