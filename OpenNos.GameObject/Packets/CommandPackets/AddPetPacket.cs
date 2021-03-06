////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;
using OpenNos.Domain;

namespace OpenNos.GameObject.CommandPackets
{
    [PacketHeader("$AddPet", PassNonParseablePacket = true, Authority = AuthorityType.GameMaster)]
    public class AddPetPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public short MonsterVNum { get; set; }

        [PacketIndex(1)]
        public byte Level { get; set; }

        #endregion

        #region Methods

        public static string ReturnHelp()
        {
            return "$AddPet MONSTERVNUM LEVEL";
        }

        #endregion
    }
}