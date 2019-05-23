using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace CommonLibrary.Security
{
	/// <summary>
	/// EncryptionDES에 대한 요약 설명입니다.
	/// </summary>
	public class EncryptionDES
	{
		private string strErrorMsg;
		private DecryptTransformer transformer = null;

		public EncryptionDES()
		{
			this.transformer = new DecryptTransformer();

		}

		public string Decrypt(string inputByte)
		{
			if (SetKeyAndVec.IsNullStr(inputByte))
			{
				this.strErrorMsg = "Null Input";
				return "";
			}
			try
			{
				byte[] inputBytes = Convert.FromBase64String(inputByte);

				MemoryStream memStream = new MemoryStream();  
				this.transformer.IV = SetKeyAndVec.GetVec;
				ICryptoTransform transform = this.transformer.GetCryptoServiceProvider(SetKeyAndVec.GetKey);

				CryptoStream cryptStream = new CryptoStream(memStream,transform,CryptoStreamMode.Write);

				UTF8Encoding UTF8Encoder = new UTF8Encoding();

				cryptStream.Write(inputBytes,0,inputBytes.Length);
				cryptStream.FlushFinalBlock();

				byte[] output = memStream.ToArray();
				cryptStream.Close();

				return UTF8Encoder.GetString(output);
			}
			catch (Exception exception1)
			{
				this.strErrorMsg = exception1.Message;
				return "";
			}
		}
	}

	public class DecryptTransformer
	{
		private byte[] initVec;
 
		public ICryptoTransform GetCryptoServiceProvider(byte[] bytesKey)
		{
			DESCryptoServiceProvider provider1 = new DESCryptoServiceProvider();
			provider1.Mode = CipherMode.CBC;
			provider1.Key = bytesKey;
			provider1.IV = this.initVec;
			return provider1.CreateDecryptor();
		}

		public byte[] IV
		{
			set
			{
				this.initVec = value;
			}
		}
 
	}

	public class SetKeyAndVec
	{
		public SetKeyAndVec(){}
		public static bool IsNullCheck(object objData)
		{
			if (objData == null)
			{
				return true;
			}
			return false;
		}

		public static bool IsNullStr(string str)
		{
			str = str.Trim();
			if ((str == null) || (str == string.Empty))
			{
				return true;
			}
			return false;

		}

		// Properties
		public static byte[] GetKey {
			get
			{
				return Encoding.UTF8.GetBytes("WEBZEN99");
			}

		}
		public static byte[] GetVec { 
			get
			{
				return Encoding.UTF8.GetBytes("WEBDEV99");
			}

		}
		
	}
}
