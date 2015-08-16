#region

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MVcamview.Properties;
using ToupTek;

#endregion

namespace MVcamview
{
    public partial class Form1 : Form
    {
        private readonly uint MSG_CAMEVENT = 0x8001; // WM_APP = 0x8000
        private bool _flipHorizontal1;
        private bool _flipHorizontal2 = false;
        private bool _flipVertical1;
        private bool _flipVertical2 = false;
        private ImageFilter _imageFilter;
        private Thread _thread;
        private Bitmap bmp1_;
        private Bitmap bmp2_;
        private float image1Opacity;
        private float image2Opacity;
        private bool imageOneOnTop = true;
        private bool IsRunning;
        private ToupCam toupcam1_;
        private ToupCam toupcam2_;

        private Bitmap bmp1BackUp;
        private Bitmap bmp2BackUp;

        public Form1()
        {
            InitializeComponent();
        }

        private void savefile(IntPtr pData, ref ToupCam.BITMAPINFOHEADER header)
        {
            Bitmap bmp = new Bitmap(header.biWidth, header.biHeight, PixelFormat.Format24bppRgb);
            BitmapData bmpdata = bmp.LockBits(new Rectangle(0, 0, header.biWidth, header.biHeight),
                ImageLockMode.WriteOnly, bmp.PixelFormat);

            ToupCam.CopyMemory(bmpdata.Scan0, pData, header.biSizeImage);

            bmp.UnlockBits(bmpdata);

            bmp.Save("ToupcamTwoCameraSupport.jpg");
        }

        private void OnEventError()
        {
            if (toupcam1_ != null)
            {
                toupcam1_.Close();
                toupcam1_ = null;
            }

            if (toupcam2_ != null)
            {
                toupcam2_.Close();
                toupcam2_ = null;
            }
            MessageBox.Show(@"Error");
            //TODO:make it retry
            OnStart(null, null);
        }

        private void OnEventDisconnected()
        {
            if (toupcam1_ != null)
            {
                toupcam1_.Close();
                toupcam1_ = null;
            }

            if (toupcam2_ != null)
            {
                toupcam2_.Close();
                toupcam2_ = null;
            }

            MessageBox.Show(@"The camera is disconnected, maybe has been pulled out.");
        }

        private void OnEventExposure1()
        {
            if (toupcam1_ == null) return;
            uint nTime = 0;
            if (toupcam1_.get_ExpoTime(out nTime))
            {
                ExpoValue1.Value = (int)nTime;
                label1.Text = (nTime / 1000) + " ms";
            }
        }

        private void OnEventExposure2()
        {
            if (toupcam2_ == null) return;
            uint nTime = 0;
            if (toupcam2_.get_ExpoTime(out nTime))
            {
                ExpoValue2.Value = (int)nTime;
                label4.Text = (nTime / 1000) + " ms";
            }
        }

        private void OnEventImage()
        {
            if (bmp1_ != null)
            {
                BitmapData bmpdata = bmp1_.LockBits(new Rectangle(0, 0, bmp1_.Width, bmp1_.Height),
                    ImageLockMode.WriteOnly, bmp1_.PixelFormat);

                uint nWidth = 0, nHeight = 0;
                toupcam1_.PullImage(bmpdata.Scan0, 24, out nWidth, out nHeight);

                bmp1_.UnlockBits(bmpdata);

                if (_flipHorizontal1)
                {
                    bmp1_.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }

                if (_flipVertical1)
                {
                    bmp1_.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }
                pictureBox1.Image = bmp1_;
                pictureBox1.Invalidate();
            }

            if (bmp2_ != null)
            {
                BitmapData bmpdata = bmp2_.LockBits(new Rectangle(0, 0, bmp2_.Width, bmp2_.Height),
                    ImageLockMode.WriteOnly, bmp2_.PixelFormat);

                uint nWidth = 0, nHeight = 0;
                toupcam2_.PullImage(bmpdata.Scan0, 24, out nWidth, out nHeight);

                bmp2_.UnlockBits(bmpdata);
                if (_flipHorizontal2)
                {
                    bmp2_.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }

                if (_flipVertical2)
                {
                    bmp2_.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }
                pictureBox2.Image = bmp2_;
                pictureBox2.Invalidate();
            }


            if (!IsRunning && bmp1_!=null && bmp2_!=null)
            {
                BackgroundWorker bg = new BackgroundWorker();
                bg.DoWork += Bg_ChangeOpcity;
                bg.RunWorkerCompleted += Bg_ChangeOpacityFinished;
                IsRunning = true;
                bmp1BackUp = (Bitmap) bmp1_.Clone();
                bmp2BackUp = (Bitmap) bmp2_.Clone();
                bg.RunWorkerAsync();
            }
        }

