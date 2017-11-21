﻿using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class DropMapper
    {
        public DropMapper()
        {

        }

        public void ToDropDTO(Drop input, DropDTO output)
        {
            output.Amount = input.Amount;
            output.DropChance = input.DropChance;
            output.DropId = input.DropId;
            output.ItemVNum = input.ItemVNum;
            output.MapTypeId = input.MapTypeId;
            output.MonsterVNum = input.MonsterVNum;
        }

        public void ToDrop(DropDTO input, Drop output)
        {
            output.Amount = input.Amount;
            output.DropChance = input.DropChance;
            output.DropId = input.DropId;
            output.ItemVNum = input.ItemVNum;
            output.MapTypeId = input.MapTypeId;
            output.MonsterVNum = input.MonsterVNum;
        }
    }
}