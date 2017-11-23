using OpenNos.DAL.EF;
using OpenNos.Data;
using System;

namespace OpenNos.Mapper.Mappers
{
    public class ItemInstanceMapper
    {
        public ItemInstanceMapper()
        {

        }

        public bool ToItemInstanceDTO(ItemInstance input, ItemInstanceDTO output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.Amount = (byte)input.Amount;
            output.BoundCharacterId = input.BoundCharacterId;
            output.CharacterId = input.CharacterId;
            output.Design = input.Design;
            output.DurabilityPoint = input.DurabilityPoint;
            output.Id = input.Id;
            output.ItemDeleteTime = input.ItemDeleteTime;
            output.ItemVNum = input.ItemVNum;
            output.Rare = (sbyte)input.Rare;
            output.Slot = input.Slot;
            output.Type = input.Type;
            output.Upgrade = input.Upgrade;
            return true;
        }

        public bool ToItemInstance(ItemInstanceDTO input, ItemInstance output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.Amount = input.Amount;
            output.BoundCharacterId = input.BoundCharacterId;
            output.CharacterId = input.CharacterId;
            output.Design = input.Design;
            output.DurabilityPoint = input.DurabilityPoint;
            output.Id = input.Id;
            output.ItemDeleteTime = input.ItemDeleteTime;
            output.ItemVNum = input.ItemVNum;
            output.Rare = input.Rare;
            output.Slot = input.Slot;
            output.Type = input.Type;
            output.Upgrade = input.Upgrade;
            return true;
        }
    }
}
