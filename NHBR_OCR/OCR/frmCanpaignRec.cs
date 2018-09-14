using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHBR_OCR.common;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using GrapeCity.Win.MultiRow;
using Leadtools.ImageProcessing;
using Leadtools;

namespace NHBR_OCR.OCR
{
    public partial class frmCanpaignRec : Form
    {
        public frmCanpaignRec(string myCode)
        {
            InitializeComponent();

            _myCode = myCode;
            hAdp.Fill(dts.キャンペーンヘッダ);
            mAdp.Fill(dts.キャンペーン明細);
        }

        // グローバルクラス
        global gl = new global();

        OracleConnection Conn = new OracleConnection();

        NHBR_CLIDataSet dtsC = new NHBR_CLIDataSet();
        NHBRDataSet dts = new NHBRDataSet();
        NHBRDataSetTableAdapters.キャンペーンヘッダTableAdapter hAdp = new NHBRDataSetTableAdapters.キャンペーンヘッダTableAdapter();
        NHBRDataSetTableAdapters.キャンペーン明細TableAdapter mAdp = new NHBRDataSetTableAdapters.キャンペーン明細TableAdapter();

        string _myCode = string.Empty;
        string _imgFile = string.Empty;
        int imgX = 0;

        // キャンペーンデータクラス
        clsCamData[] camArray = null;

