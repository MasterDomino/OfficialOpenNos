﻿using System;

namespace OpenNos.Master.Library.Data
{
    [Serializable]
    public class ConfigurationObject
    {
        public int SessionLimit { get; set; }

        public bool SceneOnCreate { get; set; }

        public bool WorldInformation { get; set; }

        public int RateXP { get; set; }

        public int RateHeroicXP { get; set; }

        public int RateGold { get; set; }

        public int RateGoldDrop { get; set; }

        public long MaxGold { get; set; }

        public int RateDrop { get; set; }

        public byte MaxLevel { get; set; }

        public byte MaxJobLevel { get; set; }

        public byte MaxHeroLevel { get; set; }

        public byte HeroicStartLevel { get; set; }

        public byte MaxSPLevel { get; set; }

        public int RateFairyXP { get; set; }

        public byte MaxUpgrade { get; set; }
    }
}
