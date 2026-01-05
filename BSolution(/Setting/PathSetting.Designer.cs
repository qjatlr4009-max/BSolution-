namespace BSolution_.Setting
{
    partial class PathSetting
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtModelDir = new System.Windows.Forms.TextBox();
            this.txtImageDir = new System.Windows.Forms.TextBox();
            this.btnSelModelDir = new System.Windows.Forms.Button();
            this.btnSelImageDir = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "모델 경로";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "이미지 경로";
            // 
            // txtModelDir
            // 
            this.txtModelDir.Location = new System.Drawing.Point(147, 22);
            this.txtModelDir.Name = "txtModelDir";
            this.txtModelDir.Size = new System.Drawing.Size(256, 28);
            this.txtModelDir.TabIndex = 2;
            // 
            // txtImageDir
            // 
            this.txtImageDir.Location = new System.Drawing.Point(147, 65);
            this.txtImageDir.Name = "txtImageDir";
            this.txtImageDir.Size = new System.Drawing.Size(256, 28);
            this.txtImageDir.TabIndex = 3;
            // 
            // btnSelModelDir
            // 
            this.btnSelModelDir.Location = new System.Drawing.Point(409, 22);
            this.btnSelModelDir.Name = "btnSelModelDir";
            this.btnSelModelDir.Size = new System.Drawing.Size(59, 29);
            this.btnSelModelDir.TabIndex = 4;
            this.btnSelModelDir.Text = "...";
            this.btnSelModelDir.UseVisualStyleBackColor = true;
            this.btnSelModelDir.Click += new System.EventHandler(this.btnSelModelDir_Click);
            // 
            // btnSelImageDir
            // 
            this.btnSelImageDir.Location = new System.Drawing.Point(409, 65);
            this.btnSelImageDir.Name = "btnSelImageDir";
            this.btnSelImageDir.Size = new System.Drawing.Size(59, 28);
            this.btnSelImageDir.TabIndex = 5;
            this.btnSelImageDir.Text = "...";
            this.btnSelImageDir.UseVisualStyleBackColor = true;
            this.btnSelImageDir.Click += new System.EventHandler(this.btnSelImageDir_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(379, 112);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(89, 33);
            this.btnApply.TabIndex = 6;
            this.btnApply.Text = "적용";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // PathSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnSelImageDir);
            this.Controls.Add(this.btnSelModelDir);
            this.Controls.Add(this.txtImageDir);
            this.Controls.Add(this.txtModelDir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "PathSetting";
            this.Size = new System.Drawing.Size(491, 171);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtModelDir;
        private System.Windows.Forms.TextBox txtImageDir;
        private System.Windows.Forms.Button btnSelModelDir;
        private System.Windows.Forms.Button btnSelImageDir;
        private System.Windows.Forms.Button btnApply;
    }
}
