// Decompiled with JetBrains decompiler
// Type: AmaknaCore.Sniffer.View.MainForm
// Assembly: AmaknaCore.Sniffer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CA1535F7-60A2-442D-94BC-9B222E991CC0
// Assembly location: C:\Users\hugop\Desktop\Logiciels\Dofus\AmaknaCore Sniffer\AmaknaCore.Sniffer.exe

using AmaknaCore.Sniffer.Managers;
using AmaknaCore.Sniffer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using QuickType;
using QuickTypeDM;
using Newtonsoft.Json;
using System.Windows.Input;
using System.Runtime.InteropServices;

namespace AmaknaCore.Sniffer.View
{
    public class MainForm : Form
    {
        private List<int> ProcessesId = new List<int>();
        private IContainer components = (IContainer)null;
        public string GamePath;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem ToolStripMenuItem_ShowConsole;
        private ToolStripMenuItem ToolStripMenuItem_Infos;
        private ComboBox comboBoxChoixMap;
        private ToolStripMenuItem ToolStripMenuItem_Run;
        private Label label1;
        private Button button4;
        private Label label2;
        private TextBox textBox_Haut;
        private Label label6;
        private Label label3;
        private TextBox textBox_Droite;
        private Label label4;
        private TextBox textBox_Gauche;
        private Label label5;
        private TextBox textBox_Bas;
        private Label label7;
        private TextBox textBox_PremierDrapeau;
        private CheckBox checkBox1;
        private Label label8;
        private Label label9;
        private Label label10;
        static public int ChoixMap = 0;
        public Microsoft.Win32.RegistryKey keyRegister;
        public static int ResolutionXEcran = SystemInformation.VirtualScreen.Width;
        private Label label11;
        private TextBox textBox_secondDrapeau;
        public static int ResolutionYEcran = SystemInformation.VirtualScreen.Height;

        public MainForm()
        {
            this.InitializeComponent();
            this.GamePath = "";
        }

        private void BrowseFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.Description = "Sélectionner le chemin d'accès Dofus.";
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                return;
            string selectedPath = folderBrowserDialog.SelectedPath;
            if (selectedPath == "" || !File.Exists(Path.Combine(selectedPath, "Dofus\\Dofus.exe")))
            {
                ConsoleManager.Logger.Error("Chemin d'accès invalide.");
            }
            else
            {
                this.GamePath = selectedPath;
                File.WriteAllText(Assembly.GetExecutingAssembly().Location.Replace(Assembly.GetExecutingAssembly().GetName().Name + ".exe", "GamePath.txt"), selectedPath);
                ConsoleManager.Logger.Info("Chemin d'accès défini.");
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            keyRegister = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("BotChasseMITM",true);
            if (keyRegister != null)
            {
                try
                {
                    UserForm.ptBas.X = Int32.Parse(keyRegister.GetValue("ptBasX").ToString());
                    UserForm.ptBas.Y = Int32.Parse(keyRegister.GetValue("ptBasY").ToString());
                    textBox_Bas.Text = "Défini depuis la dernière session";
                    UserForm.ptHaut.X = Int32.Parse(keyRegister.GetValue("ptHautX").ToString());
                    UserForm.ptHaut.Y = Int32.Parse(keyRegister.GetValue("ptHautY").ToString());
                    textBox_Haut.Text = "Défini depuis la dernière session";
                    UserForm.ptGauche.X = Int32.Parse(keyRegister.GetValue("ptGaucheX").ToString());
                    UserForm.ptGauche.Y = Int32.Parse(keyRegister.GetValue("ptGaucheY").ToString());
                    textBox_Gauche.Text = "Défini depuis la dernière session";
                    UserForm.ptDroite.X = Int32.Parse(keyRegister.GetValue("ptDroiteX").ToString());
                    UserForm.ptDroite.Y = Int32.Parse(keyRegister.GetValue("ptDroiteY").ToString());
                    textBox_Droite.Text = "Défini depuis la dernière session";
                    UserForm.ptPremierDrapeau.X = Int32.Parse(keyRegister.GetValue("ptPremierDrapeauX").ToString());
                    UserForm.ptPremierDrapeau.Y = Int32.Parse(keyRegister.GetValue("ptPremierDrapeauY").ToString());
                    textBox_PremierDrapeau.Text = "Défini depuis la dernière session";
                    UserForm.ptSecondDrapeau.X = Int32.Parse(keyRegister.GetValue("ptSecondDrapeauX").ToString());
                    UserForm.ptSecondDrapeau.Y = Int32.Parse(keyRegister.GetValue("ptSecondDrapeauY").ToString());
                    textBox_secondDrapeau.Text = "Défini depuis la dernière session";
                    UserForm.ecartDrapeau = UserForm.ptSecondDrapeau.Y - UserForm.ptPremierDrapeau.Y;
                }
                catch
                {
                    MessageBox.Show("Impossible de recuperer tous les points enregistrés lors de la derniere session");
                }
                
            }
            else
            {
                keyRegister = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("BotChasseMITM"); 
            }
            comboBoxChoixMap.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxChoixMap.SelectedIndex = 0;
            
            WindowManager.Initialize(this);
            string path = Assembly.GetExecutingAssembly().Location.Replace(Assembly.GetExecutingAssembly().GetName().Name + ".exe", "GamePath.txt");
            if (File.Exists(path))
            {
                string path1 = File.ReadAllText(path);
                if (File.Exists(Path.Combine(path1, "Dofus\\Dofus.exe")))
                    this.GamePath = path1;
            }
            if (this.GamePath == "")
                ConsoleManager.Logger.Warning("Aucun chemin d'accès n'a été défini.");
            new Thread(new ThreadStart(this.PatcherThread))
            {
                IsBackground = true
            }.Start();
            ConsoleManager.ShowConsole();
        }

