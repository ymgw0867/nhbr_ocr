using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHBR_OCR.common;

namespace NHBR_OCR.config
{
    public partial class frmShukkaKIjun : Form
    {
        public frmShukkaKIjun()
        {
            InitializeComponent();
            adp.Fill(dts.出荷基準設定);
        }

        NHBRDataSet dts = new NHBRDataSet();
        NHBRDataSetTableAdapters.出荷基準設定TableAdapter adp = new NHBRDataSetTableAdapters.出荷基準設定TableAdapter();

        private void txtA10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
            {
                e.Handled = true;
                return;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            dataUpdate();
        }

        private void dataUpdate()
        {
            //// エラーチェック
            //if (!errCheck())
            //{
            //    return;
            //}

            if (MessageBox.Show("データを更新してよろしいですか", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No) return;

            NHBRDataSet.出荷基準設定Row r = dts.出荷基準設定.Single(a => a.ID == global.configKEY);

            // Ａ：小瓶グループ
            r.小瓶条件A = Utility.StrtoInt(txtA10.Text);

            if (cmbA.SelectedIndex == 0)
            {
                r.小瓶注文単位数A = global.SH_GUUSU;
            }
            else if (cmbA.SelectedIndex == 1)
            {
                r.小瓶注文単位数A = global.SH_KISU;
            }
            else if (cmbA.SelectedIndex == 2)
            {
                r.小瓶注文単位数A = global.SH_EMPTY;
            }

            r.小瓶合計単位数A = txtA20.Text;

            // Ｂ：中瓶グループ
            r.中瓶条件1B = Utility.StrtoInt(txtB10.Text);
            r.中瓶条件2B = Utility.StrtoInt(txtB11.Text);

            if (cmbB.SelectedIndex == 0)
            {
                r.中瓶注文単位数B = global.SH_GUUSU;
            }
            else if (cmbB.SelectedIndex == 1)
            {
                r.中瓶注文単位数B = global.SH_KISU;
            }
            else if (cmbB.SelectedIndex == 2)
            {
                r.中瓶注文単位数B = global.SH_EMPTY;
            }

            r.中瓶合計単位数1B = txtB20.Text;
            r.中瓶合計単位数2B = txtB21.Text;

            // Ｃ：大瓶グループ
            r.大瓶条件1C = txtCc10.Text;
            r.大瓶条件2C = txtCc11.Text;
            r.大瓶条件3C = txtCc12.Text;
            r.大瓶条件4C = txtCc13.Text;
            r.大瓶条件5C = txtCc14.Text;
            r.大瓶条件6C = txtCc15.Text;
            r.大瓶条件7C = txtCc16.Text;
            r.大瓶条件8C = txtCc17.Text;
            r.大瓶条件9C = txtCc18.Text;
            r.大瓶条件10C = txtCc19.Text;
            r.大瓶条件11C = txtCc20.Text;
            r.大瓶条件12C = txtCc21.Text;
            r.大瓶条件13C = txtCc22.Text;
            r.大瓶条件14C = txtCc23.Text;
            r.大瓶条件15C = txtCc24.Text;
            r.大瓶条件16C = txtCc25.Text;
            r.大瓶条件17C = txtCc26.Text;
            r.大瓶条件18C = txtCc27.Text;
            r.大瓶条件19C = txtCc28.Text;
            r.大瓶条件20C = txtCc29.Text;

            if (cmbC.SelectedIndex == 0)
            {
                r.大瓶注文単位数C = global.SH_GUUSU;
            }
            else if (cmbC.SelectedIndex == 1)
            {
                r.大瓶注文単位数C = global.SH_KISU;
            }
            else if (cmbC.SelectedIndex == 2)
            {
                r.大瓶注文単位数C = global.SH_EMPTY;
            }

            r.大瓶合計単位数C = txtC20.Text;

            // Ｄ：特瓶グループ
            r.特瓶商品1D = txtDc10.Text;
            r.特瓶商品2D = txtDc11.Text;
            r.特瓶商品3D = txtDc12.Text;
            r.特瓶商品4D = txtDc13.Text;
            r.特瓶商品5D = txtDc14.Text;
            r.特瓶商品6D = txtDc15.Text;
            r.特瓶商品7D = txtDc16.Text;
            r.特瓶商品8D = txtDc17.Text;
            r.特瓶商品9D = txtDc18.Text;
            r.特瓶商品10D = txtDc19.Text;
            r.特瓶商品11D = txtDc20.Text;
            r.特瓶商品12D = txtDc21.Text;
            r.特瓶商品13D = txtDc22.Text;
            r.特瓶商品14D = txtDc23.Text;
            r.特瓶商品15D = txtDc24.Text;
            r.特瓶商品16D = txtDc25.Text;
            r.特瓶商品17D = txtDc26.Text;
            r.特瓶商品18D = txtDc27.Text;
            r.特瓶商品19D = txtDc28.Text;
            r.特瓶商品20D = txtDc29.Text;

            if (cmbD.SelectedIndex == 0)
            {
                r.特瓶注文単位数D = global.SH_GUUSU;
            }
            else if (cmbD.SelectedIndex == 1)
            {
                r.特瓶注文単位数D = global.SH_KISU;
            }
            else if (cmbD.SelectedIndex == 2)
            {
                r.特瓶注文単位数D = global.SH_EMPTY;
            }

            r.特瓶合計単位数D = string.Empty;

            // Ｅ：缶グループ
            r.缶条件E = txtE10.Text;
            r.缶注文単位数E = txtE20.Text;
            r.缶合計単位数E = txtE21.Text;

            // Ｆ：缶グループ（ノンアルコール）
            r.缶条件F = txtF10.Text;
            r.缶注文単位数F = txtF20.Text;
            r.缶合計単位数F = txtF21.Text;

            // Ｇ：樽グループ
            r.樽条件G = string.Empty;
            r.樽注文単位数G = txtG20.Text;
            r.樽合計単位数G = string.Empty;
            
            r.更新年月日 = DateTime.Now;

            // データ更新
            adp.Update(r);
            
            // 終了
            this.Close();
        }

        private void frmShukkaKIjun_Load(object sender, EventArgs e)
        {
            Utility.WindowsMaxSize(this, this.Width, this.Height);
            Utility.WindowsMinSize(this, this.Width, this.Height);

            // データ表示
            showData();
        }


        private void showData()
        {
            NHBRDataSet.出荷基準設定Row r = dts.出荷基準設定.Single(a => a.ID == global.configKEY);

            // グループＡ：小瓶
            if (r.Is小瓶条件ANull())
            {
                txtA10.Text = string.Empty;
            }
            else
            {
                txtA10.Text = r.小瓶条件A.ToString();
            }

            if (r.Is小瓶注文単位数ANull())
            {
                cmbA.SelectedIndex = -1;
            }
            else if (r.小瓶注文単位数A == global.SH_GUUSU)
            {
                cmbA.SelectedIndex = 0;
            }
            else if (r.小瓶注文単位数A == global.SH_KISU)
            {
                cmbA.SelectedIndex = 1;
            }
            else if (r.小瓶注文単位数A == global.SH_EMPTY)
            {
                cmbA.SelectedIndex = 2;
            }

            if (r.Is小瓶合計単位数ANull())
            {
                txtA20.Text = string.Empty;
            }
            else
            {
                txtA20.Text = r.小瓶合計単位数A;
            }

            // グループＢ：中瓶
            if (r.Is中瓶条件1BNull())
            {
                txtB10.Text = string.Empty;
            }
            else
            {
                txtB10.Text = r.中瓶条件1B.ToString();
            }

            if (r.Is中瓶条件2BNull())
            {
                txtB11.Text = string.Empty;
            }
            else
            {
                txtB11.Text = r.中瓶条件2B.ToString();
            }

            if (r.Is中瓶注文単位数BNull())
            {
                cmbB.SelectedIndex = -1;
            }
            else if (r.中瓶注文単位数B == global.SH_GUUSU)
            {
                cmbB.SelectedIndex = 0;
            }
            else if (r.中瓶注文単位数B == global.SH_KISU)
            {
                cmbB.SelectedIndex = 1;
            }
            else if (r.中瓶注文単位数B == global.SH_EMPTY)
            {
                cmbB.SelectedIndex = 2;
            }

            if (r.Is中瓶合計単位数1BNull())
            {
                txtB20.Text = string.Empty;
            }
            else
            {
                txtB20.Text = r.中瓶合計単位数1B;
            }

            if (r.Is中瓶合計単位数2BNull())
            {
                txtB21.Text = string.Empty;
            }
            else
            {
                txtB21.Text = r.中瓶合計単位数2B;
            }

            // グループＣ：大瓶
            if (r.Is大瓶条件1CNull())
            {
                txtCc10.Text = string.Empty;
            }
            else
            {
                txtCc10.Text = r.大瓶条件1C;
            }

            if (r.Is大瓶条件2CNull())
            {
                txtCc11.Text = string.Empty;
            }
            else
            {
                txtCc11.Text = r.大瓶条件2C;
            }

            if (r.Is大瓶条件3CNull())
            {
                txtCc12.Text = string.Empty;
            }
            else
            {
                txtCc12.Text = r.大瓶条件3C;
            }

            if (r.Is大瓶条件4CNull())
            {
                txtCc13.Text = string.Empty;
            }
            else
            {
                txtCc13.Text = r.大瓶条件4C;
            }

            if (r.Is大瓶条件5CNull())
            {
                txtCc14.Text = string.Empty;
            }
            else
            {
                txtCc14.Text = r.大瓶条件5C;
            }

            if (r.Is大瓶条件6CNull())
            {
                txtCc15.Text = string.Empty;
            }
            else
            {
                txtCc15.Text = r.大瓶条件6C;
            }

            if (r.Is大瓶条件7CNull())
            {
                txtCc16.Text = string.Empty;
            }
            else
            {
                txtCc16.Text = r.大瓶条件7C;
            }

            if (r.Is大瓶条件8CNull())
            {
                txtCc17.Text = string.Empty;
            }
            else
            {
                txtCc17.Text = r.大瓶条件8C;
            }

            if (r.Is大瓶条件9CNull())
            {
                txtCc18.Text = string.Empty;
            }
            else
            {
                txtCc18.Text = r.大瓶条件9C;
            }

            if (r.Is大瓶条件10CNull())
            {
                txtCc19.Text = string.Empty;
            }
            else
            {
                txtCc19.Text = r.大瓶条件10C;
            }

            if (r.Is大瓶条件11CNull())
            {
                txtCc20.Text = string.Empty;
            }
            else
            {
                txtCc20.Text = r.大瓶条件11C;
            }

            if (r.Is大瓶条件12CNull())
            {
                txtCc21.Text = string.Empty;
            }
            else
            {
                txtCc21.Text = r.大瓶条件12C;
            }

            if (r.Is大瓶条件13CNull())
            {
                txtCc22.Text = string.Empty;
            }
            else
            {
                txtCc22.Text = r.大瓶条件13C;
            }

            if (r.Is大瓶条件14CNull())
            {
                txtCc23.Text = string.Empty;
            }
            else
            {
                txtCc23.Text = r.大瓶条件14C;
            }

            if (r.Is大瓶条件15CNull())
            {
                txtCc24.Text = string.Empty;
            }
            else
            {
                txtCc24.Text = r.大瓶条件15C;
            }

            if (r.Is大瓶条件16CNull())
            {
                txtCc25.Text = string.Empty;
            }
            else
            {
                txtCc25.Text = r.大瓶条件16C;
            }

            if (r.Is大瓶条件17CNull())
            {
                txtCc26.Text = string.Empty;
            }
            else
            {
                txtCc26.Text = r.大瓶条件17C;
            }

            if (r.Is大瓶条件18CNull())
            {
                txtCc27.Text = string.Empty;
            }
            else
            {
                txtCc27.Text = r.大瓶条件18C;
            }

            if (r.Is大瓶条件19CNull())
            {
                txtCc28.Text = string.Empty;
            }
            else
            {
                txtCc28.Text = r.大瓶条件19C;
            }

            if (r.Is大瓶条件20CNull())
            {
                txtCc29.Text = string.Empty;
            }
            else
            {
                txtCc29.Text = r.大瓶条件20C;
            }

            if (r.Is大瓶注文単位数CNull())
            {
                cmbC.SelectedIndex = -1;
            }
            else if (r.大瓶注文単位数C == global.SH_GUUSU)
            {
                cmbC.SelectedIndex = 0;
            }
            else if (r.大瓶注文単位数C == global.SH_KISU)
            {
                cmbC.SelectedIndex = 1;
            }            
            else if (r.大瓶注文単位数C == global.SH_EMPTY)
            {
                cmbC.SelectedIndex = 2;
            }

            if (r.Is大瓶合計単位数CNull())
            {
                txtC20.Text  = string.Empty;
            }
            else
            {
                txtC20.Text = r.大瓶合計単位数C;
            }

            // グループＤ：特瓶
            if (r.Is特瓶商品1DNull())
            {
                txtDc10.Text = string.Empty;
            }
            else
            {
                txtDc10.Text = r.特瓶商品1D;
            }

            if (r.Is特瓶商品2DNull())
            {
                txtDc11.Text = string.Empty;
            }
            else
            {
                txtDc11.Text = r.特瓶商品2D;
            }

            if (r.Is特瓶商品3DNull())
            {
                txtDc12.Text = string.Empty;
            }
            else
            {
                txtDc12.Text = r.特瓶商品3D;
            }

            if (r.Is特瓶商品4DNull())
            {
                txtDc13.Text = string.Empty;
            }
            else
            {
                txtDc13.Text = r.特瓶商品4D;
            }

            if (r.Is特瓶商品5DNull())
            {
                txtDc14.Text = string.Empty;
            }
            else
            {
                txtDc14.Text = r.特瓶商品5D;
            }

            if (r.Is特瓶商品6DNull())
            {
                txtDc15.Text = string.Empty;
            }
            else
            {
                txtDc15.Text = r.特瓶商品6D;
            }

            if (r.Is特瓶商品7DNull())
            {
                txtDc16.Text = string.Empty;
            }
            else
            {
                txtDc16.Text = r.特瓶商品7D;
            }

            if (r.Is特瓶商品8DNull())
            {
                txtDc17.Text = string.Empty;
            }
            else
            {
                txtDc17.Text = r.特瓶商品8D;
            }

            if (r.Is特瓶商品9DNull())
            {
                txtDc18.Text = string.Empty;
            }
            else
            {
                txtDc18.Text = r.特瓶商品9D;
            }

            if (r.Is特瓶商品10DNull())
            {
                txtDc19.Text = string.Empty;
            }
            else
            {
                txtDc19.Text = r.特瓶商品10D;
            }

            if (r.Is特瓶商品11DNull())
            {
                txtDc20.Text = string.Empty;
            }
            else
            {
                txtDc20.Text = r.特瓶商品11D;
            }

            if (r.Is特瓶商品12DNull())
            {
                txtDc21.Text = string.Empty;
            }
            else
            {
                txtDc21.Text = r.特瓶商品12D;
            }

            if (r.Is特瓶商品13DNull())
            {
                txtDc22.Text = string.Empty;
            }
            else
            {
                txtDc22.Text = r.特瓶商品13D;
            }

            if (r.Is特瓶商品14DNull())
            {
                txtDc23.Text = string.Empty;
            }
            else
            {
                txtDc23.Text = r.特瓶商品14D;
            }

            if (r.Is特瓶商品15DNull())
            {
                txtDc24.Text = string.Empty;
            }
            else
            {
                txtDc24.Text = r.特瓶商品15D;
            }

            if (r.Is特瓶商品16DNull())
            {
                txtDc25.Text = string.Empty;
            }
            else
            {
                txtDc25.Text = r.特瓶商品16D;
            }

            if (r.Is特瓶商品17DNull())
            {
                txtDc26.Text = string.Empty;
            }
            else
            {
                txtDc26.Text = r.特瓶商品17D;
            }

            if (r.Is特瓶商品18DNull())
            {
                txtDc27.Text = string.Empty;
            }
            else
            {
                txtDc27.Text = r.特瓶商品18D;
            }

            if (r.Is特瓶商品19DNull())
            {
                txtDc28.Text = string.Empty;
            }
            else
            {
                txtDc28.Text = r.特瓶商品19D;
            }

            if (r.Is特瓶商品20DNull())
            {
                txtDc29.Text = string.Empty;
            }
            else
            {
                txtDc29.Text = r.特瓶商品20D;
            }

            if (r.Is特瓶注文単位数DNull())
            {
                cmbD.SelectedIndex = -1;
            }
            else if (r.特瓶注文単位数D == global.SH_GUUSU)
            {
                cmbD.SelectedIndex = 0;
            }
            else if (r.特瓶注文単位数D == global.SH_KISU)
            {
                cmbD.SelectedIndex = 1;
            }
            else if (r.特瓶注文単位数D == global.SH_EMPTY)
            {
                cmbD.SelectedIndex = 2;
            }

            // グループＥ：缶
            if (r.Is缶条件ENull())
            {
                txtE10.Text = string.Empty;
            }
            else
            {
                txtE10.Text = r.缶条件E;
            }

            if (r.Is缶注文単位数ENull())
            {
                txtE20.Text = string.Empty;
            }
            else
            {
                txtE20.Text = r.缶注文単位数E;
            }

            if (r.Is缶合計単位数ENull())
            {
                txtE21.Text = string.Empty;
            }
            else
            {
                txtE21.Text = r.缶合計単位数E;
            }

            // グループＦ：缶（ノンアルコール）
            if (r.Is缶条件FNull())
            {
                txtF10.Text = string.Empty;
            }
            else
            {
                txtF10.Text = r.缶条件F;
            }

            if (r.Is缶注文単位数FNull())
            {
                txtF20.Text = string.Empty;
            }
            else
            {
                txtF20.Text = r.缶注文単位数F;
            }

            if (r.Is缶合計単位数FNull())
            {
                txtF21.Text = string.Empty;
            }
            else
            {
                txtF21.Text = r.缶合計単位数F;
            }

            // グループＧ：樽
            if (r.Is樽注文単位数GNull())
            {
                txtG20.Text = string.Empty;
            }
            else
            {
                txtG20.Text = r.樽注文単位数G;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmShukkaKIjun_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 後片付け
            Dispose();
        }
    }
}
