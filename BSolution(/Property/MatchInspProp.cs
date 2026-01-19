using BSolution_.Algorithm;
using BSolution_.Core;
using BSolution_.Teach;
using BSolution_.UIControl;
using BSolution_.Util;
using OpenCvSharp;
using OpenCvSharp.Extensions;
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


namespace BSolution_.Property
{
    public partial class MatchInspProp : UserControl
    {
        public event EventHandler<EventArgs> PropertyChanged;

        MatchAlgorithm _matchAlgo = null;

        public MatchInspProp()
        {
            InitializeComponent();

            txtExtendX.Leave += OnUpdateValue;
            txtExtendY.Leave += OnUpdateValue;
            txtScore.Leave += OnUpdateValue;

            patternImageEditor.ButtonChanged += PatternImage_ButtonChanged;

        }

        public void SetAlgorithm(MatchAlgorithm matchAlgo)
        {
            _matchAlgo = matchAlgo;
            SetProperty();
        }

        public void SetProperty()
        {
            if (_matchAlgo is null)
                return;

            chkUse.Checked = _matchAlgo.IsUse;

            OpenCvSharp.Size extendSize = _matchAlgo.ExtSize;
            int matchScore = _matchAlgo.MatchScore;
            int matchCount = _matchAlgo.MatchCount;

            txtExtendX.Text = extendSize.Width.ToString();
            txtExtendY.Text = extendSize.Height.ToString();
            txtScore.Text = matchScore.ToString();

            chkInvertResult.Checked = _matchAlgo.InvertResult;

            List<Mat> templateImages = _matchAlgo.GetTemplateImages();
            if (templateImages.Count > 0)
            {
                List<Bitmap> teachImages = new List<Bitmap>();

                foreach (var teachImage in templateImages)
                {
                    Bitmap bmpImage = BitmapConverter.ToBitmap(teachImage);
                    teachImages.Add(bmpImage);
                }

                patternImageEditor.DrawThumbnails(teachImages);
            }
        }

        private void OnUpdateValue(object sender, EventArgs e)
        {
            if (_matchAlgo == null)
                return;

            OpenCvSharp.Size extendSize = _matchAlgo.ExtSize;

            if (!int.TryParse(txtExtendX.Text, out extendSize.Width))
            {
                MessageBox.Show("숫자만 입력 가능합니다.");
                return;
            }

            if (!int.TryParse(txtExtendY.Text, out extendSize.Height))
            {
                MessageBox.Show("숫자만 입력 가능합니다.");
                return;
            }

            int score = _matchAlgo.MatchScore;
            if (!int.TryParse(txtScore.Text, out score))
            {
                MessageBox.Show("숫자만 입력 가능합니다.");
                return;
            }
            ;

            _matchAlgo.ExtSize = extendSize;
            _matchAlgo.MatchScore = score;

            PropertyChanged?.Invoke(this, null);
        }

        private void chkUse_CheckedChanged(object sender, EventArgs e)
        {
            bool useMatch = chkUse.Checked;

            grpMatch.Enabled = useMatch;
            patternImageEditor.Enabled = useMatch;

            if (_matchAlgo != null)
                _matchAlgo.IsUse = useMatch;
        }

        private void chkInvertResult_CheckedChanged(object sender, EventArgs e)
        {
            if (_matchAlgo is null)
                return;

            _matchAlgo.InvertResult = chkInvertResult.Checked;
        }

        private void PatternImage_ButtonChanged(object sender, PatternImageEventArgs e)
        {
            int index = e.Index;

            switch (e.Button)
            {
                case PatternImageButton.UpdateImage:
                    Global.Inst.InspStage.UpdateTeachingImage(index);
                    break;
                case PatternImageButton.AddImage:
                    Global.Inst.InspStage.UpdateTeachingImage(-1);
                    break;
                case PatternImageButton.DelImage:
                    Global.Inst.InspStage.DelTeachingImage(index);
                    break;
            }
        }


        private bool _isAutoTeaching = false;




        private void btnClearAutoTeach_Click(object sender, EventArgs e)
        {
            if (_matchAlgo is null)
                return;


            var cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
                cameraForm.ResetDisplay();

            var model = Global.Inst.InspStage.CurModel;
            if (model != null && !string.IsNullOrEmpty(model.ModelPath))
                model.Save();
        }

        private void btnAutoTeach_Click_1(object sender, EventArgs e)
        {
            if (_isAutoTeaching) return;
            _isAutoTeaching = true;

            try
            {
                if (_matchAlgo == null)
                    return;

                InspWindow win = GetSelectedInspWindowSafe();
                if (win == null)
                {
                    MessageBox.Show("ROI가 선택되어 있을 때만 오토티칭이 가능합니다.");
                    return;
                }

                win.PatternLearn();

                List<Mat> templates = _matchAlgo.GetTemplateImages();
                if (templates == null || templates.Count <= 0 || templates[0] == null || templates[0].Empty())
                {
                    MessageBox.Show("티칭(템플릿) 이미지가 없습니다. 먼저 티칭 이미지를 등록하세요.");
                    return;
                }

                Mat template = templates[0];

                Mat src = Global.Inst.InspStage.GetMat(0, _matchAlgo.ImageChannel);
                if (src == null || src.Empty())
                {
                    MessageBox.Show("현재 이미지가 없습니다.");
                    return;
                }

                Rect ext = win.WindowArea;
                ext.Inflate(_matchAlgo.ExtSize);

                if (ext.X < 0) ext.X = 0;
                if (ext.Y < 0) ext.Y = 0;
                if (ext.Right > src.Width) ext.Width = src.Width - ext.X;
                if (ext.Bottom > src.Height) ext.Height = src.Height - ext.Y;

                if (ext.Width <= 0 || ext.Height <= 0)
                {
                    MessageBox.Show("검색 영역(ExtArea)이 유효하지 않습니다.");
                    return;
                }

                List<OpenCvSharp.Point> points;
                List<int> scores;

                using (Mat target = src[ext])
                {
                    int cnt = _matchAlgo.MatchTemplateMultiple(target, ext.TopLeft, out points, out scores);
                    if (cnt <= 0)
                    {
                        MessageBox.Show("오토티칭 결과가 없습니다.");
                        _matchAlgo.AutoTeachResults.Clear();
                        return;
                    }

                    _matchAlgo.AutoTeachRois.Clear();
                    _matchAlgo.AutoTeachScores.Clear();

                    int w = template.Width;
                    int h = template.Height;

                    for (int i = 0; i < points.Count; i++)
                    {
                        var p = points[i];
                        int sc = (i < scores.Count) ? scores[i] : 0;

                        _matchAlgo.AutoTeachRois.Add(new OpenCvSharp.Rect(p.X, p.Y, w, h));
                        _matchAlgo.AutoTeachScores.Add(sc);
                    }

                    Slogger.Write($"AutoTeachRois Count = " + _matchAlgo.AutoTeachRois.Count);
                }

                _matchAlgo.AutoTeachResults.Clear();

                int tw = template.Width;
                int th = template.Height;

                List<DrawInspectInfo> rectInfos = new List<DrawInspectInfo>();

                for (int i = 0; i < points.Count; i++)
                {
                    OpenCvSharp.Point p = points[i];
                    int sc = (i < scores.Count) ? scores[i] : 0;

                    _matchAlgo.AutoTeachResults.Add(new AutoTeach(p.X, p.Y, tw, th, sc));

                    Rect r = new Rect(p.X, p.Y, tw, th);
                    string info = string.Format("{0}%", sc);

                    rectInfos.Add(new DrawInspectInfo(r, info, InspectType.InspMatch, DecisionType.None));
                }

                CameraForm cam = MainForm.GetDockForm<CameraForm>();
                if (cam != null)
                {
                    cam.ResetDisplay();
                    cam.AddRect(rectInfos);
                }

                Model model = Global.Inst.InspStage.CurModel;
                if (model != null && !string.IsNullOrEmpty(model.ModelPath))
                    model.Save();

                MessageBox.Show(string.Format("오토티칭 완료: {0}개", points.Count));
            }
            finally
            {
                _isAutoTeaching = false;
            }
        }


        private InspWindow GetSelectedInspWindowSafe()
        {
            return Global.Inst.InspStage.SelectedInspWindow;
        }

        private void btnApplyAutoTeachRoi_Click(object sender, EventArgs e)
        {
            if (_matchAlgo == null)
                return;

            var model = Global.Inst.InspStage.CurModel;
            if (model == null)
                return;

            // 오토티칭 결과 스냅샷(선택 변경 방지)
            MatchAlgorithm srcAlgo = _matchAlgo;

            if (srcAlgo.AutoTeachRois == null || srcAlgo.AutoTeachRois.Count == 0)
            {
                MessageBox.Show("오토티칭 결과가 없습니다. 먼저 오토티칭을 실행하세요.");
                return;
            }

            // ROI 선택 조건
            InspWindow selected = Global.Inst.InspStage.SelectedInspWindow;
            if (selected == null)
            {
                MessageBox.Show("ROI가 선택된 상태에서만 적용할 수 있습니다.");
                return;
            }

            // 모델 경로 체크 (Teach 폴더 만들기 위해 필요)
            if (string.IsNullOrEmpty(model.ModelFilePath))
            {
                MessageBox.Show("모델을 먼저 저장하거나 로드한 뒤 ROI 적용 가능");
                return;
            }

            string modelDir = Path.GetDirectoryName(model.ModelFilePath);
            string teachDir = Path.Combine(modelDir, "Teach");
            Directory.CreateDirectory(teachDir);

            // 소스 이미지 확보(오토티칭 채널 기준)
            Mat srcImage = Global.Inst.InspStage.GetMat(0, srcAlgo.ImageChannel);

            int createdCount = srcAlgo.AutoTeachRois.Count;
            InspWindowType windowType = selected.InspWindowType;

            // "추가 전" 리스트 카운트 저장 (void 메서드라 newWin을 못 받으니 이걸로 찾음)
            var list = model.InspWindowList;
            int beforeCount = (list != null) ? list.Count : 0;

            // ROI 반복 추가 + 티칭 파일 저장 + TeachImagePaths 등록
            for (int i = 0; i < srcAlgo.AutoTeachRois.Count; i++)
            {
                var r = srcAlgo.AutoTeachRois[i];

                // 1) ROI 추가
                Global.Inst.InspStage.AddInspWindowFromAutoTeach(windowType, r);

                // 2) 방금 추가된 InspWindow 가져오기(일반적으로 마지막)
                InspWindow newWin = null;
                if (list != null && list.Count > beforeCount)
                {
                    newWin = list[list.Count - 1];
                    beforeCount = list.Count; // 다음 반복을 위해 갱신
                }

                if (newWin == null)
                {
                    Slogger.Write("AutoTeach ROI added, but failed to fetch new InspWindow.");
                    continue;
                }

                // 3) 템플릿(티칭 이미지) 저장 경로
                string id = Guid.NewGuid().ToString("N");
                string savePath = Path.Combine(teachDir, id + "_match.png");

                // 4) 템플릿 파일 저장
                bool saved = SaveTeachTemplate(srcImage, r, savePath);
                if (!saved)
                {
                    Slogger.Write("Teach template save failed: " + savePath);
                    continue;
                }

                // 5) TeachImagePaths 등록 (모델 저장 대상)
                if (newWin.TeachImagePaths == null)
                    newWin.TeachImagePaths = new List<string>();

                newWin.TeachImagePaths.Add(savePath);

                // 6) 재학습 준비
                newWin.IsPatternLearn = false;
                newWin.ClearTeachImageCache();

                // 여기서 즉시 학습까지 하고 싶으면(권장):
                // newWin.PatternLearn();
            }

            Global.Inst.InspStage.UpdateDiagramEntity();
            Global.Inst.InspStage.RedrawMainView();

            // 모델 저장 (ModelPath가 아니라 ModelFilePath 기준이 더 일관적)
            model.Save();

            MessageBox.Show(string.Format("ROI 적용 완료 : {0}개 생성", createdCount));
            Slogger.Write(string.Format("ROI : {0}개 생성", createdCount));
        }


        private static OpenCvSharp.Rect ClampRect(OpenCvSharp.Rect r, OpenCvSharp.Mat img)
        {
            int x = Math.Max(0, r.X);
            int y = Math.Max(0, r.Y);

            int right = Math.Min(img.Width, r.Right);
            int bottom = Math.Min(img.Height, r.Bottom);

            int w = right - x;
            int h = bottom - y;

            // 비어있음 처리: 너비/높이가 0 이하
            if (w <= 0 || h <= 0)
                return new OpenCvSharp.Rect(0, 0, 0, 0);

            return new OpenCvSharp.Rect(x, y, w, h);
        }

        private static bool SaveTeachTemplate(OpenCvSharp.Mat src, OpenCvSharp.Rect roi, string savePath)
        {
            OpenCvSharp.Rect r = ClampRect(roi, src);

            // Rect.Empty 대신 Width/Height로 검사
            if (r.Width <= 0 || r.Height <= 0)
                return false;

            using (var crop = new OpenCvSharp.Mat(src, r))
            using (var gray = new OpenCvSharp.Mat())
            {
                if (crop.Type() == OpenCvSharp.MatType.CV_8UC3)
                    OpenCvSharp.Cv2.CvtColor(crop, gray, OpenCvSharp.ColorConversionCodes.BGR2GRAY);
                else
                    crop.CopyTo(gray);

                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(savePath));
                return OpenCvSharp.Cv2.ImWrite(savePath, gray);
            }
        }


    }
}


