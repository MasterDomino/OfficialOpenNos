﻿using OpenNos.Domain;
using System.Collections.Generic;
using System.Linq;

namespace OpenNos.GameObject
{
    public class BattleEntity
    {
        public BattleEntity(Character character, Skill skill)
        {
            Session = character.Session;
            Buffs = character.Buff.ToList();
            BCards = character.EquipmentBCards.ToList();
            Level = character.Level;
            EntityType = EntityType.Player;
            DamageMinimum = character.MinHit;
            DamageMaximum = character.MaxHit;
            Hitrate = character.HitRate;
            CritChance = character.HitCriticalRate;
            CritRate = character.HitCritical;
            Morale = character.Level;
            FireResistance = character.FireResistance;
            WaterResistance = character.WaterResistance;
            LightResistance = character.LightResistance;
            ShadowResistance = character.DarkResistance;
            PositionX = character.PositionX;
            PositionY = character.PositionY;

            WearableInstance weapon = null;

            if (skill != null)
            {
                switch (skill.Type)
                {
                    case 0:
                        AttackType = AttackType.Melee;
                        if (character.Class == ClassType.Archer)
                        {
                            DamageMinimum = character.MinDistance;
                            DamageMaximum = character.MaxDistance;
                            Hitrate = character.DistanceRate;
                            CritChance = character.DistanceCriticalRate;
                            CritRate = character.DistanceCritical;
                            weapon = character.Inventory.LoadBySlotAndType<WearableInstance>((byte)EquipmentType.SecondaryWeapon, InventoryType.Wear);
                        }
                        else
                        {
                            weapon = character.Inventory.LoadBySlotAndType<WearableInstance>((byte)EquipmentType.MainWeapon, InventoryType.Wear);
                        }
                        break;
                    case 1:
                        AttackType = AttackType.Range;
                        if (character.Class == ClassType.Adventurer || character.Class == ClassType.Swordman || character.Class == ClassType.Magician)
                        {
                            DamageMinimum = character.MinDistance;
                            DamageMaximum = character.MaxDistance;
                            Hitrate = character.DistanceRate;
                            CritChance = character.DistanceCriticalRate;
                            CritRate = character.DistanceCritical;
                            weapon = character.Inventory.LoadBySlotAndType<WearableInstance>((byte)EquipmentType.SecondaryWeapon, InventoryType.Wear);
                        }
                        else
                        {
                            weapon = character.Inventory.LoadBySlotAndType<WearableInstance>((byte)EquipmentType.MainWeapon, InventoryType.Wear);
                        }
                        break;
                    case 2:
                        AttackType = AttackType.Magical;
                        weapon = character.Inventory.LoadBySlotAndType<WearableInstance>((byte)EquipmentType.MainWeapon, InventoryType.Wear);
                        break;
                    case 3:
                        weapon = character.Inventory.LoadBySlotAndType<WearableInstance>((byte)EquipmentType.MainWeapon, InventoryType.Wear);
                        switch (character.Class)
                        {
                            case ClassType.Adventurer:
                            case ClassType.Swordman:
                                AttackType = AttackType.Melee;
                                break;
                            case ClassType.Archer:
                                AttackType = AttackType.Range;
                                break;
                            case ClassType.Magician:
                                AttackType = AttackType.Magical;
                                break;
                        }
                        break;
                    case 5:
                        AttackType = AttackType.Melee;
                        switch (character.Class)
                        {
                            case ClassType.Adventurer:
                            case ClassType.Swordman:
                            case ClassType.Magician:
                                weapon = character.Inventory.LoadBySlotAndType<WearableInstance>((byte)EquipmentType.MainWeapon, InventoryType.Wear);
                                break;
                            case ClassType.Archer:
                                weapon = character.Inventory.LoadBySlotAndType<WearableInstance>((byte)EquipmentType.SecondaryWeapon, InventoryType.Wear);
                                break;

                        }
                        break;
                }
            }
            else
            {
                weapon = character.Inventory.LoadBySlotAndType<WearableInstance>((byte)EquipmentType.SecondaryWeapon, InventoryType.Wear);
                switch (character.Class)
                {
                    case ClassType.Adventurer:
                    case ClassType.Swordman:
                        AttackType = AttackType.Melee;
                        break;
                    case ClassType.Archer:
                        AttackType = AttackType.Range;
                        break;
                    case ClassType.Magician:
                        AttackType = AttackType.Magical;
                        break;
                }
            }

            if (weapon != null)
            {
                AttackUpgrade = weapon.Upgrade;
                WeaponDamageMinimum = weapon.DamageMinimum;
                WeaponDamageMaximum = weapon.DamageMaximum;
            }

            WearableInstance armor = character.Inventory.LoadBySlotAndType<WearableInstance>((byte)EquipmentType.Armor, InventoryType.Wear);
            if (armor != null)
            {
                DefenseUpgrade = armor.Upgrade;
                ArmorMeleeDefense = armor.CloseDefence;
                ArmorRangeDefense = armor.DistanceDefence;
                ArmorMagicalDefense = armor.MagicDefence;
            }

            MeleeDefense = character.Defence;
            MeleeDefenseDodge = character.DefenceRate;
            RangeDefense = character.DistanceDefence;
            RangeDefenseDodge = character.DistanceDefenceRate;
            MagicalDefense = character.MagicalDefence;

            Element = character.Element;
            ElementRate = character.ElementRate + character.ElementRateSP;
        }

