// Decompiled with JetBrains decompiler
// Type: AmaknaCore.Sniffer.Managers.ConsoleManager
// Assembly: AmaknaCore.Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CA1535F7-60A2-442D-94BC-9B222E991CC0
// Assembly location: C:\Users\hugop\Desktop\Logiciels\Dofus\AmaknaCore Sniffer\AmaknaCore.Sniffer.exe

using AmaknaCore.Sniffer.Utilities.Logger;
using AmaknaCore.Sniffer.View;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace AmaknaCore.Sniffer.Managers
{
  public class ConsoleManager
  {
    public static ConsoleForm Window = new ConsoleForm();
    public static ContainerLogger Logger = new ContainerLogger(ConsoleManager.Window.RichTextBox_Main, true);

    public static void HideConsole()
    {
      ConsoleManager.Window.Visible = false;
    }

    public static void ShowConsole()
    {
        if (WindowManager.ActiveChildrens.FirstOrDefault<Form>((Func<Form, bool>)(f => f.GetType() == typeof(ConsoleForm))) == null)
        {
            //WindowManager.AddChildrenForm((Form)ConsoleManager.Window);
            ConsoleManager.Window.Show();
        }
        else
            ConsoleManager.Window.Visible = true;
    }
  }
}
