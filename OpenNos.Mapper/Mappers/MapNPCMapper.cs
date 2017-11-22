using OpenNos.DAL.EF;
using OpenNos.Data;
using System;

namespace OpenNos.Mapper.Mappers
{
    public class MapNPCMapper
    {
        public MapNPCMapper()
        {

        }

        public void ToMapNPCDTO(MapNpc input, MapNpcDTO output)
        {
            output.Dialog = input.Dialog;
            output.Effect = input.Effect;
            output.EffectDelay = input.EffectDelay;
            output.IsDisabled = input.IsDisabled;
            output.IsMoving = input.IsMoving;
            output.IsSitting = input.IsSitting;
            output.MapId = input.MapId;
            output.MapNpcId = input.MapNpcId;
            output.MapX = input.MapX;
            output.MapY = input.MapY;
            output.NpcVNum = input.NpcVNum;
            output.Position = input.Position;
        }

        public void ToMapNPC(MapNpcDTO input, MapNpc output)
        {
            output.Dialog = input.Dialog;
            output.Effect = input.Effect;
            output.EffectDelay = input.EffectDelay;
            output.IsDisabled = input.IsDisabled;
            output.IsMoving = input.IsMoving;
            output.IsSitting = input.IsSitting;
            output.MapId = input.MapId;
            output.MapNpcId = input.MapNpcId;
            output.MapX = input.MapX;
            output.MapY = input.MapY;
            output.NpcVNum = input.NpcVNum;
            output.Position = input.Position;
        }
    }
}
