using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CommonLibrary.Logging
{
	public class FileManager
	{
		private const String className = "FileManager";
		object thisLock = new object();

		public FileManager()
		{
			//
			// TODO: 생성자 논리를 여기에 추가합니다.
			//
		}

		/// <summary>
		/// FolderExists
		/// </summary>
		/// <param name="sDirectoryName"></param>
		/// <returns></returns>
		public bool FolderExists(String sDirectoryName)
		{
			try
			{
				DirectoryInfo di = new DirectoryInfo(sDirectoryName);
				return di.Exists;
			}
			catch (System.Security.SecurityException se)
			{
				return true;
				throw new Exception(se.Message);
			}
			catch (Exception ex)
			{
				return false;
				throw new Exception(ex.Message);
			}
		}


		/// <summary>
		/// FileExists
		/// </summary>
		/// <param name="sFileName"></param>
		/// <returns></returns>
		public bool FileExists(String sFileName)
		{
			return File.Exists(sFileName);
		}


		/// <summary>
		/// CheckSlash
		/// </summary>
		/// <param name="sFolderName"></param>
		/// <returns></returns>
		public String CheckSlash(String sFolderName)
		{
			try
			{
				if (sFolderName.Substring(sFolderName.Length - 1, 1).Equals(@"\"))
				{
					return sFolderName;
				}
				else
				{
					return sFolderName.Insert(sFolderName.Length, @"\");
				}
			}
			catch (Exception)
			{
				return String.Empty;
			}
		}


		/// <summary>
		/// CreateFolder
		/// </summary>
		/// <param name="sFolderName"></param>
		public void CreateFolder(String sFolderName)
		{
			try
			{
				if (!FolderExists(sFolderName))
				{
					Directory.CreateDirectory(sFolderName);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(String.Format("{1}{2}There was a problem creating a folder. -- {0}", sFolderName, ex.Message, Environment.NewLine), ex);
				//new CException(className, String.Format("{1}{2}There was a problem creating a folder. -- {0}", sFolderName, ex.Message, Environment.NewLine), ex);
			}
		}


		/// <summary>
		/// AppendToFile
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="message"></param>
		public void AppendToFile(String fileName, String message)
		{
			this.AppendToFile(fileName, message, true);
		}


		/// <summary>
		/// AppendToFile
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="message"></param>
		/// <param name="append"></param>
		public void AppendToFile(String fileName, String message, bool append)
		{
			//StreamWriter sw = null;

			if (append)
			{
				if (!this.FileExists(fileName))
				{
					append = false;
				}
			}

			using (StreamWriter sw = new StreamWriter(fileName, append))
			{

				try
				{
					//sw = new StreamWriter(fileName, append);

					//lock (this)
					//{
					//    if (!sw.BaseStream.CanWrite)
					//    {
					//        throw new System.InvalidOperationException(" Logfile cannot be read-only");
					//    }
					//    else
					//    {
					//        sw.WriteLine(message);
					//        sw.Flush();
					//        sw.Close();
					//    }
					//}

					lock (thisLock)
					{
						if (!sw.BaseStream.CanWrite)
						{
							throw new System.InvalidOperationException(" Logfile cannot be read-only");
						}
						else
						{
							sw.WriteLine(message);
							sw.Flush();
							sw.Close();
						}
					}
				}
				catch (System.InvalidOperationException)
				{
					//sw.Close();
					//sw = null;
					//new CException(className, String.Format(" AppendToFile : FileName -- {0} \t Message -- {1}", fileName, ioe.Message, Environment.NewLine), ioe);
				}
				catch (Exception)
				{
					//sw.Close();
					//sw = null;
					//new CException(className, String.Format(" AppendToFile : Can not 'AppendToFile' file -- {0} \t Message -- {1}", fileName, ex.Message, Environment.NewLine), ex);
				}
				finally
				{
					if (sw != null)
					{
						sw.Close();
					}
					//sw의 관련된 리소스를 해제한다.
					sw.Dispose();
				}
			}
		}
	}
}
