#region

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

#endregion

namespace MVcamview
{
    internal class ImageFilter
    {
        private Image _FirstImageAfterOpacity;
        private Image _SecondImageAfterOpacity;
        public Image FirstImage = null;
        public float FirstImageOpacity = 1.0f;
        public Image Result;
        public Image SecondImage = null;
        public float SecondImageOpacity = 1.0f;
        public int RightLine = -1;
        public int LeftLine = -1;
        public Color LineColor = Color.Red;
        public int Thickness=5;

        public void StartToCombine()
        {
            _FirstImageAfterOpacity = SetImageOpacity(FirstImage, FirstImageOpacity);
            _SecondImageAfterOpacity = SetImageOpacity(SecondImage, SecondImageOpacity);
            Result = CombineTwoPicture(_FirstImageAfterOpacity, _SecondImageAfterOpacity);
            if (RightLine!=-1)
            {
                DrawVerticalLine(RightLine, LineColor);
            }

            if (LeftLine != -1)
            {
                DrawVerticalLine(LeftLine, LineColor);
            }

        }

        public Bitmap ChangeOpacity(Image img, float opacityvalue)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix();
            colormatrix.Matrix33 = opacityvalue;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height,
                GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose(); // Releasing all resource used by graphics 
            return bmp;
        }

        /// <summary>
        ///     method for changing the opacity of an image
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
                    gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height,
                        GraphicsUnit.Pixel, attributes);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public Image CombineTwoPicture(Image imageOne, Image imageTwo)
        {
            // ReSharper disable once SuggestVarOrType_SimpleTypes
            Bitmap first = new Bitmap(imageOne);
            Bitmap second = new Bitmap(imageTwo);
            //Bitmap result = new Bitmap(first.Width, first.Height);
            //fix :
            Bitmap result = new Bitmap(Math.Max(first.Width, second.Width), Math.Max(first.Height, second.Height));
            //Console.WriteLine(first.Width);
            Graphics g = Graphics.FromImage(result);
            g.DrawImageUnscaled(first, 0, 0);
            g.DrawImageUnscaled(second, 0, 0);
            return result;
        }

        public Image DrawVerticalLine(int x, Color color)
        {
            Bitmap imgBitmap = (Bitmap)Result;

            LockBitmap lb = new LockBitmap(imgBitmap);
            lb.LockBits();
            for (int y = 0; y < imgBitmap.Height; y++)
            {
                for (int t = 1; t < Thickness; t++)
                {
                    lb.SetPixel(x+t, y, color);
                }
            }
            lb.UnlockBits();
            return imgBitmap;
        }
    }
}