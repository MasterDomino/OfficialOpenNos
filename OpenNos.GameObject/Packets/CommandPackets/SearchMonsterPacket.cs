﻿////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)
using OpenNos.Core;
using OpenNos.Domain;

namespace OpenNos.GameObject
{
    [PacketHeader("$SearchMonster", PassNonParseablePacket = true, Authority = AuthorityType.GameMaster)]
    public class SearchMonsterPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0, SerializeToEnd = true)]
        public string Name { get; set; }

        #endregion
    }
}