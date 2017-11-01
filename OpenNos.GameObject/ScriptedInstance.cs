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

using OpenNos.Data;
using OpenNos.Domain;
using OpenNos.GameObject.Helpers;
using OpenNos.XMLModel.Models.ScriptedInstance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace OpenNos.GameObject
{
    public class ScriptedInstance : ScriptedInstanceDTO
    {
        #region Members

        private readonly Dictionary<int, MapInstance> _mapInstanceDictionary = new Dictionary<int, MapInstance>();

        private IDisposable _disposable;

        #endregion

        #region Properties

        public List<Gift> DrawItems { get; set; }

        public MapInstance FirstMap { get; set; }

        public List<Gift> GiftItems { get; set; }

        public ScriptedInstanceModel Model { get; set; }

        public long Gold { get; set; }

        public byte Id { get; set; }

        public InstanceBag InstanceBag { get; } = new InstanceBag();

        public string Label { get; set; }

        public string Name { get; set; }

        public byte LevelMaximum { get; set; }

        public byte LevelMinimum { get; set; }

        public byte Lives { get; set; }

        public int MonsterAmount { get; internal set; }

        public int NpcAmount { get; internal set; }

        public int Reputation { get; set; }

        public List<Gift> RequiredItems { get; set; }

        public int RoomAmount { get; internal set; }

        public List<Gift> SpecialItems { get; set; }

        public short StartX { get; set; }

        public short StartY { get; set; }

        #endregion

        #region Methods

        public void Dispose()
        {
            Thread.Sleep(10000);
            _mapInstanceDictionary.Values.ToList().ForEach(m => m.Dispose());
        }

        public string GenerateMainInfo() => $"minfo 0 1 -1.0/0 -1.0/0 -1/0 -1.0/0 1 {InstanceBag.Lives + 1} 0";

        public List<string> GenerateMinimap()
        {
            List<string> lst = new List<string> { "rsfm 0 0 4 12" };
            _mapInstanceDictionary.Values.ToList().ForEach(s => lst.Add(s.GenerateRsfn(true)));
            return lst;
        }

        public string GenerateRbr()
        {
            string drawgift = string.Empty;
            string requireditem = string.Empty;
            string bonusitems = string.Empty;
            string specialitems = string.Empty;

            for (int i = 0; i < 5; i++)
            {
                Gift gift = DrawItems?.ElementAtOrDefault(i);
                drawgift += $" {(gift == null ? "-1.0" : $"{gift.VNum}.{gift.Amount}")}";
            }
            for (int i = 0; i < 2; i++)
            {
                Gift gift = SpecialItems?.ElementAtOrDefault(i);
                specialitems += $" {(gift == null ? "-1.0" : $"{gift.VNum}.{gift.Amount}")}";
            }

            for (int i = 0; i < 3; i++)
            {
                Gift gift = GiftItems?.ElementAtOrDefault(i);
                bonusitems += $"{(i == 0 ? string.Empty : " ")}{(gift == null ? "-1.0" : $"{gift.VNum}.{gift.Amount}")}";
            }
            const int WinnerScore = 0;
            const string Winner = "";
            return $"rbr 0.0.0 4 15 {LevelMinimum}.{LevelMaximum} {RequiredItems?.Sum(s => s.Amount)} {drawgift} {specialitems} {bonusitems} {WinnerScore}.{(WinnerScore > 0 ? Winner : string.Empty)} 0 0 {Name}\n{Label}";
        }

        public string GenerateWp() => $"wp {PositionX} {PositionY} {ScriptedInstanceId} 0 {LevelMinimum} {LevelMaximum}";

        public void LoadGlobals()
        {
            // initialize script as byte stream
            if (Script != null)
            {
                byte[] xml = Encoding.UTF8.GetBytes(Script);
                MemoryStream memoryStream = new MemoryStream(xml);
                XmlReader reader = XmlReader.Create(memoryStream);
                XmlSerializer serializer = new XmlSerializer(typeof(ScriptedInstanceModel));
                Model = (ScriptedInstanceModel)serializer.Deserialize(reader);
                memoryStream.Close();

                RequiredItems = new List<Gift>();
                DrawItems = new List<Gift>();
                SpecialItems = new List<Gift>();
                GiftItems = new List<Gift>();

                // set the values
                Id = Model.Globals.Id?.Value ?? 0;
                Gold = Model.Globals.Gold?.Value ?? 0;
                Reputation = Model.Globals.Reputation?.Value ?? 0;
                StartX = Model.Globals.StartX?.Value ?? 0;
                StartY = Model.Globals.StartY?.Value ?? 0;
                Lives = Model.Globals.Lives?.Value ?? 0;
                LevelMinimum = Model.Globals.LevelMinimum?.Value ?? 1;
                LevelMaximum = Model.Globals.LevelMaximum?.Value ?? 99;
                Name = Model.Globals.Name?.Value ?? "No Name";
                Label = Model.Globals.Label?.Value ?? "No Description";
                if (Model.Globals.RequiredItems != null)
                {
                    foreach (XMLModel.Objects.Item item in Model.Globals.RequiredItems)
                    {
                        RequiredItems.Add(new Gift(item.VNum, item.Amount, item.Design, item.IsRandomRare));
                    }
                }
                if (Model.Globals.DrawItems != null)
                {
                    foreach (XMLModel.Objects.Item item in Model.Globals.DrawItems)
                    {
                        DrawItems.Add(new Gift(item.VNum, item.Amount, item.Design, item.IsRandomRare));
                    }
                }
                if (Model.Globals.SpecialItems != null)
                {
                    foreach (XMLModel.Objects.Item item in Model.Globals.SpecialItems)
                    {
                        SpecialItems.Add(new Gift(item.VNum, item.Amount, item.Design, item.IsRandomRare));
                    }
                }
                if (Model.Globals.GiftItems != null)
                {
                    foreach (XMLModel.Objects.Item item in Model.Globals.GiftItems)
                    {
                        GiftItems.Add(new Gift(item.VNum, item.Amount, item.Design, item.IsRandomRare));
                    }
                }
            }
        }

        public void LoadScript(MapInstanceType mapinstancetype)
        {
            XmlDocument doc = new XmlDocument();
            if (Model != null)
            {
                InstanceBag.Lives = Lives;
                foreach (XMLModel.Objects.CreateMap createMap in Model.InstanceEvents.CreateMap)
                {
                    MapInstance mapInstance = ServerManager.Instance.GenerateMapInstance(createMap.VNum, mapinstancetype, new InstanceBag());
                    mapInstance.Portals?.Clear();
                    mapInstance.MapIndexX = createMap.IndexX;
                    mapInstance.MapIndexY = createMap.IndexY;
                    if (!_mapInstanceDictionary.ContainsKey(createMap.Map))
                    {
                        _mapInstanceDictionary.Add(createMap.Map, mapInstance);
                    }
                }

                FirstMap = _mapInstanceDictionary.Values.FirstOrDefault();
                Observable.Timer(TimeSpan.FromMinutes(3)).Subscribe(x =>
                {
                    if (!InstanceBag.Lock)
                    {
                        _mapInstanceDictionary.Values.ToList().ForEach(m => EventHelper.Instance.RunEvent(new EventContainer(m, EventActionType.SCRIPTEND, (byte)1)));
                        Dispose();
                    }
                });
                _disposable = Observable.Interval(TimeSpan.FromMilliseconds(100)).Subscribe(x =>
                {
                    if (InstanceBag.Lives - InstanceBag.DeadList.Count < 0)
                    {
                        _mapInstanceDictionary.Values.ToList().ForEach(m => EventHelper.Instance.RunEvent(new EventContainer(m, EventActionType.SCRIPTEND, (byte)3)));
                        Dispose();
                        _disposable.Dispose();
                    }
                    if (InstanceBag.Clock.DeciSecondRemaining <= 0)
                    {
                        _mapInstanceDictionary.Values.ToList().ForEach(m => EventHelper.Instance.RunEvent(new EventContainer(m, EventActionType.SCRIPTEND, (byte)1)));
                        Dispose();
                        _disposable.Dispose();
                    }
                });
                if (Script != null)
                {
                    doc.LoadXml(Script);
                    XmlNode InstanceEvents = doc.SelectSingleNode("Definition");
                    generateEvent(InstanceEvents, FirstMap);
                }
            }
        }

        private List<EventContainer> summonMonster(MapInstance mapInstance, XMLModel.Events.SummonMonster[] summonMonster)
        {
            List<EventContainer> evts = new List<EventContainer>();

            // SummonMonster
            foreach (XMLModel.Events.SummonMonster summon in summonMonster)
            {
                short positionX = summon.PositionX;
                short positionY = summon.PositionY;
                if (positionX == -1 || positionY == -1)
                {
                    MapCell cell = mapInstance?.Map?.GetRandomPosition();
                    if (cell != null)
                    {
                        positionX = cell.X;
                        positionY = cell.Y;
                    }
                }
                MonsterAmount++;
                MonsterToSummon monster = new MonsterToSummon(summon.VNum, new MapCell() { X = positionX, Y = positionY }, -1, summon.Move, summon.IsTarget, summon.IsBonus, summon.IsHostile, summon.IsBoss);

                // OnDeath
                if (summon.OnDeath != null)
                {
                    // RefreshRaidGoals
                    if (summon.OnDeath.RefreshRaidGoals != null)
                    {
                        monster.DeathEvents.Add(new EventContainer(mapInstance, EventActionType.REFRESHRAIDGOAL, null));
                    }

                    // RemoveButtonLocker
                    if (summon.OnDeath.RemoveButtonLocker != null)
                    {
                        monster.DeathEvents.Add(new EventContainer(mapInstance, EventActionType.REMOVEBUTTONLOCKER, null));
                    }

                    // RemoveMonsterLocker
                    if (summon.OnDeath.RemoveMonsterLocker != null)
                    {
                        monster.DeathEvents.Add(new EventContainer(mapInstance, EventActionType.REMOVEMONSTERLOCKER, null));
                    }

                    // ThrowItem
                    foreach (XMLModel.Events.ThrowItem throwItem in summon.OnDeath.ThrowItem)
                    {
                        monster.DeathEvents.Add(new EventContainer(mapInstance, EventActionType.THROWITEMS, new Tuple<int, short, byte, int, int>(-1, throwItem.VNum, throwItem.PackAmount == 0 ? (byte)1 : throwItem.PackAmount, throwItem.MinAmount == 0 ? 1 : throwItem.MinAmount, throwItem.MaxAmount == 0 ? 1 : throwItem.MaxAmount)));
                    }

                    // End
                    if (summon.OnDeath.End != null)
                    {
                        monster.DeathEvents.Add(new EventContainer(mapInstance, EventActionType.SCRIPTEND, summon.OnDeath.End.Type));
                    }
                }

                // OnNoticing
                if (summon.OnNoticing != null)
                {
                    // Effect
                    if (summon.OnNoticing.Effect != null)
                    {
                        monster.NoticingEvents.Add(new EventContainer(mapInstance, EventActionType.EFFECT, summon.OnNoticing.Effect.Value));
                    }

                    // Move
                    if (summon.OnNoticing.Move != null)
                    {
                        List<EventContainer> events = new List<EventContainer>();

                        // Effect
                        if (summon.OnNoticing.Move.Effect != null)
                        {
                            events.Add(new EventContainer(mapInstance, EventActionType.EFFECT, summon.OnNoticing.Move.Effect.Value));
                        }

                        // review OnTarget
                        //if (summon.OnNoticing.Move.OnTarget != null)
                        //{
                        //    summon.OnNoticing.Move.OnTarget.Move
                        //    foreach ()
                        //    //events.Add(new EventContainer(mapInstance, EventActionType.ONTARGET, summon.OnNoticing.Move.OnTarget.));
                        //}

                        monster.NoticingEvents.Add(new EventContainer(mapInstance, EventActionType.MOVE, new ZoneEvent() { X = summon.OnNoticing.Move.PositionX, Y = summon.OnNoticing.Move.PositionY, Events = events }));
                    }
                }

                evts.Add(new EventContainer(mapInstance, EventActionType.SPAWNMONSTER, monster));
            }

            return evts;
        }

        private List<EventContainer> onMapOnMove(MapInstance mapInstance, XMLModel.Events.OnMoveOnMap onMoveOnMap)
        {
            List<EventContainer> evts = new List<EventContainer>();

            // OnMoveOnMap
            if (onMoveOnMap != null)
            {
                List<EventContainer> onMoveOnMapEvents = new List<EventContainer>();
                List<EventContainer> waveEvent = new List<EventContainer>();

                // OnMapClean
                if (onMoveOnMap.OnMapClean != null)
                {
                    List<EventContainer> onMapCleanEvents = new List<EventContainer>();

                    // ChangePortalType
                    foreach (XMLModel.Events.ChangePortalType changePortalType in onMoveOnMap.OnMapClean.ChangePortalType)
                    {
                        onMapCleanEvents.Add(new EventContainer(mapInstance, EventActionType.CHANGEPORTALTYPE, new Tuple<int, PortalType>(changePortalType.IdOnMap, (PortalType)changePortalType.Type)));
                    }

                    // SendMessage
                    if (onMoveOnMap.OnMapClean.SendMessage != null)
                    {
                        onMapCleanEvents.Add(new EventContainer(mapInstance, EventActionType.SENDPACKET, UserInterfaceHelper.Instance.GenerateMsg(onMoveOnMap.OnMapClean.SendMessage.Value, onMoveOnMap.OnMapClean.SendMessage.Type)));
                    }

                    // SendPacket
                    if (onMoveOnMap.OnMapClean.SendPacket != null)
                    {
                        onMapCleanEvents.Add(new EventContainer(mapInstance, EventActionType.SENDPACKET, onMoveOnMap.OnMapClean.SendPacket.Value));
                    }

                    // RefreshMapItems
                    if (onMoveOnMap.OnMapClean.RefreshMapItems != null)
                    {
                        onMapCleanEvents.Add(new EventContainer(mapInstance, EventActionType.REFRESHMAPITEMS, null));
                    }

                    // NpcDialog
                    if (onMoveOnMap.OnMapClean.NpcDialog != null)
                    {
                        onMapCleanEvents.Add(new EventContainer(mapInstance, EventActionType.NPCDIALOG, onMoveOnMap.OnMapClean.NpcDialog.Value));
                    }

                    evts.Add(new EventContainer(mapInstance, EventActionType.REGISTEREVENT, new Tuple<string, List<EventContainer>>(nameof(XMLModel.Events.OnMapClean), onMapCleanEvents)));
                }

                // Wave
                foreach (XMLModel.Objects.Wave wave in onMoveOnMap.Wave)
                {
                    // SummonMonster
                    waveEvent.AddRange(summonMonster(mapInstance, wave.SummonMonster));

                    // SendMessage
                    if (wave.SendMessage != null)
                    {
                        waveEvent.Add(new EventContainer(mapInstance, EventActionType.SENDPACKET, UserInterfaceHelper.Instance.GenerateMsg(wave.SendMessage.Value, wave.SendMessage.Type)));
                    }

                    onMoveOnMapEvents.Add(new EventContainer(mapInstance, EventActionType.REGISTERWAVE, new EventWave(wave.Delay, waveEvent, wave.Offset)));
                }

                evts.Add(new EventContainer(mapInstance, EventActionType.REGISTEREVENT, new Tuple<string, List<EventContainer>>(nameof(XMLModel.Events.OnMoveOnMap), onMoveOnMapEvents)));
            }

            return evts;
        }

        private List<EventContainer> generateEventWIP(MapInstance parentMapInstance)
        {
            // Rewrite this shit soo it uses proper separate private methods for example onTraversalEvents way of doing things, we want to avoid loop calls
            List<EventContainer> evts = new List<EventContainer>();
            foreach (XMLModel.Objects.CreateMap createMap in Model.InstanceEvents.CreateMap)
            {
                MapInstance mapInstance = _mapInstanceDictionary.FirstOrDefault(s => s.Key == createMap.Map).Value ?? parentMapInstance;

                // OnMoveOnMap
                foreach (XMLModel.Events.OnMoveOnMap onMoveOnMap in createMap.OnMoveOnMap)
                {
                    evts.AddRange(onMapOnMove(mapInstance, onMoveOnMap));
                }

                // SummonMonster
                evts.AddRange(summonMonster(mapInstance, createMap.SummonMonster));

                // SpawnPortal
                foreach (XMLModel.Events.SpawnPortal portalEvent in createMap.SpawnPortal)
                {
                    MapInstance destinationMap = _mapInstanceDictionary.First(s => s.Key == portalEvent.ToMap).Value;
                    Portal portal = new Portal()
                    {
                        PortalId = portalEvent.IdOnMap,
                        SourceX = portalEvent.PositionX,
                        SourceY = portalEvent.PositionY,
                        Type = portalEvent.Type,
                        DestinationX = portalEvent.ToX,
                        DestinationY = portalEvent.ToY,
                        DestinationMapId = (short)(destinationMap.MapInstanceId == default ? -1 : 0),
                        SourceMapInstanceId = mapInstance.MapInstanceId,
                        DestinationMapInstanceId = destinationMap.MapInstanceId,
                    };
                    if (portalEvent.OnTraversal?.End != null)
                    {
                        portal.OnTraversalEvent = new EventContainer(mapInstance, EventActionType.SCRIPTEND, portalEvent.OnTraversal.End.Type);
                    }
                    evts.Add(new EventContainer(mapInstance, EventActionType.SPAWNPORTAL, portal));
                }

                // OnCharacterDiscoveringMap
                if (createMap.OnCharacterDiscoveringMap != null)
                {
                    List<EventContainer> onDiscoverEvents = new List<EventContainer>();

                    // NpcDialog
                    if (createMap.OnCharacterDiscoveringMap.NpcDialog != null)
                    {
                        onDiscoverEvents.Add(new EventContainer(mapInstance, EventActionType.NPCDIALOG, createMap.OnCharacterDiscoveringMap.NpcDialog.Value));
                    }

                    // OnMoveOnMap
                    if (createMap.OnCharacterDiscoveringMap.OnMoveOnMap != null)
                    {
                        onDiscoverEvents.AddRange(onMapOnMove(mapInstance, createMap.OnCharacterDiscoveringMap.OnMoveOnMap));
                    }

                    // SendPacket
                    if (createMap.OnCharacterDiscoveringMap.SendPacket != null)
                    {
                        onDiscoverEvents.Add(new EventContainer(mapInstance, EventActionType.SENDPACKET, createMap.OnCharacterDiscoveringMap.SendPacket.Value));
                    }

                    // SummonNpc
                    onDiscoverEvents.AddRange(summonNpc(mapInstance, createMap.OnCharacterDiscoveringMap.SummonNpc));

                    evts.Add(new EventContainer(mapInstance, EventActionType.REGISTEREVENT, new Tuple<string, List<EventContainer>>(nameof(XMLModel.Events.OnCharacterDiscoveringMap), onDiscoverEvents)));
                }

                // OnLockerOpen
                if (createMap.OnLockerOpen != null)
                {
                    evts.Add(new EventContainer(mapInstance, EventActionType.REGISTEREVENT, new Tuple<string, List<EventContainer>>(nameof(XMLModel.Events.OnLockerOpen), generateEventWIP(mapInstance))));
                }

                // OnAreaEntry
                foreach (XMLModel.Events.OnAreaEntry onAreaEntry in createMap.OnAreaEntry)
                {
                    evts.Add(new EventContainer(mapInstance, EventActionType.SETAREAENTRY, new ZoneEvent() { X = onAreaEntry.PositionX, Y = onAreaEntry.PositionY, Range = onAreaEntry.Range, Events = generateEventWIP(mapInstance) }));
                }

                // SetButtonLockers
                if (createMap.SetButtonLockers != null)
                {
                    evts.Add(new EventContainer(mapInstance, EventActionType.SETBUTTONLOCKERS, createMap.SetButtonLockers.Value));
                }

                // SetMonsterLockers
                if (createMap.SetMonsterLockers != null)
                {
                    evts.Add(new EventContainer(mapInstance, EventActionType.SETMONSTERLOCKERS, createMap.SetMonsterLockers.Value));
                }
            }
            return evts;
        }

        private List<EventContainer> summonNpc(MapInstance mapInstance, XMLModel.Events.SummonNpc[] summonNpc)
        {
            List<EventContainer> evts = new List<EventContainer>();

            foreach (XMLModel.Events.SummonNpc summon in summonNpc)
            {
                short positionX = summon.PositionX;
                short positionY = summon.PositionY;

                if (positionX == -1 || positionY == -1)
                {
                    MapCell cell = mapInstance?.Map?.GetRandomPosition();
                    if (cell != null)
                    {
                        positionX = cell.X;
                        positionY = cell.Y;
                    }
                }

                NpcAmount++;
                NpcToSummon npcToSummon = new NpcToSummon(summon.VNum, new MapCell() { X = positionX, Y = positionY }, -1, summon.IsMate, summon.IsProtected);

                // OnDeath
                if (summon.OnDeath != null)
                {
                    // RefreshRaidGoals
                    if (summon.OnDeath.RefreshRaidGoals != null)
                    {
                        npcToSummon.DeathEvents.Add(new EventContainer(mapInstance, EventActionType.REFRESHRAIDGOAL, null));
                    }

                    // RemoveButtonLocker
                    if (summon.OnDeath.RemoveButtonLocker != null)
                    {
                        npcToSummon.DeathEvents.Add(new EventContainer(mapInstance, EventActionType.REMOVEBUTTONLOCKER, null));
                    }

                    // RemoveMonsterLocker
                    if (summon.OnDeath.RemoveMonsterLocker != null)
                    {
                        npcToSummon.DeathEvents.Add(new EventContainer(mapInstance, EventActionType.REMOVEMONSTERLOCKER, null));
                    }

                    // ThrowItem
                    foreach (XMLModel.Events.ThrowItem throwItem in summon.OnDeath.ThrowItem)
                    {
                        npcToSummon.DeathEvents.Add(new EventContainer(mapInstance, EventActionType.THROWITEMS, new Tuple<int, short, byte, int, int>(-1, throwItem.VNum, throwItem.PackAmount == 0 ? (byte)1 : throwItem.PackAmount, throwItem.MinAmount == 0 ? 1 : throwItem.MinAmount, throwItem.MaxAmount == 0 ? 1 : throwItem.MaxAmount)));
                    }

                    // End
                    if (summon.OnDeath.End != null)
                    {
                        npcToSummon.DeathEvents.Add(new EventContainer(mapInstance, EventActionType.SCRIPTEND, summon.OnDeath.End.Type));
                    }
                }

                evts.Add(new EventContainer(mapInstance, EventActionType.SPAWNNPC, npcToSummon));
            }
            return evts;
        }

        private List<EventContainer> generateEvent(XmlNode node, MapInstance parentMapInstance)
        {
            List<EventContainer> evts = new List<EventContainer>();

            // IMPERFORMANT AS FUCK OPTIMIZE AS HELL!!!
            foreach (XmlNode mapEvent in node.ChildNodes)
            {
                if (mapEvent.Name == "#comment")
                {
                    continue;
                }
                int mapid = -1;
                short positionX = -1;
                short positionY = -1;
                short toY = -1;
                short toX = -1;
                int toMap = -1;
                Guid destmapInstanceId = default;
                if (!int.TryParse(mapEvent.Attributes["Map"]?.Value, out mapid))
                {
                    mapid = -1;
                }
                if (!short.TryParse(mapEvent.Attributes["PositionX"]?.Value, out positionX) || !short.TryParse(mapEvent.Attributes["PositionY"]?.Value, out positionY))
                {
                    positionX = -1;
                    positionY = -1;
                }
                if (int.TryParse(mapEvent.Attributes["ToMap"]?.Value, out toMap))
                {
                    MapInstance destmap = _mapInstanceDictionary.First(s => s.Key == toMap).Value;
                    if (!short.TryParse(mapEvent?.Attributes["ToY"]?.Value, out toY) || !short.TryParse(mapEvent?.Attributes["ToX"]?.Value, out toX))
                    {
                        if (destmap != null)
                        {
                            MapCell cell2 = destmap.Map.GetRandomPosition();
                            toY = cell2.Y;
                            toX = cell2.X;
                            destmapInstanceId = destmap.MapInstanceId;
                        }
                        else
                        {
                            toY = -1;
                            toX = -1;
                        }
                    }
                    else
                    {
                        destmapInstanceId = destmap.MapInstanceId;
                    }
                }
                bool.TryParse(mapEvent?.Attributes["IsTarget"]?.Value, out bool isTarget);
                bool.TryParse(mapEvent?.Attributes["IsBonus"]?.Value, out bool isBonus);
                bool.TryParse(mapEvent?.Attributes["IsBoss"]?.Value, out bool isBoss);
                bool.TryParse(mapEvent?.Attributes["IsProtected"]?.Value, out bool isProtected);
                bool.TryParse(mapEvent?.Attributes["IsMate"]?.Value, out bool isMate);
                if (!bool.TryParse(mapEvent?.Attributes["Move"]?.Value, out bool move))
                {
                    move = true;
                }
                if (!bool.TryParse(mapEvent?.Attributes["IsHostile"]?.Value, out bool isHostile))
                {
                    isHostile = true;
                }
                MapInstance mapInstance = _mapInstanceDictionary.FirstOrDefault(s => s.Key == mapid).Value ?? parentMapInstance;
                MapCell cell;
                switch (mapEvent.Name)
                {
                    //master events
                    case "CreateMap":
                    case "InstanceEvents":
                        generateEvent(mapEvent, mapInstance).ForEach(e => EventHelper.Instance.RunEvent(e));
                        break;

                    case "End":
                        _mapInstanceDictionary.Values.ToList().ForEach(m => evts.Add(new EventContainer(m, EventActionType.SCRIPTEND, byte.Parse(mapEvent?.Attributes["Type"].Value))));
                        break;

                    //register events
                    case "OnCharacterDiscoveringMap":
                    case "OnMoveOnMap":
                    case "OnMapClean":
                    case "OnLockerOpen":
                        evts.Add(new EventContainer(mapInstance, EventActionType.REGISTEREVENT, new Tuple<string, List<EventContainer>>(mapEvent.Name, generateEvent(mapEvent, mapInstance))));
                        break;

                    case "OnAreaEntry":
                        evts.Add(new EventContainer(mapInstance, EventActionType.SETAREAENTRY, new ZoneEvent() { X = positionX, Y = positionY, Range = byte.Parse(mapEvent?.Attributes["Range"]?.Value), Events = generateEvent(mapEvent, mapInstance) }));
                        break;

                    case "Wave":
                        byte.TryParse(mapEvent?.Attributes["Offset"]?.Value, out byte offSet);
                        evts.Add(new EventContainer(mapInstance, EventActionType.REGISTERWAVE, new EventWave(byte.Parse(mapEvent?.Attributes["Delay"]?.Value), generateEvent(mapEvent, mapInstance), offSet)));
                        break;

                    case "SetMonsterLockers":
                        evts.Add(new EventContainer(mapInstance, EventActionType.SETMONSTERLOCKERS, byte.Parse(mapEvent?.Attributes["Value"]?.Value)));
                        break;

                    case "SetButtonLockers":
                        evts.Add(new EventContainer(mapInstance, EventActionType.SETBUTTONLOCKERS, byte.Parse(mapEvent?.Attributes["Value"]?.Value)));
                        break;

                    case "ControlMonsterInRange":
                        short.TryParse(mapEvent?.Attributes["VNum"]?.Value, out short vNum);
                        evts.Add(new EventContainer(mapInstance, EventActionType.CONTROLEMONSTERINRANGE, new Tuple<short, byte, List<EventContainer>>(vNum, byte.Parse(mapEvent?.Attributes["Range"]?.Value), generateEvent(mapEvent, mapInstance))));
                        break;

                    //child events
                    case "OnDeath":
                        evts.AddRange(generateEvent(mapEvent, mapInstance));
                        break;

                    case "OnTarget":
                        evts.Add(new EventContainer(mapInstance, EventActionType.ONTARGET, generateEvent(mapEvent, mapInstance)));
                        break;

                    case "Effect":
                        evts.Add(new EventContainer(mapInstance, EventActionType.EFFECT, short.Parse(mapEvent?.Attributes["Value"].Value)));
                        break;

                    case "SummonMonsters":
                        MonsterAmount += short.Parse(mapEvent?.Attributes["Amount"].Value);
                        evts.Add(new EventContainer(mapInstance, EventActionType.SPAWNMONSTERS, mapInstance.Map.GenerateMonsters(short.Parse(mapEvent?.Attributes["VNum"].Value), short.Parse(mapEvent?.Attributes["Amount"].Value), move, new List<EventContainer>(), isBonus, isHostile, isBoss)));
                        break;

                    case "SummonMonster":
                        if (positionX == -1 || positionY == -1)
                        {
                            cell = mapInstance?.Map?.GetRandomPosition();
                            if (cell != null)
                            {
                                positionX = cell.X;
                                positionY = cell.Y;
                            }
                        }
                        MonsterAmount++;
                        List<EventContainer> notice = new List<EventContainer>();
                        List<EventContainer> death = new List<EventContainer>();
                        byte noticeRange = 0;
                        foreach (XmlNode eventNode in mapEvent.ChildNodes)
                        {
                            switch (eventNode.Name)
                            {
                                case "OnDeath":
                                    death.AddRange(generateEvent(eventNode, mapInstance));
                                    break;

                                case "OnNoticing":
                                    byte.TryParse(eventNode?.Attributes["Range"]?.Value, out noticeRange);
                                    notice.AddRange(generateEvent(eventNode, mapInstance));
                                    break;
                            }
                        }
                        MonsterToSummon monster = new MonsterToSummon(short.Parse(mapEvent?.Attributes["VNum"].Value), new MapCell() { X = positionX, Y = positionY }, -1, move, isTarget, isBonus, isHostile, isBoss)
                        {
                            DeathEvents = death,
                            NoticingEvents = notice,
                            NoticeRange = noticeRange
                        };
                        evts.Add(new EventContainer(mapInstance, EventActionType.SPAWNMONSTER, monster));
                        break;

                    case "SummonNpcs":
                        NpcAmount += short.Parse(mapEvent?.Attributes["Amount"].Value);
                        evts.Add(new EventContainer(mapInstance, EventActionType.SPAWNNPCS,
                            mapInstance.Map.GenerateNpcs(short.Parse(mapEvent?.Attributes["VNum"].Value),
                            short.Parse(mapEvent?.Attributes["Amount"].Value), new List<EventContainer>(), isMate, isProtected)));
                        break;

                    case "RefreshRaidGoals":
                        evts.Add(new EventContainer(mapInstance, EventActionType.REFRESHRAIDGOAL, null));
                        break;

                    case "Move":
                        List<EventContainer> moveEvents = new List<EventContainer>();
                        moveEvents.AddRange(generateEvent(mapEvent, mapInstance));
                        evts.Add(new EventContainer(mapInstance, EventActionType.MOVE, new ZoneEvent() { X = positionX, Y = positionY, Events = moveEvents }));
                        break;

                    case "SummonNpc":
                        if (positionX == -1 || positionY == -1)
                        {
                            cell = mapInstance?.Map?.GetRandomPosition();
                            if (cell != null)
                            {
                                positionX = cell.X;
                                positionY = cell.Y;
                            }
                        }
                        NpcAmount++;

                        List<EventContainer> onDeath = new List<EventContainer>();

                        foreach (XmlNode eventNode in mapEvent.ChildNodes)
                        {
                            if (eventNode.Name == "OnDeath")
                            {
                                onDeath.AddRange(generateEvent(eventNode, mapInstance));
                            }
                        }

                        evts.Add(new EventContainer(mapInstance, EventActionType.SPAWNNPC, new NpcToSummon(short.Parse(mapEvent?.Attributes["VNum"].Value), new MapCell() { X = positionX, Y = positionY }, -1, isMate, isProtected) { DeathEvents = onDeath }));
                        break;

                    case "SpawnButton":
                        if (positionX == -1 || positionY == -1)
                        {
                            cell = mapInstance?.Map?.GetRandomPosition();
                            if (cell != null)
                            {
                                positionX = cell.X;
                                positionY = cell.Y;
                            }
                        }
                        MapButton button = new MapButton(
                            int.Parse(mapEvent?.Attributes["Id"].Value), positionX, positionY,
                            short.Parse(mapEvent?.Attributes["VNumEnabled"].Value),
                            short.Parse(mapEvent?.Attributes["VNumDisabled"].Value), new List<EventContainer>(), new List<EventContainer>(), new List<EventContainer>());
                        foreach (XmlNode var in mapEvent.ChildNodes)
                        {
                            switch (var.Name)
                            {
                                case "OnFirstEnable":
                                    button.FirstEnableEvents.AddRange(generateEvent(var, mapInstance));
                                    break;

                                case "OnEnable":
                                    button.EnableEvents.AddRange(generateEvent(var, mapInstance));
                                    break;

                                case "OnDisable":
                                    button.DisableEvents.AddRange(generateEvent(var, mapInstance));
                                    break;
                            }
                        }
                        evts.Add(new EventContainer(mapInstance, EventActionType.SPAWNBUTTON, button));
                        break;

                    case "StopClock":
                        evts.Add(new EventContainer(mapInstance, EventActionType.STOPCLOCK, null));
                        break;

                    case "StopMapClock":
                        evts.Add(new EventContainer(mapInstance, EventActionType.STOPMAPCLOCK, null));
                        break;

                    case "RefreshMapItems":
                        evts.Add(new EventContainer(mapInstance, EventActionType.REFRESHMAPITEMS, null));
                        break;

                    case "RemoveMonsterLocker":
                        evts.Add(new EventContainer(mapInstance, EventActionType.REMOVEMONSTERLOCKER, null));
                        break;

                    case "ThrowItem":
                        short.TryParse(mapEvent?.Attributes["VNum"]?.Value, out short vnum2);
                        byte.TryParse(mapEvent?.Attributes["PackAmount"]?.Value, out byte packAmount);
                        int.TryParse(mapEvent?.Attributes["MinAmount"]?.Value, out int minAmount);
                        int.TryParse(mapEvent?.Attributes["MaxAmount"]?.Value, out int maxAmount);
                        evts.Add(new EventContainer(mapInstance, EventActionType.THROWITEMS, new Tuple<int, short, byte, int, int>(-1, vnum2, packAmount == 0 ? (byte)1 : packAmount, minAmount == 0 ? 1 : minAmount, maxAmount == 0 ? 1 : maxAmount)));
                        break;

                    case "RemoveButtonLocker":
                        evts.Add(new EventContainer(mapInstance, EventActionType.REMOVEBUTTONLOCKER, null));
                        break;

                    case "ChangePortalType":
                        evts.Add(new EventContainer(mapInstance, EventActionType.CHANGEPORTALTYPE,
                            new Tuple<int, PortalType>(int.Parse(mapEvent?.Attributes["IdOnMap"].Value), (PortalType)sbyte.Parse(mapEvent?.Attributes["Type"].Value))));
                        break;

                    case "SendPacket":
                        evts.Add(new EventContainer(mapInstance, EventActionType.SENDPACKET, mapEvent?.Attributes["Value"].Value));
                        break;

                    case "NpcDialog":
                        evts.Add(new EventContainer(mapInstance, EventActionType.NPCDIALOG, int.Parse(mapEvent?.Attributes["Value"].Value)));
                        break;

                    case "SendMessage":
                        evts.Add(new EventContainer(mapInstance, EventActionType.SENDPACKET, UserInterfaceHelper.Instance.GenerateMsg(mapEvent?.Attributes["Value"].Value, byte.Parse(mapEvent?.Attributes["Type"].Value))));
                        break;

                    case "GenerateClock":
                        evts.Add(new EventContainer(mapInstance, EventActionType.CLOCK, int.Parse(mapEvent?.Attributes["Value"].Value)));
                        break;

                    case "GenerateMapClock":
                        evts.Add(new EventContainer(mapInstance, EventActionType.MAPCLOCK, int.Parse(mapEvent?.Attributes["Value"].Value)));
                        break;

                    case "Teleport":
                        evts.Add(new EventContainer(mapInstance, EventActionType.TELEPORT, new Tuple<short, short, short, short>(short.Parse(mapEvent?.Attributes["PositionX"].Value), short.Parse(mapEvent?.Attributes["PositionY"].Value), short.Parse(mapEvent?.Attributes["DestinationX"].Value), short.Parse(mapEvent?.Attributes["DestinationY"].Value))));
                        break;

                    case "StartClock":
                        Tuple<List<EventContainer>, List<EventContainer>> eve = new Tuple<List<EventContainer>, List<EventContainer>>(new List<EventContainer>(), new List<EventContainer>());
                        foreach (XmlNode childEvent in mapEvent.ChildNodes)
                        {
                            switch (childEvent.Name)
                            {
                                case "OnTimeout":
                                    eve.Item1.AddRange(generateEvent(childEvent, mapInstance));
                                    break;

                                case "OnStop":
                                    eve.Item2.AddRange(generateEvent(childEvent, mapInstance));
                                    break;
                            }
                        }
                        evts.Add(new EventContainer(mapInstance, EventActionType.STARTCLOCK, eve));
                        break;

                    case "StartMapClock":
                        eve = new Tuple<List<EventContainer>, List<EventContainer>>(new List<EventContainer>(), new List<EventContainer>());
                        foreach (XmlNode childEvent in mapEvent.ChildNodes)
                        {
                            switch (childEvent.Name)
                            {
                                case "OnTimeout":
                                    eve.Item1.AddRange(generateEvent(childEvent, mapInstance));
                                    break;

                                case "OnStop":
                                    eve.Item2.AddRange(generateEvent(childEvent, mapInstance));
                                    break;
                            }
                        }
                        evts.Add(new EventContainer(mapInstance, EventActionType.STARTMAPCLOCK, eve));
                        break;

                    case "SpawnPortal":
                        Portal portal = new Portal()
                        {
                            PortalId = byte.Parse(mapEvent?.Attributes["IdOnMap"].Value),
                            SourceX = positionX,
                            SourceY = positionY,
                            Type = short.Parse(mapEvent?.Attributes["Type"].Value),
                            DestinationX = toX,
                            DestinationY = toY,
                            DestinationMapId = (short)(destmapInstanceId == default ? -1 : 0),
                            SourceMapInstanceId = mapInstance.MapInstanceId,
                            DestinationMapInstanceId = destmapInstanceId,
                        };
                        foreach (XmlNode childEvent in mapEvent.ChildNodes)
                        {
                            if (childEvent.Name == "OnTraversal")
                            {
                                portal.OnTraversalEvent = generateEvent(childEvent, mapInstance).FirstOrDefault();
                            }
                        }
                        evts.Add(new EventContainer(mapInstance, EventActionType.SPAWNPORTAL, portal));
                        break;
                }
            }
            return evts;
        }

        #endregion
    }
}