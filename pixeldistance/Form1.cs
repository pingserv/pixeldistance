using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pixeldistance
{
    public partial class Form1 : Form
    {
        public PixelDistance pd;
        public Form1()
        {
            InitializeComponent();

            pixelEditor1.APBox = new PictureBox();
            pixelEditor1.APBox.Image = new Bitmap(780, 530);
            pixelEditor1.TgtBitmap = (Bitmap)pixelEditor1.APBox.Image;

            pd = new PixelDistance(pixelEditor1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void combobox1_Changed(object sender, EventArgs e)
        {
            Color color = (Color)comboBox1.SelectedValue;
            pixelEditor1.DrawColor = color;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pd.CalculatePoints(true);
            pd.RepaintMap(true);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (msg.Msg != 256)
                return false;

            Vector2 direction = new Vector2();

            switch (keyData)
            {
                case Keys.Up:
                    direction.Y--;
                    break;
                case Keys.Down:
                    direction.Y++;
                    break;
                case Keys.Left:
                    direction.X--;
                    break;
                case Keys.Right:
                    direction.X++;
                    break;
            }

            pd.MoveObserver(direction);

            return true;
            //return base.ProcessCmdKey(ref msg, keyData);

        }
    }
}