        private void PatcherThread()
        {
            while (true)
            {
                foreach (Process process in Process.GetProcessesByName("Dofus"))
                {
                    if (!this.ProcessesId.Contains(process.Id))
                    {
                        this.ProcessesId.Add(process.Id);
                        Process.Start(new ProcessStartInfo("D2Injector.exe", process.Id.ToString())
                        {
                            WindowStyle = ProcessWindowStyle.Hidden,
                            UseShellExecute = true
                        });
                    }
                }
                Thread.Sleep(800);
            }
        }

        private void MainForm_Closing(object sender, FormClosingEventArgs e)
        {
            try
            {
                foreach (Process process in Process.GetProcessesByName("D2Injector"))
                    process.Kill();
                foreach (Process process in Process.GetProcessesByName("EasyHook64Svc"))
                    process.Kill();
                foreach (Process process in Process.GetProcessesByName("EasyHook32Svc"))
                    process.Kill();
            }
            catch (Exception ex)
            {
            }
            Program.Exit();
        }

        private void ToolStripMenuItem_ShowConsole_Click(object sender, EventArgs e)
        {
            ConsoleManager.ShowConsole();
        }

        private void ToolStripMenuItem_Infos_Click(object sender, EventArgs e)
        {
            if (WindowManager.ActiveChildrens.FirstOrDefault<Form>((Func<Form, bool>)(f => f.GetType() == typeof(InfosForm))) != null)
                return;
            WindowManager.AddChildrenForm((Form)new InfosForm());
        }

