using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHBR_OCR.common;
using Oracle.ManagedDataAccess.Client;

namespace NHBR_OCR.OCR
{
    partial class frmCorrect
    {
        clsSKijun[] orderArray = new clsSKijun[40];

        private void kijunCheckMain()
        {
            setOrderArray();

            kijunCheck();

            // グループＡ判定結果表示
            if (grp_A_Suu == 0)
            {
                lblGrpA.Text = "－";
            }
            else
            {
                if (grp_A)
                {
                    lblGrpA.Text = "◯";
                }
                else
                {
                    lblGrpA.Text = "✕";
                }
            }

            // グループＢ判定結果表示
            if (grp_B_Suu == 0)
            {
                lblGrpB.Text = "－";
            }
            else
            {
                if (grp_B)
                {
                    lblGrpB.Text = "◯";
                }
                else
                {
                    lblGrpB.Text = "✕";
                }
            }

            // グループＣ判定結果表示
            if (grp_C_Suu == 0)
            {
                lblGrpC.Text = "－";
            }
            else
            {
                if (grp_C)
                {
                    lblGrpC.Text = "◯";
                }
                else
                {
                    lblGrpC.Text = "✕";
                }
            }

            // グループＤ判定結果表示
            if (grp_D_Suu == 0)
            {
                lblGrpD.Text = "－";
            }
            else
            {
                if (grp_D)
                {
                    lblGrpD.Text = "◯";
                }
                else
                {
                    lblGrpD.Text = "✕";
                }
            }

            // グループＥ判定結果表示
            if (grp_E_Suu == 0)
            {
                lblGrpE.Text = "－";
            }
            else
            {
                if (grp_E)
                {
                    lblGrpE.Text = "◯";
                }
                else
                {
                    lblGrpE.Text = "✕";
                }
            }

            // グループＦ判定結果表示
            if (grp_F_Suu == 0)
            {
                lblGrpF.Text = "－";
            }
            else
            {
                if (grp_F)
                {
                    lblGrpF.Text = "◯";
                }
                else
                {
                    lblGrpF.Text = "✕";
                }
            }

            // グループＧ判定結果表示
            if (grp_G_Suu == 0)
            {
                lblGrpG.Text = "－";
            }
            else
            {
                if (grp_G)
                {
                    lblGrpG.Text = "◯";
                }
                else
                {
                    lblGrpG.Text = "✕";
                }
            }

            //string msg;
            //msg = "A:" + grp_A + Environment.NewLine;
            //msg += "B:" + grp_B + Environment.NewLine;
            //msg += "C:" + grp_C + Environment.NewLine;
            //msg += "D:" + grp_D + Environment.NewLine;
            //msg += "E:" + grp_E + Environment.NewLine;
            //msg += "F:" + grp_F + Environment.NewLine;
            //msg += "G:" + grp_G + Environment.NewLine;

            //MessageBox.Show(msg);
        }

        // 出荷基準判定
        int grp_A_Suu = 0;
        bool grp_A1 = true;
        bool grp_A2 = true;
        bool grp_A = true;

        int grp_B_Suu = 0;
        bool grp_B1 = true;
        bool grp_B2 = true;
        bool grp_B = true;

        int grp_C_Suu = 0;
        bool grp_C1 = true;
        bool grp_C2 = true;
        bool grp_C = true;

        int grp_D_Suu = 0;
        bool grp_D = true;

        int grp_E_Suu = 0;
        bool grp_E1 = true;
        bool grp_E2 = true;
        bool grp_E = true;

        int grp_F_Suu = 0;
        bool grp_F1 = true;
        bool grp_F2 = true;
        bool grp_F = true;

        int grp_G_Suu = 0;
        bool grp_G = true;

