using OpenNos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.Data
{
    public class ShellEffectGenerationDTO : MappingBaseDTO
    {

        public long ShellEffectGenerationId { get; set; }

        public byte Rare { get; set; }

        public ShellEffectLevelType EffectLevel { get; set; }

        public byte Effect { get; set; }

        public byte MinimumValue { get; set; }

        public byte MaximumValue { get; set; }
    }
}
