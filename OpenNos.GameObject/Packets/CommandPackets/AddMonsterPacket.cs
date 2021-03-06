////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;
using OpenNos.Domain;

namespace OpenNos.GameObject.CommandPackets
{
    [PacketHeader("$AddMonster", PassNonParseablePacket = true, Authority = AuthorityType.GameMaster)]
    public class AddMonsterPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public short MonsterVNum { get; set; }

        [PacketIndex(1)]
        public bool IsMoving { get; set; }

        public static string ReturnHelp()
        {
            return "$AddMonster VNUM MOVE";
        }

        #endregion
    }
}