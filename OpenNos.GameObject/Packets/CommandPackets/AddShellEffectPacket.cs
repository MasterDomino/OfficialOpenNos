﻿using OpenNos.Core;
using OpenNos.Domain;

namespace OpenNos.GameObject.CommandPackets
{
    [PacketHeader("$AddShellEffect", PassNonParseablePacket = true, Authority = AuthorityType.GameMaster)]
    public class AddShellEffectPacket:PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public byte Slot { get; set; }

        [PacketIndex(1)]
        public byte EffectLevel { get; set; }

        [PacketIndex(2)]
        public byte Effect { get; set; }

        [PacketIndex(3)]
        public short Value { get; set; }

        #endregion

        #region Methods

        public static string ReturnHelp() => "$AddShellEffect Slot EffectLevel Effect Value";

        #endregion
    }
}
