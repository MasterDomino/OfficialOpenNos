using OpenNos.Domain;
using System;

namespace OpenNos.Data
{
    public class ShellEffectDTO : MappingBaseDTO
    {
        public long ShellEffectId { get; set; }

        public ShellEffectLevelType EffectLevel { get; set; }

        public byte Effect { get; set; }

        public short Value { get; set; }

        public Guid EquipmentSerialId { get; set; }
    }
}
