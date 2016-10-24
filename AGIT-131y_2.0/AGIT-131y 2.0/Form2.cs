using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AGIT_131y_2._0
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            timer1.Enabled = true;
            textBox1.Text = "0";
        }
        ColorDialog colorDialog1 = new ColorDialog();
        FontDialog fontDialog1 = new FontDialog();
        
        private void Form2_Load(object sender, EventArgs e)
        {
           

            label1.Font = Properties.Settings.Default.Font1;
            label2.Font = Properties.Settings.Default.Font2;
            label3.Font = Properties.Settings.Default.Font3;
            label1.ForeColor = Properties.Settings.Default.Color1;
            label2.ForeColor = Properties.Settings.Default.Color2;
            label3.ForeColor = Properties.Settings.Default.Color3;
            textBox1.Text = Properties.Settings.Default.timeout.ToString();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Font1 = label1.Font;
            Properties.Settings.Default.Font2 = label2.Font;
            Properties.Settings.Default.Font3 = label3.Font;
            Properties.Settings.Default.Color1 = label1.ForeColor;
            Properties.Settings.Default.Color2 = label2.ForeColor;
            Properties.Settings.Default.Color3 = label3.ForeColor;
            Properties.Settings.Default.timeout = Convert.ToInt32(textBox1.Text);
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            label1.Font = Properties.Settings.Default.fnt1;
            label2.Font = Properties.Settings.Default.fnt2;
            label3.Font = Properties.Settings.Default.fnt3;
            label1.ForeColor = Properties.Settings.Default.clr1;
            label2.ForeColor = Properties.Settings.Default.clr2;
            label3.ForeColor = Properties.Settings.Default.clr3;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = label1.Font;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                label1.Font = fontDialog1.Font;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                label1.ForeColor = colorDialog1.Color;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = label2.Font;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                label2.Font = fontDialog1.Font;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                label2.ForeColor = colorDialog1.Color;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = label3.Font;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                label3.Font = fontDialog1.Font;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                label3.ForeColor = colorDialog1.Color;
            }
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            textBox1.Text = hScrollBar1.Value.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = "0";
            hScrollBar1.Value = Convert.ToInt32(textBox1.Text);
           
        }
    }
}
