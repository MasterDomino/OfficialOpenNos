using OpenNos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using static OpenNos.Domain.BCardType;

namespace OpenNos.GameObject
{
    public class DamageHelper
    {
        private static DamageHelper instance;

        public static DamageHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DamageHelper();
                }
                return instance;
            }
        }

        /// <summary>
        /// Calculates the damage attacker inflicts defender
        /// </summary>
        /// <param name="attacker">The attacking Entity</param>
        /// <param name="defender">The defending Entity</param>
        /// <param name="skill">The used Skill</param>
        /// <param name="hitMode">reference to HitMode</param>
        /// <returns>Damage</returns>
        public int CalculateDamage(BattleEntity attacker, BattleEntity defender, Skill skill, ref int hitMode)
        {
            if (skill != null)
            {
                attacker.BCards.AddRange(skill.BCards);
            }
            #region Basic Buff Initialisation

            attacker.Morale += GetBuff(attacker.Level, attacker.Buffs, attacker.BCards, CardType.Morale, (byte)AdditionalTypes.Morale.MoraleIncreased)[0];
            defender.Morale += GetBuff(defender.Level, defender.Buffs, defender.BCards, CardType.Morale, (byte)AdditionalTypes.Morale.MoraleIncreased)[0];

            attacker.AttackUpgrade += (short)GetBuff(attacker.Level, attacker.Buffs, attacker.BCards, CardType.AttackPower, (byte)AdditionalTypes.AttackPower.AttackLevelIncreased)[0];
            defender.DefenseUpgrade += (short)GetBuff(defender.Level, defender.Buffs, defender.BCards, CardType.Defence, (byte)AdditionalTypes.Defence.DefenceLevelIncreased)[0];


            /*
             * 
             * Percentage Boost categories:
             *  1.: Adds to Total Damage
             *  2.: Adds to Normal Damage
             *  3.: Adds to Base Damage
             *  4.: Adds to Defense
             *  5.: Adds to Element
             * 
             * Buff Effects get added, whereas
             * Shell Effects get multiplied afterwards.
             * 
             * Simplified Example on Defense (Same for Attack):
             *  - 1k Defense
             *  - Costume(+5% Defense)
             *  - Defense Potion(+20% Defense)
             *  - S-Defense Shell with 20% Boost
             *  
             * Calculation:
             *  1000 * 1.25 * 1.2 = 1500
             *  Def    Buff   Shell Total
             *  
             * Keep in Mind that after each step, one has
             * to round the current value down if necessary
             * 
             * Static Boost categories:
             *  1.: Adds to Total Damage
             *  2.: Adds to Normal Damage
             *  3.: Adds to Base Damage
             *  4.: Adds to Defense
             *  5.: Adds to Element
             *  
             */

            double boostCategory1 = 1;
            double boostCategory2 = 1;
            double boostCategory3 = 1;
            double boostCategory4 = 1;
            double boostCategory5 = 1;
            double shellBoostCategory1 = 1;
            double shellBoostCategory2 = 1;
            double shellBoostCategory3 = 1;
            double shellBoostCategory4 = 1;
            double shellBoostCategory5 = 1;
            int staticBoostCategory1 = 0;
            int staticBoostCategory2 = 0;
            int staticBoostCategory3 = 0;
            int staticBoostCategory4 = 0;
            int staticBoostCategory5 = 0;

            staticBoostCategory3 += (short)GetBuff(attacker.Level, attacker.Buffs, attacker.BCards, CardType.AttackPower, (byte)AdditionalTypes.AttackPower.AllAttacksIncreased)[0];
            staticBoostCategory3 += (short)GetBuff(defender.Level, defender.Buffs, defender.BCards, CardType.AttackPower, (byte)AdditionalTypes.AttackPower.AllAttacksIncreased)[0];

            #endregion

            #region Attack Type Related Variables

            switch (attacker.AttackType)
            {
                case AttackType.Melee:
                    defender.Defense = defender.MeleeDefense;
                    defender.ArmorDefense = defender.ArmorMeleeDefense;
                    defender.Dodge = defender.MeleeDefenseDodge;
                    break;
                case AttackType.Range:
                    defender.Defense = defender.RangeDefense;
                    defender.ArmorDefense = defender.ArmorRangeDefense;
                    defender.Dodge = defender.RangeDefenseDodge;
                    break;
                case AttackType.Magical:
                    defender.Defense = defender.MagicalDefense;
                    defender.ArmorDefense = defender.ArmorMagicalDefense;
                    break;
            }

            #endregion

            #region Too Near Range Attack Penalty (boostCategory2)

            if (attacker.AttackType == AttackType.Range)
            {
                if (Map.GetDistance(new MapCell { X = attacker.PositionX, Y = attacker.PositionY }, new MapCell { X = defender.PositionX, Y = defender.PositionY }) < 4)
                {
                    boostCategory2 -= 0.3;
                }
            }

            #endregion

            #region Morale and Dodge

            attacker.Morale -= defender.Morale;

            if (attacker.AttackType != AttackType.Magical)
            {
                double multiplier = defender.Dodge / (attacker.Hitrate + attacker.Morale);
                if (multiplier > 5)
                {
                    multiplier = 5;
                }
                double chance = -0.25 * Math.Pow(multiplier, 3) - 0.57 * Math.Pow(multiplier, 2) + 25.3 * multiplier - 1.41;
                if (chance <= 1)
                {
                    chance = 1;
                }
                //if (GetBuff(CardType.Buff, (byte)AdditionalTypes.DodgeAndDefencePercent.)[0] != 0)    TODO: Eagle Eyes AND Other Fixed Hitrates
                //{
                //    chance = 10;
                //}
                if (!defender.Invincible)
                {
                    if (ServerManager.Instance.RandomNumber() < chance)
                    {
                        hitMode = 1;
                        return 0;
                    }
                }
            }

            #endregion

            #region Base Damage

            int baseDamage = ServerManager.Instance.RandomNumber(attacker.DamageMinimum, attacker.DamageMaximum + 1);
            int weaponDamage = ServerManager.Instance.RandomNumber(attacker.WeaponDamageMinimum, attacker.WeaponDamageMaximum + 1);
            
            #region Attack Level Calculation

            attacker.AttackUpgrade -= defender.DefenseUpgrade;

            if (attacker.AttackUpgrade < -10)
            {
                attacker.AttackUpgrade = -10;
            }
            else if (attacker.AttackUpgrade > 10)
            {
                attacker.AttackUpgrade = 10;
            }

            switch (attacker.AttackUpgrade)
            {
                case 0:
                    weaponDamage = 0;
                    break;
                case 1:
                    weaponDamage = (int)(weaponDamage * 0.1);
                    break;

                case 2:
                    weaponDamage = (int)(weaponDamage * 0.15);
                    break;

                case 3:
                    weaponDamage = (int)(weaponDamage * 0.22);
                    break;

                case 4:
                    weaponDamage = (int)(weaponDamage * 0.32);
                    break;

                case 5:
                    weaponDamage = (int)(weaponDamage * 0.43);
                    break;

                case 6:
                    weaponDamage = (int)(weaponDamage * 0.54);
                    break;

                case 7:
                    weaponDamage = (int)(weaponDamage * 0.65);
                    break;

                case 8:
                    weaponDamage = (int)(weaponDamage * 0.9);
                    break;

                case 9:
                    weaponDamage = (int)(weaponDamage * 1.2);
                    break;

                case 10:
                    weaponDamage = weaponDamage * 2;
                    break;
            }

            #endregion

            baseDamage = (int)((int)((baseDamage + staticBoostCategory3 + weaponDamage + 15) * boostCategory3) * shellBoostCategory3);
            #endregion

            #region Defense

            switch (attacker.AttackUpgrade)
            {
                case -10:
                    defender.ArmorDefense = defender.ArmorDefense * 2;
                    break;

                case -9:
                    defender.ArmorDefense = (int)(defender.ArmorDefense * 1.2);
                    break;

                case -8:
                    defender.ArmorDefense = (int)(defender.ArmorDefense * 0.9);
                    break;

                case -7:
                    defender.ArmorDefense = (int)(defender.ArmorDefense * 0.65);
                    break;

                case -6:
                    defender.ArmorDefense = (int)(defender.ArmorDefense * 0.54);
                    break;

                case -5:
                    defender.ArmorDefense = (int)(defender.ArmorDefense * 0.43);
                    break;

                case -4:
                    defender.ArmorDefense = (int)(defender.ArmorDefense * 0.32);
                    break;

                case -3:
                    defender.ArmorDefense = (int)(defender.ArmorDefense * 0.22);
                    break;

                case -2:
                    defender.ArmorDefense = (int)(defender.ArmorDefense * 0.15);
                    break;

                case -1:
                    defender.ArmorDefense = (int)(defender.ArmorDefense * 0.1);
                    break;
                case 0:
                    defender.ArmorDefense = 0;
                    break;
            }

            int defense = (int)((int)((defender.Defense + defender.ArmorDefense + staticBoostCategory4) * boostCategory4) * shellBoostCategory4);

            #endregion

            #region Normal Damage

            int normalDamage = (int)((int)((baseDamage + staticBoostCategory2 - defense) * boostCategory2) * shellBoostCategory2);

            if (normalDamage < 0)
            {
                normalDamage = 0;
            }

            #endregion

            #region Crit Damage

            if (ServerManager.Instance.RandomNumber() < attacker.CritChance)
            {
                if (attacker.AttackType != AttackType.Magical)
                {
                    double multiplier = attacker.CritRate / 100D;
                    if (multiplier > 3)
                    {
                        multiplier = 3;
                    }
                    normalDamage += (int)(normalDamage * multiplier);
                    hitMode = 3;
                }
            }

            #endregion

            #region Fairy Damage

            int fairyDamage = (int)((baseDamage + 100) * attacker.ElementRate / 100D);

            #endregion

            #region Elemental Damage Advantage

            double elementalBoost = 0;

            switch (attacker.Element)
            {
                case 0:
                    break;

                case 1:
                    defender.Resistance = defender.FireResistance;
                    switch (defender.Element)
                    {
                        case 0:
                            elementalBoost = 1.3; // Damage vs no element
                            break;

                        case 1:
                            elementalBoost = 1; // Damage vs fire
                            break;

                        case 2:
                            elementalBoost = 2; // Damage vs water
                            break;

                        case 3:
                            elementalBoost = 1; // Damage vs light
                            break;

                        case 4:
                            elementalBoost = 1.5; // Damage vs darkness
                            break;
                    }
                    break;

                case 2:
                    defender.Resistance = defender.WaterResistance;
                    switch (defender.Element)
                    {
                        case 0:
                            elementalBoost = 1.3;
                            break;

                        case 1:
                            elementalBoost = 2;
                            break;

                        case 2:
                            elementalBoost = 1;
                            break;

                        case 3:
                            elementalBoost = 1.5;
                            break;

                        case 4:
                            elementalBoost = 1;
                            break;
                    }
                    break;

                case 3:
                    defender.Resistance = defender.LightResistance;
                    switch (defender.Element)
                    {
                        case 0:
                            elementalBoost = 1.3;
                            break;

                        case 1:
                            elementalBoost = 1.5;
                            break;

                        case 2:
                            elementalBoost = 1;
                            break;

                        case 3:
                            elementalBoost = 1;
                            break;

                        case 4:
                            elementalBoost = 3;
                            break;
                    }
                    break;

                case 4:
                    defender.Resistance = defender.ShadowResistance;
                    switch (defender.Element)
                    {
                        case 0:
                            elementalBoost = 1.3;
                            break;

                        case 1:
                            elementalBoost = 1;
                            break;

                        case 2:
                            elementalBoost = 1.5;
                            break;

                        case 3:
                            elementalBoost = 3;
                            break;

                        case 4:
                            elementalBoost = 1;
                            break;
                    }
                    break;
            }

            if (skill?.Element == 0 || skill?.Element != attacker.Element)
            {
                elementalBoost = 0;
            }

            #endregion

            #region Elemental Damage 


            int elementalDamage = (int)((int)((int)((int)((staticBoostCategory5 + fairyDamage) * elementalBoost) * (1 - defender.Resistance / 100D))*boostCategory5)*shellBoostCategory5);
        
            if(elementalDamage < 0)
            {
                elementalDamage = 0;
            }

            #endregion

            #region Total Damage

            int totalDamage = (int)((int)((normalDamage + elementalDamage + attacker.Morale + staticBoostCategory1) * boostCategory1) * shellBoostCategory1);

            if ((attacker.EntityType == EntityType.Player || attacker.EntityType == EntityType.Mate) && (defender.EntityType == EntityType.Player || defender.EntityType == EntityType.Mate))
            {
                totalDamage = totalDamage / 2;
            }

            if (defender.EntityType == EntityType.Monster || defender.EntityType == EntityType.NPC)
            {
                totalDamage -= GetMonsterDamageBonus(defender.Level);
            }

            if (totalDamage < 5)
            {
                totalDamage = ServerManager.Instance.RandomNumber(1, 6);
            }

            if(attacker.EntityType == EntityType.Monster || attacker.EntityType == EntityType.NPC)
            {
                totalDamage += GetMonsterDamageBonus(attacker.Level);
            }

            #endregion

            return totalDamage;
        }

        private int GetMonsterDamageBonus(byte Level)
        {
            if (Level < 45)
            {
                return 0;
            }
            else if (Level < 55)
            {
                return Level;
            }
            else if (Level < 60)
            {
                return Level * 2;
            }
            else if (Level < 65)
            {
                return Level * 3;
            }
            else if (Level < 70)
            {
                return Level * 4;
            }
            else
            {
                return Level * 5;
            }
        }

        private int[] GetBuff(byte Level, List<Buff> buffs, List<BCard> bcards, CardType type, byte subtype)
        {
            int value1 = 0;
            int value2 = 0;

            foreach (BCard entry in bcards.Where(s => s.Type.Equals((byte)type) && s.SubType.Equals((byte)(subtype / 10))))
            {
                if (entry.IsLevelScaled)
                {
                    value1 += entry.FirstData * Level;
                }
                else
                {
                    value1 += entry.FirstData;
                }
                value2 += entry.SecondData;
            }

            if (buffs != null)
            {
                foreach (Buff buff in buffs)
                {
                    // THIS ONE DOES NOT FOR STUFFS
                    foreach (BCard entry in buff.Card.BCards
                        .Where(s => s.Type.Equals((byte)type) && s.SubType.Equals((byte)(subtype / 10)) && (!s.IsDelayed || (s.IsDelayed && buff.Start.AddMilliseconds(buff.Card.Delay * 100) < DateTime.Now))))
                    {
                        if (entry.IsLevelScaled)
                        {
                            value1 += entry.FirstData * buff.Level;
                        }
                        else
                        {
                            value1 += entry.FirstData;
                        }
                        value2 += entry.SecondData;
                    }
                }
            }

            return new[] { value1, value2 };
        }
    }
}
