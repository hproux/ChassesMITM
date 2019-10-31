// Decompiled with JetBrains decompiler
// Type: AmaknaCore.Sniffer.Utilities.Logger.ConsoleLogger
// Assembly: AmaknaCore.Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CA1535F7-60A2-442D-94BC-9B222E991CC0
// Assembly location: C:\Users\hugop\Desktop\Logiciels\Dofus\AmaknaCore Sniffer\AmaknaCore.Sniffer.exe

using System;

namespace AmaknaCore.Sniffer.Utilities.Logger
{
  public static class ConsoleLogger
  {
    public static object Locker = new object();
    public static bool DebugMode = true;

    public static string GetFormattedDate
    {
      get
      {
        return DateTime.Now.ToString().Replace(":", ".").Replace("\\", ".").Replace("/", ".");
      }
    }

    public static void Append(string header, string message, ConsoleColor headcolor, bool line = true)
    {
      lock (ConsoleLogger.Locker)
      {
        if (line)
          Console.Write("\n");
        Console.ForegroundColor = headcolor;
        Console.Write(header);
        Console.Write(" ");
        Console.ForegroundColor = ConsoleColor.Gray;
        foreach (char ch in message)
        {
          if (ch == '@')
            Console.ForegroundColor = Console.ForegroundColor != ConsoleColor.Gray ? ConsoleColor.Gray : ConsoleColor.White;
          else
            Console.Write(ch);
        }
      }
    }

    public static void Infos(string message)
    {
      ConsoleLogger.Append("[Infos]", message, ConsoleColor.Green, true);
    }

    public static void Error(string message)
    {
      ConsoleLogger.Append("[Error]", message, ConsoleColor.Red, true);
    }

    public static void Debug(string message)
    {
      if (!ConsoleLogger.DebugMode)
        return;
      ConsoleLogger.Append("[Debug]", message, ConsoleColor.Magenta, true);
    }

    public static void Warning(string message)
    {
      ConsoleLogger.Append("[Warning]", message, ConsoleColor.Yellow, true);
    }

    public static void Script(string message)
    {
      ConsoleLogger.Append("[Script]", message, ConsoleColor.DarkGreen, true);
    }

    public static void Stage(string stage)
    {
      Console.Write("\n\n");
      Console.ForegroundColor = ConsoleColor.White;
      Console.Write("                 ================ ");
      Console.ForegroundColor = ConsoleColor.Gray;
      Console.Write(stage);
      Console.ForegroundColor = ConsoleColor.White;
      Console.Write(" ================ ");
      Console.Write("\n");
    }
  }
}
