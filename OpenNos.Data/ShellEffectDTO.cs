using OpenNos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.Data
{
    public class ShellEffectDTO : MappingBaseDTO
    {
        public long ShellEffectId { get; set; }

        public ShellEffectLevelType EffectLevel { get; set; }

        public byte Effect { get; set; }

        public short Value { get; set; }

        public Guid ItemInstanceId { get; set; }
    }
}
