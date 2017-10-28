using OpenNos.Core;
using OpenNos.Domain;

namespace OpenNos.GameObject
{
    [PacketHeader("$DirectConnect", PassNonParseablePacket = true, Authority = AuthorityType.GameMaster)]
    public class DirectConnectPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public string IPAddress { get; set; }

        [PacketIndex(1)]
        public int Port { get; set; }

        public override string ToString()
        {
            return $"DirectConnect Command IPAddress: {IPAddress} Port: {Port}";
        }

        #endregion
    }
}
