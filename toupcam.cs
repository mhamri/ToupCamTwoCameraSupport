﻿using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Security.Permissions;
using System.Runtime.ConstrainedExecution;
using System.Drawing;

/*
    Versin: 1.1.4594.20141229
    
    We use P/Invoke to call into the toupcam.dll API, the c# class ToupCam is a thin wrapper class to the native api of toupcam.dll.
    So the manual en.html and hans.html are also applicable for programming with toupcam.cs.
    See it in the 'doc' directory.
*/
namespace ToupTek
{
    public class SafeHToupCamHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern void Toupcam_Close(IntPtr h);

        public SafeHToupCamHandle()
            : base(true)
        {
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        override protected bool ReleaseHandle()
        {
            // Here, we must obey all rules for constrained execution regions.
            Toupcam_Close(handle);
            return true;
        }
    };

    public class ToupCam : IDisposable
    {
        [Flags]
        public enum eFALG : uint
        {
            FLAG_CMOS = 0x0001,   /* cmos sensor */
            FLAG_CCD_PROGRESSIVE = 0x0002,   /* progressive ccd sensor */
            FLAG_CCD_INTERLACED = 0x0004,   /* interlaced ccd sensor */
            FLAG_MONO = 0x0010,   /* monochromatic */
            FLAG_BINSKIP_SUPPORTED = 0x0020,   /* support bin/skip mode */
            FLAG_USB30 = 0x0040,   /* USB 3.0 */
            FLAG_COOLED = 0x0080,   /* Cooled */
            FLAG_USB30_OVER_USB20 = 0x0100,   /* usb3.0 camera connected to usb2.0 port */
            FLAG_ST4 = 0x0200,   /* ST4 */
            FLAG_AD10 = 0x1000,   /* 10 bits A/D */
            FLAG_AD12 = 0x2000,   /* 12 bits A/D */
            FLAG_AD14 = 0x4000,   /* 14 bits A/D */
            FLAG_AD16 = 0x8000    /* 16 bits A/D */
        };

        public enum eEVENT : uint
        {
            EVENT_EXPOSURE = 0x0001, /* exposure time changed */
            EVENT_TEMPTINT = 0x0002, /* white balance changed */
            EVENT_CHROME = 0x0003, /* reversed, do not use it */
            EVENT_IMAGE = 0x0004, /* live image arrived, use Toupcam_PullImage to get this image */
            EVENT_STILLIMAGE = 0x0005, /* snap (still) frame arrived, use Toupcam_PullStillImage to get this frame */
            EVENT_ERROR = 0x0080, /* something error happens */
            EVENT_DISCONNECTED = 0x0081  /* camera disconnected */
        };

        public enum ePROCESSMODE : uint
        {
            PROCESSMODE_FULL = 0x00, /* better image quality, more cpu usage. this is the default value */
            PROCESSMODE_FAST = 0x01 /* lower image quality, less cpu usage */
        };

        public enum eOPTION : uint
        {
            OPTION_NOFRAME_TIMEOUT = 0x01,    /* iValue: 1 = enable; 0 = disable. default: enable */
            OPTION_THREAD_PRIORITY = 0x02,    /* set the priority of the internal thread which grab data from the usb device. iValue: 0 = THREAD_PRIORITY_NORMAL; 1 = THREAD_PRIORITY_ABOVE_NORMAL; 2 = THREAD_PRIORITY_HIGHEST; default: 0; see: msdn SetThreadPriority */
            OPTION_PROCESSMODE = 0x03,    /* 0 = better image quality, more cpu usage. this is the default value
                                                 1 = lower image quality, less cpu usage */
            OPTION_RAW = 0x04,    /* raw mode, read the sensor data. This can be set only BEFORE Toupcam_StartXXX() */
            OPTION_HISTOGRAM = 0x05     /* 0 = only one, 1 = continue mode */
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;

            public void Init()
            {
                biSize = (uint)Marshal.SizeOf(this);
            }
        }

        public struct Resolution
        {
            public uint width;
            public uint height;
        };
        public struct Model
        {
            public string name;
            public eFALG flag;
            public uint maxspeed;
            public uint preview;
            public uint still;
            public Resolution[] res;
        };
        public struct Instance
        {
            public string displayname; /* display name */
            public string id; /* unique and opaque id of a connected camera */
            public Model model;
        };

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory")]
        public static extern void CopyMemory(IntPtr Destination, IntPtr Source, uint Length);

        public delegate void DelegateEventCallback(eEVENT nEvent);
        public delegate void DelegateDataCallback(IntPtr pData, ref BITMAPINFOHEADER header, bool bSnap);
        public delegate void DelegateExposureCallback();
        public delegate void DelegateTempTintCallback(int nTemp, int nTint);
        public delegate void DelegateHistogramCallback(double[] aHistY, double[] aHistR, double[] aHistG, double[] aHistB);
        public delegate void DelegateChromeCallback();

