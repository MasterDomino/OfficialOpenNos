using OpenNos.Domain;

namespace OpenNos.Data
{
    public class ShellEffectGenerationDTO : MappingBaseDTO
    {
        #region Properties

        public byte Effect { get; set; }

        public ShellEffectLevelType EffectLevel { get; set; }

        public byte MaximumValue { get; set; }

        public byte MinimumValue { get; set; }

        public byte Rare { get; set; }

        public long ShellEffectGenerationId { get; set; }

        #endregion
    }
}