        private void Bg_ChangeOpacityFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox3.Image = _imageFilter.Result;
            IsRunning = false;
        }

        private void Bg_ChangeOpcity(object sender, DoWorkEventArgs e)
        {
            if (bmp1BackUp == null || bmp2BackUp== null) return;

            Image topImage;
            Image bottomImage;
            float topImageOpacity;
            float bottomImageOpacity;

            if (!imageOneOnTop)
            {
                topImage = (Image)bmp1BackUp.Clone();
                topImageOpacity = image1Opacity / 100;
                bottomImage = (Image)bmp2BackUp.Clone();
                bottomImageOpacity = image2Opacity / 100;
            }
            else
            {
                topImage = (Image)bmp2BackUp.Clone();
                topImageOpacity = image2Opacity / 100;
                bottomImage = (Image)bmp1BackUp.Clone();
                bottomImageOpacity = image1Opacity / 100;
            }

            _imageFilter.FirstImage = topImage;
            _imageFilter.FirstImageOpacity = topImageOpacity;
            _imageFilter.SecondImage = bottomImage;
            _imageFilter.SecondImageOpacity = bottomImageOpacity;

            _imageFilter.StartToCombine();
        }

        private void OnEventStillImage()
        {
            uint nWidth = 0, nHeight = 0;
            if (toupcam1_.PullStillImage(IntPtr.Zero, 24, out nWidth, out nHeight)) /* peek the width and height */
            {
                Bitmap sbmp = new Bitmap((int)nWidth, (int)nHeight, PixelFormat.Format24bppRgb);

                BitmapData bmpdata = sbmp.LockBits(new Rectangle(0, 0, sbmp.Width, sbmp.Height), ImageLockMode.WriteOnly,
                    sbmp.PixelFormat);
                toupcam1_.PullStillImage(bmpdata.Scan0, 24, out nWidth, out nHeight);
                sbmp.UnlockBits(bmpdata);

                sbmp.Save("ToupcamTwoCameraSupport.jpg");
            }

            if (toupcam2_.PullStillImage(IntPtr.Zero, 24, out nWidth, out nHeight)) /* peek the width and height */
            {
                Bitmap sbmp = new Bitmap((int)nWidth, (int)nHeight, PixelFormat.Format24bppRgb);

                BitmapData bmpdata = sbmp.LockBits(new Rectangle(0, 0, sbmp.Width, sbmp.Height), ImageLockMode.WriteOnly,
                    sbmp.PixelFormat);
                toupcam2_.PullStillImage(bmpdata.Scan0, 24, out nWidth, out nHeight);
                sbmp.UnlockBits(bmpdata);

                sbmp.Save("toupcamdemowinformcs2.jpg");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /* camera 1 */
            SelectImage1OnTop.Enabled = false;
            ResoulationList1.Enabled = false;
            ExpoValue1.Enabled = false;
            AutoExposure1.Enabled = false;
            WhiteBalancePush1.Enabled = false;
            TempValue1.Enabled = false;
            TintValue1.Enabled = false;
            Vertical1.Enabled = false;
            Horizontal1.Enabled = false;
            OpacityImage1.Enabled = false;
            CaptureCamera1.Enabled = false;

            /* camera 2 */
            SelectImage2OnTop.Enabled = false;
            ResoulationList2.Enabled = false;
            ExpoValue2.Enabled = false;
            AutoExposure2.Enabled = false;
            WhiteBalancePush2.Enabled = false;
            TempValue2.Enabled = false;
            TintValue2.Enabled = false;
            Vertical2.Enabled = false;
            Horizontal2.Enabled = false;
            OpacityImage2.Enabled = false;
            CaptureCamera2.Enabled = false;

            /* combined Picture */
            
            pictureBox3.BackColor = Color.White;
            LeftLineValue.Enabled = false;
            ShowLeftLine.Enabled = false;
            RightLineValue.Enabled = false;
            ShowRightLine.Enabled = false;
            Capture3.Enabled = false;

            /* debug */
#if DEBUG
            pictureBox1.Image = Resources.pic1;
            pictureBox2.Image = Resources.pic2;
#endif
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            if (MSG_CAMEVENT == m.Msg)
            {
                switch ((ToupCam.eEVENT)m.WParam.ToInt32())
                {
                    case ToupCam.eEVENT.EVENT_ERROR:
                        OnEventError();
                        break;
                    case ToupCam.eEVENT.EVENT_DISCONNECTED:
                        OnEventDisconnected();
                        break;
                    case ToupCam.eEVENT.EVENT_EXPOSURE:
                        OnEventExposure1();
                        OnEventExposure2();
                        break;
                    case ToupCam.eEVENT.EVENT_IMAGE:
                        OnEventImage();
                        break;
                    case ToupCam.eEVENT.EVENT_STILLIMAGE:
                        OnEventStillImage();
                        break;
                    case ToupCam.eEVENT.EVENT_TEMPTINT:
                        OnEventTempTint();
                        break;
                }
                return;
            }
            base.WndProc(ref m);
        }

        private void OnStart(object sender, EventArgs e)
        {
            if (toupcam1_ != null || toupcam2_ != null)
                return;

            ToupCam.Instance[] arr = ToupCam.Enum();
            if (arr.Length <= 0)
            {
                MessageBox.Show(@"no device");
            }
            else
            {
                if (arr.Length >= 1)
                {
                    toupcam1_ = new ToupCam();
                }
                if (arr.Length == 2)
                {
                    toupcam2_ = new ToupCam();
                }

                if (!string.IsNullOrWhiteSpace(arr[0].id))
                {
                    if (toupcam1_ != null && !toupcam1_.Open(arr[0].id))
                    {
                        toupcam1_ = null;
                    }
                }

                if (!string.IsNullOrWhiteSpace(arr[1].id))
                {
                    if (toupcam2_ != null && !toupcam2_.Open(arr[1].id))
                    {
                        toupcam2_ = null;
                    }
                }

                if (toupcam1_ != null)
                {
                    SelectImage1OnTop.Enabled = true;
                    ResoulationList1.Enabled = true;
                    ExpoValue1.Enabled = true;
                    AutoExposure1.Enabled = true;
                    WhiteBalancePush1.Enabled = true;
                    TempValue1.Enabled = true;
                    TintValue1.Enabled = true;
                    Vertical1.Enabled = true;
                    Horizontal1.Enabled = true;
                    CaptureCamera1.Enabled = true;


                    LeftLineValue.Enabled = true;
                    ShowLeftLine.Enabled = true;
                    RightLineValue.Enabled = true;
                    ShowRightLine.Enabled = true;

                    lOpacityImage1.Text = @"Opacity - 100%";
                    CaptureCamera1.ContextMenuStrip = null;
                    InitSnapContextMenuAndExpoTimeRange();

                    TempValue1.Minimum = 2000;
                    TempValue1.Maximum = 15000;

                    TintValue1.Minimum = 200;
                    TintValue1.Maximum = 2500;
                    OnEventTempTint();

                    uint resnum = toupcam1_.ResolutionNumber;
                    uint eSize = 0;
                    if (toupcam1_.get_eSize(out eSize))
                    {
                        for (uint i = 0; i < resnum; ++i)
                        {
                            int w = 0, h = 0;
                            if (toupcam1_.get_Resolution(i, out w, out h))
                                ResoulationList1.Items.Add(w + "*" + h);
                        }
                        ResoulationList1.SelectedIndex = (int)eSize;

                        int width = 0, height = 0;
                        if (toupcam1_.get_Size(out width, out height))
                        {
                            bmp1_ = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                            if (!toupcam1_.StartPullModeWithWndMsg(Handle, MSG_CAMEVENT))
                                MessageBox.Show(@"failed to start device");
                            else
                            {
                                bool autoexpo = true;
                                toupcam1_.get_AutoExpoEnable(out autoexpo);
                                AutoExposure1.Checked = autoexpo;
                                ExpoValue1.Enabled = !AutoExposure1.Checked;
                            }
                        }
                    }
                }

                if (toupcam2_ != null)
                {
                    SelectImage2OnTop.Enabled = true;
                    ResoulationList2.Enabled = true;
                    ExpoValue2.Enabled = true;
                    AutoExposure2.Enabled = true;
                    WhiteBalancePush2.Enabled = true;
                    TempValue2.Enabled = true;
                    TintValue2.Enabled = true;
                    Vertical2.Enabled = true;
                    Horizontal2.Enabled = true;
                    CaptureCamera2.Enabled = true;

                    LeftLineValue.Enabled = true;
                    ShowLeftLine.Enabled = true;
                    RightLineValue.Enabled = true;
                    ShowRightLine.Enabled = true;

                    lOpacityImage2.Text = @"Opacity - 100%";
                    CaptureCamera1.ContextMenuStrip = null;
                    InitSnapContextMenuAndExpoTimeRange();

                    TempValue2.Minimum = 2000;
                    TempValue2.Maximum = 15000;

                    TintValue2.Minimum = 200;
                    TintValue2.Maximum = 2500;

                    OnEventTempTint2();

                    uint resnum = toupcam2_.ResolutionNumber;
                    uint eSize = 0;
                    if (toupcam2_.get_eSize(out eSize))
                    {
                        for (uint i = 0; i < resnum; ++i)
                        {
                            int w = 0, h = 0;
                            if (toupcam2_.get_Resolution(i, out w, out h))
                                ResoulationList2.Items.Add(w + "*" + h);
                        }
                        ResoulationList2.SelectedIndex = (int)eSize;

                        int width = 0, height = 0;
                        if (toupcam2_.get_Size(out width, out height))
                        {
                            bmp2_ = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                            if (!toupcam2_.StartPullModeWithWndMsg(Handle, MSG_CAMEVENT))
                                MessageBox.Show(@"failed to start device");
                            else
                            {
                                bool autoexpo = true;
                                toupcam2_.get_AutoExpoEnable(out autoexpo);
                                AutoExposure2.Checked = autoexpo;
                                ExpoValue2.Enabled = !AutoExposure2.Checked;
                            }
                        }
                    }
                }
                if (toupcam1_ != null && toupcam2_ != null)
                {
                    //opacity is avaliable only if both camera is connected
                    _imageFilter = new ImageFilter();
                    image1Opacity = 100;
                    image2Opacity = 100;
                    IsRunning = false;
                    OpacityImage1.Enabled = true;
                    OpacityImage1.Value = (decimal)image1Opacity;
                    OpacityImage2.Enabled = true;
                    OpacityImage2.Value = (decimal)image2Opacity;
                    Capture3.Enabled = true;
                }
            }
        }

        private void SnapClickedHandler1(object sender, ToolStripItemClickedEventArgs e)
        {
            int k = CaptureCamera1.ContextMenuStrip.Items.IndexOf(e.ClickedItem);
            if (k >= 0)
            {
                toupcam1_.Snap((uint)k);
            }
        }

        private void SnapClickedHandler2(object sender, ToolStripItemClickedEventArgs e)
        {
            int k = CaptureCamera2.ContextMenuStrip.Items.IndexOf(e.ClickedItem);
            if (k >= 0)
            {
                toupcam2_.Snap((uint)k);
            }
        }

        private void InitSnapContextMenuAndExpoTimeRange()
        {
            uint nMin = 0, nMax = 0, nDef = 0;
            if (toupcam1_ != null)
            {
                if (toupcam1_.get_ExpTimeRange(out nMin, out nMax, out nDef))
                {
                    ExpoValue1.Minimum = (int)nMin;
                    ExpoValue1.Maximum = (int)nMax;
                }

                OnEventExposure1();

                if (toupcam1_.StillResolutionNumber <= 0)
                    return;

                CaptureCamera1.ContextMenuStrip = new ContextMenuStrip();
                CaptureCamera1.ContextMenuStrip.ItemClicked += SnapClickedHandler1;

                if (toupcam1_.StillResolutionNumber < toupcam1_.ResolutionNumber)
                {
                    uint eSize = 0;
                    if (toupcam1_.get_eSize(out eSize))
                    {
                        if (0 == eSize)
                        {
                            StringBuilder sb = new StringBuilder();
                            int w = 0, h = 0;
                            toupcam1_.get_Resolution(eSize, out w, out h);
                            sb.Append(w);
                            sb.Append(" * ");
                            sb.Append(h);
                            CaptureCamera1.ContextMenuStrip.Items.Add(sb.ToString());
                            return;
                        }
                    }
                }

                for (uint i = 0; i < toupcam1_.ResolutionNumber; ++i)
                {
                    StringBuilder sb = new StringBuilder();
                    int w = 0, h = 0;
                    toupcam1_.get_Resolution(i, out w, out h);
                    sb.Append(w);
                    sb.Append(" * ");
                    sb.Append(h);
                    CaptureCamera1.ContextMenuStrip.Items.Add(sb.ToString());
                }
            }


            if (toupcam2_ != null)
            {
                if (toupcam2_.get_ExpTimeRange(out nMin, out nMax, out nDef))
                {
                    ExpoValue2.Minimum = (int)nMin;
                    ExpoValue2.Maximum = (int)nMax;
                }

                OnEventExposure2();

                if (toupcam2_ != null && toupcam2_.StillResolutionNumber <= 0)
                    return;

                CaptureCamera2.ContextMenuStrip = new ContextMenuStrip();
                CaptureCamera2.ContextMenuStrip.ItemClicked += SnapClickedHandler2;

                if (toupcam2_.StillResolutionNumber < toupcam2_.ResolutionNumber)
                {
                    uint eSize = 0;
                    if (toupcam2_.get_eSize(out eSize))
                    {
                        if (0 == eSize)
                        {
                            StringBuilder sb = new StringBuilder();
                            int w = 0, h = 0;
                            toupcam2_.get_Resolution(eSize, out w, out h);
                            sb.Append(w);
                            sb.Append(" * ");
                            sb.Append(h);
                            CaptureCamera2.ContextMenuStrip.Items.Add(sb.ToString());
                            return;
                        }
                    }
                }

                for (uint i = 0; i < toupcam2_.ResolutionNumber; ++i)
                {
                    StringBuilder sb = new StringBuilder();
                    int w = 0, h = 0;
                    toupcam2_.get_Resolution(i, out w, out h);
                    sb.Append(w);
                    sb.Append(" * ");
                    sb.Append(h);
                    CaptureCamera2.ContextMenuStrip.Items.Add(sb.ToString());
                }
            }
        }

        private void OnSnap(object sender, EventArgs e)
        {
            if (toupcam1_ == null) return;
            if (toupcam1_.StillResolutionNumber <= 0)
            {

                if (bmp1_ != null)
                {
                    bmp1_.Save("Camera2.jpg");
                }

            }
            else
            {
                if (CaptureCamera1.ContextMenuStrip != null)
                {
                    CaptureCamera1.ContextMenuStrip.Show(Cursor.Position);
                }

            }
        }

        private void OnSnap2(object sender, EventArgs e)
        {
            if (toupcam2_ == null) return;
            if (toupcam2_.StillResolutionNumber <= 0)
            {
                if (bmp2_ != null)
                    bmp2_.Save("Camera2.jpg");
            }
            else
            {

                if (CaptureCamera2.ContextMenuStrip != null)
                {
                    CaptureCamera2.ContextMenuStrip.Show(Cursor.Position);
                }

            }
        }

        private void OnSnap3(object sender, EventArgs e)
        {
            if (_imageFilter.Result == null) return;
            Bitmap resultBmp = (Bitmap)_imageFilter.Result;
            resultBmp.Save("Combo.jpg");
        }

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            if (toupcam1_ != null)
            {
                toupcam1_.Close();
                toupcam1_ = null;
            }

            if (toupcam2_ != null)
            {
                toupcam2_.Close();
                toupcam2_ = null;
            }
        }

