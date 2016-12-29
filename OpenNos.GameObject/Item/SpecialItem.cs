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
using OpenNos.Data;
using OpenNos.Domain;
using System.Linq;

namespace OpenNos.GameObject
{
    public class SpecialItem : Item
    {
        #region Instantiation

        public SpecialItem(ItemDTO item) : base(item)
        {
        }

        #endregion

        #region Methods

        public override void Use(ClientSession session, ref ItemInstance inv, bool delay = false, string[] packetsplit = null)
        {
            switch (Effect)
            {

                // sp point potions
                case 150:
                case 151:
                    session.Character.SpAdditionPoint += EffectValue;
                    if (session.Character.SpAdditionPoint > 1000000)
                    {
                        session.Character.SpAdditionPoint = 1000000;
                    }
                    session.SendPacket(session.Character.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("SP_POINTSADDED"), EffectValue), 0));
                    session.Character.Inventory.RemoveItemAmountFromInventory(1, inv.Id);
                    session.SendPacket(session.Character.GenerateSpPoint());
                    break;

                case 204:
                    session.Character.SpPoint += EffectValue;
                    session.Character.SpAdditionPoint += EffectValue * 3;
                    if (session.Character.SpAdditionPoint > 1000000)
                    {
                        session.Character.SpAdditionPoint = 1000000;
                    }
                    if (session.Character.SpPoint > 10000)
                    {
                        session.Character.SpPoint = 10000;
                    }
                    session.SendPacket(session.Character.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("SP_POINTSADDEDBOTH"), EffectValue, EffectValue * 3), 0));
                    session.Character.Inventory.RemoveItemAmountFromInventory(1, inv.Id);
                    session.SendPacket(session.Character.GenerateSpPoint());
                    break;

                // Divorce letter
                case 6969: // this is imaginary number I = √(-1)
                    break;

                // Cupid's arrow
                case 34: // this is imaginary number I = √(-1)
                    break;

                // wings
                case 650:
                    SpecialistInstance specialistInstance = session.Character.Inventory.LoadBySlotAndType<SpecialistInstance>((byte)EquipmentType.Sp, InventoryType.Wear);
                    if (session.Character.UseSp && specialistInstance != null)
                    {
                        if (!delay)
                        {
                            session.SendPacket($"qna #u_i^1^{session.Character.CharacterId}^{(byte)inv.Type}^{inv.Slot}^3 {Language.Instance.GetMessageFromKey("ASK_WINGS_CHANGE")}");
                        }
                        else
                        {
                            specialistInstance.Design = (byte)EffectValue;
                            session.Character.MorphUpgrade2 = EffectValue;
                            session.CurrentMap?.Broadcast(session.Character.GenerateCMode());
                            session.SendPacket(session.Character.GenerateStat());
                            session.SendPacket(session.Character.GenerateStatChar());
                            session.Character.Inventory.RemoveItemAmountFromInventory(1, inv.Id);
                        }
                    }
                    else
                    {
                        session.SendPacket(session.Character.GenerateMsg(Language.Instance.GetMessageFromKey("NO_SP"), 0));
                    }
                    break;

                // presentation messages
                case 203:
                    if (!session.Character.IsVehicled)
                    {
                        if (!delay)
                        {
                            session.SendPacket(session.Character.GenerateGuri(10, 2, 1));
                        }
                    }
                    break;

                // magic lamps
                case 651:
                    if (session.Character.Inventory.GetAllItems().All(i => i.Type != InventoryType.Wear))
                    {
                        if (!delay)
                        {
                            session.SendPacket($"qna #u_i^1^{session.Character.CharacterId}^{(byte)inv.Type}^{inv.Slot}^3 {Language.Instance.GetMessageFromKey("ASK_USE")}");
                        }
                        else
                        {
                            session.Character.ChangeSex();
                            session.Character.Inventory.RemoveItemAmountFromInventory(1, inv.Id);
                        }
                    }
                    else
                    {
                        session.SendPacket(session.Character.GenerateMsg(Language.Instance.GetMessageFromKey("EQ_NOT_EMPTY"), 0));
                    }
                    break;

                // vehicles
                case 1000:
                    if (Morph > 0)
                    {
                        if (!delay && !session.Character.IsVehicled)
                        {
                            if (session.Character.IsSitting)
                            {
                                session.Character.IsSitting = false;
                                session.CurrentMap?.Broadcast(session.Character.GenerateRest());
                            }
                            session.SendPacket(session.Character.GenerateDelay(3000, 3, $"#u_i^1^{session.Character.CharacterId}^{(byte)inv.Type}^{inv.Slot}^2"));
                        }
                        else
                        {
                            if (!session.Character.IsVehicled)
                            {
                                session.Character.Speed = Speed;
                                session.Character.IsVehicled = true;
                                session.Character.VehicleSpeed = Speed;
                                session.Character.MorphUpgrade = 0;
                                session.Character.MorphUpgrade2 = 0;
                                session.Character.Morph = Morph + (byte)session.Character.Gender;
                                session.CurrentMap?.Broadcast(session.Character.GenerateEff(196), session.Character.MapX, session.Character.MapY);
                                session.CurrentMap?.Broadcast(session.Character.GenerateCMode());
                                session.SendPacket(session.Character.GenerateCond());
                            }
                            else
                            {
                                session.Character.RemoveVehicle();
                            }
                        }
                    }
                    break;
                case 1002:
                    if (EffectValue == 69)
                    {
                        int rnd = ServerManager.RandomNumber(0, 1000);
                        if (rnd < 5)
                        {
                            session.Character.GiftAdd(1160, 10);
                        }
                        else if (rnd < 15)
                        {
                            short[] specialVnums = new short[] { 5560, 5591, 4099, 907 };
                            session.Character.GiftAdd(specialVnums[ServerManager.RandomNumber(0,4)], 1);
                        }
                        else
                        {
                            short[] vnums = new short[] { 1160, 2282, 1030, 1244, 1218, 5369, 1012, 1363, 1364, 2160, 2173, 5959, 5983, 2514, 2515, 2516, 2517, 2518, 2519, 2520, 2521, 1685, 1686, 5087, 5203 };
                            byte[] counts = new byte[] { 1, 10, 20, 5, 1, 1, 99, 1, 1, 5, 5, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1 };
                            int item = ServerManager.RandomNumber(0, 25);
                            session.Character.GiftAdd(vnums[item], counts[item]);
                        }
                        session.Character.Inventory.RemoveItemAmountFromInventory(1, inv.Id);
                    }
                    else
                    {
                        if (session.HasCurrentMap)
                        {
                            short[] vnums = new short[] { 1386, 1387, 1388, 1389, 1390, 1391, 1392, 1393, 1394, 1395, 1396, 1397, 1398, 1399, 1400, 1401, 1402, 1403, 1404, 1405 };
                            short vnum = vnums[ServerManager.RandomNumber(0, 20)];

                            NpcMonster npcmonster = ServerManager.GetNpc(vnum);
                            if (npcmonster == null)
                            {
                                return;
                            }
                            // ReSharper disable once PossibleNullReferenceException HasCurrentMap NullCheck
                            MapMonster monster = new MapMonster { MonsterVNum = vnum, MapY = session.Character.MapY, MapX = session.Character.MapX, MapId = session.Character.MapId, Position = (byte)session.Character.Direction, IsMoving = true, MapMonsterId = session.CurrentMap.GetNextMonsterId(), ShouldRespawn = false };
                            monster.Initialize(session.CurrentMap);
                            monster.StartLife();
                            session.CurrentMap.AddMonster(monster);
                            session.CurrentMap.Broadcast(monster.GenerateIn3());
                            session.Character.Inventory.RemoveItemAmountFromInventory(1, inv.Id);
                        }
                    }
                    break;

                case 69:
                    session.Character.Reput += ReputPrice;
                    session.SendPacket(session.Character.GenerateFd());
                    session.Character.Inventory.RemoveItemAmountFromInventory(1, inv.Id);
                    break;

                default:
                    Logger.Log.Warn(string.Format(Language.Instance.GetMessageFromKey("NO_HANDLER_ITEM"), GetType()));
                    break;
            }
        }

        #endregion
    }
}