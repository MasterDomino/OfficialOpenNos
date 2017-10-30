﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.Domain
{
    public enum QuestGiverType : byte
    {
        InitialQuest = 0,
        NPC = 1,
        NPCDialog = 2,
        ItemLoot = 3,
        ItemUse = 4,
    }
}
