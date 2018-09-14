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

namespace NHBR_OCR.common
{
    public partial class frmPtnCall : Form
    {
        public frmPtnCall()
        {
            InitializeComponent();

            adp.Fill(dts.パターンID);
        }

        NHBRDataSet dts = new NHBRDataSet();
        NHBRDataSetTableAdapters.パターンIDTableAdapter adp = new NHBRDataSetTableAdapters.パターンIDTableAdapter();

        clsVNouhin[] vn = null;

        // カラム定義
        private string colNouCode = "c0";
        private string colNouName = "c1";
        private string colTel = "c2";
        private string colZip = "c3";
        private string colAddress = "c4";
        private string colPtnID = "c5";
        private string colDate = "c6";
        private string colID = "c7";
        private string colMemo = "c8";

        /// <summary>
        /// データグリッドビューの定義を行います
        /// </summary>
        private void GridviewSet(DataGridView tempDGV)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                //フォームサイズ定義

                // 列スタイルを変更する

                tempDGV.EnableHeadersVisualStyles = false;
                tempDGV.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                tempDGV.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                tempDGV.EnableHeadersVisualStyles = false;

                // 列ヘッダー表示位置指定
                tempDGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 列ヘッダーフォント指定
                tempDGV.ColumnHeadersDefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9, FontStyle.Regular);

