using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.Domain
{
    public enum CellonOptionType : byte
    {
        HPMax=0,
        MPMax=1,
        HPRestore = 2,
        MPRestore = 3,
        MPUsage = 4,
        CritReduce = 5
    }
}
