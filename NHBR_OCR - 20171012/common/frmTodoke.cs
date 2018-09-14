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
    public partial class frmTodoke : Form
    {
        public frmTodoke(bool mMode)
        {
            InitializeComponent();
            _mMode = mMode;
        }

        bool _mMode = false;

        // カラム定義
        private string colNouCode = "c0";
        private string colNouName = "c1";
        private string colTel = "c2";
        private string colZip = "c3";
        private string colAddress = "c4";

        /// <summary>
        /// データグリッドビューの定義を行います
        /// </summary>
        private void GridviewSet(DataGridView tempDGV)
        {
            try
            {
                //フォームサイズ定義

                // 列スタイルを変更する

                tempDGV.EnableHeadersVisualStyles = false;
                tempDGV.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(225, 243, 190);
                tempDGV.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

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
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 253, 230);

                // 各列幅指定
                tempDGV.Columns.Add(colNouCode, "届先番号");
                tempDGV.Columns.Add(colNouName, "お届先名");
                tempDGV.Columns.Add(colTel, "TEL");
                tempDGV.Columns.Add(colAddress, "住所");

                tempDGV.Columns[colNouCode].Width = 80;
                tempDGV.Columns[colNouName].Width = 200;
                tempDGV.Columns[colTel].Width = 90;
                //tempDGV.Columns[colAddress].Width = 200;

                tempDGV.Columns[colAddress].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                tempDGV.Columns[colNouCode].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colTel].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //tempDGV.Columns[colAddress].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 編集可否
                tempDGV.ReadOnly = true;

                // 行ヘッダを表示しない
                tempDGV.RowHeadersVisible = false;

                // 選択モード
                tempDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                if (_mMode)
                {
                    tempDGV.MultiSelect = true;
                }
                else
                {
                    tempDGV.MultiSelect = false;
                }

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
        }

        private void showNouhin(DataGridView g)
        {
            string mySql = string.Empty;

            // 届先名
            if (sName.Text.Trim() != string.Empty)
            {
                mySql += "WHERE (NOU_NAME LIKE '%" + sName.Text + "%') ";
            }

            // 住所
            if (sAddress.Text.Trim() != string.Empty)
            {
                if (mySql == string.Empty)
                {
                    mySql = "WHERE ";
                }
                else
                {
                    mySql += "AND ";
                }

                mySql += "((NOU_JYU1 LIKE '%" + sAddress.Text + "%') OR (NOU_JYU2 LIKE '%" + sAddress.Text + "%')) ";
            }
            
            // 電話番号
            if (sTel.Text.Trim() != string.Empty)
            {
                if (mySql == string.Empty)
                {
                    mySql = "WHERE ";
                }
                else
                {
                    mySql += "AND ";
                }

                mySql += "(NOU_TEL LIKE '%" + sTel.Text + "%') ";
            }

            // 届先番号
            if (sCode.Text.Trim() != string.Empty)
            {
                if (mySql == string.Empty)
                {
                    mySql = "WHERE ";
                }
                else
                {
                    mySql += "AND ";
                }

                mySql += "(KOK_ID LIKE '%" + sCode.Text + "%') ";
            }

            this.Cursor = Cursors.WaitCursor;

            g.Rows.Clear();

            int cnt = 0;

            using (var Conn = new OracleConnection())
            {
                Conn.ConnectionString = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                Conn.Open();

                string strSQL = "SELECT KOK_ID, NOU_NAME, NOU_JYU1, NOU_JYU2, NOU_TEL FROM RAKUSYO_FAXOCR.V_NOUHINSAKI ";
                strSQL += mySql;
                strSQL += " ORDER BY KOK_ID";

                OracleCommand Cmd = new OracleCommand(strSQL, Conn);
                OracleDataReader dR = Cmd.ExecuteReader();
                while (dR.Read())
                {
                    g.Rows.Add();
                    g[colNouCode, cnt].Value = dR["KOK_ID"].ToString().Trim();
                    g[colNouName, cnt].Value = dR["NOU_NAME"].ToString();
                    g[colTel, cnt].Value = dR["NOU_TEL"].ToString();
                    g[colAddress, cnt].Value = dR["NOU_JYU1"].ToString() + " " + dR["NOU_JYU2"].ToString();

                    cnt++;
                }

                dR.Dispose();
                Cmd.Dispose();

                g.CurrentCell = null;

                // 該当なしメッセージ
                if (cnt == 0)
                {
                    MessageBox.Show("該当するお届け先はありませんでした", "結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            this.Cursor = Cursors.Default;
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void frmTodoke_Load(object sender, EventArgs e)
        {
            // フォーム最大値
            Utility.WindowsMaxSize(this, this.Width, this.Height);

            // フォーム最小値
            Utility.WindowsMinSize(this, this.Width, this.Height);

            GridviewSet(dataGridView1);

            _nouCode = null;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            Array.Resize(ref _nouCode, 1);

            int r = dataGridView1.SelectedRows[0].Index;

            _nouCode[0] = dataGridView1[colNouCode, r].Value.ToString();

            Close();
        }

        public string[] _nouCode;

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            getSelectRows();
        }

        private void getSelectRows()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            int iX = 0;

            if (_mMode)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Selected)
                    {
                        Array.Resize(ref _nouCode, iX + 1);
                        _nouCode[iX] = dataGridView1[colNouCode, i].Value.ToString();
                        iX++;
                    }
                }
            }
            else
            {
                Array.Resize(ref _nouCode, iX + 1);
                _nouCode[iX] = dataGridView1[colNouCode, dataGridView1.SelectedRows[0].Index].Value.ToString();
            }

            Close();
        }

        private void frmTodoke_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F10 && button3.Enabled)
            {
                getSelectRows();
            }

            if (e.KeyData == Keys.F12)
            {
                Close();
            }
        }

        private void btnS_Click(object sender, EventArgs e)
        {
            showNouhin(dataGridView1);
        }

        private void sCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '\t')
                e.Handled = true;
        }
    }
}
