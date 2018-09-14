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
    public partial class frmCamGet : Form
    {
        public frmCamGet()
        {
            InitializeComponent();

            hAdp.Fill(dts.キャンペーンヘッダ);
            sID = 0;
        }

        bool _mMode = false;

        // カラム定義
        private string colNouCode = "c0";
        private string colNouName = "c1";
        private string colKaishi = "c2";
        private string colShuRyo = "c3";

        NHBRDataSet dts = new NHBRDataSet();
        NHBRDataSetTableAdapters.キャンペーンヘッダTableAdapter hAdp = new NHBRDataSetTableAdapters.キャンペーンヘッダTableAdapter();

        DateTime staDate = DateTime.Parse("1900/01/01");
        DateTime endDate = DateTime.Parse("2900/12/31");

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
                tempDGV.Columns.Add(colNouCode, "番号");
                tempDGV.Columns.Add(colNouName, "キャンペーン名称");
                tempDGV.Columns.Add(colKaishi, "開始");
                tempDGV.Columns.Add(colShuRyo, "終了");

                tempDGV.Columns[colNouCode].Width = 60;
                tempDGV.Columns[colNouName].Width = 300;
                tempDGV.Columns[colKaishi].Width = 100;
                tempDGV.Columns[colShuRyo].Width = 100;

                tempDGV.Columns[colNouName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                tempDGV.Columns[colNouCode].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colKaishi].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colShuRyo].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
            
            this.Cursor = Cursors.WaitCursor;

            g.Rows.Clear();

            int cnt = 0;

            var s = dts.キャンペーンヘッダ.Where(a => a.ID > 0).OrderByDescending(a => a.ID);
                
            if (sCode.Text != string.Empty)
            {
                s = s.Where(a => a.名称.Contains(sCode.Text)).OrderByDescending(a => a.ID);
            }

            if (dateTimePicker1.Checked)
            {
                DateTime dt = DateTime.Parse(dateTimePicker1.Value.ToShortDateString());
                s = s.Where(a => a.終了年月日 < dt).OrderByDescending(a => a.ID);
            }

            foreach (var t in s)
            {
                g.Rows.Add();
                g[colNouCode, cnt].Value = t.ID.ToString();
                g[colNouName, cnt].Value = t.名称;

                if (t.開始年月日 != staDate)
                {
                    g[colKaishi, cnt].Value = t.開始年月日.ToShortDateString();
                }
                else
                {
                    g[colKaishi, cnt].Value = "";
                }

                if (t.終了年月日 != endDate)
                {
                    g[colShuRyo, cnt].Value = t.終了年月日.ToShortDateString();
                }
                else
                {
                    g[colShuRyo, cnt].Value = "";
                }

                cnt++;
            }
                
            g.CurrentCell = null;

            // 該当なしメッセージ
            if (cnt == 0)
            {
                MessageBox.Show("該当するキャンペーンはありませんでした", "結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            dateTimePicker1.Checked = false;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            int r = dataGridView1.SelectedRows[0].Index;

            sID = Utility.StrtoInt(dataGridView1[colNouCode, r].Value.ToString());
            sName = dataGridView1[colNouName, r].Value.ToString();
            sDate = dataGridView1[colKaishi, r].Value.ToString();
            eDate = dataGridView1[colShuRyo, r].Value.ToString();

            Close();
        }

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
            
            int r = dataGridView1.SelectedRows[0].Index;

            sID = Utility.StrtoInt(dataGridView1[colNouCode, r].Value.ToString());
            sName = dataGridView1[colNouName, r].Value.ToString();
            sDate = dataGridView1[colKaishi, r].Value.ToString();
            eDate = dataGridView1[colShuRyo, r].Value.ToString();

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

        public int sID { get; set; }
        public string sName { get; set; }
        public string sDate { get; set; }
        public string eDate { get; set; }
    }
}
