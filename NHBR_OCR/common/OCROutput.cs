using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;

namespace NHBR_OCR.common
{
    ///------------------------------------------------------------------
    /// <summary>
    ///     楽商用受入データ作成クラス </summary>     
    ///------------------------------------------------------------------
    class OCROutput
    {
        // 親フォーム
        Form _preForm;
        NHBR_CLIDataSet.FAX注文書DataTable _hTbl;
        NHBRDataSet.パターンIDDataTable _pTbl;
        NHBRDataSetTableAdapters.環境設定TableAdapter cnfAdp = new NHBRDataSetTableAdapters.環境設定TableAdapter();
        
        private const string TXTFILENAME = "FAX受注";

        NHBR_CLIDataSet _dtsC = new NHBR_CLIDataSet();
        NHBRDataSet dts = new NHBRDataSet();

        // 就業奉行汎用データヘッダ項目
        const string H1 = @"""EBAS001""";   // 社員番号
        const string H2 = @"""LTLT001""";   // 日付
        const string H3 = @"""LTLT002""";   // 勤務回
        const string H4 = @"""LTLT003""";   // 勤務体系コード
        const string H5 = @"""LTLT004""";   // 事由コード１
        const string H6 = @"""LTLT005""";   // 事由コード２
        const string H7 = @"""LTLT006""";   // 事由コード３
        const string H8 = @"""LTDT001""";   // 出勤時刻
        const string H9 = @"""LTDT002""";   // 退出時刻

        DateTime _numDate = DateTime.Today;
        int dtSeq = 0;
        
        ///--------------------------------------------------------------------------
        /// <summary>
        ///     楽商用受入データ作成クラスコンストラクタ</summary>
        /// <param name="preFrm">
        ///     親フォーム</param>
        /// <param name="dtsC">
        ///     NHBR_CLIDataSet</param>
        /// <param name="_dts">
        ///     NHBRDataSet:データセットオブジェクト</param>
        /// <param name="Conn">
        ///     Oracle.ManagedDataAccess.Client.OracleConnection : 接続情報</param>
        /// <param name="myCode">
        ///     担当者コード</param>
        ///--------------------------------------------------------------------------
        public OCROutput(Form preFrm, NHBR_CLIDataSet dtsC, NHBRDataSet _dts, Oracle.ManagedDataAccess.Client.OracleConnection Conn, string myCode)
        {
            _preForm = preFrm;
            _dtsC = dtsC;
            dts = _dts;
            _hTbl = dtsC.FAX注文書;
            _pTbl = _dts.パターンID;
            _Conn = Conn;
            _myCode = myCode;

            cnfAdp.Fill(dts.環境設定);
        }

        Oracle.ManagedDataAccess.Client.OracleConnection _Conn;
        string _myCode;

