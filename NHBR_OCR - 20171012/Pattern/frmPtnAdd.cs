using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using GrapeCity.Win.MultiRow;
using Oracle.ManagedDataAccess.Client;
using NHBR_OCR.common;

namespace NHBR_OCR.Pattern
{
    public partial class frmPtnAdd : Form
    {
        public frmPtnAdd()
        {
            InitializeComponent();
            adp.Fill(dts.パターンID);
        }

        NHBRDataSet dts = new NHBRDataSet();
        NHBRDataSetTableAdapters.パターンIDTableAdapter adp = new NHBRDataSetTableAdapters.パターンIDTableAdapter();

        const int ADDMODE = 0;
        const int EDITMODE = 1;
        
        const int GET_NOUHIN_NAME = 0;
        const int GET_NOUHIN_TEL = 1;
        const int GET_NOUHIN_ADD = 2;

        const int CALL_GC = 0;
        const int CALL_DG = 1;

        int fMode;
        int fID;

        DateTime frDt;    // 商品履歴の表示開始日

        // valueChangeステータス
        bool valueChangeStatus = false;

        private void frmPtnAdd_Load(object sender, EventArgs e)
        {
            // フォーム最大値
            Utility.WindowsMaxSize(this, this.Width, this.Height);

            // フォーム最小値
            Utility.WindowsMinSize(this, this.Width, this.Height);

            // Tabキーの既定のショートカットキーを解除する。
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);

            // Tabキーのショートカットキーにユーザー定義のショートカットキーを割り当てる。
            gcMultiRow1.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Tab);
            //gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Enter);

            tdkGridviewSet(dataGridView2);
            GridviewSet(dataGridView1);

