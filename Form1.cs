using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Collections.ObjectModel;

using HalconDotNet;
using DeviceSource;
using MvCamCtrl.NET;

using FtdAdapter;
using Modbus.Data;
using Modbus.Device;
using Modbus.Utility;


namespace PlaceInspect
{
    public partial class Form1 : Form
    {
        MyCamera.MV_CC_DEVICE_INFO_LIST m_pDeviceList;
        CameraOperator m_pOperator;
        bool m_bGrabbing;
        bool m_bCali = false;
        bool m_bBigblackspot = true;  
        bool m_bSmallblackspot = true;

        float m_fMmpixel;
        float m_Caliradius;
        
        int m_iThreshold;
        int m_iMedianImageSize;
        int m_iDynOffset;
        float m_fSpotAreaBig;
        float m_fSpotAreaSmall;


        private uint iWidth;
        private uint iHeigth;

        UInt32 m_nBufSizeForDriver;
        byte[] m_pBufForDriver;

        UInt32 m_nBufSizeForSaveImage;
        byte[] m_pBufForSaveImage;

        byte[] m_pDataForRed;
        byte[] m_pDataForGreen;
        byte[] m_pDataForBlue;
        int m_nDataLenForRed = 0;
        int m_nDataLenForGreen = 0;
        int m_nDataLenForBlue = 0;

 


        HWindow m_Window;
        MyCamera.MV_SAVE_IAMGE_TYPE m_nSaveImageType;
        MyCamera.cbOutputExdelegate ImageCallback;
        INIClass IniFile = new INIClass();
        
        
        public Form1()
        {
            InitializeComponent();
            AutoAdjustFormSize();
            Control.CheckForIllegalCrossThreadCalls = false; //允许跨线程访问UI控件  
            IniFile.IniFiles(IniFile.IniFilePath);
            
            UpdateParam();

            m_pDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            m_pOperator = new CameraOperator();
            m_bGrabbing = false;
            m_nSaveImageType = (MyCamera.MV_SAVE_IAMGE_TYPE)0;
            m_Window = new HWindow();
            DisplayWindowsInitial();
            DeviceListAcq();
            Result_label.Text = "OK";

     

        }
        /********************************************************************************************/

        private void DisplayWindowsInitial()
        {
            // ch: 定义显示的起点和宽高 || en: Definition the width and height of the display window
            HTuple hWindowRow, hWindowColumn, hWindowWidth, hWindowHeight;

            // ch: 设置显示窗口的起点和宽高 || en: Set the width and height of the display window
            hWindowRow = 0;
            hWindowColumn = 0;
            hWindowWidth = pictureBox1.Width;
            hWindowHeight = pictureBox1.Height;

            try
            {
                HTuple hWindowID = (HTuple)pictureBox1.Handle;
                m_Window.OpenWindow(hWindowRow, hWindowColumn, hWindowWidth, hWindowHeight, hWindowID, "visible", "");
                //HOperatorSet.SetLineWidth(m_Window, 2);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

        }
        private void bnEnum_Click(object sender, EventArgs e)
        {
            DeviceListAcq();
        }

        private void DeviceListAcq()
        {
            int nRet;
            // ch:创建设备列表 || en: Create device list
            System.GC.Collect();
            cbDeviceList.Items.Clear();
            nRet = CameraOperator.EnumDevices(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_pDeviceList);
            if (0 != nRet)
            {
                MessageBox.Show("枚举设备失败!");
                return;
            }

            // ch:在窗体列表中显示设备名 || Display the device'name on window's list
            for (int i = 0; i < m_pDeviceList.nDeviceNum; i++)
            {
                MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                    if (gigeInfo.chUserDefinedName != "")
                    {
                        cbDeviceList.Items.Add("GigE: " + gigeInfo.chUserDefinedName + " (" + gigeInfo.chSerialNumber + ")");
                    }
                    else
                    {
                        cbDeviceList.Items.Add("GigE: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")");
                    }
                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stUsb3VInfo, 0);
                    MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                    if (usbInfo.chUserDefinedName != "")
                    {
                        cbDeviceList.Items.Add("USB: " + usbInfo.chUserDefinedName + " (" + usbInfo.chSerialNumber + ")");
                    }
                    else
                    {
                        cbDeviceList.Items.Add("USB: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")");
                    }
                }
            }

