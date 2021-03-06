////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;
using OpenNos.Domain;

namespace OpenNos.GameObject.CommandPackets
{
    [PacketHeader("$AddPortal", PassNonParseablePacket = true, Authority = AuthorityType.GameMaster)]
    public class AddPortalPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public short DestinationMapId { get; set; }

        [PacketIndex(1)]
        public short DestinationX { get; set; }

        [PacketIndex(2)]
        public short DestinationY { get; set; }

        [PacketIndex(3)]
        public PortalType? PortalType { get; set; }

        public static string ReturnHelp()
        {
            return "$AddPortal MAPID DESTX DESTY PORTALTYPE(?)";
        }

        #endregion
    }
}