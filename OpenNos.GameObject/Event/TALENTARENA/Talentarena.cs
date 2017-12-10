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
using OpenNos.DAL;
using OpenNos.Data;
using OpenNos.Domain;
using OpenNos.Master.Library.Client;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace OpenNos.GameObject.Event
{
    public static class TalentArena
    {

        public static bool IsRunning { get; set; }

        public static ThreadSafeSortedList<long, ClientSession> RegisteredParticipants { get; set; }

        public static ThreadSafeSortedList<long, Group> RegisteredGroups { get; set; }

        #region Methods

        public static void Run()
        {
            RegisteredParticipants = new ThreadSafeSortedList<long, ClientSession>();
            RegisteredGroups = new ThreadSafeSortedList<long, Group>();

            ServerManager.Shout(Language.Instance.GetMessageFromKey("TALENTARENA_OPEN"));

            // Create Matchmaking thread
            // Create other threads

            Observable.Timer(TimeSpan.FromMinutes(30)).Subscribe(observer =>
            {
                RegisteredParticipants.ClearAll();
                RegisteredGroups.ClearAll();
                IsRunning = false;
                ServerManager.Instance.StartedEvents.Remove(EventType.TALENTARENA);
            });
        }

        #endregion
    }
}