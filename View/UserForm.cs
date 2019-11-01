// Decompiled with JetBrains decompiler
// Type: AmaknaCore.Sniffer.View.UserForm
// Assembly: AmaknaCore.Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CA1535F7-60A2-442D-94BC-9B222E991CC0
// Assembly location: C:\Users\hugop\Desktop\Logiciels\Dofus\AmaknaCore Sniffer\AmaknaCore.Sniffer.exe

using AmaknaCore.Sniffer.Managers;
using AmaknaCore.AppData.Messages.Security;
using AmaknaCore.Sniffer.Client;
using AmaknaCore.Sniffer.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using QuickType;
using QuickTypePOI;
using QuickTypeIND;
using QuickTypeDM;
using QuickTypeCorresp;
using Newtonsoft.Json;

namespace AmaknaCore.Sniffer.View
{
  public class UserForm : Form, ChildrenForm
  {
    private IContainer components = (IContainer) null;
    private SyncClient Client;
    public List<MessageEntry> Messages;
    private object CheckLock;
    internal GroupBox GroupBox_Packets;
    public CheckBox CheckBox_Enable;
    public DataGridView DataGridView_PacketsList;
    private DataGridViewTextBoxColumn Column_Hour;
    private DataGridViewTextBoxColumn Column_Origin;
    private DataGridViewTextBoxColumn Column_ID;
    private DataGridViewTextBoxColumn Column_Name;
    internal Button Button_Clean;
    internal GroupBox GroupBox_Details;
    public TreeView TreeView_InfosPacket;
    private SplitContainer splitContainer1;
    private RichTextBox RichTextBox_Data;
    private Button Button_Save;
    public Button Button_SaveRDM;
    private Button Buton_SaveJSON;
    //Publics vars
    public List<MapPosition> ListeMaps;
    public List<PointOfInterest> ListePointOfInterest;
    public List<IndicesTxt> ListeIndices;
    public List<IndicesCorrespondance> ListeCorrespondance;
    public static string mapStartX = "";
    public static string mapStartY = "";
    public string indiceX = "";
    public string indiceY = "";
    public static string Direction="";
    public string Label = "";
    public string MapIdFlag = "";
    public static string indiceDistActu = "";
    public static int DernierFlag;
    public static bool CherchPhorreur = false;
    public static Point ptGauche = new Point();
    public static Point ptHaut = new Point();
    public static Point ptBas = new Point();
    public static Point ptDroite = new Point();
    public static Point ptPremierDrapeau = new Point();
    public static Point ptSecondDrapeau = new Point();
    public static int ecartDrapeau;
      public static List<string> phorreurMaps = new List<string>();
    public Dictionary<string, object> MapXYDictionary = new Dictionary<string, object>();

    public UserForm(SyncClient client)
    {
      this.Client = client;
      this.Messages = new List<MessageEntry>();
      this.CheckLock = new object();
      this.InitializeComponent();
      this.InitializeEvents();
    }

    public void UpdateClient(SyncClient client)
    {
      this.Client = client;
      this.InitializeEvents();
    }

    private void InitializeEvents()
    {
      this.Client.SyncStopped += new EventHandler<SyncClient.SyncStoppedEventArgs>(this.SynchroStopped);
      this.Client.MessageSent_Event += new EventHandler<SyncClient.MessageSentEventArgs>(this.MessageSent);
      this.Client.MessageReceived_Event += new EventHandler<SyncClient.MessageReceivedEventArgs>(this.MessageReceived);
    }

    private void RemoveEvents()
    {
      this.Client.SyncStopped -= new EventHandler<SyncClient.SyncStoppedEventArgs>(this.SynchroStopped);
      this.Client.MessageSent_Event -= new EventHandler<SyncClient.MessageSentEventArgs>(this.MessageSent);
      this.Client.MessageReceived_Event -= new EventHandler<SyncClient.MessageReceivedEventArgs>(this.MessageReceived);
    }

