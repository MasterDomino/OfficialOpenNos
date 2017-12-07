using OpenNos.DAL.EF;
using OpenNos.Data;

namespace OpenNos.Mapper.Mappers
{
    public class TeleporterMapper
    {
        #region Methods

        public bool ToTeleporter(TeleporterDTO input, Teleporter output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.Index = input.Index;
            output.MapId = input.MapId;
            output.MapNpcId = input.MapNpcId;
            output.MapX = input.MapX;
            output.MapY = input.MapY;
            output.TeleporterId = input.TeleporterId;
            return true;
        }

        public bool ToTeleporterDTO(Teleporter input, TeleporterDTO output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.Index = input.Index;
            output.MapId = input.MapId;
            output.MapNpcId = input.MapNpcId;
            output.MapX = input.MapX;
            output.MapY = input.MapY;
            output.TeleporterId = input.TeleporterId;
            return true;
        }

        #endregion
    }
}