        private void kijunStatusDefault()
        {
            grp_A_Suu = 0;
            grp_A1 = true;
            grp_A2 = true;
            grp_A = true;

            grp_B_Suu = 0;
            grp_B1 = true;
            grp_B2 = true;
            grp_B = true;
            
            grp_C_Suu = 0;
            grp_C1 = true;
            grp_C2 = true;
            grp_C = true;

            grp_D_Suu = 0;
            grp_D = true;

            grp_E_Suu = 0;
            grp_E1 = true;
            grp_E2 = true;
            grp_E = true;

            grp_F_Suu = 0;
            grp_F1 = true;
            grp_F2 = true;
            grp_F = true;

            grp_G_Suu = 0;
            grp_G = true;
        }

        private void kijunCheck()
        {
            // 合計数クリア
            kijunStatusDefault();

            // 出荷基準設定データ取得
            NHBRDataSet.出荷基準設定Row sr = dts.出荷基準設定.Single(a => a.ID == global.configKEY);

            for (int i = 0; i < orderArray.Length; i++)
            {
                // 発注数無記入（0含む）は対象外
                if (Utility.StrtoInt(orderArray[i].sSuu) == global.flgOff)
                {
                    continue;
                }

                // 商品コード記入なしは対象外
                if (orderArray[i].sHinCode == string.Empty)
                {
                    continue;
                }

                // 発注明細毎にグループ判定
                getHinGroup(sr, ref orderArray[i]);
            }


            // 出荷基準判定
            chkShukkakijun(sr, orderArray);
        }

        private void chkShukkakijun(NHBRDataSet.出荷基準設定Row sr, clsSKijun [] kArray)
        {
            for (int i = 0; i < kArray.Length; i++)
            {
                int cSuu = Utility.StrtoInt(kArray[i].sSuu);

                // グループＡ
                if (kArray[i].sGroup == global.SH_GROUP_A)
                {
                    itemCheck_A(cSuu, sr.小瓶注文単位数A);
                }

                // グループＢ
                if (kArray[i].sGroup == global.SH_GROUP_B)
                {
                    itemCheck_B(cSuu, sr.中瓶注文単位数B);
                }

                // グループＣ
                if (kArray[i].sGroup == global.SH_GROUP_C)
                {
                    itemCheck_C(cSuu, sr.大瓶注文単位数C);
                }

                // グループＤ
                if (kArray[i].sGroup == global.SH_GROUP_D)
                {
                    itemCheck_D(cSuu, sr.特瓶注文単位数D);
                }

                // グループＥ 
                if (kArray[i].sGroup == global.SH_GROUP_E)
                {
                    itemCheck_E(cSuu, sr.缶注文単位数E);
                }
                
                // グループＦ
                if (kArray[i].sGroup == global.SH_GROUP_F)
                {
                    itemCheck_F(cSuu, sr.缶注文単位数F);
                }

                // グループＧ
                if (kArray[i].sGroup == global.SH_GROUP_G)
                {
                    itemCheck_G(cSuu, sr.樽注文単位数G);
                }
            }

            // グループＡの判定結果
            if (!grp_A1)
            {
                // 発注書の各注文書が偶数ではない
                grp_A = false;
            }
            else
            {
                // 合計注文数が設定値の倍数であること
                if (grp_A_Suu % Utility.StrtoInt(sr.小瓶合計単位数A) != 0)
                {
                    grp_A = false;
                }
            }

            // グループＢの判定結果
            if (!grp_B1)
            {
                // 発注書の各注文書が偶数ではない
                grp_B = false;
            }
            else
            {
                // 合計注文数が設定値の倍数であること
                if (grp_B_Suu % Utility.StrtoInt(sr.中瓶合計単位数1B) != 0 &&
                    grp_B_Suu % Utility.StrtoInt(sr.中瓶合計単位数2B) != 0)
                {
                    grp_B = false;
                }
            }

            // グループCの判定結果
            if (!grp_C1)
            {
                // 発注書の各注文書が偶数ではない
                grp_C = false;
            }
            else
            {
                // 合計注文数が設定値の倍数であること
                if (grp_C_Suu % Utility.StrtoInt(sr.大瓶合計単位数C) != 0)
                {
                    grp_C = false;
                }
            }

            // グループＤの判定結果
            // グループ別で判定済

            // グループＥの判定結果
            if (!grp_E1)
            {
                // 発注書の各注文書が偶数ではない
                grp_E = false;
            }
            else
            {
                // 合計注文数が設定値の倍数であること
                if (grp_E_Suu % Utility.StrtoInt(sr.缶合計単位数E) != 0)
                {
                    grp_E = false;
                }
            }

            // グループＦの判定結果
            if (!grp_F1)
            {
                // 発注書の各注文書が偶数ではない
                grp_F = false;
            }
            else
            {
                // 合計注文数が設定値の倍数であること
                if (grp_F_Suu % Utility.StrtoInt(sr.缶合計単位数F) != 0)
                {
                    grp_F = false;
                }
            }

            // グループＧの判定結果
            // グループ別で判定済

        }

