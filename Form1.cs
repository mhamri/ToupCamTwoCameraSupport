using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using ToupTek;
using System.Runtime.InteropServices;
using toupcamTwoCameraSupport;

namespace ToupcamTwoCameraSupport
{
    public partial class Form1 : Form
    {
        private ToupCam toupcam1_ = null;
        private ToupCam toupcam2_ = null;
        private Bitmap bmp1_ = null;
        private Bitmap bmp2_ = null;
        private uint MSG_CAMEVENT = 0x8001; // WM_APP = 0x8000
        private float image1Opacity;
        private float image2Opacity;


        private void savefile(IntPtr pData, ref ToupCam.BITMAPINFOHEADER header)
        {
            Bitmap bmp = new Bitmap(header.biWidth, header.biHeight, PixelFormat.Format24bppRgb);
            BitmapData bmpdata = bmp.LockBits(new Rectangle(0, 0, header.biWidth, header.biHeight), ImageLockMode.WriteOnly, bmp.PixelFormat);

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
            MessageBox.Show("Error");
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

            MessageBox.Show("The camera is disconnected, maybe has been pulled out.");
        }

        private void OnEventExposure()
        {
            if (toupcam1_ != null)
            {
                uint nTime = 0;
                if (toupcam1_.get_ExpoTime(out nTime))
                {
                    trackBar1.Value = (int)nTime;
                    label1.Text = (nTime / 1000).ToString() + " ms";
                }
            }

            if (toupcam2_ != null)
            {
                uint nTime = 0;
                if (toupcam2_.get_ExpoTime(out nTime))
                {
                    trackBar4.Value = (int)nTime;
                    label4.Text = (nTime / 1000).ToString() + " ms";
                }
            }
        }

        private void OnEventImage()
        {
            if (bmp1_ != null)
            {
                BitmapData bmpdata = bmp1_.LockBits(new Rectangle(0, 0, bmp1_.Width, bmp1_.Height), ImageLockMode.WriteOnly, bmp1_.PixelFormat);

                uint nWidth = 0, nHeight = 0;
                toupcam1_.PullImage(bmpdata.Scan0, 24, out nWidth, out nHeight);

                bmp1_.UnlockBits(bmpdata);

                pictureBox1.Image = bmp1_;
                pictureBox1.Invalidate();
            }

            if (bmp2_ != null)
            {
                BitmapData bmpdata = bmp2_.LockBits(new Rectangle(0, 0, bmp2_.Width, bmp2_.Height), ImageLockMode.WriteOnly, bmp2_.PixelFormat);

                uint nWidth = 0, nHeight = 0;
                toupcam2_.PullImage(bmpdata.Scan0, 24, out nWidth, out nHeight);

                bmp2_.UnlockBits(bmpdata);

                pictureBox2.Image = bmp2_;
                pictureBox2.Invalidate();
            }

        }

