using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NHBR_OCR.common;
using Leadtools;
using Leadtools.Codecs;
using Leadtools.ImageProcessing;
using Leadtools.WinForms;

namespace NHBR_OCR.OCR
{
    public partial class frmNgRecovery : Form
    {
        public frmNgRecovery()
        {
            InitializeComponent();
        }

        string _InPath = Properties.Settings.Default.ngPath;
        string _OutPath = Properties.Settings.Default.dataPath;
        string _reFaxPath = Properties.Settings.Default.reFaxPath;

        clsNG[] ngf;

        string _img = string.Empty;
        global gl = new global();

        private void frmNgRecovery_Load(object sender, EventArgs e)
        {
            common.Utility.WindowsMaxSize(this, this.Width, this.Height);
            common.Utility.WindowsMinSize(this, this.Width, this.Height);

            // NGリスト
            GetNgList();

            // ボタン
            BtnEnabled_false();
        }

        private void BtnEnabled_false()
        {
            btnPlus.Enabled = false;
            btnMinus.Enabled = false;
            btnLeft.Enabled = false;
            btnRight.Enabled = false;
        }

        private void BtnEnabled_true()
        {
            btnPlus.Enabled = true;
            btnMinus.Enabled = true;
            btnLeft.Enabled = true;
            btnRight.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
        }

        private void frmNgRecovery_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        ///----------------------------------------------------------
        /// <summary>
        ///     ＮＧ画像リストを表示する </summary>
        ///----------------------------------------------------------
        private void GetNgList()
        {
            checkedListBox1.Items.Clear();
            string[] f = System.IO.Directory.GetFiles(_InPath, "*.tif");

            if (f.Length == 0)
            {
                label1.Text = "NG画像はありませんでした";
                return;
            }

            ngf = new clsNG[f.Length];

            int Cnt = 0;

            foreach (string files in System.IO.Directory.GetFiles(_InPath, "*.tif"))
            {
                ngf[Cnt] = new clsNG();
                ngf[Cnt].ngFileName = files;
                string fn = System.IO.Path.GetFileName(files);
                ngf[Cnt].ngRecDate = fn.Substring(0, 4) + "年" + fn.Substring(4, 2) + "月" + fn.Substring(6, 2) + "日" +
                                     fn.Substring(8, 2) + "時" + fn.Substring(10, 2) + "分" + fn.Substring(12, 2) + "秒";

                checkedListBox1.Items.Add(System.IO.Path.GetFileName(ngf[Cnt].ngRecDate));
                Cnt++;
            }

            label1.Text = "NG画像が" + f.Length.ToString() + "件あります";
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItem == null) return;
            else BtnEnabled_true();

            //画像イメージ表示
            ShowImage(ngf[checkedListBox1.SelectedIndex].ngFileName);
        }

        ///----------------------------------------------------------------
        /// <summary>
        ///     伝票画像表示 </summary>
        /// <param name="iX">
        ///     現在の伝票</param>
        /// <param name="tempImgName">
        ///     画像名</param>
        ///----------------------------------------------------------------
        public void ShowImage(string tempImgName)
        {
            //string wrkFileName;

            //修正画面へ組み入れた画像フォームの表示    
            //画像の出力が無い場合は、画像表示をしない。
            if (tempImgName == string.Empty)
            {
                leadImg.Visible = false;
                //global.pblImageFile = string.Empty;
                return;
            }

            //画像ファイルがあるときのみ表示
            //wrkFileName = tempImgName;
            if (System.IO.File.Exists(tempImgName))
            {
                leadImg.Visible = true;

                //画像ロード
                RasterCodecs.Startup();
                RasterCodecs cs = new RasterCodecs();

                // 描画時に使用される速度、品質、およびスタイルを制御します。 
                RasterPaintProperties prop = new RasterPaintProperties();
                prop = RasterPaintProperties.Default;
                prop.PaintDisplayMode = RasterPaintDisplayModeFlags.Resample;
                leadImg.PaintProperties = prop;

                leadImg.Image = cs.Load(tempImgName, 0, CodecsLoadByteOrder.BgrOrGray, 1, 1);

                //画像表示倍率設定
                if (gl.miMdlZoomRate == 0f)
                {
                    if (leadImg.ImageDpiX == 200)
                        leadImg.ScaleFactor *= gl.ZOOM_RATE;    // 200*200 画像のとき
                    else leadImg.ScaleFactor *= gl.ZOOM_RATE;       // 300*300 画像のとき
                }
                else
                {
                    leadImg.ScaleFactor *= gl.miMdlZoomRate;
                }

                //画像のマウスによる移動を可能とする
                leadImg.InteractiveMode = RasterViewerInteractiveMode.Pan;

                ////右へ90°回転させる
                //RotateCommand rc = new RotateCommand();
                //rc.Angle = 90 * 100;
                //rc.FillColor = new RasterColor(255, 255, 255);
                ////rc.Flags = RotateCommandFlags.Bicubic;
                //rc.Flags = RotateCommandFlags.Resize;
                //rc.Run(leadImg.Image);

                // グレースケールに変換
                GrayscaleCommand grayScaleCommand = new GrayscaleCommand();
                grayScaleCommand.BitsPerPixel = 8;
                grayScaleCommand.Run(leadImg.Image);
                leadImg.Refresh();

                cs.Dispose();
                RasterCodecs.Shutdown();
                //global.pblImageFile = wrkFileName;

                // 画像操作ボタン
                btnPlus.Enabled = true;
                btnMinus.Enabled = true;
            }
            else
            {
                //画像ファイルがないとき
                leadImg.Visible = false;
                //global.pblImageFile = string.Empty;

                // 画像操作ボタン
                btnPlus.Enabled = false;
                btnMinus.Enabled = false;
            }
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
        }