        private void itemCheck_A(int cSuu, string kigu)
        {
            // グループＡ
            if (kigu == global.SH_GUUSU)
            {
                if ((cSuu % 2) != 0)
                {
                    grp_A1 = false;
                }
            }
            else if (kigu == global.SH_KISU)
            {
                if ((cSuu % 2) != 1)
                {
                    grp_A1 = false;
                }
            }

            grp_A_Suu += cSuu;
        }

        private void itemCheck_B(int cSuu, string kigu)
        {
            // グループＢ
            if (kigu == global.SH_GUUSU)
            {
                if ((cSuu % 2) != 0)
                {
                    grp_B1 = false;
                }
            }
            else if (kigu == global.SH_KISU)
            {
                if ((cSuu % 2) != 1)
                {
                    grp_B1 = false;
                }
            }

            grp_B_Suu += cSuu;
        }

        private void itemCheck_C(int cSuu, string kigu)
        {
            // グループＣ
            if (kigu == global.SH_GUUSU)
            {
                if ((cSuu % 2) != 0)
                {
                    grp_C1 = false;
                }
            }
            else if (kigu == global.SH_KISU)
            {
                if ((cSuu % 2) != 1)
                {
                    grp_C1 = false;
                }
            }

            grp_C_Suu += cSuu;
        }

        private void itemCheck_D(int cSuu, string kigu)
        {
            // グループＤ
            if (kigu == global.SH_GUUSU)
            {
                if ((cSuu % 2) != 0)
                {
                    grp_D = false;
                }
            }
            else if (kigu == global.SH_KISU)
            {
                if ((cSuu % 2) != 1)
                {
                    grp_D = false;
                }
            }

            grp_D_Suu += cSuu;
        }

        private void itemCheck_E(int cSuu, string sTani)
        {
            // グループＥ
            if ((cSuu % Utility.StrtoInt(sTani)) != 0)
            {
                grp_E = false;
            }

            grp_E_Suu += cSuu;
        }

        private void itemCheck_F(int cSuu, string sTani)
        {
            // グループＦ
            if ((cSuu % Utility.StrtoInt(sTani)) != 0)
            {
                grp_F = false;
            }

            grp_F_Suu += cSuu;
        }

        private void itemCheck_G(int cSuu, string sTani)
        {
            // グループＧ
            if (cSuu < Utility.StrtoInt(sTani))
            {
                grp_G = false;
            }

            grp_G_Suu += cSuu;
        }

