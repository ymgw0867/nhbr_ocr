namespace NHBR_OCR.OCR
{
    partial class frmCorrect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCorrect));
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblNoImage = new System.Windows.Forms.Label();
            this.leadImg = new Leadtools.WinForms.RasterImageViewer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblErrMsg = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.txtMemo = new System.Windows.Forms.TextBox();
            this.lblCam = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblBikou = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnErrCheck = new System.Windows.Forms.Button();
            this.btnDataMake = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.btnPlus = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnBefore = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnEnd = new System.Windows.Forms.Button();
            this.btnHold = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblGrpG = new System.Windows.Forms.Label();
            this.lblGrpF = new System.Windows.Forms.Label();
            this.lblGrpE = new System.Windows.Forms.Label();
            this.lblGrpD = new System.Windows.Forms.Label();
            this.lblGrpC = new System.Windows.Forms.Label();
            this.lblGrpB = new System.Windows.Forms.Label();
            this.lblGrpA = new System.Windows.Forms.Label();
            this.txtErrStatus = new System.Windows.Forms.TextBox();
            this.gcMultiRow3 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.template111 = new NHBR_OCR.Pattern.Template11();
            this.gcMultiRow2 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.template101 = new NHBR_OCR.Pattern.Template10();
            this.gcMultiRow1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.template091 = new NHBR_OCR.OCR.Template09();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(931, 837);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(226, 24);
            this.hScrollBar1.TabIndex = 13;
            this.toolTip1.SetToolTip(this.hScrollBar1, "出勤簿を移動します");
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // toolTip1
            // 
            this.toolTip1.BackColor = System.Drawing.Color.LemonChiffon;
            // 
            // lblNoImage
            // 
            this.lblNoImage.Font = new System.Drawing.Font("メイリオ", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblNoImage.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.lblNoImage.Location = new System.Drawing.Point(135, 336);
            this.lblNoImage.Name = "lblNoImage";
            this.lblNoImage.Size = new System.Drawing.Size(322, 42);
            this.lblNoImage.TabIndex = 119;
            this.lblNoImage.Text = "画像はありません";
            this.lblNoImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // leadImg
            // 
            this.leadImg.AutoScroll = false;
            this.leadImg.Location = new System.Drawing.Point(5, 4);
            this.leadImg.Name = "leadImg";
            this.leadImg.Size = new System.Drawing.Size(593, 828);
            this.leadImg.TabIndex = 121;
            this.leadImg.MouseLeave += new System.EventHandler(this.leadImg_MouseLeave);
            this.leadImg.MouseMove += new System.Windows.Forms.MouseEventHandler(this.leadImg_MouseMove);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.lblErrMsg);
            this.panel1.Location = new System.Drawing.Point(4, 837);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(594, 44);
            this.panel1.TabIndex = 162;
            // 
            // lblErrMsg
            // 
            this.lblErrMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblErrMsg.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblErrMsg.ForeColor = System.Drawing.Color.Red;
            this.lblErrMsg.Location = new System.Drawing.Point(0, 0);
            this.lblErrMsg.Name = "lblErrMsg";
            this.lblErrMsg.Size = new System.Drawing.Size(590, 40);
            this.lblErrMsg.TabIndex = 0;
            this.lblErrMsg.Text = "label33";
            this.lblErrMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Meiryo UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.checkBox1.Location = new System.Drawing.Point(1160, 839);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(66, 21);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "確認済";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // txtMemo
            // 
            this.txtMemo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMemo.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtMemo.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtMemo.Location = new System.Drawing.Point(652, 862);
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Size = new System.Drawing.Size(575, 19);
            this.txtMemo.TabIndex = 6;
            // 
            // lblCam
            // 
            this.lblCam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCam.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCam.ForeColor = System.Drawing.Color.Red;
            this.lblCam.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblCam.Location = new System.Drawing.Point(719, 839);
            this.lblCam.Name = "lblCam";
            this.lblCam.Size = new System.Drawing.Size(56, 20);
            this.lblCam.TabIndex = 3;
            this.lblCam.Text = "あり";
            this.lblCam.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(243)))), ((int)(((byte)(190)))));
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(604, 839);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 20);
            this.label4.TabIndex = 304;
            this.label4.Text = "キャンペーン申込み";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(243)))), ((int)(((byte)(190)))));
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(781, 839);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 20);
            this.label5.TabIndex = 306;
            this.label5.Text = "備考欄記入";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // lblBikou
            // 
            this.lblBikou.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBikou.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblBikou.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblBikou.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblBikou.Location = new System.Drawing.Point(872, 839);
            this.lblBikou.Name = "lblBikou";
            this.lblBikou.Size = new System.Drawing.Size(56, 20);
            this.lblBikou.TabIndex = 4;
            this.lblBikou.Text = "--";
            this.lblBikou.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(243)))), ((int)(((byte)(190)))));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(604, 862);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 19);
            this.label2.TabIndex = 307;
            this.label2.Text = "メモ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnErrCheck
            // 
            this.btnErrCheck.BackColor = System.Drawing.SystemColors.Control;
            this.btnErrCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnErrCheck.Location = new System.Drawing.Point(762, 887);
            this.btnErrCheck.Name = "btnErrCheck";
            this.btnErrCheck.Size = new System.Drawing.Size(82, 24);
            this.btnErrCheck.TabIndex = 13;
            this.btnErrCheck.Text = "エラーチェック";
            this.btnErrCheck.UseVisualStyleBackColor = false;
            this.btnErrCheck.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnDataMake
            // 
            this.btnDataMake.BackColor = System.Drawing.SystemColors.Control;
            this.btnDataMake.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDataMake.Location = new System.Drawing.Point(844, 887);
            this.btnDataMake.Name = "btnDataMake";
            this.btnDataMake.Size = new System.Drawing.Size(100, 24);
            this.btnDataMake.TabIndex = 14;
            this.btnDataMake.Text = "楽商データ作成";
            this.btnDataMake.UseVisualStyleBackColor = false;
            this.btnDataMake.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.Control;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDelete.Location = new System.Drawing.Point(944, 887);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(82, 24);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "発注書削除";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.Control;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button5.Location = new System.Drawing.Point(1160, 887);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(67, 24);
            this.button5.TabIndex = 18;
            this.button5.Text = "終了";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.Location = new System.Drawing.Point(628, 887);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(67, 24);
            this.btnPlus.TabIndex = 7;
            this.btnPlus.Text = "画像拡大";
            this.btnPlus.UseVisualStyleBackColor = true;
            this.btnPlus.Click += new System.EventHandler(this.button6_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.ForeColor = System.Drawing.Color.SkyBlue;
            this.label1.Location = new System.Drawing.Point(1173, 914);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 15);
            this.label1.TabIndex = 313;
            this.label1.Text = "Ｆ12";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.ForeColor = System.Drawing.Color.SkyBlue;
            this.label3.Location = new System.Drawing.Point(381, 914);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 15);
            this.label3.TabIndex = 314;
            this.label3.Text = "Ｆ１";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.ForeColor = System.Drawing.Color.SkyBlue;
            this.label6.Location = new System.Drawing.Point(716, 914);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 15);
            this.label6.TabIndex = 315;
            this.label6.Text = "Ｆ６";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.ForeColor = System.Drawing.Color.SkyBlue;
            this.label10.Location = new System.Drawing.Point(649, 914);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(28, 15);
            this.label10.TabIndex = 319;
            this.label10.Text = "Ｆ５";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label15.ForeColor = System.Drawing.Color.SkyBlue;
            this.label15.Location = new System.Drawing.Point(1043, 914);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 15);
            this.label15.TabIndex = 324;
            this.label15.Text = "Ｆ10";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label16.ForeColor = System.Drawing.Color.SkyBlue;
            this.label16.Location = new System.Drawing.Point(788, 914);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(28, 15);
            this.label16.TabIndex = 325;
            this.label16.Text = "Ｆ７";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label17.ForeColor = System.Drawing.Color.SkyBlue;
            this.label17.Location = new System.Drawing.Point(446, 914);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(28, 15);
            this.label17.TabIndex = 326;
            this.label17.Text = "Ｆ２";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label18.ForeColor = System.Drawing.Color.SkyBlue;
            this.label18.Location = new System.Drawing.Point(972, 914);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(28, 15);
            this.label18.TabIndex = 327;
            this.label18.Text = "Ｆ９";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label20.ForeColor = System.Drawing.Color.SkyBlue;
            this.label20.Location = new System.Drawing.Point(1108, 914);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(35, 15);
            this.label20.TabIndex = 329;
            this.label20.Text = "Ｆ11";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label21.ForeColor = System.Drawing.Color.SkyBlue;
            this.label21.Location = new System.Drawing.Point(512, 914);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(28, 15);
            this.label21.TabIndex = 330;
            this.label21.Text = "Ｆ３";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label22.ForeColor = System.Drawing.Color.SkyBlue;
            this.label22.Location = new System.Drawing.Point(882, 914);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(28, 15);
            this.label22.TabIndex = 331;
            this.label22.Text = "Ｆ８";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label23.ForeColor = System.Drawing.Color.SkyBlue;
            this.label23.Location = new System.Drawing.Point(581, 914);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(28, 15);
            this.label23.TabIndex = 332;
            this.label23.Text = "Ｆ４";
            // 
            // btnMinus
            // 
            this.btnMinus.Location = new System.Drawing.Point(695, 887);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(67, 24);
            this.btnMinus.TabIndex = 8;
            this.btnMinus.Text = "画像縮小";
            this.btnMinus.UseVisualStyleBackColor = true;
            this.btnMinus.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // btnFirst
            // 
            this.btnFirst.Location = new System.Drawing.Point(360, 887);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(67, 24);
            this.btnFirst.TabIndex = 9;
            this.btnFirst.Text = "先頭データ";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // btnBefore
            // 
            this.btnBefore.Location = new System.Drawing.Point(427, 887);
            this.btnBefore.Name = "btnBefore";
            this.btnBefore.Size = new System.Drawing.Size(67, 24);
            this.btnBefore.TabIndex = 10;
            this.btnBefore.Text = "前データ";
            this.btnBefore.UseVisualStyleBackColor = true;
            this.btnBefore.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(494, 887);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(67, 24);
            this.btnNext.TabIndex = 11;
            this.btnNext.Text = "次データ";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.button7_Click);
            // 
            // btnEnd
            // 
            this.btnEnd.Location = new System.Drawing.Point(561, 887);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(67, 24);
            this.btnEnd.TabIndex = 12;
            this.btnEnd.Text = "最終データ";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.button8_Click);
            // 
            // btnHold
            // 
            this.btnHold.BackColor = System.Drawing.SystemColors.Control;
            this.btnHold.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnHold.Location = new System.Drawing.Point(1026, 887);
            this.btnHold.Name = "btnHold";
            this.btnHold.Size = new System.Drawing.Size(67, 24);
            this.btnHold.TabIndex = 16;
            this.btnHold.Text = "保留";
            this.btnHold.UseVisualStyleBackColor = false;
            this.btnHold.Click += new System.EventHandler(this.button11_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.SystemColors.Control;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrint.Location = new System.Drawing.Point(1093, 887);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(67, 24);
            this.btnPrint.TabIndex = 17;
            this.btnPrint.Text = "画像印刷";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.button9_Click);
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(243)))), ((int)(((byte)(190)))));
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(97, 887);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 19);
            this.label7.TabIndex = 343;
            this.label7.Text = "Ａ";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(243)))), ((int)(((byte)(190)))));
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(132, 887);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 19);
            this.label8.TabIndex = 344;
            this.label8.Text = "Ｂ";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(243)))), ((int)(((byte)(190)))));
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.Location = new System.Drawing.Point(202, 887);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 19);
            this.label9.TabIndex = 346;
            this.label9.Text = "Ｄ";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(243)))), ((int)(((byte)(190)))));
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label11.Location = new System.Drawing.Point(167, 887);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 19);
            this.label11.TabIndex = 345;
            this.label11.Text = "Ｃ";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(243)))), ((int)(((byte)(190)))));
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label12.Location = new System.Drawing.Point(272, 887);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(36, 19);
            this.label12.TabIndex = 348;
            this.label12.Text = "Ｆ";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(243)))), ((int)(((byte)(190)))));
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label13.Location = new System.Drawing.Point(237, 887);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(36, 19);
            this.label13.TabIndex = 347;
            this.label13.Text = "Ｅ";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(243)))), ((int)(((byte)(190)))));
            this.label19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label19.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label19.Location = new System.Drawing.Point(307, 887);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(36, 19);
            this.label19.TabIndex = 349;
            this.label19.Text = "Ｇ";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(243)))), ((int)(((byte)(190)))));
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label14.Location = new System.Drawing.Point(4, 887);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(94, 36);
            this.label14.TabIndex = 350;
            this.label14.Text = "出荷基準判定";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGrpG
            // 
            this.lblGrpG.BackColor = System.Drawing.Color.White;
            this.lblGrpG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGrpG.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblGrpG.Location = new System.Drawing.Point(307, 904);
            this.lblGrpG.Name = "lblGrpG";
            this.lblGrpG.Size = new System.Drawing.Size(36, 19);
            this.lblGrpG.TabIndex = 357;
            this.lblGrpG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGrpF
            // 
            this.lblGrpF.BackColor = System.Drawing.Color.White;
            this.lblGrpF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGrpF.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblGrpF.Location = new System.Drawing.Point(272, 904);
            this.lblGrpF.Name = "lblGrpF";
            this.lblGrpF.Size = new System.Drawing.Size(36, 19);
            this.lblGrpF.TabIndex = 356;
            this.lblGrpF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGrpE
            // 
            this.lblGrpE.BackColor = System.Drawing.Color.White;
            this.lblGrpE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGrpE.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblGrpE.Location = new System.Drawing.Point(237, 904);
            this.lblGrpE.Name = "lblGrpE";
            this.lblGrpE.Size = new System.Drawing.Size(36, 19);
            this.lblGrpE.TabIndex = 355;
            this.lblGrpE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGrpD
            // 
            this.lblGrpD.BackColor = System.Drawing.Color.White;
            this.lblGrpD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGrpD.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblGrpD.Location = new System.Drawing.Point(202, 904);
            this.lblGrpD.Name = "lblGrpD";
            this.lblGrpD.Size = new System.Drawing.Size(36, 19);
            this.lblGrpD.TabIndex = 354;
            this.lblGrpD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGrpC
            // 
            this.lblGrpC.BackColor = System.Drawing.Color.White;
            this.lblGrpC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGrpC.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblGrpC.Location = new System.Drawing.Point(167, 904);
            this.lblGrpC.Name = "lblGrpC";
            this.lblGrpC.Size = new System.Drawing.Size(36, 19);
            this.lblGrpC.TabIndex = 353;
            this.lblGrpC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGrpB
            // 
            this.lblGrpB.BackColor = System.Drawing.Color.White;
            this.lblGrpB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGrpB.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblGrpB.Location = new System.Drawing.Point(132, 904);
            this.lblGrpB.Name = "lblGrpB";
            this.lblGrpB.Size = new System.Drawing.Size(36, 19);
            this.lblGrpB.TabIndex = 352;
            this.lblGrpB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGrpA
            // 
            this.lblGrpA.BackColor = System.Drawing.Color.White;
            this.lblGrpA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGrpA.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblGrpA.Location = new System.Drawing.Point(97, 904);
            this.lblGrpA.Name = "lblGrpA";
            this.lblGrpA.Size = new System.Drawing.Size(36, 19);
            this.lblGrpA.TabIndex = 351;
            this.lblGrpA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtErrStatus
            // 
            this.txtErrStatus.Location = new System.Drawing.Point(549, 817);
            this.txtErrStatus.Name = "txtErrStatus";
            this.txtErrStatus.ReadOnly = true;
            this.txtErrStatus.Size = new System.Drawing.Size(23, 19);
            this.txtErrStatus.TabIndex = 358;
            this.txtErrStatus.Visible = false;
            // 
            // gcMultiRow3
            // 
            this.gcMultiRow3.AllowUserToAddRows = false;
            this.gcMultiRow3.AllowUserToDeleteRows = false;
            this.gcMultiRow3.AllowUserToResize = false;
            this.gcMultiRow3.AllowUserToZoom = false;
            this.gcMultiRow3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gcMultiRow3.EditMode = GrapeCity.Win.MultiRow.EditMode.EditProgrammatically;
            this.gcMultiRow3.HideSelection = true;
            this.gcMultiRow3.Location = new System.Drawing.Point(604, 711);
            this.gcMultiRow3.Name = "gcMultiRow3";
            this.gcMultiRow3.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.gcMultiRow3.Size = new System.Drawing.Size(623, 121);
            this.gcMultiRow3.TabIndex = 2;
            this.gcMultiRow3.Template = this.template111;
            this.gcMultiRow3.Text = "gcMultiRow3";
            this.gcMultiRow3.CellValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.gcMultiRow3_CellValueChanged);
            this.gcMultiRow3.CellEnter += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.gcMultiRow3_CellEnter);
            this.gcMultiRow3.EditingControlShowing += new System.EventHandler<GrapeCity.Win.MultiRow.EditingControlShowingEventArgs>(this.gcMultiRow3_EditingControlShowing);
            this.gcMultiRow3.Leave += new System.EventHandler(this.gcMultiRow3_Leave);
            // 
            // gcMultiRow2
            // 
            this.gcMultiRow2.AllowUserToAddRows = false;
            this.gcMultiRow2.AllowUserToDeleteRows = false;
            this.gcMultiRow2.AllowUserToResize = false;
            this.gcMultiRow2.AllowUserToZoom = false;
            this.gcMultiRow2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gcMultiRow2.EditMode = GrapeCity.Win.MultiRow.EditMode.EditProgrammatically;
            this.gcMultiRow2.HideSelection = true;
            this.gcMultiRow2.Location = new System.Drawing.Point(604, 67);
            this.gcMultiRow2.Name = "gcMultiRow2";
            this.gcMultiRow2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.gcMultiRow2.ScrollBarStyle = System.Windows.Forms.FlatStyle.Popup;
            this.gcMultiRow2.Size = new System.Drawing.Size(622, 640);
            this.gcMultiRow2.TabIndex = 1;
            this.gcMultiRow2.Template = this.template101;
            this.gcMultiRow2.Text = "gcMultiRow2";
            this.gcMultiRow2.CellValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.gcMultiRow2_CellValueChanged);
            this.gcMultiRow2.CellEnter += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.gcMultiRow2_CellEnter);
            this.gcMultiRow2.EditingControlShowing += new System.EventHandler<GrapeCity.Win.MultiRow.EditingControlShowingEventArgs>(this.gcMultiRow2_EditingControlShowing);
            this.gcMultiRow2.Leave += new System.EventHandler(this.gcMultiRow2_Leave);
            // 
            // gcMultiRow1
            // 
            this.gcMultiRow1.AllowUserToAddRows = false;
            this.gcMultiRow1.AllowUserToDeleteRows = false;
            this.gcMultiRow1.AllowUserToResize = false;
            this.gcMultiRow1.AllowUserToZoom = false;
            this.gcMultiRow1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gcMultiRow1.EditMode = GrapeCity.Win.MultiRow.EditMode.EditProgrammatically;
            this.gcMultiRow1.HideSelection = true;
            this.gcMultiRow1.Location = new System.Drawing.Point(604, 4);
            this.gcMultiRow1.Name = "gcMultiRow1";
            this.gcMultiRow1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.gcMultiRow1.Size = new System.Drawing.Size(620, 59);
            this.gcMultiRow1.TabIndex = 0;
            this.gcMultiRow1.Template = this.template091;
            this.gcMultiRow1.Text = "gcMultiRow1";
            this.gcMultiRow1.CellValueChanged += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.gcMultiRow1_CellValueChanged);
            this.gcMultiRow1.CellEnter += new System.EventHandler<GrapeCity.Win.MultiRow.CellEventArgs>(this.gcMultiRow1_CellEnter);
            this.gcMultiRow1.EditingControlShowing += new System.EventHandler<GrapeCity.Win.MultiRow.EditingControlShowingEventArgs>(this.gcMultiRow1_EditingControlShowing);
            this.gcMultiRow1.Leave += new System.EventHandler(this.gcMultiRow1_Leave);
            // 
            // frmCorrect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 932);
            this.Controls.Add(this.txtErrStatus);
            this.Controls.Add(this.lblGrpG);
            this.Controls.Add(this.lblGrpF);
            this.Controls.Add(this.lblGrpE);
            this.Controls.Add(this.lblGrpD);
            this.Controls.Add(this.lblGrpC);
            this.Controls.Add(this.lblGrpB);
            this.Controls.Add(this.lblGrpA);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnHold);
            this.Controls.Add(this.btnEnd);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnBefore);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.btnMinus);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPlus);
            this.Controls.Add(this.btnErrCheck);
            this.Controls.Add(this.btnDataMake);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblBikou);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblCam);
            this.Controls.Add(this.txtMemo);
            this.Controls.Add(this.gcMultiRow3);
            this.Controls.Add(this.gcMultiRow2);
            this.Controls.Add(this.gcMultiRow1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.lblNoImage);
            this.Controls.Add(this.leadImg);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmCorrect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FAX発注書データ作成";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCorrect_FormClosing);
            this.Load += new System.EventHandler(this.frmCorrect_Load);
            this.Shown += new System.EventHandler(this.frmCorrect_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCorrect_KeyDown);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblNoImage;
        private Leadtools.WinForms.RasterImageViewer leadImg;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblErrMsg;
        //private Template1 template11;
        //private Template2 template21;
        private System.Windows.Forms.CheckBox checkBox1;
        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow1;
        private Template09 template091;
        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow2;
        private Pattern.Template10 template101;
        private GrapeCity.Win.MultiRow.GcMultiRow gcMultiRow3;
        private Pattern.Template11 template111;
        private System.Windows.Forms.TextBox txtMemo;
        private System.Windows.Forms.Label lblCam;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblBikou;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnErrCheck;
        private System.Windows.Forms.Button btnDataMake;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnBefore;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Button btnHold;
        private System.Windows.Forms.Button btnPrint;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblGrpG;
        private System.Windows.Forms.Label lblGrpF;
        private System.Windows.Forms.Label lblGrpE;
        private System.Windows.Forms.Label lblGrpD;
        private System.Windows.Forms.Label lblGrpC;
        private System.Windows.Forms.Label lblGrpB;
        private System.Windows.Forms.Label lblGrpA;
        private System.Windows.Forms.TextBox txtErrStatus;
    }
}