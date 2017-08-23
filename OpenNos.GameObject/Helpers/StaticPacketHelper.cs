using OpenNos.Domain;
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

        public static string Say(byte type, long callerId, byte secondaryType, string message)
        {
            return $"say {type} {callerId} {secondaryType} {message}";
        }

        public static string In(UserType type)
        {
            //in 1 {(Authority == AuthorityType.Moderator && !Undercover ? "[Support]" + _name : Authority == AuthorityType.BitchNiggerFaggot ? _name + "[BitchNiggerFaggot]" : _name)} - {CharacterId} {PositionX} {PositionY} {Direction} {(Undercover ? (byte)AuthorityType.User : Authority < AuthorityType.User ? (byte)AuthorityType.User : (byte)Authority)} {(byte)Gender} {(byte)HairStyle} {color} {(byte)Class} {GenerateEqListForPacket()} {Math.Ceiling(Hp / HPLoad() * 100)} {Math.Ceiling(Mp / MPLoad() * 100)} {(IsSitting ? 1 : 0)} {(Group?.GroupType == GroupType.Group ? (Group?.GroupId ?? -1) : -1)} {(fairy != null && !Undercover ? 4 : 0)} {fairy?.Item.Element ?? 0} 0 {fairy?.Item.Morph ?? 0} 0 {(UseSp || IsVehicled ? Morph : 0)} {GenerateEqRareUpgradeForPacket()} {(!Undercover ? (foe ? -1 : Family?.FamilyId ?? -1) : -1)} {(!Undercover ? (foe ? _name : Family?.Name ?? "-") : "-")} {(GetDignityIco() == 1 ? GetReputationIco() : -GetDignityIco())} {(Invisible ? 1 : 0)} {(UseSp ? MorphUpgrade : 0)} {_faction} {(UseSp ? MorphUpgrade2 : 0)} {Level} {Family?.FamilyLevel ?? 0} {ArenaWinner} {(Authority == AuthorityType.Moderator && !Undercover ? 500 : Compliment)} {Size} {HeroLevel}
            //in 2 {NpcVNum} {MapNpcId} {MapX} {MapY} {Position} 100 100 {Dialog} 0 0 -1 {respawnType} {(IsSitting ? 1 : 0)} -1 - 0 -1 0 0 0 0 0 0 0 0
            //in 3 {MonsterVNum} {MapMonsterId} {MapX} {MapY} {Position} {(int)((float)CurrentHp / (float)MaxHp * 100)} {(int)((float)CurrentMp / (float)MaxMp * 100)} 0 0 0 -1 {(NoAggresiveIcon ? (byte)InRespawnType.NoEffect : (byte)InRespawnType.TeleportationEffect)} 0 -1 - 0 -1 0 0 0 0 0 0 0 0
            //in 9 {ItemVNum} {TransportId} {PositionX} {PositionY} {(this is MonsterMapItem && ((MonsterMapItem)this).GoldAmount > 1 ? ((MonsterMapItem)this).GoldAmount : Amount)} 0 0 -1
            switch (type)
            {
                case UserType.Player:
                    return string.Empty;
                case UserType.Npc:
                    return string.Empty;
                case UserType.Monster:
                    return string.Empty;
                case UserType.Object:
                    return string.Empty;
            }
            return string.Empty;
        }

        public static string CastOnTarget(UserType type, long callerId, byte secondaryType, long targetId, short castAnimation, short castEffect, short skillVNum)
        {
            return $"ct {(byte)type} {callerId} {secondaryType} {targetId} {castAnimation} {castEffect} {skillVNum}";
        }

        public static EffectPacket GenerateEff(UserType effectType, long callerId, int effectId)
        {
            return new EffectPacket
            {
                EffectType = effectType,
                CallerId = callerId,
                EffectId = effectId
            };
        }

        public static MovePacket Move(UserType type, long callerId, short positionX, short positionY, byte speed)
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

        public static string Out(UserType type, long callerId)
        {
            return $"out {(byte)type} {callerId}";
        }

        public static string SkillReset(int castId)
        {
            return $"sr {castId}";
        }

        public static string SkillUsed(UserType type, long callerId, byte secondaryType, long targetId, short skillVNum, short cooldown, short attackAnimation, short skillEffect, short x, short y, bool isAlive, int health, int damage, int hitmode, byte skillType)
        {
            return $"su {(byte)type} {callerId} {secondaryType} {targetId} {skillVNum} {cooldown} {attackAnimation} {skillEffect} {x} {y} {(isAlive ? 1 : 0)} {health} {damage} {hitmode} {skillType}";
        }

        #endregion
    }
}