        [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
        internal delegate void PTOUPCAM_DATA_CALLBACK(IntPtr pData, IntPtr pHeader, bool bSnap, IntPtr pCallbackCtx);
        [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
        internal delegate void PITOUPCAM_EXPOSURE_CALLBACK(IntPtr pCtx);
        [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
        internal delegate void PITOUPCAM_TEMPTINT_CALLBACK(int nTemp, int nTint, IntPtr pCtx);
        [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
        internal delegate void PITOUPCAM_HISTOGRAM_CALLBACK(IntPtr aHistY, IntPtr aHistR, IntPtr aHistG, IntPtr aHistB, IntPtr pCtx);
        [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
        internal delegate void PITOUPCAM_CHROME_CALLBACK(IntPtr pCtx);
        [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
        internal delegate void PTOUPCAM_EVENT_CALLBACK(eEVENT nEvent, IntPtr pCtx);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left, top, right, bottom;
        };

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr Toupcam_Version();
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern uint Toupcam_Enum(IntPtr ti);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern SafeHToupCamHandle Toupcam_Open([MarshalAs(UnmanagedType.LPWStr)] string id);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_StartPullModeWithWndMsg(SafeHToupCamHandle h, IntPtr hWnd, uint nMsg);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_StartPullModeWithCallback(SafeHToupCamHandle h, PTOUPCAM_EVENT_CALLBACK pEventCallback, IntPtr pCallbackCtx);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_PullImage(SafeHToupCamHandle h, IntPtr pImageData, int bits, out uint pnWidth, out uint pnHeight);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_PullStillImage(SafeHToupCamHandle h, IntPtr pImageData, int bits, out uint pnWidth, out uint pnHeight);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_StartPushMode(SafeHToupCamHandle h, PTOUPCAM_DATA_CALLBACK pDataCallback, IntPtr pCallbackCtx);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_Stop(SafeHToupCamHandle h);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_Pause(SafeHToupCamHandle h, int bPause);

        /* for still image snap */
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_Snap(SafeHToupCamHandle h, uint nResolutionIndex);

        /*
            put_Size, put_eSize, can be used to set the video output resolution BEFORE Start.
            put_Size use width and height parameters, put_eSize use the index parameter.
            for example, UCMOS03100KPA support the following resolutions:
                index 0:    2048,   1536
                index 1:    1024,   768
                index 2:    680,    510
            so, we can use put_Size(h, 1024, 768) or put_eSize(h, 1). Both have the same effect.
        */
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_Size(SafeHToupCamHandle h, int nWidth, int nHeight);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_Size(SafeHToupCamHandle h, out int nWidth, out int nHeight);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_eSize(SafeHToupCamHandle h, uint nResolutionIndex);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_eSize(SafeHToupCamHandle h, out uint nResolutionIndex);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern uint Toupcam_get_ResolutionNumber(SafeHToupCamHandle h);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern uint Toupcam_get_Resolution(SafeHToupCamHandle h, uint nResolutionIndex, out int pWidth, out int pHeight);

        /*
            FourCC:
                MAKEFOURCC('G', 'B', 'R', 'G')
                MAKEFOURCC('R', 'G', 'G', 'B')
                MAKEFOURCC('B', 'G', 'B', 'R')
                MAKEFOURCC('G', 'R', 'B', 'G')
                MAKEFOURCC('Y', 'V', 'Y', 'U')
        */
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern uint Toupcam_get_RawFormat(SafeHToupCamHandle h, out uint nFourCC, out uint bits);

        /*
            set or get the process mode: TOUPCAM_PROCESSMODE_FULL or TOUPCAM_PROCESSMODE_FAST
        */
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_ProcessMode(SafeHToupCamHandle h, ePROCESSMODE nProcessMode);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_ProcessMode(SafeHToupCamHandle h, out ePROCESSMODE pnProcessMode);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_RealTime(SafeHToupCamHandle h, int bEnable);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_RealTime(SafeHToupCamHandle h, out int bEnable);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_Flush(SafeHToupCamHandle h);

        /*
            ------------------------------------------------------------|
            | Parameter         |   Range       |   Default             |
            |-----------------------------------------------------------|
            | AutoExpoTarget    |   10~230      |   120                 |
            | Temp              |   2000~15000  |   6503                |
            | Tint              |   200~2500    |   1000                |
            | LevelRange        |   0~255       |   Low = 0, High = 255 |
            | Contrast          |   -100~100    |   0                   |
            | Hue               |   -180~180    |   0                   |
            | Saturation        |   0~255       |   128                 |
            | Brightness        |   -64~64      |   0                   |
            | Gamma             |   20~180      |   100                 |
            ------------------------------------------------------------|
        */
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_AutoExpoEnable(SafeHToupCamHandle h, out int bAutoExposure);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_AutoExpoEnable(SafeHToupCamHandle h, int bAutoExposure);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_AutoExpoTarget(SafeHToupCamHandle h, out ushort Target);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_AutoExpoTarget(SafeHToupCamHandle h, ushort Target);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_MaxAutoExpoTimeAGain(SafeHToupCamHandle h, uint maxTime, ushort maxAGain);

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_ExpoTime(SafeHToupCamHandle h, out uint Time)/* in microseconds */;
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_ExpoTime(SafeHToupCamHandle h, uint Time)/* inmicroseconds */;
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_ExpTimeRange(SafeHToupCamHandle h, out uint nMin, out uint nMax, out uint nDef);

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_ExpoAGain(SafeHToupCamHandle h, out ushort AGain);/* percent, such as 300 */
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_ExpoAGain(SafeHToupCamHandle h, ushort AGain);/* percent */
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_ExpoAGainRange(SafeHToupCamHandle h, out ushort nMin, out ushort nMax, out ushort nDef);

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_LevelRange(SafeHToupCamHandle h, [In] ushort[] aLow, [In] ushort[] aHigh);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_LevelRange(SafeHToupCamHandle h, [Out] ushort[] aLow, [Out] ushort[] aHigh);

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_Hue(SafeHToupCamHandle h, int Hue);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_Hue(SafeHToupCamHandle h, out int Hue);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_Saturation(SafeHToupCamHandle h, int Saturation);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_Saturation(SafeHToupCamHandle h, out int Saturation);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_Brightness(SafeHToupCamHandle h, int Brightness);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_Brightness(SafeHToupCamHandle h, out int Brightness);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_Contrast(SafeHToupCamHandle h, out int Contrast);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_Contrast(SafeHToupCamHandle h, int Contrast);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_Gamma(SafeHToupCamHandle h, out int Gamma);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_Gamma(SafeHToupCamHandle h, int Gamma);

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_Chrome(SafeHToupCamHandle h, out int bChrome);    /* monochromatic mode */
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_Chrome(SafeHToupCamHandle h, int bChrome);

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_VFlip(SafeHToupCamHandle h, out int bVFlip);  /* vertical flip */
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_VFlip(SafeHToupCamHandle h, int bVFlip);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_HFlip(SafeHToupCamHandle h, out int bHFlip);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_HFlip(SafeHToupCamHandle h, int bHFlip);  /* horizontal flip */

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_Negative(SafeHToupCamHandle h, out int bNegative);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_Negative(SafeHToupCamHandle h, int bNegative);

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_Speed(SafeHToupCamHandle h, ushort nSpeed);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_Speed(SafeHToupCamHandle h, out ushort pSpeed);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern uint Toupcam_get_MaxSpeed(SafeHToupCamHandle h);/* get the maximum speed, "Frame Speed Level", speed range = [0, max] */

        /* power supply: 
                0 -> 60HZ AC
                1 -> 50Hz AC
                2 -> DC
        */
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_HZ(SafeHToupCamHandle h, int nHZ);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_HZ(SafeHToupCamHandle h, out int nHZ);

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_Mode(SafeHToupCamHandle h, int bSkip); /* skip or bin */
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_Mode(SafeHToupCamHandle h, out int bSkip);

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_TempTint(SafeHToupCamHandle h, int nTemp, int nTint);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_TempTint(SafeHToupCamHandle h, out int nTemp, out int nTint);

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_AWBAuxRect(SafeHToupCamHandle h, ref RECT pAuxRect);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_AWBAuxRect(SafeHToupCamHandle h, out RECT pAuxRect);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_AEAuxRect(SafeHToupCamHandle h, ref RECT pAuxRect);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_AEAuxRect(SafeHToupCamHandle h, out RECT pAuxRect);

        /*
            S_FALSE:    color mode
            S_OK:       mono mode, such as EXCCD00300KMA and UHCCD01400KMA
        */
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_MonoMode(SafeHToupCamHandle h);

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern uint Toupcam_get_StillResolutionNumber(SafeHToupCamHandle h);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_StillResolution(SafeHToupCamHandle h, uint nIndex, out int pWidth, out int pHeight);

        /*
            get the serial number which is always 32 chars which is zero-terminated such as "TP110826145730ABCD1234FEDC56787"
        */
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_SerialNumber(SafeHToupCamHandle h, IntPtr sn);

        /*
            get the camera firmware version, such as: 3.2.1.20140922
        */
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_FwVersion(SafeHToupCamHandle h, IntPtr fwver);
        /*
            get the camera hardware version, such as: 3.2.1.20140922
        */
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_HwVersion(SafeHToupCamHandle h, IntPtr hwver);

        /*
                    ------------------------------------------------------------|
                    | Parameter         |   Range       |   Default             |
                    |-----------------------------------------------------------|
                    | VidgetAmount      |   -100~100    |   0                   |
                    | VignetMidPoint    |   0~100       |   50                  |
                    -------------------------------------------------------------
        */
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_VignetEnable(SafeHToupCamHandle h, int bEnable);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_VignetEnable(SafeHToupCamHandle h, out int bEnable);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_VignetAmountInt(SafeHToupCamHandle h, int nAmount);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_VignetAmountInt(SafeHToupCamHandle h, out int nAmount);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_VignetMidPointInt(SafeHToupCamHandle h, int nMidPoint);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_VignetMidPointInt(SafeHToupCamHandle h, out int nMidPoint);

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_ExpoCallback(SafeHToupCamHandle h, PITOUPCAM_EXPOSURE_CALLBACK fnExpoProc, IntPtr pExpoCtx);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_ChromeCallback(SafeHToupCamHandle h, PITOUPCAM_CHROME_CALLBACK fnChromeProc, IntPtr pChromeCtx);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_AwbOnePush(SafeHToupCamHandle h, PITOUPCAM_TEMPTINT_CALLBACK fnTTProc, IntPtr pTTCtx);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_LevelRangeAuto(SafeHToupCamHandle h);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_GetHistogram(SafeHToupCamHandle h, PITOUPCAM_HISTOGRAM_CALLBACK fnHistogramProc, IntPtr pHistogramCtx);

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_LEDState(SafeHToupCamHandle h, ushort iLed, ushort iState, ushort iPeriod);

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_write_EEPROM(SafeHToupCamHandle h, uint addr, IntPtr pData, uint nDataLen);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_read_EEPROM(SafeHToupCamHandle h, uint addr, IntPtr pBuffer, uint nBufferLen);

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_put_Option(SafeHToupCamHandle h, eOPTION iOption, uint iValue);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_get_Option(SafeHToupCamHandle h, eOPTION iOption, out uint iValue);

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_TempTint2RGB(int nTemp, int nTint, [Out] int[] nRGB);
        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int Toupcam_RGB2TempTint([In] int[] nRGB, out int nTemp, out int nTint);

        [DllImport("toupcam.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern double Toupcam_calc_ClarityFactor(IntPtr pImageData, int bits, uint nImgWidth, uint nImgHeight);

        private SafeHToupCamHandle _handle;
        private GCHandle _gchandle;
        private DelegateDataCallback _dDataCallback;
        private DelegateEventCallback _dEventCallback;
        private DelegateExposureCallback _dExposureCallback;
        private DelegateTempTintCallback _dTempTintCallback;
        private DelegateHistogramCallback _dHistogramCallback;
        private DelegateChromeCallback _dChromeCallback;
        private PTOUPCAM_DATA_CALLBACK _pDataCallback;
        private PTOUPCAM_EVENT_CALLBACK _pEventCallback;
        private PITOUPCAM_EXPOSURE_CALLBACK _pExposureCallback;
        private PITOUPCAM_TEMPTINT_CALLBACK _pTempTintCallback;
        private PITOUPCAM_HISTOGRAM_CALLBACK _pHistogramCallback;
        private PITOUPCAM_CHROME_CALLBACK _pChromeCallback;

        private void EventCallback(eEVENT nEvent)
        {
            if (_dEventCallback != null)
                _dEventCallback(nEvent);
        }

        private void DataCallback(IntPtr pData, IntPtr pHeader, bool bSnap)
        {
            if (pData == IntPtr.Zero || pHeader == IntPtr.Zero) /* pData == 0 means that something error, we callback to tell the application */
            {
                if (_dDataCallback != null)
                {
                    BITMAPINFOHEADER h = new BITMAPINFOHEADER();
                    _dDataCallback(IntPtr.Zero, ref h, bSnap);
                }
            }
            else
            {
                BITMAPINFOHEADER h = (BITMAPINFOHEADER)Marshal.PtrToStructure(pHeader, typeof(BITMAPINFOHEADER));
                if (_dDataCallback != null)
                    _dDataCallback(pData, ref h, bSnap);
            }
        }

        private void ExposureCallback()
        {
            if (_dExposureCallback != null)
                _dExposureCallback();
        }

        private void TempTintCallback(int nTemp, int nTint)
        {
            if (_dTempTintCallback != null)
            {
                _dTempTintCallback(nTemp, nTint);
                _dTempTintCallback = null;
            }
            _pTempTintCallback = null;
        }

        private void ChromeCallback()
        {
            if (_dChromeCallback != null)
                _dChromeCallback();
        }

        private void HistogramCallback(double[] aHistY, double[] aHistR, double[] aHistG, double[] aHistB)
        {
            if (_dHistogramCallback != null)
            {
                _dHistogramCallback(aHistY, aHistR, aHistG, aHistB);
                _dHistogramCallback = null;
            }
            _pHistogramCallback = null;
        }

        private static void DataCallback(IntPtr pData, IntPtr pHeader, bool bSnap, IntPtr pCallbackCtx)
        {
            GCHandle gch = GCHandle.FromIntPtr(pCallbackCtx);
            if (gch != null)
            {
                ToupCam pthis = gch.Target as ToupCam;
                if (pthis != null)
                    pthis.DataCallback(pData, pHeader, bSnap);
            }
        }

        private static void EventCallback(eEVENT nEvent, IntPtr pCallbackCtx)
        {
            GCHandle gch = GCHandle.FromIntPtr(pCallbackCtx);
            if (gch != null)
            {
                ToupCam pthis = gch.Target as ToupCam;
                if (pthis != null)
                {
                    pthis.EventCallback(nEvent);
                }
            }
        }

        private static void ExposureCallback(IntPtr pCallbackCtx)
        {
            GCHandle gch = GCHandle.FromIntPtr(pCallbackCtx);
            {
                ToupCam pthis = gch.Target as ToupCam;
                if (pthis != null)
                {
                    pthis.ExposureCallback();
                }
            }
        }

        private static void TempTintCallback(int nTemp, int nTint, IntPtr pCallbackCtx)
        {
            GCHandle gch = GCHandle.FromIntPtr(pCallbackCtx);
            {
                ToupCam pthis = gch.Target as ToupCam;
                if (pthis != null)
                {
                    pthis.TempTintCallback(nTemp, nTint);
                }
            }
        }

        private static void ChromeCallback(IntPtr pCallbackCtx)
        {
            GCHandle gch = GCHandle.FromIntPtr(pCallbackCtx);
            {
                ToupCam pthis = gch.Target as ToupCam;
                if (pthis != null)
                {
                pthis.ChromeCallback();

                }
            }
        }

        private static void HistogramCallback(IntPtr aHistY, IntPtr aHistR, IntPtr aHistG, IntPtr aHistB, IntPtr pCallbackCtx)
        {
            GCHandle gch = GCHandle.FromIntPtr(pCallbackCtx);
            if (gch != null)
            {
                ToupCam pthis = gch.Target as ToupCam;
                if (pthis != null)
                {
                    double[] arrHistY = new double[256];
                    double[] arrHistR = new double[256];
                    double[] arrHistG = new double[256];
                    double[] arrHistB = new double[256];
                    Marshal.Copy(aHistY, arrHistY, 0, 256);
                    Marshal.Copy(aHistR, arrHistR, 0, 256);
                    Marshal.Copy(aHistG, arrHistG, 0, 256);
                    Marshal.Copy(aHistB, arrHistB, 0, 256);
                    pthis.HistogramCallback(arrHistY, arrHistR, arrHistG, arrHistB);
                }
            }
        }

        ~ToupCam()
        {
            Dispose(false);
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        protected virtual void Dispose(bool disposing)
        {
            // Note there are three interesting states here:
            // 1) CreateFile failed, _handle contains an invalid handle
            // 2) We called Dispose already, _handle is closed.
            // 3) _handle is null, due to an async exception before
            //    calling CreateFile. Note that the finalizer runs
            //    if the constructor fails.
            if (_handle != null && !_handle.IsInvalid)
            {
                // Free the handle
                _handle.Dispose();
            }
            // SafeHandle records the fact that we've called Dispose.
        }

        public void Dispose()  // Follow the Dispose pattern - public nonvirtual.
        {
            Dispose(true);
            if (_gchandle.IsAllocated)
                _gchandle.Free();
            GC.SuppressFinalize(this);
        }

        public void Close()
        {
            Dispose();
        }

        public static string Version()
        {
            return Marshal.PtrToStringUni(Toupcam_Version());
        }

        /* enumerate ToupCam cameras that are currently connected to computer */
        public static Instance[] Enum()
        {
            IntPtr ti = Marshal.AllocHGlobal(512 * 16);
            uint cnt = Toupcam_Enum(ti);
            Instance[] arr = new Instance[cnt];
            if (cnt != 0)
            {
                long p = ti.ToInt64();
                for (uint i = 0; i < cnt; ++i)
                {
                    arr[i].displayname = Marshal.PtrToStringUni((IntPtr)p);
                    p += sizeof(char) * 64;
                    arr[i].id = Marshal.PtrToStringUni((IntPtr)p);
                    p += sizeof(char) * 64;

                    IntPtr pm = Marshal.ReadIntPtr((IntPtr)p);
                    p += IntPtr.Size;

                    {
                        long q = pm.ToInt64();
                        IntPtr pmn = Marshal.ReadIntPtr((IntPtr)q);
                        arr[i].model.name = Marshal.PtrToStringUni(pmn);
                        q += IntPtr.Size;
                        arr[i].model.flag = (eFALG)Marshal.ReadInt32((IntPtr)q);
                        q += sizeof(int);
                        arr[i].model.maxspeed = (uint)Marshal.ReadInt32((IntPtr)q);
                        q += sizeof(int);
                        arr[i].model.preview = (uint)Marshal.ReadInt32((IntPtr)q);
                        q += sizeof(int);
                        arr[i].model.still = (uint)Marshal.ReadInt32((IntPtr)q);
                        q += sizeof(int);

                        uint resn = Math.Max(arr[i].model.preview, arr[i].model.still);
                        arr[i].model.res = new Resolution[resn];
                        for (uint j = 0; j < resn; ++j)
                        {
                            arr[i].model.res[j].width = (uint)Marshal.ReadInt32((IntPtr)q);
                            q += sizeof(int);
                            arr[i].model.res[j].height = (uint)Marshal.ReadInt32((IntPtr)q);
                            q += sizeof(int);
                        }
                    }
                }
            }
            Marshal.FreeHGlobal(ti);
            return arr;
        }

        // id: enumerated by Enum
        public bool Open(string id)
        {
            SafeHToupCamHandle tmphandle = Toupcam_Open(id);
            if (tmphandle == null || tmphandle.IsInvalid || tmphandle.IsClosed)
                return false;
            _handle = tmphandle;
            _gchandle = GCHandle.Alloc(this);
            return true;
        }

        public SafeHToupCamHandle Handle
        {
            get
            {
                return _handle;
            }
        }

        public uint ResolutionNumber
        {
            get
            {
                if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                    return 0;
                return Toupcam_get_ResolutionNumber(_handle);
            }
        }

        public uint StillResolutionNumber
        {
            get
            {
                if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                    return 0;
                return Toupcam_get_StillResolutionNumber(_handle);
            }
        }

        public bool MonoMode
        {
            get
            {
                if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                    return false;
                return (0 == Toupcam_get_MonoMode(_handle));
            }
        }

        /* get the maximum speed, "Frame Speed Level" */
        public uint MaxSpeed
        {
            get
            {
                if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                    return 0;
                return Toupcam_get_MaxSpeed(_handle);
            }
        }

        /* get the serial number which is always 32 chars which is zero-terminated such as "TP110826145730ABCD1234FEDC56787" */
        public string SerialNumber
        {
            get
            {
                string sn = "";
                if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                    return sn;
                IntPtr ptr = Marshal.AllocHGlobal(64);
                if (Toupcam_get_SerialNumber(_handle, ptr) < 0)
                    sn = "";
                else
                    sn = Marshal.PtrToStringAnsi(ptr);

                Marshal.FreeHGlobal(ptr);
                return sn;
            }
        }

        /* get the camera firmware version, such as: 3.2.1.20140922 */
        public string FwVersion
        {
            get
            {
                string fwver = "";
                if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                    return fwver;
                IntPtr ptr = Marshal.AllocHGlobal(32);
                if (Toupcam_get_FwVersion(_handle, ptr) < 0)
                    fwver = "";
                else
                    fwver = Marshal.PtrToStringAnsi(ptr);

                Marshal.FreeHGlobal(ptr);
                return fwver;
            }
        }

        /* get the camera hardware version, such as: 3.2.1.20140922 */
        public string HwVersion
        {
            get
            {
                string hwver = "";
                if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                    return hwver;
                IntPtr ptr = Marshal.AllocHGlobal(32);
                if (Toupcam_get_HwVersion(_handle, ptr) < 0)
                    hwver = "";
                else
                    hwver = Marshal.PtrToStringAnsi(ptr);

                Marshal.FreeHGlobal(ptr);
                return hwver;
            }
        }

        public bool StartPullModeWithWndMsg(IntPtr hWnd, uint nMsg)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            return (Toupcam_StartPullModeWithWndMsg(_handle, hWnd, nMsg) >= 0);
        }

        public bool StartPullModeWithCallback(DelegateEventCallback edelegate)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            _dEventCallback = edelegate;
            if (edelegate != null)
            {
                _pEventCallback = new PTOUPCAM_EVENT_CALLBACK(EventCallback);
                return (Toupcam_StartPullModeWithCallback(_handle, _pEventCallback, GCHandle.ToIntPtr(_gchandle)) >= 0);
            }
            else
            {
                return (Toupcam_StartPullModeWithCallback(_handle, null, IntPtr.Zero) >= 0);
            }
        }

        /*  bits: 24 (RGB24), 32 (RGB32), or 8 (Grey) */
        public bool PullImage(IntPtr pImageData, int bits, out uint pnWidth, out uint pnHeight)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
            {
                pnWidth = pnHeight = 0;
                return false;
            }

            return (Toupcam_PullImage(_handle, pImageData, bits, out pnWidth, out pnHeight) >= 0);
        }

        /*  bits: 24 (RGB24), 32 (RGB32), or 8 (Grey) */
        public bool PullStillImage(IntPtr pImageData, int bits, out uint pnWidth, out uint pnHeight)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
            {
                pnWidth = pnHeight = 0;
                return false;
            }

            return (Toupcam_PullStillImage(_handle, pImageData, bits, out pnWidth, out pnHeight) >= 0);
        }

        public bool StartPushMode(DelegateDataCallback ddelegate)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            _dDataCallback = ddelegate;
            _pDataCallback = new PTOUPCAM_DATA_CALLBACK(DataCallback);
            return (Toupcam_StartPushMode(_handle, _pDataCallback, GCHandle.ToIntPtr(_gchandle)) >= 0);
        }

        public bool Stop()
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_Stop(_handle) >= 0);
        }

        public bool Pause(bool bPause)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_Pause(_handle, bPause ? 1 : 0) >= 0);
        }