        ///--------------------------------------------------------------------------------------
        /// <summary>
        ///     楽商受入データ作成</summary>
        ///--------------------------------------------------------------------------------------     
        public void SaveData()
        {
            // 楽商データ作成先パス取得
            var ss = dts.環境設定.Single(a => a.ID == 1);
            string okPath = ss.受け渡しデータ作成パス;

            // 事前に入力された日付を採用する 2017/10/23
            _numDate = ss.当日日付;
            dtSeq = ss.連番;

            //if (ss.Is当日日付Null())
            //{
            //    _numDate = DateTime.Today;
            //    dtSeq = 0;
            //}
            //else if (ss.当日日付 != DateTime.Today)
            //{
            //    _numDate = DateTime.Today;
            //    dtSeq = 0;
            //}
            //else
            //{
            //    _numDate = ss.当日日付;
            //    dtSeq = ss.連番;
            //}

            #region 出力配列
            string[] arrayCsv = null;     // 出力配列
            #endregion

            #region 出力件数変数
            int sCnt = 0;   // 社員出力件数
            #endregion

            StringBuilder sb = new StringBuilder();
            global gl = new global();

            // 出力先フォルダがあるか？なければ作成する
            if (!System.IO.Directory.Exists(okPath))
            {
                System.IO.Directory.CreateDirectory(okPath);
            }

            DateTime dt = DateTime.Now;
            string ts = dt.Year + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0') + dt.Hour.ToString().PadLeft(2, '0') + dt.Minute.ToString().PadLeft(2, '0') + dt.Second.ToString().PadLeft(2, '0');
            
            try
            {
                //オーナーフォームを無効にする
                _preForm.Enabled = false;

                //プログレスバーを表示する
                frmPrg frmP = new frmPrg();
                frmP.Owner = _preForm;
                frmP.Show();

                int rCnt = 1;
                
                // 発注書データ取得
                var s = _hTbl.OrderBy(a => a.ID);
                foreach (var r in s)
                {
                    // プログレスバー表示
                    frmP.Text = "楽商データ作成中です・・・" + rCnt.ToString() + "/" + s.Count().ToString();
                    frmP.progressValue = rCnt * 100 / s.Count();
                    frmP.ProgressStep();

                    // 共通データを作成する
                    string uCSV = getHeadCsv(r, ts, rCnt, ss);

                    // 商品パターン・注文数クラスインスタンス作成
                    hinSuu[] hS = new hinSuu[40];
                    setHinSuuArray(ref hS, r);

                    int iX = 1;

                    for (int i = 0; i < hS.Length; i++)
                    {
                        if (Utility.StrtoInt(hS[i].Suu) != 0)
                        {
                            sb.Clear();
                            sb.Append(iX.ToString()).Append("\t");
                            sb.Append(hS[i].hin.ToString().PadLeft(8, '0')).Append("\t");
                            sb.Append(getHinName(hS[i].hin.ToString().PadLeft(8, '0'))).Append("\t");
                            sb.Append(hS[i].Suu).Append("\t");
                            sb.Append(DateTime.Now);

                            // 配列にデータを格納します
                            sCnt++;
                            Array.Resize(ref arrayCsv, sCnt);
                            arrayCsv[sCnt - 1] = uCSV + sb.ToString();

                            iX++;
                        }
                    }

                    rCnt++;
                }

                // CSVファイル出力
                if (arrayCsv != null)
                {
                    txtFileWrite(okPath, arrayCsv);
                }

                // いったんオーナーをアクティブにする
                _preForm.Activate();

                // 進行状況ダイアログを閉じる
                frmP.Close();

                // オーナーのフォームを有効に戻す
                _preForm.Enabled = true;

                // 当日連番を更新
                //ss.当日日付 = _numDate;   // 2017/10/23
                ss.連番 = dtSeq;
                cnfAdp.Update(dts.環境設定);
            }
            catch (Exception e)
            {
                MessageBox.Show("楽商データ作成中" + Environment.NewLine + e.Message, "エラー", MessageBoxButtons.OK);
            }
            finally
            {
                //if (OutData.sCom.Connection.State == ConnectionState.Open) OutData.sCom.Connection.Close();

            }
        }


