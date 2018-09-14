﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using NHBR_OCR.common;
using NHBR_OCR.OCR;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using GrapeCity.Win.MultiRow;
using Excel = Microsoft.Office.Interop.Excel;

namespace NHBR_OCR.OCR
{
    public partial class frmCorrect : Form
    {
        /// ------------------------------------------------------------
        /// <summary>
        ///     コンストラクタ </summary>
        /// <param name="myCode">
        ///     入力担当者コード</param>
        /// ------------------------------------------------------------
        public frmCorrect(string myCode)
        {
            InitializeComponent();

            //_dbName = dbName;       // データベース名
            //_comName = comName;     // 会社名
            //dID = sID;              // 処理モード
            //_eMode = eMode;         // 処理モード2

            _myCode = myCode;       // 担当者コード

            // パターンIDデータ読み込み
            pAdp.Fill(dts.パターンID);

            // 環境設定読み込み
            cAdp.Fill(dts.環境設定);

            // 出荷基準設定
            skAdp.Fill(dts.出荷基準設定);
        }

        // データアダプターオブジェクト
        NHBR_CLIDataSetTableAdapters.FAX注文書TableAdapter fAdp = new NHBR_CLIDataSetTableAdapters.FAX注文書TableAdapter();
        NHBRDataSetTableAdapters.パターンIDTableAdapter pAdp = new NHBRDataSetTableAdapters.パターンIDTableAdapter();
        NHBRDataSetTableAdapters.出荷基準設定TableAdapter sAdp = new NHBRDataSetTableAdapters.出荷基準設定TableAdapter();
        NHBRDataSetTableAdapters.環境設定TableAdapter cAdp = new NHBRDataSetTableAdapters.環境設定TableAdapter();
        NHBRDataSetTableAdapters.出荷基準設定TableAdapter skAdp = new NHBRDataSetTableAdapters.出荷基準設定TableAdapter();

        // データセットオブジェクト
        NHBRDataSet dts = new NHBRDataSet();
        NHBR_CLIDataSet dtsC = new NHBR_CLIDataSet();

        // セル値
        private string cellName = string.Empty;         // セル名
        private string cellBeforeValue = string.Empty;  // 編集前
        private string cellAfterValue = string.Empty;   // 編集後

        #region 編集ログ・項目名 2015/09/08
        private const string LOG_YEAR = "年";
        private const string LOG_MONTH = "月";
        private const string LOG_DAY = "日";
        private const string LOG_TAIKEICD = "体系コード";
        private const string CELL_TORIKESHI = "取消";
        private const string CELL_NUMBER = "社員番号";
        private const string CELL_KIGOU = "記号";
        private const string CELL_FUTSU = "普通残業・時";
        private const string CELL_FUTSU_M = "普通残業・分";
        private const string CELL_SHINYA = "深夜残業・時";
        private const string CELL_SHINYA_M = "深夜残業・分";
        private const string CELL_SHIGYO = "始業時刻・時";
        private const string CELL_SHIGYO_M = "始業時刻・分";
        private const string CELL_SHUUGYO = "終業時刻・時";
        private const string CELL_SHUUGYO_M = "終業時刻・分";
        #endregion 編集ログ・項目名

        // カレント社員情報
        //SCCSDataSet.社員所属Row cSR = null;
        
        // 社員マスターより取得した所属コード
        string mSzCode = string.Empty;

        #region 終了ステータス定数
        const string END_BUTTON = "btn";
        const string END_MAKEDATA = "data";
        const string END_CONTOROL = "close";
        const string END_NODATA = "non Data";
        #endregion

        string dID = string.Empty;                  // 表示する過去データのID
        string sDBNM = string.Empty;                // データベース名

        string _dbName = string.Empty;           // 会社領域データベース識別番号
        string _comNo = string.Empty;            // 会社番号
        string _comName = string.Empty;          // 会社名
        string _myCode = string.Empty;           // 担当者コード
        string _imgFile = string.Empty;         // 画像名

        bool _eMode = true;

        // dataGridView1_CellEnterステータス
        bool gridViewCellEnterStatus = true;

        // 編集ログ書き込み状態
        bool editLogStatus = false;
        
        // カレントデータRowsインデックス
        string [] cID = null;
        int cI = 0;

        // グローバルクラス
        global gl = new global();

        OracleConnection Conn = new OracleConnection();

        // 画面表示時ステータス
        bool showStatus = false;
                