            dispInitial();
        }

        private void dispInitial()
        {
            valueChangeStatus = false;
            gcMrSetting();
            valueChangeStatus = true;
            
            dataGridView2.Rows.Clear();
            dataGridView2.Rows.Add(50);
            dataGridView2.CurrentCell = null;
            dataGridView2.ReadOnly = false;

            dataGridView1.Rows.Clear();
            dataGridView1.CurrentCell = null;

            button1.Enabled = true;
            button2.Enabled = false;
            comboBox1.SelectedIndex = 0;
            comboBox1.Enabled = false;
            //lblFrDate.Visible = false;
            dateTimePicker1.Enabled = false;

            txtMemo.Text = string.Empty;

            fMode = ADDMODE;
            btnDel.Visible = false;
            button7.Enabled = true;
        }


        private void continueInitial()
        {
            valueChangeStatus = false;
            gcMrSetting();
            valueChangeStatus = true;

            // パターン番号インクリメント
            for (int i = 0; i < 50; i++)
            {
                int nn = Utility.StrtoInt(Utility.NulltoStr(dataGridView2[colPtnNum, i].Value));
                if (nn != 0)
                {
                    dataGridView2[colPtnNum, i].Value = nn + 1;
                }
            }

            valueChangeStatus = true;

            //dataGridView2.Rows.Clear();
            //dataGridView2.Rows.Add(50);
            //dataGridView2.CurrentCell = null;
            //dataGridView2.ReadOnly = false;

            //dataGridView1.Rows.Clear();
            //dataGridView1.CurrentCell = null;

            //button1.Enabled = true;
            //button2.Enabled = false;
            //comboBox1.SelectedIndex = 0;
            //comboBox1.Enabled = false;
            //dateTimePicker1.Enabled = false;

            //txtMemo.Text = string.Empty;

            fMode = ADDMODE;
            btnDel.Visible = false;
            button7.Enabled = true;
        }

        // カラム定義
        private string colTdkCode = "c0";
        private string colTdkName = "c1";
        private string colPtnNum = "c2";
        private string colTel = "c3";
        private string colAddress = "c4";

        /// <summary>
        /// 届先データグリッドビューの定義を行います
        /// </summary>
        private void tdkGridviewSet(DataGridView tempDGV)
        {
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
                tempDGV.Height = 199;

                // 奇数行の色
                //tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

                // 各列幅指定
                tempDGV.Columns.Add(colTdkCode, "届先番号");
                tempDGV.Columns.Add(colTdkName, "届先名");
                tempDGV.Columns.Add(colPtnNum, "PID");
                tempDGV.Columns.Add(colTel, "TEL");
                tempDGV.Columns.Add(colAddress, "住所");

                tempDGV.Columns[colTdkCode].Width = 70;
                tempDGV.Columns[colTdkName].Width = 180;
                tempDGV.Columns[colPtnNum].Width = 50;
                tempDGV.Columns[colTel].Width = 100;
                tempDGV.Columns[colAddress].Width = 320;

                tempDGV.Columns[colTdkCode].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colPtnNum].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colTel].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 編集可否
                tempDGV.ReadOnly = false;

                foreach (DataGridViewColumn item in dataGridView2.Columns)
                {
                    if (item.Name == colTdkCode)
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
                tempDGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
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
                //tempDGV.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
                //tempDGV.CellBorderStyle = DataGridViewCellBorderStyle.None;

                dataGridView2.Rows.Add(50);
                dataGridView2.CurrentCell = null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラーメッセージ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // カラム定義
        private string colHinCode = "c0";
        private string colHinName = "c1";
        private string colRyou = "c2";
        private string colIrisu = "c3";
        private string colShubetsu = "c4";
        private string colTani = "c5";
        private string colUriDate = "c6";
        private string colSuu = "c7";

        ///------------------------------------------------------------------------
        /// <summary>
        ///     商品一覧データグリッドビューの定義を行います </summary>
        ///------------------------------------------------------------------------
        private void GridviewSet(DataGridView tempDGV)
        {
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
                tempDGV.Height = 382;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

                // 各列幅指定
                tempDGV.Columns.Add(colHinCode, "商品コード");
                tempDGV.Columns.Add(colHinName, "商品名");
                tempDGV.Columns.Add(colIrisu, "入数");
                tempDGV.Columns.Add(colTani, "単位");

                tempDGV.Columns[colHinCode].Width = 80;
                tempDGV.Columns[colIrisu].Width = 50;
                tempDGV.Columns[colTani].Width = 50;

                tempDGV.Columns[colHinName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                tempDGV.Columns[colHinCode].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colIrisu].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colTani].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                // 編集可否
                tempDGV.ReadOnly = true;

                // 行ヘッダを表示しない
                tempDGV.RowHeadersVisible = false;

                // 選択モード
                tempDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                tempDGV.MultiSelect = true;

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

        ///------------------------------------------------------------------------
        /// <summary>
        ///     商品履歴一覧データグリッドビューの定義を行います </summary>
        ///------------------------------------------------------------------------
        private void rirekiGridviewSet(DataGridView tempDGV)
        {
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
                tempDGV.Height = 382;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

                // 各列幅指定
                tempDGV.Columns.Add(colHinCode, "商品コード");
                tempDGV.Columns.Add(colHinName, "商品名");
                tempDGV.Columns.Add(colUriDate, "最終売上日");
                tempDGV.Columns.Add(colSuu, "販売数");

                tempDGV.Columns[colHinCode].Width = 80;
                tempDGV.Columns[colUriDate].Width = 90;
                tempDGV.Columns[colSuu].Width = 70;

                tempDGV.Columns[colHinName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                tempDGV.Columns[colHinCode].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colUriDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colSuu].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 編集可否
                tempDGV.ReadOnly = true;

                // 行ヘッダを表示しない
                tempDGV.RowHeadersVisible = false;

                // 選択モード
                tempDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                tempDGV.MultiSelect = true;

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

        private void gcMrSetting()
        {
            //multirow編集モード
            gcMultiRow1.EditMode = EditMode.EditProgrammatically;

            this.gcMultiRow1.AllowUserToAddRows = false;                    // 手動による行追加を禁止する
            this.gcMultiRow1.AllowUserToDeleteRows = false;                 // 手動による行削除を禁止する
            this.gcMultiRow1.Rows.Clear();                                  // 行数をクリア
            this.gcMultiRow1.RowCount = 15;                     // 行数を設定
            this.gcMultiRow1.HideSelection = true;                          // GcMultiRow コントロールがフォーカスを失ったとき、セルの選択状態を非表示にする

            gcMultiRow1[0, "lblNum"].Value = "1";
            gcMultiRow1[0, "lblNum2"].Value = "16";
            gcMultiRow1[1, "lblNum"].Value = "2";
            gcMultiRow1[1, "lblNum2"].Value = "17";
            gcMultiRow1[2, "lblNum"].Value = "3";
            gcMultiRow1[2, "lblNum2"].Value = "18";
            gcMultiRow1[3, "lblNum"].Value = "4";
            gcMultiRow1[3, "lblNum2"].Value = "19";
            gcMultiRow1[4, "lblNum"].Value = "5";
            gcMultiRow1[4, "lblNum2"].Value = "20";
            gcMultiRow1[5, "lblNum"].Value = "6";
            gcMultiRow1[5, "lblNum2"].Value = "21";
            gcMultiRow1[6, "lblNum"].Value = "7";
            gcMultiRow1[6, "lblNum2"].Value = "22";
            gcMultiRow1[7, "lblNum"].Value = "8";
            gcMultiRow1[7, "lblNum2"].Value = "23";
            gcMultiRow1[8, "lblNum"].Value = "9";
            gcMultiRow1[8, "lblNum2"].Value = "24";
            gcMultiRow1[9, "lblNum"].Value = "10";
            gcMultiRow1[9, "lblNum2"].Value = "25";
            gcMultiRow1[10, "lblNum"].Value = "11";
            gcMultiRow1[10, "lblNum2"].Value = "26";
            gcMultiRow1[11, "lblNum"].Value = "12";
            gcMultiRow1[11, "lblNum2"].Value = "27";
            gcMultiRow1[12, "lblNum"].Value = "13";
            gcMultiRow1[12, "lblNum2"].Value = "28";
            gcMultiRow1[13, "lblNum"].Value = "14";
            gcMultiRow1[13, "lblNum2"].Value = "29";
            gcMultiRow1[14, "lblNum"].Value = "15";
            gcMultiRow1[14, "lblNum2"].Value = "30";            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            GridviewSet(dataGridView1);
            showShohin(dataGridView1);
        }

        ///--------------------------------------------------------
        /// <summary>
        ///     データグリッドに商品一覧を表示する </summary>
        /// <param name="g">
        ///     データグリッドビューオブジェクト</param>
        ///--------------------------------------------------------
        private void showShohin(DataGridView g)
        {
            this.Cursor = Cursors.WaitCursor;

            g.Rows.Clear();

            int cnt = 0;
            this.txtMemo.Text = "";
            using (var Conn = new OracleConnection())
            {
                Conn.ConnectionString = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                Conn.Open();
                string strSQL = "select SYO_ID, SYO_NAME, SYO_IRI_KESU, SYO_TANI from RAKUSYO_FAXOCR.V_SYOHIN ORDER BY SYO_ID";
                OracleCommand Cmd = new OracleCommand(strSQL, Conn);
                OracleDataReader dR = Cmd.ExecuteReader();
                while (dR.Read())
                {
                    // 販売終了商品はネグる 2017/08/01
                    if (dR["SYO_NAME"].ToString().Contains("終売") || dR["SYO_NAME"].ToString().Contains("休売"))
                    {
                        continue;
                    }
                    
                    // 商品表示
                    g.Rows.Add();
                    g[colHinCode, cnt].Value = dR["SYO_ID"].ToString().Trim();
                    g[colHinName, cnt].Value = dR["SYO_NAME"].ToString();
                    g[colIrisu, cnt].Value = dR["SYO_IRI_KESU"].ToString();
                    g[colTani, cnt].Value = dR["SYO_TANI"].ToString();

                    cnt++;
                }

                dR.Dispose();
                Cmd.Dispose();

                g.CurrentCell = null;
            }

            this.Cursor = Cursors.Default;
        }

        ///--------------------------------------------------------
        /// <summary>
        ///     データグリッドに商品履歴を表示する </summary>
        /// <param name="g">
        ///     データグリッドビューオブジェクト</param>
        ///--------------------------------------------------------
        private void showShohinRireki(DataGridView g, string sNou_Code, string frDate)
        {
            this.Cursor = Cursors.WaitCursor;

            g.Rows.Clear();

            int cnt = 0;
            this.txtMemo.Text = "";
            using (var Conn = new OracleConnection())
            {
                Conn.ConnectionString = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                Conn.Open();

                string strSQL = "SELECT RAKUSYO_FAXOCR.V_URIAGE.SYO_ID, RAKUSYO_FAXOCR.V_URIAGE.KOK_ID, RAKUSYO_FAXOCR.V_SYOHIN.SYO_NAME, ";
                strSQL += "RAKUSYO_FAXOCR.V_URIAGE.IRISU, RAKUSYO_FAXOCR.V_URIAGE.LURIDAY, RAKUSYO_FAXOCR.V_URIAGE.SUU, RAKUSYO_FAXOCR.V_URIAGE.TOTAL_SUU ";
                strSQL += "FROM RAKUSYO_FAXOCR.V_URIAGE INNER JOIN RAKUSYO_FAXOCR.V_SYOHIN ";
                strSQL += "ON RAKUSYO_FAXOCR.V_URIAGE.SYO_ID = RAKUSYO_FAXOCR.V_SYOHIN.SYO_ID ";
                strSQL += "WHERE RTRIM(RAKUSYO_FAXOCR.V_URIAGE.KOK_ID) = '" + sNou_Code + "'";
                //strSQL += "WHERE RAKUSYO_FAXOCR.V_URIAGE.KOK_ID = " + sNou_Code;

                if (frDate != string.Empty)
                {
                    strSQL += " AND RAKUSYO_FAXOCR.V_URIAGE.LURIDAY >= '" + frDate + "' ";
                }

                strSQL += " ORDER BY SYO_ID";

                OracleCommand Cmd = new OracleCommand(strSQL, Conn);
                OracleDataReader dR = Cmd.ExecuteReader();
                while (dR.Read())
                {
                    // 販売終了商品はネグる 2017/08/01
                    if (dR["SYO_NAME"].ToString().Contains("終売") || dR["SYO_NAME"].ToString().Contains("休売"))
                    {
                        continue;
                    }
                    
                    // 商品表示
                    g.Rows.Add();
                    g[colHinCode, cnt].Value = dR["SYO_ID"].ToString().Trim();
                    g[colHinName, cnt].Value = dR["SYO_NAME"].ToString();
                    g[colUriDate, cnt].Value = DateTime.Parse(dR["LURIDAY"].ToString()).ToShortDateString();
                    g[colSuu, cnt].Value = dR["TOTAL_SUU"].ToString();  // 2017/08/21

                    cnt++;
                }

                dR.Dispose();
                Cmd.Dispose();

                g.CurrentCell = null;

                // 該当なしメッセージ
                if (cnt == 0)
                {
                    MessageBox.Show("該当する商品はありませんでした", "結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            this.Cursor = Cursors.Default;
        }

        private void gcMultiRow2_CellValueChanged(object sender, CellEventArgs e)
        {
            if (!valueChangeStatus)
            {
                return;
            }

            if (e.CellName == "txtTdkNum")
            {
                valueChangeStatus = false;

                // 届先名表示
                string gTel = string.Empty;
                string gJyu = string.Empty; 

                valueChangeStatus = true;
            }
        }

        ///-------------------------------------------------------------------
        /// <summary>
        ///     お届け先情報取得 </summary>
        /// <param name="tID">
        ///     届先番号</param>
        /// <param name="sTel">
        ///     電話番号</param>
        /// <param name="sJyu">
        ///     住所</param>
        /// <returns>
        ///     届先名</returns>
        ///-------------------------------------------------------------------
        //private string getNouhinName(string tID, out string sTel, out string sJyu)
        //{
        //    string val = string.Empty;
        //    sTel = string.Empty;
        //    sJyu = string.Empty;

        //    using (var Conn = new OracleConnection())
        //    {
        //        Conn.ConnectionString = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
        //        Conn.Open();

        //        string strSQL = "SELECT KOK_ID, NOU_NAME, NOU_JYU1, NOU_JYU2, NOU_TEL from RAKUSYO_FAXOCR.V_NOUHINSAKI WHERE KOK_ID = '" + tID + "'";
        //        OracleCommand Cmd = new OracleCommand(strSQL, Conn);
        //        OracleDataReader dR = Cmd.ExecuteReader();
        //        while (dR.Read())
        //        {
        //            val = dR["NOU_NAME"].ToString().Trim();
        //            sTel = dR["NOU_TEL"].ToString().Trim();
        //            sJyu = dR["NOU_JYU1"].ToString().Trim() + " " + dR["NOU_JYU2"].ToString().Trim();
        //        }

        //        dR.Dispose();
        //        Cmd.Dispose();
        //    }

        //    return val;
        //}

        private void gcMultiRow1_CellEnter(object sender, CellEventArgs e)
        {
            if (gcMultiRow1.EditMode == EditMode.EditProgrammatically)
            {
                gcMultiRow1.BeginEdit(true);
            }
        }

        private void gcMultiRow1_CellClick(object sender, CellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int iX = 0;

                if (e.CellName == "lblNum")
                {
                    //for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    //{
                    //    if (dataGridView1.Rows[i].Selected)
                    //    {                            
                    //        gcMultiRow1[e.RowIndex + iX, "txtHinCode"].Value = dataGridView1[colHinCode, dataGridView1.Rows[i].Index].Value.ToString();
                    //        iX++;
                    //    }
                    //}

                    for (int i = dataGridView1.SelectedRows.Count - 1; i >= 0; i--)
                    {
                        int gRow = e.RowIndex + dataGridView1.SelectedRows.Count - 1 - i;

                        if (gRow > 14)
                        {
                            break;
                        }

                        gcMultiRow1[gRow, "txtHinCode"].Value = dataGridView1[colHinCode, dataGridView1.SelectedRows[i].Index].Value.ToString();
                        dataGridView1.SelectedRows[i].DefaultCellStyle.BackColor = Color.LightPink;
                    }

                    //gcMultiRow1[e.RowIndex, "lblHinName"].Value = dataGridView1[colHinName, dataGridView1.SelectedRows[0].Index].Value.ToString();
                    //gcMultiRow1[e.RowIndex, "txtHinCode"].Value = dataGridView1[colHinCode, dataGridView1.SelectedRows[0].Index].Value.ToString();
                    //gcMultiRow1[e.RowIndex, "lblIrisu"].Value = dataGridView1[colIrisu, dataGridView1.SelectedRows[0].Index].Value.ToString();
                    //gcMultiRow1[e.RowIndex, "lblTani"].Value = dataGridView1[colTani, dataGridView1.SelectedRows[0].Index].Value.ToString();
                }

                if (e.CellName == "lblNum2")
                {
                    for (int i = dataGridView1.SelectedRows.Count - 1; i >= 0; i--)
                    {
                        int gRow = e.RowIndex + dataGridView1.SelectedRows.Count - 1 - i;

                        if (gRow > 14)
                        {
                            break;
                        }

                        gcMultiRow1[gRow, "txtHinCode2"].Value = dataGridView1[colHinCode, dataGridView1.SelectedRows[i].Index].Value.ToString();
                        dataGridView1.SelectedRows[i].DefaultCellStyle.BackColor = Color.LightPink;
                    }

                    //gcMultiRow1[e.RowIndex, "lblHinName2"].Value = dataGridView1[colHinName, dataGridView1.SelectedRows[0].Index].Value.ToString();
                    //gcMultiRow1[e.RowIndex, "txtHinCode2"].Value = dataGridView1[colHinCode, dataGridView1.SelectedRows[0].Index].Value.ToString();
                    //gcMultiRow1[e.RowIndex, "lblIrisu2"].Value = dataGridView1[colIrisu, dataGridView1.SelectedRows[0].Index].Value.ToString();
                    //gcMultiRow1[e.RowIndex, "lblTani2"].Value = dataGridView1[colTani, dataGridView1.SelectedRows[0].Index].Value.ToString();
                }
            }
            
            //// 選択した商品を非表示にする
            //foreach (DataGridViewRow r in dataGridView1.SelectedRows)
            //{
            //    //dataGridView1.Rows.Remove(r);
            //    r.DefaultCellStyle.BackColor = Color.LightPink;
            //}

            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    if (dataGridView1.Rows[i].Selected)
            //    {
            //        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Ivory;
            //    }
            //}



            dataGridView1.CurrentCell = null;
            gcMultiRow1.CurrentCell = null;


        }

        private void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
        {
            if (!valueChangeStatus)
            {
                return;
            }

            valueChangeStatus = false;

            string hinCode = string.Empty;

            if (e.CellName == "txtHinCode")
            {
                hinCode = Utility.NulltoStr(gcMultiRow1[e.RowIndex, "txtHinCode"].Value).PadLeft(8, '0');

                if (hinCode != "00000000")
                {
                    gcMultiRow1[e.RowIndex, "txtHinCode"].Value = hinCode;
                }

                gcMultiRow1[e.RowIndex, "lblHinName"].Value = string.Empty;
                gcMultiRow1[e.RowIndex, "lblIrisu"].Value = string.Empty;
                gcMultiRow1[e.RowIndex, "lblTani"].Value = string.Empty;
            }
            else if (e.CellName == "txtHinCode2")
            {
                hinCode = Utility.NulltoStr(gcMultiRow1[e.RowIndex, "txtHinCode2"].Value).PadLeft(8, '0');

                if (hinCode != "00000000")
                {
                    gcMultiRow1[e.RowIndex, "txtHinCode2"].Value = hinCode;
                }

                gcMultiRow1[e.RowIndex, "lblHinName2"].Value = string.Empty;
                gcMultiRow1[e.RowIndex, "lblIrisu2"].Value = string.Empty;
                gcMultiRow1[e.RowIndex, "lblTani2"].Value = string.Empty;
            }

            using (var Conn = new OracleConnection())
            {
                Conn.ConnectionString = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                Conn.Open();

                string strSQL = "select SYO_ID, SYO_NAME, SYO_IRI_KESU, SYO_TANI from RAKUSYO_FAXOCR.V_SYOHIN WHERE SYO_ID = '" + hinCode + "'";
                OracleCommand Cmd = new OracleCommand(strSQL, Conn);
                OracleDataReader dR = Cmd.ExecuteReader();

                while (dR.Read())
                {
                    if (e.CellName == "txtHinCode")
                    {
                        gcMultiRow1[e.RowIndex, "lblHinName"].Value = dR["SYO_NAME"].ToString().Trim();
                        gcMultiRow1[e.RowIndex, "lblIrisu"].Value = dR["SYO_IRI_KESU"].ToString().Trim();
                        gcMultiRow1[e.RowIndex, "lblTani"].Value = dR["SYO_TANI"].ToString().Trim();
                    }
                    else if (e.CellName == "txtHinCode2")
                    {
                        gcMultiRow1[e.RowIndex, "lblHinName2"].Value = dR["SYO_NAME"].ToString().Trim();
                        gcMultiRow1[e.RowIndex, "lblIrisu2"].Value = dR["SYO_IRI_KESU"].ToString().Trim();
                        gcMultiRow1[e.RowIndex, "lblTani2"].Value = dR["SYO_TANI"].ToString().Trim();
                    }
                }

                dR.Dispose();
                Cmd.Dispose();
            }

            valueChangeStatus = true;
        }

        private void gcMultiRow2_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == Keys.F10)
            //{
            //    frmTodoke frm = new frmTodoke();
            //    frm.ShowDialog();

            //    if (frm._nouCode != string.Empty)
            //    {
            //        gcMultiRow2[0, "txtTdkNum"].Value = frm._nouCode;
            //    }

            //    //if (frm._nouName != string.Empty)
            //    //{
            //    //    gcMultiRow2[0, "labelCell3"].Value = frm._nouName;
            //    //}

            //    frm.Dispose();
            //    gcMultiRow2.CurrentCell = null;
            //}
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '\t')
                e.Handled = true;
        }

        private void Control_KeyDown2(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F12)
            {
                frmTodoke frm = new frmTodoke(true);
                frm.ShowDialog();

                if (frm._nouCode != null)
                {
                    int r =  dataGridView2.CurrentCell.RowIndex;

                    for (int i = 0; i < frm._nouCode.Length; i++)
                    {
                        if ((r + i) < 50)
                        {
                            dataGridView2[colTdkCode, r + i].Value = frm._nouCode[i];
                        }

                        //dataGridView2.Rows.Add();
                    }
                }

                // 後片付け
                frm.Dispose();
                dataGridView2.CurrentCell = null;
            }
        }

        private void gcMultiRow2_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            //if (e.Control is TextBoxEditingControl)
            //{
            //    //イベントハンドラが複数回追加されてしまうので最初に削除する
            //    e.Control.KeyDown -= new KeyEventHandler(Control_KeyDown);
            //    e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);

            //    //イベントハンドラを追加する
            //    if (gcMultiRow2.CurrentCell.Name == "txtTdkNum")
            //    {
            //        // お届け先検索画面表示
            //        e.Control.KeyDown += new KeyEventHandler(Control_KeyDown);

            //        // 数字のみ入力可能とする
            //        e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
            //    }
            //}
        }

        private void gcMultiRow1_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            if (e.Control is TextBoxEditingControl)
            {
                //イベントハンドラが複数回追加されてしまうので最初に削除する
                e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);

                // 数字のみ入力可能とする
                if (gcMultiRow1.CurrentCell.Name == "txtHinCode" || gcMultiRow1.CurrentCell.Name == "txtHinCode2")
                {
                    //イベントハンドラを追加する
                    e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }
            }
        }
        
        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                //イベントハンドラが複数回追加されてしまうので最初に削除する
                //e.Control.KeyDown -= new KeyEventHandler(Control_KeyDown2);
                e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);

                //イベントハンドラを追加する
                if (dataGridView2.CurrentCell.ColumnIndex == 0)
                {
                    // お届け先検索画面表示
                    //e.Control.KeyDown += new KeyEventHandler(Control_KeyDown2);

                    // 数字のみ入力可能とする
                    e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!valueChangeStatus)
            {
                return;
            }

            // 選択したお届け先件数を取得する
            int cnt = 0;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (Utility.NulltoStr(dataGridView2[colTdkCode, i].Value) != string.Empty)
                {
                    cnt++;
                }
            }

            // 届先単独指定のときのみ「商品履歴」ボタンは使用可
            if (cnt == 1)
            {
                button2.Enabled = true;
                comboBox1.Enabled = true;
                dateTimePicker1.Enabled = false;
            }
            else
            {
                // 届先1件以外は「商品履歴」ボタンは使用不可
                button2.Enabled = false;
                comboBox1.Enabled = false;
                dateTimePicker1.Enabled = false; ;
            }

            valueChangeStatus = false;

            if (e.ColumnIndex == 0)
            {
                string tdkCode = Utility.NulltoStr(dataGridView2[colTdkCode, e.RowIndex].Value).PadLeft(6, '0');

                if (tdkCode != "000000")
                {
                    dataGridView2[colTdkCode, e.RowIndex].Value = tdkCode;
                    dataGridView2.RefreshEdit();
                }

                dataGridView2[colTdkName, e.RowIndex].Value = string.Empty;

                // 届先名、電話番号、住所表示
                string gName = string.Empty;
                string gTel = string.Empty;
                string gJyu = string.Empty;

                gName = Utility.getNouhinName(tdkCode, out gTel, out gJyu);

                if (tdkCode != "000000" && gName == string.Empty)
                {
                    MessageBox.Show("未登録の届先番号です", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                dataGridView2[colTdkName, e.RowIndex].Value = Utility.getNouhinName(tdkCode, out gTel, out gJyu);
                dataGridView2[colTel, e.RowIndex].Value = gTel;
                dataGridView2[colAddress, e.RowIndex].Value = gJyu;

                if (dataGridView2[colTdkName, e.RowIndex].Value.ToString() != string.Empty)
                {
                    int seqNum = 0;
                    int tdNum = Utility.StrtoInt(tdkCode);

                    if (dts.パターンID.Any(a => a.届先番号 == tdNum))
                    {
                        // 現在の連番に「１」加算
                        seqNum = dts.パターンID.Where(a => a.届先番号 == tdNum).Max(a => a.連番);
                        seqNum++;
                        dataGridView2[colPtnNum, e.RowIndex].Value = seqNum.ToString();
                    }
                    else
                    {
                        // 初期値の「１」
                        dataGridView2[colPtnNum, e.RowIndex].Value = (seqNum + 1).ToString();
                    }
                }
                else
                {
                    dataGridView2[colPtnNum, e.RowIndex].Value = string.Empty;
                }

                dataGridView2.CurrentCell = null;
                valueChangeStatus = true;
            }
        }

        private void dataGridView2_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //string colName = dataGridView2.Columns[dataGridView2.CurrentCell.ColumnIndex].Name;
            //if (colName == colTdkCode)
            //{
            //    if (dataGridView2.IsCurrentCellDirty)
            //    {
            //        //コミットする
            //        dataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit);
            //        dataGridView2.RefreshEdit();
            //    }
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataUpdate();
        }

        private void dataUpdate()
        {
            // 届先確認
            if (getTdksaki() == 0)
            {
                MessageBox.Show("お届け先をひとつ以上選択してください", "お届先未選択", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dataGridView2.Focus();
                return;
            }

            int eCodeRow = sameTdkFind();
            if (eCodeRow != -1)
            {
                MessageBox.Show("同じお届け先が複数選択されています", "お届け先選択", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dataGridView2.Focus();
                dataGridView2.CurrentCell = dataGridView2[colTdkCode, eCodeRow];
                return;
            }

            eCodeRow = getErrTdksaki();
            if (eCodeRow != -1)
            {
                MessageBox.Show("マスター未登録の届先番号が選択されています", "お届け先選択", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dataGridView2.Focus();
                dataGridView2.CurrentCell = dataGridView2[colTdkCode, eCodeRow];
                return;
            }

            // 商品確認
            if (getPtnShohin() == 0)
            {
                MessageBox.Show("商品パターンを登録してください", "商品選択", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                gcMultiRow1.Focus();
                gcMultiRow1.CurrentCell = gcMultiRow1[0, "lblNum"];
                return;
            }

            // 登録確認
            int dCnt = 0;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (Utility.NulltoStr(dataGridView2[colTdkCode, i].Value) == string.Empty)
                {
                    continue;
                }
                dCnt++;
            }

            if (MessageBox.Show(dCnt + "件のお届け先の注文書パターンを登録します。よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            if (fMode == ADDMODE)
            {
                // 登録処理
                dataAdd(dataGridView2, gcMultiRow1);

                // 続けて登録確認：2017/08/21
                if (MessageBox.Show("現在の届先を続けて登録しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                {
                    // 画面初期化
                    dispInitial();
                }
                else
                {
                    // 一部初期化
                    continueInitial();
                }
            }
            else if (fMode == EDITMODE)
            {
                // 更新処理
                dataUpdate(fID, gcMultiRow1);

                // 画面初期化
                dispInitial();
            }
        }

        private void dataAdd(DataGridView g, GcMultiRow m)
        {
            try
            {
                for (int i = 0; i < g.Rows.Count; i++)
                {
                    if (Utility.NulltoStr(g[colTdkCode, i].Value) == string.Empty)
                    {
                        continue;
                    }

                    NHBRDataSet.パターンIDRow r = dts.パターンID.NewパターンIDRow();

                    r.届先番号 = Utility.StrtoInt(g[colTdkCode, i].Value.ToString());
                    r.連番 = Utility.StrtoInt(g[colPtnNum, i].Value.ToString());
                    r.商品1 = Utility.StrtoInt(Utility.NulltoStr(m[0, "txtHinCode"].Value));
                    r.商品2 = Utility.StrtoInt(Utility.NulltoStr(m[1, "txtHinCode"].Value));
                    r.商品3 = Utility.StrtoInt(Utility.NulltoStr(m[2, "txtHinCode"].Value));
                    r.商品4 = Utility.StrtoInt(Utility.NulltoStr(m[3, "txtHinCode"].Value));
                    r.商品5 = Utility.StrtoInt(Utility.NulltoStr(m[4, "txtHinCode"].Value));
                    r.商品6 = Utility.StrtoInt(Utility.NulltoStr(m[5, "txtHinCode"].Value));
                    r.商品7 = Utility.StrtoInt(Utility.NulltoStr(m[6, "txtHinCode"].Value));
                    r.商品8 = Utility.StrtoInt(Utility.NulltoStr(m[7, "txtHinCode"].Value));
                    r.商品9 = Utility.StrtoInt(Utility.NulltoStr(m[8, "txtHinCode"].Value));
                    r.商品10 = Utility.StrtoInt(Utility.NulltoStr(m[9, "txtHinCode"].Value));
                    r.商品11 = Utility.StrtoInt(Utility.NulltoStr(m[10, "txtHinCode"].Value));
                    r.商品12 = Utility.StrtoInt(Utility.NulltoStr(m[11, "txtHinCode"].Value));
                    r.商品13 = Utility.StrtoInt(Utility.NulltoStr(m[12, "txtHinCode"].Value));
                    r.商品14 = Utility.StrtoInt(Utility.NulltoStr(m[13, "txtHinCode"].Value));
                    r.商品15 = Utility.StrtoInt(Utility.NulltoStr(m[14, "txtHinCode"].Value));
                    r.商品16 = Utility.StrtoInt(Utility.NulltoStr(m[0, "txtHinCode2"].Value));
                    r.商品17 = Utility.StrtoInt(Utility.NulltoStr(m[1, "txtHinCode2"].Value));
                    r.商品18 = Utility.StrtoInt(Utility.NulltoStr(m[2, "txtHinCode2"].Value));
                    r.商品19 = Utility.StrtoInt(Utility.NulltoStr(m[3, "txtHinCode2"].Value));
                    r.商品20 = Utility.StrtoInt(Utility.NulltoStr(m[4, "txtHinCode2"].Value));
                    r.商品21 = Utility.StrtoInt(Utility.NulltoStr(m[5, "txtHinCode2"].Value));
                    r.商品22 = Utility.StrtoInt(Utility.NulltoStr(m[6, "txtHinCode2"].Value));
                    r.商品23 = Utility.StrtoInt(Utility.NulltoStr(m[7, "txtHinCode2"].Value));
                    r.商品24 = Utility.StrtoInt(Utility.NulltoStr(m[8, "txtHinCode2"].Value));
                    r.商品25 = Utility.StrtoInt(Utility.NulltoStr(m[9, "txtHinCode2"].Value));
                    r.商品26 = Utility.StrtoInt(Utility.NulltoStr(m[10, "txtHinCode2"].Value));
                    r.商品27 = Utility.StrtoInt(Utility.NulltoStr(m[11, "txtHinCode2"].Value));
                    r.商品28 = Utility.StrtoInt(Utility.NulltoStr(m[12, "txtHinCode2"].Value));
                    r.商品29 = Utility.StrtoInt(Utility.NulltoStr(m[13, "txtHinCode2"].Value));
                    r.商品30 = Utility.StrtoInt(Utility.NulltoStr(m[14, "txtHinCode2"].Value));
                    r.備考 = txtMemo.Text;
                    r.更新年月日 = DateTime.Now;

                    dts.パターンID.AddパターンIDRow(r);

                    // データベース更新
                    adp.Update(dts.パターンID);
                }

                MessageBox.Show("注文書パターンが登録されました", "処理終了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dataUpdate(int sID, GcMultiRow m)
        {
            try
            {
                NHBRDataSet.パターンIDRow r = dts.パターンID.Single(a => a.ID == sID);

                //r.届先番号 = Utility.StrtoInt(g[colTdkCode, i].Value.ToString());
                //r.連番 = Utility.StrtoInt(g[colPtnNum, i].Value.ToString());

                r.商品1 = Utility.StrtoInt(Utility.NulltoStr(m[0, "txtHinCode"].Value));
                r.商品2 = Utility.StrtoInt(Utility.NulltoStr(m[1, "txtHinCode"].Value));
                r.商品3 = Utility.StrtoInt(Utility.NulltoStr(m[2, "txtHinCode"].Value));
                r.商品4 = Utility.StrtoInt(Utility.NulltoStr(m[3, "txtHinCode"].Value));
                r.商品5 = Utility.StrtoInt(Utility.NulltoStr(m[4, "txtHinCode"].Value));
                r.商品6 = Utility.StrtoInt(Utility.NulltoStr(m[5, "txtHinCode"].Value));
                r.商品7 = Utility.StrtoInt(Utility.NulltoStr(m[6, "txtHinCode"].Value));
                r.商品8 = Utility.StrtoInt(Utility.NulltoStr(m[7, "txtHinCode"].Value));
                r.商品9 = Utility.StrtoInt(Utility.NulltoStr(m[8, "txtHinCode"].Value));
                r.商品10 = Utility.StrtoInt(Utility.NulltoStr(m[9, "txtHinCode"].Value));
                r.商品11 = Utility.StrtoInt(Utility.NulltoStr(m[10, "txtHinCode"].Value));
                r.商品12 = Utility.StrtoInt(Utility.NulltoStr(m[11, "txtHinCode"].Value));
                r.商品13 = Utility.StrtoInt(Utility.NulltoStr(m[12, "txtHinCode"].Value));
                r.商品14 = Utility.StrtoInt(Utility.NulltoStr(m[13, "txtHinCode"].Value));
                r.商品15 = Utility.StrtoInt(Utility.NulltoStr(m[14, "txtHinCode"].Value));
                r.商品16 = Utility.StrtoInt(Utility.NulltoStr(m[0, "txtHinCode2"].Value));
                r.商品17 = Utility.StrtoInt(Utility.NulltoStr(m[1, "txtHinCode2"].Value));
                r.商品18 = Utility.StrtoInt(Utility.NulltoStr(m[2, "txtHinCode2"].Value));
                r.商品19 = Utility.StrtoInt(Utility.NulltoStr(m[3, "txtHinCode2"].Value));
                r.商品20 = Utility.StrtoInt(Utility.NulltoStr(m[4, "txtHinCode2"].Value));
                r.商品21 = Utility.StrtoInt(Utility.NulltoStr(m[5, "txtHinCode2"].Value));
                r.商品22 = Utility.StrtoInt(Utility.NulltoStr(m[6, "txtHinCode2"].Value));
                r.商品23 = Utility.StrtoInt(Utility.NulltoStr(m[7, "txtHinCode2"].Value));
                r.商品24 = Utility.StrtoInt(Utility.NulltoStr(m[8, "txtHinCode2"].Value));
                r.商品25 = Utility.StrtoInt(Utility.NulltoStr(m[9, "txtHinCode2"].Value));
                r.商品26 = Utility.StrtoInt(Utility.NulltoStr(m[10, "txtHinCode2"].Value));
                r.商品27 = Utility.StrtoInt(Utility.NulltoStr(m[11, "txtHinCode2"].Value));
                r.商品28 = Utility.StrtoInt(Utility.NulltoStr(m[12, "txtHinCode2"].Value));
                r.商品29 = Utility.StrtoInt(Utility.NulltoStr(m[13, "txtHinCode2"].Value));
                r.商品30 = Utility.StrtoInt(Utility.NulltoStr(m[14, "txtHinCode2"].Value));
                r.備考 = txtMemo.Text;
                r.更新年月日 = DateTime.Now;

                // データベース更新
                adp.Update(dts.パターンID);

                MessageBox.Show("注文書パターンが更新されました", "処理終了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dataDelete(int sID)
        {
            NHBRDataSet.パターンIDRow r = dts.パターンID.Single(a => a.ID == sID);
            r.Delete();

            // データベース更新
            adp.Update(dts.パターンID);

            MessageBox.Show("注文書パターンが削除されました", "処理終了", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmPtnAdd_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 後片付け
            Dispose();
        }

        ///---------------------------------------------------------
        /// <summary>
        ///     登録届先数取得  </summary>
        /// <returns>
        ///     件数</returns>
        ///---------------------------------------------------------
        private int getTdksaki()
        {
            int cnt = 0;

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (Utility.NulltoStr(dataGridView2[colTdkCode, i].Value) == string.Empty)
                {
                    continue;
                }

                cnt++;
            }

            return cnt;
        }

        ///----------------------------------------------------------------------
        /// <summary>
        ///     マスター未登録の届先番号の行番号を取得する </summary>
        /// <returns>
        ///     行番号</returns>
        ///----------------------------------------------------------------------
        private int getErrTdksaki()
        {
            int cnt = -1;

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if ((Utility.NulltoStr(dataGridView2[colTdkCode, i].Value) != string.Empty) && 
                    (Utility.NulltoStr(dataGridView2[colTdkName, i].Value) == string.Empty))
                {
                    cnt = i;
                    break;
                }
            }

            return cnt;
        }

        private string getTdksakiCode(DataGridView g)
        {
            string nou_Code = string.Empty;

            for (int i = 0; i < g.Rows.Count; i++)
            {
                nou_Code = Utility.NulltoStr(g[colTdkCode, i].Value);

                if (nou_Code != string.Empty)
                {
                    break;
                }
            }

            return nou_Code;
        }

        private int getPtnShohin()
        {
            int cnt = 0;

            for (int i = 0; i < gcMultiRow1.RowCount; i++)
            {
                if (Utility.NulltoStr(gcMultiRow1[i, "txtHinCode"].Value) != string.Empty)
                {
                    cnt++;
                }

                if (Utility.NulltoStr(gcMultiRow1[i, "txtHinCode2"].Value) != string.Empty)
                {
                    cnt++;
                }
            }

            return cnt;
        }

        private int sameTdkFind()
        {
            int val = -1;

            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                if (val != -1)
                {
                    break;
                }

                string tdkCode = Utility.NulltoStr(dataGridView2[colTdkCode, i].Value);

                if (tdkCode == string.Empty)
                {
                    continue;
                }

                if (i < dataGridView2.RowCount - 1)
                {
                    for (int j = i + 1; j < dataGridView2.RowCount; j++)
                    {
                        if (Utility.NulltoStr(dataGridView2[colTdkCode, j].Value) == tdkCode)
                        {
                            val = j;
                            break;
                        }
                    }
                }
            }

            return val;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            rirekiGridviewSet(dataGridView1);

            string fDate = string.Empty;

            if (dateTimePicker1.Checked)
            {
                fDate = dateTimePicker1.Text;
            }

            // 商品履歴表示
            showShohinRireki(dataGridView1, getTdksakiCode(dataGridView2), fDate);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                //lblFrDate.Text = "";
                dateTimePicker1.Checked = false;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                // １ヶ月以内
                frDt = DateTime.Today.AddMonths(-1);
                dateTimePicker1.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                // ３ヶ月以内
                frDt = DateTime.Today.AddMonths(-3);
                dateTimePicker1.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                // ６ヶ月以内
                frDt = DateTime.Today.AddMonths(-6);
                dateTimePicker1.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                // １年以内
                frDt = DateTime.Today.AddYears(-1);
                dateTimePicker1.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 5)
            {
                // 期間を指定
                frDt = DateTime.Today;
                dateTimePicker1.Enabled = true;
            }

            if (comboBox1.SelectedIndex != 0)
            {
                //lblFrDate.Text = frDt.ToShortDateString() + "～";
                dateTimePicker1.Checked = true;
                dateTimePicker1.Value = frDt;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // パターン呼出
            callPattern();
        }

        private void callPattern()
        {
            // 画面初期化
            dispInitial();

            // パターン呼出
            adp.Fill(dts.パターンID);

            frmPtnCall frm = new frmPtnCall();
            frm.ShowDialog();

            if (frm.ptnID != string.Empty)
            {
                fID = Utility.StrtoInt(frm.ptnID);
                getPatterIDData(fID);

                fMode = EDITMODE;
                dataGridView2.ReadOnly = true;
                btnDel.Visible = true;
                button7.Enabled = false;
            }
            else
            {
                fID = 0;
            }

            // 後片付け
            frm.Dispose();
        }


        ///--------------------------------------------------------------------
        /// <summary>
        ///     登録済み注文書パターンを取得して表示する </summary>
        /// <param name="sID">
        ///     ID</param>
        ///--------------------------------------------------------------------
        private void getPatterIDData(int sID)
        {
            if (!dts.パターンID.Any(a => a.ID == sID))
            {
                MessageBox.Show("注文書パターンの取得に失敗しました。:" + sID.ToString());
                return;
            }

            var s = dts.パターンID.Single(a => a.ID == sID);

            // 届先グリッドは1行とする
            dataGridView2.Rows.Clear();
            dataGridView2.Rows.Add();
            dataGridView2[colTdkCode, 0].Value = s.届先番号.ToString().PadLeft(6, '0');

            valueChangeStatus = false;
            dataGridView2[colPtnNum, 0].Value = s.連番.ToString();

            valueChangeStatus = true;
            gcMultiRow1[0, "txtHinCode"].Value = Utility.ptnShohinStr(s.商品1);
            gcMultiRow1[1, "txtHinCode"].Value = Utility.ptnShohinStr(s.商品2);
            gcMultiRow1[2, "txtHinCode"].Value = Utility.ptnShohinStr(s.商品3);
            gcMultiRow1[3, "txtHinCode"].Value = Utility.ptnShohinStr(s.商品4);
            gcMultiRow1[4, "txtHinCode"].Value = Utility.ptnShohinStr(s.商品5);
            gcMultiRow1[5, "txtHinCode"].Value = Utility.ptnShohinStr(s.商品6);
            gcMultiRow1[6, "txtHinCode"].Value = Utility.ptnShohinStr(s.商品7);
            gcMultiRow1[7, "txtHinCode"].Value = Utility.ptnShohinStr(s.商品8);
            gcMultiRow1[8, "txtHinCode"].Value = Utility.ptnShohinStr(s.商品9);
            gcMultiRow1[9, "txtHinCode"].Value = Utility.ptnShohinStr(s.商品10);
            gcMultiRow1[10, "txtHinCode"].Value = Utility.ptnShohinStr(s.商品11);
            gcMultiRow1[11, "txtHinCode"].Value = Utility.ptnShohinStr(s.商品12);
            gcMultiRow1[12, "txtHinCode"].Value = Utility.ptnShohinStr(s.商品13);
            gcMultiRow1[13, "txtHinCode"].Value = Utility.ptnShohinStr(s.商品14);
            gcMultiRow1[14, "txtHinCode"].Value = Utility.ptnShohinStr(s.商品15);
            gcMultiRow1[0, "txtHinCode2"].Value = Utility.ptnShohinStr(s.商品16);
            gcMultiRow1[1, "txtHinCode2"].Value = Utility.ptnShohinStr(s.商品17);
            gcMultiRow1[2, "txtHinCode2"].Value = Utility.ptnShohinStr(s.商品18);
            gcMultiRow1[3, "txtHinCode2"].Value = Utility.ptnShohinStr(s.商品19);
            gcMultiRow1[4, "txtHinCode2"].Value = Utility.ptnShohinStr(s.商品20);
            gcMultiRow1[5, "txtHinCode2"].Value = Utility.ptnShohinStr(s.商品21);
            gcMultiRow1[6, "txtHinCode2"].Value = Utility.ptnShohinStr(s.商品22);
            gcMultiRow1[7, "txtHinCode2"].Value = Utility.ptnShohinStr(s.商品23);
            gcMultiRow1[8, "txtHinCode2"].Value = Utility.ptnShohinStr(s.商品24);
            gcMultiRow1[9, "txtHinCode2"].Value = Utility.ptnShohinStr(s.商品25);
            gcMultiRow1[10, "txtHinCode2"].Value = Utility.ptnShohinStr(s.商品26);
            gcMultiRow1[11, "txtHinCode2"].Value = Utility.ptnShohinStr(s.商品27);
            gcMultiRow1[12, "txtHinCode2"].Value = Utility.ptnShohinStr(s.商品28);
            gcMultiRow1[13, "txtHinCode2"].Value = Utility.ptnShohinStr(s.商品29);
            gcMultiRow1[14, "txtHinCode2"].Value = Utility.ptnShohinStr(s.商品30);

            txtMemo.Text = s.備考;
        }

        //private string ptnShohinStr(int s)
        //{
        //    string val = string.Empty;

        //    if (s == global.flgOff)
        //    {
        //        val = string.Empty;
        //    }
        //    else
        //    {
        //        val = s.ToString().PadLeft(8, '0');
        //    }

        //    return val;
        //}

        private void button6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("画面を初期化します。よろしいですか", "取消確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            dispInitial();
        }

        private void dataGridView2_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                valueChangeStatus = false;

                string tdkCode = Utility.NulltoStr(dataGridView2[colTdkCode, e.RowIndex].Value).PadLeft(6, '0');

                if (tdkCode != "000000")
                {
                    dataGridView2[colTdkCode, e.RowIndex].Value = tdkCode;
                }

                valueChangeStatus = true;
            }
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == Keys.F12)
            //{
            //    frmTodoke frm = new frmTodoke(true);
            //    frm.ShowDialog();

            //    if (frm._nouCode != null)
            //    {
            //        int r = dataGridView2.CurrentCell.RowIndex;

            //        for (int i = 0; i < frm._nouCode.Length; i++)
            //        {
            //            if ((r + i) < 50)
            //            {
            //                dataGridView2[colTdkCode, r + i].Value = frm._nouCode[i];
            //            }

            //            //dataGridView2.Rows.Add();
            //        }
            //    }

            //    // 後片付け
            //    frm.Dispose();
            //    dataGridView2.CurrentCell = null;
            //}
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmTodoke frm = new frmTodoke(true);
            frm.ShowDialog();

            if (frm._nouCode != null)
            {
                //int r = dataGridView2.CurrentCell.RowIndex;
                bool edt;
                int r = 0;

                // 上書きセル指定か？
                if (dataGridView2.CurrentCell == null)
                {
                    edt = false;
                }
                else
                {
                    edt = true;
                    r = dataGridView2.CurrentCell.RowIndex;
                }

                for (int i = 0; i < frm._nouCode.Length; i++)
                {
                    // 新規追加登録
                    if (!edt)
                    {
                        for (int iX = 0; iX < 50; iX++)
                        {
                            if (dataGridView2[colTdkCode, iX].Value == null ||
                                dataGridView2[colTdkCode, iX].Value.ToString() == string.Empty)
                            {
                                dataGridView2[colTdkCode, iX].Value = frm._nouCode[i];
                                break;
                            }
                        }
                    }
                    else
                    {
                        // 上書き
                        if ((r + i) < 50)
                        {
                            dataGridView2[colTdkCode, r + i].Value = frm._nouCode[i];
                        }
                    }
                }
            }

            // 後片付け
            frm.Dispose();
            dataGridView2.CurrentCell = null;
        }

        private void frmPtnAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F8)
            {
                // パターン呼出
                callPattern();
            }

            if (e.KeyData == Keys.F9)
            {
                if (MessageBox.Show("画面を初期化します。よろしいですか", "取消確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                dispInitial();
            }

            if (e.KeyData == Keys.F10)
            {
                if (MessageBox.Show("注文商品を初期化します。よろしいですか", "取消確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                valueChangeStatus = false;
                gcMrSetting();
                valueChangeStatus = true;
            }

            if (e.KeyData == Keys.F11)
            {
                // 更新
                dataUpdate();
            }

            if (e.KeyData == Keys.F12)
            {
                // 終了
                Close();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("注文商品を初期化します。よろしいですか", "取消確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            valueChangeStatus = false;
            gcMrSetting();
            valueChangeStatus = true;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("表示中の注文書パターンを削除します。よろしいですか", "削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            // 削除処理
            dataDelete(fID);

            // 画面初期化
            dispInitial();
        }

        private void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Name == colSuu || e.Column.Name == colIrisu)
            {
                e.SortResult = Utility.StrtoInt(Utility.NulltoStr(e.CellValue1)) - Utility.StrtoInt(Utility.NulltoStr(e.CellValue2));
                e.Handled = true;
            }
        }

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            int s = dataGridView1.SelectedRows.Count;
            if (s > 15)
            {
                MessageBox.Show(s + "件選択されています。16件以上は選択できません。", "選択制限数オーバー",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dataGridView1.ClearSelection();
            }
        }
    }
}