        ///--------------------------------------------------------------------------------------
        /// <summary>
        ///     楽商受入データ作成：キャンペーン発注書</summary>
        ///--------------------------------------------------------------------------------------     
        public void SaveData(clsCamData [] camArray)
        {
            // 楽商データ作成先パス取得
            var ss = dts.環境設定.Single(a => a.ID == 1);

            string okPath = ss.受け渡しデータ作成パス;

            // 事前に入力された日付を採用する 2017/10/23
            _numDate = ss.当日日付;
            dtSeq = ss.連番;

            //if (ss.Is当日日付Null())
            //{
            //    _numDate = DateTime.Today;
            //    dtSeq = 0;
            //}
            //else if (ss.当日日付 != DateTime.Today)
            //{
            //    _numDate = DateTime.Today;
            //    dtSeq = 0;
            //}
            //else
            //{
            //    _numDate = ss.当日日付;
            //    dtSeq = ss.連番;
            //}

            #region 出力配列
            string[] arrayCsv = null;     // 出力配列
            #endregion

            #region 出力件数変数
            int sCnt = 0;   // 社員出力件数
            #endregion

            StringBuilder sb = new StringBuilder();
            global gl = new global();

            // 出力先フォルダがあるか？なければ作成する
            if (!System.IO.Directory.Exists(okPath))
            {
                System.IO.Directory.CreateDirectory(okPath);
            }

            DateTime dt = DateTime.Now;
            string ts = dt.Year + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0') + dt.Hour.ToString().PadLeft(2, '0') + dt.Minute.ToString().PadLeft(2, '0') + dt.Second.ToString().PadLeft(2, '0');

            try
            {
                //オーナーフォームを無効にする
                _preForm.Enabled = false;

                //プログレスバーを表示する
                frmPrg frmP = new frmPrg();
                frmP.Owner = _preForm;
                frmP.Show();

                int rCnt = 1;

                // キャンペーンデータ配列取得
                for (int n = 0; n < camArray.Length; n++)
                {
                    // プログレスバー表示
                    frmP.Text = "楽商データ作成中です・・・" + n + "/" + camArray.Length;
                    frmP.progressValue = n * 100 / camArray.Length;
                    frmP.ProgressStep();

                    // 社内伝票番号連番インクリメント
                    dtSeq++;
                                        
                    // ０が商品、１がプレゼント
                    for (int sp = 0; sp < 2; sp++)
                    {
                        int r = 0;　// 行番号

                        // 共通データを作成する
                        string uCSV = getHeadCsv(camArray, n, ts, rCnt, ss, sp);
                        
                        for (int iX = 0; iX < 15; iX++)
                        {
                            if (camArray[n].cCheck[iX])
                            {
                                // 2017/11/19
                                //r++;
                                //sb.Clear();
                                //sb.Append(r.ToString()).Append("\t");

                                if (sp == 0)
                                {
                                    if (camArray[n].cSSu[iX] > 0) // 数量有りを登録対象にする 2017/11/19
                                    {
                                        r++;
                                        sb.Clear();
                                        sb.Append(r.ToString()).Append("\t");

                                        sb.Append(camArray[n].cSCode[iX].ToString().PadLeft(8, '0')).Append("\t");
                                        sb.Append(camArray[n].cSName[iX].PadLeft(8, '0')).Append("\t");
                                        sb.Append(camArray[n].cSSu[iX].ToString()).Append("\t");

                                        // 2017.11.19
                                        sb.Append(DateTime.Now);

                                        // 配列にデータを格納します: 2017.11.19
                                        sCnt++;
                                        Array.Resize(ref arrayCsv, sCnt);
                                        arrayCsv[sCnt - 1] = uCSV + sb.ToString();
                                    }
                                }
                                else
                                {
                                    if (camArray[n].cPSu[iX] > 0) // 数量有りを登録対象にする 2017/11/19
                                    {
                                        r++;
                                        sb.Clear();
                                        sb.Append(r.ToString()).Append("\t");

                                        sb.Append(camArray[n].cPCode[iX].ToString().PadLeft(8, '0')).Append("\t");
                                        sb.Append(camArray[n].cPName[iX].PadLeft(8, '0')).Append("\t");
                                        sb.Append(camArray[n].cPSu[iX].ToString()).Append("\t");

                                        // 2017.11.19
                                        sb.Append(DateTime.Now);

                                        // 配列にデータを格納します: 2017.11.19
                                        sCnt++;
                                        Array.Resize(ref arrayCsv, sCnt);
                                        arrayCsv[sCnt - 1] = uCSV + sb.ToString();
                                    }
                                }

                                // 2017.11.19
                                //sb.Append(DateTime.Now);

                                //// 配列にデータを格納します
                                //sCnt++;
                                //Array.Resize(ref arrayCsv, sCnt);
                                //arrayCsv[sCnt - 1] = uCSV + sb.ToString();
                            }
                        }

                        rCnt++;
                    }
                }

                // 楽商受入CSVファイル出力
                if (arrayCsv != null)
                {
                    txtFileWrite(okPath, arrayCsv);
                }

                // いったんオーナーをアクティブにする
                _preForm.Activate();

                // 進行状況ダイアログを閉じる
                frmP.Close();

                // オーナーのフォームを有効に戻す
                _preForm.Enabled = true;

                // 当日連番を更新
                //ss.当日日付 = _numDate;   // 2017/10/23
                ss.連番 = dtSeq;
                cnfAdp.Update(dts.環境設定);
            }
            catch (Exception e)
            {
                MessageBox.Show("楽商データ作成中" + Environment.NewLine + e.Message, "エラー", MessageBoxButtons.OK);
            }
            finally
            {
                //if (OutData.sCom.Connection.State == ConnectionState.Open) OutData.sCom.Connection.Close();

            }
        }

