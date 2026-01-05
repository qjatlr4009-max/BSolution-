namespace BSolution_.Property
{
    partial class SaigeAIProp
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
            this.cbAIModelType = new System.Windows.Forms.ComboBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btn_AISelect = new System.Windows.Forms.Button();
            this.btn_Loading = new System.Windows.Forms.Button();
            this.btn_AIInsp = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // cbAIModelType
            // 
            this.cbAIModelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAIModelType.FormattingEnabled = true;
            this.cbAIModelType.Location = new System.Drawing.Point(41, 44);
            this.cbAIModelType.Name = "cbAIModelType";
            this.cbAIModelType.Size = new System.Drawing.Size(184, 26);
            this.cbAIModelType.TabIndex = 0;
            this.cbAIModelType.SelectedIndexChanged += new System.EventHandler(this.cbAIModelType_SelectedIndexChanged);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(40, 90);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(184, 28);
            this.txtPath.TabIndex = 1;
            // 
            // btn_AISelect
            // 
            this.btn_AISelect.Location = new System.Drawing.Point(57, 140);
            this.btn_AISelect.Name = "btn_AISelect";
            this.btn_AISelect.Size = new System.Drawing.Size(137, 32);
            this.btn_AISelect.TabIndex = 2;
            this.btn_AISelect.Text = "AI 모델 선택";
            this.btn_AISelect.UseVisualStyleBackColor = true;
            this.btn_AISelect.Click += new System.EventHandler(this.btn_AISelect_Click);
            // 
            // btn_Loading
            // 
            this.btn_Loading.Location = new System.Drawing.Point(57, 194);
            this.btn_Loading.Name = "btn_Loading";
            this.btn_Loading.Size = new System.Drawing.Size(137, 32);
            this.btn_Loading.TabIndex = 3;
            this.btn_Loading.Text = "모델 로딩";
            this.btn_Loading.UseVisualStyleBackColor = true;
            this.btn_Loading.Click += new System.EventHandler(this.btn_Loading_Click);
            // 
            // btn_AIInsp
            // 
            this.btn_AIInsp.Location = new System.Drawing.Point(57, 253);
            this.btn_AIInsp.Name = "btn_AIInsp";
            this.btn_AIInsp.Size = new System.Drawing.Size(137, 32);
            this.btn_AIInsp.TabIndex = 4;
            this.btn_AIInsp.Text = "AI 검사";
            this.btn_AIInsp.UseVisualStyleBackColor = true;
            this.btn_AIInsp.Click += new System.EventHandler(this.btn_AIInsp_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // SaigeAIProp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_AIInsp);
            this.Controls.Add(this.btn_Loading);
            this.Controls.Add(this.btn_AISelect);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.cbAIModelType);
            this.Name = "SaigeAIProp";
            this.Size = new System.Drawing.Size(273, 395);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbAIModelType;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btn_AISelect;
        private System.Windows.Forms.Button btn_Loading;
        private System.Windows.Forms.Button btn_AIInsp;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}