        ///------------------------------------------------------------------------
        /// <summary>
        ///     商品コードから出荷基準グループを判定する </summary>
        /// <param name="sr">
        ///     NHBRDataSet.出荷基準設定Row</param>
        /// <param name="kArray">
        ///     clsSKijunクラスオブジェクト</param>
        ///------------------------------------------------------------------------
        private void getHinGroup(NHBRDataSet.出荷基準設定Row sr, ref clsSKijun kArray)
        {
            string hinCode = Utility.NulltoStr(kArray.sHinCode).PadLeft(8, '0');

            string strSQL = "select SYO_ID, SYO_NAME, SYO_KANSAN, SYO_EXT11, SYO_EXT13 from RAKUSYO_FAXOCR.V_SYOHIN WHERE SYO_ID = '" + hinCode + "'";
            OracleCommand Cmd = new OracleCommand(strSQL, Conn);
            OracleDataReader dR = Cmd.ExecuteReader();

            decimal kansan = 0;
            string kbt = string.Empty;
            int dosuu = 0;
            
            while (dR.Read())
            {
                kansan = 0;
                kbt = string.Empty;
                dosuu = 0;
            
                // 内容量（換算値）取得
                kansan = (decimal)Utility.StrtoDouble(dR["SYO_KANSAN"].ToString());
                kansan *= 1000;

                // 缶・瓶・樽
                kbt = dR["SYO_EXT13"].ToString();

                // 度数
                dosuu = (int)Utility.StrtoDouble(dR["SYO_EXT11"].ToString());
            }

            dR.Dispose();
            Cmd.Dispose();


            // Ａグループ
            if (kbt == global.SH_PACK_BIN)
            {
                if (kansan <= sr.小瓶条件A)
                {
                    kArray.sGroup = global.SH_GROUP_A;
                    return;
                }
            }

            // Ｂグループ
            if (kbt == global.SH_PACK_BIN)
            {
                if (kansan >= sr.中瓶条件1B && kansan <= sr.中瓶条件2B)
                {
                    kArray.sGroup = global.SH_GROUP_B;
                    return;
                }
            }

            // Ｃグループ
            if (kArray.sHinCode == sr.大瓶条件1C.PadLeft(8, '0') || kArray.sHinCode == sr.大瓶条件2C.PadLeft(8, '0') ||
                kArray.sHinCode == sr.大瓶条件3C.PadLeft(8, '0') || kArray.sHinCode == sr.大瓶条件4C.PadLeft(8, '0') ||
                kArray.sHinCode == sr.大瓶条件5C.PadLeft(8, '0') || kArray.sHinCode == sr.大瓶条件6C.PadLeft(8, '0') ||
                kArray.sHinCode == sr.大瓶条件7C.PadLeft(8, '0') || kArray.sHinCode == sr.大瓶条件8C.PadLeft(8, '0') ||
                kArray.sHinCode == sr.大瓶条件9C.PadLeft(8, '0') || kArray.sHinCode == sr.大瓶条件10C.PadLeft(8, '0') ||
                kArray.sHinCode == sr.大瓶条件11C.PadLeft(8, '0') || kArray.sHinCode == sr.大瓶条件12C.PadLeft(8, '0') ||
                kArray.sHinCode == sr.大瓶条件13C.PadLeft(8, '0') || kArray.sHinCode == sr.大瓶条件14C.PadLeft(8, '0') ||
                kArray.sHinCode == sr.大瓶条件15C.PadLeft(8, '0') || kArray.sHinCode == sr.大瓶条件16C.PadLeft(8, '0') ||
                kArray.sHinCode == sr.大瓶条件17C.PadLeft(8, '0') || kArray.sHinCode == sr.大瓶条件18C.PadLeft(8, '0') ||
                kArray.sHinCode == sr.大瓶条件19C.PadLeft(8, '0') || kArray.sHinCode == sr.大瓶条件20C.PadLeft(8, '0'))
            {
                kArray.sGroup = global.SH_GROUP_C;
                return;
            }

            // Ｄグループ
            if (kArray.sHinCode == sr.特瓶商品1D.PadLeft(8, '0') || kArray.sHinCode == sr.特瓶商品2D.PadLeft(8, '0') ||
                kArray.sHinCode == sr.特瓶商品3D.PadLeft(8, '0') || kArray.sHinCode == sr.特瓶商品4D.PadLeft(8, '0') ||
                kArray.sHinCode == sr.特瓶商品5D.PadLeft(8, '0') || kArray.sHinCode == sr.特瓶商品6D.PadLeft(8, '0') ||
                kArray.sHinCode == sr.特瓶商品7D.PadLeft(8, '0') || kArray.sHinCode == sr.特瓶商品8D.PadLeft(8, '0') ||
                kArray.sHinCode == sr.特瓶商品9D.PadLeft(8, '0') || kArray.sHinCode == sr.特瓶商品10D.PadLeft(8, '0') ||
                kArray.sHinCode == sr.特瓶商品11D.PadLeft(8, '0') || kArray.sHinCode == sr.特瓶商品12D.PadLeft(8, '0') ||
                kArray.sHinCode == sr.特瓶商品13D.PadLeft(8, '0') || kArray.sHinCode == sr.特瓶商品14D.PadLeft(8, '0') ||
                kArray.sHinCode == sr.特瓶商品15D.PadLeft(8, '0') || kArray.sHinCode == sr.特瓶商品16D.PadLeft(8, '0') ||
                kArray.sHinCode == sr.特瓶商品17D.PadLeft(8, '0') || kArray.sHinCode == sr.特瓶商品18D.PadLeft(8, '0') ||
                kArray.sHinCode == sr.特瓶商品19D.PadLeft(8, '0') || kArray.sHinCode == sr.特瓶商品20D.PadLeft(8, '0'))
            {
                kArray.sGroup = global.SH_GROUP_D;
                return;
            }

            // Ｅグループ
            if (kbt == global.SH_PACK_KAN)
            {
                if (dosuu != 0)
                {
                    kArray.sGroup = global.SH_GROUP_E;
                    return;
                }
            }

            // Ｆグループ
            if (kbt == global.SH_PACK_KAN)
            {
                if (dosuu == 0)
                {
                    kArray.sGroup = global.SH_GROUP_F;
                    return;
                }
            }

            // Ｇグループ
            if (kbt == global.SH_PACK_TARU)
            {
                kArray.sGroup = global.SH_GROUP_G;
                return;
            }
        }



