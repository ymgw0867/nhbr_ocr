using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHBR_OCR.common;

namespace NHBR_OCR.OCR
{
    public partial class frmUserCode : Form
    {
        public frmUserCode()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmUserCode_Load(object sender, EventArgs e)
        {
            Utility.WindowsMaxSize(this, this.Width, this.Height);
            Utility.WindowsMinSize(this, this.Width, this.Height);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            myCode = string.Empty;

            this.Close();
        }

        public string myCode { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("担当者コードを入力してください","確認",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                textBox1.Focus();
                return;
            }

            myCode = textBox1.Text.Trim();

            this.Close();
        }
    }
}
