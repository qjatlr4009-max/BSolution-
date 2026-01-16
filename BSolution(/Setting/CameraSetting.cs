using BSolution_.Grab;
using BSolution_.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BSolution_.Setting
{
    public partial class CameraSetting : UserControl
    {
        public CameraSetting()
        {
            InitializeComponent();

            LoadSetting();
        }

        private void LoadSetting()
        {
            cbCameraType.DataSource = Enum.GetValues(typeof(CameraType)).Cast<CameraType>().ToList();

            cbCameraType.SelectedItem = (int)SettingXml.Inst.CamType;
        }

        private void SaveSetting()
        {
            SettingXml.Inst.CamType = (CameraType)cbCameraType.SelectedIndex;
            
            SettingXml.Save();

            Slogger.Write($"카메라 설정 저장");
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveSetting();
        }
    }
}
