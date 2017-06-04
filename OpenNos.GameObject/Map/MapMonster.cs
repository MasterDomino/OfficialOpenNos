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
using OpenNos.GameObject.Helpers;
using OpenNos.GameObject.Networking;
using OpenNos.PathFinder;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using static OpenNos.Domain.BCardType;

namespace OpenNos.GameObject
{
    public class MapMonster : MapMonsterDTO
    {
        #region Members

        private int _movetime;
        private Random _random;

        #endregion

        #region Instantiation

        public MapMonster()
        {
            HitQueue = new ConcurrentQueue<HitRequest>();
            OnDeathEvents = new List<EventContainer>();
        }

        #endregion

        #region Properties

        public int CurrentHp { get; set; }

        public int CurrentMp { get; set; }

        public IDictionary<long, long> DamageList { get; private set; }

        public DateTime Death { get; set; }

        public ConcurrentQueue<HitRequest> HitQueue { get; }

        public bool IsAlive { get; set; }

        public bool IsBonus { get; set; }

        public bool IsHostile { get; set; }

        public bool IsTarget { get; set; }

        public DateTime LastEffect { get; set; }

        public DateTime LastMove { get; set; }

        public DateTime LastSkill { get; set; }

        public IDisposable LifeEvent { get; set; }

        public MapInstance MapInstance { get; set; }

        public NpcMonster Monster { get; private set; }

        public List<EventContainer> OnDeathEvents { get; set; }

        public List<Node> Path { get; set; }

        public bool? ShouldRespawn { get; set; }

        public List<NpcMonsterSkill> Skills { get; set; }

        public bool Started { get; internal set; }

        public long Target { get; set; }

        private short FirstX { get; set; }

        private short FirstY { get; set; }

        #endregion

        #region Methods

        public EffectPacket GenerateEff(int effectid)
        {
            return new EffectPacket
            {
                EffectType = 3,
                CharacterId = MapMonsterId,
                Id = effectid
            };
        }

        public string GenerateIn()
        {
            if (IsAlive && !IsDisabled)
            {
                return $"in 3 {MonsterVNum} {MapMonsterId} {MapX} {MapY} {Position} {(int)((float)CurrentHp / (float)Monster.MaxHP * 100)} {(int)((float)CurrentMp / (float)Monster.MaxMP * 100)} 0 0 0 -1 {(Monster.NoAggresiveIcon ? (byte)InRespawnType.NoEffect : (byte)InRespawnType.TeleportationEffect)} 0 -1 - 0 -1 0 0 0 0 0 0 0 0";
            }
            return string.Empty;
        }

        public string GenerateOut()
        {
            return $"out 3 {MapMonsterId}";
        }

        public string GenerateSay(string message, int type)
        {
            return $"say 3 {MapMonsterId} {type} {message}";
        }

        public void Initialize(MapInstance currentMapInstance)
        {
            MapInstance = currentMapInstance;
            Initialize();
            StartLife();
        }

        public override void Initialize()
        {
            FirstX = MapX;
            FirstY = MapY;
            LastSkill = LastMove = LastEffect = DateTime.Now;
            Target = -1;
            Path = new List<Node>();
            IsAlive = true;
            ShouldRespawn = ShouldRespawn ?? true;
            Monster = ServerManager.Instance.GetNpc(MonsterVNum);
            IsHostile = Monster.IsHostile;
            CurrentHp = Monster.MaxHP;
            CurrentMp = Monster.MaxMP;
            Skills = Monster.Skills.ToList();
            DamageList = new Dictionary<long, long>();
            _random = new Random(MapMonsterId);
            _movetime = ServerManager.Instance.RandomNumber(400, 3200);
        }

