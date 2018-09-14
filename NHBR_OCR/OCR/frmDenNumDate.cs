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
    public partial class frmDenNumDate : Form
    {
        public frmDenNumDate()
        {
            InitializeComponent();

            // 環境設定読み込み
            cAdp.Fill(dts.環境設定);
        }

        private void frmDenNumDate_Load(object sender, EventArgs e)
        {
            // フォーム最大値
            Utility.WindowsMaxSize(this, this.Width, this.Height);

            // フォーム最小値
            Utility.WindowsMinSize(this, this.Width, this.Height);

            // 楽商データ作成先パス取得
            var ss = dts.環境設定.Single(a => a.ID == 1);

            if (ss.Is当日日付Null())
            {
                txtNum.Text = DateTime.Today.Day.ToString();
            }
            else
            {
                txtNum.Text = ss.当日日付.Day.ToString();
            }

            txtNum.SelectAll();
        }

        NHBRDataSetTableAdapters.環境設定TableAdapter cAdp = new NHBRDataSetTableAdapters.環境設定TableAdapter();

        // データセットオブジェクト
        NHBRDataSet dts = new NHBRDataSet();

        private void button1_Click(object sender, EventArgs e)
        {
            if (numUpdate())
            {
                this.Close();
            }
        }

        private bool numUpdate()
        {
            try
            {
                DateTime dt01 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                int maxDay = dt01.AddMonths(1).AddDays(-1).Day;

                int sNum = Utility.StrtoInt(txtNum.Text);
                if (sNum < 1 || sNum > maxDay)
                {
                    MessageBox.Show("日付が正しくありません", "社内伝票番号日付", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtNum.Focus();
                    txtNum.SelectAll();
                    return false;
                }

                if (MessageBox.Show("社内伝票番号日付を " + txtNum.Text + " とします。よろしいですか。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return false;
                }

                DateTime denDt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, sNum);

                // 楽商データ作成先パス取得
                var ss = dts.環境設定.Single(a => a.ID == 1);

                if (ss.Is当日日付Null())
                {
                    ss.当日日付 = denDt;
                    ss.連番 = 0;
                }
                else if (DateTime.Parse(ss.当日日付.ToShortDateString()) != denDt)
                {
                    ss.当日日付 = denDt;
                    ss.連番 = 0;
                }

                ss.更新年月日 = DateTime.Now;
                cAdp.Update(dts.環境設定);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDenNumDate_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void txtNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '\t')
                e.Handled = true;
        }
    }
}
