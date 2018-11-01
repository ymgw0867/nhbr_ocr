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
using Excel = Microsoft.Office.Interop.Excel;
using NHBR_OCR.common;

namespace NHBR_OCR.Pattern
{
    public partial class frmPrnPtn : Form
    {
        public frmPrnPtn()
        {
            InitializeComponent();

            adp.Fill(dts.パターンID);
        }

        NHBRDataSet dts = new NHBRDataSet();
        NHBRDataSetTableAdapters.パターンIDTableAdapter adp = new NHBRDataSetTableAdapters.パターンIDTableAdapter();

        clsVNouhin[] vn = null;     // 届先マスター配列
        clsVSYOHIN[] vSyo = null;   // 商品マスター配列

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
        private string colChk = "c9";

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
                DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                chk.Name = colChk;
                tempDGV.Columns.Add(chk);
                tempDGV.Columns[colChk].HeaderText = "";

                tempDGV.Columns.Add(colNouCode, "届先番号");
                tempDGV.Columns.Add(colNouName, "お届先名");
                tempDGV.Columns.Add(colPtnID, "pID");
                tempDGV.Columns.Add(colMemo, "備考");
                tempDGV.Columns.Add(colTel, "TEL");
                tempDGV.Columns.Add(colAddress, "住所");
                tempDGV.Columns.Add(colDate, "登録日");
                tempDGV.Columns.Add(colID, "ID");

                tempDGV.Columns[colChk].Width = 30;
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
                tempDGV.ReadOnly = false;

                foreach (DataGridViewColumn item in tempDGV.Columns)
                {
                    if (item.Name == colChk)
                    {
                        item.ReadOnly = false;
                    }
                    else
                    {
                        item.ReadOnly = true;
                    }
                }

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

