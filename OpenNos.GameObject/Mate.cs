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

using OpenNos.Data;
using OpenNos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenNos.GameObject
{
    public class Mate : MateDTO
    {
        #region Members

        private NpcMonster monster;

        private Character owner;

        #endregion

        #region Instantiation

        public Mate()
        {
        }

        public Mate(Character owner, NpcMonster npcMonster, byte level, MateType matetype)
        {
            NpcMonsterVNum = npcMonster.NpcMonsterVNum;
            Monster = npcMonster;
            Hp = npcMonster.MaxHP;
            Mp = npcMonster.MaxMP;
            Name = npcMonster.Name;
            MateType = matetype;
            Level = level;
            Loyalty = 1000;
            PositionY = (short)(owner.PositionY + 1);
            PositionX = (short)(owner.PositionX + 1);
            MapX = (short)(owner.PositionX + 1);
            MapY = (short)(owner.PositionY + 1);
            Direction = 2;
            CharacterId = owner.CharacterId;
            Owner = owner;
            GeneateMateTransportId();
        }

        #endregion

        #region Properties

        public bool IsSitting { get; set; }

        public int MateTransportId { get; set; }

        public int MaxHp => Monster.MaxHP;

        public int MaxMp => Monster.MaxMP;

        public int MinHit
        {
            get
            {
                const int dmg = 100; //TODO: get proper Damage
                return Monster.DamageMinimum + dmg;
            }
        }

        public int MaxHit
        {
            get
            {
                const int dmg = 100; //TODO: get proper Damage
                return Monster.DamageMaximum + dmg;
            }
        }

        public NpcMonster Monster
        {
            get
            {
                return monster ?? (monster = ServerManager.Instance.GetNpc(NpcMonsterVNum));
            }
            set
            {
                monster = value;
            }
        }

        public Character Owner
        {
            get
            {
                return owner ?? (owner = ServerManager.Instance.GetSessionByCharacterId(CharacterId).Character);
            }
            set
            {
                owner = value;
            }
        }

        public byte PetId { get; set; }

        public short PositionX { get; set; }

        public short PositionY { get; set; }

        public int MeleeDefense
        {
            get
            {
                return Monster.CloseDefence; //TODO: get proper Defense
            }
        }

        public int MeleeDefenseRate
        {
            get
            {
                return Monster.DefenceDodge; //TODO: get proper Defense
            }
        }

        public int RangeDefense
        {
            get
            {
                return Monster.DistanceDefence; //TODO: get proper Defense
            }
        }

        public int RangeDefenseRate
        {
            get
            {
                return Monster.DistanceDefenceDodge; //TODO: get proper Defense
            }
        }

        public int MagicalDefense
        {
            get
            {
                return Monster.MagicDefence; //TODO: get proper Defense
            }
        }

        #endregion

        #region Methods

        public void GeneateMateTransportId()
        {
            int nextId = ServerManager.Instance.MateIds.Count > 0 ? ServerManager.Instance.MateIds.Last() + 1 : 2000000;
            ServerManager.Instance.MateIds.Add(nextId);
            MateTransportId = nextId;
        }

        public string GenerateCMode(short morphId)
        {
            return $"c_mode 2 {MateTransportId} {morphId} 0 0";
        }

        public EffectPacket GenerateEff(int effectid)
        {
            return new EffectPacket
            {
                EffectType = 2,
                CharacterId = MateTransportId,
                Id = effectid
            };
        }

        public string GenerateEInfo()
        {
            return $"e_info 10 {NpcMonsterVNum} {Level} {Monster.Element} {Monster.AttackClass} {Monster.ElementRate} {Monster.AttackUpgrade} {Monster.DamageMinimum} {Monster.DamageMaximum} {Monster.Concentrate} {Monster.CriticalChance} {Monster.CriticalRate} {Monster.DefenceUpgrade} {Monster.CloseDefence} {Monster.DefenceDodge} {Monster.DistanceDefence} {Monster.DistanceDefenceDodge} {Monster.MagicDefence} {Monster.FireResistance} {Monster.WaterResistance} {Monster.LightResistance} {Monster.DarkResistance} {Monster.MaxHP} {Monster.MaxMP} -1 {Name.Replace(' ', '^')}";
        }

        public string GenerateIn(bool foe = false)
        {
            string _name = Name.Replace(' ', '^');
            if (foe)
            {
                _name = "!§$%&/()=?*+~#";
            }
            int _faction = 0;
            if (ServerManager.Instance.ChannelId == 51)
            {
                _faction = (byte)Owner.Faction + 2;
            }
            return $"in 2 {NpcMonsterVNum} {MateTransportId} {(IsTeamMember ? PositionX : MapX)} {(IsTeamMember ? PositionY : MapY)} {Direction} {(int)(Hp / (float)MaxHp * 100)} {(int)(Mp / (float)MaxMp * 100)} 0 {_faction} 3 {CharacterId} 1 0 {(Skin != 0 ? Skin : -1)} {_name} 0 -1 0 0 0 0 0 0 0 0";
        }

        public string GenerateOut()
        {
            return $"out 2 {MateTransportId}";
        }

        public string GenerateRest()
        {
            IsSitting = !IsSitting;
            return $"rest 2 {MateTransportId} {(IsSitting ? 1 : 0)}";
        }

        public string GenerateSay(string message, int type)
        {
            return $"say 2 {MateTransportId} {type} {message}";
        }

        public string GenerateScPacket()
        {
            switch (MateType)
            {
                case MateType.Partner:
                    List<ItemInstance> items = GetInventory();
                    ItemInstance Weapon = items.Find(s => s.Slot == (short)EquipmentType.MainWeapon);
                    ItemInstance Armor = items.Find(s => s.Slot == (short)EquipmentType.Armor);
                    ItemInstance Gloves = items.Find(s => s.Slot == (short)EquipmentType.Gloves);
                    ItemInstance Boots = items.Find(s => s.Slot == (short)EquipmentType.Boots);
                    return $"sc_n {PetId} {NpcMonsterVNum} {MateTransportId} {Level} {Loyalty} {Experience} {(Weapon != null ? $"{Weapon.ItemVNum}.{Weapon.Rare}.{Weapon.Upgrade}" : "-1")} {(Armor != null ? $"{Armor.ItemVNum}.{Armor.Rare}.{Armor.Upgrade}" : "-1")} {(Gloves != null ? $"{Gloves.ItemVNum}.0.0" : "-1")} {(Boots != null ? $"{Boots.ItemVNum}.0.0" : "-1")} 0 0 1 0 142 174 232 4 70 0 73 158 86 158 69 0 0 0 0 0 2641 2641 1065 1065 0 285816 {Name.Replace(' ', '^')} {(Skin != 0 ? Skin : -1)} {(IsSummonable ? 1 : 0)} -1 -1 -1 -1";

                case MateType.Pet:
                    return $"sc_p {PetId} {NpcMonsterVNum} {MateTransportId} {Level} {Loyalty} {Experience} 0 0 {Monster.AttackUpgrade} {Monster.DamageMinimum} {Monster.DamageMaximum} {Monster.Concentrate} {Monster.CriticalChance} {Monster.CriticalRate} {Monster.DefenceUpgrade} {Monster.CloseDefence} {Monster.DefenceDodge} {Monster.DistanceDefence} {Monster.DistanceDefenceDodge} {Monster.MagicDefence} {Monster.FireResistance} {Monster.WaterResistance} {Monster.LightResistance} {Monster.DarkResistance} {Hp} {MaxHp} {Mp} {MaxMp} 0 15 {(CanPickUp ? 1 : 0)} {Name.Replace(' ', '^')} {(IsSummonable ? 1 : 0)}";
            }
            return string.Empty;
        }

        public string GenerateStatInfo()
        {
            return $"st 2 {MateTransportId} {Level} 0 {(int)(Hp / (float)MaxHp * 100)} {(int)(Mp / (float)MaxMp * 100)} {Hp} {Mp}";
        }

        public List<ItemInstance> GetInventory()
        {
            switch (PetId)
            {
                case 0:
                    return Owner.Inventory.GetAllItems().Where(s => s.Type == InventoryType.FirstPartnerInventory).ToList();

                case 1:
                    return Owner.Inventory.GetAllItems().Where(s => s.Type == InventoryType.SecondPartnerInventory).ToList();

                case 2:
                    return Owner.Inventory.GetAllItems().Where(s => s.Type == InventoryType.ThirdPartnerInventory).ToList();
            }
            return new List<ItemInstance>();
        }

        public override void Initialize()
        {
        }

        /// <summary>
        /// Checks if the current character is in range of the given position
        /// </summary>
        /// <param name="xCoordinate">The x coordinate of the object to check.</param>
        /// <param name="yCoordinate">The y coordinate of the object to check.</param>
        /// <param name="range">The range of the coordinates to be maximal distanced.</param>
        /// <returns>True if the object is in Range, False if not.</returns>
        public bool IsInRange(int xCoordinate, int yCoordinate, int range)
        {
            return Math.Abs(PositionX - xCoordinate) <= range && Math.Abs(PositionY - yCoordinate) <= range;
        }

        #endregion
    }
}