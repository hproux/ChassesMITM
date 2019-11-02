using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace AmaknaCore.Sniffer
{
    class MouvementsAuto
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_MOVE = 0x0001;
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowDC(IntPtr window);
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern uint GetPixel(IntPtr dc, int x, int y);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ReleaseDC(IntPtr window, IntPtr dc);

        public static Color GetColorAt(Point pt = new Point())
        {
            int x = pt.X;
            int y  =pt.Y;
            IntPtr desk = GetDesktopWindow();
            IntPtr dc = GetWindowDC(desk);
            int a = (int)GetPixel(dc, x, y);
            ReleaseDC(desk, dc);
            return Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
        }

        public Bitmap GetScreenShot()
        {
            Bitmap result = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppPArgb);
                using (Graphics gfx = Graphics.FromImage(result))
                {
                    gfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    gfx.DrawImage(result, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                    gfx.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                }
                //result.Save("test.tiff", ImageFormat.Tiff);
            return result;
        }

        public bool SommeCouleurPixEcran(Color CouleurVoulu, Color CouleurVoulu2)//Permet de verifier si un pixel d'une certaine couleur est actuellement sur l'ecran
        {
            Color ColorPixCourant;
            int largeurScreen = AmaknaCore.Sniffer.View.MainForm.ResolutionXEcran;
            int hauteurScreen = AmaknaCore.Sniffer.View.MainForm.ResolutionYEcran;
            using (Bitmap bmp = GetScreenShot())
            {
                for (int posYCourant = 0; posYCourant < bmp.Height; posYCourant+=2)
                {//On parcourt tous les pixels de l'ecran
                    for (int posXCourant = 0; posXCourant < bmp.Width; posXCourant+=2)
                    {                      
                        ColorPixCourant = bmp.GetPixel(posXCourant,posYCourant);
                        if ((ColorPixCourant.R == CouleurVoulu.R && ColorPixCourant.G == CouleurVoulu.G && ColorPixCourant.B == CouleurVoulu.B) || (ColorPixCourant.R == CouleurVoulu2.R && ColorPixCourant.G == CouleurVoulu2.G && ColorPixCourant.B == CouleurVoulu2.B))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public async Task LeftClick()
        {
            //perform click            
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            AmaknaCore.Sniffer.Managers.ConsoleManager.Logger.Info("Clic Gauche");
        }

        public void SetCursorPos(int X, int Y)
        {
            Point pt = new Point();
            pt.X = X;
            pt.Y = Y;
            Cursor.Position = pt;
        }

        public Point GetCursorPos()
        {
            Point pt = new Point();
            pt.X = Cursor.Position.X;
            pt.Y = Cursor.Position.Y;
            return pt;
        }

        public int sommeCouleur(Color color)
        {
            int somme = color.R + color.G + color.B; ;
            return somme;
        }

        public async Task SetDrapeau(int NumDrapeau)
        {
            NumDrapeau++;
            Point ResetSouris = new Point();
            ResetSouris = GetCursorPos();

            SetCursorPos(AmaknaCore.Sniffer.View.UserForm.ptPremierDrapeau.X, AmaknaCore.Sniffer.View.UserForm.ptPremierDrapeau.Y+(NumDrapeau*AmaknaCore.Sniffer.View.UserForm.ecartDrapeau));
            LeftClick();
            SetCursorPos(ResetSouris.X, ResetSouris.Y);
        }

        public async Task MouvDirection(string Direction, int Distance)
        {
            if (AmaknaCore.Sniffer.View.UserForm.isMooving == false)
            {
                AmaknaCore.Sniffer.View.UserForm.isMooving = true;
                Color CouleurPhorreur;
                Color CouleurPhorreur2;
                CouleurPhorreur = Color.FromArgb(71, 105, 86);
                CouleurPhorreur2 = Color.FromArgb(73, 106, 85);
                Point ClicPosition = new Point();
                Point ptMilieuEcran = new Point();
                Color ColorMilieuEcran = new Color();
                Point ptMilieuEcran2 = new Point();
                Color ColorMilieuEcran2 = new Color();
                ptMilieuEcran.X = AmaknaCore.Sniffer.View.MainForm.ResolutionXEcran / 2;
                ptMilieuEcran.Y = AmaknaCore.Sniffer.View.MainForm.ResolutionYEcran / 2;
                ptMilieuEcran2.X = ptMilieuEcran.X - 300;
                ptMilieuEcran2.Y = ptMilieuEcran.Y + 200;

                switch (Direction)
                {
                    case "2":
                        ClicPosition = AmaknaCore.Sniffer.View.UserForm.ptBas;
                        break;
                    case "4":
                        ClicPosition = AmaknaCore.Sniffer.View.UserForm.ptGauche;
                        break;
                    case "6":
                        ClicPosition = AmaknaCore.Sniffer.View.UserForm.ptHaut;
                        break;
                    case "0":
                        ClicPosition = AmaknaCore.Sniffer.View.UserForm.ptDroite;
                        break;
                }
                Point ResetSouris = new Point();
           
                int compteurBoucle = 0;//Pour verifier si le personnage ne bloque pas
            
                for (int i = 0; i < Distance; i++)
                {

                    if (AmaknaCore.Sniffer.View.ConsoleForm.DeplAuto == true)
                    {
                        ResetSouris = GetCursorPos();
                        SetCursorPos(ClicPosition.X, ClicPosition.Y);
                        LeftClick();
                        SetCursorPos(ResetSouris.X, ResetSouris.Y);
                        ColorMilieuEcran = GetColorAt(ptMilieuEcran);
                        ColorMilieuEcran2 = GetColorAt(ptMilieuEcran2);
                        AmaknaCore.Sniffer.Managers.ConsoleManager.Logger.Info("En cours de deplacement...");
                        while (sommeCouleur(ColorMilieuEcran) >= 1 && sommeCouleur(ColorMilieuEcran2)>= 1) //on test a quel moment la map change (si pixels noirs)
                        {
                            ColorMilieuEcran = GetColorAt(ptMilieuEcran);
                            ColorMilieuEcran2 = GetColorAt(ptMilieuEcran2);
                            await Task.Delay(50);
                            compteurBoucle += 1;
                            if (compteurBoucle > 120)
                            {
                                AmaknaCore.Sniffer.Managers.ConsoleManager.Logger.Error("Impossible de changer de map");
                                //CherchPhorreur = false;
                                AmaknaCore.Sniffer.View.UserForm.isMooving = false;
                                return;
                            }

                        }
                        compteurBoucle = 0;
                        AmaknaCore.Sniffer.Managers.ConsoleManager.Logger.Info("Ecran Noir");
                        while (sommeCouleur(ColorMilieuEcran) <= 1 && sommeCouleur(ColorMilieuEcran2) <= 1)//on attend que l'ecran ne soit plus noir
                        {
                            ColorMilieuEcran = GetColorAt(ptMilieuEcran);
                            ColorMilieuEcran2 = GetColorAt(ptMilieuEcran2);
                            await Task.Delay(50);
                            compteurBoucle += 1;
                            if (compteurBoucle > 120)
                            {
                                AmaknaCore.Sniffer.Managers.ConsoleManager.Logger.Error("Impossible de changer de map");
                                //CherchPhorreur = false;
                                AmaknaCore.Sniffer.View.UserForm.isMooving = false;
                                return;
                            }
                        }
                        AmaknaCore.Sniffer.Managers.ConsoleManager.Logger.Info("Changement de map effectué");
                        await Task.Delay(500);
                        if (AmaknaCore.Sniffer.View.UserForm.CherchPhorreur == true)//Si on recherche un phorreur
                        {
                            await Task.Delay(2000);//Tempo pour charger la map(utilisation des trames a la place ?)
                            bool isPhorreurHere = SommeCouleurPixEcran(CouleurPhorreur, CouleurPhorreur2);//On test si un phorreur est sur l'ecran
                            string PosActu="";
                            if (isPhorreurHere == true)
                            {
                                AmaknaCore.Sniffer.Managers.ConsoleManager.Logger.Info("Phorreur détécté sur cette map");
                                int PosDepl=0;
                                switch (Direction)
                                {
                                    case "2":
                                        PosDepl = Int32.Parse(AmaknaCore.Sniffer.View.UserForm.mapStartY) + (i + 1);
                                        PosActu = AmaknaCore.Sniffer.View.UserForm.mapStartX.ToString() + PosDepl;
                                        break;
                                    case "4":
                                        PosDepl = Int32.Parse(AmaknaCore.Sniffer.View.UserForm.mapStartX) - (i + 1);
                                        PosActu = PosDepl + AmaknaCore.Sniffer.View.UserForm.mapStartY.ToString();
                                        break;
                                    case "6":
                                        PosDepl = Int32.Parse(AmaknaCore.Sniffer.View.UserForm.mapStartY) - (i + 1);
                                        PosActu = AmaknaCore.Sniffer.View.UserForm.mapStartX.ToString() + PosDepl;
                                        break;
                                    case "0":
                                        PosDepl = Int32.Parse(AmaknaCore.Sniffer.View.UserForm.mapStartX) + (i + 1);
                                        PosActu = PosDepl + AmaknaCore.Sniffer.View.UserForm.mapStartY.ToString();
                                        break;
                                }

                                if (AmaknaCore.Sniffer.View.UserForm.phorreurMaps != null && AmaknaCore.Sniffer.View.UserForm.CherchPhorreur == true)
                                {
                                    if (AmaknaCore.Sniffer.View.UserForm.phorreurMaps.Contains(PosActu) == false) {
                                        AmaknaCore.Sniffer.View.UserForm.phorreurMaps.Add(PosActu);
                                        SetDrapeau(AmaknaCore.Sniffer.View.UserForm.DernierFlag);
                                        AmaknaCore.Sniffer.View.UserForm.CherchPhorreur = false;
                                        AmaknaCore.Sniffer.View.UserForm.isMooving = false;
                                        return;
                                    }
                                }
                                else if(AmaknaCore.Sniffer.View.UserForm.CherchPhorreur==true)
                                {
                                    AmaknaCore.Sniffer.View.UserForm.phorreurMaps.Add(PosActu);
                                    SetDrapeau(AmaknaCore.Sniffer.View.UserForm.DernierFlag);
                                    AmaknaCore.Sniffer.View.UserForm.CherchPhorreur = false;
                                    AmaknaCore.Sniffer.View.UserForm.isMooving = false;
                                    return;
                                }
                            
                            }
                            else if (i >= 10)
                            {
                                AmaknaCore.Sniffer.Managers.ConsoleManager.Logger.Error("Phorreur introuvable, trouves le toi même!");
                                AmaknaCore.Sniffer.View.UserForm.CherchPhorreur = false;
                                AmaknaCore.Sniffer.View.UserForm.isMooving = false;
                                return;
                            }
                            else
                            {
                                AmaknaCore.Sniffer.Managers.ConsoleManager.Logger.Info("Phorreur introuvable sur cette map");
                            }
                        }
                    }
                    else
                    {
                        AmaknaCore.Sniffer.Managers.ConsoleManager.Logger.Warning("Arret d'urgence effectué");
                        AmaknaCore.Sniffer.View.UserForm.isMooving = false;
                        return;
                    }
                }
                SetDrapeau(AmaknaCore.Sniffer.View.UserForm.DernierFlag);
                AmaknaCore.Sniffer.View.UserForm.isMooving = false;
            }
        }
    }
}