        /// <summary>
        /// Check if the Monster is in the given Range.
        /// </summary>
        /// <param name="mapX">The X coordinate on the Map of the object to check.</param>
        /// <param name="mapY">The Y coordinate on the Map of the object to check.</param>
        /// <param name="distance">The maximum distance of the object to check.</param>
        /// <returns>True if the Monster is in range, False if not.</returns>
        public bool IsInRange(short mapX, short mapY, byte distance)
        {
            return Map.GetDistance(
             new MapCell
             {
                 X = mapX,
                 Y = mapY
             }, new MapCell
             {
                 X = MapX,
                 Y = MapY
             }) <= distance + 1;
        }

        public void RunDeathEvent()
        {
            if (IsBonus)
            {
                MapInstance.InstanceBag.Combo++;
                MapInstance.InstanceBag.Point += EventHelper.Instance.CalculateComboPoint(MapInstance.InstanceBag.Combo + 1);
            }
            else
            {
                MapInstance.InstanceBag.Combo = 0;
                MapInstance.InstanceBag.Point += EventHelper.Instance.CalculateComboPoint(MapInstance.InstanceBag.Combo);
            }
            MapInstance.InstanceBag.MonstersKilled++;
            OnDeathEvents.ForEach(e =>
            {
                EventHelper.Instance.RunEvent(e);
            });
        }

