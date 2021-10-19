using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pixeldistance
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            pixelEditor1.APBox = new PictureBox();
            pixelEditor1.APBox.Image = new Bitmap(200, 100);
            pixelEditor1.TgtBitmap = (Bitmap)pixelEditor1.APBox.Image;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pixelEditor1_Click(object sender, EventArgs e)
        {

        }

        private void combobox1_Changed(object sender, EventArgs e)
        {
            Color color = (Color)comboBox1.SelectedValue;
            pixelEditor1.DrawColor = color;
        }
    }
}
