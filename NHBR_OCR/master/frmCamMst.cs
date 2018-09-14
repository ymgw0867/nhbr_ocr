using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using NHBR_OCR.common;
using GrapeCity.Win.MultiRow;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;

namespace NHBR_OCR.master
{
    public partial class frmCamMst : Form
    {
        public frmCamMst()
        {
            InitializeComponent();

            /* テーブルアダプターマネージャーにキャンペーンヘッダ、
             * キャンペーン明細アダプターを割り付ける */
            adpMn.キャンペーンヘッダTableAdapter = hAdp;
            adpMn.キャンペーン明細TableAdapter = iAdp;

            hAdp.Fill(dts.キャンペーンヘッダ);
            iAdp.Fill(dts.キャンペーン明細);
        }

        private void frmCalendar_Load(object sender, EventArgs e)
        {
            Utility.WindowsMaxSize(this, this.Width, this.Height);
            Utility.WindowsMinSize(this, this.Width, this.Height);

            // Tabキーの既定のショートカットキーを解除する。
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);

            // Tabキーのショートカットキーにユーザー定義のショートカットキーを割り当てる。
            gcMultiRow1.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Tab);
            //gcMultiRow1.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Enter);
            gcMultiRow1.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Enter);

            GridViewSetting(dataGridView1); // グリッドビュー設定
            GridViewShow(dataGridView1);    // グリッドビュー表示
            DispClr();                      // 画面初期化
            
            // 楽商データベース接続
            Conn.ConnectionString = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
            Conn.Open();
        }

        NHBRDataSet dts = new NHBRDataSet();
        NHBRDataSetTableAdapters.TableAdapterManager adpMn = new NHBRDataSetTableAdapters.TableAdapterManager();
        NHBRDataSetTableAdapters.キャンペーンヘッダTableAdapter hAdp = new NHBRDataSetTableAdapters.キャンペーンヘッダTableAdapter();
        NHBRDataSetTableAdapters.キャンペーン明細TableAdapter iAdp = new NHBRDataSetTableAdapters.キャンペーン明細TableAdapter();
        
        // ID
        string _ID;

        // 登録モード
        int _fMode = 0;

        // グリッドビューカラム名
        private string cDate = "c1";
        private string cGekkyu = "c2";
        private string cJikyu = "c3";
        private string cMemo = "c4";
        private string cID = "c5";
        private string eDate = "c6";

        // valueChangeステータス
        bool valueChangeStatus = false;
        
        OracleConnection Conn = new OracleConnection();

        DateTime staDate = DateTime.Parse("1900/01/01");
        DateTime endDate = DateTime.Parse("2900/12/31");

        string nonHinName = "-----";    // 該当商品なし表示

        ///------------------------------------------------------------------------
        /// <summary>
        ///     グリッドビューの定義を行います  </summary>
        /// <param name="dg">
        ///     データグリッドビューオブジェクト</param>
            ///------------------------------------------------------------------------
        private void GridViewSetting(DataGridView dg)
        {
            try
            {
                dg.EnableHeadersVisualStyles = false;
                dg.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                dg.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                //フォームサイズ定義

                // 列スタイルを変更する

                dg.EnableHeadersVisualStyles = false;

                // 列ヘッダー表示位置指定
                dg.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

                // 列ヘッダーフォント指定
                dg.ColumnHeadersDefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9, FontStyle.Regular);

                // データフォント指定
                dg.DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9, FontStyle.Regular);

                // 行の高さ
                dg.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dg.ColumnHeadersHeight = 20;
                dg.RowTemplate.Height = 20;

                // 全体の高さ
                dg.Height = 122;

                // 全体の幅
                //dg.Width = 583;

                // 奇数行の色
                //dg.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;

                //各列幅指定
                dg.Columns.Add(cMemo, "名称");
                dg.Columns.Add(cDate, "開始年月日");
                dg.Columns.Add(eDate, "終了年月日");
                dg.Columns.Add(cID, "ID");
                dg.Columns[cID].Visible = false;

                dg.Columns[cDate].Width = 110;
                dg.Columns[eDate].Width = 110;
                dg.Columns[cMemo].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dg.Columns[cDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dg.Columns[eDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
              
                // 行ヘッダを表示しない
                dg.RowHeadersVisible = false;

                // 選択モード
                dg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dg.MultiSelect = true;

                // 編集不可とする
                dg.ReadOnly = true;

                // 追加行表示しない
                dg.AllowUserToAddRows = false;

                // データグリッドビューから行削除を禁止する
                dg.AllowUserToDeleteRows = false;

                // 手動による列移動の禁止
                dg.AllowUserToOrderColumns = false;

                // 列サイズ変更不可
                dg.AllowUserToResizeColumns = false;

                // 行サイズ変更禁止
                dg.AllowUserToResizeRows = false;

                // 行ヘッダーの自動調節
                //dg.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

                //TAB動作
                dg.StandardTab = false;

                // ソート禁止
                foreach (DataGridViewColumn c in dg.Columns)
                {
                    c.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                //dg.Columns[cDay].SortMode = DataGridViewColumnSortMode.NotSortable;

                // 罫線
                dg.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
                dg.CellBorderStyle = DataGridViewCellBorderStyle.None;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラーメッセージ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        ///------------------------------------------------------------------------
        /// <summary>
        ///     キャンペーンデータをグリッドビューへ表示します </summary>
        /// <param name="tempGrid">
        ///     データグリッドビューオブジェクト</param>
        ///------------------------------------------------------------------------
        private void GridViewShow(DataGridView tempGrid)
        {
            int iX = 0;
            tempGrid.RowCount = 0;

            foreach (var t in dts.キャンペーンヘッダ.OrderByDescending(a => a.ID))
            {
                tempGrid.Rows.Add();

                if (t.開始年月日 == staDate)
                {
                    tempGrid[cDate, iX].Value = string.Empty;
                }
                else
                {
                    tempGrid[cDate, iX].Value = t.開始年月日.ToShortDateString();
                }

                if (t.終了年月日 == endDate)
                {
                    tempGrid[eDate, iX].Value = string.Empty;
                }
                else
                {
                    tempGrid[eDate, iX].Value = t.終了年月日.ToShortDateString();
                }

                tempGrid[cMemo, iX].Value = t.名称;
                tempGrid[cID, iX].Value = t.ID.ToString();
                iX++;
            }

            tempGrid.CurrentCell = null;
        }

        private void DispClr()
        {
            valueChangeStatus = false;
            gcMrSetting();
            valueChangeStatus = true;
            
            txtName.Text = string.Empty;
            dateTimePicker1.Checked = false;
            dateTimePicker2.Checked = false;

            button3.Enabled = true;
            button2.Enabled = false;
            button1.Enabled = false;

            _fMode = 0;
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
            gcMultiRow1[1, "lblNum"].Value = "2";
            gcMultiRow1[2, "lblNum"].Value = "3";
            gcMultiRow1[3, "lblNum"].Value = "4";
            gcMultiRow1[4, "lblNum"].Value = "5";
            gcMultiRow1[5, "lblNum"].Value = "6";
            gcMultiRow1[6, "lblNum"].Value = "7";
            gcMultiRow1[7, "lblNum"].Value = "8";
            gcMultiRow1[8, "lblNum"].Value = "9";
            gcMultiRow1[9, "lblNum"].Value = "10";
            gcMultiRow1[10, "lblNum"].Value = "11";
            gcMultiRow1[11, "lblNum"].Value = "12";
            gcMultiRow1[12, "lblNum"].Value = "13";
            gcMultiRow1[13, "lblNum"].Value = "14";
            gcMultiRow1[14, "lblNum"].Value = "15";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
        }

        ///----------------------------------------------------------------------------
        /// <summary>
        ///     キャンペーンヘッダとキャンペーン明細データを新規に登録する </summary>
        /// <param name="dt">
        ///     ヘッダID</param>
        ///----------------------------------------------------------------------------
        private void dataInsert(int hID)
        {
            try
            {
                // キャンペーンヘッダ
                NHBRDataSet.キャンペーンヘッダRow r = dts.キャンペーンヘッダ.NewキャンペーンヘッダRow();
                r.ID = hID;
                r.名称 = txtName.Text;

                if (dateTimePicker1.Checked)
                {
                    r.開始年月日 = DateTime.Parse(dateTimePicker1.Value.ToShortDateString());
                }
                else
                {
                    r.開始年月日 = staDate;
                }

                if (dateTimePicker2.Checked)
                {
                    r.終了年月日 = DateTime.Parse(dateTimePicker2.Value.ToShortDateString());
                }
                else
                {
                    r.終了年月日 = endDate;
                }

                r.更新年月日 = DateTime.Now;

                dts.キャンペーンヘッダ.AddキャンペーンヘッダRow(r);

                // キャンペーン明細
                for (int i = 0; i < gcMultiRow1.RowCount; i++)
                {
                    // 商品またはプレゼントが登録されている場合、登録可とした : 2017/11/19
                    if (gcMultiRow1[i, "txtSCode"].Value != null || gcMultiRow1[i, "txtPCode"].Value != null)
                    {
                        NHBRDataSet.キャンペーン明細Row m = dts.キャンペーン明細.Newキャンペーン明細Row();
                        m.ヘッダID = hID;
                        m.商品コード = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[i, "txtSCode"].Value));
                        m.商品数量 = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[i, "txtSSu"].Value));
                        m.プレゼント商品コード = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[i, "txtPCode"].Value));
                        m.プレゼント数量 = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[i, "txtPSu"].Value));
                        m.更新年月日 = DateTime.Now;

                        dts.キャンペーン明細.Addキャンペーン明細Row(m);
                    }
                }

                // データベースアップデート
                adpMn.UpdateAll(dts);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "キャンペーンマスターの登録に失敗しました");
            }
            finally
            {

            }
        }

        ///----------------------------------------------------------------
        /// <summary>
        ///     休日データを更新する </summary>
        /// <param name="dt">
        ///     対象となる日付</param>
        ///----------------------------------------------------------------
        private void dataUpdate(int sID)
        {
            try 
	        {
                if (dts.キャンペーンヘッダ.Any(a => a.ID == sID))
                {
                    // キャンペーンヘッダデータ
                    var s = dts.キャンペーンヘッダ.Single(a => a.ID == sID);
                    s.名称 = txtName.Text;

                    if (dateTimePicker1.Checked)
                    {
                        s.開始年月日 = DateTime.Parse(dateTimePicker1.Value.ToShortDateString());
                    }
                    else
                    {
                        s.開始年月日 = staDate;
                    }

                    if (dateTimePicker2.Checked)
                    {
                        s.終了年月日 = DateTime.Parse(dateTimePicker2.Value.ToShortDateString());
                    }
                    else
                    {
                        s.終了年月日 = endDate;
                    }

                    s.更新年月日 = DateTime.Now;

                    // キャンペーン明細データ
                    for (int i = 0; i < gcMultiRow1.RowCount; i++)
                    {
                        // 更新明細
                        if (gcMultiRow1[i, "lblID"].Value != null)
                        {
                            if (dts.キャンペーン明細.Any(a => a.ID == Utility.StrtoInt(gcMultiRow1[i, "lblID"].Value.ToString())))
                            {
                                var sss = dts.キャンペーン明細.Single(a => a.ID == Utility.StrtoInt(gcMultiRow1[i, "lblID"].Value.ToString()));
                                sss.商品コード = Utility.StrtoInt(gcMultiRow1[i, "txtSCode"].Value.ToString());
                                sss.商品数量 = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[i, "txtSSu"].Value));
                                sss.プレゼント商品コード = Utility.StrtoInt(gcMultiRow1[i, "txtPCode"].Value.ToString());
                                sss.プレゼント数量 = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[i, "txtPSu"].Value));
                            }
                        }
                        else
                        {
                            // 新規登録明細
                            if (gcMultiRow1[i, "txtSCode"].Value != null)
                            {
                                NHBRDataSet.キャンペーン明細Row m = dts.キャンペーン明細.Newキャンペーン明細Row();
                                m.ヘッダID = sID;
                                m.商品コード = Utility.StrtoInt(gcMultiRow1[i, "txtSCode"].Value.ToString());
                                m.商品数量 = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[i, "txtSSu"].Value));
                                m.プレゼント商品コード = Utility.StrtoInt(gcMultiRow1[i, "txtPCode"].Value.ToString());
                                m.プレゼント数量 = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[i, "txtPSu"].Value));
                                m.更新年月日 = DateTime.Now;

                                dts.キャンペーン明細.Addキャンペーン明細Row(m);
                            }
                        }
                    }

                    // データベースアップデート
                    adpMn.UpdateAll(dts);
                }
	        }
	        catch (Exception ex)
	        {
		        MessageBox.Show(ex.Message + Environment.NewLine + "キャンペーンマスターの更新に失敗しました");
	        }
        }

        ///--------------------------------------------------------------
        /// <summary>
        ///     キャンペーンデータを削除する </summary>
        /// <param name="sID">
        ///     レコードID</param>
        /// <returns>
        ///     true:削除成功、false:削除失敗</returns>
        ///--------------------------------------------------------------
        private void dataDelete(int sID)
        {
            try
            {
                // 削除ヘッダデータ取得（エラー回避のためDataRowState.Deleted と DataRowState.Detachedは除外して抽出する）
                var d = dts.キャンペーンヘッダ.Where(a => a.RowState != DataRowState.Deleted && a.RowState != DataRowState.Detached && a.ID == sID);

                // foreach用の配列を作成する
                var list = d.ToList();

                // 削除
                foreach (var it in list)
                {
                    NHBRDataSet.キャンペーンヘッダRow dl = dts.キャンペーンヘッダ.FindByID(it.ID);
                    dl.Delete();
                }


                // 削除明細データ取得（エラー回避のためDataRowState.Deleted と DataRowState.Detachedは除外して抽出する）
                var md = dts.キャンペーン明細.Where(a => a.RowState != DataRowState.Deleted && a.RowState != DataRowState.Detached && a.ヘッダID == sID);

                // foreach用の配列を作成する
                var list2 = md.ToList();

                // 削除
                foreach (var it in list2)
                {
                    NHBRDataSet.キャンペーン明細Row dl = dts.キャンペーン明細.FindByID(it.ID);
                    dl.Delete();
                }

                adpMn.UpdateAll(dts);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + Environment.NewLine + "キャンペーンマスターの削除に失敗しました", "削除失敗", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            int r = dataGridView1.SelectedRows[0].Index;
            _ID = dataGridView1[cID, r].Value.ToString();

            // 画面初期化
            DispClr();

            // データ表示
            GetGridViewData(dataGridView1, Utility.StrtoInt(_ID));

            txtName.Focus();
        }

        ///---------------------------------------------------------------------
        /// <summary>
        ///     グリッドビューの選択された行データを表示する </summary>
        /// <param name="g">
        ///     データグリッドビューオブジェクト</param>
        ///---------------------------------------------------------------------
        private void GetGridViewData(DataGridView g, int sID)
        {
            if (dts.キャンペーンヘッダ.Any(a => a.ID == sID))
            {
                // キャンペーンヘッダデータ
                var s = dts.キャンペーンヘッダ.Single(a => a.ID == sID);
                txtName.Text = s.名称;

                if (s.開始年月日 != staDate)
                {
                    dateTimePicker1.Value = s.開始年月日;
                    dateTimePicker1.Checked = true;
                }
                else
                {
                    dateTimePicker1.Checked = false;
                }

                if (s.終了年月日 != endDate)
                {
                    dateTimePicker2.Value = s.終了年月日;
                    dateTimePicker2.Checked = true;
                }
                else
                {
                    dateTimePicker2.Checked = false;
                }
                
                // キャンペーン明細データ
                int n = 0;
                foreach (var t in dts.キャンペーン明細.Where(a => a.ヘッダID == sID)
                    .OrderBy(a => a.ID))
                {
                    gcMultiRow1[n, "txtSCode"].Value = t.商品コード;
                    gcMultiRow1[n, "txtSSu"].Value = t.商品数量;
                    gcMultiRow1[n, "txtPCode"].Value = t.プレゼント商品コード;
                    gcMultiRow1[n, "txtPSu"].Value = t.プレゼント数量;
                    gcMultiRow1[n, "lblID"].Value = t.ID;
                    n++;
                }

                gcMultiRow1.CurrentCell = null;
            }

            button3.Enabled = true;
            button2.Enabled = true;
            button1.Enabled = true;

            _fMode = 1;
        }

        private void btnClr_Click(object sender, EventArgs e)
        {
        }

        ///---------------------------------------------------------------------
        /// <summary>
        ///     データ削除 </summary>
        /// <param name="sender">
        ///     </param>
        /// <param name="e">
        ///     </param>
        ///---------------------------------------------------------------------
        private void btnDelete_Click(object sender, EventArgs e)
        {
        }

        ///---------------------------------------------------------------------
        /// <summary>
        ///     休日データを検索する </summary>
        /// <param name="dt">
        ///     対象となる日付</param>
        /// <returns>
        ///     true:データなし、false:データあり</returns>
        ///---------------------------------------------------------------------
        private bool dataSearch(DateTime dt)
        {
            //string s2 = dt.ToShortDateString();

            //if (dts.休日.Any(a => a.年月日.ToShortDateString() == s2))
            //{
            //    return false;
            //}
                          
            return true;
        }

        private void btnRtn_Click(object sender, EventArgs e)
        {
        }

        private void frmCalendar_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 楽商データベース接続解除
            Conn.Close();
            Conn.Dispose();

            // 後片付け
            this.Dispose();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewShow(dataGridView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //frmCalenderBatch frm = new config.frmCalenderBatch();
            //frm.ShowDialog();
            //adp.Fill(dts.休日);
            //GridViewShow(dataGridView1);    // グリッドビュー表示
        }

        private void linkLabel4_Click(object sender, EventArgs e)
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

        private void gcMultiRow1_CellEnter(object sender, CellEventArgs e)
        {
            if (gcMultiRow1.EditMode == EditMode.EditProgrammatically)
            {
                gcMultiRow1.BeginEdit(true);
            }
        }


        private void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
        {
            if (!valueChangeStatus)
            {
                return;
            }

            if (e.RowIndex < 0)
            {
                return;
            }

            // 商品名表示
            if (e.CellName == "txtSCode" || e.CellName == "txtPCode")
            {
                valueChangeStatus = false;
                gcHinCodeChange(gcMultiRow1, e.CellName, e.RowIndex, true);                
                valueChangeStatus = true;
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
            string hinName = string.Empty;

            if (cCellName == "txtSCode")
            {
                hinName = "lblSName";
                hinCode = Utility.NulltoStr(gc[rIndex, "txtSCode"].Value).PadLeft(8, '0');
            }
            else
            {
                hinName = "lblPName";
                hinCode = Utility.NulltoStr(gc[rIndex, "txtPCode"].Value).PadLeft(8, '0');
            }


            if (hinCode != "00000000")
            {
                gc[rIndex, cCellName].Value = hinCode;
            }

            gc[rIndex, hinName].Value = string.Empty;

            string strSQL = "select SYO_ID, SYO_NAME, SYO_IRI_KESU, SYO_TANI from RAKUSYO_FAXOCR.V_SYOHIN WHERE SYO_ID = '" + hinCode + "'";
            OracleCommand Cmd = new OracleCommand(strSQL, Conn);
            OracleDataReader dR = Cmd.ExecuteReader();

            while (dR.Read())
            {
                gc[rIndex, hinName].Value = dR["SYO_NAME"].ToString().Trim();
            }
            
            dR.Dispose();
            Cmd.Dispose();
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

        void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '\t')
                e.Handled = true;
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

        private void gcMultiRow1_Leave(object sender, EventArgs e)
        {
            gcMultiRow1.EndEdit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            camDataUpdate();
        }

        private void camDataUpdate()
        {
            if (txtName.Text == string.Empty)
            {
                MessageBox.Show("キャンペーン名称が入力されていません", "キャンペーン名称未入力", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
                return;
            }

            string sCode = string.Empty;

            for (int i = 0; i < gcMultiRow1.RowCount; i++)
            {
                // 2017/11/19
                //if (gcMultiRow1[i, "txtSCode"].Value != null && gcMultiRow1[i, "txtPCode"].Value == null)
                //{
                //    MessageBox.Show("プレゼントが入力されていません", "プレゼント名称未入力", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    gcMultiRow1.CurrentCellPosition = new CellPosition(i, "txtPCode");
                //    return;
                //}

                if (gcMultiRow1[i, "txtPCode"].Value == null && gcMultiRow1[i, "txtPSu"].Value != null)
                {
                    MessageBox.Show("プレゼントが入力されていません", "プレゼント名称未入力", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    gcMultiRow1.CurrentCellPosition = new CellPosition(i, "txtPCode");
                    return;
                }

                // 2017/11/19
                //if (gcMultiRow1[i, "txtSCode"].Value == null && gcMultiRow1[i, "txtPCode"].Value != null)
                //{
                //    MessageBox.Show("対象商品が入力されていません", "商品名称未入力", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    gcMultiRow1.CurrentCellPosition = new CellPosition(i, "txtSCode");
                //    return;
                //}

                if (gcMultiRow1[i, "txtSCode"].Value == null && gcMultiRow1[i, "txtSSu"].Value != null)
                {
                    MessageBox.Show("対象商品が入力されていません", "商品名称未入力", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    gcMultiRow1.CurrentCellPosition = new CellPosition(i, "txtSCode");
                    return;
                }

                if (gcMultiRow1[i, "txtSCode"].Value != null)
                {
                    sCode = gcMultiRow1[i, "txtSCode"].Value.ToString();
                }
            }

            if (sCode == string.Empty)
            {
                MessageBox.Show("対象商品が入力されていません", "商品未入力", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                gcMultiRow1.CurrentCellPosition = new CellPosition(0, "txtSCode");
                return;
            }

            switch (_fMode)
            {
                case 0:
                    if (MessageBox.Show("登録しますか？", "新規キャンペーン", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }

                    int hid = 0;
                    if (dts.キャンペーンヘッダ.Count() > 0)
                    {
                        hid = dts.キャンペーンヘッダ.Max(a => a.ID);
                    }

                    hid++;
                    dataInsert(hid);
                    break;

                case 1:
                    if (MessageBox.Show("更新しますか？", "キャンペーン情報", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }

                    dataUpdate(Utility.StrtoInt(_ID));
                    break;

                default:
                    break;
            }

            hAdp.Fill(dts.キャンペーンヘッダ);
            iAdp.Fill(dts.キャンペーン明細);

            GridViewShow(dataGridView1);
            DispClr();
        }

        private void camDataDelete()
        {
            if (MessageBox.Show("削除しますか？", "新規キャンペーン", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            dataDelete(Utility.StrtoInt(_ID));

            hAdp.Fill(dts.キャンペーンヘッダ);
            iAdp.Fill(dts.キャンペーン明細);

            GridViewShow(dataGridView1);
            DispClr();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            camDataDelete();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            DispClr();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCamMst_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F9 && button3.Enabled)
            {
                camDataUpdate();
            }

            if (e.KeyData == Keys.F10 && button2.Enabled)
            {
                camDataDelete();
            }

            if (e.KeyData == Keys.F11 && button1.Enabled)
            {
                DispClr();
            }

            if (e.KeyData == Keys.F12)
            {
                Close();
            }
        }

    }
}