    private void AddMessage(MessageEntry msg)
    {
      try
      {
        if (this.Messages.Count >= 500)
          this.ClearMessages();
        lock (this.CheckLock)
        {
          this.Messages.Add(msg);
          int scrollingRowIndex = this.DataGridView_PacketsList.FirstDisplayedScrollingRowIndex;
          int num1 = this.DataGridView_PacketsList.DisplayedRowCount(true);
          int num2 = scrollingRowIndex + num1 - 1;
          int num3 = this.DataGridView_PacketsList.RowCount - 1;
          this.DataGridView_PacketsList.Rows.Add((object) DateTime.Now.ToString("HH:mm:ss"), (object) msg.Origin.ToString(), (object) msg.Id.ToString(), (object) msg.Name);
          foreach (DataGridViewCell cell in (BaseCollection) this.DataGridView_PacketsList.Rows[this.DataGridView_PacketsList.Rows.Count - 1].Cells)
          {
            switch (msg.Origin)
            {
              case MessageOriginEnum.Server:
                cell.Style.BackColor = Color.Salmon;
                break;
              case MessageOriginEnum.Client:
                cell.Style.BackColor = Color.DeepSkyBlue;
                break;
            }
          }
          if (num2 != num3)
            return;
          this.DataGridView_PacketsList.FirstDisplayedScrollingRowIndex = scrollingRowIndex + 1;
        }
      }
      catch (Exception ex)
      {
      }
    }

    public void ClearMessages()
    {
      lock (this.CheckLock)
      {
        this.Messages = new List<MessageEntry>();
        this.DataGridView_PacketsList.Rows.Clear();
      }
    }

    private void ShowProperties(object msg, TreeNodeCollection node)
    {
      if (msg == null || msg.GetType() == typeof (RawDataMessage))
        return;
      try
      {
        foreach (PropertyInfo property in msg.GetType().GetProperties())
        {
          string str1 = property.GetValue(msg).ToString();
          if (property.Name != "MessageId" && property.Name != "TypeId")
          {
            if (str1.Contains("."))
              str1 = str1.Substring(str1.LastIndexOf('.') + 1);
            node.Add(property.Name + " = " + str1);
          }
          string str2 = property.ToString();
          if (str2.Contains("[]"))
          {
            int num = 0;
            foreach (object msg1 in (IEnumerable) property.GetValue(msg))
            {
              this.ShowProperties(msg1, node[node.Count - 1].Nodes.Add(string.Format("Element {0} = {1}", (object) num.ToString(), msg1)).Nodes);
              ++num;
            }
          }
          else if (str2.Contains("AmaknaCore"))
            this.ShowProperties(property.GetValue(msg), node[node.Count - 1].Nodes);
        }
      }
      catch (Exception ex)
      {
      }
    }

    public void Exit()
    {
      Application.Exit();
    }

