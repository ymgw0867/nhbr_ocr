using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHBR_OCR.common;

namespace NHBR_OCR.OCR
{
    public partial class frmFaxSelect : Form
    {
        public frmFaxSelect()
        {
            InitializeComponent();

            fAdp.Fill(dtsC.FAX注文書);
            hAdp.Fill(dtsC.保留注文書);
        }
        
        int ALLCnt = 0;

        NHBR_CLIDataSet dtsC = new NHBR_CLIDataSet();
        NHBR_CLIDataSetTableAdapters.FAX注文書TableAdapter fAdp = new NHBR_CLIDataSetTableAdapters.FAX注文書TableAdapter();
        NHBR_CLIDataSetTableAdapters.保留注文書TableAdapter hAdp = new NHBR_CLIDataSetTableAdapters.保留注文書TableAdapter();

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '\t')
                e.Handled = true;
        }

        private void frmFaxSelect_Load(object sender, EventArgs e)
        {
            ALLCnt = System.IO.Directory.GetFiles(Properties.Settings.Default.dataPath, "*.csv").Count();

            if (ALLCnt < 1)
            {
                //this.Close();
                //return;

                textBox1.Enabled = false;
            }
            else
            {
                textBox1.Enabled = true;
            }

            //件数表示
            lblFaxCnt.Text = ALLCnt.ToString();

            //label5.Text = ALLCnt + " 枚の発注書があります。処理する枚数を指定してください。";
            //lblCnt.Text = "現在、" + ALLCnt + " 枚の発注書があります。処理する枚数を指定してください。";

            foreach (var t in dtsC.保留注文書.OrderBy(a => a.更新年月日))
            {
                checkedListBox1.Items.Add(t.更新年月日.ToShortDateString() + " " + t.更新年月日.Hour + ":" + t.更新年月日.Minute + ":" + t.更新年月日.Second + ", " + t.ID);                
            }

            lblDataCnt.Text = dtsC.FAX注文書.Count().ToString();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (Utility.StrtoInt(textBox1.Text) > ALLCnt)
            {
                textBox1.Text = ALLCnt.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            myCnt = 0;
            myBool = false;
            Close();
        }

        public int myCnt { get; set; }
        public bool myBool { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = getDataCount();

            if (n == 0)
            {
                MessageBox.Show("処理するデータがありません", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                if (MessageBox.Show(n + "件の発注書を処理します。よろしいですか。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            
            myBool = true;

            myCnt = Utility.StrtoInt(textBox1.Text);

            // 受信したＦＡＸ発注書を取り込む
            if (myCnt > 0)
            {
                getFaxData(Utility.StrtoInt(textBox1.Text));
            }

            // 保留データをＦＡＸ発注書データに戻す
            holdToData();

            Close();
        }

        ///-------------------------------------------------------
        /// <summary>
        ///     保留データをＦＡＸ発注書データに戻す </summary>
        ///-------------------------------------------------------
        private void holdToData()
        {
            if (checkedListBox1.SelectedItems.Count == 0)
            {
                return;
            }

            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                string s = checkedListBox1.CheckedItems[i].ToString();
                string[] st = s.Split(',');

                setHoldToData(st[1].Trim());
            }
        }
        
        ///----------------------------------------------------------
        /// <summary>
        ///     保留処理 </summary>
        /// <param name="iX">
        ///     データインデックス</param>
        ///----------------------------------------------------------
        private void setHoldToData(string iX)
        {
            try
            {
                var t = dtsC.保留注文書.Single(a => a.ID == iX);

                NHBR_CLIDataSet.FAX注文書Row hr = dtsC.FAX注文書.NewFAX注文書Row();

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

                // ＦＡＸ発注書追加処理
                dtsC.FAX注文書.AddFAX注文書Row(hr);
                fAdp.Update(dtsC.FAX注文書);

                // 保留データ削除
                t.Delete();
                hAdp.Update(dtsC.保留注文書);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        ///-----------------------------------------------------------
        /// <summary>
        ///     処理データ件数を取得する </summary>
        /// <returns>
        ///     データ件数 </returns>
        ///-----------------------------------------------------------
        private int getDataCount()
        {
            int dCnt = 0;

            dCnt += Utility.StrtoInt(textBox1.Text);
            dCnt += checkedListBox1.CheckedItems.Count;
            dCnt += Utility.StrtoInt(lblDataCnt.Text);

            return dCnt;
        }
        
        ///-------------------------------------------------------
        /// <summary>
        ///     ＦＡＸ発注書を自分のフォルダへ取り込む </summary>
        /// <param name="mCnt">
        ///     取り込む枚数</param>
        ///-------------------------------------------------------
        private void getFaxData(int mCnt)
        {
            int MoveFileCnt = 0;

            Boolean fMoveFlg = false;

            //if (MessageBox.Show(textBox1.Text + "枚の受信ＦＡＸ発注書を取り込みます。よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
            //{
            //    return;
            //}
 
            foreach (string files in System.IO.Directory.GetFiles(Properties.Settings.Default.dataPath, "*.csv"))
            {
                fMoveFlg = false;

                try
                {
                    //*****CSV******
                    //移動先に同じ名前のファイルが存在する場合、既にあるファイルを削除する
                    string csvFname = Properties.Settings.Default.mydataPath + System.IO.Path.GetFileName(files);
                   
                    if (System.IO.File.Exists(csvFname))
                    {
                        System.IO.File.Delete(csvFname);
                    }

                    System.IO.File.Move(files, csvFname);

                    fMoveFlg = true;
                }
                catch
                {
                }

                if (fMoveFlg) 
                {
                    //*****TIF******
                    //移動先に同じ名前のファイルが存在する場合、既にあるファイルを削除する
                    string tifName = Properties.Settings.Default.mydataPath + System.IO.Path.GetFileName(files.Replace("csv", "tif"));
                    
                    if (System.IO.File.Exists(tifName))
                    {
                        System.IO.File.Delete(tifName);
                    }

                    System.IO.File.Move(files.Replace("csv", "tif"), tifName);

                    MoveFileCnt++;
                }

                // 取り込み枚数に達したら終了
                if (MoveFileCnt >= mCnt)
                {
                    break;
                }
            }

            //移動したファイル数を保持する
            if (MoveFileCnt != mCnt)    //指定枚数と実枚数が違う場合は、エラーがあったことを表示し、枚数を伝える
            {
                MessageBox.Show("取り込み中にエラーが発生したため、取り込み枚数は" + MoveFileCnt.ToString() + "枚になります。");
            }
        }
    }
}
