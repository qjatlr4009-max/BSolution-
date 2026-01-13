using BSolution_.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using OpenCvSharp;
using BSolution_.Algorithm;
using System.Security.Cryptography.X509Certificates;
using BSolution_.UIControl;
using BSolution_.Teach;

namespace BSolution_
{
    //public partial class CameraForm : Form
    public partial class CameraForm : DockContent

    {
        public CameraForm()
        {
            InitializeComponent();

            imageViewer.DiagramEntityEvent += ImageViewer_DiagramEntityEvent;
        }

        private void ImageViewer_DiagramEntityEvent(object sender, DiagramEntityEventArgs e)
        {
            switch (e.ActionType)
            {
                case EntityActionType.Select:
                    Global.Inst.InspStage.SelectInspWindow(e.InspWindow);
                    imageViewer.Focus();
                    break;
                case EntityActionType.Inspect:
                    UpdateDiagramEntity();
                    Global.Inst.InspStage.TryInspection(e.InspWindow);
                    break;
                case EntityActionType.Add:
                    Global.Inst.InspStage.AddInspWindow(e.WindowType, e.Rect);
                    break;
                case EntityActionType.Copy:
                    Global.Inst.InspStage.AddInspWindow(e.InspWindow, e.OffsetMove);
                    break;
                case EntityActionType.Move:
                    Global.Inst.InspStage.MoveInspWindow(e.InspWindow, e.OffsetMove);
                    break;
                case EntityActionType.Resize:
                    Global.Inst.InspStage.ModifyInspWindow(e.InspWindow, e.Rect);
                    break;
                case EntityActionType.Delete:
                    Global.Inst.InspStage.DelInspWindow(e.InspWindow);
                    break;
                case EntityActionType.DeleteList:
                    Global.Inst.InspStage.DelInspWindow(e.InspWindowList);
                    break;
            }
        }

        public void LoadImage(string filename)
        {
            if (File.Exists(filename) == false)
                return;

            Image bitmap = Bitmap.FromFile(filename);
            imageViewer.LoadBitmap((Bitmap)bitmap);

        }

        public void UpdateDisplay(Bitmap bitmap = null)
        {
            if (bitmap == null)
            {

                bitmap = Global.Inst.InspStage.GetBitmap(0);
                if (bitmap == null)
                    return;
            }

            if (imageViewer != null)
                imageViewer.LoadBitmap(bitmap);

            Mat curImage = Global.Inst.InspStage.GetMat();
            Global.Inst.InspStage.PreView.SetImage(curImage);
        }

        public Mat GetDisplayImage()
        {
            return Global.Inst.InspStage.ImageSpace.GetMat();
        }
        public void UpdateImageViewer()
        {
            imageViewer.Invalidate();
        }

        public void UpdateDiagramEntity()
        {
            imageViewer.ResetEntity();

            Model model = Global.Inst.InspStage.CurModel;
            List<DiagramEntity> diagramEntityList = new List<DiagramEntity>();

            foreach (InspWindow window in model.InspWindowList)
            {
                if (window is null)
                    continue;

                DiagramEntity entity = new DiagramEntity()
                {
                    LinkedWindow = window,
                    EntityROI = new Rectangle(
                            window.WindowArea.X, window.WindowArea.Y,
                            window.WindowArea.Width, window.WindowArea.Height),
                    EntityColor = imageViewer.GetWindowColor(window.InspWindowType),
                    IsHold = window.IsTeach
                };
                diagramEntityList.Add(entity);
            }

            imageViewer.SetDiagramEntityList(diagramEntityList);
        }

        public void SelectDiagramEntity(InspWindow window)
        {
            imageViewer.SelectDiagramEntity(window);
        }
        public void ResetDisplay()
        {
            imageViewer.ResetEntity();
        }

        public void AddRect(List<DrawInspectInfo> rectInfos)
        {
            imageViewer.AddRect(rectInfos);
        }

        public void AddRoi(InspWindowType inspWindowType)
        {
            imageViewer.NewRoi(inspWindowType);
        }
    }
}

        //private void CameraForm_Resize(object sender, EventArgs e)
        //{
        //    int margin = 0;

        //    imageViewer.Width = this.Width - margin * 2;
        //    imageViewer.Height = this.Height - margin * 2;

        //    imageViewer.Location = new Point(margin, margin);
        //}