        public BattleEntity(Mate mate)
        {
            //mate.Buff.CopyTo(Buffs);
            BCards = mate.Monster.BCards.ToList();
            Level = mate.Level;
            EntityType = EntityType.Mate;
            DamageMinimum = mate.MinHit;
            DamageMaximum = mate.MaxHit;
            WeaponDamageMinimum = mate.MinHit;
            WeaponDamageMaximum = mate.MaxHit;
            Hitrate = mate.Monster.Concentrate;
            CritChance = mate.Monster.CriticalChance;
            CritRate = mate.Monster.CriticalRate;
            Morale = mate.Level;
            AttackUpgrade = mate.Attack;
            FireResistance = mate.Monster.FireResistance;
            WaterResistance = mate.Monster.WaterResistance;
            LightResistance = mate.Monster.LightResistance;
            ShadowResistance = mate.Monster.DarkResistance;
            PositionX = mate.PositionX;
            PositionY = mate.PositionY;

            switch (mate.Monster.AttackClass)
            {
                case 0:
                    AttackType = AttackType.Melee;
                    break;
                case 1:
                    AttackType = AttackType.Range;
                    break;
                case 2:
                    AttackType = AttackType.Magical;
                    break;
            }

            DefenseUpgrade = mate.Defence;
            MeleeDefense = mate.MeleeDefense;
            MeleeDefenseDodge = mate.MeleeDefenseRate;
            RangeDefense = mate.RangeDefense;
            RangeDefenseDodge = mate.RangeDefenseRate;
            MagicalDefense = mate.MagicalDefense;
            ArmorMeleeDefense = mate.Monster.CloseDefence;
            ArmorRangeDefense = mate.Monster.DistanceDefence;
            ArmorMagicalDefense = mate.Monster.MagicDefence;

            Element = mate.Monster.Element;
            ElementRate = mate.Monster.ElementRate;
        }

        public BattleEntity(MapMonster monster)
        {
            //monster.Buff.CopyTo(Buffs);
            BCards = monster.Monster.BCards.ToList();
            Level = monster.Monster.Level;
            EntityType = EntityType.Monster;
            DamageMinimum = monster.Monster.DamageMinimum;
            DamageMaximum = monster.Monster.DamageMaximum;
            WeaponDamageMinimum = monster.Monster.DamageMinimum;
            WeaponDamageMaximum = monster.Monster.DamageMaximum;
            Hitrate = monster.Monster.Concentrate;
            CritChance = monster.Monster.CriticalChance;
            CritRate = monster.Monster.CriticalRate;
            Morale = monster.Monster.Level;
            AttackUpgrade = monster.Monster.AttackUpgrade;
            FireResistance = monster.Monster.FireResistance;
            WaterResistance = monster.Monster.WaterResistance;
            LightResistance = monster.Monster.LightResistance;
            ShadowResistance = monster.Monster.DarkResistance;
            PositionX = monster.MapX;
            PositionY = monster.MapY;

            switch (monster.Monster.AttackClass)
            {
                case 0:
                    AttackType = AttackType.Melee;
                    break;
                case 1:
                    AttackType = AttackType.Range;
                    break;
                case 2:
                    AttackType = AttackType.Magical;
                    break;
            }

            DefenseUpgrade = monster.Monster.DefenceUpgrade;
            MeleeDefense = monster.Monster.CloseDefence;
            MeleeDefenseDodge = monster.Monster.DefenceDodge;
            RangeDefense = monster.Monster.DistanceDefence;
            RangeDefenseDodge = monster.Monster.DistanceDefenceDodge;
            MagicalDefense = monster.Monster.MagicDefence;
            ArmorMeleeDefense = monster.Monster.CloseDefence;
            ArmorRangeDefense = monster.Monster.DistanceDefence;
            ArmorMagicalDefense = monster.Monster.MagicDefence;

            Element = monster.Monster.Element;
            ElementRate = monster.Monster.ElementRate;
        }

