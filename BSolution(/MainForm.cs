using BSolution_.Core;
using BSolution_.Setting;
using System;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using BSolution_.Teach;
using BSolution_.Util;

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

            Global.Inst.InspStage.Initialize();

            LoadSetting();
        }

        public static T GetDockForm<T>() where T : DockContent
        {
            var findForm = _dockPanel.Contents
                .OfType<T>()
                .FirstOrDefault();
            return findForm;
        }

        private void imageOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraForm cameraForm = GetDockForm<CameraForm>();
            if (cameraForm is null) return;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "이미지 파일 선택";
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff|All Files|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    cameraForm.LoadImage(openFileDialog.FileName);

                    Global.Inst.InspStage.SetImageBuffer(filePath);
                    Global.Inst.InspStage.CurModel.InspectImagePath = filePath;
                }
            }
        }

        private void LoadDockingWindows()
        {
            _dockPanel.AllowEndUserDocking = false;

            var cameraWindow = new CameraForm();
            cameraWindow.Show(_dockPanel, DockState.Document);

            var resultWindow = new LogForm();
            resultWindow.Show(cameraWindow.Pane, DockAlignment.Bottom, 0.3);

            var modelTreeWindow = new ModelTreeForm();
            modelTreeWindow.Show(resultWindow.Pane, DockAlignment.Right, 0.3);

            var runWindow = new RunForm();
            runWindow.Show(modelTreeWindow.Pane, null);

            var propWindow = new PropertiesForm();
            propWindow.Show(_dockPanel, DockState.DockRight);

            var logWindow = new ResultForm();
            logWindow.Show(propWindow.Pane, DockAlignment.Bottom,0.3);
        }

        private void LoadSetting()
        {
            cycleModeMenuItem.Checked = SettingXml.Inst.CycleMode;
        }

        private void imageSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraForm cameraForm = GetDockForm<CameraForm>();
            if (cameraForm is null) return;
        }

        private void setupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Slogger.Write($"환경설정창 열기");
            SetupForm setupForm = new SetupForm();
            setupForm.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Global.Inst.Dispose();
        }

        private string GetModelTitle(Model curModel)
        {
            if (curModel is null)
                return "";

            string modelName = curModel.ModelName;
            return $"{Define.PPOGRAM_NAME} - MODEL : {modelName}";
        }

        private void modelNewMenuItem_Click(object sender, EventArgs e)
        {
            NewModel newModel = new NewModel();
            newModel.ShowDialog();

            Model curModel = Global.Inst.InspStage.CurModel;
            if (curModel != null)
            {
                this.Text = GetModelTitle(curModel);
            }
        }

        private void modelOpenMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "모델 파일 선택";
                openFileDialog.Filter = "Model Files|*.xml;";
                openFileDialog.Multiselect = false;
                openFileDialog.InitialDirectory = SettingXml.Inst.ModelDir;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    if (Global.Inst.InspStage.LoadModel(filePath))
                    {
                        Model curModel = Global.Inst.InspStage.CurModel;
                        if (curModel != null)
                        {
                            this.Text = GetModelTitle(curModel);
                        }
                    }
                }
            }
        }

        private void modelSaveMenuItem_Click(object sender, EventArgs e)
        {
            Global.Inst.InspStage.SaveModel("");
        }

        private void modelSaveAsMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = SettingXml.Inst.ModelDir;
                saveFileDialog.Title = "모델 파일 선택";
                saveFileDialog.Filter = "Model Files|*.xml;";
                saveFileDialog.DefaultExt = "xml";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    Global.Inst.InspStage.SaveModel(filePath);
                }
            }
        }

        private void cycleModeMenuItem_Click(object sender, EventArgs e)
        {
            bool isChecked = cycleModeMenuItem.Checked;
            SettingXml.Inst.CycleMode = isChecked;
        }
    }
}
