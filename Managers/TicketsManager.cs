// Decompiled with JetBrains decompiler
// Type: AmaknaCore.Sniffer.Managers.TicketsManager
// Assembly: AmaknaCore.Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CA1535F7-60A2-442D-94BC-9B222E991CC0
// Assembly location: C:\Users\hugop\Desktop\Logiciels\Dofus\AmaknaCore Sniffer\AmaknaCore.Sniffer.exe

using AmaknaCore.Sniffer.View;
using System.Collections.Generic;

namespace AmaknaCore.Sniffer.Managers
{
  public class TicketsManager
  {
    private static List<TicketEntry> Tickets = new List<TicketEntry>();

    public static void RegisterTicket(string address, ushort port, uint instance, UserForm window)
    {
      TicketsManager.Tickets.Add(new TicketEntry(address, port, instance, window));
    }

    public static TicketEntry GetTicket()
    {
      if (TicketsManager.Tickets.Count > 0)
        return TicketsManager.Tickets[TicketsManager.Tickets.Count - 1];
      return new TicketEntry("", (ushort) 0, 0U, (UserForm) null);
    }
  }
}