        private void frmCorrect_Load(object sender, EventArgs e)
        {
            //this.pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            // フォーム最大値
            Utility.WindowsMaxSize(this, this.Width, this.Height);

            // フォーム最小値
            Utility.WindowsMinSize(this, this.Width, this.Height);

            // Tabキーの既定のショートカットキーを解除する。
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow2.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow3.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow2.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow3.ShortcutKeyManager.Unregister(Keys.Enter);

            // Tabキーのショートカットキーにユーザー定義のショートカットキーを割り当てる。
            gcMultiRow1.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Tab);
            gcMultiRow2.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Tab);
            gcMultiRow3.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Enter);
            gcMultiRow2.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Enter);
            gcMultiRow3.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Enter);
            
            // 勤務データ登録
            if (dID == string.Empty)
            {
                // CSVデータをMDBへ読み込みます
                GetCsvDataToMDB();

                // データセットへＦＡＸ注文書データを読み込みます
                getDataSet();

                // データテーブル件数カウント
                if (dtsC.FAX注文書.Count == 0)
                {
                    MessageBox.Show("ＦＡＸ発注書データがありません", "発注書登録", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    //終了処理
                    Environment.Exit(0);
                }

                // キー配列作成
                keyArrayCreate();
            }
            
            // キャプション
            this.Text = "ＦＡＸ発注書表示";
                        
            // 楽商データベース接続
            Conn.ConnectionString = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
            Conn.Open();

            // GCMultiRow初期化
            gcMrSetting();

            // 編集作業、過去データ表示の判断
            if (dID == string.Empty) // パラメータのヘッダIDがないときは編集作業
            {
                // 最初のレコードを表示
                cI = 0;
                showOcrData(cI);
            }

            // tagを初期化
            this.Tag = string.Empty;

            // 現在の表示倍率を初期化
            gl.miMdlZoomRate = 0f;
        }

        ///-------------------------------------------------------------
        /// <summary>
        ///     キー配列作成 </summary>
        ///-------------------------------------------------------------
        private void keyArrayCreate()
        {
            int iX = 0;
            foreach (var t in dtsC.FAX注文書.OrderBy(a => a.ID))
            {
                Array.Resize(ref cID, iX + 1);
                cID[iX] = t.ID;
                iX++;
            }
        }

        #region データグリッドビューカラム定義
        private static string cCheck = "col1";      // 取消
        private static string cShainNum = "col2";   // 社員番号
        private static string cName = "col3";       // 氏名
        private static string cKinmu = "col4";      // 勤務記号
        private static string cZH = "col5";         // 残業時
        private static string cZE = "col6";         // :
        private static string cZM = "col7";         // 残業分
        private static string cSIH = "col8";        // 深夜時
        private static string cSIE = "col9";        // :
        private static string cSIM = "col10";       // 深夜分
        private static string cSH = "col11";        // 開始時
        private static string cSE = "col12";        // :
        private static string cSM = "col13";        // 開始分
        private static string cEH = "col14";        // 終了時
        private static string cEE = "col15";        // :
        private static string cEM = "col16";        // 終了分
        //private static string cID = "colID";        // ID
        private static string cSzCode = "colSzCode";  // 所属コード
        private static string cSzName = "colSzName";  // 所属名

        #endregion

        private void gcMrSetting()
        {
            //multirow編集モード
            gcMultiRow1.EditMode = EditMode.EditProgrammatically;

            this.gcMultiRow1.AllowUserToAddRows = false;                    // 手動による行追加を禁止する
            this.gcMultiRow1.AllowUserToDeleteRows = false;                 // 手動による行削除を禁止する
            //this.gcMultiRow1.Rows.Clear();                                  // 行数をクリア
            this.gcMultiRow1.RowCount = 1;                                  // 行数を設定
            this.gcMultiRow1.HideSelection = true;                          // GcMultiRow コントロールがフォーカスを失ったとき、セルの選択状態を非表示にする

            //multirow編集モード
            gcMultiRow2.EditMode = EditMode.EditProgrammatically;

            this.gcMultiRow2.AllowUserToAddRows = false;                    // 手動による行追加を禁止する
            this.gcMultiRow2.AllowUserToDeleteRows = false;                 // 手動による行削除を禁止する
            //this.gcMultiRow2.Rows.Clear();                                  // 行数をクリア
            this.gcMultiRow2.RowCount = global.MAX_GYO;                                  // 行数を設定
            this.gcMultiRow2.HideSelection = true;                          // GcMultiRow コントロールがフォーカスを失ったとき、セルの選択状態を非表示にする
            
            //multirow編集モード
            gcMultiRow3.EditMode = EditMode.EditProgrammatically;

            this.gcMultiRow3.AllowUserToAddRows = false;                    // 手動による行追加を禁止する
            this.gcMultiRow3.AllowUserToDeleteRows = false;                 // 手動による行削除を禁止する
            //this.gcMultiRow3.Rows.Clear();                                  // 行数をクリア
            this.gcMultiRow3.RowCount = 5;                                  // 行数を設定
            this.gcMultiRow3.HideSelection = true;                          // GcMultiRow コントロールがフォーカスを失ったとき、セルの選択状態を非表示にする

        }

        ///----------------------------------------------------------------------------
        /// <summary>
        ///     CSVデータをMDBへインサートする</summary>
        ///----------------------------------------------------------------------------
        private void GetCsvDataToMDB()
        {
            // CSVファイル数をカウント
            string[] inCsv = System.IO.Directory.GetFiles(Properties.Settings.Default.mydataPath, "*.csv");

            // CSVファイルがなければ終了
            if (inCsv.Length == 0) return;

            // オーナーフォームを無効にする
            this.Enabled = false;

            //プログレスバーを表示する
            frmPrg frmP = new frmPrg();
            frmP.Owner = this;
            frmP.Show();

            // OCRのCSVデータをMDBへ取り込む
            OCRData ocr = new OCRData(Conn);
            ocr.CsvToMdb(Properties.Settings.Default.mydataPath, frmP, _myCode);

            // いったんオーナーをアクティブにする
            this.Activate();

            // 進行状況ダイアログを閉じる
            frmP.Close();

            // オーナーのフォームを有効に戻す
            this.Enabled = true;
        }

        private void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //if (e.Control is DataGridViewTextBoxEditingControl)
            //{
            //    // 数字のみ入力可能とする
            //    if (dGV.CurrentCell.ColumnIndex != 0 && dGV.CurrentCell.ColumnIndex != 2)
            //    {
            //        //イベントハンドラが複数回追加されてしまうので最初に削除する
            //        e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
            //        e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress2);

            //        //イベントハンドラを追加する
            //        e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
            //    }
            //}
        }

        void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '\t')
                e.Handled = true;
        }

        void Control_KeyPress2(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar >= 'a' && e.KeyChar <= 'z') ||
                e.KeyChar == '\b' || e.KeyChar == '\t')
                e.Handled = false;
            else e.Handled = true;
        }

        void Control_KeyPress3(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '0' && e.KeyChar != '5' && e.KeyChar != '\b' && e.KeyChar != '\t')
                e.Handled = true;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void frmCorrect_Shown(object sender, EventArgs e)
        {
            //if (dID != string.Empty) lnkRtn.Focus();
        }

        private void dataGridView3_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                //イベントハンドラが複数回追加されてしまうので最初に削除する
                e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
                //イベントハンドラを追加する
                e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
            }
        }

        private void dataGridView4_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                //イベントハンドラが複数回追加されてしまうので最初に削除する
                e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
                //イベントハンドラを追加する
                e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
        }

        ///-----------------------------------------------------------------------------------
        /// <summary>
        ///     カレントデータを更新する</summary>
        /// <param name="iX">
        ///     カレントレコードのインデックス</param>
        ///-----------------------------------------------------------------------------------
        private void CurDataUpDate(string iX)
        {
            // エラーメッセージ
            string errMsg = "ＦＡＸ発注書テーブル更新";

            try
            {
                // ＦＡＸ発注書を取得
                NHBR_CLIDataSet.FAX注文書Row r = dtsC.FAX注文書.Single(a => a.ID == iX);

                // ＦＡＸ発注書テーブルセット更新
                r.届先番号 = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[0, "txtTdkNum"].Value));
                r.パターンID = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[0, "txtPtnNum"].Value));
                r.発注番号 = Utility.NulltoStr(gcMultiRow1[0, "txtOrderNum"].Value);
                r.納品希望月 = Utility.NulltoStr(gcMultiRow1[0, "txtMonth"].Value);
                r.納品希望日 = Utility.NulltoStr(gcMultiRow1[0, "txtDay"].Value);

                // 2018/08/02
                if (Convert.ToInt32(gcMultiRow1[0, "chkReFax"].Value) == global.flgOff)
                {
                    r.メモ = txtMemo.Text;
                }
                else
                {
                    r.メモ = txtMemo.Text + global.REFAX;
                }

                r.注文数1 = Utility.NulltoStr(gcMultiRow2[0, "txtSuu"].Value);
                r.注文数2 = Utility.NulltoStr(gcMultiRow2[1, "txtSuu"].Value);
                r.注文数3 = Utility.NulltoStr(gcMultiRow2[2, "txtSuu"].Value);
                r.注文数4 = Utility.NulltoStr(gcMultiRow2[3, "txtSuu"].Value);
                r.注文数5 = Utility.NulltoStr(gcMultiRow2[4, "txtSuu"].Value);
                r.注文数6 = Utility.NulltoStr(gcMultiRow2[5, "txtSuu"].Value);
                r.注文数7 = Utility.NulltoStr(gcMultiRow2[6, "txtSuu"].Value);
                r.注文数8 = Utility.NulltoStr(gcMultiRow2[7, "txtSuu"].Value);
                r.注文数9 = Utility.NulltoStr(gcMultiRow2[8, "txtSuu"].Value);
                r.注文数10 = Utility.NulltoStr(gcMultiRow2[9, "txtSuu"].Value);
                r.注文数11 = Utility.NulltoStr(gcMultiRow2[10, "txtSuu"].Value);
                r.注文数12 = Utility.NulltoStr(gcMultiRow2[11, "txtSuu"].Value);
                r.注文数13 = Utility.NulltoStr(gcMultiRow2[12, "txtSuu"].Value);
                r.注文数14 = Utility.NulltoStr(gcMultiRow2[13, "txtSuu"].Value);
                r.注文数15 = Utility.NulltoStr(gcMultiRow2[14, "txtSuu"].Value);
                r.注文数16 = Utility.NulltoStr(gcMultiRow2[0, "txtSuu2"].Value);
                r.注文数17 = Utility.NulltoStr(gcMultiRow2[1, "txtSuu2"].Value);
                r.注文数18 = Utility.NulltoStr(gcMultiRow2[2, "txtSuu2"].Value);
                r.注文数19 = Utility.NulltoStr(gcMultiRow2[3, "txtSuu2"].Value);
                r.注文数20 = Utility.NulltoStr(gcMultiRow2[4, "txtSuu2"].Value);
                r.注文数21 = Utility.NulltoStr(gcMultiRow2[5, "txtSuu2"].Value);
                r.注文数22 = Utility.NulltoStr(gcMultiRow2[6, "txtSuu2"].Value);
                r.注文数23 = Utility.NulltoStr(gcMultiRow2[7, "txtSuu2"].Value);
                r.注文数24 = Utility.NulltoStr(gcMultiRow2[8, "txtSuu2"].Value);
                r.注文数25 = Utility.NulltoStr(gcMultiRow2[9, "txtSuu2"].Value);
                r.注文数26 = Utility.NulltoStr(gcMultiRow2[10, "txtSuu2"].Value);
                r.注文数27 = Utility.NulltoStr(gcMultiRow2[11, "txtSuu2"].Value);
                r.注文数28 = Utility.NulltoStr(gcMultiRow2[12, "txtSuu2"].Value);
                r.注文数29 = Utility.NulltoStr(gcMultiRow2[13, "txtSuu2"].Value);
                r.注文数30 = Utility.NulltoStr(gcMultiRow2[14, "txtSuu2"].Value);

                r.追加注文商品コード1 = Utility.NulltoStr(gcMultiRow3[0, "txtHinCode"].Value);
                r.追加注文商品コード2 = Utility.NulltoStr(gcMultiRow3[1, "txtHinCode"].Value);
                r.追加注文商品コード3 = Utility.NulltoStr(gcMultiRow3[2, "txtHinCode"].Value);
                r.追加注文商品コード4 = Utility.NulltoStr(gcMultiRow3[3, "txtHinCode"].Value);
                r.追加注文商品コード5 = Utility.NulltoStr(gcMultiRow3[4, "txtHinCode"].Value);

                r.追加注文商品コード6 = Utility.NulltoStr(gcMultiRow3[0, "txtHinCode2"].Value);
                r.追加注文商品コード7 = Utility.NulltoStr(gcMultiRow3[1, "txtHinCode2"].Value);
                r.追加注文商品コード8 = Utility.NulltoStr(gcMultiRow3[2, "txtHinCode2"].Value);
                r.追加注文商品コード9 = Utility.NulltoStr(gcMultiRow3[3, "txtHinCode2"].Value);
                r.追加注文商品コード10 = Utility.NulltoStr(gcMultiRow3[4, "txtHinCode2"].Value);
                
                r.追加注文数1 = Utility.NulltoStr(gcMultiRow3[0, "txtSuu"].Value);
                r.追加注文数2 = Utility.NulltoStr(gcMultiRow3[1, "txtSuu"].Value);
                r.追加注文数3 = Utility.NulltoStr(gcMultiRow3[2, "txtSuu"].Value);
                r.追加注文数4 = Utility.NulltoStr(gcMultiRow3[3, "txtSuu"].Value);
                r.追加注文数5 = Utility.NulltoStr(gcMultiRow3[4, "txtSuu"].Value);
                
                r.追加注文数6 = Utility.NulltoStr(gcMultiRow3[0, "txtSuu2"].Value);
                r.追加注文数7 = Utility.NulltoStr(gcMultiRow3[1, "txtSuu2"].Value);
                r.追加注文数8 = Utility.NulltoStr(gcMultiRow3[2, "txtSuu2"].Value);
                r.追加注文数9 = Utility.NulltoStr(gcMultiRow3[3, "txtSuu2"].Value);
                r.追加注文数10 = Utility.NulltoStr(gcMultiRow3[4, "txtSuu2"].Value);

                r.担当者コード = _myCode;

                //r.メモ = txtMemo.Text;

                r.エラー有無 = Utility.StrtoInt(txtErrStatus.Text);

                r.更新年月日 = DateTime.Now;

                if (checkBox1.Checked)
                {
                    r.確認 = global.flgOn;
                }
                else
                {
                    r.確認 = global.flgOff;
                }

                r.出荷基準A = kigouToNum(lblGrpA.Text);
                r.出荷基準B = kigouToNum(lblGrpB.Text);
                r.出荷基準C = kigouToNum(lblGrpC.Text);
                r.出荷基準D = kigouToNum(lblGrpD.Text);
                r.出荷基準E = kigouToNum(lblGrpE.Text);
                r.出荷基準F = kigouToNum(lblGrpF.Text);
                r.出荷基準G = kigouToNum(lblGrpG.Text);

                // フリー入力商品コード：2017/08/22
                if (Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[0, "txtPtnNum"].Value)) != global.flgOff)
                {
                    r.商品コード1 = string.Empty;
                    r.商品コード2 = string.Empty;
                    r.商品コード3 = string.Empty;
                    r.商品コード4 = string.Empty;
                    r.商品コード5 = string.Empty;
                    r.商品コード6 = string.Empty;
                    r.商品コード7 = string.Empty;
                    r.商品コード8 = string.Empty;
                    r.商品コード9 = string.Empty;
                    r.商品コード10 = string.Empty;
                    r.商品コード11 = string.Empty;
                    r.商品コード12 = string.Empty;
                    r.商品コード13 = string.Empty;
                    r.商品コード14 = string.Empty;
                    r.商品コード15 = string.Empty;
                    r.商品コード16 = string.Empty;
                    r.商品コード17 = string.Empty;
                    r.商品コード18 = string.Empty;
                    r.商品コード19 = string.Empty;
                    r.商品コード20 = string.Empty;
                    r.商品コード21 = string.Empty;
                    r.商品コード22 = string.Empty;
                    r.商品コード23 = string.Empty;
                    r.商品コード24 = string.Empty;
                    r.商品コード25 = string.Empty;
                    r.商品コード26 = string.Empty;
                    r.商品コード27 = string.Empty;
                    r.商品コード28 = string.Empty;
                    r.商品コード29 = string.Empty;
                    r.商品コード30 = string.Empty;
                }
                else
                {
                    r.商品コード1 = Utility.NulltoStr(gcMultiRow2[0, "txtHinCode"].Value);
                    r.商品コード2 = Utility.NulltoStr(gcMultiRow2[1, "txtHinCode"].Value);
                    r.商品コード3 = Utility.NulltoStr(gcMultiRow2[2, "txtHinCode"].Value);
                    r.商品コード4 = Utility.NulltoStr(gcMultiRow2[3, "txtHinCode"].Value);
                    r.商品コード5 = Utility.NulltoStr(gcMultiRow2[4, "txtHinCode"].Value);
                    r.商品コード6 = Utility.NulltoStr(gcMultiRow2[5, "txtHinCode"].Value);
                    r.商品コード7 = Utility.NulltoStr(gcMultiRow2[6, "txtHinCode"].Value);
                    r.商品コード8 = Utility.NulltoStr(gcMultiRow2[7, "txtHinCode"].Value);
                    r.商品コード9 = Utility.NulltoStr(gcMultiRow2[8, "txtHinCode"].Value);
                    r.商品コード10 = Utility.NulltoStr(gcMultiRow2[9, "txtHinCode"].Value);
                    r.商品コード11 = Utility.NulltoStr(gcMultiRow2[10, "txtHinCode"].Value);
                    r.商品コード12 = Utility.NulltoStr(gcMultiRow2[11, "txtHinCode"].Value);
                    r.商品コード13 = Utility.NulltoStr(gcMultiRow2[12, "txtHinCode"].Value);
                    r.商品コード14 = Utility.NulltoStr(gcMultiRow2[13, "txtHinCode"].Value);
                    r.商品コード15 = Utility.NulltoStr(gcMultiRow2[14, "txtHinCode"].Value);
                    r.商品コード16 = Utility.NulltoStr(gcMultiRow2[0, "txtHinCode2"].Value);
                    r.商品コード17 = Utility.NulltoStr(gcMultiRow2[1, "txtHinCode2"].Value);
                    r.商品コード18 = Utility.NulltoStr(gcMultiRow2[2, "txtHinCode2"].Value);
                    r.商品コード19 = Utility.NulltoStr(gcMultiRow2[3, "txtHinCode2"].Value);
                    r.商品コード20 = Utility.NulltoStr(gcMultiRow2[4, "txtHinCode2"].Value);
                    r.商品コード21 = Utility.NulltoStr(gcMultiRow2[5, "txtHinCode2"].Value);
                    r.商品コード22 = Utility.NulltoStr(gcMultiRow2[6, "txtHinCode2"].Value);
                    r.商品コード23 = Utility.NulltoStr(gcMultiRow2[7, "txtHinCode2"].Value);
                    r.商品コード24 = Utility.NulltoStr(gcMultiRow2[8, "txtHinCode2"].Value);
                    r.商品コード25 = Utility.NulltoStr(gcMultiRow2[9, "txtHinCode2"].Value);
                    r.商品コード26 = Utility.NulltoStr(gcMultiRow2[10, "txtHinCode2"].Value);
                    r.商品コード27 = Utility.NulltoStr(gcMultiRow2[11, "txtHinCode2"].Value);
                    r.商品コード28 = Utility.NulltoStr(gcMultiRow2[12, "txtHinCode2"].Value);
                    r.商品コード29 = Utility.NulltoStr(gcMultiRow2[13, "txtHinCode2"].Value);
                    r.商品コード30 = Utility.NulltoStr(gcMultiRow2[14, "txtHinCode2"].Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, errMsg, MessageBoxButtons.OK);
            }
            finally
            {
            }
        }

        ///----------------------------------------------------------------
        /// <summary>
        ///     出荷基準判定記号を数値に変換する </summary>
        /// <param name="str">
        ///     出荷基準判定記号</param>
        /// <returns>
        ///     数値（0:◯、1:✕、2:－）</returns>
        ///----------------------------------------------------------------
        private string kigouToNum(string str)
        {
            string rtn = string.Empty;

            switch (str)
            {
                case "◯":
                    rtn = global.FLGOFF;
                    break;

                case "✕":
                    rtn = global.FLGON;
                    break;
                
                default:
                    rtn = "2";
                    break;
            }

            return rtn;
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     空白以外のとき、指定された文字数になるまで左側に０を埋めこみ、右寄せした文字列を返す
        /// </summary>
        /// <param name="tm">
        ///     文字列</param>
        /// <param name="len">
        ///     文字列の長さ</param>
        /// <returns>
        ///     文字列</returns>
        /// ----------------------------------------------------------------------------------------------------
        private string timeVal(object tm, int len)
        {
            string t = Utility.NulltoStr(tm);
            if (t != string.Empty) return t.PadLeft(len, '0');
            else return t;
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     空白以外のとき、先頭文字が０のとき先頭文字を削除した文字列を返す　
        ///     先頭文字が０以外のときはそのまま返す
        /// </summary>
        /// <param name="tm">
        ///     文字列</param>
        /// <returns>
        ///     文字列</returns>
        /// ----------------------------------------------------------------------------------------------------
        private string timeValH(object tm)
        {
            string t = Utility.NulltoStr(tm);

            if (t != string.Empty)
            {
                t = t.PadLeft(2, '0');
                if (t.Substring(0, 1) == "0")
                {
                    t = t.Substring(1, 1);
                }
            }

            return t;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        ///     Bool値を数値に変換する </summary>
        /// <param name="b">
        ///     True or False</param>
        /// <returns>
        ///     true:1, false:0</returns>
        /// ------------------------------------------------------------------------------------
        private int booltoFlg(string b)
        {
            if (b == "True") return global.flgOn;
            else return global.flgOff;
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
        }

        private void btnBefore_Click(object sender, EventArgs e)
        {
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
        }

        ///-----------------------------------------------------------------
        /// <summary>
        ///     エラーチェックボタン </summary>
        /// <param name="sender">
        ///     </param>
        /// <param name="e">
        ///     </param>
        ///-----------------------------------------------------------------
        private void btnErrCheck_Click(object sender, EventArgs e)
        {
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            //カレントデータの更新
            CurDataUpDate(cID[cI]);

            //レコードの移動
            cI = hScrollBar1.Value;
            showOcrData(cI);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
        }

        ///-------------------------------------------------------------------------------
        /// <summary>
        ///     １．指定した勤務票ヘッダデータと勤務票明細データを削除する　
        ///     ２．該当する画像データを削除する</summary>
        /// <param name="i">
        ///     勤務票ヘッダRow インデックス</param>
        ///-------------------------------------------------------------------------------
        private void DataDelete(int i)
        {
            string sImgNm = string.Empty;
            string errMsg = string.Empty;

            // 勤務票データ削除
            try
            {
                // IDを取得します
                NHBR_CLIDataSet.FAX注文書Row r = dtsC.FAX注文書.Single(a => a.ID == cID[i]);

                // 画像ファイル名を取得します
                sImgNm = r.画像名;

                // データテーブルから勤務票ヘッダデータを削除します
                errMsg = "FAX注文書データ";
                r.Delete();

                // データベース更新
                fAdp.Update(dtsC.FAX注文書);

                // 画像ファイルを削除します
                errMsg = "FAX発注書画像";
                if (sImgNm != string.Empty)
                {
                    if (System.IO.File.Exists(Properties.Settings.Default.mydataPath + sImgNm))
                    {
                        System.IO.File.Delete(Properties.Settings.Default.mydataPath + sImgNm);
                    }
                }

                // 配列キー再構築
                keyArrayCreate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(errMsg + "の削除に失敗しました" + Environment.NewLine + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
            }

        }

        private void btnRtn_Click(object sender, EventArgs e)
        {
        }

        private void frmCorrect_FormClosing(object sender, FormClosingEventArgs e)
        {
            //「受入データ作成終了」「勤務票データなし」以外での終了のとき
            if (this.Tag.ToString() != END_MAKEDATA && this.Tag.ToString() != END_NODATA)
            {
                //if (MessageBox.Show("終了します。よろしいですか", "終了確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                //{
                //    e.Cancel = true;
                //    return;
                //}

                // カレントデータ更新
                if (dID == string.Empty)
                {
                    CurDataUpDate(cID[cI]);
                }

                //// 勤務表データのない帰宅後勤務データを削除する
                //kitakuClean();
            }

            // データベース更新
            fAdp.Update(dtsC.FAX注文書);

            // 楽商データベース接続解除
            Conn.Close();
            Conn.Dispose();

            // 解放する
            this.Dispose();
        }

        private void btnDataMake_Click(object sender, EventArgs e)
        {
        }

        /// -----------------------------------------------------------------------
        /// <summary>
        ///     楽商受入CSVデータ出力 </summary>
        /// -----------------------------------------------------------------------
        private void textDataMake()
        {
            if (MessageBox.Show("楽商データを作成します。よろしいですか", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            // OCRDataクラス生成
            OCRData ocr = new OCRData(Conn);

            // エラーチェックを実行
            if (getErrData(cI, ocr))
            {
                // 社内伝票番号日付を入力
                frmDenNumDate frmDen = new frmDenNumDate();
                frmDen.ShowDialog();

                // OCROutputクラス インスタンス生成
                OCROutput kd = new OCROutput(this, dtsC, dts, Conn, _myCode);

                // 楽商発注データ作成
                kd.SaveData();          
            }
            else
            {
                // カレントインデックスをエラーありインデックスで更新
                cI = ocr._errHeaderIndex;

                // データ表示
                showOcrData(cI);

                // エラー表示
                ErrShow(ocr);

                return;
            }

            // 画像ファイル退避
            tifFileMove();
            
            // FAX注文書データ削除
            deleteDataAll();

            // MDBファイル最適化
            mdbCompact();

            //終了
            MessageBox.Show("終了しました。楽商で発注データ受け入れを行ってください。", "楽商受入データ作成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Tag = END_MAKEDATA;
            this.Close();
        }

        /// -----------------------------------------------------------------------------------
        /// <summary>
        ///     エラーチェックを実行する</summary>
        /// <param name="cIdx">
        ///     現在表示中の勤務票ヘッダデータインデックス</param>
        /// <param name="ocr">
        ///     OCRDATAクラスインスタンス</param>
        /// <returns>
        ///     エラーなし：true, エラーあり：false</returns>
        /// -----------------------------------------------------------------------------------
        private bool getErrData(int cIdx, OCRData ocr)
        {
            // カレントレコード更新
            CurDataUpDate(cID[cIdx]);

            // エラー番号初期化
            ocr._errNumber = ocr.eNothing;

            // エラーメッセージクリーン
            ocr._errMsg = string.Empty;

            // エラーチェック実行①:カレントレコードから最終レコードまで
            if (!ocr.errCheckMain(cIdx, (dtsC.FAX注文書.Rows.Count - 1), this, dtsC, dts, cID))
            {
                return false;
            }

            // エラーチェック実行②:最初のレコードからカレントレコードの前のレコードまで
            if (cIdx > 0)
            {
                if (!ocr.errCheckMain(0, (cIdx - 1), this, dtsC, dts, cID))
                {
                    return false;
                }
            }

            // エラーなし
            lblErrMsg.Text = string.Empty;

            return true;
        }

        ///----------------------------------------------------------------------------------
        /// <summary>
        ///     画像ファイル退避処理 </summary>
        ///----------------------------------------------------------------------------------
        private void tifFileMove()
        {
            string sTel = string.Empty;
            string sJyu = string.Empty;

            var s = dts.環境設定.Single(a => a.ID == global.flgOn);

            // 移動先フォルダがあるか？なければ作成する（TIFフォルダ）
            if (!System.IO.Directory.Exists(s.画像保存先パス))
            {
                System.IO.Directory.CreateDirectory(s.画像保存先パス);
            }

            string fromImg = string.Empty;
            string toImg = string.Empty;

            // 発注書データを取得する
            foreach (var t in dtsC.FAX注文書.OrderBy(a => a.ID))
            {
                // 発注書画像ファイルパスを取得する
                fromImg = Properties.Settings.Default.mydataPath + t.画像名;

                // 移動先フォルダ
                string sName = Utility.getNouhinName(t.届先番号.ToString().PadLeft(6, '0'), out sTel, out sJyu);   // 届先名

                // 発注書移動先ファイルパス
                string userFolder = s.画像保存先パス + t.届先番号.ToString().PadLeft(6, '0') + "_" + sName;
                
                // お客様フォルダがあるか？なければ作成する
                if (!System.IO.Directory.Exists(userFolder))
                {
                    System.IO.Directory.CreateDirectory(userFolder);
                }

                // 同名ファイルが既に登録済みのときは削除する
                toImg = userFolder + @"\" + t.画像名;
                if (System.IO.File.Exists(toImg)) 
                {
                    System.IO.File.Delete(toImg);
                }

                // ファイルを移動する
                if (System.IO.File.Exists(fromImg)) 
                {
                    System.IO.File.Move(fromImg, toImg);
                }
            }
        }

        /// ---------------------------------------------------------------------
        /// <summary>
        ///     MDBファイルを最適化する </summary>
        /// ---------------------------------------------------------------------
        private void mdbCompact()
        {
            try
            {
                JRO.JetEngine jro = new JRO.JetEngine();
                string OldDb = Properties.Settings.Default.mdbOlePath;
                string NewDb = Properties.Settings.Default.mdbPathTemp;

                jro.CompactDatabase(OldDb, NewDb);

                //今までのバックアップファイルを削除する
                System.IO.File.Delete(Properties.Settings.Default.mdbPath + global.MDBBACK);

                //今までのファイルをバックアップとする
                System.IO.File.Move(Properties.Settings.Default.mdbPath + global.MDBFILE, Properties.Settings.Default.mdbPath + global.MDBBACK);

                //一時ファイルをMDBファイルとする
                System.IO.File.Move(Properties.Settings.Default.mdbPath + global.MDBTEMP, Properties.Settings.Default.mdbPath + global.MDBFILE);
            }
            catch (Exception e)
            {
                MessageBox.Show("MDB最適化中" + Environment.NewLine + e.Message, "エラー", MessageBoxButtons.OK);
            }
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {

            //if (dGV.RowCount == global.NIPPOU_TATE)
            //{
            //    global.miMdlZoomRate_TATE = (float)leadImg.ScaleFactor;
            //}
            //else if (dGV.RowCount == global.NIPPOU_YOKO)
            //{
            //    global.miMdlZoomRate_YOKO = (float)leadImg.ScaleFactor;
            //}
        }

        /// ---------------------------------------------------------------------------------
        /// <summary>
        ///     設定月数分経過した過去画像と過去勤務データ、過去応援移動票データを削除する </summary> 
        /// ---------------------------------------------------------------------------------
        private void deleteArchived()
        {
            //// 削除月設定が0のとき、「過去画像削除しない」とみなし終了する
            //if (Properties.Settings.Default.dataDelSpan == global.flgOff) return;

            //try
            //{
            //    // 削除年月の取得
            //    DateTime dt = DateTime.Parse(DateTime.Today.Year.ToString() + "/" + DateTime.Today.Month.ToString() + "/01");
            //    DateTime delDate = dt.AddMonths(Properties.Settings.Default.dataDelSpan * (-1));
            //    int _dYY = delDate.Year;            //基準年
            //    int _dMM = delDate.Month;           //基準月
            //    int _dYYMM = _dYY * 100 + _dMM;     //基準年月
            //    int _waYYMM = (delDate.Year - Properties.Settings.Default.rekiHosei) * 100 + _dMM;   //基準年月(和暦）

            //    // 設定月数分経過した過去画像・過去勤務票データを削除する
            //    deleteLastDataArchived(_dYYMM);

            //    // 設定月数分経過した過去画像・過去応援移動票データを削除する
            //    deleteLastOuenDataArchived(_dYYMM);
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("過去画像・過去勤務票データ削除中" + Environment.NewLine + e.Message, "エラー", MessageBoxButtons.OK);
            //    return;
            //}
            //finally
            //{
            //    //if (ocr.sCom.Connection.State == ConnectionState.Open) ocr.sCom.Connection.Close();
            //}
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        ///     過去勤務票データ削除～登録 </summary>
        /// ---------------------------------------------------------------------------
        private void saveLastData()
        {
            //try
            //{
            //    // データベース更新
            //    adpMn.UpdateAll(dts);
            //    pAdpMn.UpdateAll(dts);

            //    //  過去勤務票ヘッダデータとその明細データを削除します
            //    //deleteLastData();
            //    delPastData();

            //    // データセットへデータを再読み込みします
            //    getDataSet();

            //    // 過去勤務票ヘッダデータと過去勤務票明細データを作成します
            //    addLastdata();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "過去勤務票データ作成エラー", MessageBoxButtons.OK);
            //}
            //finally
            //{
            //}
        }


        ///------------------------------------------------------
        /// <summary>
        ///     過去勤務票データ削除 </summary>
        ///------------------------------------------------------
        private void delPastData()
        {
            //// 過去勤務票ヘッダデータ削除
            //foreach (var t in dts.勤務票ヘッダ)
            //{
            //    string sBusho = t.部署コード;
            //    int sYY = t.年;
            //    int sMM = t.月;
            //    int sDD = t.日;

            //    // 過去勤務票ヘッダ削除
            //    delPastHeader(sBusho, sYY, sMM, sDD);
            //}

            //// 過去勤務票明細データ削除
            //delPastItem();
        }

        ///----------------------------------------------------------------
        /// <summary>
        ///     過去勤務票ヘッダデータ削除 </summary>
        /// <param name="bCode">
        ///     部署コード</param>
        /// <param name="syy">
        ///     対象年</param>
        /// <param name="smm">
        ///     対象月</param>
        /// <param name="sdd">
        ///     対象日</param>
        ///----------------------------------------------------------------
        private void delPastHeader(string bCode, int syy, int smm, int sdd)
        {
            //OleDbCommand sCom = new OleDbCommand();
            //mdbControl mdb = new mdbControl();
            //mdb.dbConnect(sCom);

            //try
            //{
            //    StringBuilder sb = new StringBuilder();

            //    sb.Clear();
            //    sb.Append("delete from 過去勤務票ヘッダ ");
            //    sb.Append("where 部署コード = ? and 年 = ? and 月 = ? and 日 = ?");

            //    sCom.CommandText = sb.ToString();
            //    sCom.Parameters.Clear();
            //    sCom.Parameters.AddWithValue("@b", bCode);
            //    sCom.Parameters.AddWithValue("@y", syy);
            //    sCom.Parameters.AddWithValue("@m", smm);
            //    sCom.Parameters.AddWithValue("@d", sdd);

            //    sCom.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    throw;
            //}
            //finally
            //{
            //    if (sCom.Connection.State == ConnectionState.Open)
            //    {
            //        sCom.Connection.Close();
            //    }
            //}
        }

        ///--------------------------------------------------------
        /// <summary>
        ///     過去勤務票明細データ削除 </summary>
        ///--------------------------------------------------------
        private void delPastItem()
        {
            //OleDbCommand sCom = new OleDbCommand();
            //mdbControl mdb = new mdbControl();
            //mdb.dbConnect(sCom);

            //try
            //{
            //    StringBuilder sb = new StringBuilder();

            //    sb.Clear();
            //    sb.Append("delete a.ヘッダID from  過去勤務票明細 as a ");
            //    sb.Append("where not EXISTS (select * from 過去勤務票ヘッダ ");
            //    sb.Append("WHERE 過去勤務票ヘッダ.ID = a.ヘッダID)");
                
            //    sCom.CommandText = sb.ToString();
            //    sCom.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    throw;
            //}
            //finally
            //{
            //    if (sCom.Connection.State == ConnectionState.Open)
            //    {
            //        sCom.Connection.Close();
            //    }
            //}
        }

        /// -------------------------------------------------------------------------
        /// <summary>
        ///     過去勤務票ヘッダデータとその明細データを削除します</summary>    
        ///     
        /// -------------------------------------------------------------------------
        private void deleteLastData()
        {
            //OleDbCommand sCom = new OleDbCommand();
            //OleDbCommand sCom2 = new OleDbCommand();
            //OleDbCommand sCom3 = new OleDbCommand();

            //mdbControl mdb = new mdbControl();
            //mdb.dbConnect(sCom);
            //mdb.dbConnect(sCom2);
            //mdb.dbConnect(sCom3);

            //OleDbDataReader dR = null;
            //OleDbDataReader dR2 = null;

            //StringBuilder sb = new StringBuilder();
            //StringBuilder sbd = new StringBuilder();

            //try
            //{
            //    // 対象データ : 取消は対象外とする
            //    sb.Clear();
            //    sb.Append("Select 勤務票明細.ヘッダID, 勤務票明細.ID,");
            //    sb.Append("勤務票ヘッダ.年, 勤務票ヘッダ.月, 勤務票ヘッダ.日,");
            //    sb.Append("勤務票明細.社員番号 from 勤務票ヘッダ inner join 勤務票明細 ");
            //    sb.Append("on 勤務票ヘッダ.ID = 勤務票明細.ヘッダID ");
            //    sb.Append("where 勤務票明細.取消 = '").Append(global.FLGOFF).Append("'");
            //    sb.Append("order by 勤務票明細.ヘッダID, 勤務票明細.ID");

            //    sCom.CommandText = sb.ToString();
            //    dR = sCom.ExecuteReader();

            //    while (dR.Read())
            //    {
            //        // ヘッダID
            //        string hdID = string.Empty;

            //        // 日付と社員番号で過去データを抽出（該当するのは1件）
            //        sb.Clear();
            //        sb.Append("Select 過去勤務票明細.ヘッダID,過去勤務票明細.ID,");
            //        sb.Append("過去勤務票ヘッダ.年, 過去勤務票ヘッダ.月, 過去勤務票ヘッダ.日,");
            //        sb.Append("過去勤務票明細.社員番号 from 過去勤務票ヘッダ inner join 過去勤務票明細 ");
            //        sb.Append("on 過去勤務票ヘッダ.ID = 過去勤務票明細.ヘッダID ");
            //        sb.Append("where ");
            //        sb.Append("過去勤務票ヘッダ.年 = ? and ");
            //        sb.Append("過去勤務票ヘッダ.月 = ? and ");
            //        sb.Append("過去勤務票ヘッダ.日 = ? and ");
            //        sb.Append("過去勤務票ヘッダ.データ領域名 = ? and ");
            //        sb.Append("過去勤務票明細.社員番号 = ?");

            //        sCom2.CommandText = sb.ToString();
            //        sCom2.Parameters.Clear();
            //        sCom2.Parameters.AddWithValue("@yy", dR["年"].ToString());
            //        sCom2.Parameters.AddWithValue("@mm", dR["月"].ToString());
            //        sCom2.Parameters.AddWithValue("@dd", dR["日"].ToString());
            //        sCom2.Parameters.AddWithValue("@db", _dbName);
            //        sCom2.Parameters.AddWithValue("@n", dR["社員番号"].ToString());

            //        dR2 = sCom2.ExecuteReader();

            //        while (dR2.Read())
            //        {
            //            //// ヘッダIDを取得
            //            //if (hdID == string.Empty)
            //            //{
            //            //    hdID = dR2["ヘッダID"].ToString();
            //            //}

            //            // 過去勤務票明細レコード削除
            //            sbd.Clear();
            //            sbd.Append("delete from 過去勤務票明細 ");
            //            sbd.Append("where ID = ?");

            //            sCom3.CommandText = sbd.ToString();
            //            sCom3.Parameters.Clear();
            //            sCom3.Parameters.AddWithValue("@id", dR2["ID"].ToString());

            //            sCom3.ExecuteNonQuery();
            //        }

            //        dR2.Close();
            //    }

            //    dR.Close();

            //    // データベース接続解除
            //    if (sCom.Connection.State == ConnectionState.Open)
            //    {
            //        sCom.Connection.Close();
            //    }

            //    if (sCom2.Connection.State == ConnectionState.Open)
            //    {
            //        sCom2.Connection.Close();
            //    }

            //    if (sCom3.Connection.State == ConnectionState.Open)
            //    {
            //        sCom3.Connection.Close();
            //    }

            //    // データベース再接続
            //    mdb.dbConnect(sCom);
            //    mdb.dbConnect(sCom2);

            //    // 明細データのない過去勤務票ヘッダデータを抽出
            //    sb.Clear();
            //    sb.Append("Select 過去勤務票ヘッダ.ID,過去勤務票明細.ヘッダID ");
            //    sb.Append("from 過去勤務票ヘッダ left join 過去勤務票明細 ");
            //    sb.Append("on 過去勤務票ヘッダ.ID = 過去勤務票明細.ヘッダID ");
            //    sb.Append("where ");
            //    sb.Append("過去勤務票明細.ヘッダID is null");
            //    sCom.CommandText = sb.ToString();
            //    dR = sCom.ExecuteReader();

            //    while (dR.Read())
            //    {
            //        // 過去勤務票ヘッダレコード削除
            //        sbd.Clear();

            //        sbd.Append("delete from 過去勤務票ヘッダ ");
            //        sbd.Append("where ID = ?");

            //        sCom2.CommandText = sbd.ToString();
            //        sCom2.Parameters.Clear();
            //        sCom2.Parameters.AddWithValue("@id", dR["ID"].ToString());

            //        sCom2.ExecuteNonQuery();
            //    }

            //    dR.Close();
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.Message);
            //}
            //finally
            //{
            //    if (sCom.Connection.State == ConnectionState.Open)
            //    {
            //        sCom.Connection.Close();
            //    }

            //    if (sCom2.Connection.State == ConnectionState.Open)
            //    {
            //        sCom2.Connection.Close();
            //    }

            //    if (sCom3.Connection.State == ConnectionState.Open)
            //    {
            //        sCom3.Connection.Close();
            //    }
            //}
        }


        /// -------------------------------------------------------------------------
        /// <summary>
        ///     過去勤務票ヘッダデータと過去勤務票明細データを作成します</summary>
        ///     
        /// -------------------------------------------------------------------------
        private void addLastdata()
        {
            //for (int i = 0; i < dts.勤務票ヘッダ.Rows.Count; i++)
            //{
            //    // -------------------------------------------------------------------------
            //    //      過去勤務票ヘッダレコードを作成します
            //    // -------------------------------------------------------------------------
            //    DataSet1.勤務票ヘッダRow hr = (DataSet1.勤務票ヘッダRow)dts.勤務票ヘッダ.Rows[i];
            //    DataSet1.過去勤務票ヘッダRow nr = dts.過去勤務票ヘッダ.New過去勤務票ヘッダRow();

            //    #region テーブルカラム名比較～データコピー

            //    // 勤務票ヘッダのカラムを順番に読む
            //    for (int j = 0; j < dts.勤務票ヘッダ.Columns.Count; j++)
            //    {
            //        // 過去勤務票ヘッダのカラムを順番に読む
            //        for (int k = 0; k < dts.過去勤務票ヘッダ.Columns.Count; k++)
            //        {
            //            // フィールド名が同じであること
            //            if (dts.勤務票ヘッダ.Columns[j].ColumnName == dts.過去勤務票ヘッダ.Columns[k].ColumnName)
            //            {
            //                if (dts.過去勤務票ヘッダ.Columns[k].ColumnName == "更新年月日")
            //                {
            //                    nr[k] = DateTime.Now;   // 更新年月日はこの時点のタイムスタンプを登録
            //                }
            //                else
            //                {
            //                    nr[k] = hr[j];          // データをコピー
            //                }
            //                break;
            //            }
            //        }
            //    }
            //    #endregion

            //    // 過去勤務票ヘッダデータテーブルに追加
            //    dts.過去勤務票ヘッダ.Add過去勤務票ヘッダRow(nr);

            //    // -------------------------------------------------------------------------
            //    //      過去勤務票明細レコードを作成します
            //    // -------------------------------------------------------------------------
            //    var mm = dts.勤務票明細
            //        .Where(a => a.RowState != DataRowState.Deleted && a.RowState != DataRowState.Detached &&
            //               a.ヘッダID == hr.ID)
            //        .OrderBy(a => a.ID);

            //    foreach (var item in mm)
            //    {
            //        DataSet1.勤務票明細Row m = (DataSet1.勤務票明細Row)dts.勤務票明細.Rows.Find(item.ID);
            //        DataSet1.過去勤務票明細Row nm = dts.過去勤務票明細.New過去勤務票明細Row();

            //        // 取消は対象外：2015/10/01
            //        if (m.取消 == global.FLGON) continue;

            //        // 社員番号が空白のレコードは対象外とします
            //        if (m.社員番号 == string.Empty) continue;

            //        #region  テーブルカラム名比較～データコピー

            //        // 勤務票明細のカラムを順番に読む
            //        for (int j = 0; j < dts.勤務票明細.Columns.Count; j++)
            //        {
            //            // IDはオートナンバーのため値はコピーしない
            //            if (dts.勤務票明細.Columns[j].ColumnName != "ID")
            //            {
            //                // 過去勤務票ヘッダのカラムを順番に読む
            //                for (int k = 0; k < dts.過去勤務票明細.Columns.Count; k++)
            //                {
            //                    // フィールド名が同じであること
            //                    if (dts.勤務票明細.Columns[j].ColumnName == dts.過去勤務票明細.Columns[k].ColumnName)
            //                    {
            //                        if (dts.過去勤務票明細.Columns[k].ColumnName == "更新年月日")
            //                        {
            //                            nm[k] = DateTime.Now;   // 更新年月日はこの時点のタイムスタンプを登録
            //                        }
            //                        else
            //                        {
            //                            nm[k] = m[j];          // データをコピー
            //                        }
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //        #endregion

            //        // 過去勤務票明細データテーブルに追加
            //        dts.過去勤務票明細.Add過去勤務票明細Row(nm);
            //    }
            //}

            //// データベース更新
            //pAdpMn.UpdateAll(dts);
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
        //    //if (e.RowIndex < 0) return;

        //    string colName = dGV.Columns[e.ColumnIndex].Name;

        //    if (colName == cSH || colName == cSE || colName == cEH || colName == cEE ||
        //        colName == cZH || colName == cZE || colName == cSIH || colName == cSIE)
        //    {
        //        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
        //    }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //string colName = dGV.Columns[dGV.CurrentCell.ColumnIndex].Name;
            ////if (colName == cKyuka || colName == cCheck)
            ////{
            ////    if (dGV.IsCurrentCellDirty)
            ////    {
            ////        dGV.CommitEdit(DataGridViewDataErrorContexts.Commit);
            ////        dGV.RefreshEdit();
            ////    }
            ////}
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellEnter_1(object sender, DataGridViewCellEventArgs e)
        {
            //// 時が入力済みで分が未入力のとき分に"00"を表示します
            //if (dGV[ColH, dGV.CurrentRow.Index].Value != null)
            //{
            //    if (dGV[ColH, dGV.CurrentRow.Index].Value.ToString().Trim() != string.Empty)
            //    {
            //        if (dGV[ColM, dGV.CurrentRow.Index].Value == null)
            //        {
            //            dGV[ColM, dGV.CurrentRow.Index].Value = "00";
            //        }
            //        else if (dGV[ColM, dGV.CurrentRow.Index].Value.ToString().Trim() == string.Empty)
            //        {
            //            dGV[ColM, dGV.CurrentRow.Index].Value = "00";
            //        }
            //    }
            //}
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        ///     伝票画像表示 </summary>
        /// <param name="iX">
        ///     現在の伝票</param>
        /// <param name="tempImgName">
        ///     画像名</param>
        /// ------------------------------------------------------------------------------
        public void ShowImage(string tempImgName)
        {
            //修正画面へ組み入れた画像フォームの表示    
            //画像の出力が無い場合は、画像表示をしない。
            if (tempImgName == string.Empty)
            {
                leadImg.Visible = false;
                lblNoImage.Visible = false;
                //global.pblImagePath = string.Empty;
                return;
            }

            //画像ファイルがあるとき表示
            if (File.Exists(tempImgName))
            {
                lblNoImage.Visible = false;
                leadImg.Visible = true;

                // 画像操作ボタン
                btnPlus.Enabled = true;
                btnMinus.Enabled = true;

                // 画像回転ボタン
                btnLeft.Enabled = true;
                btnRight.Enabled = true;

                //画像ロード
                Leadtools.Codecs.RasterCodecs.Startup();
                Leadtools.Codecs.RasterCodecs cs = new Leadtools.Codecs.RasterCodecs();

                // 描画時に使用される速度、品質、およびスタイルを制御します。 
                Leadtools.RasterPaintProperties prop = new Leadtools.RasterPaintProperties();
                prop = Leadtools.RasterPaintProperties.Default;
                prop.PaintDisplayMode = Leadtools.RasterPaintDisplayModeFlags.Resample;
                leadImg.PaintProperties = prop;

                leadImg.Image = cs.Load(tempImgName, 0, Leadtools.Codecs.CodecsLoadByteOrder.BgrOrGray, 1, 1);

                //画像表示倍率設定
                if (gl.miMdlZoomRate == 0f)
                {
                    leadImg.ScaleFactor *= gl.ZOOM_RATE;
                }
                else
                {
                    leadImg.ScaleFactor *= gl.miMdlZoomRate;
                }

                //画像のマウスによる移動を可能とする
                leadImg.InteractiveMode = Leadtools.WinForms.RasterViewerInteractiveMode.Pan;

                // グレースケールに変換
                Leadtools.ImageProcessing.GrayscaleCommand grayScaleCommand = new Leadtools.ImageProcessing.GrayscaleCommand();
                grayScaleCommand.BitsPerPixel = 8;
                grayScaleCommand.Run(leadImg.Image);
                leadImg.Refresh();

                cs.Dispose();
                Leadtools.Codecs.RasterCodecs.Shutdown();
                //global.pblImagePath = tempImgName;
            }
            else
            {
                //画像ファイルがないとき
                lblNoImage.Visible = true;

                // 画像操作ボタン
                btnPlus.Enabled = false;
                btnMinus.Enabled = false;

                leadImg.Visible = false;
                //global.pblImagePath = string.Empty;

                // 画像回転ボタン
                btnLeft.Enabled = false;
                btnRight.Enabled = false;
            }
        }

        private void leadImg_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void leadImg_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        /// -------------------------------------------------------------------------
        /// <summary>
        ///     基準年月以前の過去勤務票ヘッダデータとその明細データを削除します</summary>
        /// <param name="sYYMM">
        ///     基準年月</param>     
        /// -------------------------------------------------------------------------
        private void deleteLastDataArchived(int sYYMM)
        {
            //// データ読み込み
            //getDataSet();

            //// 基準年月以前の過去勤務票ヘッダデータを取得します
            //var h = dts.過去勤務票ヘッダ
            //        .Where(a => a.RowState != DataRowState.Deleted && a.RowState != DataRowState.Detached &&
            //                    a.年 * 100 + a.月 < sYYMM);

            //// foreach用の配列を作成
            //var hLst = h.ToList();

            //foreach (var lh in hLst)
            //{
            //    // ヘッダIDが一致する過去勤務票明細を取得します
            //    var m = dts.過去勤務票明細
            //        .Where(a => a.RowState != DataRowState.Deleted && a.RowState != DataRowState.Detached &&
            //                    a.ヘッダID == lh.ID);

            //    // foreach用の配列を作成
            //    var list = m.ToList();

            //    // 該当過去勤務票明細を削除します
            //    foreach (var lm in list)
            //    {
            //        DataSet1.過去勤務票明細Row lRow = (DataSet1.過去勤務票明細Row)dts.過去勤務票明細.Rows.Find(lm.ID);
            //        lRow.Delete();
            //    }

            //    // 画像ファイルを削除します
            //    string imgPath = Properties.Settings.Default.tifPath + lh.画像名;
            //    File.Delete(imgPath);

            //    // 過去勤務票ヘッダを削除します
            //    lh.Delete();
            //}

            //// データベース更新
            //pAdpMn.UpdateAll(dts);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     設定月数分経過した過去画像を削除する</summary>
        /// <param name="_dYYMM">
        ///     基準年月 (例：201401)</param>
        /// -----------------------------------------------------------------------------
        private void deleteImageArchived(int _dYYMM)
        {
            //int _DataYYMM;
            //string fileYYMM;

            //// 設定月数分経過した過去画像を削除する            
            //foreach (string files in System.IO.Directory.GetFiles(Properties.Settings.Default.tifPath, "*.tif"))
            //{
            //    // ファイル名が規定外のファイルは読み飛ばします
            //    if (System.IO.Path.GetFileName(files).Length < 21) continue;

            //    //ファイル名より年月を取得する
            //    fileYYMM = System.IO.Path.GetFileName(files).Substring(0, 6);

            //    if (Utility.NumericCheck(fileYYMM))
            //    {
            //        _DataYYMM = int.Parse(fileYYMM);

            //        //基準年月以前なら削除する
            //        if (_DataYYMM <= _dYYMM) File.Delete(files);
            //    }
            //}
        }

        /// -------------------------------------------------------------------
        /// <summary>
        ///     FAX注文書データを全件削除します</summary>
        /// -------------------------------------------------------------------
        private void deleteDataAll()
        {
            // FAX注文書データ読み込み
            getDataSet();

            // FAX注文書削除
            var m = dtsC.FAX注文書.Where(a => a.RowState != DataRowState.Deleted);
            foreach (var t in m)
            {
                t.Delete();
            }

            // データベース更新
            fAdp.Update(dtsC.FAX注文書);

            // 後片付け
            dtsC.FAX注文書.Dispose();
        }

        private void maskedTextBox3_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void txtYear_TextChanged(object sender, EventArgs e)
        {
            //// 曜日
            //DateTime eDate;
            //int tYY = Utility.StrtoInt(txtYear.Text);
            //string sDate = tYY.ToString() + "/" + Utility.EmptytoZero(txtMonth.Text) + "/" +
            //        Utility.EmptytoZero(txtDay.Text);

            //// 存在する日付と認識された場合、曜日を表示する
            //if (DateTime.TryParse(sDate, out eDate))
            //{
            //    txtWeekDay.Text = ("日月火水木金土").Substring(int.Parse(eDate.DayOfWeek.ToString("d")), 1);
            //}
            //else
            //{
            //    txtWeekDay.Text = string.Empty;
            //}
        }

        private void dGV_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            //if (editLogStatus)
            //{
            //    if (e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 3 || e.ColumnIndex == 4 ||
            //        e.ColumnIndex == 6 || e.ColumnIndex == 7 || e.ColumnIndex == 9 || e.ColumnIndex == 10 ||
            //        e.ColumnIndex == 12 || e.ColumnIndex == 13 || e.ColumnIndex == 15)
            //    {
            //        dGV.CommitEdit(DataGridViewDataErrorContexts.Commit);
            //        cellAfterValue = Utility.NulltoStr(dGV[e.ColumnIndex, e.RowIndex].Value);

            //        //// 変更のとき編集ログデータを書き込み
            //        //if (cellBeforeValue != cellAfterValue)
            //        //{
            //        //    logDataUpdate(e.RowIndex, cI, global.flgOn);
            //        //}
            //    }
            //}
        }

        private void txtYear_Enter(object sender, EventArgs e)
        {
            //if (editLogStatus)
            //{
            //    if (sender == txtYear) cellName = LOG_YEAR;
            //    if (sender == txtMonth) cellName = LOG_MONTH;
            //    if (sender == txtDay) cellName = LOG_DAY;
            //    //if (sender == txtSftCode) cellName = LOG_TAIKEICD;

            //    TextBox tb = (TextBox)sender;

            //    // 値を保持
            //    cellBeforeValue = Utility.NulltoStr(tb.Text);
            //}
        }

        private void txtYear_Leave(object sender, EventArgs e)
        {
            if (editLogStatus)
            {
                TextBox tb = (TextBox)sender;
                cellAfterValue = Utility.NulltoStr(tb.Text);

                //// 変更のとき編集ログデータを書き込み
                //if (cellBeforeValue != cellAfterValue)
                //{
                //    logDataUpdate(0, cI, global.flgOff);
                //}
            }
        }

        private void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
        {
            if (!gl.ChangeValueStatus) return;

            if (e.RowIndex < 0) return;

            // 過去データ表示のときは終了
            if (dID != string.Empty) return;

            // パターンコードのとき発注書パターンを更新
            if (e.CellName == "txtPtnNum")
            {
                // 発注パターン表示
                ptnShow(gcMultiRow2,
                    Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[e.RowIndex, "txtTdkNum"].Value)),
                    Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[e.RowIndex, e.CellName].Value)));

                // パターンID「０」でフリー入力のとき
                if (Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[e.RowIndex, e.CellName])) == global.flgOff)
                {
                    // 商品コード、数量入力可能とする
                    for (int i = 0; i < gcMultiRow2.RowCount; i++)
                    {
                        gcMultiRow2[i, "txtHinCode"].ReadOnly = false;
                        gcMultiRow2[i, "txtHinCode"].Selectable = true;

                        gcMultiRow2[i, "txtSuu"].ReadOnly = false;
                        gcMultiRow2[i, "txtSuu"].Selectable = true;

                        gcMultiRow2[i, "txtHinCode2"].ReadOnly = false;
                        gcMultiRow2[i, "txtHinCode2"].Selectable = true;

                        gcMultiRow2[i, "txtSuu2"].ReadOnly = false;
                        gcMultiRow2[i, "txtSuu2"].Selectable = true;
                    }
                }
                else
                {
                    // 商品コード、数量入力可能とする
                    for (int i = 0; i < gcMultiRow2.RowCount; i++)
                    {
                        gcMultiRow2[i, "txtHinCode"].ReadOnly = true;
                        gcMultiRow2[i, "txtHinCode"].Selectable = false;

                        gcMultiRow2[i, "txtSuu"].ReadOnly = false;
                        gcMultiRow2[i, "txtSuu"].Selectable = true;

                        gcMultiRow2[i, "txtHinCode2"].ReadOnly = true;
                        gcMultiRow2[i, "txtHinCode2"].Selectable = false;

                        gcMultiRow2[i, "txtSuu2"].ReadOnly = false;
                        gcMultiRow2[i, "txtSuu2"].Selectable = true;
                    }
                }
            }
            else if (e.CellName == "txtTdkNum")
            {
                // お客様番号のときお客様名を表示します

                // ChangeValueイベントを発生させない
                gl.ChangeValueStatus = false;

                // 氏名と電話番号を初期化
                gcMultiRow1[e.RowIndex, "lblName"].Value = string.Empty;
                gcMultiRow1[e.RowIndex, "lblTel"].Value = string.Empty;
                
                // 楽商データベースよりお客様名を取得して表示します
                if (Utility.NulltoStr(gcMultiRow1[0, "txtTdkNum"].Value) != string.Empty)
                {
                    // 届先名、電話番号、住所表示
                    string gName = string.Empty;
                    string gTel = string.Empty;
                    string gJyu = string.Empty;

                    string bCode = gcMultiRow1[e.RowIndex, "txtTdkNum"].Value.ToString().PadLeft(6, '0');
                    gName = getUserName(bCode, out gTel, out gJyu);

                    gcMultiRow1[e.RowIndex, "lblName"].Value = gName;
                    gcMultiRow1[e.RowIndex, "lblTel"].Value = gTel;

                    // ChangeValueイベントステータスをtrueに戻す
                    gl.ChangeValueStatus = true;
                }

                // 発注パターン表示
                ptnShow(gcMultiRow2,
                    Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[e.RowIndex, e.CellName].Value)),
                    Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[e.RowIndex, "txtPtnNum"].Value)));
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
        private string getUserName(string tID, out string sTel, out string sJyu)
        {
            string val = string.Empty;
            sTel = string.Empty;
            sJyu = string.Empty;

            string strSQL = "SELECT KOK_ID, NOU_NAME, NOU_JYU1, NOU_JYU2, NOU_TEL from RAKUSYO_FAXOCR.V_NOUHINSAKI WHERE KOK_ID = '" + tID + "'";
            OracleCommand Cmd = new OracleCommand(strSQL, Conn);
            OracleDataReader dR = Cmd.ExecuteReader();
            while (dR.Read())
            {
                val = dR["NOU_NAME"].ToString().Trim();
                sTel = dR["NOU_TEL"].ToString().Trim();
                sJyu = dR["NOU_JYU1"].ToString().Trim() + " " + dR["NOU_JYU2"].ToString().Trim();
            }

            dR.Dispose();
            Cmd.Dispose();

            return val;
        }

        private void gcMultiRow1_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            if (e.Control is TextBoxEditingControl)
            {
                //イベントハンドラが複数回追加されてしまうので最初に削除する
                e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
                e.Control.KeyDown -= new KeyEventHandler(Control_KeyDown2);

                // 数字のみ入力可能とする
                if (gcMultiRow1.CurrentCell.Name == "txtPtnNum" || gcMultiRow1.CurrentCell.Name == "txtTdkNum" ||
                    gcMultiRow1.CurrentCell.Name == "txtOrderNum" || gcMultiRow1.CurrentCell.Name == "txtMonth" ||
                    gcMultiRow1.CurrentCell.Name == "txtDay")
                {
                    //イベントハンドラを追加する
                    e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }

                // お客様検索画面
                if (gcMultiRow1.CurrentCell.Name == "txtTdkNum")
                {
                    //イベントハンドラを追加する
                    e.Control.KeyDown += new KeyEventHandler(Control_KeyDown2);
                }
            }
        }

        private void Control_KeyDown2(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
            {
                gcMultiRow1.EndEdit();

                frmTodoke frm = new frmTodoke(false);
                frm.ShowDialog();

                if (frm._nouCode != null)
                {
                    gcMultiRow1.SetValue(0, "txtTdkNum", frm._nouCode[0]);
                    gcMultiRow1.CurrentCellPosition = new CellPosition(0, "txtOrderNum");
                }

                // 後片付け
                frm.Dispose();
            }
        }

        private void Control_KeyDownHinM2(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
            {
                //gcMultiRow2.EndEdit();

                frmSyohin frm = new frmSyohin(false);
                frm.ShowDialog();

                if (frm._nouCode != null)
                {
                    gcMultiRow2.SetValue(gcMultiRow2.CurrentCell.RowIndex, gcMultiRow2.CurrentCellPosition.CellName, frm._nouCode[0]);

                    if (gcMultiRow2.CurrentCellPosition.CellName == "txtHinCode")
                    {
                        gcMultiRow2.CurrentCellPosition = new CellPosition(gcMultiRow2.CurrentCell.RowIndex, "txtSuu");
                        //gcMultiRow2.CurrentCell = null;
                    }
                    else if (gcMultiRow2.CurrentCellPosition.CellName == "txtHinCode2")
                    {
                        gcMultiRow2.CurrentCellPosition = new CellPosition(gcMultiRow2.CurrentCell.RowIndex, "txtSuu2");
                        //gcMultiRow2.CurrentCell = null;
                    }
                }

                // 後片付け
                frm.Dispose();
            }
        }

        private void Control_KeyDownHin(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
            {
                gcMultiRow3.EndEdit();

                frmSyohin frm = new frmSyohin(false);
                frm.ShowDialog();

                if (frm._nouCode != null)
                {
                    gcMultiRow3.SetValue(gcMultiRow3.CurrentCell.RowIndex, gcMultiRow3.CurrentCellPosition.CellName, frm._nouCode[0]);

                    if (gcMultiRow3.CurrentCellPosition.CellName == "txtHinCode")
                    {
                        gcMultiRow3.CurrentCellPosition = new CellPosition(gcMultiRow3.CurrentCell.RowIndex, "txtSuu");
                        //gcMultiRow3.CurrentCell = null;
                    }
                    else if (gcMultiRow3.CurrentCellPosition.CellName == "txtHinCode2")
                    {
                        gcMultiRow3.CurrentCellPosition = new CellPosition(gcMultiRow3.CurrentCell.RowIndex, "txtSuu2");
                        //gcMultiRow3.CurrentCell = null;
                    }
                }

                // 後片付け
                frm.Dispose();
            }
        }

        private void gcMultiRow2_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            if (e.Control is TextBoxEditingControl)
            {
                //イベントハンドラが複数回追加されてしまうので最初に削除する
                e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
                //e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress2);
                e.Control.KeyDown -= new KeyEventHandler(Control_KeyDownHinM2);

                // 数字のみ入力可能とする
                if (gcMultiRow2.CurrentCell.Name == "txtHinCode" || gcMultiRow2.CurrentCell.Name == "txtHinCode2" || 
                    gcMultiRow2.CurrentCell.Name == "txtSuu" || gcMultiRow2.CurrentCell.Name == "txtSuu2")
                {
                    //イベントハンドラを追加する
                    e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }
                
                // 商品検索画面呼出
                if (gcMultiRow2.CurrentCell.Name == "txtHinCode" || gcMultiRow2.CurrentCell.Name == "txtHinCode2")
                {
                    //イベントハンドラを追加する
                    e.Control.KeyDown += new KeyEventHandler(Control_KeyDownHinM2);
                }
            }
        }

        private void gcMultiRow2_CellValueChanged(object sender, CellEventArgs e)
        {
            if (!gl.ChangeValueStatus)
            {
                return;
            }

            if (e.RowIndex < 0)
            {
                return;
            }
            
            // 商品名表示
            if (e.CellName == "txtHinCode" || e.CellName == "txtHinCode2")
            {
                gl.ChangeValueStatus = false;

                gcHinCodeChange(gcMultiRow2, e.CellName, e.RowIndex, true);

                //if (!showStatus)
                //{
                //    // 出荷基準判定
                //    kijunCheckMain();
                //}

                // パターンIDが０のときフリー入力可能とする：2017/08/21
                int ptnCode = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[0, "txtPtnNum"].Value));

                if (ptnCode == global.flgOff)
                {
                    gcMultiRow2[e.RowIndex, e.CellName].ReadOnly = false;
                    gcMultiRow2[e.RowIndex, e.CellName].Selectable = true;
                }
                else
                {
                    gcMultiRow2[e.RowIndex, e.CellName].ReadOnly = true;
                    gcMultiRow2[e.RowIndex, e.CellName].Selectable = false;
                }

                gl.ChangeValueStatus = true;
            }

            // 発注数
            if (e.CellName == "txtSuu" || e.CellName == "txtSuu2")
            {
                gl.ChangeValueStatus = false;

                if (!showStatus)
                {
                    // 出荷基準判定
                    kijunCheckMain();
                }

                gl.ChangeValueStatus = true;
            }


        }

        ///-------------------------------------------------------------------------
        /// <summary>
        ///     奉行シリーズ部署名取得 </summary>
        /// <param name="dName">
        ///     取得する部署名</param>
        /// <param name="dCode">
        ///     部署コード</param>
        /// <param name="r">
        ///     MultiRowRowIndex</param>
        /// <returns>
        ///     true:該当あり, false:該当なし</returns>
        ///-------------------------------------------------------------------------
        private bool getDepartMentName(out string dName, string dCode, int r)
        {
            bool rtn = false;
            //int c = 0;

            //// 部署名を初期化
            dName = string.Empty;

            //// 奉行データベースより部署名を取得して表示します
            //if (Utility.NulltoStr(gcMultiRow2[r, "txtBushoCode"].Value) != string.Empty)
            //{
            //    string b = string.Empty;

            //    // 検索用部署コード
            //    if (Utility.StrtoInt(gcMultiRow2[r, "txtBushoCode"].Value.ToString()) != global.flgOff)
            //    {
            //        b = gcMultiRow2[r, "txtBushoCode"].Value.ToString().Trim().PadLeft(15, '0');
            //    }
            //    else
            //    {
            //        b = gcMultiRow2[r, "txtBushoCode"].Value.ToString().Trim().PadRight(15, ' ');
            //    }

            //    // 接続文字列取得
            //    string sc = sqlControl.obcConnectSting.get(_dbName);
            //    sqlControl.DataControl sdCon = new Common.sqlControl.DataControl(sc);

            //    string dt = DateTime.Today.ToShortDateString();
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append("SELECT DepartmentID, DepartmentCode, DepartmentName ");
            //    sb.Append("FROM tbDepartment ");
            //    sb.Append("where EstablishDate <= '").Append(dt).Append("'");
            //    sb.Append(" and AbolitionDate >= '").Append(dt).Append("'");
            //    sb.Append(" and ValidDate <= '").Append(dt).Append("'");
            //    sb.Append(" and InValidDate >= '").Append(dt).Append("'");
            //    sb.Append(" and DepartmentCode = '").Append(b).Append("'");

            //    SqlDataReader dR = sdCon.free_dsReader(sb.ToString());

            //    while (dR.Read())
            //    {
            //        dName = dR["DepartmentName"].ToString().Trim();
            //        c++;
            //    }

            //    dR.Close();
            //    sdCon.Close();

            //    if (c > 0)
            //    {
            //        rtn = true;
            //    }
            //}
            
            return rtn;
        }

        ///-------------------------------------------------------------------
        /// <summary>
        ///     ライン・部門・製品群コード配列取得   </summary>
        /// <returns>
        ///     ID,コード配列</returns>
        ///-------------------------------------------------------------------
        private string[] getCategoryArray()
        {
            //// 接続文字列取得
            //string sc = sqlControl.obcConnectSting.get(_dbName);
            //sqlControl.DataControl sdCon = new sqlControl.DataControl(sc);

            //StringBuilder sb = new StringBuilder();
            //sb.Append("select CategoryID, CategoryCode from tbHistoryDivisionCategory");
            //SqlDataReader dr = sdCon.free_dsReader(sb.ToString());

            //int iX = 0;
            string[] hArray = new string[1];

            //while (dr.Read())
            //{
            //    if (iX > 0)
            //    {
            //        Array.Resize(ref hArray, iX + 1);
            //    }

            //    hArray[iX] = dr["CategoryID"].ToString() + "," + dr["CategoryCode"].ToString();
            //    iX++;
            //}

            //dr.Close();
            //sdCon.Close();

            return hArray;
        }

        private void gcMultiRow2_CellEnter(object sender, CellEventArgs e)
        {
            if (gcMultiRow2.EditMode == EditMode.EditProgrammatically)
            {
                gcMultiRow2.BeginEdit(true);
            }
        }

        private void gcMultiRow1_CellEnter(object sender, CellEventArgs e)
        {
            if (gcMultiRow1.EditMode == EditMode.EditProgrammatically)
            {
                gcMultiRow1.BeginEdit(true);
            }
        }

        private void gcMultiRow1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            string colName = gcMultiRow1.CurrentCell.Name;

            if (colName == "chkReFax")
            {
                if (gcMultiRow1.IsCurrentCellDirty)
                {
                    gcMultiRow1.CommitEdit(DataErrorContexts.Commit);
                    gcMultiRow1.Refresh();
                }
            }
        }

        private void gcMultiRow1_CellLeave(object sender, CellEventArgs e)
        {
           
        }

        private void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        {
            // 2018/08/02
            if (e.CellName == "chkReFax")
            {
                if (Convert.ToInt32(gcMultiRow1[0, "chkReFax"].Value) == global.flgOn)
                {
                    gcMultiRow1[0, "labelCell2"].Style.BackColor = Color.Red;
                }
                else
                {
                    gcMultiRow1[0, "labelCell2"].Style.BackColor = Color.FromArgb(225, 243, 190);
                }
            }

            // 2018/08/02
            if (e.CellName == "buttonCell1")
            {
                if (Convert.ToInt32(gcMultiRow1[0, "chkReFax"].Value) == global.flgOff)
                {
                    return;
                }
                else
                {
                    if (MessageBox.Show("表示中の発注書を再FAXフォルダへ移動しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        // 画像を再FAXフォルダへ移動
                        moveReFax(cI);
                        MessageBox.Show("発注書データを再FAXフォルダへ移動しました", "発注書移動", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // 件数カウント
                        if (dtsC.FAX注文書.Count() > 0)
                        {
                            // カレントレコードインデックスを再設定
                            if (dtsC.FAX注文書.Count() - 1 < cI) cI = dtsC.FAX注文書.Count() - 1;

                            // データ画面表示
                            showOcrData(cI);
                        }
                        else
                        {
                            // ゼロならばプログラム終了
                            MessageBox.Show("全ての発注書データが削除されました。処理を終了します。", "発注書削除", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            //終了処理
                            this.Tag = END_NODATA;
                            this.Close();
                        }
                    }                        
                }               
            }

            //if (e.CellName == "btnCell")
            //{
            //    //カレントデータの更新
            //    CurDataUpDate(cID[cI]);
                
            //    int sMID = Utility.StrtoInt(gcMultiRow1[e.RowIndex, "txtID"].Value.ToString());

            //    if (dts.勤務票明細.Any(a => a.ID == sMID))
            //    {
            //        var s = dts.勤務票明細.Single(a => a.ID == sMID);
            //        string kID = s.帰宅後勤務ID;
            //        frmKitakugo frm = new frmKitakugo(_dbName, sMID, kID, hArray, bs, true);
            //        frm.ShowDialog();

            //        // 帰宅後勤務データ再読み込み
            //        tAdp.Fill(dts.帰宅後勤務);

            //        //// 勤務票明細再読み込み
            //        //adpMn.勤務票明細TableAdapter.Fill(dts.勤務票明細);

            //        // データ再表示
            //        showOcrData(cI);
            //    }
            //}
        }

        ///------------------------------------------------------------------------
        /// <summary>
        ///     画像ファイルを再FAXフォルダへ移動して勤務データを削除 </summary>
        /// <param name="i">
        ///     IDインデックス</param>
        ///------------------------------------------------------------------------
        private void moveReFax(int i)
        {
            string sImgNm = string.Empty;
            string _fromImgFile = string.Empty;
            string _toImgFile = string.Empty;
            string errMsg = string.Empty;

            // 勤務票データ再FAXフォルダへ移動
            try
            {
                // IDを取得します
                NHBR_CLIDataSet.FAX注文書Row r = dtsC.FAX注文書.Single(a => a.ID == cID[i]);
                
                // 画像ファイルを再FAXフォルダへ移動
                _fromImgFile = Properties.Settings.Default.mydataPath + r.画像名.ToString();
                _toImgFile = Properties.Settings.Default.reFaxPath + r.画像名.ToString();
                
                System.IO.File.Move(_fromImgFile, _toImgFile);

                // データテーブルから勤務票データを削除します
                errMsg = "FAX注文書データ";
                r.Delete();

                // データベース更新
                fAdp.Update(dtsC.FAX注文書);
                
                // 配列キー再構築
                keyArrayCreate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(errMsg + "の削除に失敗しました" + Environment.NewLine + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            //frmOCRIndex frm = new frmOCRIndex(_dbName, dts, hAdp, iAdp);
            //frm.ShowDialog();
            //string hID = frm.hdID;
            //frm.Dispose();

            //if (hID != string.Empty)
            //{
            //    //カレントデータの更新
            //    CurDataUpDate(cID[cI]);

            //    // レコード検索
            //    for (int i = 0; i < cID.Length; i++)
            //    {
            //        if (cID[i] == hID)
            //        {
            //            cI = i;
            //            showOcrData(cI);
            //            break;
            //        }
            //    }
            //}
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void lnkLblClr_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void lnkLblDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void gcMultiRow3_CellEnter(object sender, CellEventArgs e)
        {
            if (gcMultiRow3.EditMode == EditMode.EditProgrammatically)
            {
                gcMultiRow3.BeginEdit(true);
            }
        }

        private void gcMultiRow3_CellValueChanged(object sender, CellEventArgs e)
        {
            if (!gl.ChangeValueStatus)
            {
                return;
            }

            if (e.RowIndex < 0)
            {
                return;
            }

            gl.ChangeValueStatus = false;

            // 商品名表示
            if (e.CellName == "txtHinCode" || e.CellName == "txtHinCode2")
            {
                gcHinCodeChange(gcMultiRow3, e.CellName, e.RowIndex, false);

                if (!showStatus)
                {
                    // 出荷基準判定
                    kijunCheckMain();
                }
            }

            gl.ChangeValueStatus = true;

            // 追加記入があるとき行を赤表示します
            if (e.CellName == "txtSuu")
            {
                if (Utility.NulltoStr(gcMultiRow3[e.RowIndex, "txtSuu"].Value) != string.Empty)
                {
                    gcMultiRow3[e.RowIndex, "txtHinCode"].Style.BackColor = Color.MistyRose;
                    gcMultiRow3[e.RowIndex, "lblHinName"].Style.BackColor = Color.MistyRose;
                    gcMultiRow3[e.RowIndex, "txtSuu"].Style.BackColor = Color.MistyRose;
                }
                else
                {
                    gcMultiRow3[e.RowIndex, "txtHinCode"].Style.BackColor = Color.White;
                    gcMultiRow3[e.RowIndex, "lblHinName"].Style.BackColor = Color.White;
                    gcMultiRow3[e.RowIndex, "txtSuu"].Style.BackColor = Color.White;
                }

                if (!showStatus)
                {
                    // 出荷基準判定
                    kijunCheckMain();
                }
            }

            if (e.CellName == "txtSuu2")
            {
                if (Utility.NulltoStr(gcMultiRow3[e.RowIndex, "txtSuu2"].Value) != string.Empty)
                {
                    gcMultiRow3[e.RowIndex, "txtHinCode2"].Style.BackColor = Color.MistyRose;
                    gcMultiRow3[e.RowIndex, "lblHinName2"].Style.BackColor = Color.MistyRose;
                    gcMultiRow3[e.RowIndex, "txtSuu2"].Style.BackColor = Color.MistyRose;
                }
                else
                {
                    gcMultiRow3[e.RowIndex, "txtHinCode2"].Style.BackColor = Color.White;
                    gcMultiRow3[e.RowIndex, "lblHinName2"].Style.BackColor = Color.White;
                    gcMultiRow3[e.RowIndex, "txtSuu2"].Style.BackColor = Color.White;
                }
                                
                if (!showStatus)
                {
                    // 出荷基準判定
                    kijunCheckMain();
                }
            }
        }

        ///------------------------------------------------------------------------
        /// <summary>
        ///     商品コードから商品名を表示する </summary>
        /// <param name="gc">
        ///     GcMultiRowオブジェクト</param>
        /// <param name="cCellName">
        ///     該当セルの名前</param>
        /// <param name="rIndex">
        ///     該当セルのrowIndex</param>
        /// <param name="iriTani">
        ///     true:入数、単位も表示する、false:入数、単位は表示しない</param>
        ///------------------------------------------------------------------------
        private void gcHinCodeChange(GcMultiRow gc, string cCellName, int rIndex, bool iriTani)
        {
            string hinCode = string.Empty;

            if (cCellName == "txtHinCode")
            {
                hinCode = Utility.NulltoStr(gc[rIndex, "txtHinCode"].Value).PadLeft(8, '0');

                if (hinCode != "00000000")
                {
                    gc[rIndex, "txtHinCode"].Value = hinCode;
                }

                gc[rIndex, "lblHinName"].Value = string.Empty;

                if (iriTani)
                {
                    gc[rIndex, "lblIrisu"].Value = string.Empty;
                    gc[rIndex, "lblTani"].Value = string.Empty;
                }
            }
            else if (cCellName == "txtHinCode2")
            {
                hinCode = Utility.NulltoStr(gc[rIndex, "txtHinCode2"].Value).PadLeft(8, '0');

                if (hinCode != "00000000")
                {
                    gc[rIndex, "txtHinCode2"].Value = hinCode;
                }

                gc[rIndex, "lblHinName2"].Value = string.Empty;

                if (iriTani)
                {
                    gc[rIndex, "lblIrisu2"].Value = string.Empty;
                    gc[rIndex, "lblTani2"].Value = string.Empty;
                }
            }

            string strSQL = "select SYO_ID, SYO_NAME, SYO_IRI_KESU, SYO_TANI from RAKUSYO_FAXOCR.V_SYOHIN WHERE SYO_ID = '" + hinCode + "'";
            OracleCommand Cmd = new OracleCommand(strSQL, Conn);
            OracleDataReader dR = Cmd.ExecuteReader();

            while (dR.Read())
            {
                if (cCellName == "txtHinCode")
                {
                    gc[rIndex, "lblHinName"].Value = dR["SYO_NAME"].ToString().Trim();

                    if (iriTani)
                    {
                        gc[rIndex, "lblIrisu"].Value = dR["SYO_IRI_KESU"].ToString().Trim();
                        gc[rIndex, "lblTani"].Value = dR["SYO_TANI"].ToString().Trim();
                    }
                }
                else if (cCellName == "txtHinCode2")
                {
                    gc[rIndex, "lblHinName2"].Value = dR["SYO_NAME"].ToString().Trim();

                    if (iriTani)
                    {
                        gc[rIndex, "lblIrisu2"].Value = dR["SYO_IRI_KESU"].ToString().Trim();
                        gc[rIndex, "lblTani2"].Value = dR["SYO_TANI"].ToString().Trim();
                    }
                }
            }

            dR.Dispose();
            Cmd.Dispose();
        }



        private void gcMultiRow3_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            if (e.Control is TextBoxEditingControl)
            {
                //イベントハンドラが複数回追加されてしまうので最初に削除する
                e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
                e.Control.KeyDown -= new KeyEventHandler(Control_KeyDownHin);

                // 数字のみ入力可能とする
                if (gcMultiRow3.CurrentCell.Name == "txtHinCode" || gcMultiRow3.CurrentCell.Name == "txtHinCode2" ||
                    gcMultiRow3.CurrentCell.Name == "txtSuu" || gcMultiRow3.CurrentCell.Name == "txtSuu2")
                {
                    //イベントハンドラを追加する
                    e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }

                // 商品検索画面呼出
                if (gcMultiRow3.CurrentCell.Name == "txtHinCode" || gcMultiRow3.CurrentCell.Name == "txtHinCode2")
                {
                    //イベントハンドラを追加する
                    e.Control.KeyDown += new KeyEventHandler(Control_KeyDownHin);
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // エラーチェック
            errCheckClick();
        }

        ///---------------------------------------------------------
        /// <summary>
        ///     エラーチェック実行 </summary>
        ///---------------------------------------------------------
        private void errCheckClick()
        {
            // 非ログ書き込み状態とする：2015/09/25
            editLogStatus = false;

            // OCRDataクラス生成
            OCRData ocr = new OCRData(Conn);

            // エラーチェックを実行
            if (getErrData(cI, ocr))
            {
                MessageBox.Show("エラーはありませんでした", "エラーチェック", MessageBoxButtons.OK, MessageBoxIcon.Information);
                gcMultiRow1.CurrentCell = null;
                gcMultiRow2.CurrentCell = null;
                gcMultiRow3.CurrentCell = null;

                // データ表示
                showOcrData(cI);
            }
            else
            {
                // カレントインデックスをエラーありインデックスで更新
                cI = ocr._errHeaderIndex;

                // データ表示
                showOcrData(cI);

                // エラー表示
                ErrShow(ocr);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            // 非ログ書き込み状態とする
            editLogStatus = false;

            // 楽商TXTデータ出力
            textDataMake();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // FAX発注書削除
            faxDelete();
        }

        ///------------------------------------------------------------------
        /// <summary>
        ///     FAX発注書削除  </summary>
        ///------------------------------------------------------------------
        private void faxDelete()
        {
            if (MessageBox.Show("表示中のＦＡＸ発注書を削除します。よろしいですか", "削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                return;

            // 非ログ書き込み状態とする
            editLogStatus = false;

            // レコードと画像ファイルを削除する
            DataDelete(cI);

            // 件数カウント
            if (dtsC.FAX注文書.Count() > 0)
            {
                // カレントレコードインデックスを再設定
                if (dtsC.FAX注文書.Count() - 1 < cI) cI = dtsC.FAX注文書.Count() - 1;

                // データ画面表示
                showOcrData(cI);
            }
            else
            {
                // ゼロならばプログラム終了
                MessageBox.Show("全ての発注書データが削除されました。処理を終了します。", "発注書削除", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                //終了処理
                this.Tag = END_NODATA;
                this.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 非ログ書き込み状態とする
            editLogStatus = false;

            // フォームを閉じる
            this.Tag = END_BUTTON;
            this.Close();
        }

        private void frmCorrect_KeyDown(object sender, KeyEventArgs e)
        {
            // 先頭データへ移動
            if (e.KeyData == Keys.F1 && btnFirst.Enabled)
            {
                gcMultiRow1.EndEdit();
                gcMultiRow2.EndEdit();
                gcMultiRow3.EndEdit();

                //カレントデータの更新
                CurDataUpDate(cID[cI]);

                //レコードの移動
                cI = 0;
                showOcrData(cI);
            }

            // 前データへ移動
            if (e.KeyData == Keys.F2 && btnBefore.Enabled)
            {
                gcMultiRow1.EndEdit();
                gcMultiRow2.EndEdit();
                gcMultiRow3.EndEdit();

                //カレントデータの更新
                CurDataUpDate(cID[cI]);

                //レコードの移動
                if (cI > 0)
                {
                    cI--;
                    showOcrData(cI);
                }
            }

            // 次データへ移動
            if (e.KeyData == Keys.F3 && btnNext.Enabled)
            {
                gcMultiRow1.EndEdit();
                gcMultiRow2.EndEdit();
                gcMultiRow3.EndEdit();

                //カレントデータの更新
                CurDataUpDate(cID[cI]);

                //レコードの移動
                if (cI + 1 < dtsC.FAX注文書.Rows.Count)
                {
                    cI++;
                    showOcrData(cI);
                }
            }

            // 最後尾データへ移動
            if (e.KeyData == Keys.F4 && btnEnd.Enabled)
            {
                gcMultiRow1.EndEdit();
                gcMultiRow2.EndEdit();
                gcMultiRow3.EndEdit();

                //カレントデータの更新
                CurDataUpDate(cID[cI]);

                //レコードの移動
                cI = dtsC.FAX注文書.Rows.Count - 1;
                showOcrData(cI);
            }

            // 画像拡大
            if (e.KeyData == Keys.F5)
            {
                if (leadImg.ScaleFactor < gl.ZOOM_MAX)
                {
                    leadImg.ScaleFactor += gl.ZOOM_STEP;
                }
                gl.miMdlZoomRate = (float)leadImg.ScaleFactor;
            }

            // 画像縮小
            if (e.KeyData == Keys.F6)
            {
                if (leadImg.ScaleFactor > gl.ZOOM_MIN)
                {
                    leadImg.ScaleFactor -= gl.ZOOM_STEP;
                }
                gl.miMdlZoomRate = (float)leadImg.ScaleFactor;
            }
            
            // エラーチェック実行
            if (e.KeyData == Keys.F7 && btnErrCheck.Enabled)
            {
                gcMultiRow1.EndEdit();
                gcMultiRow2.EndEdit();
                gcMultiRow3.EndEdit();

                errCheckClick();
            }

            //  楽商データ作成
            if (e.KeyData == Keys.F8 && btnDataMake.Enabled)
            {
                textDataMake();
            }
          
            // ＦＡＸ発注書削除
            if (e.KeyData == Keys.F9 && btnDelete.Enabled)
            {                
                faxDelete();
            }

            // 保留処理
            if (e.KeyData == Keys.F10 && btnHold.Enabled)
            {
                if (MessageBox.Show("表示中のＦＡＸ発注書を保留にします。よろしいですか", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                //カレントデータの更新 : 2017/07/14
                CurDataUpDate(cID[cI]);

                // 保留処理
                setHoldData(cID[cI]);
            }
            
            // 画像印刷
            if (e.KeyData == Keys.F11 && btnPrint.Enabled)
            {
                if (MessageBox.Show("画像を印刷します。よろしいですか？", "印刷確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }

                // 印刷実行
                printDocument1.Print();
            }

            // 終了
            if (e.KeyData == Keys.F12)
            {
                // 非ログ書き込み状態とする
                editLogStatus = false;

                // フォームを閉じる
                this.Tag = END_BUTTON;
                this.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (leadImg.ScaleFactor < gl.ZOOM_MAX)
            {
                leadImg.ScaleFactor += gl.ZOOM_STEP;
            }
            gl.miMdlZoomRate = (float)leadImg.ScaleFactor;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (leadImg.ScaleFactor > gl.ZOOM_MIN)
            {
                leadImg.ScaleFactor -= gl.ZOOM_STEP;
            }
            gl.miMdlZoomRate = (float)leadImg.ScaleFactor;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            //カレントデータの更新
            CurDataUpDate(cID[cI]);

            //レコードの移動
            cI = 0;
            showOcrData(cI);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            //カレントデータの更新
            CurDataUpDate(cID[cI]);

            //レコードの移動
            if (cI > 0)
            {
                cI--;
                showOcrData(cI);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //カレントデータの更新
            CurDataUpDate(cID[cI]);

            //レコードの移動
            if (cI + 1 < dtsC.FAX注文書.Rows.Count)
            {
                cI++;
                showOcrData(cI);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //カレントデータの更新
            CurDataUpDate(cID[cI]);

            //レコードの移動
            cI = dtsC.FAX注文書.Rows.Count - 1;
            showOcrData(cI);
        }

        private void gcMultiRow3_Leave(object sender, EventArgs e)
        {
            gcMultiRow3.EndEdit();  
        }

        private void gcMultiRow1_Leave(object sender, EventArgs e)
        {
            gcMultiRow1.EndEdit();
        }

        private void gcMultiRow2_Leave(object sender, EventArgs e)
        {
            gcMultiRow2.EndEdit();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Image img;

            img = Image.FromFile(_imgFile);
            //e.Graphics.DrawImage(img, 0, 0);

            // 2017/12/12 縮小
            //e.Graphics.DrawImage(img, 0, 0, img.Width * 49 / 100, img.Height * 49 / 100);

            // 2018/06/21 元画像のピクセル調整を行わないことによる縮小調整
            e.Graphics.DrawImage(img, 0, 0, img.Width * 47 / 100, img.Height * 47 / 100);
            e.HasMorePages = false;

            img.Dispose();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("画像を印刷します。よろしいですか？", "印刷確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }

            // 印刷実行
            printDocument1.Print();
        }

        ///----------------------------------------------------------
        /// <summary>
        ///     保留処理 </summary>
        /// <param name="iX">
        ///     データインデックス</param>
        ///----------------------------------------------------------
        private void setHoldData(string iX)
        {
            try
            {
                NHBR_CLIDataSetTableAdapters.保留注文書TableAdapter hAdp = new NHBR_CLIDataSetTableAdapters.保留注文書TableAdapter();
                hAdp.Fill(dtsC.保留注文書);

                var t = dtsC.FAX注文書.Single(a => a.ID == iX);

                NHBR_CLIDataSet.保留注文書Row hr = dtsC.保留注文書.New保留注文書Row();
                hr.ID = t.ID;
                hr.画像名 = t.画像名;
                hr.届先番号 = t.届先番号;
                hr.パターンID = t.パターンID;
                hr.発注番号 = t.発注番号;
                hr.納品希望月 = t.納品希望月;
                hr.納品希望日 = t.納品希望日;
                hr.注文数1 = t.注文数1;
                hr.注文数2 = t.注文数2;
                hr.注文数3 = t.注文数3;
                hr.注文数4 = t.注文数4;
                hr.注文数5 = t.注文数5;
                hr.注文数6 = t.注文数6;
                hr.注文数7 = t.注文数7;
                hr.注文数8 = t.注文数8;
                hr.注文数9 = t.注文数9;
                hr.注文数10 = t.注文数10;
                hr.注文数11 = t.注文数11;
                hr.注文数12 = t.注文数12;
                hr.注文数13 = t.注文数13;
                hr.注文数14 = t.注文数14;
                hr.注文数15 = t.注文数15;
                hr.注文数16 = t.注文数16;
                hr.注文数17 = t.注文数17;
                hr.注文数18 = t.注文数18;
                hr.注文数19 = t.注文数19;
                hr.注文数20 = t.注文数20;
                hr.注文数21 = t.注文数21;
                hr.注文数22 = t.注文数22;
                hr.注文数23 = t.注文数23;
                hr.注文数24 = t.注文数24;
                hr.注文数25 = t.注文数25;
                hr.注文数26 = t.注文数26;
                hr.注文数27 = t.注文数27;
                hr.注文数28 = t.注文数28;
                hr.注文数29 = t.注文数29;
                hr.注文数30 = t.注文数30;
                hr.追加注文チェック = t.追加注文チェック;
                hr.追加注文数1 = t.追加注文数1;
                hr.追加注文数2 = t.追加注文数2;
                hr.追加注文数3 = t.追加注文数3;
                hr.追加注文数4 = t.追加注文数4;
                hr.追加注文数5 = t.追加注文数5;
                hr.追加注文数6 = t.追加注文数6;
                hr.追加注文数7 = t.追加注文数7;
                hr.追加注文数8 = t.追加注文数8;
                hr.追加注文数9 = t.追加注文数9;
                hr.追加注文数10 = t.追加注文数10;

                hr.追加注文商品コード1 = t.追加注文商品コード1;
                hr.追加注文商品コード2 = t.追加注文商品コード2;
                hr.追加注文商品コード3 = t.追加注文商品コード3;
                hr.追加注文商品コード4 = t.追加注文商品コード4;
                hr.追加注文商品コード5 = t.追加注文商品コード5;
                hr.追加注文商品コード6 = t.追加注文商品コード6;
                hr.追加注文商品コード7 = t.追加注文商品コード7;
                hr.追加注文商品コード8 = t.追加注文商品コード8;
                hr.追加注文商品コード9 = t.追加注文商品コード9;
                hr.追加注文商品コード10 = t.追加注文商品コード10;

                hr.担当者コード = t.担当者コード;
                hr.備考欄記入 = t.備考欄記入;
                hr.メモ = t.メモ;
                hr.エラー有無 = t.エラー有無;
                hr.更新年月日 = DateTime.Now;
                hr.確認 = t.確認;

                hr.出荷基準A = t.出荷基準A;
                hr.出荷基準B = t.出荷基準B;
                hr.出荷基準C = t.出荷基準C;
                hr.出荷基準D = t.出荷基準D;
                hr.出荷基準E = t.出荷基準E;
                hr.出荷基準F = t.出荷基準F;
                hr.出荷基準G = t.出荷基準G;

                // 2017/08/23
                hr.商品コード1 = t.商品コード1;
                hr.商品コード2 = t.商品コード2;
                hr.商品コード3 = t.商品コード3;
                hr.商品コード4 = t.商品コード4;
                hr.商品コード5 = t.商品コード5;
                hr.商品コード6 = t.商品コード6;
                hr.商品コード7 = t.商品コード7;
                hr.商品コード8 = t.商品コード8;
                hr.商品コード9 = t.商品コード9;
                hr.商品コード10 = t.商品コード10;

                hr.商品コード11 = t.商品コード11;
                hr.商品コード12 = t.商品コード12;
                hr.商品コード13 = t.商品コード13;
                hr.商品コード14 = t.商品コード14;
                hr.商品コード15 = t.商品コード15;
                hr.商品コード16 = t.商品コード16;
                hr.商品コード17 = t.商品コード17;
                hr.商品コード18 = t.商品コード18;
                hr.商品コード19 = t.商品コード19;
                hr.商品コード20 = t.商品コード20;

                hr.商品コード21 = t.商品コード21;
                hr.商品コード22 = t.商品コード22;
                hr.商品コード23 = t.商品コード23;
                hr.商品コード24 = t.商品コード24;
                hr.商品コード25 = t.商品コード25;
                hr.商品コード26 = t.商品コード26;
                hr.商品コード27 = t.商品コード27;
                hr.商品コード28 = t.商品コード28;
                hr.商品コード29 = t.商品コード29;
                hr.商品コード30 = t.商品コード30;

                // 保留データ追加処理
                dtsC.保留注文書.Add保留注文書Row(hr);
                hAdp.Update(dtsC.保留注文書);

                // ＦＡＸ発注書データ削除
                t.Delete();
                fAdp.Update(dtsC.FAX注文書);

                // 配列キー再構築
                keyArrayCreate();

                // 終了メッセージ
                MessageBox.Show("注文書が保留されました", "ＦＡＸ発注書保留", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 件数カウント
                if (dtsC.FAX注文書.Count() > 0)
                {
                    // カレントレコードインデックスを再設定
                    if (dtsC.FAX注文書.Count() - 1 < cI)
                    {
                        cI = dtsC.FAX注文書.Count() - 1;
                    }

                    // データ画面表示
                    showOcrData(cI);
                }
                else
                {
                    // ゼロならばプログラム終了
                    MessageBox.Show("全ての発注書データが削除されました。処理を終了します。", "発注書削除", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    //終了処理
                    this.Tag = END_NODATA;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("表示中のＦＡＸ発注書を保留にします。よろしいですか", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            //カレントデータの更新 : 2017/07/14
            CurDataUpDate(cID[cI]);

            // 保留処理
            setHoldData(cID[cI]);
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            kijunCheckMain();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            // 右へ90°回転させる
            Leadtools.ImageProcessing.RotateCommand rc = new Leadtools.ImageProcessing.RotateCommand();
            rc.Angle = 90 * 100;
            rc.FillColor = new Leadtools.RasterColor(255, 255, 255);
            //rc.Flags = RotateCommandFlags.Bicubic;
            rc.Flags = Leadtools.ImageProcessing.RotateCommandFlags.Resize;
            rc.Run(leadImg.Image);
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            // 左へ90°回転させる
            Leadtools.ImageProcessing.RotateCommand rc = new Leadtools.ImageProcessing.RotateCommand();
            rc.Angle = -90 * 100;
            rc.FillColor = new Leadtools.RasterColor(255, 255, 255);
            //rc.Flags = RotateCommandFlags.Bicubic;
            rc.Flags = Leadtools.ImageProcessing.RotateCommandFlags.Resize;
            rc.Run(leadImg.Image);
        }
    }
}
