using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.Master.Library.Data
{
    [Serializable]
    public class MallItem
    {
        public byte Amount { get; set; }

        public short ItemVNum { get; set; }

        public byte Rare { get; set; }

        public byte Upgrade { get; set; }
    }
}
