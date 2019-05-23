using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CommonLibrary.Extension
{
	public static class BitMapExtension
	{
		public static Bitmap CopyDataToBitmap(byte[] data)
		{
			///이미지의 너비와 높이
			//int width = Convert.ToInt32(App.Current.MainWindow.Width);
			//int height = Convert.ToInt32(App.Current.MainWindow.Height);

			int width = 1024;
			int height = 768;

			Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);

			BitmapData bmpData = bmp.LockBits(
				new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);

			Marshal.Copy(data, 0, bmpData.Scan0, data.Length);

			bmp.UnlockBits(bmpData);

			return bmp;
		}


		public static byte[] imageToByteArray(System.Drawing.Image imageIn)
		{
			MemoryStream ms = new MemoryStream();
			imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
			return ms.ToArray();
		}

		public static Image byteArrayToImage(byte[] byteArrayIn)
		{
			MemoryStream ms = new MemoryStream(byteArrayIn);
			Image returnImage = Image.FromStream(ms);
			return returnImage;
		}
	}
}