            //.ch: 选择第一项 || en: Select the first item
            if (m_pDeviceList.nDeviceNum != 0)
            {
                cbDeviceList.SelectedIndex = 0;
            }
        }
        private void bnOpen_Click(object sender, EventArgs e)
        {
            if (m_pDeviceList.nDeviceNum == 0 || cbDeviceList.SelectedIndex == -1)
            {
                MessageBox.Show("无设备，请选择");
                return;
            }

            int nRet = -1;
            //ch:获取选择的设备信息 | en:Get selected device information
            MyCamera.MV_CC_DEVICE_INFO device =
                (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[cbDeviceList.SelectedIndex],
                                                              typeof(MyCamera.MV_CC_DEVICE_INFO));

            // ch:打开设备 | en:Open device
            nRet = m_pOperator.Open(ref device);
            if (MyCamera.MV_OK != nRet)
            {
                MessageBox.Show("设备打开失败!");
                return;
            }
            DisplayLog("打开相机成功!",Color.Green);

            m_pOperator.GetIntValue("WidthMax", ref iWidth);
            m_pOperator.GetIntValue("HeightMax", ref iHeigth);

            m_nBufSizeForDriver = iWidth * iHeigth * 3;
            m_pBufForDriver = new byte[iWidth * iHeigth * 3];

            m_nBufSizeForSaveImage = iWidth * iHeigth * 3 * 3 + 2048;
            m_pBufForSaveImage = new byte[iWidth * iHeigth * 3 * 3 + 2048];

            m_pDataForRed = new byte[iWidth * iHeigth];
            m_pDataForGreen = new byte[iWidth * iHeigth];
            m_pDataForBlue = new byte[iWidth * iHeigth];

            // ch:设置触发模式为off || en:set trigger mode as off
            m_pOperator.SetEnumValue("AcquisitionMode", 2);// 工作在连续模式
            m_pOperator.SetEnumValue("TriggerMode", 1);    //  触发模式

            /**********************************************************************************************************/
            // ch:注册回调函数 | en:Register image callback
            ImageCallback = new MyCamera.cbOutputExdelegate(GrabImage);

            nRet = m_pOperator.RegisterImageCallBackForRGB(ImageCallback, IntPtr.Zero);
            if (CameraOperator.CO_OK != nRet)
            {
                MessageBox.Show("注册回调失败!");
            }
            /**********************************************************************************************************/
            SetCtrlWhenOpen();

            nRet = m_pOperator.SetFloatValue("ExposureTime", (float)(IniFile.ReadDouble("camera", "ExposureTime", 0, IniFile.IniFilePath)));
            if (nRet != CameraOperator.CO_OK)
            {MessageBox.Show("设置曝光时间失败！");}

            nRet = m_pOperator.SetFloatValue("Gain", (float)(IniFile.ReadDouble("camera", "Gain", 0, IniFile.IniFilePath)));
            if (nRet != CameraOperator.CO_OK)
            {MessageBox.Show("设置增益失败！");}

            nRet = m_pOperator.SetFloatValue("TriggerDelay", (float)(IniFile.ReadDouble("camera", "TriggerDelay", 0, IniFile.IniFilePath)));
            if (nRet != CameraOperator.CO_OK)
            {MessageBox.Show("设置触发延时失败！");}

            bnGetParam_Click(null, EventArgs.Empty);
        }

