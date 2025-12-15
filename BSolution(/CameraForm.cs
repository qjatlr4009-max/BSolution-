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

namespace BSolution_
{
    //public partial class CameraForm : Form
    public partial class CameraForm : DockContent

    {
        public CameraForm()
        {
            InitializeComponent();
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
        }

        public Bitmap GetDisplayImage()
        {
            Bitmap curImage = null;

            if (imageViewer != null)
                curImage = imageViewer.GetCurBitmap();

            return curImage;
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

