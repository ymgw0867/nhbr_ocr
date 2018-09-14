namespace NHBR_OCR.OCR
{
    partial class frmCanpaignRec
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCanpaignRec));
            this.leadImg = new Leadtools.WinForms.RasterImageViewer();
            this.button1 = new System.Windows.Forms.Button();
            this.lblNoImage = new System.Windows.Forms.Label();
            this.btnEnd = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBefore = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnPlus = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnDataMake = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.txtMemo = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gcMultiRow2 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.camTemplate31 = new NHBR_OCR.OCR.camTemplate3();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.camTemplate21 = new NHBR_OCR.OCR.camTemplate2();
            this.SuspendLayout();
            // 
            // leadImg
            // 
            this.leadImg.AutoScroll = false;
            this.leadImg.Location = new System.Drawing.Point(2, 3);
            this.leadImg.Name = "leadImg";
            this.leadImg.Size = new System.Drawing.Size(593, 828);
            this.leadImg.TabIndex = 122;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(243)))), ((int)(((byte)(190)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button1.Location = new System.Drawing.Point(601, 73);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(376, 24);
            this.button1.TabIndex = 1;
            this.button1.Text = "キャンペーン選択";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblNoImage
            // 
            this.lblNoImage.Font = new System.Drawing.Font("メイリオ", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblNoImage.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.lblNoImage.Location = new System.Drawing.Point(141, 375);
            this.lblNoImage.Name = "lblNoImage";
            this.lblNoImage.Size = new System.Drawing.Size(322, 42);
            this.lblNoImage.TabIndex = 126;
            this.lblNoImage.Text = "画像はありません";
            this.lblNoImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnEnd
            // 
            this.btnEnd.BackColor = System.Drawing.SystemColors.Control;
            this.btnEnd.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnEnd.Image = ((System.Drawing.Image)(resources.GetObject("btnEnd.Image")));
            this.btnEnd.Location = new System.Drawing.Point(736, 749);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(45, 36);
            this.btnEnd.TabIndex = 132;
            this.btnEnd.TabStop = false;
            this.toolTip1.SetToolTip(this.btnEnd, "最後のデータへ移動");
            this.btnEnd.UseVisualStyleBackColor = false;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.SystemColors.Control;
            this.btnNext.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnNext.Image = ((System.Drawing.Image)(resources.GetObject("btnNext.Image")));
            this.btnNext.Location = new System.Drawing.Point(691, 749);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(45, 36);
            this.btnNext.TabIndex = 131;
            this.btnNext.TabStop = false;
            this.toolTip1.SetToolTip(this.btnNext, "次のデータへ移動");
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBefore
            // 
            this.btnBefore.BackColor = System.Drawing.SystemColors.Control;
            this.btnBefore.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnBefore.Image = ((System.Drawing.Image)(resources.GetObject("btnBefore.Image")));
            this.btnBefore.Location = new System.Drawing.Point(646, 749);
            this.btnBefore.Name = "btnBefore";
            this.btnBefore.Size = new System.Drawing.Size(45, 36);
            this.btnBefore.TabIndex = 130;
            this.btnBefore.TabStop = false;
            this.toolTip1.SetToolTip(this.btnBefore, "前のデータへ移動");
            this.btnBefore.UseVisualStyleBackColor = false;
            this.btnBefore.Click += new System.EventHandler(this.btnBefore_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.BackColor = System.Drawing.SystemColors.Control;
            this.btnFirst.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnFirst.Image = ((System.Drawing.Image)(resources.GetObject("btnFirst.Image")));
            this.btnFirst.Location = new System.Drawing.Point(601, 749);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(45, 36);
            this.btnFirst.TabIndex = 129;
            this.btnFirst.TabStop = false;
            this.toolTip1.SetToolTip(this.btnFirst, "先頭データへ移動");
            this.btnFirst.UseVisualStyleBackColor = false;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnMinus
            // 
            this.btnMinus.BackColor = System.Drawing.SystemColors.Control;
            this.btnMinus.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnMinus.Image = ((System.Drawing.Image)(resources.GetObject("btnMinus.Image")));
            this.btnMinus.Location = new System.Drawing.Point(76, 834);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(36, 36);
            this.btnMinus.TabIndex = 128;
            this.btnMinus.TabStop = false;
            this.toolTip1.SetToolTip(this.btnMinus, "画像縮小");
            this.btnMinus.UseVisualStyleBackColor = false;
            this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.BackColor = System.Drawing.SystemColors.Control;
            this.btnPlus.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnPlus.Image = ((System.Drawing.Image)(resources.GetObject("btnPlus.Image")));
            this.btnPlus.Location = new System.Drawing.Point(113, 834);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(36, 36);
            this.btnPlus.TabIndex = 127;
            this.btnPlus.TabStop = false;
            this.toolTip1.SetToolTip(this.btnPlus, "画像拡大");
            this.btnPlus.UseVisualStyleBackColor = false;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // label1
            // 
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(983, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 18);
            this.label1.TabIndex = 133;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnRight
            // 
            this.btnRight.BackColor = System.Drawing.SystemColors.Control;
            this.btnRight.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnRight.Image = ((System.Drawing.Image)(resources.GetObject("btnRight.Image")));
            this.btnRight.Location = new System.Drawing.Point(39, 834);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(36, 36);
            this.btnRight.TabIndex = 135;
            this.btnRight.TabStop = false;
            this.toolTip1.SetToolTip(this.btnRight, "画像右回転");
            this.btnRight.UseVisualStyleBackColor = false;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.BackColor = System.Drawing.SystemColors.Control;
            this.btnLeft.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnLeft.Image")));
            this.btnLeft.Location = new System.Drawing.Point(2, 834);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(36, 36);
            this.btnLeft.TabIndex = 134;
            this.btnLeft.TabStop = false;
            this.toolTip1.SetToolTip(this.btnLeft, "画像左回転");
            this.btnLeft.UseVisualStyleBackColor = false;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(243)))), ((int)(((byte)(190)))));
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnPrint.Location = new System.Drawing.Point(902, 839);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(89, 26);
            this.btnPrint.TabIndex = 6;
            this.btnPrint.Text = "画像印刷(&P)";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnDataMake
            // 
            this.btnDataMake.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(243)))), ((int)(((byte)(190)))));
            this.btnDataMake.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDataMake.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDataMake.Location = new System.Drawing.Point(772, 839);
            this.btnDataMake.Name = "btnDataMake";
            this.btnDataMake.Size = new System.Drawing.Size(124, 26);
            this.btnDataMake.TabIndex = 5;
            this.btnDataMake.Text = "楽商データ作成(&C)";
            this.btnDataMake.UseVisualStyleBackColor = false;
            this.btnDataMake.Click += new System.EventHandler(this.btnDataMake_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(243)))), ((int)(((byte)(190)))));
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button5.Location = new System.Drawing.Point(997, 839);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(76, 26);
            this.button5.TabIndex = 7;
            this.button5.Text = "終了(&E)";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.checkBox1.Location = new System.Drawing.Point(1005, 758);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(68, 18);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "確認済";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // lblMsg
            // 
            this.lblMsg.BackColor = System.Drawing.SystemColors.Window;
            this.lblMsg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMsg.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(601, 788);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(473, 43);
            this.lblMsg.TabIndex = 140;
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(243)))), ((int)(((byte)(190)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button2.Location = new System.Drawing.Point(426, 839);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(169, 26);
            this.button2.TabIndex = 141;
            this.button2.TabStop = false;
            this.button2.Text = "今回処理対象としない(&M)";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(243)))), ((int)(((byte)(190)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button3.Location = new System.Drawing.Point(335, 839);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(85, 26);
            this.button3.TabIndex = 142;
            this.button3.TabStop = false;
            this.button3.Text = "削除(&D)";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtMemo
            // 
            this.txtMemo.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtMemo.Location = new System.Drawing.Point(601, 726);
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Size = new System.Drawing.Size(473, 20);
            this.txtMemo.TabIndex = 3;
            // 
            // gcMultiRow2
            // 
            this.gcMultiRow2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gcMultiRow2.Location = new System.Drawing.Point(601, 3);
            this.gcMultiRow2.Name = "gcMultiRow2";
            this.gcMultiRow2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.gcMultiRow2.Size = new System.Drawing.Size(472, 66);
            this.gcMultiRow2.TabIndex = 0;
            this.gcMultiRow2.Template = this.camTemplate31;
            this.gcMultiRow2.Text = "gcMultiRow2";
            this.gcMultiRow2.CellValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.gcMultiRow2_CellValueChanged);
            this.gcMultiRow2.CellEnter += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.gcMultiRow2_CellEnter);
            this.gcMultiRow2.CellLeave += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.gcMultiRow2_CellLeave);
            this.gcMultiRow2.EditingControlShowing += new System.EventHandler<GrapeCity.Win.MultiRow.EditingControlShowingEventArgs>(this.gcMultiRow2_EditingControlShowing);
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.AllowClipboard = false;
            this.gcMultiRow1.AllowUserToAddRows = false;
            this.gcMultiRow1.AllowUserToDeleteRows = false;
            this.gcMultiRow1.AllowUserToResize = false;
            this.gcMultiRow1.AllowUserToZoom = false;
            this.gcMultiRow1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gcMultiRow1.Location = new System.Drawing.Point(601, 102);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.ScrollBarMode = GrapeCity.Win.MultiRow.ScrollBarMode.Automatic;
            this.gcMultiRow1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.gcMultiRow1.Size = new System.Drawing.Size(473, 620);
            this.gcMultiRow1.TabIndex = 2;
            this.gcMultiRow1.Template = this.camTemplate21;
            this.gcMultiRow1.Text = "gcMultiRow1";
            this.gcMultiRow1.CellValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.gcMultiRow1_CellValueChanged);
            this.gcMultiRow1.CellEnter += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.gcMultiRow1_CellEnter);
            this.gcMultiRow1.CellLeave += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.gcMultiRow1_CellLeave);
            this.gcMultiRow1.EditingControlShowing += new System.EventHandler<GrapeCity.Win.MultiRow.EditingControlShowingEventArgs>(this.gcMultiRow1_EditingControlShowing);
            this.gcMultiRow1.CurrentCellDirtyStateChanged += new System.EventHandler(this.gcMultiRow1_CurrentCellDirtyStateChanged);
            // 
            // frmCanpaignRec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1081, 872);
            this.Controls.Add(this.txtMemo);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnDataMake);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnEnd);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnBefore);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.btnMinus);
            this.Controls.Add(this.btnPlus);
            this.Controls.Add(this.lblNoImage);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gcMultiRow2);
            this.Controls.Add(this.gcMultiRow1);
            this.Controls.Add(this.leadImg);
            this.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCanpaignRec";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "キャンペーン発注データ作成";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCanpaignRec_FormClosing);
            this.Load += new System.EventHandler(this.frmCanpaignRec_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Leadtools.WinForms.RasterImageViewer leadImg;
        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private camTemplate2 camTemplate21;
        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow2;
        private camTemplate3 camTemplate31;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblNoImage;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnBefore;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnDataMake;
        private System.Windows.Forms.Button button5;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtMemo;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}