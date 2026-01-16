namespace BSolution_.Property
{
    partial class MatchInspProp
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
                txtExtendX.Leave -= OnUpdateValue;
                txtExtendY.Leave -= OnUpdateValue;
                txtScore.Leave -= OnUpdateValue;

                patternImageEditor.ButtonChanged -= PatternImage_ButtonChanged;

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
            this.grpMatch = new System.Windows.Forms.GroupBox();
            this.btnApplyAutoTeachRoi = new System.Windows.Forms.Button();
            this.btnClearAutoTeach = new System.Windows.Forms.Button();
            this.btnAutoTeach = new System.Windows.Forms.Button();
            this.chkInvertResult = new System.Windows.Forms.CheckBox();
            this.lbScore = new System.Windows.Forms.Label();
            this.txtExtendY = new System.Windows.Forms.TextBox();
            this.txtScore = new System.Windows.Forms.TextBox();
            this.txtExtendX = new System.Windows.Forms.TextBox();
            this.lbX = new System.Windows.Forms.Label();
            this.lbExtent = new System.Windows.Forms.Label();
            this.chkUse = new System.Windows.Forms.CheckBox();
            this.patternImageEditor = new BSolution_.UIControl.PatternImageEditor();
            this.grpMatch.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpMatch
            // 
            this.grpMatch.Controls.Add(this.btnApplyAutoTeachRoi);
            this.grpMatch.Controls.Add(this.btnClearAutoTeach);
            this.grpMatch.Controls.Add(this.btnAutoTeach);
            this.grpMatch.Controls.Add(this.chkInvertResult);
            this.grpMatch.Controls.Add(this.lbScore);
            this.grpMatch.Controls.Add(this.txtExtendY);
            this.grpMatch.Controls.Add(this.txtScore);
            this.grpMatch.Controls.Add(this.txtExtendX);
            this.grpMatch.Controls.Add(this.lbX);
            this.grpMatch.Controls.Add(this.lbExtent);
            this.grpMatch.Location = new System.Drawing.Point(9, 52);
            this.grpMatch.Margin = new System.Windows.Forms.Padding(4);
            this.grpMatch.Name = "grpMatch";
            this.grpMatch.Padding = new System.Windows.Forms.Padding(4);
            this.grpMatch.Size = new System.Drawing.Size(420, 154);
            this.grpMatch.TabIndex = 5;
            this.grpMatch.TabStop = false;
            // 
            // btnApplyAutoTeachRoi
            // 
            this.btnApplyAutoTeachRoi.Font = new System.Drawing.Font("굴림", 7F);
            this.btnApplyAutoTeachRoi.Location = new System.Drawing.Point(105, 111);
            this.btnApplyAutoTeachRoi.Name = "btnApplyAutoTeachRoi";
            this.btnApplyAutoTeachRoi.Size = new System.Drawing.Size(164, 36);
            this.btnApplyAutoTeachRoi.TabIndex = 6;
            this.btnApplyAutoTeachRoi.Text = "오토티칭 -> ROI 변환";
            this.btnApplyAutoTeachRoi.UseVisualStyleBackColor = true;
            this.btnApplyAutoTeachRoi.Click += new System.EventHandler(this.btnApplyAutoTeachRoi_Click);
            // 
            // btnClearAutoTeach
            // 
            this.btnClearAutoTeach.Location = new System.Drawing.Point(275, 111);
            this.btnClearAutoTeach.Name = "btnClearAutoTeach";
            this.btnClearAutoTeach.Size = new System.Drawing.Size(134, 36);
            this.btnClearAutoTeach.TabIndex = 5;
            this.btnClearAutoTeach.Text = "오토티칭 삭제";
            this.btnClearAutoTeach.UseVisualStyleBackColor = true;
            this.btnClearAutoTeach.Click += new System.EventHandler(this.btnClearAutoTeach_Click);
            // 
            // btnAutoTeach
            // 
            this.btnAutoTeach.Location = new System.Drawing.Point(7, 110);
            this.btnAutoTeach.Name = "btnAutoTeach";
            this.btnAutoTeach.Size = new System.Drawing.Size(92, 36);
            this.btnAutoTeach.TabIndex = 4;
            this.btnAutoTeach.Text = "오토티칭";
            this.btnAutoTeach.UseVisualStyleBackColor = true;
            this.btnAutoTeach.Click += new System.EventHandler(this.btnAutoTeach_Click_1);
            // 
            // chkInvertResult
            // 
            this.chkInvertResult.AutoSize = true;
            this.chkInvertResult.Location = new System.Drawing.Point(229, 73);
            this.chkInvertResult.Margin = new System.Windows.Forms.Padding(4);
            this.chkInvertResult.Name = "chkInvertResult";
            this.chkInvertResult.Size = new System.Drawing.Size(112, 22);
            this.chkInvertResult.TabIndex = 3;
            this.chkInvertResult.Text = "결과 반전";
            this.chkInvertResult.UseVisualStyleBackColor = true;
            this.chkInvertResult.CheckedChanged += new System.EventHandler(this.chkInvertResult_CheckedChanged);
            // 
            // lbScore
            // 
            this.lbScore.AutoSize = true;
            this.lbScore.Location = new System.Drawing.Point(9, 75);
            this.lbScore.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbScore.Name = "lbScore";
            this.lbScore.Size = new System.Drawing.Size(98, 18);
            this.lbScore.TabIndex = 2;
            this.lbScore.Text = "매칭스코어";
            // 
            // txtExtendY
            // 
            this.txtExtendY.Location = new System.Drawing.Point(229, 26);
            this.txtExtendY.Margin = new System.Windows.Forms.Padding(4);
            this.txtExtendY.Name = "txtExtendY";
            this.txtExtendY.Size = new System.Drawing.Size(70, 28);
            this.txtExtendY.TabIndex = 1;
            this.txtExtendY.Text = "500";
            // 
            // txtScore
            // 
            this.txtScore.Location = new System.Drawing.Point(123, 70);
            this.txtScore.Margin = new System.Windows.Forms.Padding(4);
            this.txtScore.Name = "txtScore";
            this.txtScore.Size = new System.Drawing.Size(70, 28);
            this.txtScore.TabIndex = 1;
            this.txtScore.Text = "70";
            // 
            // txtExtendX
            // 
            this.txtExtendX.Location = new System.Drawing.Point(123, 26);
            this.txtExtendX.Margin = new System.Windows.Forms.Padding(4);
            this.txtExtendX.Name = "txtExtendX";
            this.txtExtendX.Size = new System.Drawing.Size(70, 28);
            this.txtExtendX.TabIndex = 1;
            this.txtExtendX.Text = "500";
            // 
            // lbX
            // 
            this.lbX.AutoSize = true;
            this.lbX.Location = new System.Drawing.Point(203, 34);
            this.lbX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbX.Name = "lbX";
            this.lbX.Size = new System.Drawing.Size(18, 18);
            this.lbX.TabIndex = 0;
            this.lbX.Text = "x";
            // 
            // lbExtent
            // 
            this.lbExtent.AutoSize = true;
            this.lbExtent.Location = new System.Drawing.Point(9, 39);
            this.lbExtent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbExtent.Name = "lbExtent";
            this.lbExtent.Size = new System.Drawing.Size(80, 18);
            this.lbExtent.TabIndex = 0;
            this.lbExtent.Text = "확장영역";
            // 
            // chkUse
            // 
            this.chkUse.AutoSize = true;
            this.chkUse.Checked = true;
            this.chkUse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUse.Location = new System.Drawing.Point(20, 20);
            this.chkUse.Margin = new System.Windows.Forms.Padding(4);
            this.chkUse.Name = "chkUse";
            this.chkUse.Size = new System.Drawing.Size(70, 22);
            this.chkUse.TabIndex = 6;
            this.chkUse.Text = "검사";
            this.chkUse.UseVisualStyleBackColor = true;
            this.chkUse.CheckedChanged += new System.EventHandler(this.chkUse_CheckedChanged);
            // 
            // patternImageEditor
            // 
            this.patternImageEditor.Location = new System.Drawing.Point(9, 216);
            this.patternImageEditor.Margin = new System.Windows.Forms.Padding(6);
            this.patternImageEditor.Name = "patternImageEditor";
            this.patternImageEditor.Size = new System.Drawing.Size(424, 242);
            this.patternImageEditor.TabIndex = 4;
            // 
            // MatchInspProp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.patternImageEditor);
            this.Controls.Add(this.grpMatch);
            this.Controls.Add(this.chkUse);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MatchInspProp";
            this.Size = new System.Drawing.Size(446, 472);
            this.grpMatch.ResumeLayout(false);
            this.grpMatch.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpMatch;
        private System.Windows.Forms.CheckBox chkInvertResult;
        private System.Windows.Forms.Label lbScore;
        private System.Windows.Forms.TextBox txtExtendY;
        private System.Windows.Forms.TextBox txtScore;
        private System.Windows.Forms.TextBox txtExtendX;
        private System.Windows.Forms.Label lbX;
        private System.Windows.Forms.Label lbExtent;
        private System.Windows.Forms.CheckBox chkUse;
        private UIControl.PatternImageEditor patternImageEditor;
        private System.Windows.Forms.Button btnClearAutoTeach;
        private System.Windows.Forms.Button btnAutoTeach;
        private System.Windows.Forms.Button btnApplyAutoTeachRoi;
    }
}
