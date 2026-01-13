namespace BSolution_.Property
{
    partial class BinaryProp
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpBinary = new System.Windows.Forms.GroupBox();
            this.cbHighlight = new System.Windows.Forms.ComboBox();
            this.lbHighlight = new System.Windows.Forms.Label();
            this.chkUse = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbBinMethod = new System.Windows.Forms.ComboBox();
            this.dataGridViewFilter = new System.Windows.Forms.DataGridView();
            this.chkRotatedRect = new System.Windows.Forms.CheckBox();
            this.binRangeTrackbar = new BSolution_.UIControl.RangeTrackbar();
            this.grpBinary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFilter)).BeginInit();
            this.SuspendLayout();
            // 
            // grpBinary
            // 
            this.grpBinary.Controls.Add(this.cbHighlight);
            this.grpBinary.Controls.Add(this.lbHighlight);
            this.grpBinary.Controls.Add(this.binRangeTrackbar);
            this.grpBinary.Location = new System.Drawing.Point(36, 59);
            this.grpBinary.Name = "grpBinary";
            this.grpBinary.Padding = new System.Windows.Forms.Padding(4);
            this.grpBinary.Size = new System.Drawing.Size(332, 221);
            this.grpBinary.TabIndex = 0;
            this.grpBinary.TabStop = false;
            this.grpBinary.Text = "이진화";
            // 
            // cbHighlight
            // 
            this.cbHighlight.FormattingEnabled = true;
            this.cbHighlight.Location = new System.Drawing.Point(143, 142);
            this.cbHighlight.Margin = new System.Windows.Forms.Padding(4);
            this.cbHighlight.Name = "cbHighlight";
            this.cbHighlight.Size = new System.Drawing.Size(151, 26);
            this.cbHighlight.TabIndex = 2;
            this.cbHighlight.SelectedIndexChanged += new System.EventHandler(this.cbHighlight_SelectedIndexChanged);
            // 
            // lbHighlight
            // 
            this.lbHighlight.AutoSize = true;
            this.lbHighlight.Location = new System.Drawing.Point(20, 145);
            this.lbHighlight.Name = "lbHighlight";
            this.lbHighlight.Size = new System.Drawing.Size(98, 18);
            this.lbHighlight.TabIndex = 1;
            this.lbHighlight.Text = "하이라이트";
            // 
            // chkUse
            // 
            this.chkUse.AutoSize = true;
            this.chkUse.Checked = true;
            this.chkUse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUse.Location = new System.Drawing.Point(36, 15);
            this.chkUse.Name = "chkUse";
            this.chkUse.Size = new System.Drawing.Size(70, 22);
            this.chkUse.TabIndex = 1;
            this.chkUse.Text = "검사";
            this.chkUse.UseVisualStyleBackColor = true;
            this.chkUse.CheckedChanged += new System.EventHandler(this.chkUse_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 321);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "검사 타입";
            // 
            // cbBinMethod
            // 
            this.cbBinMethod.FormattingEnabled = true;
            this.cbBinMethod.Location = new System.Drawing.Point(120, 316);
            this.cbBinMethod.Name = "cbBinMethod";
            this.cbBinMethod.Size = new System.Drawing.Size(135, 26);
            this.cbBinMethod.TabIndex = 3;
            this.cbBinMethod.SelectedIndexChanged += new System.EventHandler(this.cbBinMethod_SelectedIndexChanged);
            // 
            // dataGridViewFilter
            // 
            this.dataGridViewFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFilter.Location = new System.Drawing.Point(30, 348);
            this.dataGridViewFilter.Name = "dataGridViewFilter";
            this.dataGridViewFilter.RowHeadersWidth = 62;
            this.dataGridViewFilter.RowTemplate.Height = 30;
            this.dataGridViewFilter.Size = new System.Drawing.Size(347, 213);
            this.dataGridViewFilter.TabIndex = 4;
            this.dataGridViewFilter.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewFilter_CellValueChanged);
            this.dataGridViewFilter.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridViewFilter_CurrentCellDirtyStateChanged);
            // 
            // chkRotatedRect
            // 
            this.chkRotatedRect.AutoSize = true;
            this.chkRotatedRect.Checked = true;
            this.chkRotatedRect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRotatedRect.Location = new System.Drawing.Point(30, 597);
            this.chkRotatedRect.Name = "chkRotatedRect";
            this.chkRotatedRect.Size = new System.Drawing.Size(124, 22);
            this.chkRotatedRect.TabIndex = 5;
            this.chkRotatedRect.Text = "회전사각형";
            this.chkRotatedRect.UseVisualStyleBackColor = true;
            this.chkRotatedRect.CheckedChanged += new System.EventHandler(this.chkRotatedRect_CheckedChanged);
            // 
            // binRangeTrackbar
            // 
            this.binRangeTrackbar.Location = new System.Drawing.Point(7, 55);
            this.binRangeTrackbar.Name = "binRangeTrackbar";
            this.binRangeTrackbar.Size = new System.Drawing.Size(318, 65);
            this.binRangeTrackbar.TabIndex = 0;
            this.binRangeTrackbar.ValueLeft = 100;
            this.binRangeTrackbar.ValueRight = 200;
            this.binRangeTrackbar.Load += new System.EventHandler(this.Range_RangeChanged);
            // 
            // BinaryProp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkRotatedRect);
            this.Controls.Add(this.dataGridViewFilter);
            this.Controls.Add(this.cbBinMethod);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkUse);
            this.Controls.Add(this.grpBinary);
            this.Name = "BinaryProp";
            this.Size = new System.Drawing.Size(422, 690);
            this.grpBinary.ResumeLayout(false);
            this.grpBinary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFilter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBinary;
        private System.Windows.Forms.CheckBox chkUse;
        private System.Windows.Forms.ComboBox cbHighlight;
        private System.Windows.Forms.Label lbHighlight;
        private UIControl.RangeTrackbar binRangeTrackbar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbBinMethod;
        private System.Windows.Forms.DataGridView dataGridViewFilter;
        private System.Windows.Forms.CheckBox chkRotatedRect;
    }
}