        public bool Snap(uint nResolutionIndex)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_Snap(_handle, nResolutionIndex) >= 0);
        }

        public bool put_Size(int nWidth, int nHeight)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_Size(_handle, nWidth, nHeight) >= 0);
        }

        public bool get_Size(out int nWidth, out int nHeight)
        {
            nWidth = nHeight = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_Size(_handle, out nWidth, out nHeight) >= 0);
        }

        public bool put_eSize(uint nResolutionIndex)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_eSize(_handle, nResolutionIndex) >= 0);
        }

        public bool get_eSize(out uint nResolutionIndex)
        {
            nResolutionIndex = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_eSize(_handle, out nResolutionIndex) >= 0);
        }

        public bool get_Resolution(uint nResolutionIndex, out int pWidth, out int pHeight)
        {
            pWidth = pHeight = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_Resolution(_handle, nResolutionIndex, out pWidth, out pHeight) >= 0);
        }

        public bool get_RawFormat(out uint nFourCC, out uint bits)
        {
            nFourCC = bits = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_RawFormat(_handle, out nFourCC, out bits) >= 0);
        }

        public bool put_ProcessMode(ePROCESSMODE nProcessMode)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_ProcessMode(_handle, nProcessMode) >= 0);
        }

        public bool get_ProcessMode(out ePROCESSMODE pnProcessMode)
        {
            pnProcessMode = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_ProcessMode(_handle, out pnProcessMode) >= 0);
        }

        public bool put_RealTime(bool bEnable)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_RealTime(_handle, bEnable ? 1 : 0) >= 0);
        }

        public bool get_RealTime(out bool bEnable)
        {
            bEnable = false;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            int iEnable = 0;
            if (Toupcam_get_RealTime(_handle, out iEnable) < 0)
                return false;

            bEnable = (iEnable != 0);
            return true;
        }

        public bool Flush()
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_Flush(_handle) >= 0);
        }

        public bool get_AutoExpoEnable(out bool bAutoExposure)
        {
            bAutoExposure = false;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            int iEnable = 0;
            if (Toupcam_get_AutoExpoEnable(_handle, out iEnable) < 0)
                return false;

            bAutoExposure = (iEnable != 0);
            return true;
        }

        public bool put_AutoExpoEnable(bool bAutoExposure)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_AutoExpoEnable(_handle, bAutoExposure ? 1 : 0) >= 0);
        }

        public bool get_AutoExpoTarget(out ushort Target)
        {
            Target = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_AutoExpoTarget(_handle, out Target) >= 0);
        }

        public bool put_AutoExpoTarget(ushort Target)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_AutoExpoTarget(_handle, Target) >= 0);
        }

        public bool put_MaxAutoExpoTimeAGain(uint maxTime, ushort maxAGain)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_MaxAutoExpoTimeAGain(_handle, maxTime, maxAGain) >= 0);
        }

        public bool get_ExpoTime(out uint Time)/* in microseconds */
        {
            Time = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_ExpoTime(_handle, out Time) >= 0);
        }

        public bool put_ExpoTime(uint Time)/* in microseconds */
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_ExpoTime(_handle, Time) >= 0);
        }

        public bool get_ExpTimeRange(out uint nMin, out uint nMax, out uint nDef)
        {
            nMin = nMax = nDef = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_ExpTimeRange(_handle, out nMin, out nMax, out nDef) >= 0);
        }

        public bool get_ExpoAGain(out ushort AGain)/* percent, such as 300 */
        {
            AGain = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_ExpoAGain(_handle, out AGain) >= 0);
        }

        public bool put_ExpoAGain(ushort AGain)/* percent */
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_ExpoAGain(_handle, AGain) >= 0);
        }

        public bool get_ExpoAGainRange(out ushort nMin, out ushort nMax, out ushort nDef)
        {
            nMin = nMax = nDef = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_ExpoAGainRange(_handle, out nMin, out nMax, out nDef) >= 0);
        }

        public bool put_LevelRange(ushort[] aLow, ushort[] aHigh)
        {
            if (aLow.Length != 4 || aHigh.Length != 4)
                return false;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_LevelRange(_handle, aLow, aHigh) >= 0);
        }

        public bool get_LevelRange(ushort[] aLow, ushort[] aHigh)
        {
            if (aLow.Length != 4 || aHigh.Length != 4)
                return false;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_LevelRange(_handle, aLow, aHigh) >= 0);
        }

        public bool put_Hue(int Hue)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_Hue(_handle, Hue) >= 0);
        }

        public bool get_Hue(out int Hue)
        {
            Hue = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_Hue(_handle, out Hue) >= 0);
        }

        public bool put_Saturation(int Saturation)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_Saturation(_handle, Saturation) >= 0);
        }

        public bool get_Saturation(out int Saturation)
        {
            Saturation = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_Saturation(_handle, out Saturation) >= 0);
        }

        public bool put_Brightness(int Brightness)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_Brightness(_handle, Brightness) >= 0);
        }

        public bool get_Brightness(out int Brightness)
        {
            Brightness = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_Brightness(_handle, out Brightness) >= 0);
        }

        public bool get_Contrast(out int Contrast)
        {
            Contrast = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_Contrast(_handle, out Contrast) >= 0);
        }

        public bool put_Contrast(int Contrast)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_Contrast(_handle, Contrast) >= 0);
        }

        public bool get_Gamma(out int Gamma)
        {
            Gamma = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_Gamma(_handle, out Gamma) >= 0);
        }

        public bool put_Gamma(int Gamma)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_Gamma(_handle, Gamma) >= 0);
        }

        public bool get_Chrome(out bool bChrome)    /* monochromatic mode */
        {
            bChrome = false;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            int iEnable = 0;
            if (Toupcam_get_Chrome(_handle, out iEnable) < 0)
                return false;

            bChrome = (iEnable != 0);
            return true;
        }

        public bool put_Chrome(bool bChrome)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_Chrome(_handle, bChrome ? 1 : 0) >= 0);
        }

        public bool get_VFlip(out bool bVFlip) /* vertical flip */
        {
            bVFlip = false;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            int iVFlip = 0;
            if (Toupcam_get_VFlip(_handle, out iVFlip) < 0)
                return false;

            bVFlip = (iVFlip != 0);
            return true;
        }

        public bool put_VFlip(bool bVFlip)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_VFlip(_handle, bVFlip ? 1 : 0) >= 0);
        }

        public bool get_HFlip(out bool bHFlip)
        {
            bHFlip = false;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            int iHFlip = 0;
            if (Toupcam_get_HFlip(_handle, out iHFlip) < 0)
                return false;

            bHFlip = (iHFlip != 0);
            return true;
        }

        public bool put_HFlip(bool bHFlip)  /* horizontal flip */
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_HFlip(_handle, bHFlip ? 1 : 0) >= 0);
        }

        /* negative film */
        public bool get_Negative(out bool bNegative)
        {
            bNegative = false;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            int iNegative = 0;
            if (Toupcam_get_Negative(_handle, out iNegative) < 0)
                return false;

            bNegative = (iNegative != 0);
            return true;
        }

        /* negative film */
        public bool put_Negative(bool bNegative)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_Negative(_handle, bNegative ? 1 : 0) >= 0);
        }

        public bool put_Speed(ushort nSpeed)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_Speed(_handle, nSpeed) >= 0);
        }

        public bool get_Speed(out ushort pSpeed)
        {
            pSpeed = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_Speed(_handle, out pSpeed) >= 0);
        }

        public bool put_HZ(int nHZ)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_HZ(_handle, nHZ) >= 0);
        }

        public bool get_HZ(out int nHZ)
        {
            nHZ = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_HZ(_handle, out nHZ) >= 0);
        }

        public bool put_Mode(bool bSkip) /* skip or bin */
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_Mode(_handle, bSkip ? 1 : 0) >= 0);
        }

        public bool get_Mode(out bool bSkip)
        {
            bSkip = false;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            int iSkip = 0;
            if (Toupcam_get_Mode(_handle, out iSkip) < 0)
                return false;

            bSkip = (iSkip != 0);
            return true;
        }

        public bool put_TempTint(int nTemp, int nTint)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_TempTint(_handle, nTemp, nTint) >= 0);
        }

        public bool get_TempTint(out int nTemp, out int nTint)
        {
            nTemp = nTint = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_TempTint(_handle, out nTemp, out nTint) >= 0);
        }

        public bool put_AWBAuxRect(Rectangle AuxRect)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            RECT rc = new RECT();
            rc.left = AuxRect.X;
            rc.right = AuxRect.X + AuxRect.Width;
            rc.top = AuxRect.Y;
            rc.bottom = AuxRect.Y + AuxRect.Height;
            return (Toupcam_put_AWBAuxRect(_handle, ref rc) >= 0);
        }

        public bool get_AWBAuxRect(out Rectangle pAuxRect)
        {
            pAuxRect = Rectangle.Empty;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            RECT rc = new RECT();
            if (Toupcam_get_AWBAuxRect(_handle, out rc) < 0)
                return false;

            pAuxRect.X = rc.left;
            pAuxRect.Y = rc.top;
            pAuxRect.Width = rc.right - rc.left;
            pAuxRect.Height = rc.bottom - rc.top;
            return true;
        }

        public bool put_AEAuxRect(Rectangle AuxRect)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            RECT rc = new RECT();
            rc.left = AuxRect.X;
            rc.right = AuxRect.X + AuxRect.Width;
            rc.top = AuxRect.Y;
            rc.bottom = AuxRect.Y + AuxRect.Height;
            return (Toupcam_put_AEAuxRect(_handle, ref rc) >= 0);
        }

        public bool get_AEAuxRect(out Rectangle pAuxRect)
        {
            pAuxRect = Rectangle.Empty;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            RECT rc = new RECT();
            if (Toupcam_get_AEAuxRect(_handle, out rc) < 0)
                return false;

            pAuxRect.X = rc.left;
            pAuxRect.Y = rc.top;
            pAuxRect.Width = rc.right - rc.left;
            pAuxRect.Height = rc.bottom - rc.top;
            return true;
        }

        public bool get_StillResolution(uint nIndex, out int pWidth, out int pHeight)
        {
            pWidth = pHeight = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_StillResolution(_handle, nIndex, out pWidth, out pHeight) >= 0);
        }

        public bool put_VignetEnable(bool bEnable)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_VignetEnable(_handle, bEnable ? 1 : 0) >= 0);
        }

        public bool get_VignetEnable(out bool bEnable)
        {
            bEnable = false;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            int iEanble = 0;
            if (Toupcam_get_VignetEnable(_handle, out iEanble) < 0)
                return false;

            bEnable = (iEanble != 0);
            return true;
        }

        public bool put_VignetAmountInt(int nAmount)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_VignetAmountInt(_handle, nAmount) >= 0);
        }

        public bool get_VignetAmountInt(out int nAmount)
        {
            nAmount = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_VignetAmountInt(_handle, out nAmount) >= 0);
        }

        public bool put_VignetMidPointInt(int nMidPoint)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_VignetMidPointInt(_handle, nMidPoint) >= 0);
        }

        public bool get_VignetMidPointInt(out int nMidPoint)
        {
            nMidPoint = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_VignetMidPointInt(_handle, out nMidPoint) >= 0);
        }

        /* led state:
            iLed: Led index, (0, 1, 2, ...)
            iState: 1 -> Ever bright; 2 -> Flashing; other -> Off
            iPeriod: Flashing Period (>= 500ms)
        */
        public bool put_LEDState(ushort iLed, ushort iState, ushort iPeriod)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_LEDState(_handle, iLed, iState, iPeriod) >= 0);
        }

        public int write_EEPROM(SafeHToupCamHandle h, uint addr, IntPtr pData, uint nDataLen)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return 0;
            return Toupcam_write_EEPROM(_handle, addr, pData, nDataLen);
        }

        public int read_EEPROM(SafeHToupCamHandle h, uint addr, IntPtr pBuffer, uint nBufferLen)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return 0;
            return Toupcam_read_EEPROM(_handle, addr, pBuffer, nBufferLen);
        }

        public bool put_Option(eOPTION iOption, uint iValue)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_put_Option(_handle, iOption, iValue) >= 0);
        }

        public bool get_Option(eOPTION iOption, out uint iValue)
        {
            iValue = 0;
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_get_Option(_handle, iOption, out iValue) >= 0);
        }

        public bool LevelRangeAuto()
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;
            return (Toupcam_LevelRangeAuto(_handle) >= 0);
        }

        public bool put_ExpoCallback(DelegateExposureCallback fnExpoProc)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            _dExposureCallback = fnExpoProc;
            if (fnExpoProc == null)
                return (Toupcam_put_ExpoCallback(_handle, null, IntPtr.Zero) >= 0);
            else
            {
                _pExposureCallback = new PITOUPCAM_EXPOSURE_CALLBACK(ExposureCallback);
                return (Toupcam_put_ExpoCallback(_handle, _pExposureCallback, GCHandle.ToIntPtr(_gchandle)) >= 0);
            }
        }

        public bool put_ChromeCallback(DelegateChromeCallback fnChromeProc)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            _dChromeCallback = fnChromeProc;
            if (fnChromeProc == null)
                return (Toupcam_put_ChromeCallback(_handle, null, IntPtr.Zero) >= 0);
            else
            {
                _pChromeCallback = new PITOUPCAM_CHROME_CALLBACK(ChromeCallback);
                return (Toupcam_put_ChromeCallback(_handle, _pChromeCallback, GCHandle.ToIntPtr(_gchandle)) >= 0);
            }
        }

        public bool AwbOnePush(DelegateTempTintCallback fnTTProc)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            _dTempTintCallback = fnTTProc;
            if (fnTTProc == null)
                return (Toupcam_AwbOnePush(_handle, null, IntPtr.Zero) >= 0);
            else
            {
                _pTempTintCallback = new PITOUPCAM_TEMPTINT_CALLBACK(TempTintCallback);
                return (Toupcam_AwbOnePush(_handle, _pTempTintCallback, GCHandle.ToIntPtr(_gchandle)) >= 0);
            }
        }

        /* put_TempTintInit is obsolete, it's a synonyms for AwbOnePush. They are exactly the same */
        public bool put_TempTintInit(DelegateTempTintCallback fnTTProc)
        {
            return AwbOnePush(fnTTProc);
        }

        public bool GetHistogram(DelegateHistogramCallback fnHistogramProc)
        {
            if (_handle == null || _handle.IsInvalid || _handle.IsClosed)
                return false;

            _dHistogramCallback = fnHistogramProc;
            _pHistogramCallback = new PITOUPCAM_HISTOGRAM_CALLBACK(HistogramCallback);
            return (Toupcam_GetHistogram(_handle, _pHistogramCallback, GCHandle.ToIntPtr(_gchandle)) >= 0);
        }

        public static void TempTint2RGB(int nTemp, int nTint, int[] nRGB)
        {
            Toupcam_TempTint2RGB(nTemp, nTint, nRGB);
        }

        public static void RGB2TempTint(int[] nRGB, out int nTemp, out int nTint)
        {
            Toupcam_RGB2TempTint(nRGB, out nTemp, out nTint);
        }

        /*
            calculate the clarity factor:
            pImageData: pointer to the image data
            bits: 8(Grey), 24 (RGB24), 32(RGB32)
            nImgWidth, nImgHeight: the image width and height
        */
        public static double calcClarityFactor(IntPtr pImageData, int bits, uint nImgWidth, uint nImgHeight)
        {
            return Toupcam_calc_ClarityFactor(pImageData, bits, nImgWidth, nImgHeight);
        }
    }
}