                // データフォント指定
                tempDGV.DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9, FontStyle.Regular);

                // 行の高さ
                tempDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                tempDGV.ColumnHeadersHeight = 20;
                tempDGV.RowTemplate.Height = 20;

                // 全体の高さ
                tempDGV.Height = 502;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

                // 各列幅指定
                tempDGV.Columns.Add(colNouCode, "届先番号");
                tempDGV.Columns.Add(colNouName, "お届先名");
                tempDGV.Columns.Add(colPtnID, "pID");
                tempDGV.Columns.Add(colMemo, "備考");
                tempDGV.Columns.Add(colTel, "TEL");
                tempDGV.Columns.Add(colAddress, "住所");
                tempDGV.Columns.Add(colDate, "登録日");
                tempDGV.Columns.Add(colID, "ID");

                tempDGV.Columns[colNouCode].Width = 80;
                tempDGV.Columns[colNouName].Width = 220;
                tempDGV.Columns[colPtnID].Width = 60;
                tempDGV.Columns[colMemo].Width = 160;
                tempDGV.Columns[colTel].Width = 90;
                tempDGV.Columns[colDate].Width = 120;
                //tempDGV.Columns[colAddress].Width = 200;

                tempDGV.Columns[colAddress].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                tempDGV.Columns[colNouCode].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colPtnID].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colTel].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //tempDGV.Columns[colAddress].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                tempDGV.Columns[colID].Visible = false;

                // 編集可否
                tempDGV.ReadOnly = true;

                // 行ヘッダを表示しない
                tempDGV.RowHeadersVisible = false;

                // 選択モード
                tempDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                tempDGV.MultiSelect = false;

                // 追加行表示しない
                tempDGV.AllowUserToAddRows = false;

                // データグリッドビューから行削除を禁止する
                tempDGV.AllowUserToDeleteRows = false;

                // 手動による列移動の禁止
                tempDGV.AllowUserToOrderColumns = false;

                // 列サイズ変更禁止
                tempDGV.AllowUserToResizeColumns = true;

                // 行サイズ変更禁止
                tempDGV.AllowUserToResizeRows = false;

                // 行ヘッダーの自動調節
                //tempDGV.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

                // 罫線
                tempDGV.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
                tempDGV.CellBorderStyle = DataGridViewCellBorderStyle.None;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラーメッセージ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void getVNouhin()
        {
            int i = 0;
            using (var Conn = new OracleConnection())
            {
                Conn.ConnectionString = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                Conn.Open();

                string strSQL = "SELECT KOK_ID, NOU_NAME, NOU_JYU1, NOU_JYU2, NOU_TEL FROM RAKUSYO_FAXOCR.V_NOUHINSAKI ";

                OracleCommand Cmd = new OracleCommand(strSQL, Conn);
                OracleDataReader dR = Cmd.ExecuteReader();
                while (dR.Read())
                {
                    Array.Resize(ref vn, i + 1);
                    vn[i] = new clsVNouhin();

                    vn[i].KOK_ID = dR["KOK_ID"].ToString().Trim();
                    vn[i].NOU_NAME = dR["NOU_NAME"].ToString();
                    vn[i].NOU_TEL = dR["NOU_TEL"].ToString();
                    vn[i].NOU_JYU = dR["NOU_JYU1"].ToString() + " " + dR["NOU_JYU2"].ToString();

                    i++;
                }

                dR.Dispose();
                Cmd.Dispose();
            }
        }

        private void showPattern(DataGridView g)
        {
            this.Cursor = Cursors.WaitCursor;

            g.Rows.Clear();

            int cnt = 0;

            foreach (var t in dts.パターンID.OrderBy(a => a.届先番号))
            {
                int vI = 0;

                // 納品先配列から該当納品先を検索
                for (int i = 0; i < vn.Length; i++)
                {
                    if (vn[i].KOK_ID == t.届先番号.ToString().PadLeft(6, '0'))
                    {
                        vI = i;
                        break;
                    }
                }

                if (vI == 0)
                {
                    continue;
                }

                // 検索お届け先コード
                if (sCode.Text != string.Empty)
                {
                    if (!vn[vI].KOK_ID.Contains(sCode.Text))
                    {
                        continue;
                    }
                }

                // 検索電話番号
                if (sTel.Text != string.Empty)
                {
                    if (!vn[vI].NOU_TEL.Contains(sTel.Text))
                    {
                        continue;
                    }
                }

                // 検索お届け先名称
                if (sName.Text != string.Empty)
                {
                    if (!vn[vI].NOU_NAME.Contains(sName.Text))
                    {
                        continue;
                    }
                }

                // 検索住所
                if (sAddress.Text != string.Empty)
                {
                    if (!vn[vI].NOU_JYU.Contains(sAddress.Text))
                    {
                        continue;
                    }
                }
                
                g.Rows.Add();
                g[colNouCode, cnt].Value = t.届先番号.ToString().Trim().PadLeft(6, '0');
                g[colNouName, cnt].Value = vn[vI].NOU_NAME;
                g[colPtnID, cnt].Value = t.連番.ToString().PadLeft(4, '0');
                g[colMemo, cnt].Value = t.備考;
                g[colTel, cnt].Value = vn[vI].NOU_TEL;
                g[colAddress, cnt].Value = vn[vI].NOU_JYU;
                g[colDate, cnt].Value = t.更新年月日;
                g[colID, cnt].Value = t.ID.ToString();

                cnt++;
                g.CurrentCell = null;
            }

            this.Cursor = Cursors.Default;

            if (cnt == 0)
            {
                MessageBox.Show("該当する注文書パターンはありませんでした", "検索結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            showPattern(dataGridView1);
        }

        private void frmTodoke_Load(object sender, EventArgs e)
        {
            // フォーム最小値
            Utility.WindowsMinSize(this, this.Width, this.Height);

            GridviewSet(dataGridView1);

            ptnID = string.Empty;

            getVNouhin();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dataGridView1.SelectedRows.Count == 0)
            //{
            //    return;
            //}

            //int r = dataGridView1.SelectedRows[0].Index;

            //_nouCode = dataGridView1[colNouCode, r].Value.ToString();

            //Close();
        }

        public string ptnID;

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            ptnID = dataGridView1[colID, dataGridView1.SelectedRows[0].Index].Value.ToString();

            Close();
        }
                
        private class clsVNouhin
        {
            public string KOK_ID { get; set; }
            public string NOU_NAME { get; set; }
            public string NOU_TEL { get; set; }
            public string NOU_JYU { get; set; }
        }

        private void sCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '\t')
                e.Handled = true;
        }
    }
}