        ///-----------------------------------------------------------
        /// <summary>
        ///     マスター読み込み </summary>
        ///-----------------------------------------------------------
        private void getVNouhin()
        {
            int i = 0;
            using (var Conn = new OracleConnection())
            {
                Conn.ConnectionString = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                Conn.Open();

                // 届先マスターを配列に読み込み
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

                i = 0;

                // 商品マスターを配列に読み込み
                strSQL = "SELECT SYO_ID,SYO_NAME, SYO_IRI_KESU, SYO_TANI FROM RAKUSYO_FAXOCR.V_SYOHIN ";

                Cmd = new OracleCommand(strSQL, Conn);
                dR = Cmd.ExecuteReader();
                while (dR.Read())
                {
                    Array.Resize(ref vSyo, i + 1);
                    vSyo[i] = new clsVSYOHIN();

                    vSyo[i].SYO_ID = dR["SYO_ID"].ToString().Trim();
                    vSyo[i].SYO_NAME = dR["SYO_NAME"].ToString();
                    vSyo[i].SYO_IRI_KESU = dR["SYO_IRI_KESU"].ToString();
                    vSyo[i].SYO_TANI = dR["SYO_TANI"].ToString();

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
                g[colChk, cnt].Value = true;
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

            // 納品先マスター読み込み
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
            int pCnt = 0;

            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                // チェックされている注文書を対象とする
                if (dataGridView1[colChk, r.Index].Value.ToString() == "True")
                {
                    pCnt++;
                }
            }

            if (pCnt == 0)
            {
                MessageBox.Show("印刷する注文書を選択してください", "印刷対象", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // 印刷確認
            if (MessageBox.Show(pCnt + "件の注文書を印刷します。よろしいですか。", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            PrintDialog pd = new PrintDialog();
            pd.PrinterSettings = new System.Drawing.Printing.PrinterSettings();

            if (pd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string printerName = pd.PrinterSettings.PrinterName; // プリンター名
                int copies = pd.PrinterSettings.Copies; // 印刷部数
                bool ptof = pd.PrinterSettings.PrintToFile; // printToFile

                // ＦＡＸ注文書印刷
                prnSheet(printerName, copies, ptof);
            }
        }
                
        private class clsVNouhin
        {
            public string KOK_ID { get; set; }
            public string NOU_NAME { get; set; }
            public string NOU_TEL { get; set; }
            public string NOU_JYU { get; set; }
        }

        private class clsVSYOHIN
        {
            public string SYO_ID { get; set; }
            public string SYO_NAME { get; set; }
            public string SYO_IRI_KESU { get; set; }
            public string SYO_TANI { get; set; }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("全ての注文書を印刷対象とします。よろしいですか。", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1[colChk, i].Value = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("全ての注文書を印刷対象外とします。よろしいですか。", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1[colChk, i].Value = false;
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            string colName = dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].Name;
            if (colName == colChk)
            {
                if (dataGridView1.IsCurrentCellDirty)
                {
                    //コミットする
                    dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    dataGridView1.RefreshEdit();
                }
            }
        }


        ///----------------------------------------------------------------------
        /// <summary>
        ///     ＦＡＸ注文書印刷処理 </summary>
        /// <param name="prnName">
        ///     プリンタ名</param>
        /// <param name="copies">
        ///     印刷部数</param>
        /// <param name="ptof">
        ///     ファイルに出力</param>
        ///----------------------------------------------------------------------
        private void prnSheet(string prnName, int copies, bool ptof)
        {
            //マウスポインタを待機にする
            this.Cursor = Cursors.WaitCursor;

            // Excel起動
            string sAppPath = System.AppDomain.CurrentDomain.BaseDirectory;

            Excel.Application oXls = new Excel.Application();

            Excel.Workbook oXlsBook = (Excel.Workbook)(oXls.Workbooks.Open(Properties.Settings.Default.xlsFaxPattern, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                               Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                               Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                               Type.Missing, Type.Missing));

            Excel.Worksheet oxlsSheet = (Excel.Worksheet)oXlsBook.Sheets[1];
            Excel.Worksheet oxlsMsSheet = (Excel.Worksheet)oXlsBook.Sheets[1]; // テンプレートシート
            oxlsSheet.Select(Type.Missing);

            Excel.Range rng = null;
            Excel.Range rngFormura = null;

            int pCnt = 1;   // ページカウント
            //int bCount = 0; // progressBar部署カウント
            object[,] rtnArray = null;

            try
            {
                //// progressBar
                //toolStripProgressBar1.Maximum = 100;
                //toolStripProgressBar1.Minimum = 0;
                //toolStripProgressBar1.Visible = true;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    // チェックされている部署を対象とする
                    if (dataGridView1[colChk, i].Value.ToString() == "False")
                    {
                        continue;
                    }

                    // テンプレートシートを追加する
                    pCnt++;
                    oxlsMsSheet.Copy(Type.Missing, oXlsBook.Sheets[pCnt - 1]);
                    oxlsSheet = (Excel.Worksheet)oXlsBook.Sheets[pCnt];

                    // シートのセルを一括して配列に取得します
                    rng = oxlsMsSheet.Range[oxlsMsSheet.Cells[1, 1], oxlsMsSheet.Cells[oxlsMsSheet.UsedRange.Rows.Count, oxlsMsSheet.UsedRange.Columns.Count]];
                    //rng.Value2 = "";
                    //rtnArray = (object[,])rng.Value2;
                    rtnArray = rng.Value2;
                    rtnArray = rng.Formula;

                    // パターンＩＤ
                    string pPID = dataGridView1[colPtnID, i].Value.ToString().PadLeft(4, '0');
                    rtnArray[4, 32] = pPID.Substring(0, 1);
                    rtnArray[4, 38] = pPID.Substring(1, 1);
                    rtnArray[4, 44] = pPID.Substring(2, 1);
                    rtnArray[4, 50] = pPID.Substring(3, 1);

                    // お客様コード（届先番号）
                    pPID = dataGridView1[colNouCode, i].Value.ToString().PadLeft(6, '0');
                    rtnArray[4, 87] = pPID.Substring(0, 1);
                    rtnArray[4, 93] = pPID.Substring(1, 1);
                    rtnArray[4, 99] = pPID.Substring(2, 1);
                    rtnArray[4, 105] = pPID.Substring(3, 1);
                    rtnArray[4, 111] = pPID.Substring(4, 1);
                    rtnArray[4, 117] = pPID.Substring(5, 1);

                    // お客様名
                    rtnArray[8, 29] = Utility.NulltoStr(dataGridView1[colNouName, i].Value);

                    // 更新日：2018/11/01
                    DateTime uDt;
                    if (DateTime.TryParse(dataGridView1[colDate, i].Value.ToString(), out uDt))
                    {
                        rtnArray[15, 2] = uDt.ToShortDateString() + " 更新";
                    }
                    else
                    {
                        rtnArray[15, 2] = string.Empty;
                    }


                    var s = dts.パターンID.Single(a => a.ID == Utility.StrtoInt(dataGridView1[colID, i].Value.ToString()));

                    // 商品名
                    string sIrisu = string.Empty;
                    string sTani = string.Empty;

                    rtnArray[16, 2] = getVSyohin(s.商品1, out sIrisu, out sTani);
                    rtnArray[17, 44] = sIrisu;
                    //rtnArray[16, 73] = sTani;
                    rtnArray[16, 73] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品1 != global.flgOff) 
                    {
                        rtnArray[17, 2] = s.商品1.ToString("D8");
                    }

                    rtnArray[18, 2] = getVSyohin(s.商品2, out sIrisu, out sTani);
                    rtnArray[19, 44] = sIrisu;
                    //rtnArray[18, 73] = sTani;
                    rtnArray[18, 73] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品2 != global.flgOff)
                    {
                        rtnArray[19, 2] = s.商品2.ToString("D8");
                    }
                    
                    rtnArray[20, 2] = getVSyohin(s.商品3, out sIrisu, out sTani);
                    rtnArray[21, 44] = sIrisu;
                    //rtnArray[20, 73] = sTani;
                    rtnArray[20, 73] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品3 != global.flgOff)
                    {
                        rtnArray[21, 2] = s.商品3.ToString("D8");
                    }
                    
                    rtnArray[22, 2] = getVSyohin(s.商品4, out sIrisu, out sTani);
                    rtnArray[23, 44] = sIrisu;
                    //rtnArray[22, 73] = sTani;
                    rtnArray[22, 73] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品4 != global.flgOff)
                    {
                        rtnArray[23, 2] = s.商品4.ToString("D8");
                    }

                    rtnArray[24, 2] = getVSyohin(s.商品5, out sIrisu, out sTani);
                    rtnArray[25, 44] = sIrisu;
                    //rtnArray[24, 73] = sTani;
                    rtnArray[24, 73] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品5 != global.flgOff)
                    {
                        rtnArray[25, 2] = s.商品5.ToString("D8");
                    }

                    rtnArray[26, 2] = getVSyohin(s.商品6, out sIrisu, out sTani);
                    rtnArray[27, 44] = sIrisu;
                    //rtnArray[26, 73] = sTani;
                    rtnArray[26, 73] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品6 != global.flgOff)
                    {
                        rtnArray[27, 2] = s.商品6.ToString("D8");
                    }

                    rtnArray[28, 2] = getVSyohin(s.商品7, out sIrisu, out sTani);
                    rtnArray[29, 44] = sIrisu;
                    //rtnArray[28, 73] = sTani;
                    rtnArray[28, 73] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品7 != global.flgOff)
                    {
                        rtnArray[29, 2] = s.商品7.ToString("D8");
                    }

                    rtnArray[30, 2] = getVSyohin(s.商品8, out sIrisu, out sTani);
                    rtnArray[31, 44] = sIrisu;
                    //rtnArray[30, 73] = sTani;
                    rtnArray[30, 73] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品8 != global.flgOff)
                    {
                        rtnArray[31, 2] = s.商品8.ToString("D8");
                    }