        public BattleEntity(MapNpc npc)
        {
            //npc.Buff.CopyTo(Buffs);
            BCards = npc.Npc.BCards.ToList();
            Level = npc.Npc.Level;
            EntityType = EntityType.Monster;
            DamageMinimum = npc.Npc.DamageMinimum;
            DamageMaximum = npc.Npc.DamageMaximum;
            WeaponDamageMinimum = npc.Npc.DamageMinimum;
            WeaponDamageMaximum = npc.Npc.DamageMaximum;
            Hitrate = npc.Npc.Concentrate;
            CritChance = npc.Npc.CriticalChance;
            CritRate = npc.Npc.CriticalRate;
            Morale = npc.Npc.Level;
            AttackUpgrade = npc.Npc.AttackUpgrade;
            FireResistance = npc.Npc.FireResistance;
            WaterResistance = npc.Npc.WaterResistance;
            LightResistance = npc.Npc.LightResistance;
            ShadowResistance = npc.Npc.DarkResistance;
            PositionX = npc.MapX;
            PositionY = npc.MapY;

            switch (npc.Npc.AttackClass)
            {
                case 0:
                    AttackType = AttackType.Melee;
                    break;
                case 1:
                    AttackType = AttackType.Range;
                    break;
                case 2:
                    AttackType = AttackType.Magical;
                    break;
            }

            DefenseUpgrade = npc.Npc.DefenceUpgrade;
            MeleeDefense = npc.Npc.CloseDefence;
            MeleeDefenseDodge = npc.Npc.DefenceDodge;
            RangeDefense = npc.Npc.DistanceDefence;
            RangeDefenseDodge = npc.Npc.DistanceDefenceDodge;
            MagicalDefense = npc.Npc.MagicDefence;
            ArmorMeleeDefense = npc.Npc.CloseDefence;
            ArmorRangeDefense = npc.Npc.DistanceDefence;
            ArmorMagicalDefense = npc.Npc.MagicDefence;

            Element = npc.Npc.Element;
            ElementRate = npc.Npc.ElementRate;
        }

        public byte Level { get; set; }

        public ClientSession Session { get; set; }

        public EntityType EntityType { get; set; }

        public List<Buff> Buffs { get; set; }

        public List<BCard> BCards { get; set; }

        public bool Invincible { get; set; }

        public int DamageMinimum { get; set; }

        public int DamageMaximum { get; set; }

        public int WeaponDamageMinimum { get; set; }

        public int WeaponDamageMaximum { get; set; }

        public int Morale { get; set; }

        public short AttackUpgrade { get; set; }

        public int Hitrate { get; set; }

        public int CritChance { get; set; }

        public int CritRate { get; set; }

        public AttackType AttackType { get; set; }

        public byte DefenseUpgrade { get; set; }

        public int Defense { get; set; }

        public int ArmorDefense { get; set; }

        public int Dodge { get; set; }

        public int MeleeDefense { get; set; }
        public int ArmorMeleeDefense { get; set; }

        public int MeleeDefenseDodge { get; set; }

        public int RangeDefense { get; set; }
        public int ArmorRangeDefense { get; set; }

        public int RangeDefenseDodge { get; set; }

        public int MagicalDefense { get; set; }
        public int ArmorMagicalDefense { get; set; }

        public int FireResistance { get; set; }

        public int WaterResistance { get; set; }

        public int LightResistance { get; set; }

        public int ShadowResistance { get; set; }

        public int Resistance { get; set; }

        public byte Element { get; set; }

        public int ElementRate { get; set; }

        public short PositionX { get; set; }

        public short PositionY { get; set; }

    }
}