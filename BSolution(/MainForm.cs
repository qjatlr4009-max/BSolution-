using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using WeifenLuo.WinFormsUI.Docking;

namespace BSolution_
{
    public partial class MainForm : Form
    {
        private static DockPanel _dockPanel;
        public MainForm()
        {
            InitializeComponent();

            _dockPanel = new DockPanel
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(_dockPanel);


            _dockPanel.Theme = new VS2015DarkTheme();

            LoadDockingWindows();
        }

        public static T GetDockForm<T>() where T : DockContent
        {
            var findForm = _dockPanel.Contents
                .OfType<T>()
                .FirstOrDefault();
            return findForm;
        }

        private void LoadDockingWindows()

        {
            CameraForm cameraForm = new CameraForm();
            cameraForm.Show(_dockPanel, DockState.Document);

            ResultForm resultForm = new ResultForm();
            resultForm.Show(cameraForm.Pane, DockAlignment.Bottom, 0.3);

            var propForm = new PropertiesForm();
            propForm.Show(_dockPanel, DockState.DockRight);

            var StatisticForm = new StatisticForm();
            StatisticForm.Show(_dockPanel, DockState.DockRight);


            var LogForm = new LogForm();
            LogForm.Show(propForm.Pane, DockAlignment.Bottom, 0.5);
        }


        private void imageSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraForm cameraForm = GetDockForm<CameraForm>();
            if (cameraForm is null) return;
        

        using(OpenFileDialog openFileDialog = new OpenFileDialog())
        {
                openFileDialog.Title = "이미지 파일 선택";
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff|All Files|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    cameraForm.LoadImage(openFileDialog.FileName);
                }
            }
        }
    }
}
