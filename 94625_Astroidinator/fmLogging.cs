using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _94625_Astroidinator
{
    public partial class fmLogging : Form
    {
        int lineNr = 0;

        public fmLogging()
        {
            InitializeComponent();
        }

        private void initApplication()
        {
            this.Left = 800;
            this.Top = 100;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Text = "Serial Monitor";
            this.ControlBox = false; //switch off the control buttons in the title bar
            initApplication();
        }

        public void PrintLn(string a_text, string a_color)
        {
            string m_color;

            m_color = a_color.ToUpper();//eliminate a possible problem of the letter casing

            switch (a_color)
            {
                case "R": rtbSerialMonitor.SelectionColor = Color.Red; break;
                case "G": rtbSerialMonitor.SelectionColor = Color.Green; break;
                case "B": rtbSerialMonitor.SelectionColor = Color.Blue; break;
                default: rtbSerialMonitor.SelectionColor = Color.White; break;
            }

            if (cbxAutoScroll.Checked == true)
            {
                rtbSerialMonitor.ScrollToCaret();

            }

            if (cbxLineNumbering.Checked == true)
            {
                a_text = lineNr.ToString() + " > " + a_text;
            }

            rtbSerialMonitor.AppendText(a_text + "\n");

            lineNr++;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbSerialMonitor.Clear();
        }
    }
}
