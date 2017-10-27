﻿using Microsoft.Win32;
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
using OpenNos.Data;
using System.Collections.Generic;
using System.Linq;

namespace OpenNos.GameObject
{
    public class ProduceItem : Item
    {
        #region Instantiation

        public ProduceItem(ItemDTO item) : base(item)
        {
        }

        #endregion

        #region Methods

        public override void Use(ClientSession session, ref ItemInstance inv, byte Option = 0, string[] packetsplit = null)
        {
            switch (Effect)
            {
                case 100:
                    switch (EffectValue)
                    {
                        case 15:
                            session.Character.LastNRunId = 0;
                            session.Character.LastItemVNum = inv.ItemVNum;
                            session.SendPacket("wopen 28 0");
                            List<Recipe> tps = ServerManager.Instance.GetRecipesByItemVNum(VNum);
                            string recipelist = tps.Where(s => s.Amount > 0).Aggregate("m_list 2", (current, s) => current + $" {s.ItemVNum}");
                            session.SendPacket(recipelist);
                            break;
                    }
                    break;

                default:
                    Logger.Warn(string.Format(Language.Instance.GetMessageFromKey("NO_HANDLER_ITEM"), GetType()));
                    break;
            }
        }

        #endregion
    }
}