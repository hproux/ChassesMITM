// Decompiled with JetBrains decompiler
// Type: AmaknaCore.Sniffer.Message.MessageEntry
// Assembly: AmaknaCore.Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CA1535F7-60A2-442D-94BC-9B222E991CC0
// Assembly location: C:\Users\hugop\Desktop\Logiciels\Dofus\AmaknaCore Sniffer\AmaknaCore.Sniffer.exe

using AmaknaCore.AppData.Network;

namespace AmaknaCore.Sniffer.Message
{
  public class MessageEntry
  {
    public NetworkMessage Message;
    public uint Id;
    public string Name;
    public MessageOriginEnum Origin;
    public MessagePart Data;

    public MessageEntry(NetworkMessage msg, MessageOriginEnum origin, MessagePart data)
    {
      this.Message = msg;
      this.Origin = origin;
      this.Id = msg.MessageId;
      this.Name = msg.ToString();
      this.Data = data;
    }

    public new string ToString()
    {
      return string.Format("ID={0}; NAME={1}; ORIGIN={2};", (object) this.Id.ToString(), (object) this.Message, (object) this.Origin.ToString());
    }
  }
}
