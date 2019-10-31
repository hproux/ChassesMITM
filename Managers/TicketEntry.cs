// Decompiled with JetBrains decompiler
// Type: AmaknaCore.Sniffer.Managers.TicketEntry
// Assembly: AmaknaCore.Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CA1535F7-60A2-442D-94BC-9B222E991CC0
// Assembly location: C:\Users\hugop\Desktop\Logiciels\Dofus\AmaknaCore Sniffer\AmaknaCore.Sniffer.exe

using AmaknaCore.Sniffer.View;

namespace AmaknaCore.Sniffer.Managers
{
  public class TicketEntry
  {
    public string Address;
    public ushort Port;
    public uint Instance;
    public UserForm Window;

    public TicketEntry(string address, ushort port, uint instance, UserForm window)
    {
      this.Address = address;
      this.Port = port;
      this.Window = window;
      this.Instance = instance;
    }
  }
}
