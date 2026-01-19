using BSolution_.Algorithm;
using BSolution_.Grab;
using BSolution_.Inspect;
using BSolution_.Sequence;
using BSolution_.Setting;
using BSolution_.Teach;
using BSolution_.Util;
using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BSolution_.Core
{
    public class InspStage : IDisposable
    {
        public static readonly int MAX_GRAB_BUF = 5;

        private ImageSpace _imageSpace = null;
        private GrabModel _grabManager = null;
        private CameraType _camType = CameraType.WebCam;

        Inspect.SaigeAI _saigeAI; // SaigeAI 인스턴스

        //BlobAlgorithm _blobAlgorithm = null;
        private PreviewImage _previewImage = null;

        private Model _model = null;

        private InspWindow _selectedInspWindow = null;

        private InspWorker _inspWorker = null;
        private ImageLoader _imageLoader = null;

        RegistryKey _regKey = null;
        private bool _lastestModelOpen = false;

        public bool UseCamera {  get; set; } = false;

        private string _lotNumber;
        private string _serialID;

        private bool _isInspectMode = false;

        public InspStage() { }
        public ImageSpace ImageSpace
        {
            get => _imageSpace;
        }

        public InspWindow SeletedInspWindow
        { get { return _selectedInspWindow; } }

        public SaigeAI AIModule
        {
            get
            {
                if (_saigeAI is null)
                    _saigeAI = new SaigeAI();
                return _saigeAI;
            }
        }

        public InspWindow SelectedInspWindow
        {
            get => _selectedInspWindow;
        }

        //public BlobAlgorithm BlobAlgorithm
        //{
        //    get => _blobAlgorithm;
        //}

        public PreviewImage PreView
        {
            get => _previewImage;
        }

        public InspWorker InspWorker
        {
            get => _inspWorker;
        }

        public Model CurModel
        {
            get => _model;
        }

        public bool LiveMode { get; set; } = false;

        public int SelBufferIndex { get; set; } = 0;

        public eImageChannel SelImageChannel { get; set; } = eImageChannel.Gray;

        public bool Initialize()
        {
            Slogger.Write("InspStage 초기화");
            _imageSpace = new ImageSpace();

            //_blobAlgorithm = new BlobAlgorithm();
            _previewImage = new PreviewImage();

            _inspWorker = new InspWorker();
            _imageLoader = new ImageLoader();

            _regKey = Registry.CurrentUser.CreateSubKey("Software\\BSolution");

            _model = new Model();

            switch (_camType)
            {
                case CameraType.WebCam:
                    {
                        _grabManager = new WebCam();
                        break;
                    }
                case CameraType.HikRobotCam:
                    {
                        _grabManager = new HikRobotCam();
                        break;
                    }
            }

            if (_grabManager != null && _grabManager.InitGrab() == true)
            {
                _grabManager.TransferCompleted += _multiGrab_TransferCompleted;

                InitModelGrab(MAX_GRAB_BUF);
            }

            VisionSequence.Inst.InitSequence();
            VisionSequence.Inst.SeqCommand += SeqCommand;

            if (!LastestModelOpen())
            {
                MessageBox.Show("모델 열기 실패");
            }

            return true;
        }

        private void LoadSetting()
        {
            _camType = SettingXml.Inst.CamType;
        }

        public void InitModelGrab(int bufferCount)
        {
            if (_grabManager == null)
                return;

            int pixelBpp = 8;
            _grabManager.GetPixelBpp(out pixelBpp);

            int inspectionWidth;
            int inspectionHeight;
            int inspectionStride;
            _grabManager.GetResolution(out inspectionWidth, out inspectionHeight, out inspectionStride);

            if (_imageSpace != null)
            {
                _imageSpace.SetImageInfo(pixelBpp, inspectionWidth, inspectionHeight, inspectionStride);
            }

            SetBuffer(bufferCount);

            eImageChannel imangeChannel = (pixelBpp == 24) ? eImageChannel.Color : eImageChannel.Gray;
            SetImageChannel(imangeChannel);

            //_grabManager.SetExposureTime(25000);

        }

        public void SetImageBuffer(string filePath)
        {
            Slogger.Write($"Load Image : {filePath}");

            Mat matImage = Cv2.ImRead(filePath);

            int pixelBpp = 8;
            int imageWidth;
            int imageHeight;
            int imageStride;

            if (matImage.Type() == MatType.CV_8UC3)
                pixelBpp = 24;

            imageWidth = (matImage.Width + 3) / 4 * 4;
            imageHeight = matImage.Height;

            // 4바이트 정렬된 새로운 Mat 생성
            Mat alignedMat = new Mat();
            Cv2.CopyMakeBorder(matImage, alignedMat, 0, 0, 0, imageWidth - matImage.Width, BorderTypes.Constant, Scalar.Black);

            imageStride = imageWidth * matImage.ElemSize();

            if (_imageSpace != null)
            {
                if (_imageSpace.ImageSize.Width != imageWidth || _imageSpace.ImageSize.Height != imageHeight)
                {
                    _imageSpace.SetImageInfo(pixelBpp, imageWidth, imageHeight, imageStride);
                    SetBuffer(_imageSpace.BufferCount);
                }
            }


            int bufferIndex = 0;

            int bufSize = (int)(alignedMat.Total() * alignedMat.ElemSize());
            Marshal.Copy(alignedMat.Data, ImageSpace.GetInspectionBuffer(bufferIndex), 0, bufSize);

            _imageSpace.Split(bufferIndex);

            DisplayGrabImage(bufferIndex);

            if (_previewImage != null)
            {
                Bitmap bitmap = ImageSpace.GetBitmap(0);
                _previewImage.SetImage(BitmapConverter.ToMat(bitmap));
            }
        }

        public void CheckImageBuffer()
        {
            if (_grabManager != null && SettingXml.Inst.CamType != CameraType.None)
            {
                int imageWidth;
                int imageHeight;
                int imageStride;
                _grabManager.GetResolution(out imageWidth, out imageHeight, out imageStride);

                if (_imageSpace.ImageSize.Width != imageWidth || _imageSpace.ImageSize.Height != imageHeight)
                {
                    int pixelBpp = 8;
                    _grabManager.GetPixelBpp(out pixelBpp);

                    _imageSpace.SetImageInfo(pixelBpp, imageWidth, imageHeight, imageStride);
                    SetBuffer(_imageSpace.BufferCount);
                }
            }
        }

        private void UpdateProperty(InspWindow inspWindow)
        {
            if (inspWindow is null)
                return;

            PropertiesForm propertiesForm = MainForm.GetDockForm<PropertiesForm>();
            if (propertiesForm is null)
                return;

            propertiesForm.UpdateProperty(inspWindow);
        }

        public void UpdateTeachingImage(int index)
        {
            if (_selectedInspWindow is null)
                return;

            SetTeachingImage(_selectedInspWindow, index);
        }

        public void DelTeachingImage(int index)
        {
            if (_selectedInspWindow is null)
                return;

            InspWindow inspWindow = _selectedInspWindow;

            inspWindow.DelWindowImage(index);

            MatchAlgorithm matchAlgo = (MatchAlgorithm)inspWindow.FindInspAlgorithm(InspectType.InspMatch);
            if (matchAlgo != null)
            {
                UpdateProperty(inspWindow);
            }
        }

        public void SetTeachingImage(InspWindow inspWindow, int index = -1)
        {
            if (inspWindow is null)
                return;

            CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm is null)
                return;

            Mat curImage = cameraForm.GetDisplayImage();
            if (curImage is null)
                return;

            if (inspWindow.WindowArea.Right >= curImage.Width ||
                inspWindow.WindowArea.Bottom >= curImage.Height)
            {
                Slogger.Write("ROI 영역이 잘못되었습니다.");
                return;
            }

            Mat windowImage = curImage[inspWindow.WindowArea];

            if (index < 0)
                inspWindow.AddWindowImage(windowImage);

            else
                inspWindow.SetWindowImage(windowImage, index);

            inspWindow.IsPatternLearn = false;

            MatchAlgorithm matchAlgo = (MatchAlgorithm)inspWindow.FindInspAlgorithm(InspectType.InspMatch);
            if (matchAlgo != null)
            {
                matchAlgo.ImageChannel = SelImageChannel;
                if (matchAlgo.ImageChannel == eImageChannel.Color)
                    matchAlgo.ImageChannel = eImageChannel.Gray;
                UpdateProperty(inspWindow);
            }
        }

        public void SetBuffer(int bufferCount)
        {
            _imageSpace.InitImageSpace(bufferCount);

            if (_grabManager != null)
            {
                _grabManager.InitBuffer(bufferCount);

                for (int i = 0; i < bufferCount; i++)
                {
                    _grabManager.SetBuffer(
                        _imageSpace.GetInspectionBuffer(i),
                        _imageSpace.GetnspectionBufferPtr(i),
                        _imageSpace.GetInspectionBufferHandle(i),
                        i);
                }
            }

            Slogger.Write("버퍼 초기화 성공!");
        }

        public void TryInspection(InspWindow inspWindow)
        {
            UpdateDiagramEntity();
            InspWorker.TryInspect(inspWindow, InspectType.InspNone);
        }

        public void SelectInspWindow(InspWindow inspWindow)
        {
            _selectedInspWindow = inspWindow;

            var propForm = MainForm.GetDockForm<PropertiesForm>();
            if (propForm != null)
            {
                if (inspWindow is null)
                {
                    propForm.ResetProperty();
                    return;
                }
                propForm.ShowProperty(inspWindow);
            }
            UpdateProperty(inspWindow);

            Global.Inst.InspStage.PreView.SetInspWindow(inspWindow);
        }

        public void AddInspWindow(InspWindowType windowType, Rect rect)
        {
            InspWindow inspWindow = _model.AddInspWindow(windowType);
            if (inspWindow is null)
                return;

            inspWindow.WindowArea = rect;
            inspWindow.IsTeach = false;

            SetTeachingImage(inspWindow);
            UpdateProperty(inspWindow);
            UpdateDiagramEntity();

            CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.SelectDiagramEntity(inspWindow);
                SelectInspWindow(inspWindow);
            }
        }
        //오토티칭
            public void AddInspWindowFromAutoTeach(InspWindowType windowType, OpenCvSharp.Rect rect)
        {
            InspWindow inspWindow = _model.AddInspWindow(windowType);
            if (inspWindow is null)
                return;

            inspWindow.WindowArea = rect;
            inspWindow.IsTeach = false;   // 잠금/티칭 상태 아님


            UpdateProperty(inspWindow);
        }
        public void AddInspWindowsFromAutoTeach(InspWindowType windowType, List<OpenCvSharp.Rect> rects)
        {
            if (rects == null || rects.Count == 0)
                return;

            foreach (var r in rects)
            {
                AddInspWindowFromAutoTeach(windowType, r);
            }

            UpdateDiagramEntity();

            CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null && rects.Count > 0)
            {
                // 마지막 추가 ROI를 선택 상태로 만들고 싶으면 여기서 처리 가능
                 //SelectInspWindow(_model.InspWindowList.Last());
            }
        }




        public bool AddInspWindow(InspWindow sourceWindow, OpenCvSharp.Point offset)
        {
            InspWindow cloneWindow = sourceWindow.Clone(offset);
            if (cloneWindow is null)
                return false;

            if (!_model.AddInspWindow(cloneWindow))
                return false;

            UpdateProperty(cloneWindow);
            UpdateDiagramEntity();

            CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.SelectDiagramEntity(cloneWindow);
                SelectInspWindow(cloneWindow);
            }

            return true;
        }

        public void MoveInspWindow(InspWindow inspWindow, OpenCvSharp.Point offset)
        {
            if (inspWindow is null)
                return;
            inspWindow.OffsetMove(offset);
            UpdateProperty(inspWindow);
        }

        public void ModifyInspWindow(InspWindow inspWindow, Rect rect)
        {
            if (inspWindow == null)
                return;

            inspWindow.WindowArea = rect;
            inspWindow.IsTeach = false;

            UpdateProperty(inspWindow);
        }

        public void DelInspWindow(InspWindow inspWindow)
        {
            _model.DelInspWindow(inspWindow);
            UpdateDiagramEntity();
        }

        public void DelInspWindow(List<InspWindow> inspWindowList)
        {
            _model.DelInspWindowList(inspWindowList);
            UpdateDiagramEntity();
        }

        public bool Grab(int bufferIndex)
        {
            if (_grabManager == null)
                return false;

            if (!_grabManager.Grab(bufferIndex, true))
                return false;

            return true;
        }

        //영상 취득 완료 이벤트 발생시 후처리
        private async void _multiGrab_TransferCompleted(object sender, object e)
        {
            int bufferIndex = (int)e;
            Slogger.Write($"TransferCompleted {bufferIndex}");

            _imageSpace.Split(bufferIndex);

            DisplayGrabImage(bufferIndex);

            if (_previewImage != null)
            {
                Bitmap bitmap = ImageSpace.GetBitmap(0);
                _previewImage.SetImage(BitmapConverter.ToMat(bitmap));
            }

            if (LiveMode)
            {
                Slogger.Write("Grab");
                await Task.Delay(100);
                _grabManager.Grab(bufferIndex, true);
            }
        }

        private void DisplayGrabImage(int bufferIndex)
        {
            var cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.UpdateDisplay();
            }
        }

        public void UpdateDisplay(Bitmap bitmap)
        {
            var cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.UpdateDisplay(bitmap);
            }
        }

        public void SetPreviewImage(eImageChannel channel)
        {
            if (_previewImage is null)
                return;

            Bitmap bitmap = ImageSpace.GetBitmap(0, channel);
            _previewImage.SetImage(BitmapConverter.ToMat(bitmap));

            SetImageChannel(channel);
        }

        public void SetImageChannel(eImageChannel channel)
        {
            var cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.SetImageChannel(channel);
            }
        }

        //public Mat GetCurrentImage()
        //{
        //    Bitmap bitmap = null;
        //    var cameraForm = MainForm.GetDockForm<CameraForm>();
        //    if (cameraForm != null)
        //    {
        //        bitmap = cameraForm.GetDisplayImage();
        //    }

        //    return bitmap;
        //}

        public Bitmap GetBitmap(int bufferIndex = -1, eImageChannel imageChannel = eImageChannel.None)
        {
            if (bufferIndex >= 0)
                SelBufferIndex = bufferIndex;

            if (imageChannel != eImageChannel.None)
                SelImageChannel = imageChannel;

            if (Global.Inst.InspStage.ImageSpace is null)
                return null;

            return Global.Inst.InspStage.ImageSpace.GetBitmap(SelBufferIndex, SelImageChannel);
        }


        public Mat GetMat(int bufferIndex = -1, eImageChannel imageChannel = eImageChannel.None)
        {
            if (bufferIndex >= 0)
                SelBufferIndex = bufferIndex;

            return Global.Inst.InspStage.ImageSpace.GetMat(SelBufferIndex, SelImageChannel);
        }

        public void UpdateDiagramEntity()
        {
            CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.UpdateDiagramEntity();
            }
            ModelTreeForm modelTreeForm = MainForm.GetDockForm<ModelTreeForm>();
            if (modelTreeForm != null)
            {
                modelTreeForm.UpdateDiagramEntity();
            }
        }

        public void RedrawMainView()
        {
            CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.UpdateImageViewer();
            }
        }

        public void ResetDisplay()
        {
            CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.ResetDisplay();
            }
        }

        public bool LoadModel(string filePath)
        {
            Slogger.Write($"모델 로딩: {filePath}");

            _model = _model.Load(filePath);


            if (_model is null)
            {
                Slogger.Write($"모델 로딩 실패 :{filePath}");
                return false;
            }

            _model.ModelFilePath = filePath;

            string inspImagePath = _model.InspectImagePath;
            if (File.Exists(inspImagePath))
            {
                Global.Inst.InspStage.SetImageBuffer(inspImagePath);
            }

            UpdateDiagramEntity();

            _regKey.SetValue("LastestModelPath", filePath);

            return true;
        }



        public void SaveModel (string filePath)
        {
            Slogger.Write($"모델 저장: {filePath}");

            if (string.IsNullOrEmpty(filePath))
            {
                Global.Inst.InspStage.CurModel.Save();
            }

            if (string.IsNullOrEmpty(filePath))
                Global.Inst.InspStage.CurModel.Save();
            else
            {
                Global.Inst.InspStage.CurModel.SaveAs(filePath);
                Global.Inst.InspStage.CurModel.ModelFilePath = filePath;
            }
        }

        private bool LastestModelOpen()
        {
            if (_lastestModelOpen)
                return true;

            _lastestModelOpen = true;

            string lastestModel = (string)_regKey.GetValue("LastestModelPath");
            if (File.Exists(lastestModel) == false)
                return true;

            DialogResult result = MessageBox.Show($"최근 모델을 로딩할까요?\r\n{lastestModel}", "Question", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
                return true;

            return LoadModel(lastestModel);
        }


        public void CycleInspect(bool isCycle)
        {
            if (InspWorker.IsRunning)
                return;

            if (!UseCamera)
            {
                string inspImagePath = CurModel.InspectImagePath;
                if (inspImagePath == "")
                    return;

                string inspImageDir = Path.GetDirectoryName(inspImagePath);
                if (!Directory.Exists(inspImageDir))
                    return;

                if (!_imageLoader.IsLoadedImages())
                    _imageLoader.LoadImages(inspImageDir);
            }

            if (isCycle)
                _inspWorker.StartCycleInspectImage();
            else
                OneCycle();
        }

        public bool OneCycle()
        {
            {
                if (UseCamera)
                {
                    if (!Grab(0))
                        return false;
                }
                else
                {
                    if (!VirtualGrab())
                        return false;
                }

                ResetDisplay();

                bool isDefect;
                if (!_inspWorker.RunInspect(out isDefect))
                    return false;

                return true;
            }
        }

        public void StopCycle()
        {
            if (_inspWorker != null)
                _inspWorker.Stop();

            VisionSequence.Inst.StopAutoRun();
            _isInspectMode = false;

            SetWorkingState(WorkingState.NONE);
        }

        public bool VirtualGrab()
        {
            if (_imageLoader is null)
                return false;

            string imagePath = _imageLoader.GetNextImagePath();
            if (imagePath == "")
                return false;

            Global.Inst.InspStage.SetImageBuffer(imagePath);

            _imageSpace.Split(0);

            DisplayGrabImage(0);

            return true;
        }

        private void SeqCommand(object sender, SeqCmd seqCmd, object Param)
        {
            switch (seqCmd)
            {
                case SeqCmd.InspStart:
                    {
                        Slogger.Write("MMI : InspStart", Slogger.LogType.Info);

                        string errMsg;

                        if (UseCamera)
                        {
                            if (!Grab(0))
                            {
                                errMsg = string.Format("Failed to grab");
                                Slogger.Write(errMsg, Slogger.LogType.Error);
                            }
                        }
                        else
                        {
                            if (!VirtualGrab())
                            {
                                errMsg = string.Format("Failed to virtual grab");
                                Slogger.Write(errMsg, Slogger.LogType.Error);
                            }
                        }
                    }
                    break;
                case SeqCmd.InspEnd:
                    {
                        Slogger.Write("MMI : InspEnd", Slogger.LogType.Info);

                        string errMsg = "";

                        Slogger.Write("검사 종료");

                        VisionSequence.Inst.VisionCommand(Vision2Mmi.InspEnd, errMsg);
                    }
                    break;
            }
        }

        public bool InspectReady(string lotNumber, string serialID)
        {
            _lotNumber = lotNumber;
            _serialID = serialID;

            LiveMode = false;
            UseCamera = SettingXml.Inst.CamType != CameraType.None ? true : false;

            Global.Inst.InspStage.CheckImageBuffer();

            ResetDisplay();

            return true;
        }

        public bool StartAutoRun()
        {
            Slogger.Write("Action : StartAutoRun");

            string modelPath = CurModel.ModelPath;
            if (modelPath == "")
            {
                Slogger.Write("열려진 모델이 없습니다!", Slogger.LogType.Error);
                MessageBox.Show("열려진 모델이 없습니다!");
                return false;
            }

            LiveMode = false;
            UseCamera = SettingXml.Inst.CamType != CameraType.None ? true : false;

            SetWorkingState(WorkingState.INSPECT);

            string modelName = Path.GetFileNameWithoutExtension(modelPath);
            VisionSequence.Inst.StartAutoRun(modelName);
            _isInspectMode = true;
            return true;

        }

        public void SetWorkingState(WorkingState workingState)
        {
            var cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.SetWorkingState(workingState);
            }
        }

        #region Disposable

        private bool disposed = false; // to detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.

                    VisionSequence.Inst.SeqCommand -= SeqCommand;

                    if (_saigeAI != null)
                    {
                        _saigeAI.Dispose();
                        _saigeAI = null;
                    }
                    if (_grabManager != null)
                    {
                        _grabManager.Dispose();
                        _grabManager = null;
                    }

                    _regKey.Close();
                }

                // Dispose unmanaged managed resources.

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion //Disposable
    }
}
