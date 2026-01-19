using BSolution_.Sequence;
using BSolution_.Setting;
using BSolution_.Util;
using JidamVision4.Sequence;
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
    public partial class CommunicatorSetting : UserControl
    {
        public CommunicatorSetting()
        {
            InitializeComponent();

            LoadSetting();
        }

        private void LoadSetting()
        {
            cbCommType.DataSource = Enum.GetValues(typeof(CommunicatorType)).Cast<CommunicatorType>().ToList();

            txtMachine.Text = SettingXml.Inst.MachineName;
            cbCommType.SelectedIndex = (int)SettingXml.Inst.CommType;

            txtIpAddr.Text = SettingXml.Inst.CommIP;
        }

        private void SaveSetting()
        {
            SettingXml.Inst.MachineName = txtMachine.Text;

            SettingXml.Inst.CommIP = txtIpAddr.Text;

            SettingXml.Save();

            Slogger.Write($"통신 설정 저장");
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveSetting();
        }
    }
}