        ///--------------------------------------------------------------------------
        /// <summary>
        ///     商品パターン・注文数配列クラスのインスタンス生成 </summary>
        /// <param name="hS">
        ///     発注商品・数量クラス配列</param>
        /// <param name="r">
        ///     NHBR_CLIDataSet.FAX注文書Row</param>
        ///--------------------------------------------------------------------------
        private void setHinSuuArray(ref hinSuu[] hS, NHBR_CLIDataSet.FAX注文書Row r)
        {
            // 商品パターン・注文数配列クラスのインスタンス生成
            for (int i = 0; i < hS.Length; i++)
            {
                hS[i] = new hinSuu();
            }

            hS[0].Suu = r.注文数1;
            hS[1].Suu = r.注文数2;
            hS[2].Suu = r.注文数3;
            hS[3].Suu = r.注文数4;
            hS[4].Suu = r.注文数5;
            hS[5].Suu = r.注文数6;
            hS[6].Suu = r.注文数7;
            hS[7].Suu = r.注文数8;
            hS[8].Suu = r.注文数9;
            hS[9].Suu = r.注文数10;
            hS[10].Suu = r.注文数11;
            hS[11].Suu = r.注文数12;
            hS[12].Suu = r.注文数13;
            hS[13].Suu = r.注文数14;
            hS[14].Suu = r.注文数15;
            hS[15].Suu = r.注文数16;
            hS[16].Suu = r.注文数17;
            hS[17].Suu = r.注文数18;
            hS[18].Suu = r.注文数19;
            hS[19].Suu = r.注文数20;
            hS[20].Suu = r.注文数21;
            hS[21].Suu = r.注文数22;
            hS[22].Suu = r.注文数23;
            hS[23].Suu = r.注文数24;
            hS[24].Suu = r.注文数25;
            hS[25].Suu = r.注文数26;
            hS[26].Suu = r.注文数27;
            hS[27].Suu = r.注文数28;
            hS[28].Suu = r.注文数29;
            hS[29].Suu = r.注文数30;

            hS[30].Suu = r.追加注文数1;
            hS[31].Suu = r.追加注文数2;
            hS[32].Suu = r.追加注文数3;
            hS[33].Suu = r.追加注文数4;
            hS[34].Suu = r.追加注文数5;
            hS[35].Suu = r.追加注文数6;
            hS[36].Suu = r.追加注文数7;
            hS[37].Suu = r.追加注文数8;
            hS[38].Suu = r.追加注文数9;
            hS[39].Suu = r.追加注文数10;

            // 2017/08/23 : パターンIDで発注のとき
            if (r.パターンID != global.flgOff)
            {
                var s = _pTbl.Single(a => a.届先番号 == r.届先番号 && a.連番 == r.パターンID);

                hS[0].hin = s.商品1;
                hS[1].hin = s.商品2;
                hS[2].hin = s.商品3;
                hS[3].hin = s.商品4;
                hS[4].hin = s.商品5;
                hS[5].hin = s.商品6;
                hS[6].hin = s.商品7;
                hS[7].hin = s.商品8;
                hS[8].hin = s.商品9;
                hS[9].hin = s.商品10;
                hS[10].hin = s.商品11;
                hS[11].hin = s.商品12;
                hS[12].hin = s.商品13;
                hS[13].hin = s.商品14;
                hS[14].hin = s.商品15;
                hS[15].hin = s.商品16;
                hS[16].hin = s.商品17;
                hS[17].hin = s.商品18;
                hS[18].hin = s.商品19;
                hS[19].hin = s.商品20;
                hS[20].hin = s.商品21;
                hS[21].hin = s.商品22;
                hS[22].hin = s.商品23;
                hS[23].hin = s.商品24;
                hS[24].hin = s.商品25;
                hS[25].hin = s.商品26;
                hS[26].hin = s.商品27;
                hS[27].hin = s.商品28;
                hS[28].hin = s.商品29;
                hS[29].hin = s.商品30;
            }
            else
            {
                hS[0].hin = Utility.StrtoInt(r.商品コード1);
                hS[1].hin = Utility.StrtoInt(r.商品コード2);
                hS[2].hin = Utility.StrtoInt(r.商品コード3);
                hS[3].hin = Utility.StrtoInt(r.商品コード4);
                hS[4].hin = Utility.StrtoInt(r.商品コード5);
                hS[5].hin = Utility.StrtoInt(r.商品コード6);
                hS[6].hin = Utility.StrtoInt(r.商品コード7);
                hS[7].hin = Utility.StrtoInt(r.商品コード8);
                hS[8].hin = Utility.StrtoInt(r.商品コード9);
                hS[9].hin = Utility.StrtoInt(r.商品コード10);
                hS[10].hin = Utility.StrtoInt(r.商品コード11);
                hS[11].hin = Utility.StrtoInt(r.商品コード12);
                hS[12].hin = Utility.StrtoInt(r.商品コード13);
                hS[13].hin = Utility.StrtoInt(r.商品コード14);
                hS[14].hin = Utility.StrtoInt(r.商品コード15);
                hS[15].hin = Utility.StrtoInt(r.商品コード16);
                hS[16].hin = Utility.StrtoInt(r.商品コード17);
                hS[17].hin = Utility.StrtoInt(r.商品コード18);
                hS[18].hin = Utility.StrtoInt(r.商品コード19);
                hS[19].hin = Utility.StrtoInt(r.商品コード20);
                hS[20].hin = Utility.StrtoInt(r.商品コード21);
                hS[21].hin = Utility.StrtoInt(r.商品コード22);
                hS[22].hin = Utility.StrtoInt(r.商品コード23);
                hS[23].hin = Utility.StrtoInt(r.商品コード24);
                hS[24].hin = Utility.StrtoInt(r.商品コード25);
                hS[25].hin = Utility.StrtoInt(r.商品コード26);
                hS[26].hin = Utility.StrtoInt(r.商品コード27);
                hS[27].hin = Utility.StrtoInt(r.商品コード28);
                hS[28].hin = Utility.StrtoInt(r.商品コード29);
                hS[29].hin = Utility.StrtoInt(r.商品コード30);
            }

            hS[30].hin = Utility.StrtoInt(r.追加注文商品コード1);
            hS[31].hin = Utility.StrtoInt(r.追加注文商品コード2);
            hS[32].hin = Utility.StrtoInt(r.追加注文商品コード3);
            hS[33].hin = Utility.StrtoInt(r.追加注文商品コード4);
            hS[34].hin = Utility.StrtoInt(r.追加注文商品コード5);
            hS[35].hin = Utility.StrtoInt(r.追加注文商品コード6);
            hS[36].hin = Utility.StrtoInt(r.追加注文商品コード7);
            hS[37].hin = Utility.StrtoInt(r.追加注文商品コード8);
            hS[38].hin = Utility.StrtoInt(r.追加注文商品コード9);
            hS[39].hin = Utility.StrtoInt(r.追加注文商品コード10);
        }
        
