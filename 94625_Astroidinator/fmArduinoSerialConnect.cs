using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//for the Arduino Serial project
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _94625_Astroidinator
{
    public partial class fmArduinoSerialConnect : Form
    {
        fmLogging fmLoggingWindow = new fmLogging();

        private delegate void SafeCallDelegate();

        String receivedString = "";
        String receivedStringLast = "";

        public fmArduinoSerialConnect()
        {
            InitializeComponent();
            lblBulletFiredPfar.Text = "0";
            
        }

        private void fmArduinoSerialConnect_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName;
            Size DefaultFormSize = new Size(650, 400);

            tmrMain.Stop();

            this.Size = DefaultFormSize;

            InitTabs();

            serialPortArduinoConnection.DataReceived += serialPortArduinoConnection_DataReceived;
        }

        private void InitTabs()
        {
            //setup the tabcontrol that the tabs will not show, dirty trick but..
            tbcMain.Appearance = TabAppearance.FlatButtons;
            tbcMain.ItemSize = new Size(0, 1);
            tbcMain.SizeMode = TabSizeMode.Fixed;
            tbcMain.SelectedTab = tbpStartUpScreen;
        }

        #region Serial Handling and basics
        public void PrintLn(string a_text, string a_color)
        {
            string m_color;

            m_color = a_color.ToUpper();//eliminate a possible problem of the letter casing
            rtbLogging.BeginInvoke((MethodInvoker)delegate
            {
                switch (a_color.ToUpper())
                {
                    case "R": rtbLogging.SelectionColor = Color.Red; break;
                    case "G": rtbLogging.SelectionColor = Color.Green; break;
                    case "B": rtbLogging.SelectionColor = Color.Blue; break;
                    case "Y": rtbLogging.SelectionColor = Color.Orange; break;
                    default: rtbLogging.SelectionColor = Color.Black; break;
                }

                rtbLogging.AppendText(a_text + "\n");
                rtbLogging.ScrollToCaret();
            });
        }


        private void btnScanPortsDkal_Click(object sender, EventArgs e)
        {
            ScanComPortsDkal();
        }

        private void ScanComPortsDkal()
        {
            String[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);
            string m_portWithOutLastCharacter;

            if (serialPortArduinoConnection.IsOpen)
            {
                PrintLn("Connection was open. Closing..", "B");
                serialPortArduinoConnection.Close();
            }
            cbbSerialPortsDkal.Items.Clear();

            foreach (String port in ports)
            {
                if (cbSelectIfDangerShieldIsUsed.Checked == true)
                {
                    m_portWithOutLastCharacter = port.Substring(0, port.Length - 1);
                }
                else
                {
                    m_portWithOutLastCharacter = port;
                }

                cbbSerialPortsDkal.Items.Add(m_portWithOutLastCharacter);
                PrintLn("Found port:" + m_portWithOutLastCharacter.ToString(), "W");
            }

        }

        private void btnSerialPortOpenDkal_Click(object sender, EventArgs e)
        {
            if (!serialPortArduinoConnection.IsOpen)
            {
                try
                {
                    serialPortArduinoConnection.Open();

                    Thread.Sleep(200); //wait 100 ms to open port

                    this.Text = "Main - using com port: " + cbbSerialPortsDkal.Text;
                    PrintLn("Using com port: " + cbbSerialPortsDkal.Text, "W");
                }
                catch
                {
                    //MessageBox.Show("ERROR. Please make sure that the correct port was selected, and the device, plugged in.", "Serial port", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    PrintLn("ERROR: Please make sure that the correct port was selected, and the device, plugged in.", "R");
                }
            }
        }

        private void cbbSerialPortsDkal_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPortArduinoConnection.PortName = cbbSerialPortsDkal.Text;

            PrintLn("Port selected: " + serialPortArduinoConnection.PortName, "W");
            PrintLn("Default baudrate: " + serialPortArduinoConnection.BaudRate.ToString(), "W");

            cbbBaudRateDkal.Text = serialPortArduinoConnection.BaudRate.ToString();

        }

        private void cbbBaudRateDkal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbBaudRateDkal.Text != "select..")
            {
                serialPortArduinoConnection.BaudRate = Convert.ToInt32(cbbBaudRateDkal.Text);
                btnSerialPortOpenDkal.Enabled = true;
                PrintLn("Selected baudrate: " + serialPortArduinoConnection.BaudRate.ToString(), "W");
            }
            else
            {
                PrintLn("ERROR: Select the correct baudrate!", "R");
            }
        }

        public void WriteArduino(string a_action)
        {
            int m_length = a_action.Length;
            char[] m_data = new char[m_length];

            String m_carriageReturn = "\r";
            char[] m_cr = new char[2];

            String m_newLine = "\n";
            char[] m_nl = new char[2];

            for (int m_index = 0; m_index < m_length; m_index++)
            {
                m_data[m_index] = Convert.ToChar(a_action[m_index]);
            }

            for (int m_index = 0; m_index < 1; m_index++)
            {
                m_cr[m_index] = Convert.ToChar(m_carriageReturn[m_index]);
            }

            for (int m_index = 0; m_index < 1; m_index++)
            {
                m_nl[m_index] = Convert.ToChar(m_newLine[m_index]);
            }

            if (serialPortArduinoConnection.IsOpen == true)
            {
                serialPortArduinoConnection.Write(m_data, 0, m_length);

                if (cbxCarriageReturn.Checked == true)
                {
                    serialPortArduinoConnection.Write(m_cr, 0, 2);
                }

                if (cbxNewLine.Checked == true)
                {
                    serialPortArduinoConnection.Write(m_nl, 0, 2);
                }

                PrintLn("Transmitted message from Main: " + a_action, "Y");
            }
            else
            {
                //MessageBox.Show("ERROR. Please make sure that the correct port was selected, and the device, plugged in.", "Serial port", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PrintLn("ERROR. Please make sure that the correct port was selected, and the device, plugged in.", "R");
            }
        }
        private void serialPortArduinoConnection_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string receivedData = serialPortArduinoConnection.ReadLine().Trim(); // Lees de seriële data

                // Voorkom threading problemen
                this.Invoke((MethodInvoker)delegate
                {
                    ProcessArduinoInput(receivedData);
                    PrintLn(receivedData, "G");
                    receivedString = receivedData;
                });
            }
            catch (Exception ex)
            {
                PrintLn("ERROR: " + ex.Message, "R");
            }
        }


        public string ReadArduino()
        {
            UpdateReceivedString();

            return receivedString;
        }


        private void UpdateReceivedString()
        {
            if (receivedString != receivedStringLast)
            {
                PrintLn(receivedString, "G");
            }

            receivedStringLast = receivedString;
        }

        private void rtbLogging_DoubleClick(object sender, EventArgs e)
        {
            rtbLogging.Clear();
        }

        private void btClearInOutBuffer_Click(object sender, EventArgs e)
        {
            serialPortArduinoConnection.DiscardInBuffer();
            serialPortArduinoConnection.DiscardOutBuffer();

            PrintLn("IN and out buffers discarded", "B");
        }

        private void btnAboutDkal_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by : Dick van Kalsbeek\n" +
                            "Initial: 14aug2020\n" +
                            "Last update: 06mrt2024 by GEST\n" +
                            "Information: basic application to send to and receive from Arduino",
                            "About..", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnStartApplication_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpSerCom;
        }
        private void msiLogging_Click(object sender, EventArgs e)
        {
            if (msiLogging.Checked)
            {
                fmLoggingWindow.Show();
                msiLogging.Text = "Logging*";
            }
            else
            {
                fmLoggingWindow.Hide();
                msiLogging.Text = "Logging";
            }
            tmrMain.Stop();
            tmrMain.Stop();
        }

        private void msiScaQuit_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("Clicking OK will close the application. Since nothing is saved your work will be lost.",
                                                "Quit?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (DialogResult == DialogResult.OK)
            {
                Application.Exit();
            }
            tmrMain.Stop(); // Pauzeer de game-loop als een andere tab wordt geopend
            this.Height = 393;

        }

        private void msiViewTestGround_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpTestGround;
            tmrMain.Stop();
            Size DefaultFormSize = new Size(650, 400);
        }

        private void msiViewCustom_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpCustomApplication;
            Size CustomGameSize = new Size(650, 500);
            this.Size = CustomGameSize;

        }

        private void msiHelp_Click(object sender, EventArgs e)
        {
            tmrMain.Stop();
        }

        private void msiQuickGuide_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpQuickGuide;
            Size DefaultFormSize = new Size(650, 400);
        }

        private void msiViewSerialCommunication_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tbpSerCom;
            Size DefaultFormSize = new Size(650, 400);
        }
        #endregion

        //THIS WAY THE REGION BOUNDARIES CAN BE FOUND EASIER

        #region Test Ground

        private void btnWrite_Click(object sender, EventArgs e)
        {
            ExecuteWriteAfterEnter();
        }

        private void ExecuteWriteAfterEnter()
        {
            WriteArduino(txbWriteCustom.Text);
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            lblReturned.Text = ReadArduino();
        }


        private void btnWriteDefault_Click(object sender, EventArgs e)
        {
            String m_testText = "Test";

            WriteArduino(m_testText);
            lblSent.Text = m_testText;
        }

        private void btnResetArduino_Click(object sender, EventArgs e)
        {
            WriteArduino("Reset");
        }

        private void txbWriteCustom_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ExecuteWriteAfterEnter();
            }
        }




        #endregion

        //THIS WAY THE REGION BOUNDARIES CAN BE FOUND EASIER

        #region Custom

        // receive data from the arduino
        private void ProcessArduinoInput(string input)
        {
            movingUp = false;
            movingDown = false;
            movingLeft = false;
            movingRight = false;

            switch (input)
            {
                case "W": movingUp = true; break;
                case "S": movingDown = true; break;
                case "D": movingRight = true; break;
                case "A": movingLeft = true; break;
            }
        }

        // Flak settings
        private int moveSpeed = 5;
        private bool movingUp = false;
        private bool movingDown = false;
        private bool movingLeft = false;
        private bool movingRight = false;

        // Int declaration
        private int EnemyCountShot = 0;
        private List<Bullet> bullets = new List<Bullet>();
        private int bulletSpeed = 10;
        private List<Enemy> enemies = new List<Enemy>();
        private Random random = new Random();
        private int playerLives = 3;

        private void tmrMain_Tick(object sender, EventArgs e)
        {
            MovePicture();
            MoveBullets();
            SpawnEnemies();
            MoveEnemies();
        }

        // Movement
        private void fmArduinoSerialConnect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                movingUp = true;
            }
            if (e.KeyCode == Keys.S)
            {
                movingDown = true;
            }
            if (e.KeyCode == Keys.D)
            {
                movingRight = true;
            }
            if (e.KeyCode == Keys.A)
            {
                movingLeft = true;
            }
           
        }
        private void fmArduinoSerialConnect_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.W)
            {
                movingUp = false;
            }
            if(e.KeyCode == Keys.S)
            {
                movingDown = false;
            }
            if (e.KeyCode == Keys.D)
            {
                movingRight = false;
            }
            if (e.KeyCode == Keys.A)
            {
                movingLeft = false;
            }
        }
        private void MovePicture()
        {
            if(movingUp && pbxFlakPfar.Top > 0)
            {
                    pbxFlakPfar.Top -= moveSpeed;
            }
            if(movingDown && pbxFlakPfar.Top + pbxFlakPfar.Height < tbpCustomApplication.Height)
            {
                pbxFlakPfar.Top += moveSpeed;
            }
            if (movingRight && pbxFlakPfar.Left + pbxFlakPfar.Width < tbpCustomApplication.Width)
            {
                pbxFlakPfar.Left += moveSpeed;
            }
            if (movingLeft && pbxFlakPfar.Left > 0)
            {
                pbxFlakPfar.Left -= moveSpeed;
            }
        }

        // Bullets settings

        private void MoveBullets()
        {
            try
            {
                int i = bullets.Count - 1;
                while (i >= 0)
                {
                    bullets[i].Move(tbpCustomApplication, bullets);

                    // Delete the bullet when its off the screen
                    if (bullets[i].Bulletpicture.Left < 0)
                    {
                        bullets[i].Bulletpicture.Dispose();
                        bullets.RemoveAt(i);
                    }
                    else
                    {
                       int j = enemies.Count - 1;
                        while (j >= 0)
                        {
                            if (bullets[i].Bulletpicture.Bounds.IntersectsWith(enemies[j].EnemyPicture.Bounds))
                            {
                                // Remove the enemy and bullet by collision
                                enemies[j].Remove(this);
                                enemies.RemoveAt(j);

                                bullets[i].Bulletpicture.Dispose();
                                bullets.RemoveAt(i);
                                i--;
                                continue; // Stop after the first hit
                            }
                            j--;
                        }
                    }
                    i--;
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                // Handle the exception
                Console.WriteLine("IndexOutOfRangeException caught in MoveBullets: " + ex.Message);
                // You can also add additional error handling or logging here
            }
            catch (Exception ex)
            {
                // Catch any other unexpected exceptions
                Console.WriteLine("Unexpected exception caught in MoveBullets: " + ex.Message);
                // You can also add additional error handling or logging here
            }
        }

        private void ShootBullet()
        {
            Bullet bullet = new Bullet(tbpCustomApplication, pbxFlakPfar.Left, pbxFlakPfar.Top + pbxFlakPfar.Height / 4, bulletSpeed);
            bullets.Add(bullet);

            SoundPlayer player = new SoundPlayer("C:\\Users\\patri\\OneDrive\\Documenten\\APPR\\94625_Astroidinator\\Pew Sound Effect [JZAkxE-eR88].wav");
            player.Play();

            int bulletCount;
            if (int.TryParse(lblBulletFiredPfar.Text, out bulletCount))
            {
                lblBulletFiredPfar.Text = (bulletCount + 1).ToString();
            }
            else
            {
                lblBulletFiredPfar.Text = "1";
            }
        }

        private void fmArduinoSerialConnect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == ' ')
            {
                ShootBullet();
            }
        }

        // Enemy settings
        
        private void SpawnEnemies()
        {
            while(enemies.Count < 10) // Keep spawning enemies until there are 10
            {
                int randomY = random.Next(50, this.Height - 100); // Random Height
                int randomX = random.Next(-200, -40);
                int type = random.Next(3);
               
                Enemy newEnemy;

                switch (type)
                {
                        case 0: 
                        newEnemy = new Chuck(tbpCustomApplication, randomY, randomX);
                        break;
                        case 1:
                        newEnemy = new Terence (tbpCustomApplication, randomY, randomX);
                        break;
                        default:
                        newEnemy = new Enemy(tbpCustomApplication, randomY, randomX, 2);
                        break;

                }

                enemies.Add(newEnemy);
            }
        }
        private void MoveEnemies()
        {
            foreach (Enemy enemy in enemies.ToList())
            {
                enemy.Move();

                if (enemy.EnemyPicture.Bounds.IntersectsWith(pbxFlakPfar.Bounds))
                {
                    loseLife();
                    enemy.Remove(this);
                    enemies.Remove(enemy);

                    return;
                }

                if (enemy.IsOffScreen(this.Width))
                {
                    enemy.Remove(this);
                    enemies.Remove(enemy);
                    SpawnEnemies();
                }
                DetectBulletCollision();
            }
        }

        // Player Live
        
        private void loseLife()
        {
            playerLives--;

            switch (playerLives)
            {
                case 0:
                    GameOver();
                    pbxPlayerLive3.BackColor = Color.Gray;
                    break;
                case 1:
                    RespawnPlayer();
                    pbxPlayerLive2.BackColor = Color.Gray;
                    break;
                case 2:
                    RespawnPlayer();
                    pbxPlayerLive1.BackColor = Color.Gray;
                    break;
                default:
                    break;
            }
        }
        private void RespawnPlayer()
        {
            pbxFlakPfar.Left = tbpCustomApplication.Width - 100;
            pbxFlakPfar.Top = tbpCustomApplication.Height / 2;

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (enemies[i].EnemyPicture.Left >= pbxFlakPfar.Left - 100)
                {
                    enemies[i].Remove(this);
                    enemies.RemoveAt(i);
                }
            }
        }

        //Collision
        
        private void DetectBulletCollision()
        {
            for(int i = bullets.Count - 1; i >= 0; i--)
            {
                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    if (bullets[i].Bulletpicture.Bounds.IntersectsWith(enemies[j].EnemyPicture.Bounds))
                    {
                        // Remove the enemy and bullet by collision
                        enemies[j].Remove(this);
                        enemies.RemoveAt(j);

                        EnemyCountShot++;
                        lblEnemyShotPfar.Text =  EnemyCountShot.ToString();

                        bullets[i].Bulletpicture.Dispose();
                        bullets.RemoveAt(i);

                        break; // Stop after the first hit
                    }
                }
            }
        }
        
       
        // End Game
        private void GameOver()
        {
            tmrMain.Stop();
            MessageBox.Show("Game Over!", "Game Over", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
            if(DialogResult == DialogResult.Retry)
            {
                Application.Restart();
            }
        }

        // Buttons
        private void btnStartPfar_Click(object sender, EventArgs e)
        {
            tmrMain.Start();
            MovePicture();
            MoveBullets();
            SpawnEnemies();
            MoveEnemies();
            
        }

        private void btnResetPfar_Click(object sender, EventArgs e)
        {
            tmrMain.Stop();
            ResetGame();
            pbxPlayerLive1.BackColor = Color.Green;
            pbxPlayerLive2.BackColor = Color.Green;
            pbxPlayerLive3.BackColor = Color.Green;
            
        }
        private void ResetGame()
        {
            playerLives = 3;
            RespawnPlayer();

            foreach(var bullet in bullets)
            {
                bullet.Bulletpicture.Dispose();
            }
            bullets.Clear();

            foreach(var enemy in enemies)
            {
                enemy.Remove(this);
            }
            enemies.Clear();

            lblAccuracyPfar.Text = "0%";
            lblBulletFiredPfar.Text = "0";
            lblEnemyShotPfar.Text = "0";
        }

        private void btnChangeColorPfar_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                pbxFlakPfar.BackColor = colorDialog.Color;
            }
        }


        #endregion

        //THIS WAY THE REGION BOUNDARIES CAN BE FOUND EASIER

    }
}
