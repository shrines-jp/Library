using System;
using System.Text;
using System.Security.Cryptography;

namespace CommonLibrary.Security
{
	/// <summary>
	/// DACOM에서 제공하는 암호화 처리 Class - 수정하지 말것
	/// </summary>
	public class HexBin
	{
		private static int BASELENGTH = 0xff;
		private static int LOOKUPLENGTH = 0x10;
		private static byte[] hexNumberTable = new byte[HexBin.BASELENGTH];
		private static byte[] lookUpHexAlphabet = new byte[HexBin.LOOKUPLENGTH];

		static HexBin()
		{
			byte[] b = System.Text.Encoding.ASCII.GetBytes("-1");

			for (int i = 0; i < HexBin.BASELENGTH; i++)
			{
				HexBin.hexNumberTable[i] = (byte) Convert.ToSByte("-1");
			}
			for (int i = 0x39; i >= 0x30; i--)
			{
				HexBin.hexNumberTable[i] = (byte) (i - 0x30);
			}
			for (int i = 70; i >= 0x41; i--)
			{
				HexBin.hexNumberTable[i] = (byte) ((i - 0x41) + 10);
			}
			for (int i = 0x66; i >= 0x61; i--)
			{
				HexBin.hexNumberTable[i] = (byte) ((i - 0x61) + 10);
			}
			for (int i = 0; i < 10; i++)
			{
				HexBin.lookUpHexAlphabet[i] = (byte) (0x30 + i);
			}
			for (int i = 10; i <= 15; i++)
			{
				HexBin.lookUpHexAlphabet[i] = (byte) ((0x41 + i) - 10);
			}
		}

		#region Decode
		public static byte[] Decode(byte[] binaryData)
		{
			if (binaryData == null) return null;

			int len = binaryData.Length;
			if ((len % 2) != 0) return null;

			int bufferlen = len / 2;
			byte[] beffer = new byte[bufferlen];

			for (int i = 0; i < bufferlen; i++)
			{
				if (!HexBin.IsHex(binaryData[i * 2]) || !HexBin.IsHex(binaryData[(i * 2) + 1]))
				{
					return null;
				}

				beffer[i] = (byte) ((HexBin.hexNumberTable[binaryData[i * 2]] << 4) | HexBin.hexNumberTable[binaryData[(i * 2) + 1]]);

			}

			return beffer;

		}
		#endregion

		private static bool IsHex(byte octect)
		{
			return (HexBin.hexNumberTable[octect] != ((byte) Convert.ToSByte("-1")));
		}

	}
	
}
