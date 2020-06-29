using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab11._2
{
    public partial class Form1 : Form
    {
        public PointF gdevse;
        delegate void dvigenie(object sender, PaintEventArgs e, PointF position);
        delegate bool blizost(PointF obj1, PointF obj2, SizeF obj01, SizeF obj02, float delta);
        player vinni, pyatak, bee1, bee2, bee3;
        dvigenie dvig;
        Pen mypen = new Pen(Color.White);
        

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (vinni != null) { vinni.obl = pyatak.obl = bee1.obl = bee2.obl = bee3.obl = this.ClientSize;
                pyatak.startF.Y = pyatak.obl.Height - pyatak.razmerF.Height; }
            gdevse = new PointF(this.Width * 0.55F, this.Height * 0.35F);
        }        
        
        public Form1()
        {
            float speed = 5;
            mypen.Width = 10;

            InitializeComponent();
            

            vinni = new player(Properties.Resources.vinni, 50, 50, this.ClientSize, 250F, speed);
            pyatak = new player(Properties.Resources.pyatak, this.ClientSize.Width-150, this.ClientSize.Height - 150, this.ClientSize, 180F, speed, 1);
            pyatak.startF.Y = pyatak.obl.Height - pyatak.razmerF.Height;
            bee1 = new player(Properties.Resources.pchela1, 350, 340, this.ClientSize, 350F, speed*5, 2);
            bee2 = new player(Properties.Resources.pchela1, 380, 380, this.ClientSize, 350F, speed*5, 3);
            bee3 = new player(Properties.Resources.pchela1, 420, 350, this.ClientSize, 350F, speed*5, 4);
                        
            dvig = vinni.move;
            dvig += pyatak.move;
            dvig += bee1.move;
            dvig += bee2.move;
            dvig += bee3.move;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            dvig(this, e, gdevse);
            //this.Text = Convert.ToString(gdevse);
            //e.Graphics.DrawEllipse(mypen, gdevse.X,gdevse.Y,10,10);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }
    }   

    class player
    {
        //public static player[] players;
        private Image Kartinka;
        public PointF startF = new PointF(30,30);
        private int pora = 0, i = 0;
        public SizeF  obl, razmerF;
        private SizeF vektorF;
        private float speeed = 1;
        public player (Image pic, float startX, float startY, SizeF Oblast, float ugol, float skorost = 1.0F, int num = 0)
        {            
            obl = Oblast;
            vektorF = new SizeF();
            Kartinka = pic;
            razmerF = Kartinka.Size;
            speeed = skorost;
            vektorF.Height = (float)Math.Sin( - ugol * Math.PI / 180) * skorost;
            vektorF.Width = (float)Math.Cos( - ugol * Math.PI / 180) * skorost;
            startF.X = startX;
            startF.Y = startY;
            i = num;
        }

        private void otskok(PointF c)
        {            
            if (Math.Sqrt(Math.Pow(c.X - startF.X, 2) + Math.Pow(c.Y - startF.Y, 2)) < 70)
            {
                float a1, a2, c1, c2;
                a1 = vektorF.Width;
                a2 = vektorF.Height;
                c1 = c.X;
                c2 = c.Y;

                vektorF.Height = (- 2 * a1 * (c1 - startF.X) * (c2 - startF.Y) +
                    a2 * ((c1 - startF.X) * (c1 - startF.X) - (c2 - startF.Y) * (c2 - startF.Y))) /
                    ((c1 - startF.X) * (c1 - startF.X) + (c2 - startF.Y) * (c2 - startF.Y)); 

                vektorF.Width = (- 2 * a2 * (c1 - startF.X) * (c2 - startF.Y) -
                    a1 * ((c1 - startF.X) * (c1 - startF.X) - (c2 - startF.Y) * (c2 - startF.Y))) /
                    ((c1 - startF.X) * (c1 - startF.X) + (c2 - startF.Y) * (c2 - startF.Y));

                pora = 10;
                
            }
        }        

        public void povorot()
        {
            Kartinka.RotateFlip(RotateFlipType.RotateNoneFlipX);
        }

        public void move(object sender, PaintEventArgs e, PointF gdevse)
        {
            if(startF.X < 2 || startF.X > obl.Width - this.Kartinka.Width) { vektorF.Width *= -1; povorot(); }
            if(startF.Y < 2 || startF.Y > obl.Height - this.Kartinka.Height) { vektorF.Height *= -1; }

            if (pora == 0) { otskok(gdevse); } else { pora--; }
            startF = startF + vektorF;
           
            e.Graphics.DrawImage(Kartinka, startF);
        }        
    }
}
