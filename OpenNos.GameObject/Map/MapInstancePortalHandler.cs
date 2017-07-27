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
using System.Collections.Generic;

namespace OpenNos.GameObject
{
    public static class MapInstancePortalHandler
    {
        #region Properties

        public static int SourceX { get; }

        #endregion

        #region Methods

        public static List<Portal> GenerateMinilandEntryPortals(int entryMap, Guid exitMapinstanceId)
        {
            List<Portal> list = new List<Portal>();

            switch (entryMap)
            {
                case 1:
                    list.Add(new Portal
                    {
                        SourceX = 110,
                        SourceY = 124,
                        DestinationX = 5,
                        DestinationY = 8,
                        Type = (short)PortalType.Invisible,
                        SourceMapId = 1,
                        DestinationMapInstanceId = exitMapinstanceId
                    });
                    break;

                case 145:
                    list.Add(new Portal
                    {
                        SourceX = 9,
                        SourceY = 171,
                        DestinationX = 5,
                        DestinationY = 8,
                        Type = (short)PortalType.Invisible,
                        SourceMapId = 145,
                        DestinationMapInstanceId = exitMapinstanceId
                    });
                    break;
            }

            return list;
        }

        #endregion
    }
}