using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data.OleDb;
using NHBR_OCR.Pattern;
using NHBR_OCR.OCR;
using NHBR_OCR.common;
using NHBR_OCR.config;

namespace NHBR_OCR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            timer1.Tick += new EventHandler(timer1_Tick);

            //// 大瓶商品コード、特瓶商品コードを文字数８桁に変更 2018/02/21
            //mdbAlter();
        }

        Timer timer1 = new Timer();

        private void mdbAlter()
        {
            // データベース接続文字列
            StringBuilder sb = new StringBuilder();
            OleDbCommand cm = new OleDbCommand();
            OleDbConnection Cn = new OleDbConnection();

            Cn.ConnectionString = Properties.Settings.Default.NHBRConnectionString;
            Cn.Open();

            cm.Connection = Cn;

            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件1C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件2C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件3C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件4C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件5C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件6C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件7C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件8C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件9C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件10C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件11C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件12C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件13C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件14C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件15C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件16C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件17C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件18C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件19C TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 大瓶条件20C TEXT(8)";
            cm.ExecuteNonQuery();

            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品1D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品2D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品3D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品4D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品5D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品6D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品7D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品8D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品9D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品10D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品11D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品12D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品13D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品14D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品15D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品16D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品17D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品18D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品19D TEXT(8)";
            cm.ExecuteNonQuery();
            cm.CommandText = "ALTER TABLE 出荷基準設定 ALTER COLUMN 特瓶商品20D TEXT(8)";
            cm.ExecuteNonQuery();

            Cn.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {            
            timer1.Enabled = false;
            dCountShow();   // 再FAX件数表示
            timer1.Enabled = true;
        }

        ///--------------------------------------------------
        /// <summary>
        ///     再FAX件数表示 </summary>
        ///--------------------------------------------------
        private void dCountShow()
        {
            try
            {
                // 再FAX件数取得
                int reFaxCnt = System.IO.Directory.GetFiles(Properties.Settings.Default.reFaxPath, "*.tif").Count();

                if (reFaxCnt > 0)
                {
                    toolStripStatusLabel1.Text = "再FAX注文書受信件数 ： " + reFaxCnt;
                }
                else
                {
                    toolStripStatusLabel1.Text = string.Empty;
                }

                // NG件数取得
                int ngCnt = System.IO.Directory.GetFiles(Properties.Settings.Default.ngPath, "*.tif").Count();

                if (ngCnt > 0)
                {
                    button5.Enabled = true;
                    button5.Text = "ＮＧ画像確認 " + "(" + ngCnt + ") (&N)";
                }
                else
                {

                    button5.Enabled = false;
                    button5.Text = "ＮＧ画像なし";
                }

                // OCR認証件数取得
                int ocrCnt = System.IO.Directory.GetFiles(Properties.Settings.Default.dataPath, "*.tif").Count();

                button4.Enabled = true;
                button4.Text = "発注書データ作成 (" + ocrCnt + ") (&D)";
            }
            catch (Exception)
            {
               // 何もしない
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //int cnt = 0;
            //this.textBox1.Text = "";
            //using (var Conn = new OracleConnection())
            //{
            //    Conn.ConnectionString = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
            //    Conn.Open();
            //    string strSQL = "select KOK_ID,NOU_RYAKUSYO from RAKUSYO_FAXOCR.V_NOUHINSAKI where KOK_ID = '331213'";
            //    OracleCommand Cmd = new OracleCommand(strSQL, Conn);
            //    OracleDataReader Reader = Cmd.ExecuteReader();
            //    while (Reader.Read())
            //    {
            //        this.textBox1.Text = Reader.GetString(0) + ":" + Reader.GetString(1);
                    
            //        cnt++;
            //    }

            //    MessageBox.Show(cnt + "件を読み出しました！");

            //    Reader.Dispose();
            //    Cmd.Dispose();
            //}
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Close();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 後片付け
            Dispose();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F12)
            {
                Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Utility.WindowsMaxSize(this, Width, Height);
            Utility.WindowsMinSize(this, Width, Height);

            // 再FAX件数表示
            dCountShow();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkLabel5_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmPtnAdd frm = new frmPtnAdd();
            frm.ShowDialog();
            Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmPrnPtn frm = new frmPrnPtn();
            frm.ShowDialog();
            Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NHBR_CLIDataSet dtsC = new NHBR_CLIDataSet();
            NHBR_CLIDataSetTableAdapters.FAX注文書TableAdapter adp = new NHBR_CLIDataSetTableAdapters.FAX注文書TableAdapter();
            NHBR_CLIDataSetTableAdapters.保留注文書TableAdapter hAdp = new NHBR_CLIDataSetTableAdapters.保留注文書TableAdapter();

            // データ読み込み
            adp.Fill(dtsC.FAX注文書);
            hAdp.Fill(dtsC.保留注文書);

            // 自らのロックファイルを削除する
            Utility.deleteLockFile(Properties.Settings.Default.dataPath, Properties.Settings.Default.lockFileName);

            //他のPCで処理中の場合、続行不可
            if (Utility.existsLockFile(Properties.Settings.Default.dataPath))
            {
                MessageBox.Show("他のPCで処理中です。しばらくおまちください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // タイマー監視受信したＦＡＸ発注書件数
            int s = System.IO.Directory.GetFiles(Properties.Settings.Default.dataPath, "*.tif").Count();

            // 処理中の注文書データ
            int d = dtsC.FAX注文書.Count();

            // 保留中データ
            int h = dtsC.保留注文書.Count();
            
            // 処理可能なデータが存在するか？
            if (s == 0 && d == 0 && h == 0)
            {
                MessageBox.Show("現在、処理可能なＦＡＸ注文書データはありません", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //LOCKファイル作成
            Utility.makeLockFile(Properties.Settings.Default.dataPath, Properties.Settings.Default.lockFileName);

            this.Hide();

            // 担当者コードを入力
            frmUserCode frmCode = new frmUserCode();
            frmCode.ShowDialog();

            string _myCode = frmCode.myCode;
            frmCode.Dispose();

            if (_myCode == string.Empty)
            {
                // ロックファイルを削除する
                Utility.deleteLockFile(Properties.Settings.Default.dataPath, Properties.Settings.Default.lockFileName);

                // 処理をキャンセル
                Show();
            }
            else
            {
                // 処理するデータを取得
                frmFaxSelect frmFax = new frmFaxSelect();
                frmFax.ShowDialog();

                int _myCnt = frmFax.myCnt;
                bool _myBool = frmFax.myBool;
                frmFax.Dispose();

                // ロックファイルを削除する
                Utility.deleteLockFile(Properties.Settings.Default.dataPath, Properties.Settings.Default.lockFileName);

                if (!_myBool)
                {
                    Show();
                }
                else
                {
                    // データ作成処理へ
                    frmCorrect frm = new frmCorrect(_myCode.ToString());
                    frm.ShowDialog();
                }
            }

            Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 自らのロックファイルを削除する
            Utility.deleteLockFile(Properties.Settings.Default.ngPath, Properties.Settings.Default.lockFileName);

            //他のPCで処理中の場合、続行不可
            if (Utility.existsLockFile(Properties.Settings.Default.ngPath))
            {
                MessageBox.Show("他のPCで処理中です。しばらくおまちください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            //LOCKファイル作成
            Utility.makeLockFile(Properties.Settings.Default.ngPath, Properties.Settings.Default.lockFileName);

            // NG画像確認画面表示
            this.Hide();
            frmNgRecovery frm = new frmNgRecovery();
            frm.ShowDialog();

            // ロックファイルを削除する
            Utility.deleteLockFile(Properties.Settings.Default.ngPath, Properties.Settings.Default.lockFileName);

            // NG件数更新 : 2017/11/18
            dCountShow();

            Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmConfig frm = new frmConfig();
            frm.ShowDialog();
            Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmShukkaKIjun frm = new frmShukkaKIjun();
            frm.ShowDialog();
            Show();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            timer1.Interval = Properties.Settings.Default.menuTimeSpan * 1000;
            timer1.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            master.frmCamMst frm = new master.frmCamMst();
            frm.ShowDialog();
            Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // キャンペーン画像確認
            if (System.IO.Directory.GetFiles(Properties.Settings.Default.campaignPath, "*.tif").Count() == 0 &&
                System.IO.Directory.GetFiles(Properties.Settings.Default.camPaignWorhPath, "*.tif").Count() == 0)
            {
                MessageBox.Show("キャンペーンリカバリーデータがありません", "キャンペーン発注処理", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            // キャンペーン発注書登録処理
            this.Hide();

            // 担当者コードを入力
            frmUserCode frmCode = new frmUserCode();
            frmCode.ShowDialog();

            string _myCode = frmCode.myCode;
            frmCode.Dispose();

            if (_myCode != string.Empty)
            {
                frmCanpaignRec frm = new frmCanpaignRec(_myCode);
                frm.ShowDialog();
            }

            Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Hide();
            OCR.frmDenNumDate frm = new frmDenNumDate();
            frm.ShowDialog();
            Show();
        }
    }
}
