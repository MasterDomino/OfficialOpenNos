/*
 * This file is part of the OpenNos Emulator Project. See AUTHORS file for Copyright information
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 */

using OpenNos.Core;
using OpenNos.Data;
using OpenNos.Domain;
using OpenNos.GameObject;
using OpenNos.GameObject.Battle;
using OpenNos.GameObject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace OpenNos.Handler
{
    public class BattlePacketHandler : IPacketHandler
    {
        #region Instantiation

        public BattlePacketHandler(ClientSession session)
        {
            Session = session;
        }

        #endregion

        #region Properties

        private ClientSession Session { get; }

        #endregion

        #region Methods

        /// <summary>
        /// mtlist packet
        /// </summary>
        /// <param name="mutliTargetListPacket"></param>
        public void MultiTargetListHit(MultiTargetListPacket mutliTargetListPacket)
        {
            if (!Session.HasCurrentMapInstance)
            {
                return;
            }
            bool isMuted = Session.Character.MuteMessage();
            if (isMuted || Session.Character.IsVehicled)
            {
                Session.SendPacket(StaticPacketHelper.Cancel());
                return;
            }
            if ((DateTime.Now - Session.Character.LastTransform).TotalSeconds < 3)
            {
                Session.SendPacket(StaticPacketHelper.Cancel());
                Session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("CANT_ATTACKNOW"), 0));
                return;
            }
            if (mutliTargetListPacket.TargetsAmount > 0 && mutliTargetListPacket.TargetsAmount == mutliTargetListPacket.Targets.Count && mutliTargetListPacket.Targets != null)
            {
                Session.Character.MTListTargetQueue.Clear();
                foreach (MultiTargetListSubPacket subpacket in mutliTargetListPacket.Targets)
                {
                    Session.Character.MTListTargetQueue.Add(new MTListHitTarget(subpacket.TargetType, subpacket.TargetId));
                }
            }
        }

        /// <summary>
        /// u_s packet
        /// </summary>
        /// <param name="useSkillPacket"></param>
        public void UseSkill(UseSkillPacket useSkillPacket)
        {
            if (Session.Character.NoAttack)
            {
                return;
            }
            if (Session.Character.CanFight && useSkillPacket != null)
            {
                Session.Character.RemoveBuff(614);
                Session.Character.RemoveBuff(615);
                Session.Character.RemoveBuff(616);
                bool isMuted = Session.Character.MuteMessage();
                if (isMuted || Session.Character.IsVehicled || Session.Character.InvisibleGm)
                {
                    Session.SendPacket(StaticPacketHelper.Cancel());
                    return;
                }
                if (useSkillPacket.MapX.HasValue && useSkillPacket.MapY.HasValue)
                {
                    Session.Character.PositionX = useSkillPacket.MapX.Value;
                    Session.Character.PositionY = useSkillPacket.MapY.Value;
                }
                if (Session.Character.IsSitting)
                {
                    Session.Character.Rest();
                }
                switch (useSkillPacket.UserType)
                {
                    case UserType.Monster:
                        if (Session.Character.Hp > 0)
                        {
                            TargetHit(useSkillPacket.CastId, useSkillPacket.MapMonsterId);
                            int[] fairyWings = Session.Character.GetBuff(BCardType.CardType.EffectSummon, 11);
                            int random = ServerManager.Instance.RandomNumber();
                            if (fairyWings[0] > random)
                            {
                                Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(o =>
                                {
                                    if (Session?.Character != null)
                                    {
                                        CharacterSkill ski = (Session.Character.UseSp ? Session.Character.SkillsSp?.GetAllItems() : Session.Character.Skills?.GetAllItems()).Find(s => s.Skill?.CastId == useSkillPacket.CastId && s.Skill?.UpgradeSkill == 0);
                                        if (ski != null)
                                        {
                                            ski.LastUse = DateTime.Now.AddMilliseconds(ski.Skill.Cooldown * 100 * -1);
                                            Session.SendPacket(StaticPacketHelper.SkillReset(useSkillPacket.CastId));
                                        }
                                    }
                                });
                            }
                        }
                        break;

                    case UserType.Player:
                        if (Session.Character.Hp > 0)
                        {
                            if (useSkillPacket.MapMonsterId != Session.Character.CharacterId)
                            {
                                TargetHit(useSkillPacket.CastId, useSkillPacket.MapMonsterId, true);
                            }
                            else
                            {
                                TargetHit(useSkillPacket.CastId, useSkillPacket.MapMonsterId);
                            }
                            int[] fairyWings = Session.Character.GetBuff(BCardType.CardType.EffectSummon, 11);
                            int random = ServerManager.Instance.RandomNumber();
                            if (fairyWings[0] > random)
                            {
                                Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(o =>
                                {
                                    if (Session?.Character != null)
                                    {
                                        CharacterSkill ski = (Session.Character.UseSp ? Session.Character.SkillsSp?.GetAllItems() : Session.Character.Skills?.GetAllItems()).Find(s => s.Skill?.CastId == useSkillPacket.CastId && s.Skill?.UpgradeSkill == 0);
                                        if (ski != null)
                                        {
                                            ski.LastUse = DateTime.Now.AddMilliseconds(ski.Skill.Cooldown * 100 * -1);
                                            Session.SendPacket(StaticPacketHelper.SkillReset(useSkillPacket.CastId));
                                        }
                                    }
                                });
                            }
                        }
                        else
                        {
                            Session.SendPacket(StaticPacketHelper.Cancel(2));
                        }
                        break;

                    default:
                        Session.SendPacket(StaticPacketHelper.Cancel(2));
                        return;
                }
            }
            else
            {
                Session.SendPacket(StaticPacketHelper.Cancel(2));
            }
        }

        /// <summary>
        /// u_as packet
        /// </summary>
        /// <param name="useAOESkillPacket"></param>
        public void UseZonesSkill(UseAOESkillPacket useAOESkillPacket)
        {
            bool isMuted = Session.Character.MuteMessage();
            if (isMuted || Session.Character.IsVehicled)
            {
                Session.SendPacket(StaticPacketHelper.Cancel());
            }
            else
            {
                if (Session.Character.LastTransform.AddSeconds(3) > DateTime.Now)
                {
                    Session.SendPacket(StaticPacketHelper.Cancel());
                    Session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("CANT_ATTACK"), 0));
                    return;
                }
                if (Session.Character.CanFight && Session.Character.Hp > 0)
                {
                    ZoneHit(useAOESkillPacket.CastId, useAOESkillPacket.MapX, useAOESkillPacket.MapY);
                }
            }
        }

        private void PVPHit(HitRequest hitRequest, ClientSession target)
        {
            if (target.Character.Hp > 0 && hitRequest.Session.Character.Hp > 0)
            {
                if (target.Character.IsSitting)
                {
                    target.Character.Rest();
                }
                int hitmode = 0;
                bool onyxWings = false;
                int damage = DamageHelper.Instance.CalculateDamage(new BattleEntity(hitRequest.Session.Character, hitRequest.Skill), new BattleEntity(target.Character, null), hitRequest.Skill, ref hitmode, ref onyxWings);
                if (target.Character.HasGodMode)
                {
                    damage = 0;
                    hitmode = 1;
                }
                else if (target.Character.LastPVPRevive > DateTime.Now.AddSeconds(-10) || hitRequest.Session.Character.LastPVPRevive > DateTime.Now.AddSeconds(-10))
                {
                    damage = 0;
                    hitmode = 1;
                }
                target.Character.GetDamage(damage / 2);
                target.Character.LastDefence = DateTime.Now;
                target.SendPacket(target.Character.GenerateStat());
                bool IsAlive = target.Character.Hp > 0;
                if (!IsAlive)
                {
                    if (target?.CurrentMapInstance?.Map?.MapTypes.Any(s => s.MapTypeId == (short)MapTypeEnum.Act4) == true)
                    {
                        hitRequest.Session.Character.Act4Kill++;
                        target.Character.Act4Dead++;
                        target.Character.GetAct4Points(-1);
                        if (target.Character.Level + 10 >= hitRequest.Session.Character.Level && hitRequest.Session.Character.Level <= target.Character.Level - 10)
                        {
                            hitRequest.Session.Character.GetAct4Points(2);
                        }
                        if (target.Character.Reputation < 50000)
                        {
                            target.SendPacket(Session.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("LOSE_REP"), 0), 11));
                        }
                        else
                        {
                            target.Character.Reputation -= target.Character.Level * 50;
                            hitRequest.Session.Character.Reputation += target.Character.Level * 50;
                            hitRequest.Session.SendPacket(hitRequest.Session.Character.GenerateLev());
                            target.SendPacket(target.Character.GenerateSay(string.Format(Language.Instance.GetMessageFromKey("LOSE_REP"), (short)(target.Character.Level * 50)), 11));
                        }
                        target.SendPacket(target.Character.GenerateFd());
                        List<BuffType> bufftodisable = new List<BuffType>
                        {
                            BuffType.Bad,
                            BuffType.Good,
                            BuffType.Neutral
                        };
                        target.Character.DisableBuffs(bufftodisable);
                        target.CurrentMapInstance?.Broadcast(target, target.Character.GenerateIn(), ReceiverType.AllExceptMe);
                        target.CurrentMapInstance?.Broadcast(target, target.Character.GenerateGidx(), ReceiverType.AllExceptMe);
                        target.SendPacket(target.Character.GenerateSay(Language.Instance.GetMessageFromKey("ACT4_PVP_DIE"), 11));
                        target.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("ACT4_PVP_DIE"), 0));
                        Observable.Timer(TimeSpan.FromMilliseconds(30000)).Subscribe(o =>
                        {
                            target.Character.Hp = (int)target.Character.HPLoad();
                            target.Character.Mp = (int)target.Character.MPLoad();
                            short x = (short)(39 + ServerManager.Instance.RandomNumber(-2, 3));
                            short y = (short)(42 + ServerManager.Instance.RandomNumber(-2, 3));
                            ServerManager.Instance.ChangeMap(target.Character.CharacterId, 130, x, y);
                            target.CurrentMapInstance?.Broadcast(target, target.Character.GenerateTp());
                            target.CurrentMapInstance?.Broadcast(target.Character.GenerateRevive());
                            target.SendPacket(target.Character.GenerateStat());
                        });
                    }
                    else
                    {
                        hitRequest.Session.Character.TalentWin++;
                        target.Character.TalentLose++;
                        Observable.Timer(TimeSpan.FromMilliseconds(1000)).Subscribe(o => ServerManager.Instance.AskPVPRevive(target.Character.CharacterId));
                    }
                }

                hitRequest.Skill.BCards.Where(s => s.Type.Equals((byte)BCardType.CardType.Buff)).ToList().ForEach(s => s.ApplyBCards(target.Character));

                switch (hitRequest.TargetHitType)
                {
                    case TargetHitType.SingleTargetHit:
                        hitRequest.Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.SkillUsed(UserType.Player, hitRequest.Session.Character.CharacterId, 1, target.Character.CharacterId, hitRequest.Skill.SkillVNum, hitRequest.Skill.Cooldown, hitRequest.Skill.AttackAnimation, hitRequest.SkillEffect, hitRequest.Session.Character.PositionX, hitRequest.Session.Character.PositionY, IsAlive, (int)((float)target.Character.Hp / (float)target.Character.HPLoad() * 100), damage, hitmode, (byte)(hitRequest.Skill.SkillType - 1)));
                        break;

                    case TargetHitType.SingleTargetHitCombo:
                        hitRequest.Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.SkillUsed(UserType.Player, hitRequest.Session.Character.CharacterId, 1, target.Character.CharacterId, hitRequest.Skill.SkillVNum, hitRequest.Skill.Cooldown, hitRequest.SkillCombo.Animation, hitRequest.SkillCombo.Effect, hitRequest.Session.Character.PositionX, hitRequest.Session.Character.PositionY, IsAlive, (int)((float)target.Character.Hp / (float)target.Character.HPLoad() * 100), damage, hitmode, (byte)(hitRequest.Skill.SkillType - 1)));
                        break;

                    case TargetHitType.SingleAOETargetHit:
                        switch (hitmode)
                        {
                            case 1:
                                hitmode = 4;
                                break;

                            case 3:
                                hitmode = 6;
                                break;

                            default:
                                hitmode = 5;
                                break;
                        }
                        if (hitRequest.ShowTargetHitAnimation)
                        {
                            hitRequest.Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.SkillUsed(UserType.Player, hitRequest.Session.Character.CharacterId, 1, target.Character.CharacterId, hitRequest.Skill.SkillVNum, hitRequest.Skill.Cooldown, hitRequest.Skill.AttackAnimation, hitRequest.SkillEffect, 0, 0, IsAlive, (int)((float)target.Character.Hp / (float)target.Character.HPLoad() * 100), 0, 0, (byte)(hitRequest.Skill.SkillType - 1)));
                        }
                        hitRequest.Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.SkillUsed(UserType.Player, hitRequest.Session.Character.CharacterId, 1, target.Character.CharacterId, hitRequest.Skill.SkillVNum, hitRequest.Skill.Cooldown, hitRequest.Skill.AttackAnimation, hitRequest.SkillEffect, hitRequest.Session.Character.PositionX, hitRequest.Session.Character.PositionY, IsAlive, (int)((float)target.Character.Hp / (float)target.Character.HPLoad() * 100), damage, hitmode, (byte)(hitRequest.Skill.SkillType - 1)));
                        break;

                    case TargetHitType.AOETargetHit:
                        switch (hitmode)
                        {
                            case 1:
                                hitmode = 4;
                                break;

                            case 3:
                                hitmode = 6;
                                break;

                            default:
                                hitmode = 5;
                                break;
                        }
                        hitRequest.Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.SkillUsed(UserType.Player, hitRequest.Session.Character.CharacterId, 1, target.Character.CharacterId, hitRequest.Skill.SkillVNum, hitRequest.Skill.Cooldown, hitRequest.Skill.AttackAnimation, hitRequest.SkillEffect, hitRequest.Session.Character.PositionX, hitRequest.Session.Character.PositionY, IsAlive, (int)((float)target.Character.Hp / (float)target.Character.HPLoad() * 100), damage, hitmode, (byte)(hitRequest.Skill.SkillType - 1)));
                        break;

                    case TargetHitType.ZoneHit:
                        hitRequest.Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.SkillUsed(UserType.Player, hitRequest.Session.Character.CharacterId, 1, target.Character.CharacterId, hitRequest.Skill.SkillVNum, hitRequest.Skill.Cooldown, hitRequest.Skill.AttackAnimation, hitRequest.SkillEffect, hitRequest.MapX, hitRequest.MapY, IsAlive, (int)((float)target.Character.Hp / (float)target.Character.HPLoad() * 100), damage, 5, (byte)(hitRequest.Skill.SkillType - 1)));
                        break;

                    case TargetHitType.SpecialZoneHit:
                        hitRequest.Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.SkillUsed(UserType.Player, hitRequest.Session.Character.CharacterId, 1, target.Character.CharacterId, hitRequest.Skill.SkillVNum, hitRequest.Skill.Cooldown, hitRequest.Skill.AttackAnimation, hitRequest.SkillEffect, hitRequest.Session.Character.PositionX, hitRequest.Session.Character.PositionY, IsAlive, (int)((float)target.Character.Hp / (float)target.Character.HPLoad() * 100), damage, 0, (byte)(hitRequest.Skill.SkillType - 1)));
                        break;

                    default:
                        Logger.Log.Warn("Not Implemented TargetHitType Handling!");
                        break;
                }
            }
            else
            {
                // monster already has been killed, send cancel
                hitRequest.Session.SendPacket(StaticPacketHelper.Cancel(2, target.Character.CharacterId));
            }
        }

        private void TargetHit(int castingId, int targetId, bool isPvp = false)
        {
            bool _shouldCancel = true;
            if ((DateTime.Now - Session.Character.LastTransform).TotalSeconds < 3)
            {
                Session.SendPacket(StaticPacketHelper.Cancel());
                Session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("CANT_ATTACK"), 0));
                return;
            }

            List<CharacterSkill> skills = Session.Character.UseSp ? Session.Character.SkillsSp?.GetAllItems() : Session.Character.Skills?.GetAllItems();
            if (skills != null)
            {
                CharacterSkill ski = skills.Find(s => s.Skill?.CastId == castingId && s.Skill?.UpgradeSkill == 0);
                if (castingId != 0)
                {
                    Session.SendPacket("ms_c 0");
                }
                if (ski != null)
                {
                    if (!Session.Character.WeaponLoaded(ski) || !ski.CanBeUsed())
                    {
                        Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                        return;
                    }
                    foreach (BCard bc in ski.Skill.BCards.Where(s => s.Type.Equals((byte)BCardType.CardType.MeditationSkill)))
                    {
                        _shouldCancel = false;
                        bc.ApplyBCards(Session.Character);
                    }

                    if (Session.Character.Mp >= ski.Skill.MpCost)
                    {
                        // AOE Target hit
                        if (ski.Skill.TargetType == 1 && ski.Skill.HitType == 1)
                        {
                            if (!Session.Character.HasGodMode)
                            {
                                Session.Character.Mp -= ski.Skill.MpCost;
                            }

                            if (Session.Character.UseSp && ski.Skill.CastEffect != -1)
                            {
                                Session.SendPackets(Session.Character.GenerateQuicklist());
                            }

                            Session.SendPacket(Session.Character.GenerateStat());
                            CharacterSkill skillinfo = Session.Character.Skills.GetAllItems().OrderBy(o => o.SkillVNum).FirstOrDefault(s => s.Skill.UpgradeSkill == ski.Skill.SkillVNum && s.Skill.Effect > 0 && s.Skill.SkillType == 2);
                            Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.CastOnTarget(UserType.Player, Session.Character.CharacterId, 1, Session.Character.CharacterId, ski.Skill.CastAnimation, skillinfo?.Skill.CastEffect ?? ski.Skill.CastEffect, ski.Skill.SkillVNum));

                            // Generate scp
                            ski.LastUse = DateTime.Now;
                            if (ski.Skill.CastEffect != 0)
                            {
                                Thread.Sleep(ski.Skill.CastTime * 100);
                            }
                            if (Session.HasCurrentMapInstance)
                            {
                                Session.CurrentMapInstance.Broadcast(StaticPacketHelper.SkillUsed(UserType.Player, Session.Character.CharacterId, 1, Session.Character.CharacterId, ski.Skill.SkillVNum, ski.Skill.Cooldown, ski.Skill.AttackAnimation, skillinfo?.Skill.Effect ?? ski.Skill.Effect, Session.Character.PositionX, Session.Character.PositionY, true, (int)((double)Session.Character.Hp / Session.Character.HPLoad() * 100), 0, -2, (byte)(ski.Skill.SkillType - 1)));
                                if (ski.Skill.TargetRange != 0 && Session.HasCurrentMapInstance)
                                {
                                    foreach (ClientSession character in ServerManager.Instance.Sessions.Where(s => s.CurrentMapInstance == Session.CurrentMapInstance && s.Character.CharacterId != Session.Character.CharacterId && s.Character.IsInRange(Session.Character.PositionX, Session.Character.PositionY, ski.Skill.TargetRange)))
                                    {
                                        if (Session.CurrentMapInstance.Map.MapTypes.Any(s => s.MapTypeId == (short)MapTypeEnum.Act4))
                                        {
                                            if (Session.Character.Family == null || character.Character.Family == null || Session.Character.Family.FamilyId != character.Character.Family.FamilyId)
                                            {
                                                if (Session.Character.MapInstance.Map.MapId != 130 && Session.Character.MapInstance.Map.MapId != 131)
                                                {
                                                    PVPHit(new HitRequest(TargetHitType.AOETargetHit, Session, ski.Skill), character);
                                                }
                                            }
                                        }
                                        else if (Session.Character.MapInstance.Map.MapTypes.Any(m => m.MapTypeId == (short)MapTypeEnum.PVPMap))
                                        {
                                            if (Session.Character.Group == null || !Session.Character.Group.IsMemberOfGroup(character.Character.CharacterId))
                                            {
                                                PVPHit(new HitRequest(TargetHitType.AOETargetHit, Session, ski.Skill), character);
                                            }
                                        }
                                        else if (Session.CurrentMapInstance.IsPVP)
                                        {
                                            if (Session.Character.Group == null || !Session.Character.Group.IsMemberOfGroup(character.Character.CharacterId))
                                            {
                                                PVPHit(new HitRequest(TargetHitType.AOETargetHit, Session, ski.Skill), character);
                                            }
                                        }
                                        else
                                        {
                                            Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                        }
                                    }
                                    foreach (MapMonster mon in Session.CurrentMapInstance.GetListMonsterInRange(Session.Character.PositionX, Session.Character.PositionY, ski.Skill.TargetRange).Where(s => s.CurrentHp > 0))
                                    {
                                        mon.HitQueue.Enqueue(new HitRequest(TargetHitType.AOETargetHit, Session, ski.Skill, skillinfo?.Skill.Effect ?? ski.Skill.Effect));
                                    }
                                }
                            }
                        }
                        else if (ski.Skill.TargetType == 2 && ski.Skill.HitType == 0)
                        {
                            Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.CastOnTarget(UserType.Player, Session.Character.CharacterId, 1, Session.Character.CharacterId, ski.Skill.CastAnimation, ski.Skill.CastEffect, ski.Skill.SkillVNum));
                            Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.SkillUsed(UserType.Player, Session.Character.CharacterId, 1, targetId, ski.Skill.SkillVNum, ski.Skill.Cooldown, ski.Skill.AttackAnimation, ski.Skill.Effect, Session.Character.PositionX, Session.Character.PositionY, true, (int)((double)Session.Character.Hp / Session.Character.HPLoad() * 100), 0, -1, (byte)(ski.Skill.SkillType - 1)));
                            ClientSession target = ServerManager.Instance.GetSessionByCharacterId(targetId) ?? Session;
                            ski.Skill.BCards.Where(s => !s.Type.Equals((byte)BCardType.CardType.MeditationSkill)).ToList().ForEach(s => s.ApplyBCards(target.Character));
                        }
                        else if (ski.Skill.TargetType == 1 && ski.Skill.HitType != 1)
                        {
                            Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.CastOnTarget(UserType.Player, Session.Character.CharacterId, 1, Session.Character.CharacterId, ski.Skill.CastAnimation, ski.Skill.CastEffect, ski.Skill.SkillVNum));
                            Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.SkillUsed(UserType.Player, Session.Character.CharacterId, 1, Session.Character.CharacterId, ski.Skill.SkillVNum, ski.Skill.Cooldown, ski.Skill.AttackAnimation, ski.Skill.Effect, Session.Character.PositionX, Session.Character.PositionY, true, (int)((double)Session.Character.Hp / Session.Character.HPLoad() * 100), 0, -1, (byte)(ski.Skill.SkillType - 1)));
                            switch (ski.Skill.HitType)
                            {
                                case 2:
                                    IEnumerable<ClientSession> clientSessions = Session.CurrentMapInstance?.Sessions?.Where(s => s.Character.IsInRange(Session.Character.PositionX, Session.Character.PositionY, ski.Skill.TargetRange));
                                    if (clientSessions != null)
                                    {
                                        foreach (ClientSession target in clientSessions)
                                        {
                                            ski.Skill.BCards.Where(s => !s.Type.Equals((byte)BCardType.CardType.MeditationSkill)).ToList().ForEach(s => s.ApplyBCards(target.Character));
                                        }
                                    }
                                    break;

                                case 4:
                                case 0:
                                    ski.Skill.BCards.Where(s => !s.Type.Equals((byte)BCardType.CardType.MeditationSkill)).ToList().ForEach(s => s.ApplyBCards(Session.Character));
                                    break;
                            }
                        }
                        else if (ski.Skill.TargetType == 0 && Session.HasCurrentMapInstance) // monster target
                        {
                            if (isPvp)
                            {
                                ClientSession playerToAttack = ServerManager.Instance.GetSessionByCharacterId(targetId);
                                if (playerToAttack != null && Session.Character.Mp >= ski.Skill.MpCost)
                                {
                                    if (Map.GetDistance(new MapCell { X = Session.Character.PositionX, Y = Session.Character.PositionY }, new MapCell { X = playerToAttack.Character.PositionX, Y = playerToAttack.Character.PositionY }) <= ski.Skill.Range + 1)
                                    {
                                        if (!Session.Character.HasGodMode)
                                        {
                                            Session.Character.Mp -= ski.Skill.MpCost;
                                        }
                                        if (Session.Character.UseSp && ski.Skill.CastEffect != -1)
                                        {
                                            Session.SendPackets(Session.Character.GenerateQuicklist());
                                        }
                                        Session.SendPacket(Session.Character.GenerateStat());
                                        CharacterSkill characterSkillInfo = Session.Character.Skills.GetAllItems().OrderBy(o => o.SkillVNum).FirstOrDefault(s => s.Skill.UpgradeSkill == ski.Skill.SkillVNum && s.Skill.Effect > 0 && s.Skill.SkillType == 2);
                                        Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.CastOnTarget(UserType.Player, Session.Character.CharacterId, 3, targetId, ski.Skill.CastAnimation, characterSkillInfo?.Skill.CastEffect ?? ski.Skill.CastEffect, ski.Skill.SkillVNum));
                                        Session.Character.Skills.GetAllItems().Where(s => s.Id != ski.Id).ToList().ForEach(i => i.Hit = 0);

                                        // Generate scp
                                        if ((DateTime.Now - ski.LastUse).TotalSeconds > 3)
                                        {
                                            ski.Hit = 0;
                                        }
                                        else
                                        {
                                            ski.Hit++;
                                        }
                                        ski.LastUse = DateTime.Now;

                                        if (ski.Skill.CastEffect != 0)
                                        {
                                            Thread.Sleep(ski.Skill.CastTime * 100);
                                        }

                                        // check if we will hit mutltiple targets
                                        if (ski.Skill.TargetRange != 0)
                                        {
                                            ComboDTO skillCombo = ski.Skill.Combos.Find(s => ski.Hit == s.Hit);
                                            if (skillCombo != null)
                                            {
                                                if (ski.Skill.Combos.OrderByDescending(s => s.Hit).First().Hit == ski.Hit)
                                                {
                                                    ski.Hit = 0;
                                                }
                                                IEnumerable<ClientSession> playersInAOERange = ServerManager.Instance.Sessions.Where(s => s.CurrentMapInstance == Session.CurrentMapInstance && s.Character.CharacterId != Session.Character.CharacterId && s.Character.IsInRange(Session.Character.PositionX, Session.Character.PositionY, ski.Skill.TargetRange));
                                                int count = 0;
                                                foreach (ClientSession character in playersInAOERange)
                                                {
                                                    if (Session.CurrentMapInstance?.Map.MapTypes.Any(s => s.MapTypeId == (short)MapTypeEnum.Act4) == true)
                                                    {
                                                        if (Session.Character.Family == null || character.Character.Family == null || Session.Character.Family.FamilyId != character.Character.Family.FamilyId)
                                                        {
                                                            if (Session.Character.MapInstance.Map.MapId != 130 && Session.Character.MapInstance.Map.MapId != 131)
                                                            {
                                                                count++;
                                                                PVPHit(new HitRequest(TargetHitType.SingleTargetHitCombo, Session, ski.Skill, skillCombo: skillCombo), playerToAttack);
                                                            }
                                                        }
                                                    }
                                                    else if (Session.Character.MapInstance.Map.MapTypes.Any(m => m.MapTypeId == (short)MapTypeEnum.PVPMap))
                                                    {
                                                        if (Session.Character.Group == null || !Session.Character.Group.IsMemberOfGroup(character.Character.CharacterId))
                                                        {
                                                            count++;
                                                            PVPHit(new HitRequest(TargetHitType.SingleTargetHitCombo, Session, ski.Skill, skillCombo: skillCombo), playerToAttack);
                                                        }
                                                    }
                                                    else if (Session.CurrentMapInstance?.IsPVP == true)
                                                    {
                                                        if (Session.Character.Group == null || !Session.Character.Group.IsMemberOfGroup(character.Character.CharacterId))
                                                        {
                                                            count++;
                                                            PVPHit(new HitRequest(TargetHitType.SingleTargetHitCombo, Session, ski.Skill, skillCombo: skillCombo), playerToAttack);
                                                        }
                                                    }
                                                }
                                                if (playerToAttack.Character.Hp <= 0 || count == 0)
                                                {
                                                    Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                }
                                            }
                                            else
                                            {
                                                IEnumerable<ClientSession> playersInAOERange = ServerManager.Instance.Sessions.Where(s => s.CurrentMapInstance == Session.CurrentMapInstance && s.Character.CharacterId != Session.Character.CharacterId && s.Character.IsInRange(Session.Character.PositionX, Session.Character.PositionY, ski.Skill.TargetRange));

                                                // hit the targetted monster
                                                if (Session.CurrentMapInstance?.Map.MapTypes.Any(s => s.MapTypeId == (short)MapTypeEnum.Act4) == true)
                                                {
                                                    if (Session.Character.Family == null || playerToAttack.Character.Family == null || Session.Character.Family.FamilyId != playerToAttack.Character.Family.FamilyId)
                                                    {
                                                        if (Session.Character.MapInstance.Map.MapId != 130 && Session.Character.MapInstance.Map.MapId != 131)
                                                        {
                                                            PVPHit(new HitRequest(TargetHitType.SingleAOETargetHit, Session, ski.Skill), playerToAttack);
                                                        }
                                                        else
                                                        {
                                                            Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                    }
                                                }
                                                else if (Session.Character.MapInstance.Map.MapTypes.Any(m => m.MapTypeId == (short)MapTypeEnum.PVPMap))
                                                {
                                                    if (Session.Character.Group == null || !Session.Character.Group.IsMemberOfGroup(playerToAttack.Character.CharacterId))
                                                    {
                                                        PVPHit(new HitRequest(TargetHitType.SingleAOETargetHit, Session, ski.Skill), playerToAttack);
                                                    }
                                                    else
                                                    {
                                                        Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                    }
                                                }
                                                else if (Session.CurrentMapInstance?.IsPVP == true)
                                                {
                                                    if (Session.Character.Group == null || !Session.Character.Group.IsMemberOfGroup(playerToAttack.Character.CharacterId))
                                                    {
                                                        PVPHit(new HitRequest(TargetHitType.SingleAOETargetHit, Session, ski.Skill), playerToAttack);
                                                    }
                                                    else
                                                    {
                                                        Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                    }
                                                }
                                                else
                                                {
                                                    Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                }

                                                //hit all other monsters
                                                foreach (ClientSession character in playersInAOERange)
                                                {
                                                    if (Session.CurrentMapInstance?.Map.MapTypes.Any(s => s.MapTypeId == (short)MapTypeEnum.Act4) == true)
                                                    {
                                                        if (Session.Character.Family == null || character.Character.Family == null || Session.Character.Family.FamilyId != character.Character.Family.FamilyId)
                                                        {
                                                            if (Session.Character.MapInstance.Map.MapId != 130 && Session.Character.MapInstance.Map.MapId != 131)
                                                            {
                                                                PVPHit(new HitRequest(TargetHitType.SingleAOETargetHit, Session, ski.Skill), character);
                                                            }
                                                        }
                                                    }
                                                    else if (Session.Character.MapInstance.Map.MapTypes.Any(m => m.MapTypeId == (short)MapTypeEnum.PVPMap))
                                                    {
                                                        if (Session.Character.Group == null || !Session.Character.Group.IsMemberOfGroup(character.Character.CharacterId))
                                                        {
                                                            PVPHit(new HitRequest(TargetHitType.SingleAOETargetHit, Session, ski.Skill), character);
                                                        }
                                                    }
                                                    else if (Session.CurrentMapInstance.IsPVP)
                                                    {
                                                        if (Session.Character.Group == null || !Session.Character.Group.IsMemberOfGroup(character.Character.CharacterId))
                                                        {
                                                            PVPHit(new HitRequest(TargetHitType.SingleAOETargetHit, Session, ski.Skill), character);
                                                        }
                                                    }
                                                }
                                                if (playerToAttack.Character.Hp <= 0)
                                                {
                                                    Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ComboDTO skillCombo = ski.Skill.Combos.Find(s => ski.Hit == s.Hit);
                                            if (skillCombo != null)
                                            {
                                                if (ski.Skill.Combos.OrderByDescending(s => s.Hit).First().Hit == ski.Hit)
                                                {
                                                    ski.Hit = 0;
                                                }
                                                if (Session.CurrentMapInstance?.Map.MapTypes.Any(s => s.MapTypeId == (short)MapTypeEnum.Act4) == true)
                                                {
                                                    if (Session.Character.Family == null || playerToAttack.Character.Family == null || Session.Character.Family.FamilyId != playerToAttack.Character.Family.FamilyId)
                                                    {
                                                        if (Session.Character.MapInstance.Map.MapId != 130 && Session.Character.MapInstance.Map.MapId != 131)
                                                        {
                                                            PVPHit(new HitRequest(TargetHitType.SingleTargetHitCombo, Session, ski.Skill, skillCombo: skillCombo), playerToAttack);
                                                        }
                                                        else
                                                        {
                                                            Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                    }
                                                }
                                                else if (Session.Character.MapInstance.Map.MapTypes.Any(m => m.MapTypeId == (short)MapTypeEnum.PVPMap))
                                                {
                                                    if (Session.Character.Group == null || !Session.Character.Group.IsMemberOfGroup(playerToAttack.Character.CharacterId))
                                                    {
                                                        PVPHit(new HitRequest(TargetHitType.SingleTargetHitCombo, Session, ski.Skill, skillCombo: skillCombo), playerToAttack);
                                                    }
                                                    else
                                                    {
                                                        Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                    }
                                                }
                                                else if (Session.CurrentMapInstance?.IsPVP == true)
                                                {
                                                    if (Session.CurrentMapInstance.MapInstanceId != ServerManager.Instance.FamilyArenaInstance.MapInstanceId)
                                                    {
                                                        if (Session.Character.Group == null || !Session.Character.Group.IsMemberOfGroup(playerToAttack.Character.CharacterId))
                                                        {
                                                            PVPHit(new HitRequest(TargetHitType.SingleTargetHit, Session,
                                                                ski.Skill), playerToAttack);
                                                        }
                                                        else
                                                        {
                                                            Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (Session.Character.Family == null || Session.Character.Family.FamilyCharacters.Any(s => s.Character.CharacterId != playerToAttack.Character.CharacterId))
                                                        {
                                                            PVPHit(new HitRequest(TargetHitType.SingleTargetHit, Session, ski.Skill), playerToAttack);
                                                        }
                                                        else
                                                        {
                                                            Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                }
                                            }
                                            else
                                            {
                                                if (Session.CurrentMapInstance?.Map.MapTypes.Any(s => s.MapTypeId == (short)MapTypeEnum.Act4) == true)
                                                {
                                                    if (Session.Character.Family == null || playerToAttack.Character.Family == null || Session.Character.Family.FamilyId != playerToAttack.Character.Family.FamilyId)
                                                    {
                                                        if (Session.Character.MapInstance.Map.MapId != 130 && Session.Character.MapInstance.Map.MapId != 131)
                                                        {
                                                            PVPHit(new HitRequest(TargetHitType.SingleTargetHit, Session, ski.Skill), playerToAttack);
                                                        }
                                                        else
                                                        {
                                                            Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                    }
                                                }
                                                else if (Session.Character.MapInstance.Map.MapTypes.Any(m => m.MapTypeId == (short)MapTypeEnum.PVPMap))
                                                {
                                                    if (Session.Character.Group == null || !Session.Character.Group.IsMemberOfGroup(playerToAttack.Character.CharacterId))
                                                    {
                                                        PVPHit(new HitRequest(TargetHitType.SingleTargetHit, Session, ski.Skill), playerToAttack);
                                                    }
                                                    else
                                                    {
                                                        Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                    }
                                                }
                                                else if (Session.CurrentMapInstance?.IsPVP == true)
                                                {
                                                    if (Session.Character.Group == null || !Session.Character.Group.IsMemberOfGroup(playerToAttack.Character.CharacterId))
                                                    {
                                                        PVPHit(new HitRequest(TargetHitType.SingleTargetHit, Session, ski.Skill), playerToAttack);
                                                    }
                                                    else
                                                    {
                                                        Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                    }
                                                }
                                                else
                                                {
                                                    Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                    }
                                }
                                else
                                {
                                    Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                }
                            }
                            else
                            {
                                MapMonster monsterToAttack = Session.CurrentMapInstance.GetMonster(targetId);
                                if (monsterToAttack != null && Session.Character.Mp >= ski.Skill.MpCost)
                                {
                                    if (Map.GetDistance(new MapCell { X = Session.Character.PositionX, Y = Session.Character.PositionY },
                                                        new MapCell { X = monsterToAttack.MapX, Y = monsterToAttack.MapY }) <= ski.Skill.Range + 1 + monsterToAttack.Monster.BasicArea)
                                    {
                                        if (!Session.Character.HasGodMode)
                                        {
                                            Session.Character.Mp -= ski.Skill.MpCost;
                                        }
                                        if (Session.Character.UseSp && ski.Skill.CastEffect != -1)
                                        {
                                            Session.SendPackets(Session.Character.GenerateQuicklist());
                                        }
                                        monsterToAttack.Monster.BCards.Where(s => s.CastType == 1).ToList().ForEach(s => s.ApplyBCards(this));
                                        Session.SendPacket(Session.Character.GenerateStat());
                                        CharacterSkill characterSkillInfo = Session.Character.Skills.GetAllItems().OrderBy(o => o.SkillVNum).FirstOrDefault(s => s.Skill.UpgradeSkill == ski.Skill.SkillVNum && s.Skill.Effect > 0 && s.Skill.SkillType == 2);

                                        Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.CastOnTarget(UserType.Player, Session.Character.CharacterId, 3, monsterToAttack.MapMonsterId, ski.Skill.CastAnimation, characterSkillInfo?.Skill.CastEffect ?? ski.Skill.CastEffect, ski.Skill.SkillVNum));
                                        Session.Character.Skills.GetAllItems().Where(s => s.Id != ski.Id).ToList().ForEach(i => i.Hit = 0);

                                        // Generate scp
                                        if ((DateTime.Now - ski.LastUse).TotalSeconds > 3)
                                        {
                                            ski.Hit = 0;
                                        }
                                        else
                                        {
                                            ski.Hit++;
                                        }
                                        ski.LastUse = DateTime.Now;
                                        if (ski.Skill.CastEffect != 0)
                                        {
                                            Thread.Sleep(ski.Skill.CastTime * 100);
                                        }
                                        if (ski.Skill.TargetRange != 0) // check if we will hit mutltiple targets
                                        {
                                            ComboDTO skillCombo = ski.Skill.Combos.Find(s => ski.Hit == s.Hit);
                                            if (skillCombo != null)
                                            {
                                                if (ski.Skill.Combos.OrderByDescending(s => s.Hit).First().Hit == ski.Hit)
                                                {
                                                    ski.Hit = 0;
                                                }
                                                IEnumerable<MapMonster> monstersInAOERange = Session.CurrentMapInstance?.GetListMonsterInRange(monsterToAttack.MapX, monsterToAttack.MapY, ski.Skill.TargetRange).ToList();
                                                if (monstersInAOERange != null)
                                                {
                                                    foreach (MapMonster mon in monstersInAOERange)
                                                    {
                                                        mon.HitQueue.Enqueue(new HitRequest(TargetHitType.SingleTargetHitCombo, Session, ski.Skill, skillCombo: skillCombo));
                                                    }
                                                }
                                                else
                                                {
                                                    Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                }
                                                if (!monsterToAttack.IsAlive)
                                                {
                                                    Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                }
                                            }
                                            else
                                            {
                                                IEnumerable<MapMonster> monstersInAOERange = Session.CurrentMapInstance?.GetListMonsterInRange(monsterToAttack.MapX, monsterToAttack.MapY, ski.Skill.TargetRange).ToList();

                                                //hit the targetted monster
                                                monsterToAttack.HitQueue.Enqueue(new HitRequest(TargetHitType.SingleAOETargetHit, Session, ski.Skill, characterSkillInfo?.Skill.Effect ?? ski.Skill.Effect, showTargetAnimation: true));

                                                //hit all other monsters
                                                if (monstersInAOERange != null)
                                                {
                                                    foreach (MapMonster mon in monstersInAOERange.Where(m => m.MapMonsterId != monsterToAttack.MapMonsterId)) //exclude targetted monster
                                                    {
                                                        mon.HitQueue.Enqueue(new HitRequest(TargetHitType.SingleAOETargetHit, Session, ski.Skill, characterSkillInfo?.Skill.Effect ?? ski.Skill.Effect));
                                                    }
                                                }
                                                else
                                                {
                                                    Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                }
                                                if (!monsterToAttack.IsAlive)
                                                {
                                                    Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ComboDTO skillCombo = ski.Skill.Combos.Find(s => ski.Hit == s.Hit);
                                            if (skillCombo != null)
                                            {
                                                if (ski.Skill.Combos.OrderByDescending(s => s.Hit).First().Hit == ski.Hit)
                                                {
                                                    ski.Hit = 0;
                                                }
                                                monsterToAttack.HitQueue.Enqueue(new HitRequest(TargetHitType.SingleTargetHitCombo, Session, ski.Skill, skillCombo: skillCombo));
                                            }
                                            else
                                            {
                                                monsterToAttack.HitQueue.Enqueue(new HitRequest(TargetHitType.SingleTargetHit, Session, ski.Skill));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                    }
                                }
                                else
                                {
                                    Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                }
                            }
                        }
                        else
                        {
                            Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                        }
                        Session.SendPacketAfter(StaticPacketHelper.SkillReset(castingId), ski.Skill.Cooldown * 100);
                    }
                    else
                    {
                        Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                        Session.SendPacket(Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("NOT_ENOUGH_MP"), 10));
                    }
                }
                CharacterSkill characterSkill = skills.Find(s => s.Skill?.CastId == castingId && s.Skill?.UpgradeSkill == 3);
                if (characterSkill != null && (characterSkill.SkillVNum == 209 || characterSkill.SkillVNum == 235 || characterSkill.SkillVNum == 236 || characterSkill.SkillVNum == 237))
                {
                    Catch(characterSkill, targetId, castingId);
                }
            }
            else
            {
                Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
            }

            if ((castingId != 0 && castingId < 11 && _shouldCancel) || Session.Character.SkillComboCount > 7)
            {
                Session.SendPackets(Session.Character.GenerateQuicklist());
                Session.SendPacket("mslot 0 -1");
            }
            Session.Character.LastSkillUse = DateTime.Now;
        }

        private void Catch(CharacterSkill ski, long targetId, int castingId)
        {
            if (ski != null)
            {
                if (ski.Skill.TargetType == 0 && ski.Skill.SkillType == 1 && ski.Skill.Range == 0)
                {
                    MapMonster monsterToCatch = Session.CurrentMapInstance.GetMonster(targetId);
                    if (monsterToCatch != null && Session.Character.Mp >= ski.Skill.MpCost)
                    {
                        NpcMonster mateNpc = ServerManager.Instance.GetNpc(monsterToCatch.MonsterVNum);
                        if (mateNpc != null)
                        {
                            if (Map.GetDistance(new MapCell { X = Session.Character.PositionX, Y = Session.Character.PositionY },
                                            new MapCell { X = monsterToCatch.MapX, Y = monsterToCatch.MapY }) <= ski.Skill.Range + 1 + monsterToCatch.Monster.BasicArea)
                            {
                                if (!Session.Character.HasGodMode)
                                {
                                    Session.Character.Mp -= ski.Skill.MpCost;
                                }
                                monsterToCatch.Monster.BCards.Where(s => s.CastType == 1).ToList().ForEach(s => s.ApplyBCards(this));
                                Session.SendPacket(Session.Character.GenerateStat());
                                CharacterSkill characterSkillInfo = Session.Character.Skills.GetAllItems().OrderBy(o => o.SkillVNum).FirstOrDefault(s => s.Skill.UpgradeSkill == ski.Skill.SkillVNum && s.Skill.Effect > 0 && s.Skill.SkillType == 2);
                                Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.CastOnTarget(UserType.Player, Session.Character.CharacterId, 3, monsterToCatch.MapMonsterId, ski.Skill.CastAnimation, characterSkillInfo?.Skill.CastEffect ?? ski.Skill.CastEffect, ski.Skill.SkillVNum));
                                Session.Character.Skills.GetAllItems().Where(s => s.Id != ski.Id).ToList().ForEach(i => i.Hit = 0);
                                if (monsterToCatch.Monster.Catch)
                                {
                                    if (monsterToCatch.IsAlive && monsterToCatch.CurrentHp <= (int)((double)monsterToCatch.MaxHp / 2))
                                    {
                                        if (monsterToCatch.Monster.Level < Session.Character.Level)
                                        {
                                            int[] chance = { 100, 80, 60, 40, 20, 0 };
                                            int random = ServerManager.Instance.RandomNumber();
                                            if (random < chance[ServerManager.Instance.RandomNumber(0, 5)]) // IDK the real chance T.T
                                            {
                                                Mate mate = new Mate(Session.Character, mateNpc, (byte)(monsterToCatch.Monster.Level - 5 >= 1 ? monsterToCatch.Monster.Level - 5 : monsterToCatch.Monster.Level), MateType.Pet);
                                                if (Session.Character.CanAddMate(mate))
                                                {
                                                    Session.Character.AddPetWithSkill(mate);
                                                    Session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("CATCH_SUCCESS"), 0));
                                                    ski.LastUse = DateTime.Now;
                                                    Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.SkillUsed(UserType.Player, Session.Character.CharacterId, 3, monsterToCatch.MapMonsterId, -1, 0, 15, ski.Skill.Effect, -1, -1, true, (int)((float)monsterToCatch.CurrentHp / (float)monsterToCatch.MaxHp * 100), 0, -1, (byte)(ski.Skill.SkillType - 1)));
                                                    Session.SendPacket(StaticPacketHelper.GenerateEff(UserType.Player, Session.Character.CharacterId, 197));
                                                    Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.Out(UserType.Monster, monsterToCatch.MapMonsterId));
                                                }
                                                else
                                                {
                                                    Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                                    Session.SendPacket(Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("PET_SLOT_FULL"), 10));
                                                }
                                            }
                                            else
                                            {
                                                Session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("CATCH_FAIL"), 0));
                                                Session.CurrentMapInstance?.Broadcast(StaticPacketHelper.SkillUsed(UserType.Player, Session.Character.CharacterId, 3, monsterToCatch.MapMonsterId, -1, 0, 16, ski.Skill.Effect, -1, -1, true, (int)((float)monsterToCatch.CurrentHp / (float)monsterToCatch.MaxHp * 100), 0, -1, (byte)(ski.Skill.SkillType - 1)));
                                            }
                                        }
                                        else
                                        {
                                            Session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("LEVEL_LOW"), 0));
                                            Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                        }
                                    }
                                    else
                                    {
                                        Session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("HP_LOW_50"), 0));
                                        Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                    }
                                }
                                else
                                {
                                    Session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("CANT_CATCH_THIS"), 0));
                                    Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                                }
                                Session.SendPacket($"sr -10 {castingId} {ski.Skill.Cooldown}");
                                Session.SendPacketAfter(StaticPacketHelper.SkillReset(castingId), ski.Skill.Cooldown * 100);
                            }
                            else
                            {
                                Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                            }
                        }
                        else
                        {
                            Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                        }
                    }
                    else
                    {
                        Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                        Session.SendPacket(Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("NOT_ENOUGH_MP"), 10));
                    }
                }
                else
                {
                    Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
                }
            }
            else
            {
                Session.SendPacket(StaticPacketHelper.Cancel(2, targetId));
            }
        }

        private void ZoneHit(int castingId, short x, short y)
        {
            List<CharacterSkill> skills = Session.Character.UseSp ? Session.Character.SkillsSp.GetAllItems() : Session.Character.Skills.GetAllItems();
            CharacterSkill characterSkill = skills.Find(s => s.Skill.CastId == castingId);
            if (!Session.Character.WeaponLoaded(characterSkill) || !Session.HasCurrentMapInstance)
            {
                Session.SendPacket(StaticPacketHelper.Cancel(2));
                return;
            }

            if (characterSkill?.CanBeUsed() == true)
            {
                if (Session.Character.Mp >= characterSkill.Skill.MpCost)
                {
                    Session.CurrentMapInstance?.Broadcast($"ct_n 1 {Session.Character.CharacterId} 3 -1 {characterSkill.Skill.CastAnimation} {characterSkill.Skill.CastEffect} {characterSkill.Skill.SkillVNum}");
                    characterSkill.LastUse = DateTime.Now;
                    if (!Session.Character.HasGodMode)
                    {
                        Session.Character.Mp -= characterSkill.Skill.MpCost;
                    }
                    Session.SendPacket(Session.Character.GenerateStat());
                    characterSkill.LastUse = DateTime.Now;
                    Observable.Timer(TimeSpan.FromMilliseconds(characterSkill.Skill.CastTime * 100)).Subscribe(o =>
                    {
                        Session.Character.LastSkillUse = DateTime.Now;

                        Session.CurrentMapInstance?.Broadcast($"bs 1 {Session.Character.CharacterId} {x} {y} {characterSkill.Skill.SkillVNum} {characterSkill.Skill.Cooldown} {characterSkill.Skill.AttackAnimation} {characterSkill.Skill.Effect} 0 0 1 1 0 0 0");

                        IEnumerable<MapMonster> monstersInRange = Session.CurrentMapInstance?.GetListMonsterInRange(x, y, characterSkill.Skill.TargetRange).ToList();
                        if (monstersInRange != null)
                        {
                            foreach (MapMonster mon in monstersInRange.Where(s => s.CurrentHp > 0))
                            {
                                mon.HitQueue.Enqueue(new HitRequest(TargetHitType.ZoneHit, Session, characterSkill.Skill, x, y));
                            }
                        }
                        foreach (ClientSession character in ServerManager.Instance.Sessions.Where(s => s.CurrentMapInstance == Session.CurrentMapInstance && s.Character.CharacterId != Session.Character.CharacterId && s.Character.IsInRange(x, y, characterSkill.Skill.TargetRange)))
                        {
                            if (Session.CurrentMapInstance?.Map.MapTypes.Any(s => s.MapTypeId == (short)MapTypeEnum.Act4) == true)
                            {
                                if (Session.Character.Family == null || character.Character.Family == null || Session.Character.Family.FamilyId != character.Character.Family.FamilyId)
                                {
                                    if (Session.Character.MapInstance.Map.MapId != 130 && Session.Character.MapInstance.Map.MapId != 131)
                                    {
                                        PVPHit(new HitRequest(TargetHitType.ZoneHit, Session, characterSkill.Skill, x, y), character);
                                    }
                                }
                            }
                            else if (Session.Character.MapInstance.Map.MapTypes.Any(m => m.MapTypeId == (short)MapTypeEnum.PVPMap))
                            {
                                if (Session.Character.Group == null || !Session.Character.Group.IsMemberOfGroup(character.Character.CharacterId))
                                {
                                    PVPHit(new HitRequest(TargetHitType.ZoneHit, Session, characterSkill.Skill, x, y), character);
                                }
                            }
                            else if (Session.CurrentMapInstance?.IsPVP == true)
                            {
                                if (Session.Character.Group == null || !Session.Character.Group.IsMemberOfGroup(character.Character.CharacterId))
                                {
                                    PVPHit(new HitRequest(TargetHitType.ZoneHit, Session, characterSkill.Skill, x, y), character);
                                }
                            }
                        }
                    });
                    Observable.Timer(TimeSpan.FromMilliseconds(characterSkill.Skill.Cooldown * 100)).Subscribe(o => Session.SendPacket(StaticPacketHelper.SkillReset(castingId)));
                }
                else
                {
                    Session.SendPacket(Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("NOT_ENOUGH_MP"), 10));
                    Session.SendPacket(StaticPacketHelper.Cancel(2));
                }
            }
            else
            {
                Session.SendPacket(StaticPacketHelper.Cancel(2));
            }
        }

        #endregion
    }
}