    private void Button_SaveRDM_Click(object sender, EventArgs e)
    {
      try
      {
        string str = Configuration.DebugPath + "\\RDM";
        if (!Directory.Exists(str))
          Directory.CreateDirectory(str);
        File.WriteAllBytes(Path.Combine(str, DateTime.Now.ToString((IFormatProvider) CultureInfo.GetCultureInfo("fr-CA")).Replace(":", "-")) + "_RDM.swf", this.Client.RDM);
        Process.Start(str);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void SynchroStopped(object sender, SyncClient.SyncStoppedEventArgs e)
    {
      this.Close();
    }

    private void UserForm_Closing(object sender, FormClosingEventArgs e)
    {
      this.RemoveEvents();
      this.Client.StopSync(false);
    }

    private void MessageSent(object sender, SyncClient.MessageSentEventArgs e)
    {
      if (!this.CheckBox_Enable.Checked)
        return;
      this.BeginInvoke((Delegate) new UserForm.AddMessageDelegate(this.AddMessage), (object) new MessageEntry(e.Message, MessageOriginEnum.Client, e.Data));
    }

    private void MessageReceived(object sender, SyncClient.MessageReceivedEventArgs e)
    {
      if (!this.CheckBox_Enable.Checked)
        return;
      this.BeginInvoke((Delegate) new UserForm.AddMessageDelegate(this.AddMessage), (object) new MessageEntry(e.Message, MessageOriginEnum.Server, e.Data));
    }

    private void DataGridView_PacketsList_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (this.DataGridView_PacketsList.SelectedRows.Count <= 0)
        return;
      try
      {
        this.TreeView_InfosPacket.Nodes.Clear();
        lock (this.CheckLock)
        {
          MessageEntry message = this.Messages[this.DataGridView_PacketsList.SelectedRows[0].Index];
          this.ShowProperties((object) message.Message, this.TreeView_InfosPacket.Nodes);
          this.RichTextBox_Data.Text = UserForm.ByteArrayToString(message.Data.Data);
        }
      }
      catch (Exception ex)
      {
      }
    }

    public static string ByteArrayToString(byte[] ba)
    {
      StringBuilder stringBuilder = new StringBuilder(ba.Length * 2);
      foreach (byte num in ba)
        stringBuilder.AppendFormat("{0:x2}", (object) num);
      return stringBuilder.ToString();
    }

    private void Button_Clean_Click(object sender, EventArgs e)
    {
      this.ClearMessages();
    }

    private void Button_Save_Click(object sender, EventArgs e)
    {
      new Thread(new ThreadStart(this.SaveMessages))
      {
        IsBackground = true,
        Name = "saveLogs",
        Priority = ThreadPriority.AboveNormal
      }.Start();
    }

    private void Buton_SaveJSON_Click(object sender, EventArgs e)
    {
      this.SaveMessages(this.Messages[this.DataGridView_PacketsList.SelectedRows[0].Index]);
    }

    private void SaveMessages()
    {
      try
      {
        DateTime now = DateTime.Now;
        Console.WriteLine(now.ToString((IFormatProvider) CultureInfo.GetCultureInfo("fr-CA")));
        string debugPath = Configuration.DebugPath;
        now = DateTime.Now;
        string str1 = now.ToString((IFormatProvider) CultureInfo.GetCultureInfo("fr-CA")).Replace(":", "-");
        StreamWriter text = File.CreateText(debugPath + str1 + "_Logs.txt");
        foreach (MessageEntry message in this.Messages)
        {
          text.WriteLine(string.Format("ID={0} Name={1} Origin={2}", (object) message.Id.ToString(), (object) message.Name, (object) message.Origin.ToString()));
          foreach (FieldInfo field in message.Message.GetType().GetFields())
          {
            string str2 = field.GetValue((object) message.Message).ToString();
            if (field.Name != "Id")
              text.WriteLine(string.Format("    {0} = {1}", (object) field.Name, (object) str2));
            if (str2.Contains("[]"))
            {
              int num = 0;
              foreach (object msg in (IEnumerable) field.GetValue((object) message.Message))
              {
                this.SaveObject(msg, text, 2);
                ++num;
               
              }
            }
            else if (!str2.Contains("AmaknaCore"))
              ;
          }
          text.WriteLine();
        }
        text.Close();
        Process.Start(Configuration.DebugPath);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void SaveMessages(MessageEntry entry)
    {
      try
      {
        DateTime now = DateTime.Now;
        Console.WriteLine(now.ToString((IFormatProvider) CultureInfo.GetCultureInfo("fr-CA")));
        string debugPath = Configuration.DebugPath;
        now = DateTime.Now;
        string str1 = now.ToString((IFormatProvider) CultureInfo.GetCultureInfo("fr-CA")).Replace(":", "-");
        StreamWriter text = File.CreateText(debugPath + str1 + "_Logs.txt");
        text.WriteLine(string.Format("ID={0} Name={1} Origin={2}", (object) entry.Id.ToString(), (object) entry.Name, (object) entry.Origin.ToString()));
        foreach (FieldInfo field in entry.Message.GetType().GetFields())
        {
          string str2 = field.GetValue((object) entry.Message).ToString();
          if (field.Name != "Id")
            text.WriteLine(string.Format("    {0} = {1}", (object) field.Name, (object) str2));
          if (str2.Contains("[]"))
          {
            int num = 0;
            foreach (object msg in (IEnumerable) field.GetValue((object) entry.Message))
            {
              this.SaveObject(msg, text, 2);
              ++num;
            }
          }
          else if (!str2.Contains("AmaknaCore"))
            ;
          text.WriteLine();
        }
        text.Close();
        Process.Start(Configuration.DebugPath);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void SaveObject(object msg, StreamWriter sw, int counter)
    {
      if (msg == null)
        return;
      string str1 = "";
      for (int index = 0; index < counter; ++index)
        str1 += "    ";
      try
      {
        sw.WriteLine(str1 + "[");
        foreach (FieldInfo field in msg.GetType().GetFields())
        {
          string str2 = field.GetValue(msg).ToString();
          if (field.Name != "Id" && field.Name != "MaxValue" && field.Name != "MinValue")
            sw.WriteLine(str1 + string.Format("{0} = {1}", (object) field.Name, (object) str2));
          if (str2.Contains("[]"))
          {
            int num = 0;
            foreach (object msg1 in (IEnumerable) field.GetValue(msg))
            {
              this.SaveObject(msg1, sw, counter + 1);
              ++num;
            }
          }
          else if (!str2.Contains("AmaknaCore"))
            ;
        }
        sw.WriteLine(str1 + "]");
      }
      catch (Exception ex)
      {
      }
    }

    private void UserForm_Load(object sender, EventArgs e)
    {
        using (StreamReader r = new StreamReader("MapPositions.json"))
        {
            string json = r.ReadToEnd();
            ListeMaps = JsonConvert.DeserializeObject<List<MapPosition>>(json);//Contient toutes les maps en fonction de l'id
        }

        using (StreamReader r = new StreamReader("PointOfInterest.json"))
        {
            string json = r.ReadToEnd();
            ListePointOfInterest = PointOfInterest.FromJson(json);//Contient tous les POI
        }

        using (StreamReader r = new StreamReader("indicesTxt.json"))
        {
            string json = r.ReadToEnd();
            ListeIndices = JsonConvert.DeserializeObject<List<IndicesTxt>>(json);//Contient tous les noms d'indices
        }

        using (StreamReader r = new StreamReader("DofusMapCorrespondances.json"))
        {
            string json = r.ReadToEnd();
            ListeCorrespondance = JsonConvert.DeserializeObject<List<IndicesCorrespondance>>(json);//Contient tous les noms d'indices
        }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.GroupBox_Packets = new System.Windows.Forms.GroupBox();
            this.Button_SaveRDM = new System.Windows.Forms.Button();
            this.Button_Save = new System.Windows.Forms.Button();
            this.CheckBox_Enable = new System.Windows.Forms.CheckBox();
            this.DataGridView_PacketsList = new System.Windows.Forms.DataGridView();
            this.Column_Hour = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Button_Clean = new System.Windows.Forms.Button();
            this.GroupBox_Details = new System.Windows.Forms.GroupBox();
            this.Buton_SaveJSON = new System.Windows.Forms.Button();
            this.RichTextBox_Data = new System.Windows.Forms.RichTextBox();
            this.TreeView_InfosPacket = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.GroupBox_Packets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_PacketsList)).BeginInit();
            this.GroupBox_Details.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox_Packets
            // 
            this.GroupBox_Packets.Controls.Add(this.Button_SaveRDM);
            this.GroupBox_Packets.Controls.Add(this.Button_Save);
            this.GroupBox_Packets.Controls.Add(this.CheckBox_Enable);
            this.GroupBox_Packets.Controls.Add(this.DataGridView_PacketsList);
            this.GroupBox_Packets.Controls.Add(this.Button_Clean);
            this.GroupBox_Packets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupBox_Packets.Location = new System.Drawing.Point(0, 0);
            this.GroupBox_Packets.Name = "GroupBox_Packets";
            this.GroupBox_Packets.Size = new System.Drawing.Size(402, 513);
            this.GroupBox_Packets.TabIndex = 3;
            this.GroupBox_Packets.TabStop = false;
            // 
            // Button_SaveRDM
            // 
            this.Button_SaveRDM.Enabled = false;
            this.Button_SaveRDM.Location = new System.Drawing.Point(252, 489);
            this.Button_SaveRDM.Name = "Button_SaveRDM";
            this.Button_SaveRDM.Size = new System.Drawing.Size(107, 23);
            this.Button_SaveRDM.TabIndex = 2;
            this.Button_SaveRDM.Text = "Sauvegarder RDM";
            this.Button_SaveRDM.UseVisualStyleBackColor = true;
            this.Button_SaveRDM.Visible = false;
            this.Button_SaveRDM.Click += new System.EventHandler(this.Button_SaveRDM_Click);
            // 
            // Button_Save
            // 
            this.Button_Save.Location = new System.Drawing.Point(105, 489);
            this.Button_Save.Name = "Button_Save";
            this.Button_Save.Size = new System.Drawing.Size(144, 23);
            this.Button_Save.TabIndex = 3;
            this.Button_Save.Text = "Sauvegarder les échanges";
            this.Button_Save.UseVisualStyleBackColor = true;
            this.Button_Save.Visible = false;
            this.Button_Save.Click += new System.EventHandler(this.Button_Save_Click);
            // 
            // CheckBox_Enable
            // 
            this.CheckBox_Enable.AutoSize = true;
            this.CheckBox_Enable.Checked = true;
            this.CheckBox_Enable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBox_Enable.Dock = System.Windows.Forms.DockStyle.Right;
            this.CheckBox_Enable.Location = new System.Drawing.Point(343, 486);
            this.CheckBox_Enable.Name = "CheckBox_Enable";
            this.CheckBox_Enable.Size = new System.Drawing.Size(56, 24);
            this.CheckBox_Enable.TabIndex = 2;
            this.CheckBox_Enable.Text = "Activé";
            this.CheckBox_Enable.UseVisualStyleBackColor = true;
            // 
            // DataGridView_PacketsList
            // 
            this.DataGridView_PacketsList.AllowUserToAddRows = false;
            this.DataGridView_PacketsList.AllowUserToDeleteRows = false;
            this.DataGridView_PacketsList.AllowUserToResizeColumns = false;
            this.DataGridView_PacketsList.AllowUserToResizeRows = false;
            this.DataGridView_PacketsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView_PacketsList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_Hour,
            this.Column_Origin,
            this.Column_ID,
            this.Column_Name});
            this.DataGridView_PacketsList.Dock = System.Windows.Forms.DockStyle.Top;
            this.DataGridView_PacketsList.Location = new System.Drawing.Point(3, 18);
            this.DataGridView_PacketsList.MultiSelect = false;
            this.DataGridView_PacketsList.Name = "DataGridView_PacketsList";
            this.DataGridView_PacketsList.ReadOnly = true;
            this.DataGridView_PacketsList.RowHeadersVisible = false;
            this.DataGridView_PacketsList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridView_PacketsList.Size = new System.Drawing.Size(396, 468);
            this.DataGridView_PacketsList.TabIndex = 0;
            this.DataGridView_PacketsList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_PacketsList_CellClick);
            this.DataGridView_PacketsList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.DataGridView_PacketsList_RowsAdded);
            // 
            // Column_Hour
            // 
            this.Column_Hour.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Column_Hour.HeaderText = "Heure";
            this.Column_Hour.MinimumWidth = 45;
            this.Column_Hour.Name = "Column_Hour";
            this.Column_Hour.ReadOnly = true;
            this.Column_Hour.Width = 45;
            // 
            // Column_Origin
            // 
            this.Column_Origin.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Column_Origin.HeaderText = "Origine";
            this.Column_Origin.MinimumWidth = 50;
            this.Column_Origin.Name = "Column_Origin";
            this.Column_Origin.ReadOnly = true;
            this.Column_Origin.Width = 50;
            // 
            // Column_ID
            // 
            this.Column_ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Column_ID.HeaderText = "ID";
            this.Column_ID.MinimumWidth = 25;
            this.Column_ID.Name = "Column_ID";
            this.Column_ID.ReadOnly = true;
            this.Column_ID.Width = 25;
            // 
            // Column_Name
            // 
            this.Column_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_Name.HeaderText = "Nom";
            this.Column_Name.Name = "Column_Name";
            this.Column_Name.ReadOnly = true;
            // 
            // Button_Clean
            // 
            this.Button_Clean.Location = new System.Drawing.Point(3, 487);
            this.Button_Clean.Margin = new System.Windows.Forms.Padding(0);
            this.Button_Clean.Name = "Button_Clean";
            this.Button_Clean.Size = new System.Drawing.Size(97, 24);
            this.Button_Clean.TabIndex = 1;
            this.Button_Clean.Text = "Vider l\'historique";
            this.Button_Clean.UseVisualStyleBackColor = true;
            this.Button_Clean.Click += new System.EventHandler(this.Button_Clean_Click);
            // 
            // GroupBox_Details
            // 
            this.GroupBox_Details.Controls.Add(this.Buton_SaveJSON);
            this.GroupBox_Details.Controls.Add(this.RichTextBox_Data);
            this.GroupBox_Details.Controls.Add(this.TreeView_InfosPacket);
            this.GroupBox_Details.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupBox_Details.Location = new System.Drawing.Point(0, 0);
            this.GroupBox_Details.Name = "GroupBox_Details";
            this.GroupBox_Details.Size = new System.Drawing.Size(438, 513);
            this.GroupBox_Details.TabIndex = 4;
            this.GroupBox_Details.TabStop = false;
            // 
            // Buton_SaveJSON
            // 
            this.Buton_SaveJSON.Location = new System.Drawing.Point(339, 18);
            this.Buton_SaveJSON.Name = "Buton_SaveJSON";
            this.Buton_SaveJSON.Size = new System.Drawing.Size(109, 23);
            this.Buton_SaveJSON.TabIndex = 2;
            this.Buton_SaveJSON.Text = "Sauvegarder JSON";
            this.Buton_SaveJSON.UseVisualStyleBackColor = true;
            this.Buton_SaveJSON.Visible = false;
            this.Buton_SaveJSON.Click += new System.EventHandler(this.Buton_SaveJSON_Click);
            // 
            // RichTextBox_Data
            // 
            this.RichTextBox_Data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTextBox_Data.Location = new System.Drawing.Point(3, 408);
            this.RichTextBox_Data.Name = "RichTextBox_Data";
            this.RichTextBox_Data.ReadOnly = true;
            this.RichTextBox_Data.Size = new System.Drawing.Size(432, 102);
            this.RichTextBox_Data.TabIndex = 1;
            this.RichTextBox_Data.Text = "";
            // 
            // TreeView_InfosPacket
            // 
            this.TreeView_InfosPacket.Dock = System.Windows.Forms.DockStyle.Top;
            this.TreeView_InfosPacket.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TreeView_InfosPacket.Location = new System.Drawing.Point(3, 18);
            this.TreeView_InfosPacket.Name = "TreeView_InfosPacket";
            this.TreeView_InfosPacket.Size = new System.Drawing.Size(432, 390);
            this.TreeView_InfosPacket.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.GroupBox_Packets);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.GroupBox_Details);
            this.splitContainer1.Size = new System.Drawing.Size(844, 513);
            this.splitContainer1.SplitterDistance = 402;
            this.splitContainer1.TabIndex = 5;
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 513);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserForm";
            this.Text = "UserForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserForm_Closing);
            this.Load += new System.EventHandler(this.UserForm_Load);
            this.GroupBox_Packets.ResumeLayout(false);
            this.GroupBox_Packets.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_PacketsList)).EndInit();
            this.GroupBox_Details.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    private delegate void AddMessageDelegate(MessageEntry msg);

    public Dictionary<string, object> ReturnXYMapId(string MapId)
    {
        var dictionary = new Dictionary<string, object>();
        foreach (QuickType.MapPosition map in ListeMaps)//Recuperation des coordonnées de la map de start
        {
            if (map.Id.ToString() == MapId)
            {
                string mapX = map.PosX.ToString();
                string mapY = map.PosY.ToString();
                dictionary.Add("X", mapX);
                dictionary.Add("Y", mapY);
            }
        }
        dictionary.Add("error", "map not found!");
        return dictionary;
    }

    public string ReturnNameId(string PoiId)
    {
        foreach(QuickTypePOI.PointOfInterest PoiCourant in ListePointOfInterest)
        {
            if (PoiCourant.Id.ToString() == PoiId)
            {
                string nameId = PoiCourant.NameId.ToString();
                return nameId;
            }
        }
        return "Id introuvable dans les POI";
    }

    public string ReturnIndiceTxt(string nameId)
    {

        foreach (QuickTypeIND.IndicesTxt IndiceCourant in ListeIndices)
        {
            if (IndiceCourant.Id.ToString() == nameId)
            {
                string IndiceTxt = IndiceCourant.Value.ToString();
                return IndiceTxt;
            }
        }
        return "nameId introuvable dans les Indices";
    }

    public string ReturnIndiceCorrespondace(string IndiceTxt)
    {
        string IndiceTxtNormalize = RemoveDiacritics(IndiceTxt);
        foreach (QuickTypeCorresp.IndicesCorrespondance IndiceCourant in ListeCorrespondance)
        {
            string IndCorantNormalize = RemoveDiacritics(IndiceCourant.Nom.ToString());
            if (IndCorantNormalize == IndiceTxtNormalize)
            {
                string LabelIdDM = IndiceCourant.Id.ToString();
                return LabelIdDM;
            }
        }
        return "Indice introuvable dans la correspondance avec DofusMap";
    }

    public static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    private void DataGridView_PacketsList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
        Int32 index = DataGridView_PacketsList.Rows.Count - 1;
        MessageEntry message = this.Messages[this.DataGridView_PacketsList.Rows[index].Index];
        
        uint id = message.Id;
        if (id == 6454)//Si un depacement de map est effectué a verrif
        {

        }
        if (id == 6486)//Si une trame de chasse est recue
        {
            object msg = message.Message;
           
            //Recuperation Start MapId
            PropertyInfo propertyMapId = msg.GetType().GetProperties()[2];
            string MapId = propertyMapId.GetValue(msg).ToString();

            MapXYDictionary = ReturnXYMapId(MapId);

            try
            {
                mapStartX = MapXYDictionary["X"].ToString();
                mapStartY = MapXYDictionary["Y"].ToString();
            }
            catch
            {
                ConsoleManager.Logger.Error("Impossible de trouver la map de départ");
                return;

            }
           

            //Recuperation TotalStepCount
            PropertyInfo propertyTotalStepCount = msg.GetType().GetProperties()[4];
            string TotalStepCount = propertyTotalStepCount.GetValue(msg).ToString();

            //Recuperation CheckPointCurrent
            PropertyInfo propertyCheckPointCurrent = msg.GetType().GetProperties()[5];
            string CheckPointCurrent = propertyCheckPointCurrent.GetValue(msg).ToString();

            //Recuperation CheckPointTotal
            PropertyInfo propertyCheckPointTotal = msg.GetType().GetProperties()[6];
            string CheckPointTotal = propertyCheckPointTotal.GetValue(msg).ToString();

            if(Int32.Parse(CheckPointTotal)-Int32.Parse(CheckPointCurrent)==1)
            {
                ConsoleForm.DeplAuto = false;
                UserForm.CherchPhorreur = false;
                ConsoleManager.Logger.Info("Chasse terminée!");
                return;
            }
            //Recuperation du tableau contenant tous les indices de la chasse en cours
            PropertyInfo propertyKnownStepsList = msg.GetType().GetProperty("KnownStepsList");
            Array KnownStepsList = (Array)propertyKnownStepsList.GetValue(msg);

            int DernierInd = KnownStepsList.Length - 1;
            object DernierElement = KnownStepsList.GetValue(DernierInd);

            PropertyInfo propertyDirection = DernierElement.GetType().GetProperty("Direction");
            PropertyInfo propertyLabel = DernierElement.GetType().GetProperty("PoiLabelId");

            if (propertyLabel == null)
            {
                propertyLabel = DernierElement.GetType().GetProperty("NpcId");
            }

            Direction = propertyDirection.GetValue(DernierElement).ToString();
            string directionFr = "";
            switch (Direction)
            {
                case "2":
                    directionFr = "en bas";
                    break;
                case "4":
                    directionFr = "à gauche";
                    break;
                case "6":
                    directionFr = "en haut";
                    break;
                case "0":
                    directionFr = "à droite";
                    break;
                default:
                    directionFr = "";
                    break;
            }
            Label = propertyLabel.GetValue(DernierElement).ToString();

            string nameId = ReturnNameId(Label);//On recupere le nameId de l'indice
            string IndiceTxt = ReturnIndiceTxt(nameId);//On recupere le nom de l'indice en fonction du name Id
            string LabelIdDM = ReturnIndiceCorrespondace(IndiceTxt);
            
            //Recuperation du tableau contenant tous les drapeau de la chasse en cours
            PropertyInfo propertyFlags = msg.GetType().GetProperty("Flags");
            Array Flags = (Array)propertyFlags.GetValue(msg);
            int tailleFlags = Flags.Length;
            DernierFlag = tailleFlags - 1;
            if (tailleFlags >= Int32.Parse(TotalStepCount))
            {
                if (ConsoleForm.DeplAuto == true)
                {
                    MouvementsAuto objMouv = new MouvementsAuto();
                    objMouv.SetDrapeau(DernierFlag++);
                }
                phorreurMaps.Clear();
                ConsoleManager.Logger.Info("Etape actuelle terminée!");
                return;
            }
            if (Int32.Parse(Label) >= 2669 && Int32.Parse(Label) <= 2672)//Gestion des phorreurs on desactive les depl auto
            {
                CherchPhorreur = true;
                //ConsoleForm.DeplAuto = false;//Que si on ne cherche pas le phorreur en mode auto
                if (ConsoleForm.DeplAuto == true)
                {
                    ConsoleManager.Logger.Warning("Debut de la recherche automatique du phorreur\n");
                    MouvementsAuto objMouv = new MouvementsAuto();
                    objMouv.MouvDirection(Direction, 10);
                    
                }
                return;
                //ConsoleManager.Logger.Warning("Impossible de trouver un phorreur veuillez le trouver manuellement\n");
            }
            else
            {
                CherchPhorreur = false;
            }

            ConsoleManager.Logger.Info("Label reel:" + Label);
            ConsoleManager.Logger.Info("Label Dofus Map:" + LabelIdDM);
            if (DernierFlag >= 0)
            {
                object DernierElementFlag = Flags.GetValue(DernierFlag);

                PropertyInfo propertyFlag = DernierElementFlag.GetType().GetProperty("MapId");
                MapIdFlag = propertyFlag.GetValue(DernierElementFlag).ToString();

                MapXYDictionary = ReturnXYMapId(MapIdFlag);
                mapStartX = MapXYDictionary["X"].ToString();
                mapStartY = MapXYDictionary["Y"].ToString();

            }
            //Recuperation indices / direction sur DofusMap
            DofusMap IndicesDirection;
            HttpReq Requete = new HttpReq();

            if (DernierFlag > 0)
            {
                IndicesDirection = Requete.RecepJSON(mapStartX, mapStartY, Direction);
                ConsoleManager.Logger.Info("mapDernierIndX:" + mapStartX + " mapDernierIndY" + mapStartY);
            }
            else
            {
                IndicesDirection = Requete.RecepJSON(mapStartX, mapStartY, Direction);
                ConsoleManager.Logger.Info("mapDernierIndX:" + mapStartX + " mapDernierIndY" + mapStartY);
            }

            foreach (Hint indice in IndicesDirection.hints)
            {
                if (indice.n.ToString() == LabelIdDM)
                {
                    indiceX=indice.x.ToString();
                    indiceY=indice.y.ToString();
                    indiceDistActu = indice.d.ToString();
                    break;
                }
            }
            if (indiceX != "")
            {
                ConsoleManager.Logger.Info("Indices \""+IndiceTxt+"\" à " + indiceDistActu + " cases " + directionFr + ", en: [" + indiceX + "," + indiceY + "]\n");
                indiceX = "";
                indiceY = "";

                if (ConsoleForm.DeplAuto == true)
                {
                    MouvementsAuto objMouv = new MouvementsAuto();
                    objMouv.MouvDirection(Direction, Int32.Parse(indiceDistActu));
                }
            }
            else
            {
                ConsoleManager.Logger.Info("Erreur, indice introuvable!\n");
                ConsoleForm.DeplAuto = false;
            }
            
        }
    }

    private void label1_Click(object sender, EventArgs e)
    {

    }
  }
}
