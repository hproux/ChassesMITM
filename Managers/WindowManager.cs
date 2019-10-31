// Decompiled with JetBrains decompiler
// Type: AmaknaCore.Sniffer.Managers.WindowManager
// Assembly: AmaknaCore.Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CA1535F7-60A2-442D-94BC-9B222E991CC0
// Assembly location: C:\Users\hugop\Desktop\Logiciels\Dofus\AmaknaCore Sniffer\AmaknaCore.Sniffer.exe

using AmaknaCore.Sniffer.View;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace AmaknaCore.Sniffer.Managers
{
  public class WindowManager
  {
    public static MainForm Window;
    public static List<Form> ActiveChildrens;
    private static object CheckLock;

    public static void Initialize(MainForm window)
    {
      WindowManager.Window = window;
      WindowManager.ActiveChildrens = new List<Form>();
      WindowManager.CheckLock = new object();
    }

    public static void AddChildrenForm(Form children)
    {
      children.FormClosing += new FormClosingEventHandler(WindowManager.OnChildrenClosing);
      WindowManager.SetParent(children, WindowManager.Window);
      WindowManager.ShowFormInThread(children);
    }

    private static void ShowFormInThread(Form form)
    {
      new Thread((ThreadStart) (() => WindowManager.ProcessForm(form))).Start();
    }

    private static void ProcessForm(Form form)
    {
      lock (WindowManager.CheckLock)
        WindowManager.ActiveChildrens.Add(form);
      form.Show();
      Application.Run();
    }

    public static void Exit()
    {
      try
      {
        lock (WindowManager.CheckLock)
        {
          foreach (Form activeChildren in WindowManager.ActiveChildrens)
          {
            if (activeChildren != null)
              ((ChildrenForm) activeChildren).Exit();
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    public static void SetParent(Form children, MainForm parent)
    {
      if (parent.InvokeRequired)
        parent.Invoke((Delegate) new WindowManager.SetParentDelegate(WindowManager.SetParent), (object) children, (object) parent);
      else
        children.MdiParent = (Form) parent;
    }

    private static void OnChildrenClosing(object sender, FormClosingEventArgs e)
    {
      Form form = (Form) sender;
      form.FormClosing -= new FormClosingEventHandler(WindowManager.OnChildrenClosing);
      WindowManager.ActiveChildrens.Remove(form);
    }

    private delegate void SetParentDelegate(Form children, MainForm parent);
  }
}
