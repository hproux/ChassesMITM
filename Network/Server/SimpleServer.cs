// Decompiled with JetBrains decompiler
// Type: AmaknaCore.Sniffer.Network.Server.SimpleServer
// Assembly: AmaknaCore.Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CA1535F7-60A2-442D-94BC-9B222E991CC0
// Assembly location: C:\Users\hugop\Desktop\Logiciels\Dofus\AmaknaCore Sniffer\AmaknaCore.Sniffer.exe

using AmaknaCore.Sniffer.Managers;
using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace AmaknaCore.Sniffer.Network.Server
{
  public class SimpleServer
  {
    private bool runing = false;
    private Socket socketListener;

    public SimpleServer()
    {
      this.socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public void Start(short listenPort)
    {
      this.runing = true;
      try
      {
        this.socketListener.Bind((EndPoint) new IPEndPoint(IPAddress.Any, (int) listenPort));
        this.socketListener.Listen(5);
        this.socketListener.BeginAccept(new AsyncCallback(this.BeiginAcceptCallBack), (object) this.socketListener);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(string.Format("Impossible d'écouter le port {0}.\nRedémarrez le programme une fois le port libéré.", (object) listenPort.ToString()), "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        ConsoleManager.Logger.Error(string.Format("Impossible d'écouter le port {0}.\nRedémarrez le programme une fois le port libéré.", (object) listenPort.ToString()));
      }
    }

    public void Stop()
    {
      try
      {
        this.runing = false;
        this.socketListener.Shutdown(SocketShutdown.Both);
      }
      catch (Exception ex)
      {
      }
    }

    private void BeiginAcceptCallBack(IAsyncResult result)
    {
      if (!this.runing)
        return;
      this.OnConnectionAccepted(((Socket) result.AsyncState).EndAccept(result));
      this.socketListener.BeginAccept(new AsyncCallback(this.BeiginAcceptCallBack), (object) this.socketListener);
    }

    public event SimpleServer.ConnectionAcceptedDelegate ConnectionAccepted;

    private void OnConnectionAccepted(Socket client)
    {
      if (this.ConnectionAccepted == null)
        return;
      this.ConnectionAccepted(client);
    }

    public delegate void ConnectionAcceptedDelegate(Socket acceptedSocket);
  }
}
