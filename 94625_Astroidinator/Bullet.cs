using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace _94625_Astroidinator
{
    public class Bullet
    {
        public PictureBox Bulletpicture { get; private set; }
        private int speed;

        public Bullet(TabPage gameTab, int startX, int startY, int bulletSpeed)
        {
            this.speed = bulletSpeed;
            Bulletpicture = new PictureBox
            {
                Size = new Size(20, 10),
                BackColor = Color.Red,
                Top = startY,
                Left = startX,
            };
            gameTab.Controls.Add(Bulletpicture);
            Bulletpicture.BringToFront();
        }

        public void Move (TabPage gameTab, List<Bullet> bullets)
        {
            Bulletpicture.Left -= speed;
            if(Bulletpicture.Left < 0)
            {
                gameTab.Controls.Remove(Bulletpicture);
                bullets.Remove(this);
                Bulletpicture.Dispose();
            }
        }
    }
}
