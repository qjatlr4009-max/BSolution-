using BSolution_.Grab;
using BSolution_.Util;
using MvCameraControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows.Forms;

namespace BSolution_.Grab
{


    internal class HikRobotCam : GrabModel
    {
        private IDevice _device = null;

        // 이미지 취득 콜백함수
        void FrameGrabedEventHandler(object sender, FrameGrabbedEventArgs e)
        {
            //Slogger.Write("Get one frame : Width[{0}], Height[{1}] , ImageSize[{2}] , FrameNum[{3}]",
            //    e.FrameOut.Image.Width, e.FrameOut.Image.Height,
            //    e.FrameOut.Image.ImageSize, e.FrameOut.FrameNum);

            IFrameOut frameOut = e.FrameOut;

            // 영상 취득이 완료되었을 때 이벤트 
            OnGrabCompleted(BufferIndex);

            if (_userImageBuffer[BufferIndex].ImageBuffer != null)
            {
                if (frameOut.Image.PixelType == MvGvspPixelType.PixelType_Gvsp_Mono8)
                {
                    if (_userImageBuffer[BufferIndex].ImageBuffer != null)
                    {
                        IntPtr ptrSourceTemp = frameOut.Image.PixelDataPtr;
                        Marshal.Copy(ptrSourceTemp, _userImageBuffer[BufferIndex].ImageBuffer, 0, (int)frameOut.Image.ImageSize);
                    }
                }
                else
                {
                    IImage inputImage = frameOut.Image;
                    IImage outImage;
                    MvGvspPixelType dstPixelType = MvGvspPixelType.PixelType_Gvsp_BGR8_Packed;

                    // 픽셀 타입 변환
                    int result = _device.PixelTypeConverter.ConvertPixelType(inputImage, out outImage, dstPixelType);
                    if (result != MvError.MV_OK)
                    {
                        Slogger.Write($"Image Convert failed:{result:x8}", Slogger.LogType.Error);
                        return;
                    }

                    if (_userImageBuffer[BufferIndex].ImageBuffer != null)
                    {
                        IntPtr ptrSourceTemp = outImage.PixelDataPtr;
                        Marshal.Copy(ptrSourceTemp, _userImageBuffer[BufferIndex].ImageBuffer, 0, (int)outImage.ImageSize);
                    }
                }
            }

            // 영상 전송이 완료되었을 때 이벤트 발생
            OnTransferCompleted(BufferIndex);

            //IO 트리거 촬상시 최대 버퍼를 넘으면 첫번째 버퍼로 변경
            if (IncreaseBufferIndex)
            {
                BufferIndex++;
                if (BufferIndex >= _userImageBuffer.Count())
                    BufferIndex = 0;
            }
        }