        private class clsNG
        {
            public string ngFileName { get; set; }
            public string ngRecDate { get; set; }
        }

        private void button5_Click(object sender, EventArgs e)
        {
        }

        ///--------------------------------------------------------------
        /// <summary>
        ///     ＮＧファイルリカバリ </summary>
        ///--------------------------------------------------------------
        private void NgRecovery()
        {
            if (ngFileCount() == 0)
            {
                MessageBox.Show("画像が選択されていません", "画像未選択", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                if (MessageBox.Show(ngFileCount().ToString() + "件の画像を発注データとしてリカバリします。よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }

            DateTime dt = DateTime.Now;
            string _ID = string.Format("{0:0000}", dt.Year) + string.Format("{0:00}", dt.Month) +
                         string.Format("{0:00}", dt.Day) + string.Format("{0:00}", dt.Hour) +
                         string.Format("{0:00}", dt.Minute) + string.Format("{0:00}", dt.Second);

            // ＮＧファイルリカバリ処理
            int fCnt = 1;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    ngToData(_ID, fCnt, i);
                    fCnt++;
                }
            }

            // 終了メッセージ
            MessageBox.Show(ngFileCount().ToString() + "件の画像を発注データとしてリカバリし受信フォルダへ移動しました", "リカバリー処理完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // ＮＧ画像リスト再表示
            GetNgList();

            // イメージ表示初期化
            leadImg.Image = null;
            BtnEnabled_false();
        }
        
        ///--------------------------------------------------------------
        /// <summary>
        ///     再ＦＡＸフォルダへ指定画像を移動する </summary>
        ///--------------------------------------------------------------
        private void toReFaxDir()
        {
            try
            {
                if (ngFileCount() == 0)
                {
                    MessageBox.Show("画像が選択されていません", "画像未選択", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    if (MessageBox.Show(ngFileCount().ToString() + "件の画像を再ＦＡＸフォルダへ移動します。よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                }

                // 画像ファイル移動処理
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        File.Move(ngf[i].ngFileName, _reFaxPath + System.IO.Path.GetFileName(ngf[i].ngFileName));
                    }
                }

                // 終了メッセージ
                MessageBox.Show(ngFileCount().ToString() + "件の画像を再ＦＡＸフォルダへ移動しました", "処理完了", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // ＮＧ画像リスト再表示
                GetNgList();

                // イメージ表示初期化
                leadImg.Image = null;
                BtnEnabled_false();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        

        ///--------------------------------------------------------------------------
        /// <summary>
        ///     ＣＳＶデータファイル作成・ＮＧ画像→データ画像へ </summary>
        /// <param name="fCnt">
        ///     リカバリファイル番号</param>
        /// <param name="ind">
        ///     リストボックスインデックス</param>
        ///--------------------------------------------------------------------------
        private void ngToData(string _ID, int fCnt, int ind)
        {
            // IDを取得します
            _ID += fCnt.ToString().PadLeft(3, '0');

            // 出力ファイルインスタンス作成
            StreamWriter outFile = new StreamWriter(_OutPath + _ID + ".csv", false, System.Text.Encoding.GetEncoding(932));

            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Clear();

                // ヘッダ情報
                sb.Append("*").Append(",");
                sb.Append(_ID + ".tif").Append(",");    // 画像ファイル名
                sb.Append(string.Empty).Append(",");    // パターンＩＤ
                sb.Append(string.Empty).Append(",");    // お客様番号
                sb.Append("0").Append(",");             // 再fax
                sb.Append(string.Empty).Append(",");    // 発注番号
                sb.Append(string.Empty).Append(",");    // 納品希望・月
                sb.Append(string.Empty).Append(",").Append(Environment.NewLine);    // 納品希望・日

                // 発注数
                for (int i = 0; i < 40; i++)
                {
                    sb.Append(string.Empty).Append(Environment.NewLine);
                }

                sb.Append("0").Append(",");     // キャンペーン申し込み
                sb.Append("0");                 // 備考欄記入有無

                // ＣＳＶファイル作成
                outFile.WriteLine(sb.ToString());

                // 画像ファイル移動
                File.Move(ngf[ind].ngFileName, _OutPath + _ID + ".tif");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ＮＧ画像リカバリ処理", MessageBoxButtons.OK);
            }
            finally
            {
                outFile.Close();
            }
        }

        ///-------------------------------------------------------------
        /// <summary>
        ///     チェックボックス選択数取得 </summary>
        /// <returns>
        ///     選択アイテム数</returns>
        ///-------------------------------------------------------------
        private int ngFileCount()
        {
            return checkedListBox1.CheckedItems.Count;
        }

        private void button7_Click(object sender, EventArgs e)
        {
        }

        ///-------------------------------------------------------------
        /// <summary>
        ///     ＮＧ画像削除処理 </summary>
        ///-------------------------------------------------------------
        private void NgFileDelete()
        {
            if (ngFileCount() == 0)
            {
                MessageBox.Show("画像が選択されていません", "画像未選択", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                if (MessageBox.Show(ngFileCount().ToString() + "件の画像を削除します。よろしいですか？", "削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                    return;
            }

            // ＮＧファイル削除
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    imgDelete(ngf[i].ngFileName);
                }
            }

            // ＮＧ画像リスト再表示
            GetNgList();

            // イメージ表示初期化
            leadImg.Image = null;
            BtnEnabled_false();
        }


        ///-------------------------------------------------------------
        /// <summary>
        ///     ファイル削除 </summary>
        /// <param name="imgPath">
        ///     画像ファイルパス</param>
        ///-------------------------------------------------------------
        private void imgDelete(string imgPath)
        {
            // ファイルを削除する
            if (System.IO.File.Exists(imgPath))
            {
                System.IO.File.Delete(imgPath);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// ＮＧ画像印刷
        /// </summary>
        private void NgImagePrint()
        {
            if (ngFileCount() == 0)
            {
                MessageBox.Show("画像が選択されていません", "画像未選択", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                if (MessageBox.Show(ngFileCount().ToString() + "件の画像を印刷します。よろしいですか？", "印刷確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                    return;
                
                // ＮＧ画像印刷
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        _img = ngf[i].ngFileName;

                        // 印刷実行
                        printDocument1.Print();
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void linkPlus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void linkMinus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Image img;

            img = Image.FromFile(_img);
            e.Graphics.DrawImage(img, 0, 0);
            e.HasMorePages = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (leadImg.ScaleFactor < gl.ZOOM_MAX)
            {
                leadImg.ScaleFactor += gl.ZOOM_STEP;
            }
            gl.miMdlZoomRate = (float)leadImg.ScaleFactor;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (leadImg.ScaleFactor > gl.ZOOM_MIN)
            {
                leadImg.ScaleFactor -= gl.ZOOM_STEP;
            }
            gl.miMdlZoomRate = (float)leadImg.ScaleFactor;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            // 左へ90°回転させる
            RotateCommand rc = new RotateCommand();
            rc.Angle = -90 * 100;
            rc.FillColor = new RasterColor(255, 255, 255);
            //rc.Flags = RotateCommandFlags.Bicubic;
            rc.Flags = RotateCommandFlags.Resize;
            rc.Run(leadImg.Image);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            // 右へ90°回転させる
            RotateCommand rc = new RotateCommand();
            rc.Angle = 90 * 100;
            rc.FillColor = new RasterColor(255, 255, 255);
            //rc.Flags = RotateCommandFlags.Bicubic;
            rc.Flags = RotateCommandFlags.Resize;
            rc.Run(leadImg.Image);
        }

        private void btnPrn_Click(object sender, EventArgs e)
        {
            // ＮＧ画像印刷
            NgImagePrint();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // ＮＧ画像削除処理
            NgFileDelete();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            // ＮＧファイルリカバリ
            NgRecovery();
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            // 再ＦＡＸフォルダへ移動
            toReFaxDir();
        }
    }
}