        private void OnSelectResolution1(object sender, EventArgs e)
        {
            if (toupcam1_ == null) return;
            uint eSize = 0;
            if (toupcam1_.get_eSize(out eSize))
            {
                if (eSize != ResoulationList1.SelectedIndex)
                {
                    CaptureCamera1.ContextMenuStrip = null;

                    toupcam1_.Stop();
                    toupcam1_.put_eSize((uint)ResoulationList1.SelectedIndex);

                    InitSnapContextMenuAndExpoTimeRange();
                    OnEventTempTint();

                    int width = 0, height = 0;
                    if (toupcam1_.get_Size(out width, out height))
                    {
                        bmp1_ = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                        toupcam1_.StartPullModeWithWndMsg(Handle, MSG_CAMEVENT);
                    }
                }
            }
        }

        private void OnSelectResolution2(object sender, EventArgs e)
        {
            if (toupcam2_ == null) return;
            uint eSize = 0;
            if (toupcam2_.get_eSize(out eSize))
            {
                if (eSize != ResoulationList1.SelectedIndex)
                {
                    CaptureCamera2.ContextMenuStrip = null;

                    toupcam2_.Stop();
                    toupcam2_.put_eSize((uint)ResoulationList1.SelectedIndex);

                    InitSnapContextMenuAndExpoTimeRange();
                    OnEventTempTint2();

                    int width = 0, height = 0;
                    if (toupcam2_.get_Size(out width, out height))
                    {
                        bmp1_ = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                        toupcam2_.StartPullModeWithWndMsg(Handle, MSG_CAMEVENT);
                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        //exposure value1
        {
            if (toupcam1_ != null)
            {
                toupcam1_.put_AutoExpoEnable(AutoExposure1.Checked);
            }

            ExpoValue1.Enabled = !AutoExposure1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        //exposure value2
        {
            if (toupcam2_ != null)
            {
                toupcam2_.put_AutoExpoEnable(AutoExposure2.Checked);
            }

            ExpoValue2.Enabled = !AutoExposure2.Checked;
        }

        private void OnExpoValueChange(object sender, EventArgs e)
        {
            if (toupcam1_ == null) return;
            uint n = uint.Parse(ExpoValue1.Text);
            toupcam1_.put_ExpoTime(n);
            label1.Text = (n / 1000) + @" ms";
        }

        private void OnExpoValueChange2(object sender, EventArgs e)
        {
            if (toupcam2_ == null) return;
            uint n = (uint)ExpoValue2.Value;
            toupcam2_.put_ExpoTime(n);
            label4.Text = (n / 1000) + @" ms";
        }

        private void Form_SizeChanged(object sender, EventArgs e)
        {
            //pictureBox1.Width = ClientRectangle.Right - button1.Bounds.Right - 20;
            //pictureBox1.Height = ClientRectangle.Height - 8;
        }

        private void OnEventTempTint()
        {
            if (toupcam1_ == null) return;
            int nTemp = 0, nTint = 0;
            if (toupcam1_.get_TempTint(out nTemp, out nTint))
            {
                TempValue1.Value = nTemp;
                TintValue1.Value = nTint;
            }
        }

        private void OnEventTempTint2()
        {
            if (toupcam2_ == null) return;
            int nTemp = 0, nTint = 0;
            if (toupcam2_.get_TempTint(out nTemp, out nTint))
            {
                TempValue2.Value = nTemp;
                TintValue2.Value = nTint;
            }
        }

        private void OnWhiteBalanceOnePush1(object sender, EventArgs e)
        {
            if (toupcam1_ != null)
            {
                toupcam1_.AwbOnePush(null);
            }
        }

        private void OnWhiteBalanceOnePush2(object sender, EventArgs e)
        {
            if (toupcam2_ != null)
            {
                toupcam2_.AwbOnePush(null);
            }
        }

        private void OnTempTintChanged1(object sender, EventArgs e)
        {
            if (toupcam1_ != null)
            {
                toupcam1_.put_TempTint((int)TempValue1.Value, (int)TintValue1.Value);
            }

        }

        private void OnTempTintChanged2(object sender, EventArgs e)
        {
            if (toupcam2_ != null)
            {
                toupcam2_.put_TempTint((int)TempValue2.Value, (int)TintValue2.Value);
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void tbOpacityImage1_Scroll(object sender, EventArgs e)
        {
            image1Opacity = (int)OpacityImage1.Value;
            lOpacityImage1.Text = @"Opacity - " + image1Opacity + @"%";
        }

        private void tbOpacityImage2_Scroll(object sender, EventArgs e)
        {
            image2Opacity = (int)OpacityImage2.Value;
            lOpacityImage2.Text = @"Opacity - " + image2Opacity + @"%";
        }

        private void SelectImage1OnTop_CheckedChanged(object sender, EventArgs e)
        {
            SelectImage2OnTop.Checked = false;
            
            imageOneOnTop = true;
        }

        private void SelectImage2OnTop_CheckedChanged(object sender, EventArgs e)
        {
            SelectImage1OnTop.Checked = false;
            imageOneOnTop = false;
        }

        private void cbRightLIne_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.Checked)
            {
                int rightLineCalculated = 0;
                if (pictureBox3 != null)
                {
                    rightLineCalculated = pictureBox3.Height - int.Parse(RightLineValue.Text ?? "" + -1);
                }

                _imageFilter.RightLine = rightLineCalculated;
            }
            else
            {
                _imageFilter.RightLine = -1;
            }
        }

        private void ckbLeftLine_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.Checked)
            {
                int leftLineCalculated = int.Parse(RightLineValue.Text ?? "" + 0);

                _imageFilter.LeftLine = leftLineCalculated;
            }
            else
            {
                _imageFilter.LeftLine = -1;
            }
        }

        private void FlipCamera1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            switch (cb.Text)
            {
                case "Horizontal":
                    _flipHorizontal1 = cb.Checked;
                    break;
                case "Vertical":
                    _flipVertical1 = cb.Checked;
                    break;
            }
        }

        private void FlipCamera2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            switch (cb.Text)
            {
                case "Horizontal":
                    _flipHorizontal2 = cb.Checked;
                    break;
                case "Vertical":
                    _flipVertical2 = cb.Checked;
                    break;
            }
        }
    }
}