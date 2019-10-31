// Decompiled with JetBrains decompiler
// Type: AmaknaCore.Sniffer.Configuration
// Assembly: AmaknaCore.Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CA1535F7-60A2-442D-94BC-9B222E991CC0
// Assembly location: C:\Users\hugop\Desktop\Logiciels\Dofus\AmaknaCore Sniffer\AmaknaCore.Sniffer.exe

using System.Reflection;

namespace AmaknaCore.Sniffer
{
  public static class Configuration
  {
    public static string DebugPath = Assembly.GetExecutingAssembly().Location.Replace(Assembly.GetExecutingAssembly().GetName().Name + ".exe", "");
    public static short LoginPort = 5555;
    public static int[] GamePort = new int[1]{ 786 };
    public static string GameVersion = "2.53.9.0";
  }
}