                    rtnArray[32, 2] = getVSyohin(s.商品9, out sIrisu, out sTani);
                    rtnArray[33, 44] = sIrisu;
                    //rtnArray[32, 73] = sTani;
                    rtnArray[32, 73] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品9 != global.flgOff)
                    {
                        rtnArray[33, 2] = s.商品9.ToString("D8");
                    }

                    rtnArray[34, 2] = getVSyohin(s.商品10, out sIrisu, out sTani);
                    rtnArray[35, 44] = sIrisu;
                    //rtnArray[34, 73] = sTani;
                    rtnArray[34, 73] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品10 != global.flgOff)
                    {
                        rtnArray[35, 2] = s.商品10.ToString("D8");
                    }

                    rtnArray[36, 2] = getVSyohin(s.商品11, out sIrisu, out sTani);
                    rtnArray[37, 44] = sIrisu;
                    //rtnArray[36, 73] = sTani;
                    rtnArray[36, 73] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品11 != global.flgOff)
                    {
                        rtnArray[37, 2] = s.商品11.ToString("D8");
                    }

                    rtnArray[38, 2] = getVSyohin(s.商品12, out sIrisu, out sTani);
                    rtnArray[39, 44] = sIrisu;
                    //rtnArray[38, 73] = sTani;
                    rtnArray[38, 73] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品12 != global.flgOff)
                    {
                        rtnArray[39, 2] = s.商品12.ToString("D8");
                    }

                    rtnArray[40, 2] = getVSyohin(s.商品13, out sIrisu, out sTani);
                    rtnArray[41, 44] = sIrisu;
                    //rtnArray[40, 73] = sTani;
                    rtnArray[40, 73] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品13 != global.flgOff)
                    {
                        rtnArray[41, 2] = s.商品13.ToString("D8");
                    }

                    rtnArray[42, 2] = getVSyohin(s.商品14, out sIrisu, out sTani);
                    rtnArray[43, 44] = sIrisu;
                    //rtnArray[42, 73] = sTani;
                    rtnArray[42, 73] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品14 != global.flgOff)
                    {
                        rtnArray[43, 2] = s.商品14.ToString("D8");
                    }

                    rtnArray[44, 2] = getVSyohin(s.商品15, out sIrisu, out sTani);
                    rtnArray[45, 44] = sIrisu;
                    //rtnArray[44, 73] = sTani;
                    rtnArray[44, 73] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品15 != global.flgOff)
                    {
                        rtnArray[45, 2] = s.商品15.ToString("D8");
                    }
                    
                    rtnArray[16, 77] = getVSyohin(s.商品16, out sIrisu, out sTani);
                    rtnArray[17, 119] = sIrisu;
                    //rtnArray[16, 148] = sTani;
                    rtnArray[16, 148] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品16 != global.flgOff)
                    {
                        rtnArray[17, 77] = s.商品16.ToString("D8");
                    }

                    rtnArray[18, 77] = getVSyohin(s.商品17, out sIrisu, out sTani);
                    rtnArray[19, 119] = sIrisu;
                    //rtnArray[18, 148] = sTani;
                    rtnArray[18, 148] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品17 != global.flgOff)
                    {
                        rtnArray[19, 77] = s.商品17.ToString("D8");
                    }

                    rtnArray[20, 77] = getVSyohin(s.商品18, out sIrisu, out sTani);
                    rtnArray[21, 119] = sIrisu;
                    //rtnArray[20, 148] = sTani;
                    rtnArray[20, 148] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品18 != global.flgOff)
                    {
                        rtnArray[21, 77] = s.商品18.ToString("D8");
                    }

                    rtnArray[22, 77] = getVSyohin(s.商品19, out sIrisu, out sTani);
                    rtnArray[23, 119] = sIrisu;
                    //rtnArray[22, 148] = sTani;
                    rtnArray[22, 148] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品19 != global.flgOff)
                    {
                        rtnArray[23, 77] = s.商品19.ToString("D8");
                    }

                    rtnArray[24, 77] = getVSyohin(s.商品20, out sIrisu, out sTani);
                    rtnArray[25, 119] = sIrisu;
                    //rtnArray[24, 148] = sTani;
                    rtnArray[24, 148] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品20 != global.flgOff)
                    {
                        rtnArray[25, 77] = s.商品20.ToString("D8");
                    }

                    rtnArray[26, 77] = getVSyohin(s.商品21, out sIrisu, out sTani);
                    rtnArray[27, 119] = sIrisu;
                    //rtnArray[26, 148] = sTani;
                    rtnArray[26, 148] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品21 != global.flgOff)
                    {
                        rtnArray[27, 77] = s.商品21.ToString("D8");
                    }

                    rtnArray[28, 77] = getVSyohin(s.商品22, out sIrisu, out sTani);
                    rtnArray[29, 119] = sIrisu;
                    //rtnArray[28, 148] = sTani;
                    rtnArray[28, 148] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品22 != global.flgOff)
                    {
                        rtnArray[29, 77] = s.商品22.ToString("D8");
                    }

                    rtnArray[30, 77] = getVSyohin(s.商品23, out sIrisu, out sTani);
                    rtnArray[31, 119] = sIrisu;
                    //rtnArray[30, 148] = sTani;
                    rtnArray[30, 148] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品23 != global.flgOff)
                    {
                        rtnArray[31, 77] = s.商品23.ToString("D8");
                    }

                    rtnArray[32, 77] = getVSyohin(s.商品24, out sIrisu, out sTani);
                    rtnArray[33, 119] = sIrisu;
                    //rtnArray[32, 148] = sTani;
                    rtnArray[32, 148] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品24 != global.flgOff)
                    {
                        rtnArray[33, 77] = s.商品24.ToString("D8");
                    }

                    rtnArray[34, 77] = getVSyohin(s.商品25, out sIrisu, out sTani);
                    rtnArray[35, 119] = sIrisu;
                    //rtnArray[34, 148] = sTani;
                    rtnArray[34, 148] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品25 != global.flgOff)
                    {
                        rtnArray[35, 77] = s.商品25.ToString("D8");
                    }

                    rtnArray[36, 77] = getVSyohin(s.商品26, out sIrisu, out sTani);
                    rtnArray[37, 119] = sIrisu;
                    //rtnArray[36, 148] = sTani;
                    rtnArray[36, 148] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品26 != global.flgOff)
                    {
                        rtnArray[37, 77] = s.商品26.ToString("D8");
                    }

                    rtnArray[38, 77] = getVSyohin(s.商品27, out sIrisu, out sTani);
                    rtnArray[39, 119] = sIrisu;
                    //rtnArray[38, 148] = sTani;
                    rtnArray[38, 148] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品27 != global.flgOff)
                    {
                        rtnArray[39, 77] = s.商品27.ToString("D8");
                    }

                    rtnArray[40, 77] = getVSyohin(s.商品28, out sIrisu, out sTani);
                    rtnArray[41, 119] = sIrisu;
                    //rtnArray[40, 148] = sTani;
                    rtnArray[40, 148] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品28 != global.flgOff)
                    {
                        rtnArray[41, 77] = s.商品28.ToString("D8");
                    }

                    rtnArray[42, 77] = getVSyohin(s.商品29, out sIrisu, out sTani);
                    rtnArray[43, 119] = sIrisu;
                    //rtnArray[42, 148] = sTani;
                    rtnArray[42, 148] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品29 != global.flgOff)
                    {
                        rtnArray[43, 77] = s.商品29.ToString("D8");
                    }

                    rtnArray[44, 77] = getVSyohin(s.商品30, out sIrisu, out sTani);
                    rtnArray[45, 119] = sIrisu;
                    //rtnArray[44, 148] = sTani;
                    rtnArray[44, 148] = Utility.getStrConv(sTani);   // 2017/12/09
                    if (s.商品30 != global.flgOff)
                    {
                        rtnArray[45, 77] = s.商品30.ToString("D8");
                    }

                    // 備考
                    //rtnArray[62, 2] = s.備考; 2018/10/02
                    
                    // 配列からシートセルに一括してデータをセットします
                    rng = oxlsSheet.Range[oxlsSheet.Cells[1, 1], oxlsSheet.Cells[oxlsSheet.UsedRange.Rows.Count, oxlsSheet.UsedRange.Columns.Count]];
                    rng.Value2 = rtnArray;
                }

                // 確認のためExcelのウィンドウを表示する
                oXls.Visible = true;

                // 1枚目はテンプレートシートなので印刷時には削除する
                oXls.DisplayAlerts = false;
                oXlsBook.Sheets[1].Delete();

                //System.Threading.Thread.Sleep(1000);

                // 印刷
                oXlsBook.PrintOutEx(Type.Missing, Type.Missing, copies, true, prnName, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                //if (ptof)
                //{
                //    if (copies > 1)
                //    {
                //        int iX = 1;
                //        while (iX <= copies)
                //        {
                //            oXlsBook.PrintOutEx(Type.Missing, Type.Missing, Type.Missing, true, prnName, ptof, Type.Missing, Type.Missing, Type.Missing);
                //            iX++;
                //        }
                //    }
                //    else
                //    {
                //        oXlsBook.PrintOutEx(Type.Missing, Type.Missing, copies, true, prnName, ptof, Type.Missing, Type.Missing, Type.Missing);
                //    }
                //}
                //else
                //{
                //    oXlsBook.PrintOutEx(Type.Missing, Type.Missing, copies, true, prnName, ptof, Type.Missing, Type.Missing, Type.Missing);
                //}

                // 確認のためExcelのウィンドウを非表示にする
                oXls.Visible = false;

                // 終了メッセージ 
                MessageBox.Show("終了しました");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "印刷処理", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            finally
            {
                // ウィンドウを非表示にする
                oXls.Visible = false;

                // 保存処理
                oXls.DisplayAlerts = false;

                // Bookをクローズ
                oXlsBook.Close(Type.Missing, Type.Missing, Type.Missing);

                // Excelを終了
                oXls.Quit();

                // COM オブジェクトの参照カウントを解放する 
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oxlsMsSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oxlsSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oXlsBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oXls);

                oXls = null;
                oXlsBook = null;
                oxlsSheet = null;
                oxlsMsSheet = null;

                GC.Collect();

                //マウスポインタを元に戻す
                this.Cursor = Cursors.Default;
            }
        }

        ///--------------------------------------------------------
        /// <summary>
        ///     商品マスター取得 </summary>
        /// <param name="sCode">
        ///     商品コード </param>
        /// <param name="sIrisu">
        ///     入数</param>
        /// <param name="sTani">
        ///     単位</param>
        /// <returns>
        ///     商品名</returns>
        ///--------------------------------------------------------
        private string getVSyohin(int sCode, out string sIrisu, out string sTani)
        {
            string sName = string.Empty;
            sIrisu = string.Empty;
            sTani = string.Empty;

            if (sCode != global.flgOff)
            {
                for (int i = 0; i < vSyo.Length; i++)
                {
                    if (sCode.ToString().PadLeft(8, '0') == vSyo[i].SYO_ID)
                    {
                        sName = vSyo[i].SYO_NAME;
                        sIrisu = vSyo[i].SYO_IRI_KESU;
                        sTani = vSyo[i].SYO_TANI;
                        break;
                    }
                }
            }
            
            return sName;
        }

        private void sCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '\t')
                e.Handled = true;
        }
    }
}
