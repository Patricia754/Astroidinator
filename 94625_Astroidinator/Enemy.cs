using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;

namespace _94625_Astroidinator
{
    public class Chuck : Enemy
    {
        public Chuck(TabPage gameTab, int startY, int startX) : base(gameTab, startY, startX, 5) 
        {
            this.health = 1;
            EnemyPicture.Image = Image.FromFile("C:\\Users\\patri\\OneDrive\\Documenten\\APPR\\94625_Astroidinator\\Chuck.png");
        }

        public override void Move()
        {
            EnemyPicture.Left += speed + 2;
        }
        public override void TakeDamage()
        {
            health--;
        }
    }

    public class Terence : Enemy
    {
        public Terence(TabPage gameTab, int startY, int startX) : base(gameTab, startY, startX, 5) 
        {
            this.health = 5;
            Size enemyPicture = new Size(50, 50);
            EnemyPicture.Image = Image.FromFile("C:\\Users\\patri\\OneDrive\\Documenten\\APPR\\94625_Astroidinator\\Terence.png");
        }
        

    public override void TakeDamage()
        {
                health--;
        }

    }

    public class Enemy
    {
        
        public PictureBox EnemyPicture { get; private set; }

        protected int speed;
        protected int health;

        public Enemy(TabPage gameTab, int startY, int startX, int speed)
        {
            this.health = 1;
            this.speed = speed;

            EnemyPicture = new PictureBox
            {
                Size = new Size(20, 20),
                Image = Image.FromFile("C:\\Users\\patri\\OneDrive\\Documenten\\APPR\\94625_Astroidinator\\Red.png"),
                Top = startY,
                Left = startX,
            };
            gameTab.Controls.Add(EnemyPicture);
            EnemyPicture.BringToFront();
        }
        public virtual void Move()
        {
           EnemyPicture.Left += speed;
        } 
       
        public bool IsOffScreen(int screenWidth)
        {
            return EnemyPicture.Left > screenWidth;
        }

        public void Remove(Form form)
        {
            form.Controls.Remove(EnemyPicture);
            EnemyPicture.Dispose();
        }

        public virtual void TakeDamage()
        {
            health--;
        }
        

        
     
      
    }
}