        internal override bool Create(string strIpAddr = null)
        {
            SDKSystem.Initialize();

            _strIpAddr = strIpAddr;

            try
            {
                const DeviceTLayerType devLayerType = DeviceTLayerType.MvGigEDevice;

                List<IDeviceInfo> devInfoList;

                int ret = DeviceEnumerator.EnumDevices(devLayerType, out devInfoList);
                if (ret != MvError.MV_OK)
                {
                    Slogger.Write($"Enum device failed:{ret:x8}", Slogger.LogType.Error);
                    return false;
                }

                Slogger.Write($"Enum device count : {devInfoList.Count}");

                if (0 == devInfoList.Count)
                {
                    return false;
                }

                int selDevIndex = -1;

                int devIndex = 0;
                foreach (var devInfo in devInfoList)
                {
                    Console.WriteLine("[Device {0}] : ", devIndex);
                    if (devInfo.TLayerType == DeviceTLayerType.MvGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvVirGigEDevice || devInfo.TLayerType == DeviceTLayerType.MvGenTLGigEDevice)
                    {
                        IGigEDeviceInfo gigeDevInfo = devInfo as IGigEDeviceInfo;
                        uint nIp1 = ((gigeDevInfo.CurrentIp & 0xff000000) >> 24);
                        uint nIp2 = ((gigeDevInfo.CurrentIp & 0x00ff0000) >> 16);
                        uint nIp3 = ((gigeDevInfo.CurrentIp & 0x0000ff00) >> 8);
                        uint nIp4 = (gigeDevInfo.CurrentIp & 0x000000ff);

                        string strIP = nIp1 + "." + nIp2 + "." + nIp3 + "." + nIp4;
                        Slogger.Write($"Device {devIndex}, DevIP : " + strIP);

                        if (_strIpAddr is null || strIP == strIpAddr)
                        {
                            selDevIndex = devIndex;
                            break;
                        }
                    }

                    Slogger.Write("ModelName:" + devInfo.ModelName);
                    Slogger.Write("SerialNumber:" + devInfo.SerialNumber);
                    devIndex++;
                }

                if (selDevIndex < 0 || selDevIndex > devInfoList.Count - 1)
                {
                    Slogger.Write($"Invalid selected device number:{selDevIndex}", Slogger.LogType.Error);
                    return false;
                }

                _device = DeviceFactory.CreateDevice(devInfoList[selDevIndex]);

                _disposed = false;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
            return true;
        }

        internal override bool Grab(int bufferIndex, bool waitDone)
        {
            if (_device == null)
                return false;

            BufferIndex = bufferIndex;
            bool ret = true;

            if (!HardwareTrigger)
            {
                try
                {
                    int result = _device.Parameters.SetCommandValue("TriggerSoftware");
                    if (result != MvError.MV_OK)
                    {
                        ret = false;
                    }
                }
                catch { ret = false; }
            }
            return ret;
        }

        internal override bool Close()
        {
            if (_device != null)
            {
                _device.StreamGrabber.StopGrabbing();
                _device.Close();
            }
            return true;
        }

        internal override bool Open()
        {
            try
            {
                if (_device == null)
                    return false;
                if (!_device.IsConnected)
                {
                    int ret = _device.Open();
                    if (ret != MvError.MV_OK)
                    {
                        _device.Dispose();
                        Slogger.Write("Open device failed!", Slogger.LogType.Error);
                        MessageBox.Show($"Device open fail! {ret:X8}");
                        return false;
                    }

                    if (_device is IGigEDevice)
                    {
                        int packetSize;
                        ret = (_device as IGigEDevice).GetOptimalPacketSize(out packetSize);
                        if (packetSize > 0)
                        {
                            ret = _device.Parameters.SetIntValue("GevSCPSPacketSize", packetSize);
                            if (ret != MvError.MV_OK)
                            {
                                Slogger.Write($"Warning: Set Packet Size failed {ret:x8}", Slogger.LogType.Error);
                            }
                            else
                            {
                                Slogger.Write($"Set PacketSize to {packetSize}");
                            }
                        }
                        else
                        {
                            Slogger.Write($"Warning: Get Packet Size failed {ret:x8}", Slogger.LogType.Error);
                        }
                    }
                    ret = _device.Parameters.SetEnumValue("TriggerMode", 1);
                    if (ret != MvError.MV_OK)
                    {
                        Slogger.Write($"Set Exposure Time Fail:{ret:x8}", Slogger.LogType.Error);
                        return false;
                    }

                    if (HardwareTrigger)
                    {
                        _device.Parameters.SetEnumValueByString("TriggerSource", "Line0");
                    }
                    else
                    {
                        _device.Parameters.SetEnumValueByString("TriggerSource", "Software");
                    }

                    // Register image callback
                    _device.StreamGrabber.FrameGrabedEvent += FrameGrabedEventHandler;

                    // start grab image
                    ret = _device.StreamGrabber.StartGrabbing();
                    if (ret != MvError.MV_OK)
                    {
                        Slogger.Write("Start grabbing failed:{0:x8}", Slogger.LogType.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Slogger.Write(ex.ToString(), Slogger.LogType.Error);
                return false;
            }
            return true;
        }
        internal override bool Reconnect()
        {
            if (_device != null)
            {
                Slogger.Write("_camera is null", Slogger.LogType.Error);
                return false;
            }
            Close();
            return Open();
        }

        internal override bool GetPixelBpp(out int pixelBpp)
        {
            pixelBpp = 8;
            if (_device == null)
                return false;

            IEnumValue enumValue;
            int result = _device.Parameters.GetEnumValue("PixelFormat", out enumValue);
            if (result != MvError.MV_OK)
            {
                Console.WriteLine("Get PixelFormat failed nRet:{0:X8}", result);
                return false;
            }
            if (MvGvspPixelType.PixelType_Gvsp_Mono8 == (MvGvspPixelType)enumValue.CurEnumEntry.Value)
                pixelBpp = 8;
            else
                pixelBpp = 24;

            return true;
        }

        internal override bool SetExposureTime(long exposure)
        {
            if (_device == null)
                return false;

            _device.Parameters.SetEnumValue("ExposureTime", 0);
            int result = _device.Parameters.SetFloatValue("ExposureTime", exposure);
            if (result != MvError.MV_OK)
            {
                Console.WriteLine("Set ExposureTime Failed!", result);
                return false;
            }
            return true;
        }

        internal override bool GetExposureTime(out long exposure)
        {
            exposure = 0;
            if (_device == null)
                return false;

            IFloatValue floatValue;
            int result = _device.Parameters.GetFloatValue("ExposureTime", out floatValue);
            if (result != MvError.MV_OK)
            {
                exposure = (long)floatValue.CurValue;
            }

            return true;
        }

        internal override bool SetGain(long gain)
        {
            if (_device == null)
                return false;

            _device.Parameters.SetEnumValue("GainAuto", 0);
            int result = _device.Parameters.SetFloatValue("Gain", gain);
            if (result != MvError.MV_OK)
            {
                Slogger.Write($"Set Gain Fail:{result:x8}", Slogger.LogType.Error);
                return false;
            }

            return true;
        }

        internal override bool GetGain(out long gain)
        {
            gain = 0;
            if (_device == null)
                return false;
            IFloatValue floatValue;
            int result = _device.Parameters.GetFloatValue("Gain", out floatValue);
            if (result != MvError.MV_OK)
            {
                gain = (long)floatValue.CurValue;
            }
            return true;
        }

        internal override bool GetResolution(out int width, out int height, out int stride)
        {
            width = 0;
            height = 0;
            stride = 0;

            if (_device == null)
                return false;

            IIntValue intValue;
            IEnumValue enumValue;
            MvGvspPixelType pixelType;

            int result;

            result = _device.Parameters.GetIntValue("Width", out intValue);
            if (result != MvError.MV_OK)
            {
                Slogger.Write($"Get Width Fail:{result:x8}", Slogger.LogType.Error);
                return false;
            }
            width = (int)intValue.CurValue;

            result = _device.Parameters.GetIntValue("Height", out intValue);
            if (result != MvError.MV_OK)
            {
                Slogger.Write($"Get Height Fail:{result:x8}", Slogger.LogType.Error);
                return false;
            }
            height = (int)intValue.CurValue;

            result = _device.Parameters.GetEnumValue("PixelFormat", out enumValue);
            if (result != MvError.MV_OK)
            {
                Slogger.Write($"Get PixelFormat Fail:{result:x8}", Slogger.LogType.Error);
                return false;
            }

            pixelType = (MvGvspPixelType)enumValue.CurEnumEntry.Value;

            if (pixelType == MvGvspPixelType.PixelType_Gvsp_Mono8)
                stride = width * 1;
            else
                stride = width * 3;
            return true;
        }

        internal override bool SetTriggerMode(bool hardwareTrigger)
        {
            if (_device == null)
                return false;

            HardwareTrigger = hardwareTrigger;

            if (HardwareTrigger)
            {
                _device.Parameters.SetEnumValueByString("TriggerSource", "Line0");
            }
            else
            {
                _device.Parameters.SetEnumValueByString("TriggerSource", "Software");
            }

            return true;
        }

        private bool _disposed = false;

        protected void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_device != null)
                {
                    _device.StreamGrabber.FrameGrabedEvent -= FrameGrabedEventHandler;
                    _device.StreamGrabber.StopGrabbing();
                    _device.Close();
                    _device.Dispose();
                    _device = null;

                    SDKSystem.Finalize();
                }
            }
            _disposed = true;
        }
        internal override void Dispose()
        {
            Dispose(disposing: true);
        }
    }
}

