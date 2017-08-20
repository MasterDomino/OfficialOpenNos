﻿using OpenNos.Core;
using OpenNos.Domain;
using OpenNos.GameObject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenNos.GameObject.Event.GAMES
{
    public static class MeteoriteGame
    {
        #region Methods

        public static void GenerateMeteoriteGame()
        {
            ServerManager.Instance.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("METEORITE_MINUTES"), 5), 0));
            ServerManager.Instance.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("METEORITE_MINUTES"), 5), 1));
            Thread.Sleep(4 * 60 * 1000);
            ServerManager.Instance.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("METEORITE_MINUTES"), 1), 0));
            ServerManager.Instance.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("METEORITE_MINUTES"), 1), 1));
            Thread.Sleep(30 * 1000);
            ServerManager.Instance.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("METEORITE_SECONDS"), 30), 0));
            ServerManager.Instance.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("METEORITE_SECONDS"), 30), 1));
            Thread.Sleep(20 * 1000);
            ServerManager.Instance.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("METEORITE_SECONDS"), 10), 0));
            ServerManager.Instance.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("METEORITE_SECONDS"), 10), 1));
            Thread.Sleep(10 * 1000);
            ServerManager.Instance.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("METEORITE_STARTED"), 1));
            ServerManager.Instance.Broadcast($"qnaml 100 #guri^506 The Meteorite Game is starting! Join now!");
            ServerManager.Instance.EventInWaiting = true;
            Thread.Sleep(30 * 1000);
            ServerManager.Instance.Sessions.Where(s => s.Character?.IsWaitingForEvent == false).ToList().ForEach(s => s.SendPacket("esf"));
            ServerManager.Instance.EventInWaiting = false;
            IEnumerable<ClientSession> sessions = ServerManager.Instance.Sessions.Where(s => s.Character?.IsWaitingForEvent == true && s.Character.MapInstance.MapInstanceType == MapInstanceType.BaseMapInstance);

            MapInstance map = null;
            map = ServerManager.Instance.GenerateMapInstance(2004, MapInstanceType.NormalInstance, new InstanceBag());

            foreach (ClientSession sess in sessions)
            {
                ServerManager.Instance.TeleportOnRandomPlaceInMap(sess, map.MapInstanceId);
            }

            ServerManager.Instance.Sessions.Where(s => s.Character != null).ToList().ForEach(s => s.Character.IsWaitingForEvent = false);
            ServerManager.Instance.StartedEvents.Remove(EventType.MeteoriteGame);

            MeteoriteGameThread task = new MeteoriteGameThread();
            Observable.Timer(TimeSpan.FromSeconds(10)).Subscribe(X => task.Run(map));

            #endregion
        }

        public class MeteoriteGameThread
        {
            MapInstance _map;

            public void Run(MapInstance map)
            {
                _map = map;
                int i = 0;
                while (_map?.Sessions?.Any() == true)
                {
                    RunRound(i++);
                }

                //ended
            }

            void RunRound(int number)
            {
                int amount = 120 + (60 * number);

                int i = amount;
                while (i != 0)
                {
                    SpawnCircle(number);
                    Thread.Sleep(60000 / amount);
                    i--;
                }
                Thread.Sleep(5000);
                _map.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(String.Format(Language.Instance.GetMessageFromKey("METEORITE_ROUND"), number + 1), 0));
                Thread.Sleep(5000);

                // Your dropped reward
                //_map.DropItems(GenerateDrop(_map.Map, 1046, 20, 20000 * (number + 1)).ToList());
                //_map.DropItems(GenerateDrop(_map.Map, 1030, 20, 5 * (number + 1)).ToList());
                //_map.DropItems(GenerateDrop(_map.Map, 2282, 20, 3 * (number + 1)).ToList());
                //_map.DropItems(GenerateDrop(_map.Map, 2514, 5, 1 * (number + 1)).ToList());
                //_map.DropItems(GenerateDrop(_map.Map, 2515, 5, 1 * (number + 1)).ToList());
                //_map.DropItems(GenerateDrop(_map.Map, 2516, 5, 1 * (number + 1)).ToList());
                //_map.DropItems(GenerateDrop(_map.Map, 2517, 5, 1 * (number + 1)).ToList());
                //_map.DropItems(GenerateDrop(_map.Map, 2518, 5, 1 * (number + 1)).ToList());
                //_map.DropItems(GenerateDrop(_map.Map, 2519, 5, 1 * (number + 1)).ToList());
                //_map.DropItems(GenerateDrop(_map.Map, 2520, 5, 1 * (number + 1)).ToList());
                //_map.DropItems(GenerateDrop(_map.Map, 2521, 5, 1 * (number + 1)).ToList());
                foreach (ClientSession session in _map.Sessions)
                {
                    // Your reward that every player should get
                }

                Thread.Sleep(30000);


            }

            IEnumerable<Tuple<short, int, short, short>> GenerateDrop(Map map, short vnum, int amountofdrop, int amount)
            {
                List<Tuple<short, int, short, short>> dropParameters = new List<Tuple<short, int, short, short>>();
                for (int i = 0; i < amountofdrop; i++)
                {
                    MapCell cell = map.GetRandomPosition();
                    dropParameters.Add(new Tuple<short, int, short, short>(vnum, amount, cell.X, cell.Y));
                }
                return dropParameters;
            }

            void SpawnCircle(int round)
            {
                if (_map != null)
                {
                    MapCell cell = _map.Map.GetRandomPosition();

                    int circleId = _map.GetNextMonsterId();

                    MapMonster circle = new MapMonster() { MonsterVNum = 2018, MapX = cell.X, MapY = cell.Y, MapMonsterId = circleId, IsHostile = false, IsMoving = false, ShouldRespawn = false };
                    circle.Initialize(_map);
                    circle.NoAggresiveIcon = true;
                    _map.AddMonster(circle);
                    _map.Broadcast(circle.GenerateIn());
                    _map.Broadcast($"eff 3 {circleId} 4660");

                    Observable.Timer(TimeSpan.FromSeconds(4)).Subscribe(observer =>
                    {
                        if (_map != null)
                        {
                            _map.Broadcast($"su 3 {circleId} 3 {circleId} 1220 220 0 4983 {cell.X} {cell.Y} 1 0 65535 0 0");
                            foreach (Character character in _map.GetCharactersInRange(cell.X, cell.Y, 2))
                            {
                                if (_map.Sessions.Count() < 4)
                                {
                                    // Your reward for the last three living players
                                }
                                character.GetDamage(655350);
                                Observable.Timer(TimeSpan.FromMilliseconds(1000)).Subscribe(o => ServerManager.Instance.AskRevive(character.CharacterId));
                            }
                            _map.RemoveMonster(circle);
                            _map.Broadcast(circle.GenerateOut());
                        }
                    });
                }
            }
        }
    }
}
