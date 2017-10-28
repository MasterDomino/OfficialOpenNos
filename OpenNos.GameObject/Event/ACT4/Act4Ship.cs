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
    public class ACT4SHIP
    {
        #region Methods

        public static void GenerateAct4Ship(byte faction)
        {
            EventHelper.Instance.RunEvent(new EventContainer(ServerManager.Instance.GetMapInstance(ServerManager.Instance.GetBaseMapInstanceIdByMapId(145)), EventActionType.NPCSEFFECTCHANGESTATE, true));
            Act4ShipThread shipThread = new Act4ShipThread();
            DateTime now = DateTime.Now;
            DateTime result = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);

            result = result.AddMinutes(((now.Minute / 5) + 1) * 5);

            Observable.Timer(result - now).Subscribe(X => shipThread.Run(faction));
        }

        #endregion
    }

    public class Act4ShipThread
    {
        #region Methods

        public void Run(byte faction)
        {
            MapInstance map = ServerManager.Instance.GenerateMapInstance(149, faction == 1 ? MapInstanceType.Act4ShipAngel : MapInstanceType.Act4ShipDemon, null);
            while (true)
            {
                OpenShip();
                Thread.Sleep(60 * 1000);
                map.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("SHIP_MINUTES"), 4), 0));
                Thread.Sleep(60 * 1000);
                map.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("SHIP_MINUTES"), 3), 0));
                Thread.Sleep(60 * 1000);
                map.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("SHIP_MINUTES"), 2), 0));
                Thread.Sleep(60 * 1000);
                map.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("SHIP_MINUTE"), 0));
                LockShip();
                Thread.Sleep(30 * 1000);
                map.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("SHIP_SECONDS"), 30), 0));
                Thread.Sleep(20 * 1000);
                map.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(string.Format(Language.Instance.GetMessageFromKey("SHIP_SECONDS"), 10), 0));
                Thread.Sleep(10 * 1000);
                map.Broadcast(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("SHIP_SETOFF"), 0));
                List<ClientSession> sessions = map.Sessions.Where(s => s != null && s.Character != null).ToList();
                Observable.Timer(TimeSpan.FromSeconds(0)).Subscribe(X => TeleportPlayers(sessions));
            }
        }

        private void LockShip()
        {
            EventHelper.Instance.RunEvent(new EventContainer(ServerManager.Instance.GetMapInstance(ServerManager.Instance.GetBaseMapInstanceIdByMapId(145)), EventActionType.NPCSEFFECTCHANGESTATE, true));
        }

        private void TeleportPlayers(List<ClientSession> sessions)
        {
            foreach (ClientSession s in sessions)
            {
                switch (s.Character.Faction)
                {
                    case FactionType.None:
                        ServerManager.Instance.ChangeMap(s.Character.CharacterId, 145, 51, 41);
                        s.SendPacket(UserInterfaceHelper.Instance.GenerateInfo("You need to be part of a faction to join Act 4"));
                        return;
                    case FactionType.Angel:
                        s.Character.MapId = 130;
                        s.Character.MapX = 12;
                        s.Character.MapY = 40;
                        break;
                    case FactionType.Demon:
                        s.Character.MapId = 131;
                        s.Character.MapX = 12;
                        s.Character.MapY = 40;
                        break;
                }
                //todo: get act4 channel dynamically
                // Change IP to yours
                s.Character.ChangeChannel("127.0.0.1", 4003, 1);
            }
        }

        private void OpenShip()
        {
            EventHelper.Instance.RunEvent(new EventContainer(ServerManager.Instance.GetMapInstance(ServerManager.Instance.GetBaseMapInstanceIdByMapId(145)), EventActionType.NPCSEFFECTCHANGESTATE, false));
        }

        #endregion
    }
}