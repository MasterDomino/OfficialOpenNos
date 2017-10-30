using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.XMLModel.Events.Quest
{
    [Serializable]
    public class Script
    {
        public int Type { get; set; }

        public int Value { get; set; }
    }
}