        ///-----------------------------------------------------------------------
        /// <summary>
        ///     楽商マスターより商品名を取得する </summary>
        /// <param name="hinCode">
        ///     商品コード</param>
        /// <returns>
        ///     商品名</returns>
        ///-----------------------------------------------------------------------
        private string getHinName(string hinCode)
        {
            string val = string.Empty;

            string strSQL = "select SYO_ID, SYO_NAME, SYO_IRI_KESU, SYO_TANI from RAKUSYO_FAXOCR.V_SYOHIN WHERE SYO_ID = '" + hinCode + "'";
            OracleCommand Cmd = new OracleCommand(strSQL, _Conn);
            OracleDataReader dR = Cmd.ExecuteReader();

            while (dR.Read())
            {
                val = dR["SYO_NAME"].ToString().Trim();
            }
            
            dR.Dispose();
            Cmd.Dispose();

            return val;
        }



        ///-----------------------------------------------------------
        /// <summary>
        ///     発注書から受入れデータ文字列を作成する </summary>
        /// <param name="r">
        ///     NHBR_CLIDataSet.FAX注文書Row </param>
        /// <param name="ts">
        ///     タイムスタンプ</param>
        /// <param name="sNum">
        ///     連番</param>
        /// <param name="sss">
        ///     NHBRDataSet.環境設定Row</param>
        /// <returns>
        ///     受入れデータ文字列</returns>
        ///-----------------------------------------------------------
        private string getHeadCsv(NHBR_CLIDataSet.FAX注文書Row r, string ts, int sNum, NHBRDataSet.環境設定Row sss)
        {
            string hDate = string.Empty;
            StringBuilder sb = new StringBuilder();

            sb.Clear();

            string sTel = string.Empty;
            string sJyu = string.Empty;
            dtSeq++;

            sb.Append(ts + sNum.ToString().PadLeft(3, '0')).Append("\t");   // シーケンス№ 
            sb.Append(DateTime.Today.ToShortDateString()).Append("\t");     // 受注日
            sb.Append(r.届先番号.ToString().PadLeft(6, '0')).Append("\t");  // 届先番号（お客様番号）

            // 届先名
            string sName = Utility.getNouhinName(r.届先番号.ToString().PadLeft(6, '0'), out sTel, out sJyu);
            sb.Append(sName).Append("\t");

            // 発注番号 : 発注番号なしのときは値なしとする 2017/11/30
            if (r.発注番号 == string.Empty)
            {
                sb.Append(string.Empty).Append("\t");
            }
            else
            {
                sb.Append(r.発注番号.ToString().PadLeft(8, '0')).Append("\t");
            }

            // 発注番号２ : 伝票番号日付は当日ではなく入力値を採用　2017/10/23
            //sb.Append(DateTime.Today.Day.ToString().PadLeft(2, '0') + dtSeq.ToString().PadLeft(3, '0')).Append("\t");
            //sb.Append(_numDate.Day.ToString().PadLeft(2, '0') + dtSeq.ToString().PadLeft(3, '0')).Append("\t");
            // 先頭に「８」を付加 : 2017/11/19
            sb.Append("8" + _numDate.Day.ToString().PadLeft(2, '0') + dtSeq.ToString().PadLeft(3, '0')).Append("\t");

            // 納品希望日
            sb.Append(getNouhinKibouDate(r.納品希望月, r.納品希望日)).Append("\t");

            // 2018/08/03 メモから"REFAX"文字列を除去
            string ss = r.メモ.Replace("\r", "").Replace("\n", "").Replace(global.REFAX, "");
            sb.Append(ss).Append("\t");   // メモ

            // エラー無による割引適用
            if (r.エラー有無 == global.flgOff)
            {
                // エラーなし：割引適用
                sb.Append(global.FLGON).Append("\t");   // エラー無の割引
            }
            else
            {
                // エラーあり：割引適用なし
                sb.Append(global.FLGOFF).Append("\t"); 
            }

            sb.Append(_myCode).Append("\t");   // 入力担当者コード

            // 移動先フォルダ
            // 発注書画像移動先ファイルパス
            string userFolder = sss.画像保存先パス + r.届先番号.ToString().PadLeft(6, '0') + "_" + sName;
            sb.Append(userFolder + @"\" + r.画像名).Append("\t");
            
            // null処理を追加 2018/03/28
            // グループＡ出荷基準判定
            if (r.Is出荷基準ANull())
            {
                sb.Append("2").Append("\t");
            }
            else
            {
                sb.Append(r.出荷基準A).Append("\t");
            }

            // グループB出荷基準判定
            if (r.Is出荷基準BNull())
            {
                sb.Append("2").Append("\t");
            }
            else
            {
                sb.Append(r.出荷基準B).Append("\t");
            }

            // グループC出荷基準判定
            if (r.Is出荷基準CNull())
            {
                sb.Append("2").Append("\t");
            }
            else
            {
                sb.Append(r.出荷基準C).Append("\t");
            }

            // グループD出荷基準判定
            if (r.Is出荷基準DNull())
            {
                sb.Append("2").Append("\t");
            }
            else
            {
                sb.Append(r.出荷基準D).Append("\t");
            }

            // グループE出荷基準判定
            if (r.Is出荷基準ENull())
            {
                sb.Append("2").Append("\t");
            }
            else
            {
                sb.Append(r.出荷基準E).Append("\t");
            }

            // グループF出荷基準判定
            if (r.Is出荷基準FNull())
            {
                sb.Append("2").Append("\t");
            }
            else
            {
                sb.Append(r.出荷基準F).Append("\t");
            }

            // グループG出荷基準判定
            if (r.Is出荷基準GNull())
            {
                sb.Append("2").Append("\t");
            }
            else
            {
                sb.Append(r.出荷基準G).Append("\t");
            }

            //sb.Append(r.出荷基準A).Append("\t");   // グループＡ出荷基準判定
            //sb.Append(r.出荷基準B).Append("\t");   // グループＢ出荷基準判定
            //sb.Append(r.出荷基準C).Append("\t");   // グループＣ出荷基準判定
            //sb.Append(r.出荷基準D).Append("\t");   // グループＤ出荷基準判定
            //sb.Append(r.出荷基準E).Append("\t");   // グループＥ出荷基準判定
            //sb.Append(r.出荷基準F).Append("\t");   // グループＦ出荷基準判定
            //sb.Append(r.出荷基準G).Append("\t");   // グループＧ出荷基準判定       

            return sb.ToString();
        }

        ///-----------------------------------------------------------
        /// <summary>
        ///     キャンペーン発注書から受入れデータ文字列を作成する </summary>
        /// <param name="camArray">
        ///     キャンペーン発注書配列</param>
        /// <param name="iX">
        ///     配列インデックス </param>
        /// <param name="ts">
        ///     タイムスタンプ</param>
        /// <param name="sNum">
        ///     連番</param>
        /// <param name="sss">
        ///     NHBRDataSet.環境設定Row</param>
        /// <returns>
        ///     受入れデータ文字列</returns>
        ///-----------------------------------------------------------
        private string getHeadCsv(clsCamData [] camArray, int iX, string ts, int sNum, NHBRDataSet.環境設定Row sss, int sp)
        {
            string hDate = string.Empty;
            StringBuilder sb = new StringBuilder();

            sb.Clear();

            string sTel = string.Empty;
            string sJyu = string.Empty;
            //dtSeq++;

            sb.Append(ts + sNum.ToString().PadLeft(3, '0')).Append("\t");   // シーケンス№ 
            sb.Append(DateTime.Today.ToShortDateString()).Append("\t");     // 受注日

            // 届先番号（お客様番号）・届先名
            if (sp == 0)
            {
                // 商品
                sb.Append(camArray[iX].cTdkCode.ToString().PadLeft(6, '0')).Append("\t");                
                sb.Append(camArray[iX].cTdkName).Append("\t");
            }
            else
            {
                // プレゼント
                //sb.Append("000008").Append("\t");
                //sb.Append("キャンペーンプレゼント").Append("\t");

                // 2018/02/20 プレゼントお届先を指定
                sb.Append(camArray[iX].cPreTdkCode.ToString().PadLeft(6, '0')).Append("\t");
                sb.Append(camArray[iX].cPreTdkName).Append("\t");
            }

            // 発注番号 : 2017/11/30
            //sb.Append(camArray[iX].cHaNum.PadLeft(6, '0')).Append("\t");

            // 発注番号 : 発注番号なしのときは値なしとする 2017/11/30
            if (camArray[iX].cHaNum == string.Empty)
            {
                sb.Append(string.Empty).Append("\t");
            }
            else
            {
                sb.Append(camArray[iX].cHaNum.PadLeft(8, '0')).Append("\t");
            }

            // 発注番号２
            //sb.Append(DateTime.Today.Day.ToString().PadLeft(2, '0') + dtSeq.ToString().PadLeft(3, '0')).Append("\t");
            //sb.Append(_numDate.Day.ToString().PadLeft(2, '0') + dtSeq.ToString().PadLeft(3, '0')).Append("\t");
            
            // 先頭に「８」を付加 : 2017/11/19
            sb.Append("8" + _numDate.Day.ToString().PadLeft(2, '0') + dtSeq.ToString().PadLeft(3, '0')).Append("\t");

            // 納品希望日
            sb.Append(string.Empty).Append("\t");

            string ss = camArray[iX].cMemo.Replace("\r", "").Replace("\n", "");
            sb.Append(ss).Append("\t");   // メモ

            // エラー無による割引適用：割引適用なし
            sb.Append(global.FLGOFF).Append("\t");

            // 入力担当者コード
            sb.Append(_myCode).Append("\t");   

            // 移動先フォルダ
            // 発注書画像移動先ファイルパス
            string userFolder = sss.画像保存先パス + camArray[iX].cTdkCode.ToString().PadLeft(6, '0') + "_" + camArray[iX].cTdkName;
            sb.Append(userFolder + @"\" + System.IO.Path.GetFileName(camArray[iX].cImgName)).Append("\t");

            sb.Append(global.FLGOFF).Append("\t");   // グループＡ出荷基準判定
            sb.Append(global.FLGOFF).Append("\t");   // グループＢ出荷基準判定
            sb.Append(global.FLGOFF).Append("\t");   // グループＣ出荷基準判定
            sb.Append(global.FLGOFF).Append("\t");   // グループＤ出荷基準判定
            sb.Append(global.FLGOFF).Append("\t");   // グループＥ出荷基準判定
            sb.Append(global.FLGOFF).Append("\t");   // グループＦ出荷基準判定
            sb.Append(global.FLGOFF).Append("\t");   // グループＧ出荷基準判定       

            return sb.ToString();
        }

        ///--------------------------------------------------------------
        /// <summary>
        ///     納品希望日を取得する </summary>
        /// <param name="mm">
        ///     納品希望・月</param>
        /// <param name="dd">
        ///     納品希望・日</param>
        /// <returns>
        ///     年を含んだ納品希望日文字列</returns>
        ///--------------------------------------------------------------
        private string getNouhinKibouDate(string mm, string dd)
        {
            int y = DateTime.Today.Year;
            int m = DateTime.Today.Month;
            string ymd;

            if (mm == string.Empty && dd == string.Empty)
            {
                return string.Empty;
            }
            
            if (Utility.StrtoInt(mm) < m)
            {
                // 希望月が翌年のとき
                ymd = (y + 1) + "/" + mm.PadLeft(2, '0') + "/" + dd.PadLeft(2, '0');
            }
            else
            {
                // 希望月が当年のとき
                ymd = y + "/" + mm.PadLeft(2, '0') + "/" + dd.PadLeft(2, '0');
            }

            return ymd;
        }


        ///----------------------------------------------------------------------------
        /// <summary>
        ///     配列にテキストデータをセットする </summary>
        /// <param name="array">
        ///     配列</param>
        /// <param name="cnt">
        ///     拡張する配列サイズ</param>
        /// <param name="txtData">
        ///     セットする文字列</param>
        ///----------------------------------------------------------------------------
        private void txtArraySet(string [] array, int cnt, string txtData)
        {
            Array.Resize(ref array, cnt);   // 配列のサイズ拡張
            array[cnt - 1] = txtData;       // 文字列のセット
        }
        
        ///----------------------------------------------------------------------------
        /// <summary>
        ///     テキストファイルを出力する</summary>
        /// <param name="outFilePath">
        ///     出力するフォルダ</param>
        /// <param name="arrayData">
        ///     書き込む配列データ</param>
        ///----------------------------------------------------------------------------
        private void txtFileWrite(string sPath, string [] arrayData)
        {
            // 付加文字列（タイムスタンプ）
            string newFileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                                    DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                                    DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0'); 

            // ファイル名
            string outFileName = sPath + TXTFILENAME + newFileName + ".txt";
            
            // テキストファイル出力
            //File.WriteAllLines(outFileName, arrayData, System.Text.Encoding.GetEncoding(932));
            File.WriteAllLines(outFileName, arrayData, System.Text.Encoding.Unicode);   // ℓ：環境依存文字がShift-jisでは"?"と表示されてしまうため、Unicodeとした
        }
    }

    ///----------------------------------------------------------
    /// <summary>
    ///     発注商品・数量クラス </summary>
    ///----------------------------------------------------------
    class hinSuu
    {
        public int hin { get; set; }
        public string Suu { get; set; }
    }
}