        private void OnEventStillImage()
        {
            uint nWidth = 0, nHeight = 0;
            if (toupcam1_.PullStillImage(IntPtr.Zero, 24, out nWidth, out nHeight))   /* peek the width and height */
            {
                Bitmap sbmp = new Bitmap((int)nWidth, (int)nHeight, PixelFormat.Format24bppRgb);

                BitmapData bmpdata = sbmp.LockBits(new Rectangle(0, 0, sbmp.Width, sbmp.Height), ImageLockMode.WriteOnly, sbmp.PixelFormat);
                toupcam1_.PullStillImage(bmpdata.Scan0, 24, out nWidth, out nHeight);
                sbmp.UnlockBits(bmpdata);

                sbmp.Save("ToupcamTwoCameraSupport.jpg");
            }

            if (toupcam2_.PullStillImage(IntPtr.Zero, 24, out nWidth, out nHeight))   /* peek the width and height */
            {
                Bitmap sbmp = new Bitmap((int)nWidth, (int)nHeight, PixelFormat.Format24bppRgb);

                BitmapData bmpdata = sbmp.LockBits(new Rectangle(0, 0, sbmp.Width, sbmp.Height), ImageLockMode.WriteOnly, sbmp.PixelFormat);
                toupcam2_.PullStillImage(bmpdata.Scan0, 24, out nWidth, out nHeight);
                sbmp.UnlockBits(bmpdata);

                sbmp.Save("toupcamdemowinformcs2.jpg");
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
            trackBar1.Enabled = false;
            trackBar2.Enabled = false;
            trackBar3.Enabled = false;
            checkBox1.Enabled = false;
            comboBox1.Enabled = false;
            trackBar4.Enabled = false;

            tbTempCamera2.Enabled = false;
            tbTintCamera2.Enabled = false;
            cbAutoExposureCamera2.Enabled = false;
            cbResolutionCamera2.Enabled = false;
            lOpacityImage1.Enabled = false;
            lOpacityImage2.Enabled = false;

#if DEBUG
            pictureBox1.Image = toupcamTwoCameraSupport.Properties.Resources.pic1;
            pictureBox2.Image = toupcamTwoCameraSupport.Properties.Resources.pic2;
            lOpacityImage1.Enabled = true;
            lOpacityImage2.Enabled = true;


#endif
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
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
                        OnEventExposure();
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
                MessageBox.Show("no device");
            }
            else
            {
                if (arr.Length == 1) { toupcam1_ = new ToupCam(); }
                if (arr.Length == 2) { toupcam2_ = new ToupCam(); }

                if (!string.IsNullOrWhiteSpace(arr[0].id))
                {
                    if (!toupcam1_.Open(arr[0].id))
                    {
                        toupcam1_ = null;
                    }

                }

                if (!string.IsNullOrWhiteSpace(arr[1].id))
                {
                    if (!toupcam2_.Open(arr[1].id))
                    {
                        toupcam2_ = null;
                    }

                }

                if (toupcam1_ != null)
                {
                    checkBox1.Enabled = true;
                    trackBar1.Enabled = true;
                    trackBar2.Enabled = true;
                    trackBar3.Enabled = true;
                    comboBox1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button2.ContextMenuStrip = null;
                    InitSnapContextMenuAndExpoTimeRange();

                    trackBar2.SetRange(2000, 15000);
                    trackBar3.SetRange(200, 2500);
                    OnEventTempTint();

                    uint resnum = toupcam1_.ResolutionNumber;
                    uint eSize = 0;
                    if (toupcam1_.get_eSize(out eSize))
                    {
                        for (uint i = 0; i < resnum; ++i)
                        {
                            int w = 0, h = 0;
                            if (toupcam1_.get_Resolution(i, out w, out h))
                                comboBox1.Items.Add(w.ToString() + "*" + h.ToString());
                        }
                        comboBox1.SelectedIndex = (int)eSize;

                        int width = 0, height = 0;
                        if (toupcam1_.get_Size(out width, out height))
                        {
                            bmp1_ = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                            if (!toupcam1_.StartPullModeWithWndMsg(this.Handle, MSG_CAMEVENT))
                                MessageBox.Show("failed to start device");
                            else
                            {
                                bool autoexpo = true;
                                toupcam1_.get_AutoExpoEnable(out autoexpo);
                                checkBox1.Checked = autoexpo;
                                trackBar1.Enabled = !checkBox1.Checked;
                            }
                        }
                    }
                }

                if (toupcam2_ != null)
                {
                    checkBox1.Enabled = true;
                    trackBar4.Enabled = true;
                    comboBox1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    tbTempCamera2.Enabled = true;
                    tbTintCamera2.Enabled = true;
                    button2.ContextMenuStrip = null;
                    InitSnapContextMenuAndExpoTimeRange();

                    tbTempCamera2.SetRange(2000, 15000);
                    tbTintCamera2.SetRange(200, 2500);
                    OnEventTempTint();

                    uint resnum = toupcam1_.ResolutionNumber;
                    uint eSize = 0;
                    if (toupcam2_.get_eSize(out eSize))
                    {
                        for (uint i = 0; i < resnum; ++i)
                        {
                            int w = 0, h = 0;
                            if (toupcam2_.get_Resolution(i, out w, out h))
                                cbResolutionCamera2.Items.Add(w.ToString() + "*" + h.ToString());
                        }
                        cbResolutionCamera2.SelectedIndex = (int)eSize;

                        int width = 0, height = 0;
                        if (toupcam2_.get_Size(out width, out height))
                        {
                            bmp2_ = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                            if (!toupcam2_.StartPullModeWithWndMsg(this.Handle, MSG_CAMEVENT))
                                MessageBox.Show("failed to start device");
                            else
                            {
                                bool autoexpo = true;
                                toupcam2_.get_AutoExpoEnable(out autoexpo);
                                cbAutoExposureCamera2.Checked = autoexpo;
                                trackBar4.Enabled = !cbAutoExposureCamera2.Checked;
                            }
                        }
                    }
                }

            }
        }

