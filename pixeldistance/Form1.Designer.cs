
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace pixeldistance
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pixelEditor1 = new pixeldistance.PixelEditor();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pixelEditor1)).BeginInit();
            this.SuspendLayout();
            // 
            // pixelEditor1
            // 
            this.pixelEditor1.APBox = null;
            this.pixelEditor1.BackColor = System.Drawing.Color.White;
            this.pixelEditor1.DrawColor = System.Drawing.Color.Red;
            this.pixelEditor1.GridColor = System.Drawing.Color.DimGray;
            this.pixelEditor1.Location = new System.Drawing.Point(230, 0);
            this.pixelEditor1.Name = "pixelEditor1";
            this.pixelEditor1.PixelSize = 10;
            this.pixelEditor1.Size = new System.Drawing.Size(780, 530);
            this.pixelEditor1.TabIndex = 1;
            this.pixelEditor1.TabStop = false;
            this.pixelEditor1.Text = "pixelEditor1";
            this.pixelEditor1.TgtBitmap = null;
            this.pixelEditor1.TgtMousePos = new System.Drawing.Point(0, 0);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.DisplayMember = "Key";
            this.comboBox1.ValueMember = "Value";

            BindingSource source = new BindingSource();
            source.DataSource = typeof(KeyValuePair<string, Color>);
            source.Add(new KeyValuePair<string, Color>("Falpont", Color.Red));
            source.Add(new KeyValuePair<string, Color>("Padlópont", Color.LightBlue));
            source.Add(new KeyValuePair<string, Color>("Kontúrpont", Color.Yellow));
            source.Add(new KeyValuePair<string, Color>("Ismeretlen", Color.LightGray));
            source.Add(new KeyValuePair<string, Color>("Törlés", Color.White));
            this.comboBox1.DataSource = source;

            this.comboBox1.Location = new System.Drawing.Point(20, 20);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(192, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedText = "Falpont";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.combobox1_Changed);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(19, 53);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Pontok számítása";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "A megfigyelőt (fekete pont) a billentyűzet\nnyilaival lehet irányítani.";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 531);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.pixelEditor1);
            this.Name = "Form1";
            this.Text = "Pixel distance";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pixelEditor1)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
        private PixelEditor pixelEditor1;
        private System.Windows.Forms.ComboBox comboBox1;
        private Button button1;
        private Label label1;
    }
}

