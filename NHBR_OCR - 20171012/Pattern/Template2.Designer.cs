namespace NHBR_OCR.Pattern
{
    [System.ComponentModel.ToolboxItem(true)]
    partial class Template2
    {
        /// <summary> 
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region MultiRow Template Designer generated code

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            GrapeCity.Win.MultiRow.CellStyle cellStyle1 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border1 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle2 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border2 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle3 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border3 = new GrapeCity.Win.MultiRow.Border();
            this.columnHeaderSection1 = new GrapeCity.Win.MultiRow.ColumnHeaderSection();
            this.labelCell2 = new GrapeCity.Win.MultiRow.LabelCell();
            this.txtTdkNum = new GrapeCity.Win.MultiRow.TextBoxCell();
            this.labelCell3 = new GrapeCity.Win.MultiRow.LabelCell();
            // 
            // Row
            // 
            this.Row.BackColor = System.Drawing.SystemColors.Control;
            this.Row.Cells.Add(this.labelCell2);
            this.Row.Cells.Add(this.txtTdkNum);
            this.Row.Cells.Add(this.labelCell3);
            this.Row.Height = 42;
            // 
            // columnHeaderSection1
            // 
            this.columnHeaderSection1.Height = 1;
            this.columnHeaderSection1.Name = "columnHeaderSection1";
            // 
            // labelCell2
            // 
            this.labelCell2.Location = new System.Drawing.Point(2, 2);
            this.labelCell2.Name = "labelCell2";
            this.labelCell2.Size = new System.Drawing.Size(82, 20);
            cellStyle1.BackColor = System.Drawing.Color.PaleGreen;
            border1.Bottom = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Medium, System.Drawing.Color.DimGray);
            border1.Left = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Medium, System.Drawing.Color.DimGray);
            border1.Right = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DimGray);
            border1.Top = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Medium, System.Drawing.Color.DimGray);
            cellStyle1.Border = border1;
            cellStyle1.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F);
            cellStyle1.ImeMode = System.Windows.Forms.ImeMode.Off;
            cellStyle1.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            this.labelCell2.Style = cellStyle1;
            this.labelCell2.TabIndex = 2;
            this.labelCell2.TabStop = false;
            this.labelCell2.Value = "届先番号";
            // 
            // txtTdkNum
            // 
            this.txtTdkNum.Location = new System.Drawing.Point(84, 2);
            this.txtTdkNum.MaxLength = 6;
            this.txtTdkNum.Name = "txtTdkNum";
            this.txtTdkNum.Size = new System.Drawing.Size(67, 20);
            cellStyle2.BackColor = System.Drawing.Color.White;
            border2.Bottom = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Medium, System.Drawing.Color.DimGray);
            border2.Left = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DimGray);
            border2.Right = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Medium, System.Drawing.Color.DimGray);
            border2.Top = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Medium, System.Drawing.Color.DimGray);
            cellStyle2.Border = border2;
            cellStyle2.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle2.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            this.txtTdkNum.Style = cellStyle2;
            this.txtTdkNum.TabIndex = 0;
            // 
            // labelCell3
            // 
            this.labelCell3.Location = new System.Drawing.Point(2, 22);
            this.labelCell3.Name = "labelCell3";
            this.labelCell3.Selectable = false;
            this.labelCell3.Size = new System.Drawing.Size(230, 20);
            cellStyle3.BackColor = System.Drawing.Color.White;
            border3.Bottom = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Medium, System.Drawing.Color.DimGray);
            border3.Left = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Medium, System.Drawing.Color.DimGray);
            border3.Right = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Medium, System.Drawing.Color.DimGray);
            border3.Top = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Medium, System.Drawing.Color.DimGray);
            cellStyle3.Border = border3;
            cellStyle3.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle3.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.labelCell3.Style = cellStyle3;
            this.labelCell3.TabIndex = 4;
            // 
            // Template2
            // 
            this.ColumnHeaders.AddRange(new GrapeCity.Win.MultiRow.ColumnHeaderSection[] {
            this.columnHeaderSection1});
            this.Width = 232;

        }

        #endregion

        private GrapeCity.Win.MultiRow.ColumnHeaderSection columnHeaderSection1;
        private GrapeCity.Win.MultiRow.LabelCell labelCell2;
        private GrapeCity.Win.MultiRow.TextBoxCell txtTdkNum;
        private GrapeCity.Win.MultiRow.LabelCell labelCell3;
    }
}
