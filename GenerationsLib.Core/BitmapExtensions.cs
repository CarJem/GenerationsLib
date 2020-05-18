using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GenerationsLib.Core
{
    public static class BitmapExtensions
    {
        public static System.Drawing.Bitmap LoadBitmap(string filepath)
        {
            var bytes = File.ReadAllBytes(filepath);
            var ms = new MemoryStream(bytes);
            return (System.Drawing.Bitmap)System.Drawing.Image.FromStream(ms);
        }
    }
}
