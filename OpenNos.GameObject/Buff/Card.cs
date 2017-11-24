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

using OpenNos.Data;
using System.Collections.Generic;

namespace OpenNos.GameObject
{
    public class Card : CardDTO
    {
        public Card()
        {
            
        }

        public Card(CardDTO input)
        {
            this.BuffType = input.BuffType;
            this.CardId = input.CardId;
            this.Delay = input.Delay;
            this.Duration = input.Duration;
            this.EffectId = input.EffectId;
            this.Level = input.Level;
            this.Name = input.Name;
            this.Propability = input.Propability;
            this.TimeoutBuff = input.TimeoutBuff;
            this.TimeoutBuffChance = input.TimeoutBuffChance;
        }
                    
        #region Properties

        public List<BCard> BCards { get; set; }

        #endregion
    }
}