        private void ToolStripMenuItem_Run_Click(object sender, EventArgs e)
        {
            if (this.GamePath == "")
            {
                new Thread(new ThreadStart(this.BrowseFolder))
                {
                    ApartmentState = ApartmentState.STA
                }.Start();
            }
            else
            {
                try
                {
                    Process.Start(Path.Combine(this.GamePath, "Dofus\\Dofus.exe"));
                }
                catch (Exception ex)
                {
                }
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItem_ShowConsole = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Run = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Infos = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBoxChoixMap = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Haut = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Droite = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_Gauche = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_Bas = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_PremierDrapeau = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_secondDrapeau = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_ShowConsole,
            this.ToolStripMenuItem_Run,
            this.ToolStripMenuItem_Infos});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(962, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ToolStripMenuItem_ShowConsole
            // 
            this.ToolStripMenuItem_ShowConsole.Image = global::AmaknaCore.Sniffer.Properties.Resources.Console;
            this.ToolStripMenuItem_ShowConsole.Name = "ToolStripMenuItem_ShowConsole";
            this.ToolStripMenuItem_ShowConsole.Size = new System.Drawing.Size(133, 20);
            this.ToolStripMenuItem_ShowConsole.Text = "Afficher la console";
            this.ToolStripMenuItem_ShowConsole.Click += new System.EventHandler(this.ToolStripMenuItem_ShowConsole_Click);
            // 
            // ToolStripMenuItem_Run
            // 
            this.ToolStripMenuItem_Run.Image = global::AmaknaCore.Sniffer.Properties.Resources.updater_icon_24x24;
            this.ToolStripMenuItem_Run.Name = "ToolStripMenuItem_Run";
            this.ToolStripMenuItem_Run.Size = new System.Drawing.Size(104, 20);
            this.ToolStripMenuItem_Run.Text = "Lancer Dofus";
            this.ToolStripMenuItem_Run.Click += new System.EventHandler(this.ToolStripMenuItem_Run_Click);
            // 
            // ToolStripMenuItem_Infos
            // 
            this.ToolStripMenuItem_Infos.Image = global::AmaknaCore.Sniffer.Properties.Resources.Info_2;
            this.ToolStripMenuItem_Infos.Name = "ToolStripMenuItem_Infos";
            this.ToolStripMenuItem_Infos.Size = new System.Drawing.Size(103, 20);
            this.ToolStripMenuItem_Infos.Text = "Informations";
            this.ToolStripMenuItem_Infos.Visible = false;
            this.ToolStripMenuItem_Infos.Click += new System.EventHandler(this.ToolStripMenuItem_Infos_Click);
            // 
            // comboBoxChoixMap
            // 
            this.comboBoxChoixMap.FormattingEnabled = true;
            this.comboBoxChoixMap.Items.AddRange(new object[] {
            "Defaut",
            "Village Zoths"});
            this.comboBoxChoixMap.Location = new System.Drawing.Point(12, 600);
            this.comboBoxChoixMap.Name = "comboBoxChoixMap";
            this.comboBoxChoixMap.Size = new System.Drawing.Size(178, 21);
            this.comboBoxChoixMap.TabIndex = 6;
            this.comboBoxChoixMap.SelectedIndexChanged += new System.EventHandler(this.comboBoxChoixMap_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.DarkGray;
            this.label1.Font = new System.Drawing.Font("Corbel", 11.5F);
            this.label1.Location = new System.Drawing.Point(196, 598);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(613, 19);
            this.label1.TabIndex = 8;
            this.label1.Text = "Après modification, ajouter un drapeau fictif et l\'enlever directement pour actua" +
    "liser l\'indice";
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Lime;
            this.button4.Font = new System.Drawing.Font("Corbel", 12.25F);
            this.button4.Location = new System.Drawing.Point(12, 520);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(175, 42);
            this.button4.TabIndex = 15;
            this.button4.Text = "Configuration écran";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.DarkGray;
            this.label2.Font = new System.Drawing.Font("Corbel", 12.25F);
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(12, 574);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 21);
            this.label2.TabIndex = 16;
            this.label2.Text = "Choix de la zone:";
            // 
            // textBox_Haut
            // 
            this.textBox_Haut.Font = new System.Drawing.Font("Corbel", 12.25F);
            this.textBox_Haut.Location = new System.Drawing.Point(205, 523);
            this.textBox_Haut.Multiline = true;
            this.textBox_Haut.Name = "textBox_Haut";
            this.textBox_Haut.ReadOnly = true;
            this.textBox_Haut.Size = new System.Drawing.Size(144, 51);
            this.textBox_Haut.TabIndex = 17;
            this.textBox_Haut.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_Haut_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.DarkGray;
            this.label6.Font = new System.Drawing.Font("Corbel", 12.25F);
            this.label6.ForeColor = System.Drawing.Color.Cyan;
            this.label6.Location = new System.Drawing.Point(254, 499);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 21);
            this.label6.TabIndex = 21;
            this.label6.Text = "Haut";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.DarkGray;
            this.label3.Font = new System.Drawing.Font("Corbel", 12.25F);
            this.label3.ForeColor = System.Drawing.Color.BlueViolet;
            this.label3.Location = new System.Drawing.Point(704, 499);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 21);
            this.label3.TabIndex = 23;
            this.label3.Text = "Droite";
            // 
            // textBox_Droite
            // 
            this.textBox_Droite.Font = new System.Drawing.Font("Corbel", 12.25F);
            this.textBox_Droite.Location = new System.Drawing.Point(655, 524);
            this.textBox_Droite.Multiline = true;
            this.textBox_Droite.Name = "textBox_Droite";
            this.textBox_Droite.ReadOnly = true;
            this.textBox_Droite.Size = new System.Drawing.Size(145, 50);
            this.textBox_Droite.TabIndex = 22;
            this.textBox_Droite.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_Droite_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.DarkGray;
            this.label4.Font = new System.Drawing.Font("Corbel", 12.25F);
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(547, 499);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 21);
            this.label4.TabIndex = 25;
            this.label4.Text = "Gauche";
            // 
            // textBox_Gauche
            // 
            this.textBox_Gauche.Font = new System.Drawing.Font("Corbel", 12.25F);
            this.textBox_Gauche.Location = new System.Drawing.Point(505, 523);
            this.textBox_Gauche.Multiline = true;
            this.textBox_Gauche.Name = "textBox_Gauche";
            this.textBox_Gauche.ReadOnly = true;
            this.textBox_Gauche.Size = new System.Drawing.Size(144, 51);
            this.textBox_Gauche.TabIndex = 24;
            this.textBox_Gauche.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_Gauche_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.DarkGray;
            this.label5.Font = new System.Drawing.Font("Corbel", 12.25F);
            this.label5.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label5.Location = new System.Drawing.Point(404, 499);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 21);
            this.label5.TabIndex = 27;
            this.label5.Text = "Bas";
            // 
            // textBox_Bas
            // 
            this.textBox_Bas.Font = new System.Drawing.Font("Corbel", 12.25F);
            this.textBox_Bas.Location = new System.Drawing.Point(355, 523);
            this.textBox_Bas.Multiline = true;
            this.textBox_Bas.Name = "textBox_Bas";
            this.textBox_Bas.ReadOnly = true;
            this.textBox_Bas.Size = new System.Drawing.Size(144, 51);
            this.textBox_Bas.TabIndex = 26;
            this.textBox_Bas.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_Bas_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.DarkGray;
            this.label7.Font = new System.Drawing.Font("Corbel", 12.25F);
            this.label7.ForeColor = System.Drawing.Color.Indigo;
            this.label7.Location = new System.Drawing.Point(815, 500);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(126, 21);
            this.label7.TabIndex = 29;
            this.label7.Text = "Premier drapeau";
            // 
            // textBox_PremierDrapeau
            // 
            this.textBox_PremierDrapeau.Font = new System.Drawing.Font("Corbel", 12.25F);
            this.textBox_PremierDrapeau.Location = new System.Drawing.Point(806, 524);
            this.textBox_PremierDrapeau.Multiline = true;
            this.textBox_PremierDrapeau.Name = "textBox_PremierDrapeau";
            this.textBox_PremierDrapeau.ReadOnly = true;
            this.textBox_PremierDrapeau.Size = new System.Drawing.Size(145, 50);
            this.textBox_PremierDrapeau.TabIndex = 28;
            this.textBox_PremierDrapeau.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_PremierDrapeau_KeyDown);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.DarkGray;
            this.checkBox1.Font = new System.Drawing.Font("Corbel", 11.25F);
            this.checkBox1.Location = new System.Drawing.Point(773, 27);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(186, 22);
            this.checkBox1.TabIndex = 31;
            this.checkBox1.Text = "Supprimer border Console";
            this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.DarkGray;
            this.label8.Font = new System.Drawing.Font("Corbel", 11.75F);
            this.label8.Location = new System.Drawing.Point(8, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(230, 19);
            this.label8.TabIndex = 33;
            this.label8.Text = "-Desactiver la transparence en jeu";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.DarkGray;
            this.label9.Font = new System.Drawing.Font("Corbel", 11.75F);
            this.label9.Location = new System.Drawing.Point(8, 58);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(143, 19);
            this.label9.TabIndex = 34;
            this.label9.Text = "-Jouer en plein ecran";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.DarkGray;
            this.label10.Font = new System.Drawing.Font("Corbel", 11.75F);
            this.label10.Location = new System.Drawing.Point(12, 86);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(514, 19);
            this.label10.TabIndex = 35;
            this.label10.Text = "-Toujours garder Dofus devant les autres fenêtres lors des déplacements auto";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.DarkGray;
            this.label11.Font = new System.Drawing.Font("Corbel", 12.25F);
            this.label11.ForeColor = System.Drawing.Color.Indigo;
            this.label11.Location = new System.Drawing.Point(815, 574);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(121, 21);
            this.label11.TabIndex = 38;
            this.label11.Text = "Second drapeau";
            // 
            // textBox_secondDrapeau
            // 
            this.textBox_secondDrapeau.Font = new System.Drawing.Font("Corbel", 12.25F);
            this.textBox_secondDrapeau.Location = new System.Drawing.Point(806, 594);
            this.textBox_secondDrapeau.Multiline = true;
            this.textBox_secondDrapeau.Name = "textBox_secondDrapeau";
            this.textBox_secondDrapeau.ReadOnly = true;
            this.textBox_secondDrapeau.Size = new System.Drawing.Size(145, 37);
            this.textBox_secondDrapeau.TabIndex = 37;
            this.textBox_secondDrapeau.TextChanged += new System.EventHandler(this.textBox_secondDrapeau_TextChanged);
            this.textBox_secondDrapeau.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_secondDrapeau_KeyDown);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(962, 633);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBox_secondDrapeau);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox_PremierDrapeau);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_Bas);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_Gauche);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_Droite);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox_Haut);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxChoixMap);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Corbel", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Farm Chasseur";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_Closing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void comboBoxChoixMap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxChoixMap.SelectedText == "Defaut")
            {
                ChoixMap = 0;
            }
            else if (comboBoxChoixMap.SelectedText == "Village Zoths")
            {
                ChoixMap = 2;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pour configurer la souris, cliquer dans les zone de texte correspondante, placer la souris au bon endroit puis appuyer sur n'importe quelle touche du clavier");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                ConsoleManager.Window.FormBorderStyle = FormBorderStyle.None;
            }
            else
            {
                ConsoleManager.Window.FormBorderStyle = FormBorderStyle.FixedSingle;
            }

        }

        private void textBox_Haut_KeyDown(object sender, KeyEventArgs e)
        {
            MouvementsAuto objMouv = new MouvementsAuto();
            Point pt = objMouv.GetCursorPos();
            UserForm.ptHaut = pt;
            ConsoleManager.Logger.Info("Point Haut défini en X:"+pt.X+"/Y: "+pt.Y);
            keyRegister.SetValue("ptHautX", pt.X);
            keyRegister.SetValue("ptHautY", pt.Y);
            textBox_Haut.Text = "Defini";
            this.ActiveControl = textBox_Bas;
        }

        private void textBox_Bas_KeyDown(object sender, KeyEventArgs e)
        {
            MouvementsAuto objMouv = new MouvementsAuto();
            Point pt = objMouv.GetCursorPos();
            UserForm.ptBas = pt;
            ConsoleManager.Logger.Info("Point Bas défini en X:" + pt.X + "/Y: " + pt.Y);
            keyRegister.SetValue("ptBasX", pt.X);
            keyRegister.SetValue("ptBasY", pt.Y);
            textBox_Bas.Text = "Defini";
            this.ActiveControl = textBox_Gauche;
        }

        private void textBox_Gauche_KeyDown(object sender, KeyEventArgs e)
        {
            MouvementsAuto objMouv = new MouvementsAuto();
            Point pt = objMouv.GetCursorPos();
            UserForm.ptGauche = pt;
            ConsoleManager.Logger.Info("Point Gauche défini en X:" + pt.X + "/Y: " + pt.Y);
            keyRegister.SetValue("ptGaucheX", pt.X);
            keyRegister.SetValue("ptGaucheY", pt.Y);
            textBox_Gauche.Text = "Defini";
            this.ActiveControl = textBox_Droite;
        }

        private void textBox_Droite_KeyDown(object sender, KeyEventArgs e)
        {
            MouvementsAuto objMouv = new MouvementsAuto();
            Point pt = objMouv.GetCursorPos();
            UserForm.ptDroite = pt;
            ConsoleManager.Logger.Info("Point Droite défini en X:" + pt.X + "/Y: " + pt.Y);
            keyRegister.SetValue("ptDroiteX", pt.X);
            keyRegister.SetValue("ptDroiteY", pt.Y);
            textBox_Droite.Text = "Defini";
            this.ActiveControl = textBox_PremierDrapeau;
        }

        private void textBox_PremierDrapeau_KeyDown(object sender, KeyEventArgs e)
        {
            MouvementsAuto objMouv = new MouvementsAuto();
            Point pt = objMouv.GetCursorPos();
            UserForm.ptPremierDrapeau = pt;
            ConsoleManager.Logger.Info("Point Premier Drapeau défini en X:" + pt.X + "/Y: " + pt.Y);
            keyRegister.SetValue("ptPremierDrapeauX", pt.X);
            keyRegister.SetValue("ptPremierDrapeauY", pt.Y);
            textBox_PremierDrapeau.Text = "Defini";
            this.ActiveControl = textBox_secondDrapeau;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox_secondDrapeau_KeyDown(object sender, KeyEventArgs e)
        {
            MouvementsAuto objMouv = new MouvementsAuto();
            Point pt = objMouv.GetCursorPos();
            UserForm.ptSecondDrapeau = pt;
            
            ConsoleManager.Logger.Info("Point Second Drapeau défini en X:" + pt.X + "/Y: " + pt.Y);
            UserForm.ecartDrapeau = UserForm.ptSecondDrapeau.Y-UserForm.ptPremierDrapeau.Y;
            ConsoleManager.Logger.Info("Ecart entre les drapeaux: " + UserForm.ecartDrapeau + "px");
            keyRegister.SetValue("ptSecondDrapeauX", pt.X);
            keyRegister.SetValue("ptSecondDrapeauY", pt.Y);
            textBox_secondDrapeau.Text = "Defini";
        }

        private void textBox_secondDrapeau_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
