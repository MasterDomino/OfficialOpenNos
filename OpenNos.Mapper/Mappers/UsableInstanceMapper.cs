using OpenNos.DAL.EF;
using OpenNos.Data;
using System;

namespace OpenNos.Mapper.Mappers
{
    public class UsableInstanceMapper
    {
        public UsableInstanceMapper()
        {

        }

        public void ToUsableInstanceDTO(UsableInstance input, UsableInstanceDTO output)
        {
            output.Amount = (byte)input.Amount;
            output.BoundCharacterId = input.BoundCharacterId;
            output.CharacterId = input.CharacterId;
            output.Design = input.Design;
            output.DurabilityPoint = input.DurabilityPoint;
            output.HP = input.HP ?? 0;
            output.Id = input.Id;
            output.ItemDeleteTime = input.ItemDeleteTime;
            output.ItemVNum = input.ItemVNum;
            output.MP = input.MP ?? 0;
            output.Rare = (sbyte)input.Rare;
            output.Slot = input.Slot;
            output.Type = input.Type;
            output.Upgrade = input.Upgrade;
        }

        public void ToUsableInstance(UsableInstanceDTO input, UsableInstance output)
        {
            output.Amount = input.Amount;
            output.BoundCharacterId = input.BoundCharacterId;
            output.CharacterId = input.CharacterId;
            output.Design = input.Design;
            output.DurabilityPoint = input.DurabilityPoint;
            output.HP = input.HP;
            output.Id = input.Id;
            output.ItemDeleteTime = input.ItemDeleteTime;
            output.ItemVNum = input.ItemVNum;
            output.MP = input.MP;
            output.Rare = input.Rare;
            output.Slot = input.Slot;
            output.Type = input.Type;
            output.Upgrade = input.Upgrade;
        }
    }
}
