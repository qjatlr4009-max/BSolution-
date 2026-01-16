using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using BrightIdeasSoftware;
using BSolution_.Teach;
using BSolution_.Inspect;

namespace BSolution_
{
    public partial class ResultForm : DockContent
    {
        private SplitContainer _splitContainer;
        private TreeListView _treeListView;
        private TextBox _txtDetails;
        public ResultForm()
        {
            InitializeComponent();

            InitTreeListView();
        }

        private void InitTreeListView()
        {
            _splitContainer = new SplitContainer()
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 30,
                Panel1MinSize = 70,
                Panel2MinSize = 30
            };

            _treeListView = new TreeListView()
            {
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                ShowGroups = false,
                UseFiltering = true,
                OwnerDraw = true,
                MultiSelect = false,
                GridLines = true
            };

            _treeListView.SelectionChanged += TreeListView_SelectionChanged;

            _treeListView.CanExpandGetter = x => true;

            _treeListView.ChildrenGetter = x =>
            {
                if (x is InspWindow w)
                    return w.InspResultList;
                return new List<InspResult>();
            };

            var colUID = new OLVColumn("UID", "")
            {
                Width = 100,
                IsEditable = false,
                AspectGetter = obj =>
                {
                    if (obj is InspWindow win)
                        return win.UID;
                    if (obj is InspResult res)
                        return res.InspType.ToString();
                    return "";
                }
            };

            var colAlgo = new OLVColumn("Algorithm", "")
            {
                Width = 150,
                IsEditable = false,
                AspectGetter = obj =>
                {
                    if (obj is InspResult res)
                        return res.InspType.ToString();
                    return "";
                }
            };

            var colStatus = new OLVColumn("Status", "IsDefect")
            {
                Width = 80,
                TextAlign = HorizontalAlignment.Center,
                AspectGetter = obj =>
                {
                    if (obj is InspResult res)
                        return res.IsDefect ? "NG" : "OK";
                    return "";
                }
            };
            var colValue = new OLVColumn("Result", "Result")
            {
                Width = 80,
                TextAlign = HorizontalAlignment.Center,
                AspectGetter = obj =>
                {
                    if (obj is InspResult res)
                        return res.ResultValue;
                    return "";
                }
            };
            _treeListView.Columns.AddRange(new OLVColumn[] { colUID, colAlgo, colStatus, colValue });

            _txtDetails = new TextBox()
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Arial", 10),
                ReadOnly = true
            };

            _splitContainer.Panel1.Controls.Add(_treeListView);
            _splitContainer.Panel2.Controls.Add(_txtDetails);
            Controls.Add(_splitContainer);
        }

        public void AddModelResult (Model curModel)
        {
            if (curModel is null)
                return;

            _treeListView.SetObjects(curModel.InspWindowList);

            foreach (var window in curModel.InspWindowList)
            {
                _treeListView.Expand(window);
            }
        }

        public void AddWindowResult (InspWindow inspWindow)
        {
            if (inspWindow is null)
                return;

            _treeListView.SetObjects(new List<InspWindow> { inspWindow });
            _treeListView.Expand(inspWindow);

            if(inspWindow.InspResultList.Count >0)
            {
                InspResult inspResult = inspWindow.InspResultList[0];
                ShowDetail(inspResult);
            }
        }

        public void AddInspResult (InspResult inspResult)
        {
            if (inspResult is null)
                return;

            var existingResults = _treeListView.Objects as List<InspResult>;

            if (existingResults == null)
                existingResults = new List<InspResult>();

            var parentResult = existingResults.FirstOrDefault(r => r.GroupID == inspResult.GroupID);

            existingResults.Add(inspResult);

            _treeListView.SetObjects(existingResults);
        }

        private void TreeListView_SelectionChanged(object sender, EventArgs e)
        {
            if (_treeListView.SelectedObject == null)
            {
                _txtDetails.Text = string.Empty;
                return;
            }
            if (_treeListView.SelectedObject is InspResult result)
            {
                ShowDetail(result);
            }
            else if (_treeListView.SelectedObject is InspWindow window)
            {
                var infos = window.InspResultList.Select(r => $" -{r.ObjectID}: {r.ResultInfos}").ToList();
                _txtDetails.Text = $"{window.UID}\r\n" +
                    string.Join("\r\n", infos);
            }
        }

        private void ShowDetail(InspResult result)
        {
            if (result is null)
                return;

            _txtDetails.Text = result.ResultInfos.ToString();

            if (result.ResultRectList != null)
            {
                CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
                if (cameraForm != null)
                {
                    cameraForm.AddRect(result.ResultRectList);
                }
            }
        }
    }
}
