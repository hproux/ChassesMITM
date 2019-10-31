// Decompiled with JetBrains decompiler
// Type: AmaknaCore.Sniffer.Managers.ServersManager
// Assembly: AmaknaCore.Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CA1535F7-60A2-442D-94BC-9B222E991CC0
// Assembly location: C:\Users\hugop\Desktop\Logiciels\Dofus\AmaknaCore Sniffer\AmaknaCore.Sniffer.exe

using AmaknaCore.Sniffer.Client;
using AmaknaCore.Sniffer.Network.Server;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace AmaknaCore.Sniffer.Managers
{
  public class ServersManager
  {
    private static List<SyncClient> ActiveClients = new List<SyncClient>();
    private static SimpleServer GameServer = (SimpleServer) null;
    private static SimpleServer LoginServer = (SimpleServer) null;
    private static bool Running = false;

    public static void StartAllServers()
    {
      if (ServersManager.Running)
        return;
      ServersManager.LoginServer = new SimpleServer();
      ServersManager.GameServer = new SimpleServer();
      ServersManager.LoginServer.ConnectionAccepted += new SimpleServer.ConnectionAcceptedDelegate(ServersManager.OnLoginConnectionAccepted);
      ServersManager.GameServer.ConnectionAccepted += new SimpleServer.ConnectionAcceptedDelegate(ServersManager.OnGameConnectionAccepted);
      ServersManager.LoginServer.Start(Configuration.LoginPort);
      ServersManager.GameServer.Start((short) Configuration.GamePort[0]);
      ServersManager.Running = true;
    }

    public static void StopAllServers()
    {
      try
      {
        if (!ServersManager.Running)
          return;
        foreach (SyncClient activeClient in ServersManager.ActiveClients)
          activeClient.StopSync(false);
        ServersManager.LoginServer.Stop();
        ServersManager.GameServer.Stop();
        ServersManager.LoginServer.ConnectionAccepted -= new SimpleServer.ConnectionAcceptedDelegate(ServersManager.OnLoginConnectionAccepted);
        ServersManager.GameServer.ConnectionAccepted -= new SimpleServer.ConnectionAcceptedDelegate(ServersManager.OnGameConnectionAccepted);
        ServersManager.LoginServer = (SimpleServer) null;
        ServersManager.GameServer = (SimpleServer) null;
        ServersManager.Running = false;
      }
      catch (Exception ex)
      {
      }
    }

    private static void OnGameConnectionAccepted(Socket sock)
    {
      IPEndPoint remoteEndPoint = sock.RemoteEndPoint as IPEndPoint;
      ConsoleManager.Logger.Info(string.Format("Client connected on Game Server ({0}:{1})", (object) remoteEndPoint.Address, (object) remoteEndPoint.Port));
      SyncClient syncClient = new SyncClient();
      syncClient.SyncStopped += new EventHandler<SyncClient.SyncStoppedEventArgs>(ServersManager.OnSyncClientStopped);
      ServersManager.ActiveClients.Add(syncClient);
      syncClient.StartSync(sock, true);
    }

    private static void OnLoginConnectionAccepted(Socket sock)
    {
      IPEndPoint remoteEndPoint = sock.RemoteEndPoint as IPEndPoint;
      ConsoleManager.Logger.Info(string.Format("Client connected on Login Server ({0}:{1})", (object) remoteEndPoint.Address, (object) remoteEndPoint.Port));
      SyncClient syncClient = new SyncClient();
      syncClient.SyncStopped += new EventHandler<SyncClient.SyncStoppedEventArgs>(ServersManager.OnSyncClientStopped);
      ServersManager.ActiveClients.Add(syncClient);
      syncClient.StartSync(sock, false);
    }

    private static void OnSyncClientStopped(object sender, SyncClient.SyncStoppedEventArgs e)
    {
      if (Program.AlreadyExit)
        return;
      try
      {
        ConsoleManager.Logger.Info(string.Format("Client disconnected from server ({0}:{1})", (object) e.Client.ClientIpEndPoint.Address, (object) e.Client.ClientIpEndPoint.Port));
        e.Client.SyncStopped -= new EventHandler<SyncClient.SyncStoppedEventArgs>(ServersManager.OnSyncClientStopped);
        ServersManager.ActiveClients.Remove(e.Client);
      }
      catch (Exception ex)
      {
      }
    }
  }
}
