using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Leadtools.Codecs;
using Leadtools;
using Leadtools.ImageProcessing;
using Leadtools.ImageProcessing.Core;
using NHBR_SCAN.common;


namespace NHBR_SCAN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            timer1.Tick += new EventHandler(timer1_Tick);
        }

        Timer timer1 = new Timer();

        private void Form1_Load(object sender, EventArgs e)
        {
            // バルーン表示
            notifyIcon1.ShowBalloonTip(500);

            // フォーム最小サイズ
            Utility.WindowsMinSize(this, this.Width, this.Height);

            // データグリッド定義
            GridViewSetting(dataGridView1);

            dataGridView1.Rows.Clear();

            // データ受信～OCR認識
            doOCrTimer();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // データ受信～OCR認識
            timer1.Enabled = false;
            doOCrTimer();
            timer1.Enabled = true;
        }


        #region カラム定義
        string cDate = "col0";
        string cFrom = "col1";
        string cSubject = "col2";
        string cFileName = "col3";
        string cTlcnt = "col8";
        string cIdxcnt = "col9";
        string cScnt = "col4";
        string cEcnt = "col5";
        string cNGcnt = "col6";
        string cNonOcrcnt = "col7";
        string cMessageID = "col10";
        string cMemo = "col11";
        #endregion

        ///-------------------------------------------------------------------
        /// <summary>
        ///     データグリッドビューの定義を行います </summary>
        /// <param name="tempDGV">
        ///     データグリッドビューオブジェクト</param>
        ///-------------------------------------------------------------------
        private void GridViewSetting(DataGridView g)
        {
            try
            {
                g.EnableHeadersVisualStyles = false;
                g.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                g.EnableHeadersVisualStyles = false;

                // 列ヘッダー表示位置指定
                g.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 列ヘッダーフォント指定
                g.ColumnHeadersDefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9, FontStyle.Regular);

                // データフォント指定
                g.DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 10, FontStyle.Regular);

                // 行の高さ
                g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                g.ColumnHeadersHeight = 22;
                g.RowTemplate.Height = 22;

                // 全体の高さ
                g.Height = 507;

                // 奇数行の色
                g.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

                g.Columns.Add(cDate, "受信日時");
                g.Columns.Add(cTlcnt, "OCR認識件数");
                g.Columns.Add(cEcnt, "エラー件数");
                g.Columns.Add(cMemo, "備考");


                g.Columns[cDate].Width = 160;
                g.Columns[cTlcnt].Width = 120;
                g.Columns[cEcnt].Width = 120;
                g.Columns[cMemo].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                g.Columns[cDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                g.Columns[cTlcnt].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                g.Columns[cEcnt].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 行ヘッダを表示しない
                g.RowHeadersVisible = false;

                // 選択モード
                g.SelectionMode = DataGridViewSelectionMode.CellSelect;
                g.MultiSelect = false;

                // 編集不可とする
                g.ReadOnly = true;

                // 追加行表示しない
                g.AllowUserToAddRows = false;

                // データグリッドビューから行削除を禁止する
                g.AllowUserToDeleteRows = false;

                // 手動による列移動の禁止
                g.AllowUserToOrderColumns = false;

                // 列サイズ変更可
                g.AllowUserToResizeColumns = true;

                // 行サイズ変更禁止
                g.AllowUserToResizeRows = false;

                // 行ヘッダーの自動調節
                //tempDGV.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

                //TAB動作
                g.StandardTab = true;

                // 罫線
                g.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
                g.CellBorderStyle = DataGridViewCellBorderStyle.None;
                //g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラーメッセージ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        ///------------------------------------------------------------
        /// <summary>
        ///     タイマー処理 </summary>
        ///------------------------------------------------------------
        private void doOCrTimer()
        {
            notifyIcon1.Visible = false;

            // パス１、２いずれか画像受信が成功したものを優先処理するため以下処理は撤廃：2017/10/22
            // まいと～く受信画像を確認 : 2017/06/22
            //try
            //{
            //    // まいと～く受信フォルダは2つとする：2017/07/29
            //    if (System.IO.Directory.GetFiles(Properties.Settings.Default.myTalkPath, "*.tif").Count() == 0 &&
            //        System.IO.Directory.GetFiles(Properties.Settings.Default.myTalkPath2, "*.tif").Count() == 0)
            //    {
            //        notifyIcon1.Visible = true;
            //        return;
            //    }
            //}
            //catch (Exception)
            //{
            //    // 戻す : 2017/10/22
            //    notifyIcon1.Visible = true;
            //    return;
            //}

            // まいと～く受信画像をSCANフォルダへ移動（パス１） : 2017/06/22
            try
            {
                // まいと～く受信フォルダは2つとする：2017/07/29
                if (System.IO.Directory.GetFiles(Properties.Settings.Default.myTalkPath, "*.tif").Count() > 0)
                {
                    foreach (string files in System.IO.Directory.GetFiles(Properties.Settings.Default.myTalkPath, "*.tif"))
                    {
                        // SCANフォルダにtif画像をMOVE
                        System.IO.File.Move(files, Properties.Settings.Default.scanPath + System.IO.Path.GetFileNameWithoutExtension(files) + "_a.tif");
                    }
                }
            }
            catch (Exception ex)
            {
                // ログ表示 : 2017/10/22
                string msg = Properties.Settings.Default.myTalkPath + "の受信画像を" + Properties.Settings.Default.scanPath + "に移動する処理で" + ex.Message + " 処理は継続されています。";
                logGridView(dataGridView1, 0, 0, msg);
            }

            // まいと～く受信画像をSCANフォルダへ移動（パス２） : 2017/06/22
            try
            {
                // まいと～く受信フォルダは2つとする：2017/07/29
                if (System.IO.Directory.GetFiles(Properties.Settings.Default.myTalkPath2, "*.tif").Count() > 0)
                {
                    foreach (string files in System.IO.Directory.GetFiles(Properties.Settings.Default.myTalkPath2, "*.tif"))
                    {
                        // SCANフォルダにtif画像をMOVE
                        System.IO.File.Move(files, Properties.Settings.Default.scanPath + System.IO.Path.GetFileNameWithoutExtension(files) + "_b.tif");
                    }
                }
            }
            catch (Exception ex)
            {
                // ログ表示 : 2017/10/22
                string msg = Properties.Settings.Default.myTalkPath2 + "の受信画像を" + Properties.Settings.Default.scanPath + "に移動する処理で" + ex.Message + " 処理は継続されています。";
                logGridView(dataGridView1, 0, 0, msg);
            }

            // SCANフォルダから指定フォルダ「FAX8889」へtif画像をコピーする : 2017/06/22
            try
            {
                foreach (string files in System.IO.Directory.GetFiles(Properties.Settings.Default.scanPath, "*.tif"))
                {
                    // SCANフォルダからまいと～くの指定フォルダへtif画像をCOPY
                    System.IO.File.Copy(files, Properties.Settings.Default.myTalkOutPath + System.IO.Path.GetFileName(files));
                }
            }
            catch (Exception ex)
            {
                // ログ表示 : 2017/10/22
                string msg = Properties.Settings.Default.scanPath + "の受信画像を" + Properties.Settings.Default.myTalkOutPath + "にコピーする処理で" + ex.Message + " 処理は継続されています。";
                logGridView(dataGridView1, 0, 0, msg);

                // 書き込みが完了していないので戻す : 2017/10/22
                notifyIcon1.Visible = true;
                return;
            }

            // SCANパスの画像の存在を確認してOCR認識を行う : 2017/10/22
            if (System.IO.Directory.GetFiles(Properties.Settings.Default.scanPath, "*.tif").Count() > 0)
            {
                // ファイル名（日付時間部分）
                string fName = string.Format("{0:0000}", DateTime.Today.Year) +
                        string.Format("{0:00}", DateTime.Today.Month) +
                        string.Format("{0:00}", DateTime.Today.Day) +
                        string.Format("{0:00}", DateTime.Now.Hour) +
                        string.Format("{0:00}", DateTime.Now.Minute) +
                        string.Format("{0:00}", DateTime.Now.Second);

                // マルチTiff画像をシングルtifに分解する(SCANフォルダ → TRAYフォルダ)
                if (MultiTif(Properties.Settings.Default.scanPath, Properties.Settings.Default.trayPath, fName))
                {
                    // 帳票ライブラリV8.0.3によるOCR認識実行
                    string jobname = Properties.Settings.Default.wrHands_Job;
                    wrhs803LibOCR(jobname);

                    // 再FAXフォルダ内のCSVファイルを削除する
                    Utility.FileDelete(Properties.Settings.Default.reFaxPath, "*.csv");
                }
            }

            notifyIcon1.Visible = true;
        }

        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;

            // 終了する
            Application.Exit();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //インターバルセット
            timer1.Interval = Properties.Settings.Default.timerSpan * 1000;    // 秒単位
            timer1.Enabled = true;
        }
        
        ///------------------------------------------------------------------------------
        /// <summary>
        ///     マルチフレームの画像ファイルを頁ごとに分割する </summary>
        /// <param name="InPath">
        ///     画像ファイル入力パス</param>
        /// <param name="outPath">
        ///     分割後出力パス</param>
        /// <returns>
        ///     true:分割を実施, false:分割ファイルなし</returns>
        ///------------------------------------------------------------------------------
        private bool MultiTif(string InPath, string outPath, string fName)
        {
            //スキャン出力画像を確認
            if (System.IO.Directory.GetFiles(InPath, "*.tif").Count() == 0)
            {
                return false;
            }

            // 出力先フォルダがなければ作成する
            if (System.IO.Directory.Exists(outPath) == false)
            {
                System.IO.Directory.CreateDirectory(outPath);
            }

            // 出力先フォルダ内の全てのファイルを削除する（通常ファイルは存在しないが例外処理などで残ってしまった場合に備えて念のため）
            foreach (string files in System.IO.Directory.GetFiles(outPath, "*"))
            {
                System.IO.File.Delete(files);
            }

            RasterCodecs.Startup();
            RasterCodecs cs = new RasterCodecs();

            int _pageCount = 0;
            string fnm = string.Empty;

            //コマンドを準備します。(傾き・ノイズ除去・リサイズ)
            DeskewCommand Dcommand = new DeskewCommand();
            DespeckleCommand Dkcommand = new DespeckleCommand();
            SizeCommand Rcommand = new SizeCommand();

            // マルチTIFを分解して画像ファイルをTRAYフォルダへ保存する
            foreach (string files in System.IO.Directory.GetFiles(InPath, "*.tif"))
            {
                // 画像読み出す
                RasterImage leadImg = cs.Load(files, 0, CodecsLoadByteOrder.BgrOrGray, 1, -1);

                // 頁数を取得
                int _fd_count = leadImg.PageCount;

                // 頁ごとに読み出す
                for (int i = 1; i <= _fd_count; i++)
                {
                    //ページを移動する
                    leadImg.Page = i;

                    // ファイル名設定
                    _pageCount++;
                    fnm = outPath + fName + string.Format("{0:000}", _pageCount) + ".tif";

                    // 2018/06/08 画像補正をしない
                    //画像補正処理　開始 ↓ ****************************
                    //try
                    //{
                    //    //画像の傾きを補正します。
                    //    Dcommand.Flags = DeskewCommandFlags.DeskewImage | DeskewCommandFlags.DoNotFillExposedArea;
                    //    Dcommand.Run(leadImg);
                    //}
                    //catch// (Exception e)
                    //{
                    //}                       //ノイズ除去

                    //try
                    //{
                    //    Dkcommand.Run(leadImg);
                    //}
                    //catch// (Exception e)
                    //{
                    //}

                    ////解像度調整(200*200dpi)
                    //leadImg.XResolution = 200;
                    //leadImg.YResolution = 200;

                    ////A4縦サイズに変換(ピクセル単位)
                    //Rcommand.Width = 1637;
                    //Rcommand.Height = 2322;
                    //try
                    //{
                    //    Rcommand.Run(leadImg);
                    //}
                    //catch// (Exception e)
                    //{
                    //}

                    //画像補正処理　終了↑ ****************************

                    // 画像保存
                    cs.Save(leadImg, fnm, RasterImageFormat.Tif, 0, i, i, 1, CodecsSavePageMode.Insert);
                }
            }

            //LEADTOOLS入出力ライブラリを終了します。
            RasterCodecs.Shutdown();

            // InPathフォルダの全てのtifファイルを削除する
            foreach (var files in System.IO.Directory.GetFiles(InPath, "*.tif"))
            {
                System.IO.File.Delete(files);
            }

            return true;
        }
        
        ///----------------------------------------------------------------
        /// <summary>
        ///     帳票認識ライブラリ V8.0.3 による認識処理実行 </summary>
        ///----------------------------------------------------------------
        private void wrhs803LibOCR(string jobName)
        {
            // ファイル名のタイムスタンプを設定
            string fnm = string.Format("{0:0000}", DateTime.Today.Year) +
                         string.Format("{0:00}", DateTime.Today.Month) +
                         string.Format("{0:00}", DateTime.Today.Day) +
                         string.Format("{0:00}", DateTime.Now.Hour) +
                         string.Format("{0:00}", DateTime.Now.Minute) +
                         string.Format("{0:00}", DateTime.Now.Second);

            int sNum = 0;
            int ret = 0;

            int okCnt = 0;
            int errCnt = 0;
            string msg = "";

            try
            {
                //// オーナーフォームを無効にする
                //this.Enabled = false;

                //// プログレスバーを表示する
                //frmPrg frmP = new frmPrg();
                //frmP.Owner = this;
                //frmP.Show();

                // 処理する画像数を取得
                int t = System.IO.Directory.GetFiles(Properties.Settings.Default.trayPath, "*.tif").Count();

                // 順番に認識処理を実行
                foreach (string files in System.IO.Directory.GetFiles(Properties.Settings.Default.trayPath, "*.tif"))
                {
                    // 画像数カウント
                    sNum++;

                    //// プログレス表示
                    //frmP.Text = "OCR認識中です ... " + sNum.ToString() + "/" + t.ToString();
                    //frmP.progressValue = sNum * 100 / t;
                    //frmP.ProgressStep();

                    // 標準パターンの読み込み
                    ret = FormRecog.OcrPatternLoad(Properties.Settings.Default.ocrPatternLoadPath);

                    // パターン読み込みに成功したとき
                    if (ret > 0)
                    {
                        // 帳票認識ライブラリの制御内容を設定
                        //FormRecog.OcrSetStatus(5, 1);   // 強制終了制御
                        FormRecog.OcrSetStatus(0, 0);

                        // 認識結果出力イメージファイル
                        StringBuilder outimage = new StringBuilder(256);
                        outimage.Append(Properties.Settings.Default.wrOutPath + System.IO.Path.GetFileName(files));

                        // 認識結果出力テキストファイル
                        StringBuilder outtext = new StringBuilder(256);
                        outtext.Append(Properties.Settings.Default.wrOutPath + System.IO.Path.GetFileNameWithoutExtension(files) + ".csv");

                        // 認識結果 構造体
                        FormRecog.FORM_RECOG_DATA dt = new FormRecog.FORM_RECOG_DATA();

                        // 認識処理を開始
                        ret = FormRecog.OcrFormRecogStart(jobName, files, outimage, outtext, ref dt, false, false);

                        // 認識成功のとき
                        if (ret > 0)
                        {
                            // 認識結果のメモリ解放
                            ret = FormRecog.OcrFormStructFree(ref dt);

                            // 認識終了
                            ret = FormRecog.OcrFormRecogEnd();
                            
                            // 出力先フォルダ
                            string rPath = Properties.Settings.Default.dataPath;

                            // 出力されたイメージファイルとテキストファイルのリネーム処理を行います
                            // READフォルダ → DATAフォルダ
                            string inCsvFile = Properties.Settings.Default.wrOutPath +
                                               Properties.Settings.Default.wrReaderOutFile;
                            string newFileName = rPath + fnm + sNum.ToString().PadLeft(3, '0');
                            wrhOutFileRename(inCsvFile, newFileName);

                            okCnt++;
                        }
                        else
                        {
                            //MessageBox.Show("OCR認識開始に失敗しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            msg = "OCR認識に失敗しました";

                            //エラー画像はerrorフォルダに画像をCOPY
                            System.IO.File.Copy(files, Properties.Settings.Default.ocrErrPath + System.IO.Path.GetFileName(files));

                            errCnt++;
                        }
                    }
                    else
                    {
                        MessageBox.Show("OCR標準パターンの読み込みに失敗しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        msg = "OCR標準パターンの読み込みに失敗しました";

                        //エラー画像はerrorフォルダに画像をCOPY
                        System.IO.File.Copy(files, Properties.Settings.Default.ocrErrPath + System.IO.Path.GetFileName(files));
                        errCnt++;
                    }
                }

                //// いったんオーナーをアクティブにする
                //this.Activate();

                //// 進行状況ダイアログを閉じる
                //frmP.Close();

                //// オーナーのフォームを有効に戻す
                //this.Enabled = true;

                //// 終了表示
                //MessageBox.Show(sNum.ToString() + "件のOCR認識処理を行いました", "終了", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // TRAYフォルダの全てのtifファイルを削除します
                foreach (var files in System.IO.Directory.GetFiles(Properties.Settings.Default.trayPath, "*.tif"))
                {
                    System.IO.File.Delete(files);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FAX受信処理");
            }
            finally
            {
                // ログ表示
                logGridView(dataGridView1, okCnt, errCnt, msg);
            }
        }
        
        /// -----------------------------------------------------------------
        /// <summary>
        ///     CSVファイルと画像ファイルの名前を日付スタンプに変更する </summary>
        /// <param name="readFilePath">
        ///     入力CSVファイル名(フルパス）</param>
        /// <param name="newFnm">
        ///     新ファイル名（フルパス・但し拡張子なし）</param>
        /// -----------------------------------------------------------------
        private void wrhOutFileRename(string readFilePath, string newFnm)
        {
            string imgName = string.Empty;      // 画像ファイル名
            string[] stArrayData;               // CSVファイルを１行単位で格納する配列
            string inFilePath = string.Empty;   // ＯＣＲ認識モードごとの入力ファイル名

            // CSVデータの存在を確認します
            if (!System.IO.File.Exists(readFilePath)) return;

            // StreamReader の新しいインスタンスを生成する
            //入力ファイル
            System.IO.StreamReader inFile = new System.IO.StreamReader(readFilePath, Encoding.Default);

            // 読み込んだ結果をすべて格納するための変数を宣言する
            string stResult = string.Empty;
            string stBuffer;

            // 読み込みできる文字がなくなるまで繰り返す
            while (inFile.Peek() >= 0)
            {
                // ファイルを 1 行ずつ読み込む
                stBuffer = inFile.ReadLine();

                // カンマ区切りで分割して配列に格納する
                stArrayData = stBuffer.Split(',');

                //先頭に「*」があったらヘッダー情報
                if ((stArrayData[0] == "*"))
                {
                    //文字列バッファをクリア
                    stResult = string.Empty;

                    // 文字列再構成（画像ファイル名を変更する）
                    stBuffer = string.Empty;
                    for (int i = 0; i < stArrayData.Length; i++)
                    {
                        if (stBuffer != string.Empty)
                        {
                            stBuffer += ",";
                        }

                        // 画像ファイル名を変更する
                        if (i == 1)
                        {
                            stArrayData[i] = System.IO.Path.GetFileName(newFnm) + ".tif"; // 画像ファイル名を変更
                        }

                        // フィールド結合
                        string sta = stArrayData[i].Trim();
                        stBuffer += sta;
                    }

                    // 2018/08/02 コメント化
                    //// 再ＦＡＸか確認
                    //if (stArrayData[4] == global.FLGON)
                    //{
                    //    // 書き出しファイル名を再FAXフォルダにする
                    //    newFnm = Properties.Settings.Default.reFaxPath + System.IO.Path.GetFileName(newFnm);
                    //}
                }

                // 読み込んだものを追加で格納する
                stResult += (stBuffer + Environment.NewLine);
            }

            // CSVファイル書き出し
            System.IO.StreamWriter outFile = new System.IO.StreamWriter(newFnm + ".csv",
                                                    false, System.Text.Encoding.GetEncoding(932));
            outFile.Write(stResult);

            // 出力ファイルを閉じる
            outFile.Close();

            // 入力ファイルを閉じる
            inFile.Close();

            // 入力ファイル削除 : "txtout.csv"
            string inPath = System.IO.Path.GetDirectoryName(readFilePath);
            Utility.FileDelete(inPath, Properties.Settings.Default.wrReaderOutFile);

            // 画像ファイルをリネーム
            System.IO.File.Move(Properties.Settings.Default.wrOutPath + "WRH00001.tif", newFnm + ".tif");
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;               // フォームの表示
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal; // 最小化をやめる
            }
            //this.notifyIcon1.Visible = false;  // Notifyアイコン非表示
            this.Activate();                   // フォームをアクティブにする
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                e.Cancel = true; // フォームが閉じるのをキャンセル
                this.Visible = false; // フォームの非表示
            }
        }

        ///------------------------------------------------------------
        /// <summary>
        ///     データグリッドに受信監視ログを表示する </summary>
        /// <param name="md">
        ///     メールデータ</param>
        ///------------------------------------------------------------
        private void logGridView(DataGridView dg, int cCnt, int eCnt, string msg)
        {
            dg.Rows.Add();
            dg[cDate, dg.RowCount - 1].Value = DateTime.Now.ToString();
            dg[cTlcnt, dg.RowCount - 1].Value = cCnt.ToString();
            dg[cEcnt, dg.RowCount - 1].Value = eCnt.ToString();
            dg[cMemo, dg.RowCount - 1].Value = msg;

            dg.CurrentCell = null;
        }
    }
}
