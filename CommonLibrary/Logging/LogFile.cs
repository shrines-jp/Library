using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Logging
{
	public class CLogFile
	{
		/// <summary>
		/// define variables.
		/// </summary>
		private const String className = "ClogFile";
		private const String Default_FileName_Format = "yyyyMMdd";
		private const int LoggingMessageType_MaxLength = 13;

		private String mLogFolder = @"C:\WsLog\Archlord2\";
		public readonly String mLogFolderBusiness = @"C:\WsLog\Archlord2\BusinessLogic\";
		public readonly String mLogFolderSmartClient = @"C:\WsLog\Archlord2\SmartClient\";
		public readonly String mLogFolderWebService = @"C:\WsLog\Archlord2\WebService\";

		private String mLogFileName = String.Empty;
		private String mPrefix = String.Empty;
		private String mSuffiex = String.Empty;
		private String mExtension = String.Empty;
		private String mFilenameFormat = String.Empty;

		private int mTabCount = 0;
		private bool mIsWritingToAFile = false;

		private FileManager fm = new FileManager();

		/// <summary>
		/// enum
		/// </summary>
		public enum loggingMessageType
		{
			Exception = 0,
			SqlException = 1,
			Warning = 2,
			Summary = 3,
			Informational = 4,
			Detail = 5,
			DebugLevel = 6
		}

		public enum loggingFolder
		{
			Root = 0,
			BusinessLogic = 1,
			SmartClient = 2,
			WebService = 3
		}

		/// <summary>
		/// CLogFile 생성자
		/// </summary>
		public CLogFile()
		{
			//
			// TODO: 생성자 논리를 여기에 추가합니다.
			//
			//CLogFile(String.Empty, String.Empty, String.Empty);
			SetLogFile(mLogFolder, String.Empty, String.Empty);
		}


		/// <summary>
		/// CLogFile
		/// </summary>
		/// <param name="fileName"></param>
		public CLogFile(String fileName)
		{
			SetLogFile(mLogFolder, fileName, String.Empty);
		}


		/// <summary>
		/// CLogFile
		/// </summary>
		/// <param name="logFolder"></param>
		/// <param name="fileName"></param>
		public CLogFile(String logFolder, String fileName)
		{
			SetLogFile(logFolder, fileName, String.Empty);
		}


		/// <summary>
		/// CLogFile
		/// </summary>
		/// <param name="logFolder"></param>
		/// <param name="fileName"></param>
		/// <param name="extension"></param>
		public CLogFile(String logFolder, String fileName, String extension)
		{
			SetLogFile(logFolder, fileName, extension);
		}


		/// <summary>
		/// CLogFile
		/// </summary>
		/// <param name="logFolder"></param>
		/// <param name="fileName"></param>
		public CLogFile(loggingFolder logFolder, String fileName)
		{
			SetLogFile(logFolder, fileName);
		}


		#region "Properties"
		/// <summary>
		/// LogFolder
		/// </summary>
		public string LogFolder
		{
			get { return this.mLogFolder; }
			set { mLogFolder = value; }
		}

		/// <summary>
		/// FullFileName
		/// </summary>
		public string FullFileName
		{
			get
			{
				if (mExtension.Length == 0)
				{
					return fm.CheckSlash(LogFolder) + mPrefix + mLogFileName + mSuffiex;
				}
				else
				{
					return fm.CheckSlash(LogFolder) + mPrefix + mLogFileName + mSuffiex + "." + mExtension;
				}
			}
		}

		/// <summary>
		/// TablCount
		/// </summary>
		public int TabCount
		{
			get { return this.mTabCount; }
			set { mTabCount = value; }
		}

		/// <summary>
		/// FileName
		/// </summary>
		public String FileName
		{
			get { return this.mLogFileName; }
			set { mLogFileName = value; }
		}

		/// <summary>
		/// Prefix - 접두사
		/// </summary>
		public String Prefix
		{
			get { return this.mPrefix; }
			set { mPrefix = value; }
		}


		/// <summary>
		/// Suffix - 접미사
		/// </summary>
		public String Suffix
		{
			get { return this.mSuffiex; }
			set { mSuffiex = value; }
		}


		/// <summary>
		/// Extension
		/// </summary>
		public String Extension
		{
			get { return this.mExtension; }
			set { mExtension = value; }
		}


		/// <summary>
		/// FileNameFormat
		/// </summary>
		public String FileNameFormat
		{
			get { return this.mFilenameFormat; }

			set
			{
				//NOTE -- DO NOT UPPER OR LOWER THIS VALUE -- AS THE dateTime formatting is case sensitive...
				mFilenameFormat = value;
			}
		}

		/// <summary>
		/// UseFileNameFormatDefaut
		/// </summary>
		public String UseFileNameFormatDefault
		{
			get { return Default_FileName_Format; }
		}

		#endregion


		#region "Private Method"

		/// <summary>
		/// pCreateFolder
		/// </summary>
		/// <param name="logFolderName"></param>
		private void pCreateFolder(String logFolderName)
		{
			try
			{
				if (fm.FolderExists(logFolderName))
				{
					//do nothing
				}
				else
				{
					try
					{
						//Create this folder...
						fm.CreateFolder(logFolderName);
					}
					catch (Exception ex)
					{
						throw new Exception(String.Format("Unable to create this folder: {0}", logFolderName), ex);
					}
				}
			}
			catch (Exception e)
			{
				throw new Exception(String.Format("Unable to create this folder: {0}", logFolderName), e);
				//new CException(className, String.Format("Can not Exists check for this folder: {0}", logFolderName), e);
			}
		}

		#endregion


		#region "public method"

		/// <summary>
		/// SetLogFile
		/// GMS에서 사용할 로그세팅
		/// </summary>
		/// <param name="logFolder"></param>
		/// <param name="fileName"></param>
		public void SetLogFile(loggingFolder logFolder, String fileName)
		{
			switch (logFolder)
			{
				case loggingFolder.Root:
					//mLogFolder
					break;
				case loggingFolder.BusinessLogic:
					mLogFolder = mLogFolderBusiness;
					break;
				case loggingFolder.SmartClient:
					mLogFolder = mLogFolderSmartClient;
					break;
				case loggingFolder.WebService:
					mLogFolder = mLogFolderWebService;
					break;
				default:
					break;
			}

			this.SetLogFile(mLogFolder, fileName, "TXT");
		}


		/// <summary>
		/// SetLogFile
		/// </summary>
		/// <param name="logFolder"></param>
		/// <param name="fileName"></param>
		/// <param name="extension"></param>
		public void SetLogFile(String logFolder, String fileName, String extension)
		{
			if (logFolder.Length == 0)
			{
				mLogFolder = fm.CheckSlash(Environment.CurrentDirectory);
			}
			else
			{
				mLogFolder = logFolder;
			}

			if (fileName.Length == 0)
			{
				mLogFileName = String.Empty;
				mFilenameFormat = Default_FileName_Format;
			}
			else
			{
				mLogFileName = fileName;
				mFilenameFormat = Default_FileName_Format;
			}

			if (extension.Length == 0)
			{
				mExtension = "TXT";
			}
			else
			{
				mExtension = extension;
			}

			try
			{
				pCreateFolder(mLogFolder);
			}
			catch (Exception)
			{
				throw;
			}
		}


		/// <summary>
		/// 탭증가
		/// </summary>
		public void IncrementTab()
		{
			mTabCount += 1;
		}


		/// <summary>
		/// 탭감소
		/// </summary>
		public void DecrementTab()
		{
			if (mTabCount > 0)
			{
				mTabCount -= 1;
			}
			else
			{
				mTabCount = 0;
			}
		}


		/// <summary>
		/// RefreshFileName 새포맷형식
		/// </summary>
		public void RefreshFileName()
		{
			if (mFilenameFormat.Length > 0)
			{
				if (mLogFileName.Contains(DateTime.Now.ToString(mFilenameFormat) + "_"))
				{
					this.FileName = DateTime.Now.ToString(mFilenameFormat) + "_" + mLogFileName.Replace(DateTime.Now.ToString(mFilenameFormat) + "_", String.Empty);
				}
				else
				{
					this.FileName = DateTime.Now.ToString(mFilenameFormat) + "_" + mLogFileName.Trim();
				}
			}
			else
			{
				this.FileName = DateTime.Now.ToString(mFilenameFormat);
			}

			if (String.IsNullOrEmpty(mExtension))
			{
				mExtension = "LOG";
			}
		}



		/// <summary>
		/// WriteLog
		/// </summary>
		/// <param name="messageLevelText"></param>
		/// <param name="message"></param>
		public void WriteLog(String messageLevelText, String message)
		{
			this.WriteLog(messageLevelText, String.Empty, String.Empty, message, mTabCount, false);
		}


		/// <summary>
		/// WriteLog
		/// </summary>
		/// <param name="messageLevelText"></param>
		/// <param name="message"></param>
		/// <param name="tabIndentationCount"></param>
		public void WriteLog(String messageLevelText, String message, int tabIndentationCount)
		{
			this.WriteLog(messageLevelText, String.Empty, String.Empty, message, tabIndentationCount, false);
		}


		/// <summary>
		/// WriteLog
		/// </summary>
		/// <param name="messageType"></param>
		/// <param name="message"></param>
		public void WriteLog(loggingMessageType messageType, String message)
		{
			this.WriteLog(messageType.ToString(), String.Empty, String.Empty, message, mTabCount, false);
		}

		/// <summary>
		/// WriteLog
		/// </summary>
		/// <param name="messageType"></param>
		/// <param name="message"></param>
		public void WriteLog(loggingMessageType messageType, String className, String methodName, String message)
		{
			this.WriteLog(messageType.ToString(), className, methodName, message, mTabCount, false);
		}


		/// <summary>
		/// WriteLog
		/// </summary>
		/// <param name="messageType"></param>
		/// <param name="message"></param>
		/// <param name="tabIndentationCount"></param>
		public void WriteLog(loggingMessageType messageType, String message, int tabIndentationCount)
		{
			this.WriteLog(messageType.ToString(), String.Empty, String.Empty, message, tabIndentationCount, false);
		}


		/// <summary>
		/// WriteLog
		/// </summary>
		/// <param name="messageType"></param>
		/// <param name="message"></param>
		/// <param name="tabIndentationCount"></param>
		/// <param name="bRefreshFileName"></param>
		public void WriteLog(loggingMessageType messageType, String message, int tabIndentationCount, bool bRefreshFileName)
		{
			this.WriteLog(messageType.ToString(), String.Empty, String.Empty, message, tabIndentationCount, bRefreshFileName);
		}



		/// <summary>
		/// WriteLog
		/// </summary>
		/// <param name="messageLevelText"></param>
		/// <param name="message"></param>
		/// <param name="tabIndentationCount"></param>
		/// <param name="bRefreshFileName"></param>
		public void WriteLog(String messageLevelText, String className, String methodName, String message, int tabIndentationCount, bool bRefreshFileName)
		{
			//'All writelog messages drop into this section -- so this is the overarching method call....
			//'This is where we determine if cutting a new log file is required -- ie., just about to write a new message 
			//'to a new file....

			if (mIsWritingToAFile == false)
			{
				if ((bRefreshFileName) || (mFilenameFormat.Length > 0))
				{
					//Update the log filename
					this.RefreshFileName();
				}
			}
			else
			{
				if ((bRefreshFileName) || mFilenameFormat.Length > 0)
				{
					this.RefreshFileName();
				}
			}

			try
			{
				if (mLogFileName.Length == 0)
				{
					mFilenameFormat = Default_FileName_Format;
				}
			}
			catch (Exception)
			{
				//throw new Exception(className + ": No Filename specified.");
				//new CException(className, "No Filename specified.", ex);
			}

			try
			{
				String logTime = String.Empty;
				StringBuilder sb = new StringBuilder();

				logTime = DateTime.Today.ToString("yyyyMMdd") + " " + DateTime.Now.ToString("HH:mm:ss");

				if (tabIndentationCount > 0)
				{
					for (int i = 0; i < tabIndentationCount; i++)
					{
						sb.Append("\t");
					}
				}

				sb.Append(message);

				//if Summary then put in new line...
				if (messageLevelText.ToString().Equals(loggingMessageType.Summary.ToString()))
				{
					fm.AppendToFile(this.FullFileName, "\r");
				}

				//pad out the message type text so that it is all aligned to the same width....

				if (!String.IsNullOrEmpty(className) && !String.IsNullOrEmpty(methodName))
				{
					fm.AppendToFile(this.FullFileName, String.Format("{0}  [ {1}] : [ {2}] :: [ {3}] >> {4}",
						logTime,
						messageLevelText.PadRight(LoggingMessageType_MaxLength),
						className.PadRight(LoggingMessageType_MaxLength),
						methodName.PadRight(LoggingMessageType_MaxLength),
						sb.ToString()));
				}
				else if (!String.IsNullOrEmpty(className) && String.IsNullOrEmpty(methodName))
				{
					fm.AppendToFile(this.FullFileName, String.Format("{0}  [ {1}] : [ {2}] >> {3}",
					logTime,
					messageLevelText.PadRight(LoggingMessageType_MaxLength),
					className.PadRight(LoggingMessageType_MaxLength),
					sb.ToString()));
				}
				else
				{
					fm.AppendToFile(this.FullFileName, String.Format("{0}  [ {1}] : {2}", logTime, messageLevelText.PadRight(LoggingMessageType_MaxLength), sb.ToString()));
				}


				//fm.AppendToFile(this.FullFileName, String.Format("{0}  [ {1}] : {2}", logTime, messageLevelText.PadRight(LoggingMessageType_MaxLength), sb.ToString()));

				//this flag is used to indicate that a file is in use and all subsequent messages should be collated in the same file
				//until a new start point is identifyed.
				//This is to ensure that a new file is not cut mid-stream -- when a date change would cause a refresh of the filename
				//and thus cut a new file -- this will help ensure all runs are complete -- before creating a new file...
				mIsWritingToAFile = true;

			}
			catch (Exception)
			{
				//Console.WriteLine(ex.Message);
				//new CException(className, "Can not AppendToFile - WriteLog()", ex2);
			}
		}

		#endregion
	}
}