        /**********************************************************************************************************/
        private void GrabImage(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            HTuple SecondsStart;
            HTuple SecondsEnd;
            HTuple SecondsPassed;

            HOperatorSet.CountSeconds(out SecondsStart);

            if (pData != null)
            {
                Marshal.Copy(pData, m_pBufForSaveImage, 0, (int)(iWidth * iHeigth * 3));
                HObject Himage = new HObject();

                UInt32 nSupWidth = (pFrameInfo.nWidth + (UInt32)3) & 0xfffffffc;//宽度补齐为4的倍数
                Int32 nLength = (Int32)pFrameInfo.nWidth * (Int32)pFrameInfo.nHeight;

                RellocBuf(m_pDataForRed, m_nDataLenForRed, nLength);

                RellocBuf(m_pDataForGreen, m_nDataLenForGreen, nLength);

                RellocBuf(m_pDataForBlue, m_nDataLenForBlue, nLength);

                for (int nRow = 0; nRow < pFrameInfo.nHeight; nRow++)
                {
                    for (int col = 0; col < pFrameInfo.nWidth; col++)
                    {
                        m_pDataForRed[nRow * nSupWidth + col] = m_pBufForSaveImage[nRow * pFrameInfo.nWidth * 3 + (3 * col)];
                        m_pDataForGreen[nRow * nSupWidth + col] = m_pBufForSaveImage[nRow * pFrameInfo.nWidth * 3 + (3 * col + 1)];
                        m_pDataForBlue[nRow * nSupWidth + col] = m_pBufForSaveImage[nRow * pFrameInfo.nWidth * 3 + (3 * col + 2)];
                    }
                }

                IntPtr RedPtr = BytesToIntptr(m_pDataForRed);
                IntPtr GreenPtr = BytesToIntptr(m_pDataForGreen);
                IntPtr BluePtr = BytesToIntptr(m_pDataForBlue);
                try
                {
                    HOperatorSet.GenImage3(out Himage, (HTuple)"byte", pFrameInfo.nWidth, pFrameInfo.nHeight,
                                        (new HTuple(RedPtr)), (new HTuple(GreenPtr)), (new HTuple(BluePtr)));
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                // ch: 显示 || display
                HTuple hImageWidth = 0;
                HTuple hImageHeight = 0;
                HTuple point = null;
                HTuple type = null;

                try
                {
                    HOperatorSet.GetImagePointer1(Himage, out point, out type, out hImageWidth, out hImageHeight);//.ch: 得到图像的宽高和指针 || en: Get the width and height of the image
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show(ex.ToString());
                    return;
                }
                try
                {
                    HOperatorSet.SetPart(m_Window, 0, 0, hImageHeight, hImageWidth);// ch: 使图像显示适应窗口大小 || en: Make the image adapt the window size
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show(ex.ToString());
                    return;
                }
                if (m_bGrabbing)
                {
                    if (m_Window == null)
                    {
                        return;
                    }
                    try
                    {
                        HOperatorSet.ClearWindow(m_Window);

                        if (m_bCali)
                        {
                            m_bCali = false;
                            CaliImageProcess(Himage);
                            return;
                        }

                        if (m_bBigblackspot || m_bSmallblackspot)
                        {
                            ImageProcess(Himage);
                        }

                        else
                        {
                            HOperatorSet.DispObj(Himage, m_Window);
                        }
                     
                        Himage.Dispose();
                        HOperatorSet.CountSeconds(out SecondsEnd);
                        HOperatorSet.TupleSub(SecondsEnd, SecondsStart, out SecondsPassed);
                        HOperatorSet.SetColor(m_Window, "red");
                        HOperatorSet.SetTposition(m_Window, 100, 100); //在图像上显示Index
                        HOperatorSet.WriteString(m_Window, SecondsPassed.ToString());


                        Marshal.FreeHGlobal(RedPtr);    // ch 释放空间 || en: release space
                        Marshal.FreeHGlobal(GreenPtr);  // ch 释放空间 || en: release space
                        Marshal.FreeHGlobal(BluePtr);   // ch 释放空间 || en: release space
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        MessageBox.Show(ex.ToString());
                        return;
                    }
                }
                if (m_nSaveImageType != (MyCamera.MV_SAVE_IAMGE_TYPE)0)
                {
                    return;
                }
            }
            return;
        }

        private bool CheckBlackSpot(ref HObject ho_Region4List)
        {
            HTuple Area,AreaSorted;
            HObject ho_Regionbig = null;
            HOperatorSet.GenEmptyObj(out ho_Regionbig);

            HObject ho_Regionsmall = null;
            HOperatorSet.GenEmptyObj(out ho_Regionsmall);
            
            HObject ho_Region4ListConnected = null;
            HOperatorSet.GenEmptyObj(out ho_Region4ListConnected);
            HObject ho_Region4ListConnectedSorted = null;
            HOperatorSet.GenEmptyObj(out ho_Region4ListConnectedSorted);
            
            HOperatorSet.SetColor(m_Window, "green");

            try
            {
                HOperatorSet.Connection(ho_Region4List, out ho_Region4ListConnected);
                HOperatorSet.RegionFeatures(ho_Region4ListConnected, "area", out Area);

                HOperatorSet.TupleSort(Area, out AreaSorted);
                Result_listView.Items.Clear();

                for (int i = 0; i < Area.TupleLength(); i++)
                {
                    float tmp, tmp1;
                    ListViewItem lt = new ListViewItem(i.ToString());
                    tmp = AreaSorted.ToFArr()[i];
                    tmp1 = tmp * m_fMmpixel;
                    lt.SubItems.Add((AreaSorted.ToFArr()[i] * m_fMmpixel).ToString("F2") + "(" + AreaSorted.ToFArr()[i].ToString() + ")");
                    Result_listView.Items.Add(lt);
                }

                HOperatorSet.DispObj(ho_Region4List, m_Window);
                Result_label.Text = "OK";
                Result_label.BackColor = Color.Green;
                
                HOperatorSet.SelectShape(ho_Region4ListConnected, 
                                         out ho_Regionbig, 
                                         "area", 
                                         "and",
                                  m_fSpotAreaBig / m_fMmpixel,
                                     999999999);

                bool btmp = false;
         
                if (ho_Regionbig.CountObj() > 0)
                {
                    HOperatorSet.SetColor(m_Window, "red");
                    HOperatorSet.DispObj(ho_Regionbig, m_Window);
                    HOperatorSet.SetColor(m_Window, "green");
                    Result_label.Text = "NG:大黑点";
                    Result_label.BackColor = Color.Red;
                    btmp = true;
                    DisplayLog("大黑点", Color.Red);
                    return true;
                }

                HOperatorSet.SelectShape(ho_Region4ListConnected,
                                         out ho_Regionsmall,
                                        "area",
                                        "and",
                                        m_fSpotAreaSmall / m_fMmpixel,
                                        1/m_fMmpixel);

                if (ho_Regionsmall.CountObj() > 0)
                {
                    HOperatorSet.SetColor(m_Window, "yellow");
                    HOperatorSet.DispObj(ho_Regionsmall, m_Window);
                    HOperatorSet.SetColor(m_Window, "green");

                    if (ho_Regionsmall.CountObj() > 3)
                    {
                        Result_label.BackColor = Color.Red;
                        if (btmp)
                        { 
                            Result_label.Text = "NG:大小黑点";
                            DisplayLog("大小黑点", Color.Red);
                        }
                        else
                        { 
                            Result_label.Text = "NG:小黑点";
                            DisplayLog("大小黑点", Color.Red);
                        }
                        return true;
                    }
                 }
            }
            catch (HalconException HDevExpDefaultException)
            {
                MessageBox.Show("error here");
                throw HDevExpDefaultException;
               // return false;
            }
            ho_Regionbig.Dispose();
            ho_Regionsmall.Dispose();
            ho_Region4ListConnected.Dispose();
            ho_Region4ListConnectedSorted.Dispose();
   
            return false;
        }

        private void ImageProcess(HObject ho_Image)
        {

            HObject ho_Region = null, ho_ConnectedRegions = null;
            HObject ho_SelectedRegions = null, ho_RegionFillUp = null, ho_RegionErosion = null;
            HObject ho_ImageReduced = null, ho_ImageReducedImageMean = null;
            HObject ho_RegionDynThresh = null, ho_RegionFillUp1 = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionErosion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImageReducedImageMean);
            HOperatorSet.GenEmptyObj(out ho_RegionDynThresh);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp1);
            try
            {
                ho_Image.DispObj(m_Window);
                HOperatorSet.SetDraw(m_Window, "margin");
                HOperatorSet.Threshold(ho_Image, out ho_Region, m_iThreshold, 255);
                HOperatorSet.ScaleImageMax(ho_Image, out ho_Image);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);

                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions,
                                        out ho_SelectedRegions,
                                        "area",
                                        "and",
                                        1500000,
                                        9999999);
                HOperatorSet.SetColor(m_Window, "blue");

                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_SelectedRegions, out ho_RegionFillUp);
                ho_RegionFillUp.DispObj(m_Window);

                ho_RegionErosion.Dispose();
                HOperatorSet.ErosionCircle(ho_RegionFillUp, out ho_RegionErosion, 10);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionErosion, out ho_ImageReduced);
            
                /////完成ROI取出/////
                
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 0, m_iThreshold-1);
                HOperatorSet.FillUp(ho_Region, out ho_Region);
                HOperatorSet.OpeningCircle(ho_Region, out ho_Region, 3);

                if (CheckBlackSpot(ref ho_Region))
                { 
                    return; 
                }

                ho_ImageReducedImageMean.Dispose();
                HOperatorSet.MedianImage(    ho_ImageReduced,
                                         out ho_ImageReducedImageMean,
                                             "circle",
                                             m_iMedianImageSize,
                                             "mirrored");
                ho_RegionDynThresh.Dispose();
                HOperatorSet.DynThreshold(  ho_ImageReduced, 
                                            ho_ImageReducedImageMean, 
                                            out ho_RegionDynThresh,
                                            m_iDynOffset, 
                                            "dark");
                
                 //HOperatorSet.ClosingCircle(ho_RegionDynThresh,out ho_RegionDynThresh,3);
                 HOperatorSet.OpeningCircle(ho_RegionDynThresh, out ho_RegionDynThresh, 2);
                 HOperatorSet.Connection(ho_RegionDynThresh,out ho_RegionDynThresh);

                 HOperatorSet.SelectShape(ho_RegionDynThresh, 
                                         out ho_RegionDynThresh, 
                                         "area", 
                                         "and",
                                  m_fSpotAreaBig / m_fMmpixel,
                                     999999999);

                 if (CheckBlackSpot(ref ho_RegionDynThresh))
                {
                    return;
                }


               // UpdataList(ref ho_RegionFillUp1);
                
            }


            catch (HalconException HDevExpDefaultException)
            {
                ho_Image.Dispose();
                ho_Region.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionErosion.Dispose();
                ho_ImageReduced.Dispose();
                ho_ImageReducedImageMean.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_RegionFillUp1.Dispose();

                throw HDevExpDefaultException;
            }
            
            ho_Image.Dispose();
            ho_Region.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionFillUp.Dispose();
            ho_RegionErosion.Dispose();
            ho_ImageReduced.Dispose();
            ho_ImageReducedImageMean.Dispose();
            ho_RegionDynThresh.Dispose();
            ho_RegionFillUp1.Dispose();

        }

        private void CaliImageProcess(HObject ho_Image)
        {
            try
            {
                HObject ho_Region, ho_ConnectedRegions;
                HObject ho_SelectedRegions;
                HTuple hv_Value = null, hv_Mean = null, hv_demo = null;

                HOperatorSet.GenEmptyObj(out ho_Region);
                HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
                HOperatorSet.GenEmptyObj(out ho_SelectedRegions);

                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_Image, out ho_Region, 0, 50);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
                
                //ho_ConnectedRegions.DispObj(m_Window);
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(   ho_ConnectedRegions,
                                            out ho_SelectedRegions,
                                            (new HTuple("area")).TupleConcat("roundness"),
                                            "and",
                                            (new HTuple(6200 * 0.8)).TupleConcat(0.9),
                                            (new HTuple(6200 * 1.2)).TupleConcat(1));
                ho_SelectedRegions.DispObj(m_Window);


                HOperatorSet.RegionFeatures(ho_SelectedRegions, "area", out hv_Value);
                HOperatorSet.TupleMean(hv_Value, out hv_Mean);
                HOperatorSet.TupleDiv((3.1415 * m_Caliradius) * m_Caliradius, hv_Mean, out hv_demo);

                IniFile.WriteDouble("Cali", "Mmpixel", hv_demo.ToDArr()[0], IniFile.IniFilePath);



               // ho_Image.DispObj(m_Window);
                m_Window.SetColored(12);
              //  ho_SelectedRegions.DispObj(m_Window);

                ho_Image.Dispose();
                ho_Region.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose(); 

            }
            catch (HalconException e)
            {
                MessageBox.Show("请检查标定板是否放置正确！"+e.ToString());
    
            }
            
        }

        public static IntPtr BytesToIntptr(byte[] bytes)
        {
            int size = bytes.Length;
            IntPtr buffer = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, buffer, size);
            return buffer;
        }
        /**********************************************************************************************************/
        private void RellocBuf(byte[] pBuffer, int nBufSize, int nDstBufSize)
        {
            if (null == pBuffer)
            {
                pBuffer = new byte[nDstBufSize];
                if (null == pBuffer)
                {
                    return;
                }

                nBufSize = nDstBufSize;
            }
            else
            {
                if (nBufSize != nDstBufSize)
                {
                    pBuffer = new byte[nDstBufSize]; ;
                    if (null == pBuffer)
                    {
                        return;
                    }
                    nBufSize = nDstBufSize;
                }
            }
            return;
        }

        private void SetCtrlWhenOpen()
        {
            bnOpen.Enabled = false;

            bnCloseCamera.Enabled = true;
            bnStartGrab.Enabled = true;
            bnStopGrab.Enabled = false;
            
            bnContinuesMode.Enabled = true;
            bnContinuesMode.Checked = false;
            
            bnTriggerMode.Enabled = true;
            bnTriggerMode.Checked = true;
                        
            bnTriggerExec.Enabled = false;
          
            cbSoftTrigger.Enabled = true;

            cbCaliEnable.Enabled = true;

            //////////////////////////
            tbExposure.Enabled = true;
            tbGain.Enabled = true;
            tbTriggerDelay.Enabled = true;
            bnGetParam.Enabled = true;
            bnSetParam.Enabled = true;

        }

        private void SetCtrlWhenClose()
        {
            bnOpen.Enabled = true;

            bnCloseCamera.Enabled = false;
            bnStartGrab.Enabled = false;
            bnStopGrab.Enabled = false;
            bnContinuesMode.Enabled = false;
            bnTriggerMode.Enabled = false;
            cbSoftTrigger.Enabled = false;
            bnTriggerExec.Enabled = false;
            
            cbCaliEnable.Enabled = false;
            bnTriggerCali.Enabled = false;
       

            tbExposure.Enabled = false;
            tbGain.Enabled = false;
            tbTriggerDelay.Enabled = false;

            bnGetParam.Enabled = false;
            bnSetParam.Enabled = false;

        }

        private void bnClose_Click(object sender, EventArgs e)
        {
            int nRet = -1;
            // ch:停止抓图 || en:Stop grab image
            nRet = m_pOperator.Close();
            if (nRet != CameraOperator.CO_OK)
            {
                MessageBox.Show("停止取流失败！");
                return;
            }
            SetCtrlWhenClose();
        }

        private void bnContinuesMode_CheckedChanged(object sender, EventArgs e)
        {
            if (bnContinuesMode.Checked)
            {
                m_pOperator.SetEnumValue("TriggerMode", 0);
                cbSoftTrigger.Enabled = false;
                bnTriggerExec.Enabled = false;
            }
        }

        private void bnTriggerMode_CheckedChanged(object sender, EventArgs e)
        {
            if (bnTriggerMode.Checked)
            {
                m_pOperator.SetEnumValue("TriggerMode", 1);

                // ch: 触发源选择:0 - Line0 || en :TriggerMode select;
                //           1 - Line1;
                //           2 - Line2;
                //           3 - Line3;
                //           4 - Counter;
                //           7 - Software;
                if (cbSoftTrigger.Checked)
                {
                    m_pOperator.SetEnumValue("TriggerSource", 7);
                    if (m_bGrabbing)
                    {
                        bnTriggerExec.Enabled = true;
                    }
                }
                else
                {
                    m_pOperator.SetEnumValue("TriggerSource", 0);
                }
                cbSoftTrigger.Enabled = true;
            }
        }

        private void SetCtrlWhenStartGrab()
        {


            bnStartGrab.Enabled = false;
            bnStopGrab.Enabled = true;

            if (bnTriggerMode.Checked && cbSoftTrigger.Checked)
            {
                bnTriggerExec.Enabled = true;
            }

        }

        private void bnStartGrab_Click(object sender, EventArgs e)
        {
            int nRet;
            // ch:开启抓图 | en:start grab
            nRet = m_pOperator.StartGrabbing();
            if (MyCamera.MV_OK != nRet)
            {
                MessageBox.Show("开始取流失败！");
                return;
            }
            // ch: 控件操作 || en: Control operation
            SetCtrlWhenStartGrab();
            m_bGrabbing = true;
        }

        private void cbSoftTrigger_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSoftTrigger.Checked)
            {
                // ch: 触发源设为软触发 || en: set trigger mode as Software
                m_pOperator.SetEnumValue("TriggerSource", 7);
                if (m_bGrabbing)
                {
                    bnTriggerExec.Enabled = true;
                }
            }
            else
            {
                m_pOperator.SetEnumValue("TriggerSource", 0);
                bnTriggerExec.Enabled = false;
            }
        }

        private void bnTriggerExec_Click(object sender, EventArgs e)
        {
            int nRet;

            // ch: 触发命令 || en: Trigger command
            nRet = m_pOperator.CommandExecute("TriggerSoftware");
            if (CameraOperator.CO_OK != nRet)
            {
                MessageBox.Show("触发失败！");
            }
        }

        private void SetCtrlWhenStopGrab()
        {
            bnStartGrab.Enabled = true;
            bnStopGrab.Enabled = false;
            bnTriggerExec.Enabled = false;
        }

        private void bnStopGrab_Click(object sender, EventArgs e)
        {
            int nRet = -1;
            // ch:停止抓图 || en:Stop grab image
            nRet = m_pOperator.StopGrabbing();
            if (nRet != CameraOperator.CO_OK)
            {
                MessageBox.Show("停止取流失败！");
            }
            m_bGrabbing = false;
            // ch: 控件操作 || en: Control operation
            SetCtrlWhenStopGrab();
        }

        private void bnGetParam_Click(object sender, EventArgs e)
        {
            float fExposure = 0;
            m_pOperator.GetFloatValue("ExposureTime", ref fExposure);
            tbExposure.Text = fExposure.ToString("F1");

            float fGain = 0;
            m_pOperator.GetFloatValue("Gain", ref fGain);
            tbGain.Text = fGain.ToString("F1");

            float fTriggerDelay = 0;
            m_pOperator.GetFloatValue("TriggerDelay", ref fTriggerDelay);
            tbTriggerDelay.Text = fTriggerDelay.ToString("F1");

        }

        private void bnSetParam_Click(object sender, EventArgs e)
        {
            int nRet;
            m_pOperator.SetEnumValue("ExposureAuto", 0); //非自动曝光
            try
            {
                float.Parse(tbExposure.Text);
                float.Parse(tbGain.Text);
                float.Parse(tbTriggerDelay.Text);

                IniFile.WriteDouble("camera", "ExposureTime", float.Parse(tbExposure.Text),     IniFile.IniFilePath);
                IniFile.WriteDouble("camera", "Gain",         float.Parse(tbGain.Text),         IniFile.IniFilePath);
                IniFile.WriteDouble("camera", "TriggerDelay", float.Parse(tbTriggerDelay.Text), IniFile.IniFilePath);
                MessageBox.Show("相机设置已经保存！");
            }
            catch
            {
                MessageBox.Show("请输入正确类型!");
                return;
            }

            nRet = m_pOperator.SetFloatValue("ExposureTime", float.Parse(tbExposure.Text));
            if (nRet != CameraOperator.CO_OK)
            {
                MessageBox.Show("设置曝光时间失败！");
            }

            m_pOperator.SetEnumValue("GainAuto", 0);
            nRet = m_pOperator.SetFloatValue("Gain", float.Parse(tbGain.Text));
            if (nRet != CameraOperator.CO_OK)
            {
                MessageBox.Show("设置增益失败！");
            }

            nRet = m_pOperator.SetFloatValue("TriggerDelay", float.Parse(tbTriggerDelay.Text));
            if (nRet != CameraOperator.CO_OK)
            {
                MessageBox.Show("设置触发延时失败！");
            }

        }

        private void cbBigblackspot_CheckedChanged(object sender, EventArgs e)
        {
            if (cbBigblackspot.Checked)
            { m_bBigblackspot = true; }
            else
            { m_bBigblackspot = false; }
        }

        private void cbSmallblackspot_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSmallblackspot.Checked)
            { m_bSmallblackspot = true; }
            else
            { m_bSmallblackspot = false; }
         }


        //自动调整窗口大小
        public void AutoAdjustFormSize()
        {
            Screen screen = Screen.PrimaryScreen;
            int ScreenWidth = screen.Bounds.Width;
            int ScreenHeight = screen.Bounds.Height;

            if (ScreenWidth == 1920 && ScreenHeight == 1080)
            {
                AutoReSizeForm.SetFormSize(this, 1.18F, 1.34F, 1.05F);
            }

            if (ScreenWidth == 1680 && ScreenHeight == 1050)
            {
                AutoReSizeForm.SetFormSize(this, 1.06F, 1.25F, 1.05F);
            }

            if (ScreenWidth == 1600 && ScreenHeight == 1024)
            {
                AutoReSizeForm.SetFormSize(this, 0.98F, 1.28F, 1.05F);
            }

            if (ScreenWidth == 1600 && ScreenHeight == 900)
            {
                AutoReSizeForm.SetFormSize(this, 0.98F, 1.1F, 1.05F);
            }

            if (ScreenWidth == 1440 && ScreenHeight == 900)
            {
                AutoReSizeForm.SetFormSize(this, 0.88F, 1.1F, 1.05F);
            }

            if (ScreenWidth == 1366 && ScreenHeight == 768)
            {
                AutoReSizeForm.SetFormSize(this, 0.84F, 0.9F, 1.05F);
            }

            if (ScreenWidth == 1360 && ScreenHeight == 768)
            {
                AutoReSizeForm.SetFormSize(this, 0.84F, 0.9F, 1.05F);
            }

            if (ScreenWidth == 1280 && ScreenHeight == 1024)
            {
                AutoReSizeForm.SetFormSize(this, 0.79F, 1.25F, 1.05F);
            }

            if (ScreenWidth == 1280 && ScreenHeight == 960)
            {
                AutoReSizeForm.SetFormSize(this, 0.78F, 1.18F, 1.05F);
            }

            if (ScreenWidth == 1280 && ScreenHeight == 800)
            {
                AutoReSizeForm.SetFormSize(this, 0.78F, 0.96F, 1.05F);
            }

            if (ScreenWidth == 1280 && ScreenHeight == 768)
            {
                AutoReSizeForm.SetFormSize(this, 0.78F, 0.92F, 1.05F);
            }

            if (ScreenWidth == 1280 && ScreenHeight == 720)
            {
                AutoReSizeForm.SetFormSize(this, 0.78F, 0.85F, 1.05F);
            }

            if (ScreenWidth == 1280 && ScreenHeight == 600)
            {
                AutoReSizeForm.SetFormSize(this, 0.78F, 0.7F, 1.05F);
            }

            if (ScreenWidth == 1152 && ScreenHeight == 864)
            {
                AutoReSizeForm.SetFormSize(this, 0.70F, 1.05F, 1.05F);
            }

            if (ScreenWidth == 1024 && ScreenHeight == 768)
            {
                AutoReSizeForm.SetFormSize(this, 0.63F, 0.92F, 1.05F);
            }

            if (ScreenWidth == 800 && ScreenHeight == 600)
            {
                AutoReSizeForm.SetFormSize(this, 0.5F, 0.68F, 1.05F);
            }

        }

        //分辨率类
        public class AutoReSizeForm
        {
            public static void SetFormSize(Control fm, float SW, float SH, float FontSize)
            {

                fm.Location = new Point((int)(fm.Location.X * SW), (int)(fm.Location.Y * SH));
                fm.Size = new Size((int)(fm.Size.Width * SW), (int)(fm.Size.Height * SH));
                fm.Font = new Font(fm.Font.Name, fm.Font.Size * FontSize, fm.Font.Style, fm.Font.Unit, fm.Font.GdiCharSet, fm.Font.GdiVerticalFont);
                if (fm.Controls.Count != 0)
                {
                    SetControlSize(fm, SW, SH, FontSize);
                }
            }

            private static void SetControlSize(Control InitC, float SW, float SH, float FontSize)
            {

                foreach (Control c in InitC.Controls)
                {
                    c.Location = new Point((int)(c.Location.X * SW), (int)(c.Location.Y * SH));
                    c.Size = new Size((int)(c.Size.Width * SW), (int)(c.Size.Height * SH));
                    c.Font = new Font(c.Font.Name, c.Font.Size * FontSize, c.Font.Style, c.Font.Unit, c.Font.GdiCharSet, c.Font.GdiVerticalFont);
                    if (c.Controls.Count != 0)
                    {
                        SetControlSize(c, SW, SH, FontSize);
                    }
                }
            }

        }

        private void cbCaliEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCaliEnable.Checked)
            {
                bnTriggerCali.Enabled = true;
            }
            else
            {
                bnTriggerCali.Enabled = false;
            }
        }

        private void bnTriggerCali_Click(object sender, EventArgs e)
        {
            try
            {
               m_Caliradius = float.Parse(tbCaliRadius.Text);
                
               IniFile.WriteDouble("Cali", "Radius", float.Parse(tbCaliRadius.Text), IniFile.IniFilePath);
            }
            catch
            {
                MessageBox.Show("请输入正确类型!");
                return;
            }

            m_bCali = true;

            int nRet;
            nRet = m_pOperator.CommandExecute("TriggerSoftware");
            if (CameraOperator.CO_OK != nRet)
            {
                m_bCali = false;
                MessageBox.Show("触发失败！请开始采集并使能软触发！");
            }
          }

        public void DisplayLog(string LogContent, Color col)
        {
            Monitor.Enter(this);//锁定，保持同步
            string TotalLogContent = DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒   ") + LogContent + "\n";
            rtbLog.SelectionColor = col;
            rtbLog.AppendText(TotalLogContent);
            Monitor.Exit(this);//取消锁定
        }

        /// <summary>
        /// Simple Modbus serial RTU master write holding registers example.
        /// </summary>
        public void WriteSingleCoil(byte slaveId, ushort startAddress, bool status)
        {
            try
            {
                using (SerialPort port = new SerialPort("COM2"))
                {
                    // configure serial port
                    port.BaudRate = 9600;
                    port.DataBits = 8;
                    port.Parity = Parity.None;
                    port.StopBits = StopBits.One;
                    port.ReadTimeout = 1000;
                    port.Open();
                    // create modbus master
                    IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);
                    master.WriteSingleCoil(slaveId, startAddress, status);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("测试通讯失败!");
            }

         }

        private void button1_Click_1(object sender, EventArgs e)
        {
            WriteSingleCoil(1, 100, true);
        }

        private void bnCloseForm_Click(object sender, EventArgs e)
        {    
           m_pOperator.Close();
            this.Close();
        }

        private void bnUpdateParam_Click(object sender, EventArgs e)
        {
            UpdateParam();
        }
        private void UpdateParam()
        {
            m_Caliradius = (float)IniFile.ReadDouble("Cali", "Radius", 0, IniFile.IniFilePath);
            tbCaliRadius.Text = m_Caliradius.ToString();
            m_fMmpixel = (float)IniFile.ReadDouble("Cali", "Mmpixel", 0, IniFile.IniFilePath);

            m_iThreshold = IniFile.ReadInteger("Parameters", "Threshold", 0, IniFile.IniFilePath);
            m_iMedianImageSize = IniFile.ReadInteger("Parameters", "MedianImageSize", 0, IniFile.IniFilePath);
            m_iDynOffset = IniFile.ReadInteger("Parameters", "DynOffset", 0, IniFile.IniFilePath);
            m_fSpotAreaBig = (float)IniFile.ReadDouble("Parameters", "SpotAreaBig", 0, IniFile.IniFilePath);
            m_fSpotAreaSmall = (float)IniFile.ReadDouble("Parameters", "SpotAreaSmall", 0, IniFile.IniFilePath);

            MessageBox.Show("参数更新完毕!");

        }

    }
}
