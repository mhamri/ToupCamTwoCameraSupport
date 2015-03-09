using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace toupcamTwoCameraSupport
{
    class ImageFilter
    {

        public static Bitmap ChangeOpacity(Image img, float opacityvalue)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix();
            colormatrix.Matrix33 = opacityvalue;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();   // Releasing all resource used by graphics 
            return bmp;
        }

        /// <summary>  
        /// method for changing the opacity of an image  
        /// </summary>  
        /// <param name="image">image to set opacity on</param>  
        /// <param name="opacity">percentage of opacity</param>  
        /// <returns></returns>  
        public static Image SetImageOpacity(Image image, float opacity)
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



        public static Image CombineTwoPicture(Image imageOne, Image imageTwo)
        {

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
        /// <summary>
        /// combine two not transparent image together, but all the time change opacity of second image
        /// my assumption is imageTwo all the time is on the top picture
        /// </summary>
        /// <param name="imageOne">bxack ground</param>
        /// <param name="imageTwo">on the top picture that need to change opacity</param>
        /// <param name="opacity">the opacity value, a value between 0-1</param>
        /// <returns></returns>
        public static Image combineAndChangeOpacity(Image imageOne, Image imageTwo, float opacity)
        {
            imageTwo = ChangeOpacity(imageTwo, opacity);

            return CombineTwoPicture(imageOne, imageTwo);
        }





    }
}
