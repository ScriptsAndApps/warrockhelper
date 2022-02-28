using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace autoShooterr
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
        }
        public Color pc = Color.Red;
        private void MainForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)

        {

            //create a graphics object from the form

            Graphics g = this.CreateGraphics();

            // create  a  pen object with which to draw

            Pen p = new Pen(pc, 1);  // draw the line 

            // call a member of the graphics class

            g.DrawLine(p, 4, 4, 14,14);
            g.DrawLine(p, 4, 14, 14,4);

        }
        private void Form2_Load(object sender, EventArgs e)
        {
            this.Size = new Size(20,20);
           this.Location = new Point(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width/2 - this.Width / 2, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height/2 - this.Height / 2);
        }
    }
}
