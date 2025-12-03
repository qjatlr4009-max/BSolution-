using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace BSolution_.UIControl
{
    public partial class ImageViewCtrl : UserControl
    {

        private bool _isInitialized = false;

        private Bitmap _bitmapImage = null;

        private Bitmap Canvas = null;

        private RectangleF ImageRect = new RectangleF(0, 0, 0, 0);

        private float _curZoom = 1.0f;
        private float _zoomFactor = 1.1f;

        private float MinZoom = 1.0f;
        private float MaxZoom = 100.0f;
        public ImageViewCtrl()
        {
            InitializeComponent();
            InitializeCanvas();


        }
        public Bitmap GetCurBitmap()
        {
            return _bitmapImage;
        }
        private void InitializeCanvas()
        {
            ResizeCanvas();

            DoubleBuffered = true;
        }

        private void ResizeCanvas()
        {
            if (Width <= 0 || Height <= 0 || _bitmapImage == null)
                return;

            Canvas = new Bitmap(Width, Height);
            if (Canvas == null)
                return;

            float virtualWidth = _bitmapImage.Width * _curZoom;
            float virtualHeight = _bitmapImage.Height * _curZoom;

            float offsetX = virtualWidth < Width ? (Width - virtualWidth) / 2f : 0f;
            float offsetY = virtualHeight < Height ? (Height - virtualHeight) / 2f : 0f;

            ImageRect = new RectangleF(offsetX, offsetY, virtualWidth, virtualHeight);
        }
        private void FitImageToScreen()
        {
            RecalcZoomRatio();


            float NewWidth = _bitmapImage.Width * _curZoom;
            float NewHeight = _bitmapImage.Height * _curZoom;

            ImageRect = new RectangleF((Width - NewWidth) / 2,(Height - NewHeight) / 2,NewWidth,NewHeight
            );
            Invalidate();
        }
        private void RecalcZoomRatio()
        {
            if (_bitmapImage == null || Width <= 0 || Height <= 0)
                return;

            Size imageSize = new Size(_bitmapImage.Width, _bitmapImage.Height);

            float aspectRatio = (float)imageSize.Height / (float)imageSize.Width;
            float clinetAspect = (float)Height / (float)Width;

            float ratio;
            if (aspectRatio <= clinetAspect)
                ratio = (float)Width / (float)imageSize.Width;

            else ratio = (float)Height / (float)imageSize.Height;

            float minZoom = ratio;

            MinZoom = minZoom;
            _curZoom = Math.Max(MinZoom, Math.Min(MaxZoom, ratio));

            Invalidate();
        }
        public void LoadBitmap(Bitmap bitmap)
        {
            if (_bitmapImage != null)
            {
                if (_bitmapImage.Width == bitmap.Width && _bitmapImage.Height == bitmap.Height)
                { _bitmapImage = bitmap; Invalidate(); return; }

                _bitmapImage.Dispose();
                _bitmapImage = null;
            }
            _bitmapImage = bitmap;

            if (_isInitialized == false)
            {
                _isInitialized = true;
                ResizeCanvas();
            }
            FitImageToScreen();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnResize(e);

            if (_bitmapImage != null && Canvas != null)
            {
                using (Graphics g = Graphics.FromImage(Canvas))
                {
                    g.Clear(Color.Transparent);

                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    g.DrawImage(_bitmapImage, ImageRect);

                    e.Graphics.DrawImage(Canvas, 0, 0);
                }
            }
        }
    }
}

