using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.GameObject.Event
{
    public class TalentArenaBattle
    {
        public TalentArenaBattle()
        {

        }

        public byte GroupLevel { get; set; }

        public byte Side { get; set; }

        public MapInstance MapInstance { get; set; }

        public byte Calls { get; set; }

        public List<long> CharacterOrder { get; set; }

        public List<long> KilledCharacters { get; set; }
    }
}
