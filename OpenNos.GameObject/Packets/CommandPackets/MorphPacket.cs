////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;
using OpenNos.Domain;

namespace OpenNos.GameObject.CommandPackets
{
    [PacketHeader("$Morph", PassNonParseablePacket = true, Authority = AuthorityType.GameMaster)]
    public class MorphPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public short MorphId { get; set; }

        [PacketIndex(1)]
        public byte Upgrade { get; set; }

        [PacketIndex(2)]
        public byte MorphDesign { get; set; }

        [PacketIndex(3)]
        public int ArenaWinner { get; set; }

        public static string ReturnHelp()
        {
            return "$Morph MORPHID UPGRADE WINGS ARENA";
        }

        #endregion
    }
}