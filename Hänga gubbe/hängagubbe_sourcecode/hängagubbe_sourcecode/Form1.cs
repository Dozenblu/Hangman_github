using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace hängagubbe_sourcecode
{
    public partial class hänga_Gubbe : Form
    {
        //Array för att bestämma vilken bild som ska visas 
        private Bitmap[] liv = {hängagubbe_sourcecode.Properties.Resources.liv1,
                                hängagubbe_sourcecode.Properties.Resources.liv2,
                                hängagubbe_sourcecode.Properties.Resources.liv3,
                                hängagubbe_sourcecode.Properties.Resources.liv4,
                                hängagubbe_sourcecode.Properties.Resources.liv5,
                                hängagubbe_sourcecode.Properties.Resources.liv6,
                                hängagubbe_sourcecode.Properties.Resources.liv7,
                                hängagubbe_sourcecode.Properties.Resources.liv8};
       
        //Array för ett alternativt skin som användaren kan köpa
        private Bitmap[] liv_skin_andjen = {hängagubbe_sourcecode.Properties.Resources.skin_andjen_1,
                                            hängagubbe_sourcecode.Properties.Resources.skin_andjen_2,
                                            hängagubbe_sourcecode.Properties.Resources.skin_andjen_3,
                                            hängagubbe_sourcecode.Properties.Resources.skin_andjen_4,
                                            hängagubbe_sourcecode.Properties.Resources.skin_andjen_5,
                                            hängagubbe_sourcecode.Properties.Resources.skin_andjen_6,
                                            hängagubbe_sourcecode.Properties.Resources.skin_andjen_7,
                                            hängagubbe_sourcecode.Properties.Resources.skin_andjen_8,};
        
        //Variabel för att få tillgång till en custom-ritad bakgrund
        private Bitmap bakgrund_mosaik = hängagubbe_sourcecode.Properties.Resources.bakgrund_skin_mosaik;

        //Variabler som används i programmet flera gånger i olika delar
        private bool standardSkin = true;
        private bool andjenUpplåst = false;
        private bool mosaikUpplåst = false;
        private string ord;
        private string nuvarandeOrd = "";
        private string kopiaOrd = "";
        private bool lvl1 = false;
        private bool lvl2 = false;
        private bool lvl3 = false;
        private int winningStreak = 0;
        private int förlust = 0;
        private int felGissningar = 0;
        private int indexLedtråd = 0;
        private int gissningar = 8;
        private int credits = 20;
        private double antalSpel;
        private double vinster;
        private double winrate;
        Random slumpOrd = new Random();

        public hänga_Gubbe()
        {
            InitializeComponent();
        }

        //Rita ett svart streck som avskiljer spelet från "Shop" sektionen
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SolidBrush svart = new SolidBrush(Color.Black);
            g.FillRectangle(svart, 10, 420, 1030, 5);
            base.OnPaint(e);
        }

        //Funktion som enablear alla knappar efter man tryckt på dem. Används vid flera olika tillfällen
        private void EnableButtons()
        {
            btnA.Enabled = true;
            btnB.Enabled = true;
            btnC.Enabled = true;
            btnD.Enabled = true;
            btnE.Enabled = true;
            btnF.Enabled = true;
            btnG.Enabled = true;
            btnH.Enabled = true;
            btnI.Enabled = true;
            btnJ.Enabled = true;
            btnK.Enabled = true;
            btnL.Enabled = true;
            btnM.Enabled = true;
            btnN.Enabled = true;
            btnO.Enabled = true;
            btnP.Enabled = true;
            btnQ.Enabled = true;
            btnR.Enabled = true;
            btnS.Enabled = true;
            btnT.Enabled = true;
            btnU.Enabled = true;
            btnV.Enabled = true;
            btnW.Enabled = true;
            btnX.Enabled = true;
            btnY.Enabled = true;
            btnZ.Enabled = true;
            btnLedtråd.Enabled = true;
        }

        private void väljOrd()
        {
            felGissningar = 0;
            if (standardSkin)
            {
                pbHänga.Image = liv[felGissningar];
            }
            else if (standardSkin == false)
            {
                pbHänga.Image = liv_skin_andjen[felGissningar];
            }
            nuvarandeOrd = ord;
            kopiaOrd = "";
            for (int i = 0; i < nuvarandeOrd.Length; i++)
            {
                kopiaOrd += "_";
            }
            visaKopia();

        }

        private void visaKopia()
        {
            lblOrdet.Text = "";
            for (int i = 0; i < kopiaOrd.Length; i++)
            {
                lblOrdet.Text += kopiaOrd.Substring(i, 1);
                lblOrdet.Text += " ";
            }

        }

        private void btnGissa(object sender, EventArgs e)
        {
            Button val = sender as Button;
            val.Enabled = false;
            if (nuvarandeOrd.Contains(val.Text))
            {
                char[] temp = kopiaOrd.ToCharArray();
                char[] hitta = nuvarandeOrd.ToCharArray();
                char gissaBokstav = val.Text.ElementAt(0);
                for (int i = 0; i < hitta.Length; i++)
                {
                    if (hitta[i] == gissaBokstav)
                    {
                        temp[i] = gissaBokstav;
                    }
                }
                kopiaOrd = new string(temp);
                visaKopia();
            }
            else
            {
                felGissningar++;
                gissningar--;
                lblGissningar.Text = gissningar.ToString();
            }

            if (felGissningar < 8)
            {
                if (standardSkin)
                {
                    pbHänga.Image = liv[felGissningar];
                }
                else if (standardSkin == false)
                {
                    pbHänga.Image = liv_skin_andjen[felGissningar];
                }
            }
            else
            {
                lblOrdet.Text = "Gubben är hängd!";
                tbxTidigare.AppendText(nuvarandeOrd.ToString() + " " + "- Förlust" + "\n");
                gbxKnappar.Enabled = false;
                btnOmstart.Enabled = true;
                antalSpel++;
                btnExtraLiv.Enabled = true;
                btnLedtråd.Enabled = false;
                if (winningStreak >= 2)
                {
                        System.Media.SoundPlayer comboBreaker = new System.Media.SoundPlayer(Properties.Resources.Combo_breaker);
                        comboBreaker.Play();
                    winningStreak = 0;
                }
                else
                {
                    winningStreak = 0;
                }

            }
            if (kopiaOrd.Equals(nuvarandeOrd))
            {
                lblOrdet.Text = "Du gissade rätt!";
                gbxKnappar.Enabled = false;
                tbxTidigare.AppendText(nuvarandeOrd.ToString() + " " + "- Vinst" + "\n");
                btnOmstart.Enabled = true;
                btnLedtråd.Enabled = false;
                antalSpel++;
                vinster++;
                if (winningStreak == 2)
                {
                    System.Media.SoundPlayer hattrickSound = new System.Media.SoundPlayer(Properties.Resources.Hattrick);
                    hattrickSound.Play();
                }
                else
                {
                    winningStreak++;
                }
                if (lvl1)
                {
                    credits++;
                    lblCredits.Text = credits.ToString();
                }
                else if (lvl2)
                {
                    credits = credits + 2;
                    lblCredits.Text = credits.ToString();
                }
                else if (lvl3)
                {
                    credits = credits + 3;
                    lblCredits.Text = credits.ToString();
                }
            }
        }

        private void hänga_Gubbe_Load(object sender, EventArgs e)
        {
            string[] läs = File.ReadAllLines("ordlista.txt");
            ord = läs[slumpOrd.Next(1, 7)];
            väljOrd();
            indexLedtråd = 0;
            tbxLedtråd.Text = "";
            tbxTidigare.Text = "";
            gbxKnappar.Enabled = false;
            btnOmstart.Enabled = false;
            btnLedtråd.Enabled = false;
            btnExtraLiv.Enabled = false;
            lblCredits.Text = credits.ToString();
        }

        private void btnOmstart_Click(object sender, EventArgs e)
        {
            string[] läs = File.ReadAllLines("ordlista.txt");
            ord = läs[slumpOrd.Next(1, 7)];
            btnOmstart.Enabled = false;
            väljOrd();
            indexLedtråd = 0;
            tbxLedtråd.Text = "";
            lblGissningar.Text = "";
            btnLvl1.Enabled = true;
            btnLvl2.Enabled = true;
            btnLvl3.Enabled = true;
            gbxKnappar.Enabled = false;
            btnLedtråd.Enabled = false;
            lvl1 = false;
            lvl2 = false;
            lvl3 = false;
            winrate = (vinster / antalSpel) * 100;
            tbxWinrate.Text = winrate.ToString("#,##") + "%";
        }

        private void btnLedtråd_Click(object sender, EventArgs e)
        {
            
            string ledtråd = nuvarandeOrd;
            char ledtrådBokstav = ledtråd[indexLedtråd];
            tbxLedtråd.AppendText(ledtrådBokstav.ToString());
            indexLedtråd++;
            if (indexLedtråd == nuvarandeOrd.Length)
            {
                btnLedtråd.Enabled = false;
            }
        }

        private void btnLvl1_Click(object sender, EventArgs e)
        {
            string[] läs = File.ReadAllLines("ordlista.txt");
            ord = läs[slumpOrd.Next(1, 7)];
            EnableButtons();
            väljOrd();
            indexLedtråd = 0;
            tbxLedtråd.Text = "";
            felGissningar = 0;
            gissningar = 8;
            lblGissningar.Text = gissningar.ToString();
            gbxKnappar.Enabled = true;
            btnOmstart.Enabled = false;
            btnLvl1.Enabled = false;
            btnLvl2.Enabled = false;
            btnLvl3.Enabled = false;
            lvl1 = true;


        }

        private void btnLvl2_Click(object sender, EventArgs e)
        {

            string[] läs = File.ReadAllLines("ordlista.txt");
            ord = läs[slumpOrd.Next(1, 7)];
            EnableButtons();
            väljOrd();
            indexLedtråd = 0;
            btnLedtråd.Enabled = true;
            tbxLedtråd.Text = "";
            felGissningar = 2;
            if (standardSkin)
            {
                pbHänga.Image = liv[felGissningar];
            }
            else if (standardSkin == false)
            {
                pbHänga.Image = liv_skin_andjen[felGissningar];
            }
            gissningar = 6;
            lblGissningar.Text = gissningar.ToString();
            gbxKnappar.Enabled = true;
            btnOmstart.Enabled = false;
            btnLvl1.Enabled = false;
            btnLvl2.Enabled = false;
            btnLvl3.Enabled = false;
            lvl2 = true;
        }

        private void btnLvl3_Click(object sender, EventArgs e)
        {
            string[] läs = File.ReadAllLines("ordlista.txt");
            ord = läs[slumpOrd.Next(1, 7)];
            EnableButtons();
            väljOrd();
            indexLedtråd = 0;
            btnLedtråd.Enabled = true;
            tbxLedtråd.Text = "";
            felGissningar = 4;
            if (standardSkin)
            {
                pbHänga.Image = liv[felGissningar];
            }
            else if (standardSkin == false)
            {
                pbHänga.Image = liv_skin_andjen[felGissningar];
            }
            gissningar = 4;
            lblGissningar.Text = gissningar.ToString();
            gbxKnappar.Enabled = true;
            btnOmstart.Enabled = false;
            btnLvl1.Enabled = false;
            btnLvl2.Enabled = false;
            btnLvl3.Enabled = false;
            lvl3 = true;
        }

        private void btnAvsluta_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAndjen_Click(object sender, EventArgs e)
        {
            if (andjenUpplåst)
            {
                standardSkin = false;
                pbHänga.Image = liv_skin_andjen[felGissningar];
                btnAndjen.Enabled = false;
            }
            else if (credits >= 10)
            {
                credits = credits - 10;
                lblCredits.Text = credits.ToString();
                pbHänga.Image = liv_skin_andjen[felGissningar];
                standardSkin = false;
                andjenUpplåst = true;
                btnAndjen.Text = "Andjen: Upplåst";
                btnAndjen.Enabled = false;
            }
        }

        private void btnMosaik_Click(object sender, EventArgs e)
        {
            if (mosaikUpplåst)
            {
                BackgroundImage = bakgrund_mosaik;
                btnMosaik.Enabled = false;
            }

            else if (credits >= 20)
            {
                credits = credits - 20;
                lblCredits.Text = credits.ToString();
                BackgroundImage = bakgrund_mosaik;
                btnMosaik.Text = "Mosaik: Upplåst";
                btnMosaik.Enabled = false;
                mosaikUpplåst = true;
            }
        }

        private void btnDefaultBG_Click(object sender, EventArgs e)
        {
            BackgroundImage = default(Image);
            btnMosaik.Enabled = true;

            if (standardSkin == false)
            {
                pbHänga.Image = liv[felGissningar];
                btnAndjen.Enabled = true;
            }
        }

        private void btnExtraLiv_Click(object sender, EventArgs e)
        {
            if (gissningar < 8 && credits >= 5)
            {
                felGissningar--;
                gissningar++;
                credits = credits - 5;
                if (standardSkin)
                {
                    pbHänga.Image = liv[felGissningar];
                }
                else if (standardSkin == false)
                {
                    pbHänga.Image = liv_skin_andjen[felGissningar];
                }
            }
            lblGissningar.Text = gissningar.ToString();
            lblCredits.Text = credits.ToString();
            visaKopia();
            gbxKnappar.Enabled = true;
            if (indexLedtråd != nuvarandeOrd.Length)
            {
                btnLedtråd.Enabled = true;
            }
        }
    }
}
