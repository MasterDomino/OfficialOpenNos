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

namespace OpenNos.Domain
{
    public enum ShellArmorEffectType : byte
    {
        CloseDefence = 1,
        DistanceDefence = 2,
        MagicDefence = 3,
        PercentageTotalDefence = 4,
        ReducedMinorBleeding = 5,
        ReducedBleedingAndMinorBleeding = 6,
        ReducedAllBleedingType = 7,
        ReducedStun = 8,
        ReducedAllStun = 9,
        ReducedParalysis = 10,
        ReducedFreeze = 11,
        ReducedBlind = 12,
        ReducedSlow = 13,
        ReducedArmorDeBuff = 14,
        ReducedShock = 15,
        ReducedPoisonParalysis = 16,
        ReducedAllNegativeEffect = 17,
        RecovryHPOnRest = 18,
        RevoryHP = 19,
        RecoveryMPOnRest = 20,
        RecoveryMP = 21,
        RecoveryHPInDefence = 22,
        ReducedCritChanceRecive = 23,
        IncreasedFireResistence = 24,
        IncreasedWaterResistence = 25,
        IncreasedLightResistence = 25,
        IncreasedDarkResistence = 26,
        IncreasedAllResistence = 27,
        ReducedPrideLoss = 28,
        ReducedProductionPointConsumed = 29,
        IncreasedProductionPossibility = 30,
        IncreasedRecoveryItemSpeed = 31,
        PercentageAllPVPDefence = 32,
        CloseDefenceDodgeInPVP = 33,
        DistanceDefenceDodgeInPVP = 34,
        IgnoreMagicDamage = 35,
        DodgeAllDamage = 36,
        ProtectMPInPVP = 37,
        FireDamageImmuneInPVP = 38,
        WaterDamageImmuneInPVP = 39,
        LightDamageImmuneInPVP = 40,
        DarkDamageImmuneInPVP = 41,
        ReturnAbility = 42, //wut?
        AbsorbDistanceDamage = 43,
        AbsorbMagicDamage = 44,
        IncreasedEvasion = 45
    }
}