        public void StartLife()
        {
            Observable.Interval(TimeSpan.FromMilliseconds(400)).Subscribe(x =>
            {
                try
                {
                    if (!MapInstance.IsSleeping)
                    {
                        MonsterLife();
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            });
        }

        internal void GetNearestOponent()
        {
            if (Target == -1)
            {
                const int maxDistance = 100;
                int distance = 100;
                List<ClientSession> sess = new List<ClientSession>();
                DamageList.Keys.ToList().ForEach(s => sess.Add(MapInstance.GetSessionByCharacterId(s)));
                ClientSession session = sess.OrderBy(s => distance = Map.GetDistance(new MapCell { X = MapX, Y = MapY }, new MapCell { X = s.Character.PositionX, Y = s.Character.PositionY })).FirstOrDefault();
                if (distance < maxDistance)
                {
                    if (session != null)
                    {
                        Target = session.Character.CharacterId;
                    }
                }
            }
        }

        internal void HostilityTarget()
        {
            if (IsHostile && Target == -1)
            {
                Character character = ServerManager.Instance.Sessions.FirstOrDefault(s => s?.Character != null && s.Character.Hp > 0 && !s.Character.InvisibleGm && !s.Character.Invisible && s.Character.MapInstance == MapInstance && Map.GetDistance(new MapCell { X = MapX, Y = MapY }, new MapCell { X = s.Character.PositionX, Y = s.Character.PositionY }) < Monster.NoticeRange)?.Character;
                if (character != null)
                {
                    Target = character.CharacterId;
                    if (!Monster.NoAggresiveIcon)
                    {
                        character.Session.SendPacket(GenerateEff(5000));
                    }
                }
            }
        }

        /// <summary>
        /// Remove the current Target from Monster.
        /// </summary>
        internal void RemoveTarget()
        {
            if (Target != -1)
            {
                Path.Clear();
                Target = -1;
                //return to origin
                Path = BestFirstSearch.FindPath(new Node { X = MapX, Y = MapY }, new Node { X = FirstX, Y = FirstY }, MapInstance.Map.Grid);
            }
        }

        /// <summary>
        /// Follow the Monsters target to it's position.
        /// </summary>
        /// <param name="targetSession">The TargetSession to follow</param>
        private void FollowTarget(ClientSession targetSession)
        {
            if (IsMoving)
            {
                short maxDistance = 22;
                int distance = 0;
                if (!Path.Any() && targetSession != null)
                {
                    short xoffset = (short)ServerManager.Instance.RandomNumber(-1, 1);
                    short yoffset = (short)ServerManager.Instance.RandomNumber(-1, 1);
                    try
                    {
                        List<Node> list = BestFirstSearch.TracePath(new Node() { X = MapX, Y = MapY }, targetSession.Character.BrushFire, targetSession.Character.MapInstance.Map.Grid);
                        Path = list;
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Error($"Pathfinding using Pathfinder failed. Map: {MapId} StartX: {MapX} StartY: {MapY} TargetX: {(short)(targetSession.Character.PositionX + xoffset)} TargetY: {(short)(targetSession.Character.PositionY + yoffset)}", ex);
                        RemoveTarget();
                    }
                }
                if (Monster != null && DateTime.Now > LastMove && Monster.Speed > 0 && Path.Any())
                {
                    int maxindex = Path.Count > Monster.Speed / 2 ? Monster.Speed / 2 : Path.Count;
                    short mapX = Path.ElementAt(maxindex - 1).X;
                    short mapY = Path.ElementAt(maxindex - 1).Y;
                    double waitingtime = Map.GetDistance(new MapCell { X = mapX, Y = mapY }, new MapCell { X = MapX, Y = MapY }) / (double)Monster.Speed;
                    MapInstance.Broadcast(new BroadcastPacket(null, $"mv 3 {MapMonsterId} {mapX} {mapY} {Monster.Speed}", ReceiverType.All, xCoordinate: mapX, yCoordinate: mapY));
                    LastMove = DateTime.Now.AddSeconds(waitingtime > 1 ? 1 : waitingtime);

                    Observable.Timer(TimeSpan.FromMilliseconds((int)((waitingtime > 1 ? 1 : waitingtime) * 1000))).Subscribe(x =>
                    {
                        MapX = mapX;
                        MapY = mapY;
                    });
                    distance = (int)Path.First().F;
                    Path.RemoveRange(0, maxindex);
                }

                if (targetSession == null || MapId != targetSession.Character.MapInstance.Map.MapId || distance > maxDistance)
                {
                    RemoveTarget();
                }
            }
        }

        private string GenerateMv3()
        {
            return $"mv 3 {MapMonsterId} {MapX} {MapY} {Monster.Speed}";
        }

        /// <summary>
        /// Handle any kind of Monster interaction
        /// </summary>
        private void MonsterLife()
        {
            if (Monster == null)
            {
                return;
            }
            if ((DateTime.Now - LastEffect).TotalSeconds >= 5)
            {
                LastEffect = DateTime.Now;
                if (IsTarget)
                {
                    MapInstance.Broadcast(GenerateEff(824));
                }
                if (IsBonus)
                {
                    MapInstance.Broadcast(GenerateEff(826));
                }
            }

            // handle hit queue
            while (HitQueue.TryDequeue(out HitRequest hitRequest))
            {
                if (IsAlive && hitRequest.Session.Character.Hp > 0)
                {
                    int hitmode = 0;

                    // calculate damage
                    int damage = DamageHelper.Instance.CalculateDamage(new BattleEntity(hitRequest.Session.Character, hitRequest.Skill), new BattleEntity(this), hitRequest.Skill, ref hitmode);

                    if (DamageList.ContainsKey(hitRequest.Session.Character.CharacterId))
                    {
                        DamageList[hitRequest.Session.Character.CharacterId] += damage;
                    }
                    else
                    {
                        DamageList.Add(hitRequest.Session.Character.CharacterId, damage);
                    }
                    if (CurrentHp <= damage)
                    {
                        IsAlive = false;
                        CurrentHp = 0;
                        CurrentMp = 0;
                        Death = DateTime.Now;
                        LastMove = DateTime.Now;
                    }
                    else
                    {
                        CurrentHp -= damage;
                    }

                    // only set the hit delay if we become the monsters target with this hit
                    if (Target == -1)
                    {
                        LastSkill = DateTime.Now;
                    }

                    int nearestDistance = 100;
                    foreach (KeyValuePair<long, long> kvp in DamageList)
                    {
                        ClientSession session = MapInstance.GetSessionByCharacterId(kvp.Key);
                        if (session != null)
                        {
                            int distance = Map.GetDistance(new MapCell
                            {
                                X = MapX,
                                Y = MapY
                            }, new MapCell
                            {
                                X = session.Character.PositionX,
                                Y = session.Character.PositionY
                            });
                            if (distance < nearestDistance)
                            {
                                nearestDistance = distance;
                                Target = session.Character.CharacterId;
                            }
                        }
                    }

                    switch (hitRequest.TargetHitType)
                    {
                        case TargetHitType.SingleTargetHit:
                            MapInstance?.Broadcast($"su 1 {hitRequest.Session.Character.CharacterId} 3 {MapMonsterId} {hitRequest.Skill.SkillVNum} {hitRequest.Skill.Cooldown} {hitRequest.Skill.AttackAnimation} {hitRequest.SkillEffect} {hitRequest.Session.Character.PositionX} {hitRequest.Session.Character.PositionY} {(IsAlive ? 1 : 0)} {(int)((float)CurrentHp / (float)Monster.MaxHP * 100)} {damage} {hitmode} {hitRequest.Skill.SkillType - 1}");
                            break;

                        case TargetHitType.SingleTargetHitCombo:
                            MapInstance?.Broadcast($"su 1 {hitRequest.Session.Character.CharacterId} 3 {MapMonsterId} {hitRequest.Skill.SkillVNum} {hitRequest.Skill.Cooldown} {hitRequest.SkillCombo.Animation} {hitRequest.SkillCombo.Effect} {hitRequest.Session.Character.PositionX} {hitRequest.Session.Character.PositionY} {(IsAlive ? 1 : 0)} {(int)((float)CurrentHp / (float)Monster.MaxHP * 100)} {damage} {hitmode} {hitRequest.Skill.SkillType - 1}");
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
                                MapInstance?.Broadcast($"su 1 {hitRequest.Session.Character.CharacterId} 3 {MapMonsterId} {hitRequest.Skill.SkillVNum} {hitRequest.Skill.Cooldown} {hitRequest.Skill.AttackAnimation} {hitRequest.SkillEffect} 0 0 {(IsAlive ? 1 : 0)} {(int)((float)CurrentHp / (float)Monster.MaxHP * 100)} 0 0 {hitRequest.Skill.SkillType - 1}");
                            }
                            MapInstance?.Broadcast($"su 1 {hitRequest.Session.Character.CharacterId} 3 {MapMonsterId} {hitRequest.Skill.SkillVNum} {hitRequest.Skill.Cooldown} {hitRequest.Skill.AttackAnimation} {hitRequest.SkillEffect} {hitRequest.Session.Character.PositionX} {hitRequest.Session.Character.PositionY} {(IsAlive ? 1 : 0)} {(int)((float)CurrentHp / (float)Monster.MaxHP * 100)} {damage} {hitmode} {hitRequest.Skill.SkillType - 1}");
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
                            MapInstance?.Broadcast($"su 1 {hitRequest.Session.Character.CharacterId} 3 {MapMonsterId} {hitRequest.Skill.SkillVNum} {hitRequest.Skill.Cooldown} {hitRequest.Skill.AttackAnimation} {hitRequest.SkillEffect} {hitRequest.Session.Character.PositionX} {hitRequest.Session.Character.PositionY} {(IsAlive ? 1 : 0)} {(int)((float)CurrentHp / (float)Monster.MaxHP * 100)} {damage} {hitmode} {hitRequest.Skill.SkillType - 1}");
                            break;

                        case TargetHitType.ZoneHit:
                            MapInstance?.Broadcast($"su 1 {hitRequest.Session.Character.CharacterId} 3 {MapMonsterId} {hitRequest.Skill.SkillVNum} {hitRequest.Skill.Cooldown} {hitRequest.Skill.AttackAnimation} {hitRequest.SkillEffect} {hitRequest.MapX} {hitRequest.MapY} {(IsAlive ? 1 : 0)} {(int)((float)CurrentHp / (float)Monster.MaxHP * 100)} {damage} 5 {hitRequest.Skill.SkillType - 1}");
                            break;

                        case TargetHitType.SpecialZoneHit:
                            MapInstance?.Broadcast($"su 1 {hitRequest.Session.Character.CharacterId} 3 {MapMonsterId} {hitRequest.Skill.SkillVNum} {hitRequest.Skill.Cooldown} {hitRequest.Skill.AttackAnimation} {hitRequest.SkillEffect} {hitRequest.Session.Character.PositionX} {hitRequest.Session.Character.PositionY} {(IsAlive ? 1 : 0)} {(int)((float)CurrentHp / (float)Monster.MaxHP * 100)} {damage} 0 {hitRequest.Skill.SkillType - 1}");
                            break;
                    }

                    // generate the kill bonus
                    hitRequest.Session.Character.GenerateKillBonus(this);
                }
                else
                {
                    // monster already has been killed, send cancel
                    hitRequest.Session.SendPacket($"cancel 2 {MapMonsterId}");
                }
            }

            // Respawn
            if (!IsAlive && ShouldRespawn != null && ShouldRespawn.Value)
            {
                double timeDeath = (DateTime.Now - Death).TotalSeconds;
                if (timeDeath >= Monster.RespawnTime / 10d)
                {
                    Respawn();
                }
            }

            // normal movement
            else if (Target == -1)
            {
                Move();
            }

            // target following
            else
            {
                if (MapInstance != null)
                {
                    GetNearestOponent();
                    HostilityTarget();

                    ClientSession targetSession = MapInstance.GetSessionByCharacterId(Target);

                    // remove target in some situations
                    if (targetSession == null || targetSession.Character.Invisible || targetSession.Character.Hp <= 0 || CurrentHp <= 0)
                    {
                        RemoveTarget();
                        return;
                    }

                    NpcMonsterSkill npcMonsterSkill = null;
                    if (ServerManager.Instance.RandomNumber(0, 10) > 8 && Skills != null)
                    {
                        npcMonsterSkill = Skills.Where(s => (DateTime.Now - s.LastSkillUse).TotalMilliseconds >= 100 * s.Skill.Cooldown).OrderBy(rnd => _random.Next()).FirstOrDefault();
                    }

                    // check if target is in range
                    if (!targetSession.Character.InvisibleGm && !targetSession.Character.Invisible && targetSession.Character.Hp > 0)
                    {
                        if (npcMonsterSkill != null && CurrentMp >= npcMonsterSkill.Skill.MpCost &&
                             Map.GetDistance(new MapCell
                             {
                                 X = MapX,
                                 Y = MapY
                             },
                                 new MapCell
                                 {
                                     X = targetSession.Character.PositionX,
                                     Y = targetSession.Character.PositionY
                                 }) < npcMonsterSkill.Skill.Range)
                        {
                            TargetHit(targetSession, npcMonsterSkill);
                        }
                        else if (Map.GetDistance(new MapCell
                        {
                            X = MapX,
                            Y = MapY
                        },
                                    new MapCell
                                    {
                                        X = targetSession.Character.PositionX,
                                        Y = targetSession.Character.PositionY
                                    }) <= Monster.BasicRange)
                        {
                            TargetHit(targetSession, npcMonsterSkill);
                        }
                        else
                        {
                            FollowTarget(targetSession);
                        }
                    }
                    else
                    {
                        FollowTarget(targetSession);
                    }
                }
            }
        }

        private void Move()
        {
            // Normal Move Mode
            if (Monster == null || !IsAlive)
            {
                return;
            }

            if (IsMoving && Monster.Speed > 0)
            {
                double time = (DateTime.Now - LastMove).TotalMilliseconds;

                if (Path.Any(s => s != null)) // move back to initial position after following target
                {
                    int timetowalk = 2000 / Monster.Speed;
                    if (time > timetowalk)
                    {
                        int mapX = Path.ElementAt(0).X;
                        int mapY = Path.ElementAt(0).Y;
                        Path.RemoveAt(0);
                        Observable.Timer(TimeSpan.FromMilliseconds(timetowalk))
                        .Subscribe(
                        x =>
                        {
                            MapX = (short)mapX;
                            MapY = (short)mapY;
                        });

                        LastMove = DateTime.Now;
                        MapInstance.Broadcast(new BroadcastPacket(null, GenerateMv3(), ReceiverType.All, xCoordinate: mapX, yCoordinate: mapY));
                        return;
                    }
                }
                else if (time > _movetime)
                {
                    short mapX = FirstX, mapY = FirstY;
                    if (MapInstance.Map?.GetFreePosition(ref mapX, ref mapY, (byte)ServerManager.Instance.RandomNumber(0, 2), (byte)_random.Next(0, 2)) ?? false)
                    {
                        int distance = Map.GetDistance(new MapCell
                        {
                            X = mapX,
                            Y = mapY
                        }, new MapCell
                        {
                            X = MapX,
                            Y = MapY
                        });

                        double value = 1000d * distance / (2 * Monster.Speed);
                        Observable.Timer(TimeSpan.FromMilliseconds(value))
                    .Subscribe(
                        x =>
                        {
                            MapX = mapX;
                            MapY = mapY;
                        });

                        LastMove = DateTime.Now.AddMilliseconds(value);
                        MapInstance.Broadcast(new BroadcastPacket(null, GenerateMv3(), ReceiverType.All));
                    }
                }
            }
            HostilityTarget();
        }

        private void Respawn()
        {
            if (Monster != null)
            {
                DamageList = new Dictionary<long, long>();
                IsAlive = true;
                Target = -1;
                CurrentHp = Monster.MaxHP;
                CurrentMp = Monster.MaxMP;
                MapX = FirstX;
                MapY = FirstY;
                Path = new List<Node>();
                MapInstance.Broadcast(GenerateIn());
            }
        }

        /// <summary>
        /// Hit the Target Character.
        /// </summary>
        /// <param name="targetSession"></param>
        /// <param name="npcMonsterSkill"></param>
        private void TargetHit(ClientSession targetSession, NpcMonsterSkill npcMonsterSkill)
        {
            if (Monster != null && ((DateTime.Now - LastSkill).TotalMilliseconds >= 1000 + Monster.BasicCooldown * 200 || npcMonsterSkill != null))
            {
                int hitmode = 0;
                int damage = DamageHelper.Instance.CalculateDamage(new BattleEntity(this), new BattleEntity(targetSession.Character, null), npcMonsterSkill?.Skill, ref hitmode);

                if (npcMonsterSkill != null)
                {
                    if (CurrentMp < npcMonsterSkill.Skill.MpCost)
                    {
                        FollowTarget(targetSession);
                        return;
                    }
                    npcMonsterSkill.LastSkillUse = DateTime.Now;
                    CurrentMp -= npcMonsterSkill.Skill.MpCost;
                    MapInstance.Broadcast($"ct 3 {MapMonsterId} 1 {Target} {npcMonsterSkill.Skill.CastAnimation} {npcMonsterSkill.Skill.CastEffect} {npcMonsterSkill.Skill.SkillVNum}");
                }
                LastMove = DateTime.Now;

                // deal 0 damage to GM with GodMode
                if (targetSession.Character.HasGodMode)
                {
                    damage = 0;
                }
                if (targetSession.Character.IsSitting)
                {
                    targetSession.Character.IsSitting = false;
                    MapInstance.Broadcast(targetSession.Character.GenerateRest());
                }
                int castTime = 0;
                if (npcMonsterSkill != null && npcMonsterSkill.Skill.CastEffect != 0)
                {
                    MapInstance.Broadcast(GenerateEff(npcMonsterSkill.Skill.CastEffect), MapX, MapY);
                    castTime = npcMonsterSkill.Skill.CastTime * 100;
                }
                Observable.Timer(TimeSpan.FromMilliseconds(castTime))
                    .Subscribe(
                    o =>
                    {
                        if (targetSession.Character.Hp > 0)
                        {
                            TargetHit2(targetSession, npcMonsterSkill, damage, hitmode);
                        }
                    });
            }
        }

        private void TargetHit2(ClientSession targetSession, NpcMonsterSkill npcMonsterSkill, int damage, int hitmode)
        {
            if (targetSession.Character.Hp > 0)
            {
                targetSession.Character.GetDamage(damage);

                MapInstance.Broadcast(null, ServerManager.Instance.GetUserMethod<string>(Target, "GenerateStat"), ReceiverType.OnlySomeone, "", Target);

                MapInstance.Broadcast(npcMonsterSkill != null
                    ? $"su 3 {MapMonsterId} 1 {Target} {npcMonsterSkill.SkillVNum} {npcMonsterSkill.Skill.Cooldown} {npcMonsterSkill.Skill.AttackAnimation} {npcMonsterSkill.Skill.Effect} {MapX} {MapY} {(targetSession.Character.Hp > 0 ? 1 : 0)} {(int)(targetSession.Character.Hp / targetSession.Character.HPLoad() * 100)} {damage} {hitmode} 0"
                    : $"su 3 {MapMonsterId} 1 {Target} 0 {Monster.BasicCooldown} 11 {Monster.BasicSkill} 0 0 {(targetSession.Character.Hp > 0 ? 1 : 0)} {(int)(targetSession.Character.Hp / targetSession.Character.HPLoad() * 100)} {damage} {hitmode} 0");

                LastSkill = DateTime.Now;
                if (targetSession.Character.Hp <= 0)
                {
                    RemoveTarget();
                    Observable.Timer(TimeSpan.FromMilliseconds(1000)).Subscribe(o => { ServerManager.Instance.AskRevive(targetSession.Character.CharacterId); });
                }
            }
            if (npcMonsterSkill != null && (npcMonsterSkill.Skill.Range > 0 || npcMonsterSkill.Skill.TargetRange > 0))
            {
                foreach (Character characterInRange in MapInstance.GetCharactersInRange(npcMonsterSkill.Skill.TargetRange == 0 ? MapX : targetSession.Character.PositionX, npcMonsterSkill.Skill.TargetRange == 0 ? MapY : targetSession.Character.PositionY, npcMonsterSkill.Skill.TargetRange).Where(s => s.CharacterId != Target && s.Hp > 0 && !s.InvisibleGm))
                {
                    if (characterInRange.IsSitting)
                    {
                        characterInRange.IsSitting = false;
                        MapInstance.Broadcast(characterInRange.GenerateRest());
                    }
                    if (characterInRange.HasGodMode)
                    {
                        damage = 0;
                        hitmode = 1;
                    }
                    if (characterInRange.Hp > 0)
                    {
                        characterInRange.GetDamage(damage);
                        MapInstance.Broadcast(null, characterInRange.GenerateStat(), ReceiverType.OnlySomeone, "", characterInRange.CharacterId);
                        MapInstance.Broadcast($"su 3 {MapMonsterId} 1 {characterInRange.CharacterId} 0 {Monster.BasicCooldown} 11 {Monster.BasicSkill} 0 0 {(characterInRange.Hp > 0 ? 1 : 0)} { (int)(characterInRange.Hp / characterInRange.HPLoad() * 100) } {damage} {hitmode} 0");
                        if (characterInRange.Hp <= 0)
                        {
                            RemoveTarget();
                            Observable.Timer(TimeSpan.FromMilliseconds(1000)).Subscribe(o => { ServerManager.Instance.AskRevive(characterInRange.CharacterId); });
                        }
                    }
                }
            }
        }

        #endregion
    }
}