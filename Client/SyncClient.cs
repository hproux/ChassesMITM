// Decompiled with JetBrains decompiler
// Type: AmaknaCore.Sniffer.Client.SyncClient
// Assembly: AmaknaCore.Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CA1535F7-60A2-442D-94BC-9B222E991CC0
// Assembly location: C:\Users\hugop\Desktop\Logiciels\Dofus\AmaknaCore Sniffer\AmaknaCore.Sniffer.exe

using AmaknaCore.AppData;
using AmaknaCore.AppData.IO;
using AmaknaCore.AppData.Messages.Connection;
using AmaknaCore.AppData.Messages.Game.Approach;
using AmaknaCore.AppData.Messages.Game.Character.Choice;
using AmaknaCore.AppData.Messages.Handshake;
using AmaknaCore.AppData.Messages.Security;
using AmaknaCore.AppData.Network;
using AmaknaCore.Sniffer.Managers;
using AmaknaCore.Sniffer.Network.Enums;
using AmaknaCore.Sniffer.View;
using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace AmaknaCore.Sniffer.Client
{
  public class SyncClient
  {
    public IPEndPoint ClientIpEndPoint;
    public UserForm Window;
    private SimpleClient Client;
    private SimpleClient Server;
    private bool Game;
    private string Lang;
    private string Ticket;
    private bool Register;
    private TicketEntry tick;
    private uint Instance;
    public byte[] RDM;

    public SyncClient()
    {
      this.Instance = 0U;
      this.Lang = "";
      this.Ticket = "";
      this.Register = false;
      this.tick = (TicketEntry) null;
    }

    public void StartSync(Socket client, bool game)
    {
      this.ClientIpEndPoint = client.RemoteEndPoint as IPEndPoint;
      this.Client = new SimpleClient();
      this.Server = new SimpleClient();
      this.Client.DataReceived += new EventHandler<SimpleClient.DataReceivedEventArgs>(this.OnClientDataReceived);
      this.Client.Disconnected += new EventHandler<SimpleClient.DisconnectedEventArgs>(this.OnClientDisconnected);
      this.Server.DataReceived += new EventHandler<SimpleClient.DataReceivedEventArgs>(this.OnServerDataReceived);
      this.Server.Disconnected += new EventHandler<SimpleClient.DisconnectedEventArgs>(this.OnServerDisconnected);
      this.Client.Start(client);
      if (!game)
      {
        this.Server.Start("63.34.214.78", (short) 5555);
      }
      else
      {
        this.Game = true;
        this.Send((NetworkMessage) new ProtocolRequired(1912, 1912), NetworkDestinationEnum.Client);
        this.Send((NetworkMessage) new HelloGameMessage(), NetworkDestinationEnum.Client);
      }
    }

    public void StopSync(bool silent = false)
    {
      if (this.Client != null)
      {
        this.Client.DataReceived -= new EventHandler<SimpleClient.DataReceivedEventArgs>(this.OnClientDataReceived);
        this.Client.Disconnected -= new EventHandler<SimpleClient.DisconnectedEventArgs>(this.OnClientDisconnected);
        if (this.Client.Runing)
          this.Client.Stop();
        this.Client = (SimpleClient) null;
      }
      if (this.Server != null)
      {
        this.Server.DataReceived -= new EventHandler<SimpleClient.DataReceivedEventArgs>(this.OnServerDataReceived);
        this.Server.Disconnected -= new EventHandler<SimpleClient.DisconnectedEventArgs>(this.OnServerDisconnected);
        if (this.Server.Runing)
          this.Server.Stop();
        this.Server = (SimpleClient) null;
      }
      if (silent)
        return;
      this.OnSyncStopped(new SyncClient.SyncStoppedEventArgs(this));
    }

    public void Send(NetworkMessage message, NetworkDestinationEnum destination)
    {
      ++this.Instance;
      BigEndianWriter bigEndianWriter = new BigEndianWriter();
      switch (destination)
      {
        case NetworkDestinationEnum.Client:
          message.Pack((IDataWriter) bigEndianWriter, 0U);
          this.Client.Send(bigEndianWriter.Data);
          break;
        case NetworkDestinationEnum.Server:
          message.Pack((IDataWriter) bigEndianWriter, this.Instance);
          this.Server.Send(bigEndianWriter.Data);
          break;
      }
    }

    public void Send(MessagePart part, NetworkDestinationEnum destination)
    {
      ++this.Instance;
      BigEndianWriter bigEndianWriter1 = new BigEndianWriter();
      byte num1 = part.Data.Length <= (int) ushort.MaxValue ? (part.Data.Length <= (int) byte.MaxValue ? ((uint) part.Data.Length <= 0U ? (byte) 0 : (byte) 1) : (byte) 2) : (byte) 3;
      BigEndianWriter bigEndianWriter2 = bigEndianWriter1;
      int? nullable1 = part.MessageId;
      int? nullable2 = nullable1.HasValue ? new int?(nullable1.GetValueOrDefault() << 2) : new int?();
      int num2 = (int) num1;
      int? nullable3;
      if (!nullable2.HasValue)
      {
        nullable1 = new int?();
        nullable3 = nullable1;
      }
      else
        nullable3 = new int?(nullable2.GetValueOrDefault() | num2);
      nullable1 = nullable3;
      int num3 = (int) (short) nullable1.Value;
      bigEndianWriter2.WriteShort((short) num3);
      if (destination == NetworkDestinationEnum.Server)
        bigEndianWriter1.WriteUInt(this.Instance);
      switch (num1)
      {
        case 1:
          bigEndianWriter1.WriteByte((byte) part.Data.Length);
          break;
        case 2:
          bigEndianWriter1.WriteShort((short) part.Data.Length);
          break;
        case 3:
          bigEndianWriter1.WriteByte((byte) (part.Data.Length >> 16 & (int) byte.MaxValue));
          bigEndianWriter1.WriteShort((short) (part.Data.Length & (int) ushort.MaxValue));
          break;
      }
      bigEndianWriter1.WriteBytes(part.Data);
      if (bigEndianWriter1.Data == null)
        return;
      try
      {
        switch (destination)
        {
          case NetworkDestinationEnum.Client:
            this.Client.Send(bigEndianWriter1.Data);
            break;
          case NetworkDestinationEnum.Server:
            this.Server.Send(bigEndianWriter1.Data);
            break;
        }
      }
      catch
      {
      }
    }

    private void OnClientDataReceived(object sender, SimpleClient.DataReceivedEventArgs e)
    {
      BigEndianReader bigEndianReader = new BigEndianReader(e.Data.Data);
      NetworkMessage msg = MessageReceiver.BuildMessage((uint) e.Data.MessageId.Value, (IDataReader) bigEndianReader);
      if (msg == null)
      {
        this.Send(e.Data, NetworkDestinationEnum.Server);
      }
      else
      {
        if (this.Game)
        {
          if (msg.MessageId == 110U)
          {
            AuthenticationTicketMessage authenticationTicketMessage = (AuthenticationTicketMessage) msg;
            TicketEntry ticket = TicketsManager.GetTicket();
            if (ticket.Address == "" || ticket.Port == (ushort) 0)
              return;
            this.Ticket = authenticationTicketMessage.Ticket;
            this.Lang = authenticationTicketMessage.Lang;
            this.Server.Start(ticket.Address, (short) ticket.Port);
            this.Instance = ticket.Instance;
            this.Window = ticket.Window;
            this.Window.UpdateClient(this);
          }
          else
            this.Send(e.Data, NetworkDestinationEnum.Server);
        }
        else
          this.Send(e.Data, NetworkDestinationEnum.Server);
        this.OnMessageSent(new SyncClient.MessageSentEventArgs(msg, e.Data));
      }
    }

    private void OnClientDisconnected(object sender, SimpleClient.DisconnectedEventArgs e)
    {
      if (this.Register)
        this.StopSync(true);
      else
        this.StopSync(false);
    }

    private void OnServerDataReceived(object sender, SimpleClient.DataReceivedEventArgs e)
    {
      BigEndianReader bigEndianReader = new BigEndianReader(e.Data.Data);
      NetworkMessage msg = MessageReceiver.BuildMessage((uint) e.Data.MessageId.Value, (IDataReader) bigEndianReader);
      if (msg == null)
      {
        this.Send(e.Data, NetworkDestinationEnum.Client);
      }
      else
      {
        switch (msg.MessageId)
        {
          case 1:
            if (!this.Game)
            {
              this.Send(e.Data, NetworkDestinationEnum.Client);
              this.Window = new UserForm(this);
              this.Window.Text = "Nouveau client";
              WindowManager.AddChildrenForm((Form) this.Window);
              break;
            }
            break;
          case 22:
            this.Window.Text = ((IdentificationSuccessMessage) msg).Login;
            this.Send(e.Data, NetworkDestinationEnum.Client);
            break;
          case 42:
            SelectedServerDataMessage serverDataMessage = (SelectedServerDataMessage) msg;
            TicketsManager.RegisterTicket(serverDataMessage.Address, (ushort) serverDataMessage.Ports[0], this.Instance, this.Window);
            this.Send((NetworkMessage) new SelectedServerDataMessage(serverDataMessage.ServerId, "127.0.0.1", Configuration.GamePort, serverDataMessage.CanCreateNewCharacter, serverDataMessage.Ticket), NetworkDestinationEnum.Client);
            this.Register = true;
            break;
          case 101:
            this.Send((NetworkMessage) new AuthenticationTicketMessage(this.Lang, this.Ticket), NetworkDestinationEnum.Server);
            break;
          case 153:
            CharacterSelectedSuccessMessage selectedSuccessMessage = (CharacterSelectedSuccessMessage) msg;
            UserForm window = this.Window;
            window.Text = window.Text + " (" + selectedSuccessMessage.Infos.Name + ")";
            this.Send(e.Data, NetworkDestinationEnum.Client);
            break;
          case 6253:
            this.RDM = ((RawDataMessage) msg).Content;
            this.Window.Button_SaveRDM.Enabled = true;
            this.Send(e.Data, NetworkDestinationEnum.Client);
            break;
          case 6469:
            SelectedServerDataExtendedMessage dataExtendedMessage = (SelectedServerDataExtendedMessage) msg;
            TicketsManager.RegisterTicket(dataExtendedMessage.Address, (ushort) dataExtendedMessage.Ports[0], this.Instance, this.Window);
            this.Send((NetworkMessage) new SelectedServerDataExtendedMessage(dataExtendedMessage.ServerId, "127.0.0.1", Configuration.GamePort, dataExtendedMessage.CanCreateNewCharacter, dataExtendedMessage.Ticket, dataExtendedMessage.Servers), NetworkDestinationEnum.Client);
            this.Register = true;
            break;
          default:
            this.Send(e.Data, NetworkDestinationEnum.Client);
            break;
        }
        this.OnMessageReceived(new SyncClient.MessageReceivedEventArgs(msg, e.Data));
      }
    }

    private void OnServerDisconnected(object sender, SimpleClient.DisconnectedEventArgs e)
    {
      if (this.Register)
        this.StopSync(true);
      else
        this.StopSync(false);
    }

    public event EventHandler<SyncClient.SyncStoppedEventArgs> SyncStopped;

    public event EventHandler<SyncClient.MessageReceivedEventArgs> MessageReceived_Event;

    public event EventHandler<SyncClient.MessageSentEventArgs> MessageSent_Event;

    private void OnSyncStopped(SyncClient.SyncStoppedEventArgs e)
    {
      if (this.SyncStopped == null)
        return;
      this.SyncStopped((object) this, e);
    }

    private void OnMessageReceived(SyncClient.MessageReceivedEventArgs e)
    {
      if (this.MessageReceived_Event == null)
        return;
      this.MessageReceived_Event((object) this, e);
    }

    private void OnMessageSent(SyncClient.MessageSentEventArgs e)
    {
      if (this.MessageSent_Event == null)
        return;
      this.MessageSent_Event((object) this, e);
    }

    public class SyncStoppedEventArgs : EventArgs
    {
      public SyncClient Client;

      public SyncStoppedEventArgs(SyncClient client)
      {
        this.Client = client;
      }
    }

    public class MessageReceivedEventArgs : EventArgs
    {
      public NetworkMessage Message;
      public MessagePart Data;

      public MessageReceivedEventArgs(NetworkMessage msg, MessagePart data)
      {
        this.Message = msg;
        this.Data = data;
      }
    }

    public class MessageSentEventArgs : EventArgs
    {
      public NetworkMessage Message;
      public MessagePart Data;

      public MessageSentEventArgs(NetworkMessage msg, MessagePart data)
      {
        this.Message = msg;
        this.Data = data;
      }
    }
  }
}
