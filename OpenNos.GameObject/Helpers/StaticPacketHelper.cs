using OpenNos.GameObject.Packets.ServerPackets;

namespace OpenNos.GameObject.Helpers
{
    public class StaticPacketHelper
    {
        #region Members

        private static StaticPacketHelper instance;

        #endregion

        #region Properties

        public static StaticPacketHelper Instance
        {
            get
            {
                return instance ?? (instance = new StaticPacketHelper());
            }
        }

        #endregion

        #region Methods

        public static string Out(byte type, long callerId)
        {
            return $"out {type} {callerId}";
        }

        public static string Cancel(byte type = 0, long callerId = 0)
        {
            return $"cancel {type} {callerId}";
        }

        public static string CastOnTarget(byte type, long callerId, byte secondaryType, long targetId, short castAnimation, short castEffect, short skillVNum)
        {
            return $"ct {type} {callerId} {secondaryType} {targetId} {castAnimation} {castEffect} {skillVNum}";
        }

        public static string SkillUsed(byte type, long callerId, byte secondaryType, long targetId, short skillVNum, short cooldown, short attackAnimation, short skillEffect, short x, short y, bool isAlive, int health, int damage, int hitmode, byte skillType)
        {
            return $"su {type} {callerId} {secondaryType} {targetId} {skillVNum} {cooldown} {attackAnimation} {skillEffect} {x} {y} {(isAlive ? 1 : 0)} {health} {damage} {hitmode} {skillType}";
        }

        public static MovePacket Move(byte type, long callerId, short x, short y, byte speed)
        {
            return new MovePacket
            {
                CallerId = callerId,
                MapX = x,
                MapY = y,
                Speed = speed,
                MoveType = type
            };
        }

        #endregion
    }
}