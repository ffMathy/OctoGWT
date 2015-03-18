using OpenQA.Selenium;
using System.Drawing;
using System.IO;

namespace OctoGWT.Extensions
{
    internal static class ScreenshotExtensions
    {
        public static Bitmap ConvertToBitmap(this Screenshot screenshot)
        {
            var byteArray = screenshot.AsByteArray;
            using (var stream = new MemoryStream(byteArray))
            {
                var bitmap = new Bitmap(stream);
                return bitmap;
            }
        }
    }
}
