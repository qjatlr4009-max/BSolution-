using BSolution_.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using BSolution_.Property;
using System.Windows.Forms;

namespace BSolution_.Core
{
    public class PreviewImage
    {
        private Mat _originalImage = null;
        private Mat _previewImage = null;

        private InspWindowType _inspWindow = null;
        private bool _usePreview = true;

        public void SetImage(Mat image)
        {
            _originalImage = image;
            _previewImage = new Mat();
        }

        public void SetBinary(int lowerValue, int upperValue, bool invert, ShowBinaryMode showBinMode)
        {
            if (_usePreview == false)
                return;

            if (_originalImage == null)
                return;

            var cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm == null) return;

            Bitmap bmpImage;
            if (showBinMode == ShowBinaryMode.ShowBinaryNone)
            {
                bmpImage = BitmapConverter.ToBitmap(_originalImage);
                cameraForm.UpdateDisplay(bmpImage);
                return;
            }

            Rect windowArea = new Rect(0, 0, _originalImage.Width, _originalImage.Height);

            Mat orgRoi = _originalImage[windowArea];

            Mat grayImage = new Mat();
            if (orgRoi.Type() == MatType.CV_8UC3)
                Cv2.CvtColor(orgRoi, grayImage, ColorConversionCodes.BGR2GRAY);

            else
                grayImage = orgRoi;

            Mat binaryMask = new Mat();
            Cv2.InRange(grayImage, lowerValue, upperValue, binaryMask);

            if (invert)
                binaryMask = ~binaryMask;

            Mat fullBinaryMask = Mat.Zeros(_originalImage.Size(), MatType.CV_8UC1);
            binaryMask.CopyTo(new Mat(fullBinaryMask, windowArea));

            if (showBinMode == ShowBinaryMode.ShowBinaryOnly)
            {
                if (orgRoi.Type() == MatType.CV_8UC3)
                {
                    Mat colorBinary = new Mat();
                    Cv2.CvtColor(binaryMask, colorBinary, ColorConversionCodes.GRAY2BGR);
                    _previewImage = _originalImage.Clone();
                    colorBinary.CopyTo(new Mat(_previewImage, windowArea));
                }

                else
                {
                    _previewImage = _originalImage.Clone();
                    binaryMask.CopyTo(new Mat(_previewImage, windowArea));
                }

                bmpImage = BitmapConverter.ToBitmap(_previewImage);
                cameraForm.UpdateDisplay(bmpImage);
                return;
            }

            Scalar highlightColor;
            if (showBinMode == ShowBinaryMode.ShowBinaryHighlightRed)
                highlightColor = new Scalar(0, 0, 255);
            else if (showBinMode == ShowBinaryMode.ShowBinaryHighlightGreen)
                highlightColor = new Scalar(0, 255, 0);
            else
                highlightColor = new Scalar(255, 0, 0);

            Mat overlayImage;
            if (_originalImage.Type() == MatType.CV_8UC1)
            {
                overlayImage = new Mat();
                Cv2.CvtColor(_originalImage, overlayImage, ColorConversionCodes.GRAY2BGR);

                Mat colorOrinal = overlayImage.Clone();

                overlayImage.SetTo(highlightColor, fullBinaryMask); // 빨간색으로 마스킹

                
                Cv2.AddWeighted(colorOrinal, 0.7, overlayImage, 0.3, 0, _previewImage);
            }
            else
            {
                overlayImage = _originalImage.Clone();
                overlayImage.SetTo(highlightColor, fullBinaryMask); // 빨간색으로 마스킹

                
                Cv2.AddWeighted(_originalImage, 0.7, overlayImage, 0.3, 0, _previewImage);
            }

            bmpImage = BitmapConverter.ToBitmap(_previewImage);
            cameraForm.UpdateDisplay(bmpImage);
        }
    }
}



