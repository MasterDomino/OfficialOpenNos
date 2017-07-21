using OpenNos.Core;
using OpenNos.Domain;

namespace OpenNos.GameObject.CommandPackets
{
    [PacketHeader("$Unstuck", PassNonParseablePacket = true, Authority = AuthorityType.User)]
    public class UnstuckPacket : PacketDefinition
    {
        public static string ReturnHelp()
        {
            return "$Unstuck";
        }
    }
}