using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace autoShooterr {
    public partial class Form1:Form {


        private int keyTolaggConstant = (int)Keys.K;
        private int keyTolaggShort = (int)Keys.V;
        private int keyTolaggLong = (int)Keys.C;
        private int keyTolaggSuperlong = (int)Keys.C;

        //some ajustable mechanics

        //trigger timing things
        private static int timeing = 700;
        private static int timeing2 = 100;

        //location to start with
        private int screencenterx = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2;
        private int screencentery = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2;
        private int centerimagex = 0; //adjust for different screens
        private int centerimgy = 0; //adjust for different screens
        private int detectionheight = 40; //gets RED name under trigger and shoots  //adjust for different screens

        //change to suite your network
        private string[ ] theiptolaggwith = new string[ ] { "192.168.178.2", };
        private string[ ] theoriginaldefaultgatewayip = new string [] {"192.168.178.1", };


        //variabls just make the overal program work
        private Thread th, tty;
        private static bool running = true, lagonnn = false;
        private int bx1 = 8, by1 = 20 - 2;
        private int scounter = 0;
        private int CC = 200;
        private Random retje = new Random();
        private static Thread bb = null;
        private static int lastindex = 100;
        private static bool sstop = false;
        private static Form2 f2 = new Form2();
        private static Thread t2 = null;
        private static bool lagshot = false;
        private static bool shortlagshot = true;



        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;


        [DllImport("user32.dll")] private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")] private static extern bool ReleaseCapture();
        [DllImport("user32.dll")] private static extern int GetAsyncKeyState(Int32 i);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(int dwFlags, uint dx, uint dy, int cButtons, int dwExtraInfo);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point) {
                return new Point(point.X, point.Y);
            }
        }

        // [DllImport("user32.dll")] private static extern int SetCursorPos(int x, int y);
        [DllImport("user32.dll")] private static extern bool GetCursorPos(out POINT lpPoint);
        private static Point GetCursorPosition() {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            return lpPoint;
        }

        private static void lagg(string[ ] ipp) {
            using ( var networkConfigMng = new ManagementClass("Win32_NetworkAdapterConfiguration") ) {
                using ( var networkConfigs = networkConfigMng.GetInstances() ) {
                    foreach ( var managementObject in networkConfigs.Cast<ManagementObject>().Where(managementObject => (bool)managementObject["IPEnabled"]) ) {

                        using ( var newIP = managementObject.GetMethodParameters("EnableStatic") ) {
                            using ( var newGateway = managementObject.GetMethodParameters("SetGateways") ) {
                                newGateway["DefaultIPGateway"] = ipp;
                                newGateway["GatewayCostMetric"] = new[ ] { 1 };
                                managementObject.InvokeMethod("SetGateways", newGateway, null);
                            }

                        }
                    }
                }
            }
        }

        public Form1()
        {
            lagg(theoriginaldefaultgatewayip);
            th = new Thread(() =>
            {
                while (running)
                {
                    for (int i = 0; i < 255; i++)
                    {
                        int keyState = GetAsyncKeyState(i);
                        //  32769 for windows 10. -32767 for windows 7.
                        if ( keyState == 1 || keyState == 32769 || keyState == -32767)
                        {
                            if (i == 1 || i == 2) { }
                            else
                            {
                                if (i == keyTolaggConstant)
                                {
                                    lagonnn = !lagonnn;
                                    Thread.Sleep(300);
                                }
                                if (i == keyTolaggShort)
                                {
                                   lagg(theiptolaggwith);
                                    Thread.Sleep(1800);
                                    lagg(theoriginaldefaultgatewayip);
                                    Thread.Sleep(50);

                                }
                                if (i == keyTolaggLong)
                                {
                             
                                    lagg(theiptolaggwith);
                                    Thread.Sleep(2300);
                                    lagg(theoriginaldefaultgatewayip);
                                    Thread.Sleep(400);
                                   
                                }
                                if (i == keyTolaggSuperlong)
                                {
                                
                                    lagg(theiptolaggwith);
                                    Thread.Sleep(1400);
                                    lagg(theoriginaldefaultgatewayip);
                                    Thread.Sleep(100);
                                    lagg(theiptolaggwith);
                                    Thread.Sleep(1400);
                                    lagg(theoriginaldefaultgatewayip);
                                    Thread.Sleep(100);

                                }
                            }
                        }
                    }
                }
            });
            th.Start();
            tty = new Thread(() =>
            {
                while (running)
                {
                    if (lagonnn)
                    {
                        lagg(theiptolaggwith);
                        Thread.Sleep(1000);
                        lagg(theoriginaldefaultgatewayip);
                        Thread.Sleep(600);
                    }
                }
                            
            });
            tty.Start();
            InitializeComponent();
        }
        Bitmap bs = new Bitmap(50, 40);
        Bitmap bs2 = new Bitmap(200, 200);
        private void timer1_Tick(object sender, EventArgs e)
        {
            if ( !at )
                return;
            if (lastindex >= 1)
            {
                if (f2 != null)
                {
               f2.pc = Color.Purple;
                    try
                    {
                        f2.Refresh();
                    }
                    catch { }
                }
                lastindex--;
            }
            else
            {
                if (f2 != null)
                {
                    f2.pc = Color.Green;
                    try
                    {
                        f2.Refresh();
                    }
                    catch { }
                }
            }

           

            //take the screenshot for pixels to detect for shooting
        
            try
            {
                Graphics graphics = Graphics.FromImage(bs as Image);
                graphics.CopyFromScreen((screencenterx-22)+centerimagex, (screencentery+ detectionheight) + centerimgy, 0, 0, bs.Size);
            }
            catch { }

            //take screenshot for showing user / debugging
            if ( debug ) {
                try {
                    Graphics graphics2 = Graphics.FromImage(bs2 as Image);
                    graphics2.CopyFromScreen(( screencenterx - 98 ) + centerimagex, screencentery - 61 + centerimgy, 0, 0, bs2.Size);
                } catch { }
            }

            // get detection pixel and detect..
            bool detected(int xx)
            {
                Color c1 = bs.GetPixel(bx1 + xx, 21);
                return (c1.R > 190 && c1.G < 50 && c1.B < 50);
            }
            // get detection pixel and detect.. second row
            bool detected2(int xx)
            {
                Color c1 = bs.GetPixel(bx1 + xx, 22);
                return (c1.R > 190 && c1.G < 50 && c1.B < 50);
            }
            int countDetectedPixels = 0;
            int countDetectedPixels2 = 0;
            this.Text = scounter.ToString();

            //draw on picture...

            for (int ii = 0; ii < 31; ii++)
            {
                if (detected(ii))
                {
                    //draw detected pixels
                    bs.SetPixel(bx1 + ii, by1 + 1, Color.Green);
                    countDetectedPixels++;
                }
                if (detected2(ii))
                {
                    //draw detected pixels
                    bs.SetPixel(bx1 + ii, by1 + 2, Color.Green);
                    countDetectedPixels2++;
                }
                //draw white line above detection
                bs.SetPixel(bx1 + ii, by1, Color.White);
            }
            if ( debug ) {
                //draw crosshair location dot
                bs2.SetPixel(97, 60, Color.Green);
                bs2.SetPixel(98, 60, Color.Green);
                bs2.SetPixel(97, 61, Color.Green);
                bs2.SetPixel(98, 61, Color.Green);

                //draw detection area red line
                int add = 20;
                bs2.SetPixel(94, 60 + detectionheight + add, Color.Red);
                bs2.SetPixel(95, 60 + detectionheight + add, Color.Red);
                bs2.SetPixel(96, 60 + detectionheight + add, Color.Red);
                bs2.SetPixel(97, 60 + detectionheight + add, Color.Red);
                bs2.SetPixel(98, 60 + detectionheight + add, Color.Red);
                bs2.SetPixel(99, 60 + detectionheight + add, Color.Red);
                bs2.SetPixel(100, 60 + detectionheight + add, Color.Red);
                bs2.SetPixel(101, 60 + detectionheight + add, Color.Red);
                bs2.SetPixel(102, 60 + detectionheight + add, Color.Red);
            }

            //see if there at least 4 pixels detected combined in both rows. and its not all red (so not a name)
            if (countDetectedPixels + countDetectedPixels2 > 3 && countDetectedPixels + countDetectedPixels2 <= 25)
            {
                //if detected enemy click to shoot
                scounter++;
                TopMost = false;
                click();
                Application.DoEvents();
                //show the screenshot used to trigger 
                if ( debug ) {
                    pictureBox1.Image = bs;
                     pictureBox2.Image = bs2;
                }
               //set counter to 0 so it will stay on the screen till timer is over or new screenshot..
                CC = 0;
                TopMost = true;

            }

            //Dont let the program slow down and output image not every second (needs async)
            if (CC++ >= 200)
            {
              //  pictureBox1.Image = bs;
               // pictureBox2.Image = bs2;
                CC = 100;
               // TopMost = false;
               // TopMost = true;
            }

           

        }

        private void click()
        {
            if (!(Control.MouseButtons == MouseButtons.Left))
            {
                try { if (bb != null) { bb.Abort(); } } catch { }
                    lagg(theoriginaldefaultgatewayip);
                if (shortlagshot || lagshot)
                {
                    if (lastindex <= 0)
                    {
                        bb = new Thread(() =>
                        {
                           // Disconnect();
                           lagg(theiptolaggwith);
                            if (lagshot) { 
                                     Thread.Sleep(retje.Next(300,360));
                            }
                            Thread.Sleep(retje.Next(400, 460));
                            //Connect();
                           lagg(theoriginaldefaultgatewayip);
                            /*  Thread.Sleep(retje.Next(100, 130));
                              lagg(theiptolaggwith);
                              Thread.Sleep(retje.Next(400, 460));
                              lagg(theoriginaldefaultgatewayip);*/
                        }); bb.Start();

                    }
                    lastindex = 70;
                }
                Thread.Sleep(retje.Next(3,5));
                if(lagshot) Thread.Sleep(retje.Next(280, 300));
                //down ?
                mouse_event(0x0002, (uint)GetCursorPosition().X * 65535, (uint)GetCursorPosition().Y * 65535, 0, 0);

                /*
                    Bitmap bs3 = new Bitmap(200, 200);
                    try
                    {
                        Graphics graphics2 = Graphics.FromImage(bs3 as Image);
                        graphics2.CopyFromScreen(~~~~~,~~~~~~~, 0, 0, bs3.Size);
                    }
                    catch { }
                     pictureBox2.Image = bs3;
                */
                Thread.Sleep(timeing);
                //up?
                mouse_event(0x0004, (uint)GetCursorPosition().X * 65535, (uint)GetCursorPosition().Y * 65535, 0, 0);
                Thread.Sleep(retje.Next(20,30));

                if (Control.MouseButtons == MouseButtons.Left)
                {
                    mouse_event(0x0002, (uint)GetCursorPosition().X * 65535, (uint)GetCursorPosition().Y * 65535, 0, 0);
                }
                Thread.Sleep(retje.Next(20, 30));
                if (!(Control.MouseButtons == MouseButtons.Left))
                {
                    mouse_event(0x0004, (uint)GetCursorPosition().X * 65535, (uint)GetCursorPosition().Y * 65535, 0, 0);
                }
                Thread.Sleep(timeing2+ retje.Next(5, 50));
            }
        }


        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            /*if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }*/
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            running = false;
            Application.Exit();
            this.Close();
        }
      
        private void trackBar1_Scroll(object sender, EventArgs e)
        {

            label1.Text = (((int)trackBar1.Value)*10).ToString();
            timeing = ((int)trackBar1.Value) * 10;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label2.Text = (((int)trackBar2.Value) * 10).ToString();
            timeing2 = ((int)trackBar2.Value) * 10;
        }

 
        private void button1_Click(object sender, EventArgs e)
        {
            if (sstop) {
                sstop = false;
                f2 = new Form2();
                t2.Abort();
            }
            else {
                sstop = true;
                t2 = new Thread(() =>
                  {
                      f2.Show();
                      Application.Run();
                });
                t2.IsBackground = true;
                t2.Start();
              }
        }

        private void Form1_Load(object sender, EventArgs e) {
                  int screenx = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            this.Location = new Point((screenx-Width)-20, 150);
    }
        bool at = true;
        private void checkBox3_CheckedChanged(object sender, EventArgs e) {
            at = checkBox3.Checked;
        }
        bool debug = false;
        private void checkBox4_CheckedChanged(object sender, EventArgs e) {
            debug = checkBox4.Checked;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)

        {
            running = false;
            lagg(theoriginaldefaultgatewayip);
            Application.Exit();
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            lagshot = checkBox1.Checked;


        }
        private void checkBox2_CheckStateChanged(object sender, EventArgs e)
        {
            shortlagshot = checkBox2.Checked;
        }

    

    }
}
