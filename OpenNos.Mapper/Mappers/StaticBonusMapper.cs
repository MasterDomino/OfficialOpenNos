﻿using OpenNos.DAL.EF;
using OpenNos.Data;
using System;

namespace OpenNos.Mapper.Mappers
{
    public class StaticBonusMapper
    {
        public StaticBonusMapper()
        {

        }

        public void ToStaticBonusDTO(StaticBonus input, StaticBonusDTO output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.CharacterId = input.CharacterId;
            output.DateEnd = input.DateEnd;
            output.StaticBonusId = input.StaticBonusId;
            output.StaticBonusType = input.StaticBonusType;
        }

        public void ToStaticBonus(StaticBonusDTO input, StaticBonus output)
        {
            if (input == null)
            {
                output = null;
                return;
            }
            output.CharacterId = input.CharacterId;
            output.DateEnd = input.DateEnd;
            output.StaticBonusId = input.StaticBonusId;
            output.StaticBonusType = input.StaticBonusType;
        }
    }
}
