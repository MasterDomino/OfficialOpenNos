﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;
using OpenNos.Domain;

namespace OpenNos.GameObject.CommandPackets
{
    [PacketHeader("$RestartAll", PassNonParseablePacket = true, Authority = AuthorityType.GameMaster)]
    public class RestartAllPacket : PacketDefinition
    {

        [PacketIndex(0)]
        public string WorldGroup { get; set; }

        public static string ReturnHelp()
        {
            return "$RestartAll WORLDGROUP(*)";
        }

    }
}