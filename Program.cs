// Decompiled with JetBrains decompiler
// Type: AmaknaCore.Sniffer.Program
// Assembly: AmaknaCore.Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CA1535F7-60A2-442D-94BC-9B222E991CC0
// Assembly location: C:\Users\hugop\Desktop\Logiciels\Dofus\AmaknaCore Sniffer\AmaknaCore.Sniffer.exe

using AmaknaCore.AppData;
using AmaknaCore.Sniffer.Managers;
using AmaknaCore.Sniffer.View;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace AmaknaCore.Sniffer
{
  public static class Program
  {
    public static bool AlreadyExit;
    public static int StartTime;
    private static Process[] Processes;

    private static void Main()
    {
      Program.AlreadyExit = false;
      Control.CheckForIllegalCrossThreadCalls = false;
      Program.StartTime = Environment.TickCount;
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      ProtocolTypeManager.Initialize();
      MessageReceiver.Initialize();
      ServersManager.StartAllServers();
      Application.Run((Form) new MainForm());
    }

    public static void Exit()
    {
      if (Program.AlreadyExit)
        return;
      Program.AlreadyExit = true;
      ServersManager.StopAllServers();
      WindowManager.Exit();
      Application.Exit();
      Environment.Exit(1);
    }
  }
}
