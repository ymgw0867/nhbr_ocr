using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NHBR_OCR.common
{
    class clsCamData
    {
        public string cImgName;     // 画像名
        public int cTdkCode;        // お客様コード
        public string cTdkName;     // お客様名
        public string cHaNum;       // 発注番号
        public string cCamName;     // キャンペーン番号
        public string cSDate;       // 開始日付
        public string cEDate;       // 終了日付
        public int cPreTdkCode;     // プレゼントお届先コード
        public string cPreTdkName;     // プレゼントお届先名

        public bool[] cCheck = new bool[15];        // チェックボックス
        public int[] cSCode = new int[15];          // 商品コード
        public string[] cSName = new string[15];    // 商品名
        public int[] cSSu = new int[15];            // 商品数
        public int[] cPCode = new int[15];          // プレゼント商品コード
        public string[] cPName = new string[15];    // プレゼント名
        public int[] cPSu = new int[15];            // プレゼント数

        public bool cStatus;    // データ入力確認
        public string cMemo;    // メモ欄

        ///----------------------------------------------------------
        /// <summary>
        ///     キャンペーンデータ配列をCSVデータ出力する </summary>
        /// <param name="camArray">
        ///     対象となる配列</param>
        ///----------------------------------------------------------
        public void putCamCsv(clsCamData camArray)
        {
            string[] csvArray = new string[16];

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(camArray.cImgName).Append(",");
                sb.Append(camArray.cTdkCode).Append(",");
                sb.Append(camArray.cTdkName).Append(",");
                sb.Append(camArray.cHaNum).Append(",");
                sb.Append(camArray.cPreTdkCode).Append(",");    // 2018/02/20
                sb.Append(camArray.cPreTdkName).Append(",");    // 2018/02/20
                sb.Append(camArray.cCamName).Append(",");
                sb.Append(camArray.cSDate).Append(",");
                sb.Append(camArray.cEDate).Append(",");
                sb.Append(camArray.cStatus.ToString()).Append(",");
                sb.Append(camArray.cMemo);

                csvArray[0] = sb.ToString();

                for (int ix = 0; ix < 15; ix++)
                {
                    sb.Clear();
                    sb.Append(camArray.cCheck[ix].ToString()).Append(",");
                    sb.Append(camArray.cSCode[ix].ToString()).Append(",");
                    sb.Append(camArray.cSName[ix]).Append(",");
                    sb.Append(camArray.cSSu[ix].ToString()).Append(",");
                    sb.Append(camArray.cPCode[ix].ToString()).Append(",");
                    sb.Append(camArray.cPName[ix]).Append(",");
                    sb.Append(camArray.cPSu[ix].ToString());

                    csvArray[ix + 1] = sb.ToString();
                }

                // CSVファイル出力
                if (csvArray != null)
                {
                    txtFileWrite(Properties.Settings.Default.camPaignWorhPath + System.IO.Path.GetFileNameWithoutExtension(camArray.cImgName) + ".csv", csvArray);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        ///----------------------------------------------------------------------------
        /// <summary>
        ///     CSVファイルを出力する</summary>
        /// <param name="outFilePath">
        ///     出力するフォルダ</param>
        /// <param name="arrayData">
        ///     書き込む配列データ</param>
        ///----------------------------------------------------------------------------
        private void txtFileWrite(string sPath, string[] arrayData)
        {
            //// 付加文字列（タイムスタンプ）
            //string newFileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') +
            //                        DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') +
            //                        DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');

            // ファイル名
            string outFileName = sPath;

            // 同名ファイルが存在するとき削除する
            if (System.IO.File.Exists(outFileName))
            {
                System.IO.File.Delete(outFileName);
            }
            
            // テキストファイル出力
            System.IO.File.WriteAllLines(outFileName, arrayData, System.Text.Encoding.GetEncoding(932));
        }
    }
}
