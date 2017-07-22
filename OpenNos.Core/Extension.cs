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

using System;

namespace OpenNos.Core
{
    public static class Extension
    {
        #region Methods

        public static string Truncate(this string str, int length)
        {
            return str.Length > length ? str.Substring(0, length) : str;
        }

        public static DateTime RoundUp(this DateTime dt, TimeSpan d)
        {
            long modTicks = dt.Ticks % d.Ticks;
            long delta = modTicks != 0 ? d.Ticks - modTicks : 0;
            return new DateTime(dt.Ticks + delta, dt.Kind);
        }

        public static DateTime RoundDown(this DateTime dt, TimeSpan d)
        {
            long delta = dt.Ticks % d.Ticks;
            return new DateTime(dt.Ticks - delta, dt.Kind);
        }

        public static DateTime RoundToNearest(this DateTime dt, TimeSpan d)
        {
            long delta = dt.Ticks % d.Ticks;
            bool roundUp = delta > d.Ticks / 2;
            long offset = roundUp ? d.Ticks : 0;
            DateTime targetTime = new DateTime(dt.Ticks + offset - delta, dt.Kind);

            return targetTime <= DateTime.Now ? RoundUp(dt, d) : targetTime;
        }

        #endregion
    }
}