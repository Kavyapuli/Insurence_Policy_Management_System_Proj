using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace UILayer.Models
{
    public static class LoginCaptcha
    {
        public static Image GenerateCaptchaImage(string text)
        {
            // Create an image with specified text
            var image = new Bitmap(150, 50);
            var graphics = Graphics.FromImage(image);

            // Set background color and font
            graphics.Clear(Color.White);
            var font = new Font("Arial", 15, FontStyle.Italic|FontStyle.Strikeout);
            var brush = new SolidBrush(Color.Black);

            // Draw the text on the image
            graphics.DrawString(text, font, brush, 10, 10);

            return image;
        }

        public static byte[] ImageToByteArray(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}