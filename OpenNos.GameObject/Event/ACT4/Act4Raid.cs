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
using OpenNos.Domain;
using OpenNos.GameObject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace OpenNos.GameObject.Event
{
    public static class Act4Raid
    {

        #region Methods

        public static void GenerateRaid(MapInstanceType raidType, byte faction)
        {
            ServerManager.Instance.GetMapInstance(ServerManager.Instance.GetBaseMapInstanceIdByMapId((short)(129 + faction))).CreatePortal(new Portal()
            {
                SourceMapId = (short)(129 + faction),
                SourceX = 53,
                SourceY = 53,
                DestinationMapId = 0,
                DestinationX = 1,
                DestinationY = 1,
                Type = (short)(9 + faction)
            });

            Act4RaidThread raidThread = new Act4RaidThread();
            Observable.Timer(TimeSpan.FromMinutes(0)).Subscribe(X => raidThread.Run(raidType, faction));
        }

        #endregion
    }

    public class Act4RaidThread
    {
        List<long> wonFamilies = new List<long>();
        short mapId = 135;
        short bossMapId = 136;
        short bossVNum = 563;
        short bossX = 55;
        short bossY = 11;
        short sourcePortalX = 146;
        short sourcePortalY = 43;
        short destPortalX = 55;
        short destPortalY = 80;
        bool bossMove = false;
        MapInstanceType _raidType;
        byte _faction = 0;
        #region Methods

        public void Run(MapInstanceType raidType, byte faction)
        {
            _raidType = raidType;
            _faction = faction;
            switch (raidType)
            {
                // Morcos is default
                case MapInstanceType.Act4Hatus:
                    mapId = 137;
                    bossMapId = 138;
                    bossVNum = 577;
                    bossX = 36;
                    bossY = 18;
                    sourcePortalX = 37;
                    sourcePortalY = 156;
                    destPortalX = 36;
                    destPortalY = 58;
                    bossMove = false;
                    break;
                case MapInstanceType.Act4Calvina:
                    mapId = 139;
                    bossMapId = 140;
                    bossVNum = 629;
                    bossX = 26;
                    bossY = 26;
                    sourcePortalX = 194;
                    sourcePortalY = 17;
                    destPortalX = 9;
                    destPortalY = 41;
                    bossMove = true;
                    break;
                case MapInstanceType.Act4Berios:
                    mapId = 141;
                    bossMapId = 142;
                    bossVNum = 624;
                    bossX = 29;
                    bossY = 29;
                    sourcePortalX = 188;
                    sourcePortalY = 96;
                    destPortalX = 29;
                    destPortalY = 54;
                    bossMove = true;
                    break;
            }

            int raidTime = 3600;
            const int interval = 30;

            //Run once to load everything in place
            RefreshRaid(raidTime);

            ServerManager.Instance.Act4RaidStart = DateTime.Now;

            while (raidTime > 0)
            {
                raidTime -= interval;
                Thread.Sleep(interval * 1000);
                RefreshRaid(raidTime);
            }

            EndRaid();
        }

        private void EndRaid()
        {
            foreach (Family fam in ServerManager.Instance.FamilyList.GetAllItems())
            {
                if (fam.Act4Raid != null)
                {
                    EventHelper.Instance.RunEvent(new EventContainer(fam.Act4Raid, EventActionType.DISPOSEMAP, null));
                    fam.Act4Raid = null;
                }
                if (fam.Act4RaidBossMap != null)
                {
                    EventHelper.Instance.RunEvent(new EventContainer(fam.Act4RaidBossMap, EventActionType.DISPOSEMAP, null));
                    fam.Act4RaidBossMap = null;
                }
            }

            ServerManager.Instance.GetMapInstance(ServerManager.Instance.GetBaseMapInstanceIdByMapId(130)).Portals.RemoveAll(s => s.Type.Equals(10));
            ServerManager.Instance.GetMapInstance(ServerManager.Instance.GetBaseMapInstanceIdByMapId(131)).Portals.RemoveAll(s => s.Type.Equals(11));
            switch (_faction)
            {
                case 1:
                    ServerManager.Instance.Act4AngelStat.Mode = 0;
                    ServerManager.Instance.Act4AngelStat.IsMorcos = false;
                    ServerManager.Instance.Act4AngelStat.IsHatus = false;
                    ServerManager.Instance.Act4AngelStat.IsCalvina = false;
                    ServerManager.Instance.Act4AngelStat.IsBerios = false;
                    break;
                case 2:
                    ServerManager.Instance.Act4DemonStat.Mode = 0;
                    ServerManager.Instance.Act4DemonStat.IsMorcos = false;
                    ServerManager.Instance.Act4DemonStat.IsHatus = false;
                    ServerManager.Instance.Act4DemonStat.IsCalvina = false;
                    ServerManager.Instance.Act4DemonStat.IsBerios = false;
                    break;
            }

            ServerManager.Instance.StartedEvents.Remove(EventType.Act4Raid);
        }

        private void RefreshRaid(int remaining)
        {
            foreach (Family fam in ServerManager.Instance.FamilyList.GetAllItems())
            {
                if (fam.Act4Raid == null)
                {
                    fam.Act4Raid = ServerManager.Instance.GenerateMapInstance(mapId, _raidType, new InstanceBag());
                }
                if (fam.Act4RaidBossMap == null)
                {
                    fam.Act4RaidBossMap = ServerManager.Instance.GenerateMapInstance(bossMapId, _raidType, new InstanceBag());
                }
                if (remaining <= 1800 && !fam.Act4Raid.Portals.Any(s => s.DestinationMapInstanceId.Equals(fam.Act4RaidBossMap.MapInstanceId)))
                {
                    fam.Act4Raid.CreatePortal(new Portal()
                    {
                        DestinationMapInstanceId = fam.Act4RaidBossMap.MapInstanceId,
                        DestinationX = destPortalX,
                        DestinationY = destPortalY,
                        SourceX = sourcePortalX,
                        SourceY = sourcePortalY,

                    });
                    OpenRaid(fam);
                }

                int count = fam.Act4RaidBossMap.Sessions.Count();

                if (remaining < 1800 && count != 0)
                {
                    if (count > 5)
                    {
                        count = 5;
                    }
                    List<MonsterToSummon> mobWave = new List<MonsterToSummon>();
                    for (int i = 0; i < count; i++)
                    {
                        switch (_raidType)
                        {
                            case MapInstanceType.Act4Morcos:
                                mobWave.Add(new MonsterToSummon(561, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(561, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(561, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(562, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(562, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(562, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                break;
                            case MapInstanceType.Act4Hatus:
                                mobWave.Add(new MonsterToSummon(574, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(574, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(575, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(575, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(576, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(576, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                break;
                            case MapInstanceType.Act4Calvina:
                                mobWave.Add(new MonsterToSummon(770, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(770, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(770, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(771, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(771, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(771, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                break;
                            case MapInstanceType.Act4Berios:
                                mobWave.Add(new MonsterToSummon(780, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(781, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(782, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(782, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(783, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                mobWave.Add(new MonsterToSummon(783, fam.Act4RaidBossMap.Map.GetRandomPosition(), -1, true));
                                break;
                        }
                    }
                    fam.Act4RaidBossMap.SummonMonsters(mobWave);
                }
            }
        }

        private void OpenRaid(Family fami)
        {
            List<MonsterToSummon> summonParameters = new List<MonsterToSummon>();
            List<EventContainer> onDeathEvents = new List<EventContainer>();
            onDeathEvents.Add(new EventContainer(fami.Act4RaidBossMap, EventActionType.THROWITEMS, new Tuple<int, short, byte, int, int>(bossVNum, 1046, 10, 20000, 20001)));
            onDeathEvents.Add(new EventContainer(fami.Act4RaidBossMap, EventActionType.THROWITEMS, new Tuple<int, short, byte, int, int>(bossVNum, 1244, 10, 5, 6)));
            if (_raidType.Equals(MapInstanceType.Act4Berios))
            {
                onDeathEvents.Add(new EventContainer(fami.Act4RaidBossMap, EventActionType.THROWITEMS, new Tuple<int, short, byte, int, int>(bossVNum, 2395, 3, 1, 2)));
                onDeathEvents.Add(new EventContainer(fami.Act4RaidBossMap, EventActionType.THROWITEMS, new Tuple<int, short, byte, int, int>(bossVNum, 2396, 5, 1, 2)));
                onDeathEvents.Add(new EventContainer(fami.Act4RaidBossMap, EventActionType.THROWITEMS, new Tuple<int, short, byte, int, int>(bossVNum, 2397, 10, 1, 2)));
            }
            onDeathEvents.Add(new EventContainer(fami.Act4RaidBossMap, EventActionType.SCRIPTEND, (byte)1));
            MonsterToSummon bossMob = new MonsterToSummon(bossVNum, new MapCell() { X = bossX, Y = bossY }, -1, bossMove);
            bossMob.DeathEvents = onDeathEvents;
            summonParameters.Add(bossMob);
            EventHelper.Instance.RunEvent(new EventContainer(fami.Act4RaidBossMap, EventActionType.SPAWNMONSTERS, summonParameters));
            EventHelper.Instance.RunEvent(new EventContainer(fami.Act4Raid, EventActionType.SENDPACKET, UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("ACT4RAID_OPEN"), 0)));

            Observable.Timer(TimeSpan.FromSeconds(90)).Subscribe(o =>
            {
                //TODO: Summon Monsters
            });
        }

        #endregion
    }
}