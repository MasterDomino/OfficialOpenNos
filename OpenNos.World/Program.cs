﻿/*
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

using log4net;
using OpenNos.Core;
using OpenNos.DAL;
using OpenNos.DAL.EF.Helpers;
using OpenNos.Data;
using OpenNos.GameObject;
using OpenNos.Handler;
using OpenNos.Master.Library.Client;
using OpenNos.Master.Library.Data;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace OpenNos.World
{
    public static class Program
    {
        #region Members

        private static readonly ManualResetEvent _run = new ManualResetEvent(true);

        private static EventHandler _exitHandler;

        private static bool _isDebug;

        private static bool _ignoreTelemetry;

        #endregion

        #region Delegates

        public delegate bool EventHandler(CtrlType sig);

        #endregion

        #region Enums

        public enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        #endregion

        #region Methods

        public static void Main(string[] args)
        {
#if DEBUG
            _isDebug = true;
            Thread.Sleep(1000);
#endif
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            Console.Title = $"OpenNos World Server{(_isDebug ? " Development Environment" : string.Empty)}";

            bool ignoreStartupMessages = false;
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["WorldPort"]);
            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "--nomsg":
                        ignoreStartupMessages = true;
                        break;

                    case "--notelemetry":
                        _ignoreTelemetry = true;
                        break;
                }
                if (arg.StartsWith("--port", StringComparison.CurrentCulture))
                {
                    int.TryParse(arg.Substring(7), out port);
                    Console.WriteLine(port);
                }
            }

            // initialize Logger
            Logger.InitializeLogger(LogManager.GetLogger(typeof(Program)));

            if (!ignoreStartupMessages)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                string text = $"WORLD SERVER v{fileVersionInfo.ProductVersion}dev - PORT : {port} by OpenNos Team";
                int offset = (Console.WindowWidth / 2) + (text.Length / 2);
                string separator = new string('=', Console.WindowWidth);
                Console.WriteLine(separator + string.Format("{0," + offset + "}\n", text) + separator);
            }

            // initialize api
            if (CommunicationServiceClient.Instance.Authenticate(ConfigurationManager.AppSettings["MasterAuthKey"]))
            {
                Logger.Info(Language.Instance.GetMessageFromKey("API_INITIALIZED"));
            }

            // initialize DB
            if (DataAccessHelper.Initialize())
            {
                // initialilize maps
                ServerManager.Instance.Initialize();
            }
            else
            {
                Console.ReadKey();
                return;
            }

            // TODO: initialize ClientLinkManager initialize PacketSerialization
            PacketFactory.Initialize<WalkPacket>();

            try
            {
                _exitHandler += ExitHandler;
                AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
                NativeMethods.SetConsoleCtrlHandler(_exitHandler, true);
            }
            catch (Exception ex)
            {
                Logger.Error("General Error", ex);
            }
            NetworkManager<WorldCryptography> networkManager = null;
            portloop:
            try
            {
                networkManager = new NetworkManager<WorldCryptography>(ConfigurationManager.AppSettings["IPAddress"], port, typeof(CommandPacketHandler), typeof(LoginCryptography), true);
            }
            catch (SocketException ex)
            {
                if (ex.ErrorCode == 10048)
                {
                    port++;
                    Logger.Info("Port already in use! Incrementing...");
                    goto portloop;
                }
                Logger.Error("General Error", ex);
                Environment.Exit(ex.ErrorCode);
            }

            throw new Exception("test");

            ServerManager.Instance.ServerGroup = ConfigurationManager.AppSettings["ServerGroup"];
            const int sessionLimit = 100; // Needs workaround
            int? newChannelId = CommunicationServiceClient.Instance.RegisterWorldServer(new SerializableWorldServer(ServerManager.Instance.WorldId, ConfigurationManager.AppSettings["IPAddress"], port, sessionLimit, ServerManager.Instance.ServerGroup));

            if (newChannelId.HasValue)
            {
                ServerManager.Instance.ChannelId = newChannelId.Value;

                MailServiceClient.Instance.Authenticate(ConfigurationManager.AppSettings["MasterAuthKey"], ServerManager.Instance.WorldId);
                ConfigurationServiceClient.Instance.Authenticate(ConfigurationManager.AppSettings["MasterAuthKey"], ServerManager.Instance.WorldId);
                ServerManager.Instance.Configuration = ConfigurationServiceClient.Instance.GetConfigurationObject();
                ServerManager.Instance.MallAPI = new GameObject.Helpers.MallAPIHelper(ServerManager.Instance.Configuration.MallBaseURL);
            }
            else
            {
                Logger.Error("Could not retrieve ChannelId from Web API.");
                Console.ReadKey();
            }
        }

        private static bool ExitHandler(CtrlType sig)
        {

            string serverGroup = ConfigurationManager.AppSettings["ServerGroup"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["WorldPort"]);
            CommunicationServiceClient.Instance.UnregisterWorldServer(ServerManager.Instance.WorldId);
            ServerManager.Shout(string.Format(Language.Instance.GetMessageFromKey("SHUTDOWN_SEC"), 5));
            ServerManager.Instance.SaveAll();

            Thread.Sleep(5000);
            return false;
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            ServerManager.Instance.InShutdown = true;
            Logger.Error((Exception)e.ExceptionObject);

            try
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer
                {
                    SoundLocation = "exception.wav"
                };
                player.PlaySync();
                if (!_ignoreTelemetry)
                {
                    string guid = ((GuidAttribute)Assembly.GetAssembly(typeof(SCS.Communication.ScsServices.Service.ScsServiceBuilder)).GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value;
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection
                    {
                        { "key", guid },
                        { "error", ((Exception)e.ExceptionObject).ToString() },
                        { "debug", _isDebug.ToString() }
                    };
                    WebClient wc = new WebClient();
                    byte[] responsebytes = wc.UploadValues("https://mgmt.opennos.io/Crash/ReportCrash", "POST", reqparm);
                    string[] response = Encoding.UTF8.GetString(responsebytes).Split(':');
                    if (response[0] != "saved")
                    {
                        Logger.Error(new Exception($"Unable to report crash to management Server. Please report this issue to the Developer: {response[0]}"));
                    }
                    wc.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            Logger.Debug("Server crashed! Rebooting gracefully...");
            string serverGroup = ConfigurationManager.AppSettings["ServerGroup"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["WorldPort"]);
            CommunicationServiceClient.Instance.UnregisterWorldServer(ServerManager.Instance.WorldId);
            ServerManager.Shout(string.Format(Language.Instance.GetMessageFromKey("SHUTDOWN_SEC"), 5));
            ServerManager.Instance.SaveAll();

            Process.Start("OpenNos.World.exe", "--nomsg");
            Environment.Exit(1);
        }

        #endregion

        #region Classes

        public static class NativeMethods
        {
            #region Methods

            [DllImport("Kernel32")]
            internal static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

            #endregion
        }

        #endregion
    }
}