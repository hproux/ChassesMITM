// Decompiled with JetBrains decompiler
// Type: AmaknaCore.Sniffer.View.InfosForm
// Assembly: AmaknaCore.Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CA1535F7-60A2-442D-94BC-9B222E991CC0
// Assembly location: C:\Users\hugop\Desktop\Logiciels\Dofus\AmaknaCore Sniffer\AmaknaCore.Sniffer.exe

using AmaknaCore.Sniffer.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AmaknaCore.Sniffer.View
{
  public class InfosForm : Form
  {
    private IContainer components = (IContainer) null;
    private PictureBox pictureBox1;
    private Label label1;
    private Label label2;
    private PictureBox pictureBox2;
    private Label label3;
    private LinkLabel linkLabel2;

    public InfosForm()
    {
      this.InitializeComponent();
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start("http://alexandre1004.net/");
    }

    private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start("http://cadernis.fr/");
    }

    private void pictureBox2_Click(object sender, EventArgs e)
    {
      Process.Start("http://creativecommons.org/licenses/by-nc-nd/2.0/fr/");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (InfosForm));
      this.label1 = new Label();
      this.label2 = new Label();
      this.pictureBox2 = new PictureBox();
      this.pictureBox1 = new PictureBox();
      this.label3 = new Label();
      this.linkLabel2 = new LinkLabel();
      ((ISupportInitialize) this.pictureBox2).BeginInit();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Segoe UI", 11.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(162, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(307, 20);
      this.label1.TabIndex = 1;
      this.label1.Text = "AmaknaCore Sniffer © 2017 - Alexandre1004";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Segoe UI", 11.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(162, 56);
      this.label2.Name = "label2";
      this.label2.Size = new Size(343, 80);
      this.label2.TabIndex = 2;
      this.label2.Text = "Cet utilitaire est mis à disposition selon les termes \r\nde la licence Creative Commons Paternité-Pas \r\nd'Utilisation Commerciale-Pas de Modification \r\n2.0 France.";
      this.pictureBox2.Cursor = Cursors.Hand;
      this.pictureBox2.Image = (Image) Resources.Copyright;
      this.pictureBox2.Location = new Point(414, 182);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new Size(88, 31);
      this.pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox2.TabIndex = 3;
      this.pictureBox2.TabStop = false;
      this.pictureBox2.Click += new EventHandler(this.pictureBox2_Click);
      this.pictureBox1.Image = (Image) Resources.Big_Info;
      this.pictureBox1.Location = new Point(14, 14);
      this.pictureBox1.Margin = new Padding(3, 4, 3, 4);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(128, 128);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Segoe UI", 11.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(10, 144);
      this.label3.Name = "label3";
      this.label3.Size = new Size(477, 40);
      this.label3.TabIndex = 4;
      this.label3.Text = "Basé sur une oeuvre à Cadernis, les autorisations au-delà du champ de \r\ncette licence peuvent être obtenues sur www.Cadernis.fr.";
      this.linkLabel2.AutoSize = true;
      this.linkLabel2.Font = new Font("Segoe UI", 11.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.linkLabel2.Location = new Point(13, 194);
      this.linkLabel2.Name = "linkLabel2";
      this.linkLabel2.Size = new Size(79, 20);
      this.linkLabel2.TabIndex = 6;
      this.linkLabel2.TabStop = true;
      this.linkLabel2.Text = "Cadernis.fr";
      this.linkLabel2.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
      this.AutoScaleDimensions = new SizeF(7f, 15f);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new Size(514, 220);
      this.Controls.Add((Control) this.linkLabel2);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.pictureBox2);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.pictureBox1);
      this.Font = new Font("Corbel", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Margin = new Padding(3, 4, 3, 4);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "InfosForm";
      this.Text = "Informations";
      ((ISupportInitialize) this.pictureBox2).EndInit();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
