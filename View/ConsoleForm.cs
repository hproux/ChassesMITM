// Decompiled with JetBrains decompiler
// Type: AmaknaCore.Sniffer.View.ConsoleForm
// Assembly: AmaknaCore.Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CA1535F7-60A2-442D-94BC-9B222E991CC0
// Assembly location: C:\Users\hugop\Desktop\Logiciels\Dofus\AmaknaCore Sniffer\AmaknaCore.Sniffer.exe

using AmaknaCore.Sniffer.Managers;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AmaknaCore.Sniffer.View
{
  public class ConsoleForm : Form, ChildrenForm
  {
    private IContainer components = (IContainer) null;
    private Button button1;
    public RichTextBox RichTextBox_Main;
    private Button button2;
    public static bool DeplAuto = false;

    public ConsoleForm()
    {
      this.InitializeComponent();
    }

    public void Exit()
    {
      Application.Exit();
    }

    private void ConsoleForm_Closing(object sender, FormClosingEventArgs e)
    {
      ConsoleManager.HideConsole();
      e.Cancel = true;
    }

    private void ConsoleForm_Load(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.RichTextBox_Main = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RichTextBox_Main
            // 
            this.RichTextBox_Main.BackColor = System.Drawing.Color.Black;
            this.RichTextBox_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTextBox_Main.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RichTextBox_Main.ForeColor = System.Drawing.Color.Red;
            this.RichTextBox_Main.Location = new System.Drawing.Point(0, 0);
            this.RichTextBox_Main.Name = "RichTextBox_Main";
            this.RichTextBox_Main.ReadOnly = true;
            this.RichTextBox_Main.Size = new System.Drawing.Size(544, 251);
            this.RichTextBox_Main.TabIndex = 0;
            this.RichTextBox_Main.Text = "";
            this.RichTextBox_Main.TextChanged += new System.EventHandler(this.RichTextBox_Main_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(353, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(179, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Lancer Deplacements auto";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(393, 34);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(139, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Arrêt d\'urgence";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ConsoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(544, 251);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.RichTextBox_Main);
            this.Font = new System.Drawing.Font("Corbel", 10.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConsoleForm";
            this.Text = "Console";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Black;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConsoleForm_Closing);
            this.Load += new System.EventHandler(this.ConsoleForm_Load);
            this.ResumeLayout(false);

    }

    private void RichTextBox_Main_TextChanged(object sender, EventArgs e)
    {

    }

    private void button1_Click(object sender, EventArgs e)
    {
        DeplAuto = true;
        MouvementsAuto objMouv = new MouvementsAuto();
        if (UserForm.CherchPhorreur != true)
        {
            objMouv.MouvDirection(UserForm.Direction, Int32.Parse(UserForm.indiceDistActu));
        }
        else
        {
            objMouv.MouvDirection(UserForm.Direction, 10);
        }
    }

    private void button2_Click(object sender, EventArgs e)
    {
        ConsoleManager.Logger.Info("Arret d'urgence request");
        DeplAuto = false;
        UserForm.CherchPhorreur = false;
    }
  }
}
