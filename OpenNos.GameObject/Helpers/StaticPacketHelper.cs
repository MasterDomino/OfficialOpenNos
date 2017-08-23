using OpenNos.GameObject.Packets.ServerPackets;

namespace OpenNos.GameObject.Helpers
{
    public static class StaticPacketHelper
    {
        #region Methods

        public static string Cancel(byte type = 0, long callerId = 0)
        {
            return $"cancel {type} {callerId}";
        }

        public static string CastOnTarget(byte type, long callerId, byte secondaryType, long targetId, short castAnimation, short castEffect, short skillVNum)
        {
            return $"ct {type} {callerId} {secondaryType} {targetId} {castAnimation} {castEffect} {skillVNum}";
        }

        public static EffectPacket GenerateEff(byte effectType, long callerId, int effectId)
        {
            return new EffectPacket
            {
                EffectType = effectType,
                CallerId = callerId,
                EffectId = effectId
            };
        }

        public static MovePacket Move(byte type, long callerId, short positionX, short positionY, byte speed)
        {
            return new MovePacket
            {
                CallerId = callerId,
                MapX = positionX,
                MapY = positionY,
                Speed = speed,
                MoveType = type
            };
        }

        public static string Out(byte type, long callerId)
        {
            return $"out {type} {callerId}";
        }

        public static string SkillReset(int castId)
        {
            return $"sr {castId}";
        }

        public static string SkillUsed(byte type, long callerId, byte secondaryType, long targetId, short skillVNum, short cooldown, short attackAnimation, short skillEffect, short x, short y, bool isAlive, int health, int damage, int hitmode, byte skillType)
        {
            return $"su {type} {callerId} {secondaryType} {targetId} {skillVNum} {cooldown} {attackAnimation} {skillEffect} {x} {y} {(isAlive ? 1 : 0)} {health} {damage} {hitmode} {skillType}";
        }

        #endregion
    }
}