        private void SnapClickedHandler(object sender, ToolStripItemClickedEventArgs e)
        {
            int k = button2.ContextMenuStrip.Items.IndexOf(e.ClickedItem);
            if (k >= 0)
            {
                toupcam1_.Snap((uint)k);
                toupcam2_.Snap((uint)k);
            }
        }

        private void InitSnapContextMenuAndExpoTimeRange()
        {
            if (toupcam1_ == null || toupcam2_ == null)
                return;

            uint nMin = 0, nMax = 0, nDef = 0;
            if (toupcam1_.get_ExpTimeRange(out nMin, out nMax, out nDef))
                trackBar1.SetRange((int)nMin, (int)nMax);

            if (toupcam2_.get_ExpTimeRange(out nMin, out nMax, out nDef))
                trackBar4.SetRange((int)nMin, (int)nMax);

            OnEventExposure();

            if (toupcam1_.StillResolutionNumber <= 0)
                return;

            if (toupcam2_.StillResolutionNumber <= 0)
                return;

            button2.ContextMenuStrip = new ContextMenuStrip();
            button2.ContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.SnapClickedHandler);

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
                        button2.ContextMenuStrip.Items.Add(sb.ToString());
                        return;
                    }
                }
            }

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
                        btnSnapCamera2.ContextMenuStrip.Items.Add(sb.ToString());
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
                button2.ContextMenuStrip.Items.Add(sb.ToString());
            }

            for (uint i = 0; i < toupcam2_.ResolutionNumber; ++i)
            {
                StringBuilder sb = new StringBuilder();
                int w = 0, h = 0;
                toupcam2_.get_Resolution(i, out w, out h);
                sb.Append(w);
                sb.Append(" * ");
                sb.Append(h);
                btnSnapCamera2.ContextMenuStrip.Items.Add(sb.ToString());
            }
        }

        private void OnSnap(object sender, EventArgs e)
        {
            if (toupcam1_ != null)
            {
                if (toupcam1_.StillResolutionNumber <= 0)
                {
                    if (bmp1_ != null)
                    {
                        bmp1_.Save("ToupcamTwoCameraSupport.jpg");
                    }
                }
                else
                {
                    if (button2.ContextMenuStrip != null)
                        button2.ContextMenuStrip.Show(Cursor.Position);
                }
            }
        }

        private void onSnap2(object sender, EventArgs e)
        {
            if (toupcam2_ != null)
            {
                if (toupcam2_.StillResolutionNumber <= 0)
                {
                    if (bmp2_ != null)
                    {
                        bmp2_.Save("toupcamdemowinformcs2.jpg");
                    }
                }
                else
                {
                    if (btnSnapCamera2.ContextMenuStrip != null)
                        btnSnapCamera2.ContextMenuStrip.Show(Cursor.Position);
                }
            }
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

        private void OnSelectResolution(object sender, EventArgs e)
        {
            if (toupcam1_ != null)
            {
                uint eSize = 0;
                if (toupcam1_.get_eSize(out eSize))
                {
                    if (eSize != comboBox1.SelectedIndex)
                    {
                        button2.ContextMenuStrip = null;

                        toupcam1_.Stop();
                        toupcam1_.put_eSize((uint)comboBox1.SelectedIndex);

                        InitSnapContextMenuAndExpoTimeRange();
                        OnEventTempTint();

                        int width = 0, height = 0;
                        if (toupcam1_.get_Size(out width, out height))
                        {
                            bmp1_ = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                            toupcam1_.StartPullModeWithWndMsg(this.Handle, MSG_CAMEVENT);
                        }
                    }
                }
            }
        }

        private void OnSelectResolution2(object sender, EventArgs e)
        {
            if (toupcam2_ != null)
            {
                uint eSize = 0;
                if (toupcam2_.get_eSize(out eSize))
                {
                    if (eSize != comboBox1.SelectedIndex)
                    {
                        btnSnapCamera2.ContextMenuStrip = null;

                        toupcam2_.Stop();
                        toupcam2_.put_eSize((uint)comboBox1.SelectedIndex);

                        InitSnapContextMenuAndExpoTimeRange();
                        OnEventTempTint();

                        int width = 0, height = 0;
                        if (toupcam2_.get_Size(out width, out height))
                        {
                            bmp1_ = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                            toupcam2_.StartPullModeWithWndMsg(this.Handle, MSG_CAMEVENT);
                        }
                    }
                }
            }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (toupcam1_ != null)
                toupcam1_.put_AutoExpoEnable(checkBox1.Checked);
            trackBar1.Enabled = !checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (toupcam2_ != null)
                toupcam2_.put_AutoExpoEnable(checkBox1.Checked);
            trackBar4.Enabled = !cbAutoExposureCamera2.Checked;
        }

        private void OnExpoValueChange(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                if (toupcam1_ != null)
                {
                    uint n = (uint)trackBar1.Value;
                    toupcam1_.put_ExpoTime(n);
                    label1.Text = (n / 1000).ToString() + " ms";
                }
            }
        }

        private void OnExpoValueChange2(object sender, EventArgs e)
        {
            if (!cbAutoExposureCamera2.Checked)
            {
                if (toupcam2_ != null)
                {
                    uint n = (uint)trackBar1.Value;
                    toupcam2_.put_ExpoTime(n);
                    label4.Text = (n / 1000).ToString() + " ms";
                }
            }
        }

        private void Form_SizeChanged(object sender, EventArgs e)
        {
            //pictureBox1.Width = ClientRectangle.Right - button1.Bounds.Right - 20;
            //pictureBox1.Height = ClientRectangle.Height - 8;
        }

        private void OnEventTempTint()
        {
            if (toupcam1_ != null)
            {
                int nTemp = 0, nTint = 0;
                if (toupcam1_.get_TempTint(out nTemp, out nTint))
                {
                    label2.Text = nTemp.ToString();
                    label3.Text = nTint.ToString();
                    trackBar2.Value = nTemp;
                    trackBar3.Value = nTint;
                }
            }
        }

        private void OnWhiteBalanceOnePush(object sender, EventArgs e)
        {
            if (toupcam1_ != null)
                toupcam1_.AwbOnePush(null);
        }

        private void OnTempTintChanged(object sender, EventArgs e)
        {
            if (toupcam1_ != null)
                toupcam1_.put_TempTint(trackBar2.Value, trackBar3.Value);
            label2.Text = trackBar2.Value.ToString();
            label3.Text = trackBar3.Value.ToString();
        }


        /// <summary>  
        /// method for changing the opacity of an image  
        /// </summary>  
        /// <param name="image">image to set opacity on</param>  
        /// <param name="opacity">percentage of opacity</param>  
        /// <returns></returns>  
        public Image SetImageOpacity(Image image, float opacity)
        {
            try
            {
                //create a Bitmap the size of the image provided  
                Bitmap bmp = new Bitmap(image.Width, image.Height);

                //create a graphics object from the image  
                using (Graphics gfx = Graphics.FromImage(bmp))
                {

                    //create a color matrix object  
                    ColorMatrix matrix = new ColorMatrix();

                    //set the opacity  
                    matrix.Matrix33 = opacity;

                    //create image attributes  
                    ImageAttributes attributes = new ImageAttributes();

                    //set the color(opacity) of the image  
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    //now draw the image  
                    gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void tbOpacityImage1_Scroll(object sender, EventArgs e)
        {
            image1Opacity = tbOpacityImage1.Value;
            lOpacityImage1.Text = "Opacity - " + image1Opacity.ToString();
            pictureBox3.Image = ImageFilter.ChangeOpacity(pictureBox1.Image,image1Opacity/100);
        }

        private void tbOpacityImage2_Scroll(object sender, EventArgs e)
        {
            lOpacityImage2.Text = "Opacity - " + tbOpacityImage2.Value.ToString();

        }

    }
}
