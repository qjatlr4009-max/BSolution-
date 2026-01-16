using BSolution_.Util;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace BSolution_.Grab
{
    class WebCam : GrabModel
    {

        private VideoCapture _capture = null;
        private Mat _frame = null;

        private bool _disposed = false;

        internal override bool Create(string strIpAddr = null)
        {
            _capture = new VideoCapture(0);
            if (_capture == null)
                return false;

            return true;
        }

        internal override bool Grab(int bufferIndex, bool waitDone)
        {
            if (_frame is null)
                _frame = new Mat();

            _capture.Read(_frame);
            if (!_frame.Empty())
            {
                OnGrabCompleted(bufferIndex);

                int bufSize = (int)(_frame.Total() * _frame.ElemSize());

                if (_userImageBuffer != null && _userImageBuffer.Length > bufferIndex)
                {
                    if (_userImageBuffer[bufferIndex].ImageBuffer.Length >= bufSize)
                    {
                        Marshal.Copy(_frame.Data, _userImageBuffer[BufferIndex].ImageBuffer, 0, bufSize);
                    }
                    else
                    {
                        Slogger.Write("Error: Buffer size is too small.", Slogger.LogType.Error);

                    }
                }
                OnTransferCompleted(bufferIndex);

                if (IncreaseBufferIndex)
                {
                    BufferIndex++;
                    if (BufferIndex >= _userImageBuffer.Count())
                        BufferIndex = 0;
                }
            }
            return true;
        }

        internal override bool Close()
        {
            if (_capture != null)
                _capture.Release();

            return true;
        }

        internal override bool Open()
        {
            if (_capture == null)
            return false;

            int fourccBGR3 = VideoWriter.FourCC('B', 'G', 'R', '3');
            _capture.Set(VideoCaptureProperties.CodecPixelFormat, fourccBGR3);

            return true;
        }

        internal override bool Reconnect()
        {
            Close();
            return Open();
        }

        internal override bool GetPixelBpp(out int pixelBpp)
        {
            pixelBpp = 8;

            if (_capture == null)
                return false;

            if (_frame is null)
            {
                _frame = new Mat();
                _capture.Read(_frame);
            }

            pixelBpp = _frame.ElemSize() * 8;

            return true;
        }

        internal override bool SetExposureTime(long exposure)
        {
            if (_capture == null)
                return false;

            _capture.Set(VideoCaptureProperties.Exposure, exposure);
            return true;
        }

        internal override bool GetExposureTime(out long exposure)
        {
            exposure = 0;

            if (_capture == null)
                return false;

            exposure = (long)_capture.Get(VideoCaptureProperties.Exposure);
            return true;
        }

        internal override bool SetGain(long gain)
        {
            if (_capture == null)
                return false;

            _capture.Set(VideoCaptureProperties.Gain, gain);
            return true;
        }

        internal override bool GetGain(out long gain)
        {
            gain = 0;
            if (_capture == null)
                return false;

            gain = (long)_capture.Get(VideoCaptureProperties.Gain);
            return true;
        }

        internal override bool GetResolution(out int width, out int height, out int stride)
        {
            width = 0;
            height = 0;
            stride = 0;

            if (_capture == null)
                return false;

            width = (int)_capture.Get(VideoCaptureProperties.FrameWidth);
            height = (int)_capture.Get(VideoCaptureProperties.FrameHeight);

            int bpp = 8;
            GetPixelBpp(out bpp);

            if (bpp == 8)
                stride = width * 1;
            else
                stride = width * 3;

            return true;
        }

        internal override bool SetTriggerMode(bool hardwareTrigger)
        {
            if (_capture == null)
                return false;

            HardwareTrigger = hardwareTrigger;
            return true;
        }

        internal override void Dispose()
        {
            Dispose(disposing: true);
        }

        internal void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            { 
                if (_capture != null)
                {
                    _capture.Release();
                }
                _disposed = true;
            }
        }
    }
}
