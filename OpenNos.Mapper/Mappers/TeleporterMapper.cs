using OpenNos.DAL.EF;
using OpenNos.Data;
using System;

namespace OpenNos.Mapper.Mappers
{
    public class TeleporterMapper
    {
        public TeleporterMapper()
        {

        }

        public void ToTeleporterDTO(Teleporter input, TeleporterDTO output)
        {
            output.Index = input.Index;
            output.MapId = input.MapId;
            output.MapNpcId = input.MapNpcId;
            output.MapX = input.MapX;
            output.MapY = input.MapY;
            output.TeleporterId = input.TeleporterId;
        }

        public void ToTeleporter(TeleporterDTO input, Teleporter output)
        {
            output.Index = input.Index;
            output.MapId = input.MapId;
            output.MapNpcId = input.MapNpcId;
            output.MapX = input.MapX;
            output.MapY = input.MapY;
            output.TeleporterId = input.TeleporterId;
        }
    }
}