        private void setOrderArray()
        {
            // パターン発注欄
            for (int i = 0; i < 15; i++)
            {
                orderArray[i] = new clsSKijun();
                orderArray[i].sHinCode = Utility.NulltoStr(gcMultiRow2[i, "txtHinCode"].Value);
                orderArray[i].sSuu = Utility.NulltoStr(gcMultiRow2[i, "txtSuu"].Value);
                orderArray[i].sGroup = string.Empty;

                orderArray[i + 15] = new clsSKijun();
                orderArray[i + 15].sHinCode = Utility.NulltoStr(gcMultiRow2[i, "txtHinCode2"].Value);
                orderArray[i + 15].sSuu = Utility.NulltoStr(gcMultiRow2[i, "txtSuu2"].Value);
                orderArray[i + 15].sGroup = string.Empty;
            }

            // 追加発注欄
            for (int i = 0; i < 5; i++)
            {
                orderArray[i + 30] = new clsSKijun();
                orderArray[i + 30].sHinCode = Utility.NulltoStr(gcMultiRow3[i, "txtHinCode"].Value);
                orderArray[i + 30].sSuu = Utility.NulltoStr(gcMultiRow3[i, "txtSuu"].Value);
                orderArray[i + 30].sGroup = string.Empty;
                
                orderArray[i + 35] = new clsSKijun();
                orderArray[i + 35].sHinCode = Utility.NulltoStr(gcMultiRow3[i, "txtHinCode2"].Value);
                orderArray[i + 35].sSuu = Utility.NulltoStr(gcMultiRow3[i, "txtSuu2"].Value);
                orderArray[i + 35].sGroup = string.Empty;
            }
        }

        class clsSKijun
        {
            public string sHinCode;
            public string sSuu;
            public string sGroup;
        }
    }
}
