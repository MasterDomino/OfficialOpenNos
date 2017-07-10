using OpenNos.Core;
using OpenNos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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