        private void frmCanpaignRec_Load(object sender, EventArgs e)
        {
            // フォーム最大値
            Utility.WindowsMaxSize(this, this.Width, this.Height);

            // フォーム最小値
            Utility.WindowsMinSize(this, this.Width, this.Height);

            // Tabキーの既定のショートカットキーを解除する。
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            gcMultiRow2.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow2.ShortcutKeyManager.Unregister(Keys.Enter);

            // Tabキーのショートカットキーにユーザー定義のショートカットキーを割り当てる。
            gcMultiRow1.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Enter);
            gcMultiRow2.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Tab);
            gcMultiRow2.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Enter);

            // 楽商データベース接続
            Conn.ConnectionString = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
            Conn.Open();

            // GCMultiRow初期化
            gcMrSetting();

            // tagを初期化
            this.Tag = string.Empty;

            // 現在の表示倍率を初期化
            gl.miMdlZoomRate = 0f;

            // 作業フォルダへ画像を移動する
            if (!System.IO.Directory.Exists(Properties.Settings.Default.camPaignWorhPath))
            {
                System.IO.Directory.CreateDirectory(Properties.Settings.Default.camPaignWorhPath);
            }

            foreach (var file in System.IO.Directory.GetFiles(Properties.Settings.Default.campaignPath))
            {
                System.IO.File.Move(file, Properties.Settings.Default.camPaignWorhPath + System.IO.Path.GetFileName(file));
            }

            // キャンペーンデータクラス生成・画像を配列取り込み
            getImageFile(Properties.Settings.Default.camPaignWorhPath);

            // 画像表示
            imgX = 0;
            if (camArray.Length > 0)
            {
                dataShow(imgX);
                btnFirst.Enabled = false;
                btnBefore.Enabled = false;
            }
        }

        private void gcMrSetting()
        {
            //multirow編集モード
            gcMultiRow1.EditMode = EditMode.EditProgrammatically;

            this.gcMultiRow1.AllowUserToAddRows = false;                    // 手動による行追加を禁止する
            this.gcMultiRow1.AllowUserToDeleteRows = false;                 // 手動による行削除を禁止する
            //this.gcMultiRow1.Rows.Clear();                                  // 行数をクリア
            this.gcMultiRow1.RowCount = 15;                                  // 行数を設定
            this.gcMultiRow1.HideSelection = true;                          // GcMultiRow コントロールがフォーカスを失ったとき、セルの選択状態を非表示にする

            //multirow編集モード
            gcMultiRow2.EditMode = EditMode.EditProgrammatically;

            this.gcMultiRow2.AllowUserToAddRows = false;                    // 手動による行追加を禁止する
            this.gcMultiRow2.AllowUserToDeleteRows = false;                 // 手動による行削除を禁止する
            //this.gcMultiRow1.Rows.Clear();                                  // 行数をクリア
            this.gcMultiRow2.RowCount = 1;                                  // 行数を設定
            this.gcMultiRow2.HideSelection = true;                          // GcMultiRow コントロールがフォーカスを失ったとき、セルの選択状態を非表示にする
        }

        private void gcMultiRow1_CellEnter(object sender, CellEventArgs e)
        {
            if (gcMultiRow1.EditMode == EditMode.EditProgrammatically)
            {
                gcMultiRow1.BeginEdit(true);
            }
        }

        private void gcMultiRow1_CellLeave(object sender, CellEventArgs e)
        {
            gcMultiRow1.EndEdit();
        }

        private void gcMultiRow2_CellEnter(object sender, CellEventArgs e)
        {
            if (gcMultiRow2.EditMode == EditMode.EditProgrammatically)
            {
                gcMultiRow2.BeginEdit(true);
            }
        }

        private void gcMultiRow2_CellLeave(object sender, CellEventArgs e)
        {
            gcMultiRow2.EndEdit();
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
            if (System.IO.File.Exists(tempImgName))
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

                label1.Text = (imgX + 1).ToString() + "/" + camArray.Length.ToString();
                //global.pblImagePath = tempImgName;
            }
            else
            {
                //画像ファイルがないとき
                lblNoImage.Visible = true;

                // 画像操作ボタン
                btnPlus.Enabled = false;
                btnMinus.Enabled = false;

                // 画像回転ボタン
                btnLeft.Enabled = false;
                btnRight.Enabled = false;

                leadImg.Visible = false;
                //global.pblImagePath = string.Empty;
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

        ///----------------------------------------------------------------
        /// <summary>
        ///     キャンペーンデータクラス生成・画像配列取り込み </summary>
        /// <param name="sPath">
        ///     キャンペーン画像フォルダパス</param>
        ///----------------------------------------------------------------
        private void getImageFile(string sPath)
        {
            imgX = 0;

            // キャンペーンデータクラス生成
            camArray = new clsCamData[System.IO.Directory.GetFiles(sPath, "*.tif").Count()];

            // 画像を１枚ずつ確認
            foreach (var f in System.IO.Directory.GetFiles(sPath, "*.tif"))
            {
                camArray[imgX] = new clsCamData();

                string csvFName = System.IO.Path.GetFileNameWithoutExtension(f) + ".csv";

                if (System.IO.File.Exists(Properties.Settings.Default.camPaignWorhPath + csvFName))
                {
                    bool firstRow = true;
                    int m = 0;

                    // 保存データがあるときCSVファイルインポート
                    foreach (var stBuffer in System.IO.File.ReadAllLines(Properties.Settings.Default.camPaignWorhPath + csvFName, Encoding.Default))
                    {
                        // カンマ区切りで分割して配列に格納する
                        string[] st = stBuffer.Split(',');

                        if (firstRow)
                        {
                            // ヘッダデータをセットする
                            camArray[imgX].cImgName = st[0];
                            camArray[imgX].cTdkCode = Utility.StrtoInt(st[1]);
                            camArray[imgX].cTdkName = st[2];
                            camArray[imgX].cHaNum = st[3];
                            camArray[imgX].cPreTdkCode = Utility.StrtoInt(st[4]);
                            camArray[imgX].cPreTdkName = st[5];
                            camArray[imgX].cCamName = st[6];
                            camArray[imgX].cSDate = st[7];
                            camArray[imgX].cEDate = st[8];
                            camArray[imgX].cStatus = bool.Parse(st[9]);
                            firstRow = false;
                        }
                        else
                        {
                            // 明細データをセットする
                            camArray[imgX].cCheck[m] = bool.Parse(st[0]);
                            camArray[imgX].cSCode[m] = Utility.StrtoInt(st[1]);
                            camArray[imgX].cSName[m] = st[2];
                            camArray[imgX].cSSu[m] = Utility.StrtoInt(st[3]);
                            camArray[imgX].cPCode[m] = Utility.StrtoInt(st[4]);
                            camArray[imgX].cPName[m] = st[5];
                            camArray[imgX].cPSu[m] = Utility.StrtoInt(st[6]);
                            m++;
                        }
                    }
                }
                else
                {                    
                    // 新規画像のとき
                    camArray[imgX].cImgName = f.ToString();
                }

                imgX++;
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            // データを配列にセーブ
            setCamDataArray(imgX);

            // 最初のデータを表示
            imgX = 0;
            dataShow(imgX);

            btnFirst.Enabled = false;
            btnBefore.Enabled = false;
            btnNext.Enabled = true;
            btnEnd.Enabled = true;
        }

        private void btnBefore_Click(object sender, EventArgs e)
        {
            if (imgX > 0)
            {
                // データを配列にセーブ
                setCamDataArray(imgX);

                // 前データを表示
                imgX--;
                dataShow(imgX);

                if (imgX == 0)
                {
                    btnFirst.Enabled = false;
                    btnBefore.Enabled = false;
                    btnNext.Enabled = true;
                    btnEnd.Enabled = true;
                }
                else
                {
                    btnFirst.Enabled = true;
                    btnBefore.Enabled = true;
                    btnNext.Enabled = true;
                    btnEnd.Enabled = true;
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // データを配列にセーブ
            setCamDataArray(imgX);

            // 次データを表示
            if (imgX < (camArray.Length - 1))
            {
                imgX++;
                dataShow(imgX);
            }
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            // データを配列にセーブ
            setCamDataArray(imgX);

            // 最終データを表示
            imgX = camArray.Length - 1;
            dataShow(imgX);
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            // 左へ90°回転させる
            RotateCommand rc = new RotateCommand();
            rc.Angle = -90 * 100;
            rc.FillColor = new RasterColor(255, 255, 255);
            //rc.Flags = RotateCommandFlags.Bicubic;
            rc.Flags = RotateCommandFlags.Resize;
            rc.Run(leadImg.Image);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            // 右へ90°回転させる
            RotateCommand rc = new RotateCommand();
            rc.Angle = 90 * 100;
            rc.FillColor = new RasterColor(255, 255, 255);
            //rc.Flags = RotateCommandFlags.Bicubic;
            rc.Flags = RotateCommandFlags.Resize;
            rc.Run(leadImg.Image);
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            if (leadImg.ScaleFactor < gl.ZOOM_MAX)
            {
                leadImg.ScaleFactor += gl.ZOOM_STEP;
            }
            gl.miMdlZoomRate = (float)leadImg.ScaleFactor;
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            if (leadImg.ScaleFactor > gl.ZOOM_MIN)
            {
                leadImg.ScaleFactor -= gl.ZOOM_STEP;
            }
            gl.miMdlZoomRate = (float)leadImg.ScaleFactor;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Image img;

            img = Image.FromFile(camArray[imgX].cImgName);
            e.Graphics.DrawImage(img, 0, 0);
            e.HasMorePages = false;

            img.Dispose();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("表示中の画像を印刷します。よろしいですか。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            printDocument1.Print();
        }

        private void gcMultiRow2_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            if (e.Control is TextBoxEditingControl)
            {
                //イベントハンドラが複数回追加されてしまうので最初に削除する
                e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
                e.Control.KeyDown -= new KeyEventHandler(Control_KeyDown2);
                e.Control.KeyDown -= new KeyEventHandler(Control_KeyDown3);

                // 数字のみ入力可能とする
                if (gcMultiRow2.CurrentCell.Name == "txtTdkCode" || gcMultiRow2.CurrentCell.Name == "txtHaNum" ||
                    gcMultiRow2.CurrentCell.Name == "txtPreTdkCode")
                {
                    //イベントハンドラを追加する
                    e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }

                // お客様検索画面
                if (gcMultiRow2.CurrentCell.Name == "txtTdkCode")
                {
                    //イベントハンドラを追加する
                    e.Control.KeyDown += new KeyEventHandler(Control_KeyDown2);
                }
                
                // お客様検索画面
                if (gcMultiRow2.CurrentCell.Name == "txtPreTdkCode")
                {
                    //イベントハンドラを追加する
                    e.Control.KeyDown += new KeyEventHandler(Control_KeyDown3);
                }
            }
        }

        void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '\t')
                e.Handled = true;
        }

        private void Control_KeyDown2(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
            {
                gcMultiRow2.EndEdit();

                frmTodoke frm = new frmTodoke(false);
                frm.ShowDialog();

                if (frm._nouCode != null)
                {
                    gcMultiRow2.SetValue(0, "txtTdkCode", frm._nouCode[0]);
                    gcMultiRow2.BeginEdit(true);
                    gcMultiRow2.CurrentCellPosition = new CellPosition(0, "txtHaNum");
                }

                // 後片付け
                frm.Dispose();
            }
        }

        private void Control_KeyDown3(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
            {
                gcMultiRow2.EndEdit();

                frmTodoke frm = new frmTodoke(false);
                frm.ShowDialog();

                if (frm._nouCode != null)
                {
                    gcMultiRow2.SetValue(0, "txtPreTdkCode", frm._nouCode[0]);
                    gcMultiRow2.BeginEdit(true);
                    gcMultiRow2.CurrentCellPosition = new CellPosition(0, "txtHaNum");
                }

                // 後片付け
                frm.Dispose();
            }
        }

        private void gcMultiRow2_CellValueChanged(object sender, CellEventArgs e)
        {
            if (!gl.ChangeValueStatus)
            {
                return;
            }

            // ChangeValueイベントを発生させない
            gl.ChangeValueStatus = false;

            // 2018/02/20
            if (e.CellName == "txtTdkCode")
            {
                // お客様名を初期化
                gcMultiRow2[e.RowIndex, "lblTdkName"].Value = string.Empty;

                // 楽商データベースよりお客様名を取得して表示します
                if (Utility.NulltoStr(gcMultiRow2[0, "txtTdkCode"].Value) != string.Empty)
                {
                    // 届先名
                    string gName = string.Empty;
                    string bCode = gcMultiRow2[e.RowIndex, "txtTdkCode"].Value.ToString().PadLeft(6, '0');
                    gName = getUserName(bCode);
                    gcMultiRow2[e.RowIndex, "lblTdkName"].Value = gName;
                }
            }

            // 2018/02/20 プレゼント商品届先
            if (e.CellName == "txtPreTdkCode")
            {
                // お客様名を初期化
                gcMultiRow2[e.RowIndex, "lblPreTdkName"].Value = string.Empty;

                // 楽商データベースよりお客様名を取得して表示します
                if (Utility.NulltoStr(gcMultiRow2[0, "txtPreTdkCode"].Value) != string.Empty)
                {
                    // 届先名
                    string gName = string.Empty;
                    string bCode = gcMultiRow2[e.RowIndex, "txtPreTdkCode"].Value.ToString().PadLeft(6, '0');
                    gName = getUserName(bCode);
                    gcMultiRow2[e.RowIndex, "lblPreTdkName"].Value = gName;
                }
            }

            // ChangeValueイベントステータスをtrueに戻す
            gl.ChangeValueStatus = true;
        }


        ///-------------------------------------------------------------------
        /// <summary>
        ///     お届け先情報取得 </summary>
        /// <param name="tID">
        ///     届先番号</param>
        ///-------------------------------------------------------------------
        private string getUserName(string tID)
        {
            string val = string.Empty;

            string strSQL = "SELECT KOK_ID, NOU_NAME, NOU_JYU1, NOU_JYU2, NOU_TEL from RAKUSYO_FAXOCR.V_NOUHINSAKI WHERE KOK_ID = '" + tID + "'";
            OracleCommand Cmd = new OracleCommand(strSQL, Conn);
            OracleDataReader dR = Cmd.ExecuteReader();
            while (dR.Read())
            {
                val = dR["NOU_NAME"].ToString().Trim();
            }

            dR.Dispose();
            Cmd.Dispose();

            return val;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 閉じる
            this.Close();
        }

        private void frmCanpaignRec_FormClosing(object sender, FormClosingEventArgs e)
        {
            // データを配列にセーブ
            setCamDataArray(imgX);

            if (System.IO.Directory.GetFiles(Properties.Settings.Default.camPaignWorhPath, "*.tif").Count() > 0)
            {
                int w = 0;

                clsCamData ccd = new clsCamData();
                for (int i = 0; i < camArray.Length; i++)
                {
                    if (camArray[i] != null)
                    {
                        ccd.putCamCsv(camArray[i]);
                        w++;
                    }
                }

                MessageBox.Show(w + "件のデータを保存しました", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // データベース接続解除
            Conn.Close();
            Conn.Dispose();

            // 後片付け
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int iX = 0;

            frmCamGet frm = new frmCamGet();
            frm.ShowDialog();

            if (frm.sID != 0)
            {
                gcMultiRow1.ColumnHeaders[0].Cells["lblCamName"].Value = frm.sName;
                gcMultiRow1.ColumnHeaders[0].Cells["lblSdate"].Value = frm.sDate;
                gcMultiRow1.ColumnHeaders[0].Cells["lblEdate"].Value = frm.eDate;
                
                foreach (var t in dts.キャンペーン明細.Where(a => a.ヘッダID == frm.sID).OrderBy(a => a.ID))
                {
                    gcMultiRow1.SetValue(iX, "checkBoxCell1", true);
                    gcMultiRow1.SetValue(iX, "lblNum", iX + 1);
                    gcMultiRow1.SetValue(iX, "txtSCode", t.商品コード);
                    gcMultiRow1.SetValue(iX, "txtSSu", t.商品数量);
                    gcMultiRow1.SetValue(iX, "txtPCode", t.プレゼント商品コード);
                    gcMultiRow1.SetValue(iX, "txtPSu", t.プレゼント数量);
                    
                    iX++;
                }

                //gcMultiRow1.CurrentCellPosition = new CellPosition(0, "txtHaNum");
            }

            // 後片付け
            frm.Dispose();
        }

        private void gcMultirow2_Clear()
        {
        }

        ///---------------------------------------------------
        /// <summary>
        ///     商品・プレゼント欄表示初期化 </summary>
        ///---------------------------------------------------
        private void gcMultirow1_Clear()
        {
            gcMultiRow2.SetValue(0, "txtTdkCode", "");
            gcMultiRow2.SetValue(0, "txtHaNum", "");

            gcMultiRow1.ColumnHeaders[0].Cells["lblCamName"].Value = "";
            gcMultiRow1.ColumnHeaders[0].Cells["lblSdate"].Value = "";
            gcMultiRow1.ColumnHeaders[0].Cells["lblEdate"].Value = "";

            for (int i = 0; i < 15; i++)
            {
                gcMultiRow1.SetValue(i, "checkBoxCell1", false);
                gcMultiRow1.SetValue(i, "lblNum", "");
                gcMultiRow1.SetValue(i, "txtSCode", "");
                gcMultiRow1.SetValue(i, "txtSSu", "");
                gcMultiRow1.SetValue(i, "txtPCode", "");
                gcMultiRow1.SetValue(i, "txtPSu", "");
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
        ///------------------------------------------------------------------------
        private void gcHinCodeChange(GcMultiRow gc, string cCellName, int rIndex)
        {
            string hinCode = string.Empty;

            if (cCellName == "txtSCode")
            {
                hinCode = Utility.NulltoStr(gc[rIndex, "txtSCode"].Value).PadLeft(8, '0');

                if (hinCode != "00000000")
                {
                    gc[rIndex, "txtSCode"].Value = hinCode;
                }

                gc[rIndex, "lblSName"].Value = string.Empty;
            }
            else if (cCellName == "txtPCode")
            {
                hinCode = Utility.NulltoStr(gc[rIndex, "txtPCode"].Value).PadLeft(8, '0');

                if (hinCode != "00000000")
                {
                    gc[rIndex, "txtPCode"].Value = hinCode;
                }

                gc[rIndex, "lblPName"].Value = string.Empty;
            }

            string strSQL = "select SYO_ID, SYO_NAME, SYO_IRI_KESU, SYO_TANI from RAKUSYO_FAXOCR.V_SYOHIN WHERE SYO_ID = '" + hinCode + "'";
            OracleCommand Cmd = new OracleCommand(strSQL, Conn);
            OracleDataReader dR = Cmd.ExecuteReader();

            while (dR.Read())
            {
                if (cCellName == "txtSCode")
                {
                    gc[rIndex, "lblSName"].Value = dR["SYO_NAME"].ToString().Trim();
                }
                else if (cCellName == "txtPCode")
                {
                    gc[rIndex, "lblPName"].Value = dR["SYO_NAME"].ToString().Trim();
                }
            }

            dR.Dispose();
            Cmd.Dispose();
        }

        private void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
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

            // チェックボックス
            if (e.CellName == "checkBoxCell1")
            {
                if (gcMultiRow1[e.RowIndex, "checkBoxCell1"].Value.ToString() == "False")
                {
                    gcMultiRow1.Rows[e.RowIndex].BackColor = SystemColors.Control;
                    //gcMultiRow1[e.RowIndex, "checkBoxCell1"].ReadOnly = true;
                    gcMultiRow1[e.RowIndex, "txtSCode"].ReadOnly = true;
                    gcMultiRow1[e.RowIndex, "txtSSu"].ReadOnly = true;
                    gcMultiRow1[e.RowIndex, "txtPCode"].ReadOnly = true;
                    gcMultiRow1[e.RowIndex, "txtPSu"].ReadOnly = true;

                    gcMultiRow1[e.RowIndex, "checkBoxCell1"].Style.ForeColor = Color.LightGray;
                    gcMultiRow1[e.RowIndex, "txtSCode"].Style.ForeColor = Color.LightGray;
                    gcMultiRow1[e.RowIndex, "lblSName"].Style.ForeColor = Color.LightGray;
                    gcMultiRow1[e.RowIndex, "txtSSu"].Style.ForeColor = Color.LightGray;
                    gcMultiRow1[e.RowIndex, "txtPCode"].Style.ForeColor = Color.LightGray;
                    gcMultiRow1[e.RowIndex, "lblPName"].Style.ForeColor = Color.LightGray;
                    gcMultiRow1[e.RowIndex, "txtPSu"].Style.ForeColor = Color.LightGray;
                }
                else
                {
                    gcMultiRow1.Rows[e.RowIndex].BackColor = Color.Empty;
                    //gcMultiRow1[e.RowIndex, "checkBoxCell1"].ReadOnly = false;
                    gcMultiRow1[e.RowIndex, "txtSCode"].ReadOnly = false;
                    gcMultiRow1[e.RowIndex, "txtSSu"].ReadOnly = false;
                    gcMultiRow1[e.RowIndex, "txtPCode"].ReadOnly = false;
                    gcMultiRow1[e.RowIndex, "txtPSu"].ReadOnly = false;

                    gcMultiRow1[e.RowIndex, "checkBoxCell1"].Style.ForeColor = Color.Empty;
                    gcMultiRow1[e.RowIndex, "txtSCode"].Style.ForeColor = Color.Empty;
                    gcMultiRow1[e.RowIndex, "lblSName"].Style.ForeColor = Color.Empty;
                    gcMultiRow1[e.RowIndex, "txtSSu"].Style.ForeColor = Color.Empty;
                    gcMultiRow1[e.RowIndex, "txtPCode"].Style.ForeColor = Color.Empty;
                    gcMultiRow1[e.RowIndex, "lblPName"].Style.ForeColor = Color.Empty;
                    gcMultiRow1[e.RowIndex, "txtPSu"].Style.ForeColor = Color.Empty;
                }
            }

            
            // 商品名表示
            if (e.CellName == "txtSCode" || e.CellName == "txtPCode")
            {
                gcHinCodeChange(gcMultiRow1, e.CellName, e.RowIndex);
            }

            gl.ChangeValueStatus = true;

        }

        private void gcMultiRow1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            string colName = gcMultiRow1.CurrentCell.Name;

            if (colName == "checkBoxCell1")
            {
                if (gcMultiRow1.IsCurrentCellDirty)
                {
                    gcMultiRow1.CommitEdit(DataErrorContexts.Commit);
                    gcMultiRow1.Refresh();
                }
            }
        }

        private void gcMultiRow1_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            if (e.Control is TextBoxEditingControl)
            {
                //イベントハンドラが複数回追加されてしまうので最初に削除する
                e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
                e.Control.KeyDown -= new KeyEventHandler(Control_KeyDownHinM2);

                // 数字のみ入力可能とする
                if (gcMultiRow1.CurrentCell.Name == "txtSCode" || gcMultiRow1.CurrentCell.Name == "txtPCode" ||
                    gcMultiRow1.CurrentCell.Name == "txtSSu" || gcMultiRow1.CurrentCell.Name == "txtPSu")
                {
                    //イベントハンドラを追加する
                    e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }

                // 商品検索画面呼出
                if (gcMultiRow1.CurrentCell.Name == "txtSCode" || gcMultiRow1.CurrentCell.Name == "txtPCode")
                {
                    //イベントハンドラを追加する
                    e.Control.KeyDown += new KeyEventHandler(Control_KeyDownHinM2);
                }
            }
        }

        private void Control_KeyDownHinM2(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
            {
                gcMultiRow1.EndEdit();

                frmSyohin frm = new frmSyohin(false);
                frm.ShowDialog();

                if (frm._nouCode != null)
                {
                    gcMultiRow1.SetValue(gcMultiRow1.CurrentCell.RowIndex, gcMultiRow1.CurrentCellPosition.CellName, frm._nouCode[0]);

                    if (gcMultiRow1.CurrentCellPosition.CellName == "txtSCode")
                    {
                        gcMultiRow1.CurrentCellPosition = new CellPosition(gcMultiRow1.CurrentCell.RowIndex, "txtSSu");
                        gcMultiRow1.BeginEdit(true);
                    }
                    else if (gcMultiRow1.CurrentCellPosition.CellName == "txtPCode")
                    {
                        gcMultiRow1.CurrentCellPosition = new CellPosition(gcMultiRow1.CurrentCell.RowIndex, "txtPSu");
                        gcMultiRow1.BeginEdit(true);
                    }
                }

                // 後片付け
                frm.Dispose();
            }
        }

        ///------------------------------------------------------
        /// <summary>
        ///     配列の内容を画面表示する </summary>
        /// <param name="i">
        ///     配列インデックス</param>
        ///------------------------------------------------------
        private void dataShow(int i)
        {
            // 画面初期化
            gcMultirow1_Clear();
            txtMemo.Text = string.Empty;

            // データ表示
            int num = 0;
            gcMultiRow2.SetValue(0, "txtTdkCode", camArray[i].cTdkCode);
            gcMultiRow2.SetValue(0, "txtHaNum", camArray[i].cHaNum);
            gcMultiRow2.SetValue(0, "txtPreTdkCode", camArray[i].cPreTdkCode);  // 2018/02/20

            gcMultiRow1.ColumnHeaders[0].Cells["lblCamName"].Value = camArray[i].cCamName;
            gcMultiRow1.ColumnHeaders[0].Cells["lblSdate"].Value = camArray[i].cSDate;
            gcMultiRow1.ColumnHeaders[0].Cells["lblEdate"].Value = camArray[i].cEDate;

            for (int iX = 0; iX < 15; iX++)
            {
                gcMultiRow1.SetValue(iX, "checkBoxCell1", camArray[i].cCheck[iX]);

                if (camArray[i].cCheck[iX])
                {
                    gcMultiRow1.SetValue(iX, "lblNum", num + 1);
                    gcMultiRow1.SetValue(iX, "txtSCode", camArray[i].cSCode[iX]);
                    gcMultiRow1.SetValue(iX, "txtSSu", camArray[i].cSSu[iX]);
                    gcMultiRow1.SetValue(iX, "txtPCode", camArray[i].cPCode[iX]);
                    gcMultiRow1.SetValue(iX, "txtPSu", camArray[i].cPSu[iX]);
                    num++;
                }
            }

            checkBox1.Checked = camArray[i].cStatus;
            txtMemo.Text = camArray[i].cMemo;
            lblMsg.Text = string.Empty;

            // 画像表示
            ShowImage(camArray[i].cImgName);

            // 移動ボタン状態
            if (camArray.Length == 1)
            {
                // 画像が１件のとき
                btnFirst.Enabled = false;
                btnBefore.Enabled = false;
                btnNext.Enabled = false;
                btnEnd.Enabled = false;
            }
            else
            {
                if (imgX == 0)
                {
                    // 先頭画像のとき
                    btnFirst.Enabled = false;
                    btnBefore.Enabled = false;
                    btnNext.Enabled = true;
                    btnEnd.Enabled = true;
                }
                else if (imgX == (camArray.Length - 1))
                {
                    // 最後の画像のとき
                    btnNext.Enabled = false;
                    btnEnd.Enabled = false;
                    btnFirst.Enabled = true;
                    btnBefore.Enabled = true;
                }
                else
                {
                    btnFirst.Enabled = true;
                    btnBefore.Enabled = true;
                    btnNext.Enabled = true;
                    btnEnd.Enabled = true;
                }
            }

            gcMultiRow1.CurrentCell = null;
            gcMultiRow2.CurrentCell = null;
            btnDataMake.Focus();
        }

        ///------------------------------------------------------------
        /// <summary>
        ///     キャンペーン配列データをCSVデータ出力する </summary>
        /// <param name="sID">
        ///     配列インデックス</param>
        ///------------------------------------------------------------
        private void setCamDataArray(int sID)
        {
            camArray[sID].cTdkCode = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow2[0, "txtTdkCode"].Value));
            camArray[sID].cTdkName = Utility.NulltoStr(gcMultiRow2[0, "lblTdkName"].Value);
            camArray[sID].cHaNum = Utility.NulltoStr(gcMultiRow2[0, "txtHaNum"].Value);
            camArray[sID].cCamName = Utility.NulltoStr(gcMultiRow1.ColumnHeaders[0].Cells["lblCamName"].Value);
            camArray[sID].cSDate = Utility.NulltoStr(gcMultiRow1.ColumnHeaders[0].Cells["lblSdate"].Value);
            camArray[sID].cEDate = Utility.NulltoStr(gcMultiRow1.ColumnHeaders[0].Cells["lblEdate"].Value);
            camArray[sID].cStatus = checkBox1.Checked;
            camArray[sID].cMemo = txtMemo.Text;

            // 2018/02/20
            camArray[sID].cPreTdkCode = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow2[0, "txtPreTdkCode"].Value));
            camArray[sID].cPreTdkName = Utility.NulltoStr(gcMultiRow2[0, "lblPreTdkName"].Value);

            for (int i = 0; i < 15; i++)
            {
                camArray[sID].cCheck[i] = bool.Parse(gcMultiRow1[i, "checkBoxCell1"].Value.ToString());
                camArray[sID].cSCode[i] = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[i, "txtSCode"].Value));
                camArray[sID].cSName[i] = Utility.NulltoStr(gcMultiRow1[i, "lblSName"].Value);
                camArray[sID].cSSu[i] = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[i, "txtSSu"].Value));
                camArray[sID].cPCode[i] = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[i, "txtPCode"].Value));
                camArray[sID].cPName[i] = Utility.NulltoStr(gcMultiRow1[i, "lblPName"].Value);
                camArray[sID].cPSu[i] = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[i, "txtPSu"].Value));
            }
        }

        ///-------------------------------------------------------------
        /// <summary>
        ///     エラーチェックフレーム </summary>
        /// <param name="sI">
        ///     配列インデックス</param>
        /// <returns>
        ///     true:エラーなし、false:エラー有り</returns>
        ///-------------------------------------------------------------
        private bool errCheckMain(int sI)
        {
            bool rtn = true;

            for (int i = sI; i < camArray.Length; i++)
            {
                if (!errCheck(i))
                {
                    rtn = false;
                    break;
                }
            }

            if (rtn)
            {
                if (sI > 0)
                {
                    for (int i = 0; i < sI; i++)
                    {
                        if (!errCheck(i))
                        {
                            rtn = false;
                            break;
                        }
                    }
                }
            }

            return rtn;
        }

        ///-------------------------------------------------------------
        /// <summary>
        ///     エラーチェックメインルーチン </summary>
        /// <param name="sI">
        ///     配列インデックス</param>
        /// <returns>
        ///     true:エラーなし、false:エラー有り</returns>
        ///-------------------------------------------------------------
        private bool errCheck(int i)
        {
            // 確認チェック
            if (!camArray[i].cStatus)
            {
                imgX = i;
                dataShow(imgX);
                lblMsg.Text = "入力確認が未チェックです";
                checkBox1.Focus();
                return false;
            }

            // お客様コード（お届け先）
            if (camArray[i].cTdkCode == 0)
            {
                imgX = i;
                dataShow(imgX);
                lblMsg.Text = "お客様を選択してください";
                gcMultiRow2.Focus();
                gcMultiRow2.CurrentCellPosition = new CellPosition(gcMultiRow2.CurrentCell.RowIndex, "txtTdkCode");
                return false;
            }

            // 商品・プレゼント入力チェック
            bool itm = false;
            foreach (var t in camArray[i].cCheck)
            {
                if (bool.Parse(t.ToString()))
                {
                    itm = true;
                    break;
                }
            }

            if (!itm)
            {
                imgX = i;
                dataShow(imgX);
                lblMsg.Text = "商品・プレゼントは一つ以上登録してください";
                gcMultiRow1.Focus();
                gcMultiRow1.CurrentCellPosition = new CellPosition(0, "checkBoxCell1");
                return false;
            }

            for (int n = 0; n < 15; n++)
            {
                if (camArray[i].cCheck[n])
                {
                    // 商品数が0以外
                    if (camArray[i].cSSu[n] != global.flgOff)
                    {
                        // コード未登録の商品のとき
                        if (camArray[i].cSCode[n].ToString() != string.Empty &&
                            camArray[i].cSName[n] == string.Empty)
                        {
                            imgX = i;
                            dataShow(imgX);
                            lblMsg.Text = "マスター未登録の商品コードです";
                            gcMultiRow1.Focus();
                            gcMultiRow1.CurrentCellPosition = new CellPosition(n, "txtSCode");
                            return false;
                        }
                    }

                    // 2017/11/19
                    //if (camArray[i].cSCode[n].ToString() != string.Empty &&
                    //    Utility.StrtoInt(camArray[i].cSSu[n].ToString()) == global.flgOff)
                    //{
                    //    imgX = i;
                    //    dataShow(imgX);
                    //    lblMsg.Text = "商品数量が未登録です";
                    //    gcMultiRow1.Focus();
                    //    gcMultiRow1.CurrentCellPosition = new CellPosition(n, "txtSSu");
                    //    return false;
                    //}

                    if (camArray[i].cSCode[n].ToString() != string.Empty &&
                        camArray[i].cPCode[n].ToString() == string.Empty)
                    {
                        imgX = i;
                        dataShow(imgX);
                        lblMsg.Text = "プレゼントが未登録です";
                        gcMultiRow1.Focus();
                        gcMultiRow1.CurrentCellPosition = new CellPosition(n, "txtPCode");
                        return false;
                    }

                    // プレゼント数が０以外
                    if (camArray[i].cPSu[n] != global.flgOff)
                    {
                        // 商品コード未登録のプレゼント品のとき
                        if (camArray[i].cPCode[n].ToString() != string.Empty &&
                            camArray[i].cPName[n] == string.Empty)
                        {
                            imgX = i;
                            dataShow(imgX);
                            lblMsg.Text = "マスター未登録の商品コードです";
                            gcMultiRow1.Focus();
                            gcMultiRow1.CurrentCellPosition = new CellPosition(n, "txtPCode");
                            return false;
                        }
                    }

                    // 2017/11/19
                    //if (camArray[i].cPCode[n].ToString() != string.Empty &&
                    //    Utility.StrtoInt(camArray[i].cPSu[n].ToString()) == global.flgOff)
                    //{
                    //    imgX = i;
                    //    dataShow(imgX);
                    //    lblMsg.Text = "プレゼント数量が未登録です";
                    //    gcMultiRow1.Focus();
                    //    gcMultiRow1.CurrentCellPosition = new CellPosition(n, "txtPSu");
                    //    return false;
                    //}

                    if (camArray[i].cSCode[n].ToString() == string.Empty &&
                        camArray[i].cPCode[n].ToString() != string.Empty)
                    {
                        imgX = i;
                        dataShow(imgX);
                        lblMsg.Text = "対象商品が未登録です";
                        gcMultiRow1.Focus();
                        gcMultiRow1.CurrentCellPosition = new CellPosition(n, "txtSCode");
                        return false;
                    }
                }
            }

            lblMsg.Text = string.Empty;
            return true;
        }

        private void btnDataMake_Click(object sender, EventArgs e)
        {
            // データを配列にセーブ
            setCamDataArray(imgX);

            // エラーチェック
            if (errCheckMain(imgX))
            {
                // 楽商向け受注データを作成
                if (MessageBox.Show("楽商向け受注データを作成しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                // 社内伝票番号日付を入力
                frmDenNumDate frmDen = new frmDenNumDate();
                frmDen.ShowDialog();

                // OCROutputクラス インスタンス生成
                OCROutput kd = new OCROutput(this, dtsC, dts, Conn, _myCode);

                // 楽商発注データ作成
                kd.SaveData(camArray);

                // 画像ファイル退避
                tifFileMove();

                // CSVデータ削除
                allCsvDelete();

                //終了
                MessageBox.Show("終了しました。楽商で発注データ受け入れを行ってください。", "楽商受入データ作成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        ///------------------------------------------------------------------
        /// <summary>
        ///     作業フォルダのCSVデータを全件削除する </summary>
        ///------------------------------------------------------------------
        private void allCsvDelete()
        {
            if (System.IO.Directory.GetFiles(Properties.Settings.Default.camPaignWorhPath, "*.csv").Count() > 0)
            {
                foreach (var t in System.IO.Directory.GetFiles(Properties.Settings.Default.camPaignWorhPath, "*.csv"))
                {
                    System.IO.File.Delete(t);
                }
            }
        }
        
        ///----------------------------------------------------------------------------------
        /// <summary>
        ///     画像ファイル退避処理 </summary>
        ///----------------------------------------------------------------------------------
        private void tifFileMove()
        {
            var s = dts.環境設定.Single(a => a.ID == global.flgOn);

            // 移動先フォルダがあるか？なければ作成する（TIFフォルダ）
            if (!System.IO.Directory.Exists(s.画像保存先パス))
            {
                System.IO.Directory.CreateDirectory(s.画像保存先パス);
            }

            string fromImg = string.Empty;
            string toImg = string.Empty;

            // 発注書データを取得する
            for (int i = 0; i < camArray.Length; i++)
            {
                // キャンペーン画像ファイルパスを取得する
                fromImg = camArray[i].cImgName;

                // 発注書移動先ファイルパス
                string userFolder = s.画像保存先パス + camArray[i].cTdkCode.ToString().PadLeft(6, '0') + "_" + camArray[i].cTdkName;
                
                // お客様フォルダがあるか？なければ作成する
                if (!System.IO.Directory.Exists(userFolder))
                {
                    System.IO.Directory.CreateDirectory(userFolder);
                }

                // 同名ファイルが既に登録済みのときは削除する
                toImg = userFolder + @"\" + System.IO.Path.GetFileName(camArray[i].cImgName);

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

        ///----------------------------------------------------------
        /// <summary>
        ///     指定の要素を配列から消去する </summary>
        /// <param name="x">
        ///     指定インデックス</param>
        /// <returns>
        ///     true:成功、false:失敗</returns>
        ///----------------------------------------------------------
        private bool arrayRemove(int x)
        {
            try
            {
                if (camArray.Length > 1)
                {
                    for (int i = x; i < camArray.Length - 1; i++)
                    {
                        camArray[i] = camArray[i + 1];
                    }

                    Array.Resize(ref camArray, camArray.Length - 1);

                    if (imgX >= camArray.Length)
                    {
                        imgX = camArray.Length - 1;
                    }

                    // データ再表示
                    dataShow(imgX);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "除外処理に失敗しました");
                return false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string msg = "表示中の画像を今回の処理対象外とします。" +
                Environment.NewLine + "※次回のキャンペーン発注処理には当画像は含まれます。" +
                Environment.NewLine + "※当画像がキャンペーン発注書ではないときは「削除」処理を行ってください。" + 
                Environment.NewLine + Environment.NewLine + "処理を実行しますか？";

            if (MessageBox.Show(msg, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            // 対象画像名取得
            string img = camArray[imgX].cImgName;

            if (camArray.Length == 1)
            {
                // 画像をキャンペーンフォルダへ移動する
                System.IO.File.Move(img, Properties.Settings.Default.campaignPath + System.IO.Path.GetFileName(img));
            }
            else
            {
                // 配列を更新
                if (arrayRemove(imgX))
                {
                    // 画像をキャンペーンフォルダへ移動する
                    System.IO.File.Move(img, Properties.Settings.Default.campaignPath + System.IO.Path.GetFileName(img));
                }
            }

            MessageBox.Show("１件の画像を処理対象外としました");

            if (System.IO.Directory.GetFiles(Properties.Settings.Default.camPaignWorhPath, "*.tif").Count() == 0)
            {
                MessageBox.Show("処理可能な画像がありません。終了します。","確認",MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string msg = "表示中の画像を削除します。よろしいですか？";

            if (MessageBox.Show(msg, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            // 対象画像名取得
            string img = camArray[imgX].cImgName;

            if (camArray.Length == 1)
            {
                // 画像＆CSVを削除する
                imgCsvDelete(img);
            }
            else
            {
                // 配列を更新
                if (arrayRemove(imgX))
                {
                    // 画像＆CSVを削除する
                    imgCsvDelete(img);
                }
            }

            MessageBox.Show("画像を削除しました");

            if (System.IO.Directory.GetFiles(Properties.Settings.Default.camPaignWorhPath, "*.tif").Count() == 0)
            {
                MessageBox.Show("処理可能な画像がありません。終了します。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        ///-------------------------------------------------------
        /// <summary>
        ///     画像とCSVデータを削除する </summary>
        /// <param name="sFile">
        ///     画像名</param>
        ///-------------------------------------------------------
        private void imgCsvDelete(string sFile)
        {
            try
            {
                // 画像を削除する
                System.IO.File.Delete(sFile);

                // CSVデータを削除する
                string csvName = System.IO.Path.GetDirectoryName(sFile) + @"\" + System.IO.Path.GetFileNameWithoutExtension(sFile) + ".csv";

                if (System.IO.File.Exists(csvName))
                {
                    System.IO.File.Delete(csvName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
