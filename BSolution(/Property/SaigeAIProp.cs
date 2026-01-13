using BSolution_.Core;
using BSolution_.Inspect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace BSolution_.Property
{
    public partial class SaigeAIProp : UserControl
    {
        SaigeAI _saigeAI = new SaigeAI();
        string _txtPath = string.Empty;
        AIEngineType _engineType;



        public SaigeAIProp()
        {
            InitializeComponent();

            cbAIModelType.DataSource = Enum.GetValues(typeof(AIEngineType));
            cbAIModelType.SelectedIndex = 0;
        }

        private void btn_AISelect_Click(object sender, EventArgs e)
        {
            string filter = "AI Model Files|*.*";

            switch (_engineType)
            {
                case AIEngineType.AnomalyDetection:
                    filter = "Anommaly Detection Model Files|*.saigeiad;";
                    break;
                case AIEngineType.Segmentation:
                    filter = "Segmentation Model Files|*.saigeseg;";
                    break;
                case AIEngineType.Detection:
                    filter = "Detection Model Files|*.saigedet"; ;
                    break;
            }
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {

                openFileDialog.Filter = filter;
                openFileDialog.Title = "AI 모델 선택";
                openFileDialog.InitialDirectory = @"C:\Saige\SaigeVision\engine\Examples\data\sfaw2023\models";
                openFileDialog.Multiselect = false;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _txtPath = openFileDialog.FileName;
                    txtPath.Text = _txtPath;
                }
            }
            if (string.IsNullOrEmpty(_txtPath))
            {
                MessageBox.Show("AI 모델을 선택하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_saigeAI == null)
            {
                _saigeAI = Global.Inst.InspStage.AIModule;
            }
        }

        private void btn_Loading_Click(object sender, EventArgs e)
        {
            {
                if (string.IsNullOrEmpty(_txtPath))
                {
                    MessageBox.Show("모델 파일을 선택해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_saigeAI == null)
                {
                    _saigeAI = Global.Inst.InspStage.AIModule;
                }

                _saigeAI.LoadEngine(_txtPath, _engineType);
                MessageBox.Show("모델이 성공적으로 로드되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private void btn_AIInsp_Click(object sender, EventArgs e)
        {
            if (_saigeAI == null)
            {
                MessageBox.Show("AI 모듈이 초기화되지 않았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Bitmap bitmap = Global.Inst.InspStage.GetBitmap();
            if (bitmap == null)
            {
                MessageBox.Show("현재 이미지가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _saigeAI.InspAIModule(bitmap);

            Bitmap resultImage = _saigeAI.GetResultImage();

            Global.Inst.InspStage.UpdateDisplay(resultImage);
        }


        private void cbAIModelType_SelectedIndexChanged(object sender, EventArgs e)
        {
            AIEngineType engineType = (AIEngineType)cbAIModelType.SelectedItem;

            if (engineType != _engineType)
            {
                if (_saigeAI != null)
                    _saigeAI.Dispose();
            }

            _engineType = engineType;
        }
    }
}
