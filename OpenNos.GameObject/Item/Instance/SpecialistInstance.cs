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
using OpenNos.Data.Interfaces;
using OpenNos.Domain;
using OpenNos.GameObject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenNos.GameObject
{
    public class SpecialistInstance : WearableInstance, ISpecialistInstanceDTO
    {
        #region Members

        private readonly Random _random;

        private long _transportId;

        #endregion

        #region Instantiation

        public SpecialistInstance() => _random = new Random();

        public SpecialistInstance(Guid id)
        {
            Id = id;
            _random = new Random();
        }

        public SpecialistInstance(SpecialistInstanceDTO specialistInstance)
        {
            _random = new Random();
            SpDamage = specialistInstance.SpDamage;
            SpDark = specialistInstance.SpDark;
            SpDefence = specialistInstance.SpDefence;
            SpElement = specialistInstance.SpElement;
            SpFire = specialistInstance.SpFire;
            SpHP = specialistInstance.SpHP;
            SpLight = specialistInstance.SpLight;
            SpStoneUpgrade = specialistInstance.SpStoneUpgrade;
            SpWater = specialistInstance.SpWater;
            SpLevel = specialistInstance.SpLevel;
            SlDefence = specialistInstance.SlDefence;
            SlElement = specialistInstance.SlElement;
            SlDamage = specialistInstance.SlDamage;
            SlHP = specialistInstance.SlHP;
        }

        #endregion

        #region Properties

        public short SlDamage { get; set; }

        public short SlDefence { get; set; }

        public short SlElement { get; set; }

        public short SlHP { get; set; }

        public byte SpDamage { get; set; }

        public byte SpDark { get; set; }

        public byte SpDefence { get; set; }

        public byte SpElement { get; set; }

        public byte SpFire { get; set; }

        public byte SpHP { get; set; }

        public byte SpLevel { get; set; }

        public byte SpLight { get; set; }

        public byte SpStoneUpgrade { get; set; }

        public byte SpWater { get; set; }

        public long TransportId
        {
            get
            {
                if (_transportId == 0)
                {
                    // create transportId thru factory
                    _transportId = TransportFactory.Instance.GenerateTransportId();
                }

                return _transportId;
            }
        }

        #endregion

        #region Methods

        public string GeneratePslInfo()
        {
            // 1235.3 1237.4 1239.5 <= skills SkillVNum.Grade
            return $"pslinfo {Item.VNum} {Item.Element} {Item.ElementRate} {Item.LevelJobMinimum} {Item.Speed} {Item.FireResistance} {Item.WaterResistance} {Item.LightResistance} {Item.DarkResistance} 0.0 0.0 0.0";
        }

        public string GenerateSlInfo()
        {
            int freepoint = CharacterHelper.SPPoint(SpLevel, Upgrade) - SlDamage - SlHP - SlElement - SlDefence;

            int slElement = CharacterHelper.SlPoint(SlElement, 2);
            int slHp = CharacterHelper.SlPoint(SlHP, 3);
            int slDefence = CharacterHelper.SlPoint(SlDefence, 1);
            int slHit = CharacterHelper.SlPoint(SlDamage, 0);

            string skill = string.Empty;
            List<CharacterSkill> skillsSp = new List<CharacterSkill>();
            foreach (Skill ski in ServerManager.Instance.GetAllSkill().Where(ski => ski.Class == Item.Morph + 31 && ski.LevelMinimum <= SpLevel))
            {
                skillsSp.Add(new CharacterSkill
                {
                    SkillVNum = ski.SkillVNum,
                    CharacterId = CharacterId
                });
            }
            byte spdestroyed = 0;
            if (Rare == -2)
            {
                spdestroyed = 1;
            }
            if (skillsSp.Count == 0)
            {
                skill = "-1";
            }
            short firstskillvnum = skillsSp[0].SkillVNum;

            for (int i = 1; i < 11; i++)
            {
                if (skillsSp.Count >= i + 1 && skillsSp[i].SkillVNum <= firstskillvnum + 10)
                {
                    skill += $"{skillsSp[i].SkillVNum}.";
                }
            }

            // 10 9 8 '0 0 0 0'<- bonusdamage bonusarmor bonuselement bonushpmp its after upgrade and
            // 3 first values are not important
            skill = skill.TrimEnd('.');
            return $"slinfo {(Type == InventoryType.Wear || Type == InventoryType.Specialist || Type == InventoryType.Equipment ? "0" : "2")} {ItemVNum} {Item.Morph} {SpLevel} {Item.LevelJobMinimum} {Item.ReputationMinimum} 0 0 0 0 0 0 0 {Item.SpType} {Item.FireResistance} {Item.WaterResistance} {Item.LightResistance} {Item.DarkResistance} {XP} {CharacterHelper.SPXPData[SpLevel == 0 ? 0 : SpLevel - 1]} {skill} {TransportId} {freepoint} {slHit} {slDefence} {slElement} {slHp} {Upgrade} 0 0 {spdestroyed} 0 0 0 0 {SpStoneUpgrade} {SpDamage} {SpDefence} {SpElement} {SpHP} {SpFire} {SpWater} {SpLight} {SpDark}";
        }

        public void PerfectSP(ClientSession session)
        {
            short[] upsuccess = { 50, 40, 30, 20, 10 };

            int[] goldprice = { 5000, 10000, 20000, 50000, 100000 };
            byte[] stoneprice = { 1, 2, 3, 4, 5 };
            short stonevnum;
            byte upmode = 1;

            switch (Item.Morph)
            {
                case 2:
                case 6:
                case 9:
                case 12:
                    stonevnum = 2514;
                    break;

                case 3:
                case 4:
                case 14:
                    stonevnum = 2515;
                    break;

                case 5:
                case 11:
                case 15:
                    stonevnum = 2516;
                    break;

                case 10:
                case 13:
                case 7:
                    stonevnum = 2517;
                    break;

                case 17:
                case 18:
                case 19:
                    stonevnum = 2518;
                    break;

                case 20:
                case 21:
                case 22:
                    stonevnum = 2519;
                    break;

                case 23:
                case 24:
                case 25:
                    stonevnum = 2520;
                    break;

                case 26:
                case 27:
                case 28:
                    stonevnum = 2521;
                    break;

                default:
                    return;
            }
            if (SpStoneUpgrade > 99)
            {
                return;
            }
            if (SpStoneUpgrade > 80)
            {
                upmode = 5;
            }
            else if (SpStoneUpgrade > 60)
            {
                upmode = 4;
            }
            else if (SpStoneUpgrade > 40)
            {
                upmode = 3;
            }
            else if (SpStoneUpgrade > 20)
            {
                upmode = 2;
            }

            if (IsFixed)
            {
                return;
            }
            if (session.Character.Gold < goldprice[upmode - 1])
            {
                return;
            }
            if (session.Character.Inventory.CountItem(stonevnum) < stoneprice[upmode - 1])
            {
                return;
            }

            SpecialistInstance specialist = session.Character.Inventory.LoadByItemInstance<SpecialistInstance>(Id);

            int rnd = ServerManager.Instance.RandomNumber();
            if (rnd < upsuccess[upmode - 1])
            {
                byte type = (byte)ServerManager.Instance.RandomNumber(0, 16), count = 1;
                if (upmode == 4)
                {
                    count = 2;
                }
                if (upmode == 5)
                {
                    count = (byte)ServerManager.Instance.RandomNumber(3, 6);
                }

                session.CurrentMapInstance.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, session.Character.CharacterId, 3005), session.Character.MapX, session.Character.MapY);

                if (type < 3)
                {
                    specialist.SpDamage += count;
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_ATTACK"), count), 12));
                    session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_ATTACK"), count), 0));
                }
                else if (type < 6)
                {
                    specialist.SpDefence += count;
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_DEFENSE"), count), 12));
                    session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_DEFENSE"), count), 0));
                }
                else if (type < 9)
                {
                    specialist.SpElement += count;
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_ELEMENT"), count), 12));
                    session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_ELEMENT"), count), 0));
                }
                else if (type < 12)
                {
                    specialist.SpHP += count;
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_HPMP"), count), 12));
                    session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_HPMP"), count), 0));
                }
                else if (type == 12)
                {
                    specialist.SpFire += count;
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_FIRE"), count), 12));
                    session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_FIRE"), count), 0));
                }
                else if (type == 13)
                {
                    specialist.SpWater += count;
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_WATER"), count), 12));
                    session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_WATER"), count), 0));
                }
                else if (type == 14)
                {
                    specialist.SpLight += count;
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_LIGHT"), count), 12));
                    session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_LIGHT"), count), 0));
                }
                else if (type == 15)
                {
                    specialist.SpDark += count;
                    session.SendPacket(session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_SHADOW"), count), 12));
                    session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("PERFECTSP_SUCCESS"), Language.Instance.GetMessageFromKey("PERFECTSP_SHADOW"), count), 0));
                }
                specialist.SpStoneUpgrade++;
            }
            else
            {
                session.SendPacket(session.Character.GenerateSay(Language.Instance.GetMessageFromKey("PERFECTSP_FAILURE"), 11));
                session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("PERFECTSP_FAILURE"), 0));
            }
            session.SendPacket(specialist.GenerateInventoryAdd());
            session.Character.Gold -= goldprice[upmode - 1];
            session.SendPacket(session.Character.GenerateGold());
            session.Character.Inventory.RemoveItemAmount(stonevnum, stoneprice[upmode - 1]);
            session.SendPacket("shop_end 1");
        }

        public void UpgradeSp(ClientSession sesion, UpgradeProtection protect)
        {
            if (Upgrade >= 15)
            {
                return;
            }

            byte[] upfail = { 20, 25, 30, 40, 50, 60, 65, 70, 75, 80, 90, 93, 95, 97, 99 };
            byte[] destroy = { 0, 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 70 };

            int[] goldprice = { 200000, 200000, 200000, 200000, 200000, 500000, 500000, 500000, 500000, 500000, 1000000, 1000000, 1000000, 1000000, 1000000 };
            byte[] feather = { 3, 5, 8, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 70 };
            byte[] fullmoon = { 1, 3, 5, 7, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30 };
            byte[] soul = { 2, 4, 6, 8, 10, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5 };
            const short featherVnum = 2282;
            const short fullmoonVnum = 1030;
            const short greenSoulVnum = 2283;
            const short redSoulVnum = 2284;
            const short blueSoulVnum = 2285;
            const short dragonSkinVnum = 2511;
            const short dragonBloodVnum = 2512;
            const short dragonHeartVnum = 2513;
            const short blueScrollVnum = 1363;
            const short redScrollVnum = 1364;

            if (!sesion.HasCurrentMapInstance)
            {
                return;
            }
            if (sesion.Character.Inventory.CountItem(fullmoonVnum) < fullmoon[Upgrade])
            {
                sesion.SendPacket(sesion.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.Instance.GetItem(fullmoonVnum).Name, fullmoon[Upgrade])), 10));
                return;
            }
            if (sesion.Character.Inventory.CountItem(featherVnum) < feather[Upgrade])
            {
                sesion.SendPacket(sesion.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.Instance.GetItem(featherVnum).Name, feather[Upgrade])), 10));
                return;
            }
            if (sesion.Character.Gold < goldprice[Upgrade])
            {
                sesion.SendPacket(sesion.Character.GenerateSay(Language.Instance.GetMessageFromKey("NOT_ENOUGH_MONEY"), 10));
                return;
            }

            short itemToRemove = 2283;

            if (Upgrade < 5)
            {
                if (SpLevel > 20)
                {
                    if (Item.Morph <= 16)
                    {
                        if (sesion.Character.Inventory.CountItem(greenSoulVnum) < soul[Upgrade])
                        {
                            sesion.SendPacket(sesion.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.Instance.GetItem(greenSoulVnum).Name, soul[Upgrade])), 10));
                            return;
                        }
                        if (protect == UpgradeProtection.Protected)
                        {
                            if (sesion.Character.Inventory.CountItem(blueScrollVnum) < 1)
                            {
                                sesion.SendPacket(sesion.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.Instance.GetItem(blueScrollVnum).Name, 1)), 10));
                                return;
                            }
                            sesion.Character.Inventory.RemoveItemAmount(blueScrollVnum);
                            sesion.SendPacket("shop_end 2");
                        }
                    }
                    else
                    {
                        if (sesion.Character.Inventory.CountItem(dragonSkinVnum) < soul[Upgrade])
                        {
                            sesion.SendPacket(sesion.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.Instance.GetItem(dragonSkinVnum).Name, soul[Upgrade])), 10));
                            return;
                        }
                        if (protect == UpgradeProtection.Protected)
                        {
                            if (sesion.Character.Inventory.CountItem(blueScrollVnum) < 1)
                            {
                                sesion.SendPacket(sesion.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.Instance.GetItem(blueScrollVnum).Name, 1)), 10));
                                return;
                            }
                            sesion.Character.Inventory.RemoveItemAmount(blueScrollVnum);
                            sesion.SendPacket("shop_end 2");
                        }
                        itemToRemove = dragonSkinVnum;
                    }
                }
                else
                {
                    sesion.SendPacket(sesion.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("LVL_REQUIRED"), 21), 11));
                    return;
                }
            }
            else if (Upgrade < 10)
            {
                if (SpLevel > 40)
                {
                    if (Item.Morph <= 16)
                    {
                        if (sesion.Character.Inventory.CountItem(redSoulVnum) < soul[Upgrade])
                        {
                            sesion.SendPacket(sesion.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.Instance.GetItem(redSoulVnum).Name, soul[Upgrade])), 10));
                            return;
                        }
                        if (protect == UpgradeProtection.Protected)
                        {
                            if (sesion.Character.Inventory.CountItem(blueScrollVnum) < 1)
                            {
                                sesion.SendPacket(sesion.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.Instance.GetItem(blueScrollVnum).Name, 1)), 10));
                                return;
                            }
                            sesion.Character.Inventory.RemoveItemAmount(blueScrollVnum);
                            sesion.SendPacket("shop_end 2");
                        }
                        itemToRemove = redSoulVnum;
                    }
                    else
                    {
                        if (sesion.Character.Inventory.CountItem(dragonBloodVnum) < soul[Upgrade])
                        {
                            sesion.SendPacket(sesion.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.Instance.GetItem(dragonBloodVnum).Name, soul[Upgrade])), 10));
                            return;
                        }
                        if (protect == UpgradeProtection.Protected)
                        {
                            if (sesion.Character.Inventory.CountItem(blueScrollVnum) < 1)
                            {
                                sesion.SendPacket(sesion.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.Instance.GetItem(blueScrollVnum).Name, 1)), 10));
                                return;
                            }
                            sesion.Character.Inventory.RemoveItemAmount(blueScrollVnum);
                            sesion.SendPacket("shop_end 2");
                        }
                        itemToRemove = dragonBloodVnum;
                    }
                }
                else
                {
                    sesion.SendPacket(sesion.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("LVL_REQUIRED"), 41), 11));
                    return;
                }
            }
            else if (Upgrade < 15)
            {
                if (SpLevel > 50)
                {
                    if (Item.Morph <= 16)
                    {
                        if (sesion.Character.Inventory.CountItem(blueSoulVnum) < soul[Upgrade])
                        {
                            sesion.SendPacket(sesion.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.Instance.GetItem(blueSoulVnum).Name, soul[Upgrade])), 10));
                            return;
                        }
                        if (protect == UpgradeProtection.Protected)
                        {
                            if (sesion.Character.Inventory.CountItem(redScrollVnum) < 1)
                            {
                                return;
                            }
                            sesion.Character.Inventory.RemoveItemAmount(redScrollVnum);
                            sesion.SendPacket("shop_end 2");
                        }
                        itemToRemove = blueSoulVnum;
                    }
                    else
                    {
                        if (sesion.Character.Inventory.CountItem(dragonHeartVnum) < soul[Upgrade])
                        {
                            return;
                        }
                        if (protect == UpgradeProtection.Protected)
                        {
                            if (sesion.Character.Inventory.CountItem(redScrollVnum) < 1)
                            {
                                sesion.SendPacket(sesion.Character.GenerateSay(Language.Instance.GetMessageFromKey(string.Format(Language.Instance.GetMessageFromKey("NOT_ENOUGH_ITEMS"), ServerManager.Instance.GetItem(redScrollVnum).Name, 1)), 10));
                                return;
                            }
                            sesion.Character.Inventory.RemoveItemAmount(redScrollVnum);
                            sesion.SendPacket("shop_end 2");
                        }
                        itemToRemove = dragonHeartVnum;
                    }
                }
                else
                {
                    sesion.SendPacket(sesion.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("LVL_REQUIRED"), 51), 11));
                    return;
                }
            }

            sesion.Character.Gold -= goldprice[Upgrade];

            // remove feather and fullmoon before upgrading
            sesion.Character.Inventory.RemoveItemAmount(featherVnum, feather[Upgrade]);
            sesion.Character.Inventory.RemoveItemAmount(fullmoonVnum, fullmoon[Upgrade]);

            WearableInstance wearable = sesion.Character.Inventory.LoadByItemInstance<WearableInstance>(Id);
            ItemInstance inventory = sesion.Character.Inventory.GetItemInstanceById(Id);
            int rnd = ServerManager.Instance.RandomNumber();
            if (rnd < destroy[Upgrade])
            {
                if (protect == UpgradeProtection.Protected)
                {
                    sesion.CurrentMapInstance.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, sesion.Character.CharacterId, 3004), sesion.Character.MapX, sesion.Character.MapY);
                    sesion.SendPacket(sesion.Character.GenerateSay(Language.Instance.GetMessageFromKey("UPGRADESP_FAILED_SAVED"), 11));
                    sesion.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("UPGRADESP_FAILED_SAVED"), 0));
                }
                else
                {
                    sesion.Character.Inventory.RemoveItemAmount(itemToRemove, soul[Upgrade]);
                    wearable.Rare = -2;
                    sesion.SendPacket(sesion.Character.GenerateSay(Language.Instance.GetMessageFromKey("UPGRADESP_DESTROYED"), 11));
                    sesion.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("UPGRADESP_DESTROYED"), 0));
                    sesion.SendPacket(wearable.GenerateInventoryAdd());
                }
            }
            else if (rnd < upfail[Upgrade])
            {
                if (protect == UpgradeProtection.Protected)
                {
                    sesion.CurrentMapInstance.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, sesion.Character.CharacterId, 3004), sesion.Character.MapX, sesion.Character.MapY);
                }
                else
                {
                    sesion.Character.Inventory.RemoveItemAmount(itemToRemove, soul[Upgrade]);
                }
                sesion.SendPacket(sesion.Character.GenerateSay(Language.Instance.GetMessageFromKey("UPGRADESP_FAILED"), 11));
                sesion.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("UPGRADESP_FAILED"), 0));
            }
            else
            {
                if (protect == UpgradeProtection.Protected)
                {
                    sesion.CurrentMapInstance.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, sesion.Character.CharacterId, 3004), sesion.Character.MapX, sesion.Character.MapY);
                }
                sesion.Character.Inventory.RemoveItemAmount(itemToRemove, soul[Upgrade]);
                sesion.CurrentMapInstance.Broadcast(StaticPacketHelper.GenerateEff(UserType.Player, sesion.Character.CharacterId, 3005), sesion.Character.MapX, sesion.Character.MapY);
                sesion.SendPacket(sesion.Character.GenerateSay(Language.Instance.GetMessageFromKey("UPGRADESP_SUCCESS"), 12));
                sesion.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("UPGRADESP_SUCCESS"), 0));
                wearable.Upgrade++;
                if (wearable.Upgrade > 8)
                {
                    sesion.Character.Family?.InsertFamilyLog(FamilyLogType.ItemUpgraded, sesion.Character.Name, itemVNum: wearable.ItemVNum, upgrade: wearable.Upgrade);
                }
                sesion.SendPacket(wearable.GenerateInventoryAdd());
            }
            sesion.SendPacket(sesion.Character.GenerateGold());
            sesion.SendPacket(sesion.Character.GenerateEq());
            sesion.SendPacket("shop_end 1");
        }

        #endregion
    }
}