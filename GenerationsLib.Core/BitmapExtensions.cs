﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;

namespace GenerationsLib.Core
{
    public static class BitmapExtensions
    {
        /// <summary>
        /// Clones an image object to free it from any backing resources.
        /// Code taken from http://stackoverflow.com/a/3661892/ with some extra fixes.
        /// </summary>
        /// <param name="sourceImage">The image to clone</param>
        /// <returns>The cloned image</returns>
        public static Bitmap CloneImage(Bitmap sourceImage)
        {
            Rectangle rect = new Rectangle(0, 0, sourceImage.Width, sourceImage.Height);
            Bitmap targetImage = new Bitmap(rect.Width, rect.Height, sourceImage.PixelFormat);
            targetImage.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);
            BitmapData sourceData = sourceImage.LockBits(rect, ImageLockMode.ReadOnly, sourceImage.PixelFormat);
            BitmapData targetData = targetImage.LockBits(rect, ImageLockMode.WriteOnly, targetImage.PixelFormat);
            Int32 actualDataWidth = ((Image.GetPixelFormatSize(sourceImage.PixelFormat) * rect.Width) + 7) / 8;
            Int32 h = sourceImage.Height;
            Int32 origStride = sourceData.Stride;
            Boolean isFlipped = origStride < 0;
            origStride = Math.Abs(origStride); // Fix for negative stride in BMP format.
            Int32 targetStride = targetData.Stride;
            Byte[] imageData = new Byte[actualDataWidth];
            IntPtr sourcePos = sourceData.Scan0;
            IntPtr destPos = targetData.Scan0;
            // Copy line by line, skipping by stride but copying actual data width
            for (Int32 y = 0; y < h; y++)
            {
                Marshal.Copy(sourcePos, imageData, 0, actualDataWidth);
                Marshal.Copy(imageData, 0, destPos, actualDataWidth);
                sourcePos = new IntPtr(sourcePos.ToInt64() + origStride);
                destPos = new IntPtr(destPos.ToInt64() + targetStride);
            }
            targetImage.UnlockBits(targetData);
            sourceImage.UnlockBits(sourceData);
            // Fix for negative stride on BMP format.
            if (isFlipped)
                targetImage.RotateFlip(RotateFlipType.Rotate180FlipX);
            // For indexed images, restore the palette. This is not linking to a referenced
            // object in the original image; the getter of Palette creates a new object when called.
            if ((sourceImage.PixelFormat & PixelFormat.Indexed) != 0)
                targetImage.Palette = sourceImage.Palette;
            // Restore DPI settings
            targetImage.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);
            return targetImage;
        }
        public static Bitmap LoadBitmapAlt(String filepath)
        {
            var bytes = File.ReadAllBytes(filepath);
            var ms = new MemoryStream(bytes);
            return (System.Drawing.Bitmap)System.Drawing.Image.FromStream(ms);
        }
        public static System.Drawing.Bitmap LoadBitmap(string path)
        {
            using (Bitmap sourceImage = new Bitmap(path))
            {
                return CloneImage(sourceImage);
            }
        }
    }
}
