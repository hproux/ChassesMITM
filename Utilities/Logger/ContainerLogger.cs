// Decompiled with JetBrains decompiler
// Type: AmaknaCore.Sniffer.Utilities.Logger.ContainerLogger
// Assembly: AmaknaCore.Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CA1535F7-60A2-442D-94BC-9B222E991CC0
// Assembly location: C:\Users\hugop\Desktop\Logiciels\Dofus\AmaknaCore Sniffer\AmaknaCore.Sniffer.exe

using AmaknaCore.AppData.Network;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AmaknaCore.Sniffer.Utilities.Logger
{
  public class ContainerLogger
  {
    private RichTextBox Container;
    private int LogCount;
    private bool IsConsole;

    public ContainerLogger(RichTextBox container, bool isConsole)
    {
      this.Container = container;
      this.LogCount = 0;
      this.IsConsole = isConsole;
    }

    public void Info(string Text)
    {
      if (this.IsConsole)
        this.Log(string.Format("[{0}] {1}\n", (object) "Info", (object) Text), Color.Green);
      else
        this.Log(string.Format("[{0}] ({1}) {2}\n", (object) DateTime.Now.ToString("HH:mm:ss"), (object) "Info", (object) Text), Color.Green);
    }

    public void Error(string Text)
    {
      if (this.IsConsole)
        this.Log(string.Format("[{0}] {1}\n", (object) "Erreur", (object) Text), Color.Red);
      else
        this.Log(string.Format("[{0}] ({1}) {2}\n", (object) DateTime.Now.ToString("HH:mm:ss"), (object) "Erreur", (object) Text), Color.Red);
    }

    public void Warning(string Text)
    {
      if (this.IsConsole)
        this.Log(string.Format("[{0}] {1}\n", (object) "Warning", (object) Text), Color.Orange);
      else
        this.Log(string.Format("[{0}] ({1}) {2}\n", (object) DateTime.Now.ToString("HH:mm:ss"), (object) "Warning", (object) Text), Color.Orange);
    }

    public void Debug(string Text)
    {
      if (this.IsConsole)
        this.Log(string.Format("[{0}] {1}\n", (object) "Debug", (object) Text), Color.Black);
      else
        this.Log(string.Format("[{0}] ({1}) {2}\n", (object) DateTime.Now.ToString("HH:mm:ss"), (object) "Debug", (object) Text), Color.Black);
    }

    public void Script(string Text)
    {
      if (this.IsConsole)
        this.Log(string.Format("[{0}] {1}\n", (object) DateTime.Now.ToString("HH:mm:ss"), (object) Text), Color.Peru);
      else
        this.Log(string.Format("[{0}] ({1}) {2}\n", (object) DateTime.Now.ToString("HH:mm:ss"), (object) "Script", (object) Text), Color.Peru);
    }

    public void Chat(string Text, string Prefix, Color Color)
    {
      if (Prefix == "")
        this.Log(string.Format("[{0}] {1}\n", (object) DateTime.Now.ToString("HH:mm:ss"), (object) Text), Color);
      else
        this.Log(string.Format("[{0}] ({1}) {2}\n", (object) DateTime.Now.ToString("HH:mm:ss"), (object) Prefix, (object) Text), Color);
    }

    public void Packet(NetworkMessage msg, bool received)
    {
      if (received)
        this.Log(string.Format("<{0}> ID={1}; NAME={2};\n", (object) "Received", (object) msg.MessageId, (object) msg), Color.Blue);
      else
        this.Log(string.Format("<{0}> ID={1}; NAME={2};\n", (object) "Send", (object) msg.MessageId, (object) msg), Color.Red);
    }

    private void Log(string Text, Color Color)
    {
      if (this.Container == null)
        return;
      if (this.LogCount > 2000)
        this.Clear();
      try
      {
        if (this.Container.InvokeRequired)
        {
          this.Container.Invoke((Delegate) new ContainerLogger.LogDelegate(this.Log), (object) Text, (object) Color);
        }
        else
        {
          this.Container.SelectionColor = Color;
          this.Container.AppendText(Text);
          this.Container.ScrollToCaret();
          ++this.LogCount;
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void Clear()
    {
      if (this.Container.InvokeRequired)
      {
        this.Container.Invoke((Delegate) new ContainerLogger.ClearDelegate(this.Clear));
      }
      else
      {
        this.Container.Clear();
        this.LogCount = 0;
      }
    }

    private delegate void LogDelegate(string Text, Color Color);

    private delegate void ClearDelegate();
  }
}
