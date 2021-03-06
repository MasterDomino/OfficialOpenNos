////<auto-generated <- Codemaid exclusion for now (PacketIndex Order is important for maintenance)

using OpenNos.Core;
using OpenNos.Domain;

namespace OpenNos.GameObject
{
    [PacketHeader("eff")]
    public class EffectPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public UserType EffectType { get; set; }

        [PacketIndex(1)]
        public long CallerId { get; set; }

        [PacketIndex(2)]
        public int EffectId { get; set; }

        #endregion
    }
}
