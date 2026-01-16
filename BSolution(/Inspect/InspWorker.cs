using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BSolution_.Algorithm;
using BSolution_.Core;
using BSolution_.Util;
using BSolution_.Teach;
using OpenCvSharp;



namespace BSolution_.Inspect
{
    public class InspWorker
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();

        private InspectBoard _inspectBoard = new InspectBoard();

        public bool IsRunning { get; set; } = false;

        public InspWorker()
        { }

        public void Stop()
        {
            _cts.Cancel();
        }
        public void StartCycleInspectImage()
        {
            _cts = new CancellationTokenSource();
            Task.Run(() => InspectionLoop(this, _cts.Token));
        }

        private void InspectionLoop(InspWorker inspWorker, CancellationToken token)
        {
            Global.Inst.InspStage.SetWorkingState(WorkingState.INSPECT);

            Slogger.Write("InspectionLoop Start");

            IsRunning = true;

            while (!token.IsCancellationRequested)
            {
                Global.Inst.InspStage.OneCycle();
            }

            IsRunning = false;
            Slogger.Write("InspectionLoop End");
        }

        public bool RunInspect(out bool isDefect)
        {
            isDefect = false;
            Model curMode = Global.Inst.InspStage.CurModel;
            List<InspWindow> inspWindowList = curMode.InspWindowList;
            foreach (var inspWindow in inspWindowList)
            {
                if (inspWindow is null)
                    continue;

                UpdateInspData(inspWindow);
            }
            _inspectBoard.InspectWindowList(inspWindowList);

            int totalCnt = 0;
            int okCnt = 0;
            int ngCnt = 0;
            foreach (var inspWindow in inspWindowList)
            {
                totalCnt++;

                if (inspWindow.IsDefect())
                {
                    if (!isDefect)
                        isDefect = true;

                    ngCnt++;
                }
                else { okCnt++; }

                DisplayResult(inspWindow, InspectType.InspNone);
            }

            if (totalCnt > 0)
            {
                var cameraForm = MainForm.GetDockForm<CameraForm>();
                if (cameraForm != null) { cameraForm.SetInspResultCount(totalCnt, okCnt, ngCnt); }
            }
            return true;
        }

        public bool TryInspect(InspWindow inspObj, InspectType inspType)
        {
            if (inspObj != null)
            {
                if (!UpdateInspData(inspObj))
                    return false;

                _inspectBoard.Inspect(inspObj);

                DisplayResult(inspObj, inspType);
            }
            else
            {
                bool isDefect = false;
                RunInspect(out isDefect);
            }

            ResultForm resultForm = MainForm.GetDockForm<ResultForm>();
            if (resultForm != null)
            {
                if (inspObj != null)
                    resultForm.AddWindowResult(inspObj);
                else
                {
                    Model curMode = Global.Inst.InspStage.CurModel;
                    resultForm.AddModelResult(curMode);
                }
            }

            return true;
        }
        private bool UpdateInspData(InspWindow inspWindow)
        {
            if (inspWindow is null)
                return false;

            Rect windowArea = inspWindow.WindowArea;

            inspWindow.PatternLearn();

            foreach (var inspAlgo in inspWindow.AlgorithmList)
            {
                inspAlgo.TeachRect = windowArea;
                inspAlgo.InspRect = windowArea;

                Mat srcImage = Global.Inst.InspStage.GetMat(0, inspAlgo.ImageChannel);
                inspAlgo.SetInspData(srcImage);
            }

            return true;
        }

        private bool DisplayResult(InspWindow inspObj, InspectType inspType)
        {
            if (inspObj is null)
                return false;

            List<DrawInspectInfo> totalArea = new List<DrawInspectInfo>();

            List<InspAlgorithm> inspAlgorithmList = inspObj.AlgorithmList;
            foreach (var algorithm in inspAlgorithmList)
            {
                if (algorithm.InspectType != inspType && inspType != InspectType.InspNone)
                    continue;

                List<DrawInspectInfo> resultArea = new List<DrawInspectInfo>();
                int resultCnt = algorithm.GetResultRect(out resultArea);
                if (resultCnt > 0)
                {
                    totalArea.AddRange(resultArea);
                }
            }

            if (totalArea.Count > 0)
            {
                var cameraForm = MainForm.GetDockForm<CameraForm>();
                if (cameraForm != null)
                {
                    cameraForm.AddRect(totalArea);
                }
            }

            return true;
        }

    }
}
