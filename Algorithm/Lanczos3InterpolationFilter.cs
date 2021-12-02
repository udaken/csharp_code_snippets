using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace InterpolationSample
{
    public class Lanczos3InterpolationFilter
    {
        private class BitmapDataLock : IDisposable
        {
            private Bitmap bitmap;
            private System.Drawing.Imaging.BitmapData bitmapData;
            public BitmapDataLock(Bitmap bitmap, Rectangle rect, System.Drawing.Imaging.ImageLockMode flags, System.Drawing.Imaging.PixelFormat format)
            {
                if (bitmap == null)
                    throw new ArgumentNullException();
                this.bitmap = bitmap;
                this.bitmapData = bitmap.LockBits(rect, flags, format);
            }
            public BitmapDataLock(Bitmap bitmap, Rectangle rect, System.Drawing.Imaging.ImageLockMode flags, System.Drawing.Imaging.PixelFormat format, System.Drawing.Imaging.BitmapData bitmapData)
            {
                if (bitmap == null)
                    throw new ArgumentNullException();
                this.bitmap = bitmap;
                this.bitmapData = bitmap.LockBits(rect, flags, format, bitmapData);
            }
            void IDisposable.Dispose()
            {
                if (bitmap == null)
                    throw new ObjectDisposedException("bitmap");
                bitmap.UnlockBits(bitmapData);
                bitmapData = null;
                bitmap = null;
            }
            public System.Drawing.Imaging.BitmapData BitmapData
            {
                get { return bitmapData; }
            }

            public int Height
            {
                get { return bitmapData.Height; }
                set { bitmapData.Height = value; }
            }
            public System.Drawing.Imaging.PixelFormat PixelFormat
            {
                get { return bitmapData.PixelFormat; }
                set { bitmapData.PixelFormat = value; }
            }
            public IntPtr Scan0
            {
                get { return bitmapData.Scan0; }
                set { bitmapData.Scan0 = value; }
            }
            public int Stride
            {
                get { return bitmapData.Stride; }
                set { bitmapData.Stride = value; }
            }
            public int Width
            {
                get { return bitmapData.Width; }
                set { bitmapData.Width = value; }
            }
        }
        public unsafe Bitmap Resample(Bitmap source, int width, int height)
        {
            using(var srcBitmapData = new BitmapDataLock(source, new Rectangle(Point.Empty, source.Size), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
            {
                byte* scan0 = (byte*)srcBitmapData.Scan0;
                var srcStride = srcBitmapData.Stride;
                var srcWidth = srcBitmapData.Width;
                var srcHeight = srcBitmapData.Height;
                var destBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using(var destBitmapData = new BitmapDataLock(destBitmap, new Rectangle(Point.Empty, destBitmap.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
                {
                    var destStride = destBitmapData.Stride;
                    var destScan0 = (byte*)destBitmapData.Scan0;

                    for (int y = 0; y < srcHeight; y++)
                    {
                        for (int x = 0; x < srcWidth; x++)
                        {
                            var a = (byte)255;
                            var b = (byte)(scan0[srcStride * y + x * 3 + 0] / 2);
                            var g = (byte)(scan0[srcStride * y + x * 3 + 1] / 2);
                            var r = (byte)(scan0[srcStride * y + x * 3 + 2] / 2);

                            destScan0[destStride * y + x * 4 + 0] = b; //b
                            destScan0[destStride * y + x * 4 + 1] = g; //g
                            destScan0[destStride * y + x * 4 + 2] = r; //r
                            destScan0[destStride * y + x * 4 + 3] = a;
                        }
                    }
                    return destBitmap;
                }
            }
        }
    }
}