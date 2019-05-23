using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;

namespace CommonLibrary.Helper
{
	public class LDAPHelper
	{
		/// <summary>
		/// Initializes static members of the LDAPHelper class.
		/// </summary>
		static LDAPHelper()
		{
			//RootPath = @"LDAP://10.1.5.1";
			//RootPath = @"LDAP://218.234.76.12";		//2012.01.16 박정환 AD서버 변경.
			RootPath = @"LDAP://218.234.76.18";			//2013.01.11 박정환 AD서버 변경.
			Organization = @"웹젠";
		}

		/// <summary>
		/// Gets or sets the Path to the Active Directory server [Optional].
		/// </summary>
		/// <value>The Path to the Active Directory server.</value>
		/// <remarks>The value "LDAP:\\" will be used by default.</remarks>
		public static string RootPath { get; set; }

		/// <summary>
		/// Gets or sets the UserName to be used for authentication [Optional].
		/// </summary>
		/// <value>The UserName to be used for authentication [Optional].</value>
		/// <remarks>
		/// When the code is running on a remote machine, set this value to 
		/// impersonate a user of the domain that you're querying.
		/// </remarks>
		public static string UserName { get; set; }

		/// <summary>
		/// Gets or sets the PassWord to be used for authentication [Optional].
		/// </summary>
		/// <value>The PassWord to be used for authentication [Optional].</value>
		/// <remarks>
		/// When the code is running on a remote machine, set this value to 
		/// impersonate a user of the domain that you're querying.
		/// </remarks>
		public static string Password { get; set; }



		// 
		/// <summary>
		/// Gets or sets the Organization name to be used for OU=[Optional].
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// </remarks>
		public static string Organization { get; set; }

		/// <summary>
		/// Checks if a full object path is valid.
		/// </summary>
		/// <param name="fullObjectPath">The full object path.</param>
		/// <returns>True if the path is valid.</returns>
		/// <remarks>Impersonation is not possible here.</remarks>
		public static bool Exists(string fullObjectPath)
		{
			bool found = false;
			if (DirectoryEntry.Exists(fullObjectPath))
			{
				found = true;
			}

			return found;
		}


		/// <summary>
		/// Send Query
		/// </summary>
		/// <param name="query">query full text</param>
		/// <returns>SearchResultCollection</returns>
		public static SearchResultCollection SearchQuery(string query)
		{
			using (DirectorySearcher searcher = GetDirectorySearcher())
			{
				searcher.Filter = query;// "(&(ObjectClass=user)(sAMAccountName=" + userName.Substring(userName.IndexOf('\\') + 1) + "))";
				//searcher.PropertiesToLoad.AddRange(new string[] { "sAMAccountName" });
				return searcher.FindAll(); //  One();
			}
		}



		/// <summary>
		/// Verifies if a User exists.
		/// </summary>
		/// <param name="userName">The UserName to verify.</param>
		/// <returns>True if the UserName exists in Active Directory, false if not.</returns>
		public static bool UserExists(string userName)
		{
			using (DirectorySearcher searcher = GetDirectorySearcher())
			{
				searcher.Filter = "(&(ObjectClass=user)(sAMAccountName=" + userName.Substring(userName.IndexOf('\\') + 1) + "))";

				// for performance reasons only request needed properties
				searcher.PropertiesToLoad.AddRange(new string[] { "sAMAccountName" });

				SearchResult result = searcher.FindOne();

				return result != null;
			}
		}

		/// <summary>
		/// Verifies if a Group exists.
		/// </summary>
		/// <param name="groupName">The GroupName to verify.</param>
		/// <returns>True if the GroupName exists in Active Directory, false if not.</returns>
		public static bool GroupExists(string groupName)
		{
			using (DirectorySearcher searcher = GetDirectorySearcher())
			{
				searcher.Filter = "(&(objectClass=Group)(sAMAccountName=" + groupName + "))";

				// for performance reasons only request needed properties
				searcher.PropertiesToLoad.AddRange(new string[] { "sAMAccountName" });

				SearchResult result = searcher.FindOne();

				return result != null;
			}
		}

		private static string GetProperties(SearchResult item, string str)
		{
			string strHeader = "(" + str + ") ";
			var obj = item.Properties[str];
			if (obj == null) return strHeader + "NULL";
			if (obj.Count < 1) return strHeader + "null";
			if (obj[0] == null) return strHeader + "NuLL";
			return strHeader + obj[0].ToString();
		}


		/// <summary>
		/// Returns all stored UserIds.
		/// </summary>
		/// <returns>A list of all UserIds.</returns>
		public static List<string> GetAllUserNames()
		{
			DirectoryEntry de = GetDirectoryEntry();
			List<string> result = new List<string>();
			using (DirectorySearcher srch = new DirectorySearcher(de, "(objectClass=user)"))
			//	+ (string.IsNullOrEmpty(Organization) ? "" : ("(distinguishedname=*" + Organization + "*)")) + ")"))
			{
				SearchResultCollection results = srch.FindAll();

				foreach (SearchResult item in results)
				{
					string strOption = GetProperties(item, "distinguishedname");
					if (string.IsNullOrEmpty(strOption)) continue;
					if (false == strOption.ToUpper().Contains("OU=웹젠")) continue;
					//result.Add(item.Properties["sAMAccountName"][0].ToString());

					result.Add(GetProperties(item, "sAMAccountName"));

					/*

										result.Add(
											string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\t{17}\t{18}\t{19}\t{20}\t{21}\t{22}\t{23}\t{24}\t{25}\t{26}\t{27}\t{28}\t\r\n",

												GetProperties(item, "samaccounttype"),
												GetProperties(item, "lastlogon"),
												GetProperties(item, "dscorepropagationdata"),
												GetProperties(item, "objectsid"),
												GetProperties(item, "whencreated"),
												GetProperties(item, "badpasswordtime"),
												GetProperties(item, "accountexpires"),
												GetProperties(item, "iscriticalsystemobject"),
												GetProperties(item, "name"),
												GetProperties(item, "usnchanged"),
												GetProperties(item, "objectcategory"),
												GetProperties(item, "description"),
												GetProperties(item, "codepage"),
												GetProperties(item, "instancetype"),
												GetProperties(item, "countrycode"),
												GetProperties(item, "distinguishedname"),
												GetProperties(item, "cn"),
												GetProperties(item, "objectclass"),
												GetProperties(item, "logoncount"),
												GetProperties(item, "usncreated"),
												GetProperties(item, "useraccountcontrol"),
												GetProperties(item, "objectguid"),
												GetProperties(item, "primarygroupid"),
												GetProperties(item, "lastlogoff"),
												GetProperties(item, "samaccountname"),
												GetProperties(item, "badpwdcount"),
												GetProperties(item, "whenchanged"),
												GetProperties(item, "memberof"),
												GetProperties(item, "pwdlastset"),
												GetProperties(item, "adspath")
										));
					//*/

				}
			}

			return result;
		}


		/// <summary>
		/// Returns all stored GroupNames.
		/// </summary>
		/// <returns>A list of all GroupNames.</returns>
		public static List<string> GetAllGroupNames()
		{
			DirectoryEntry de = GetDirectoryEntry();
			List<string> result = new List<string>();
			using (DirectorySearcher srch = new DirectorySearcher(de, "(objectClass=Group)"))
			{
				SearchResultCollection results = srch.FindAll();

				foreach (SearchResult item in results)
				{
					string strOption = GetProperties(item, "distinguishedname");
					if (string.IsNullOrEmpty(strOption)) continue;
					if (false == strOption.ToUpper().Contains("OU=웹젠")) continue;

					result.Add(item.Properties["sAMAccountName"][0].ToString());
				}
			}

			return result;
		}

		/// <summary>
		/// Returns all UserIds in a Group.
		/// </summary>
		/// <param name="groupName">The Group.</param>
		/// <returns>A list of all UserIds in the Group.</returns>
		public static List<string> GetAllGroupMembers(string groupName)
		{
			List<string> result = new List<string>();
			DirectorySearcher searcher = GetDirectorySearcher();
			searcher.Filter = "(CN=" + groupName + ")";
			SearchResultCollection groups = searcher.FindAll();
			foreach (SearchResult group in groups)
			{
				ResultPropertyCollection props = group.Properties;
				foreach (object member in props["member"])
				{
					DirectoryEntry memberEntry = GetDirectoryEntry();
					memberEntry.Path = RootPath + @"/" + member;
					PropertyCollection userProps = memberEntry.Properties;
					object userName = userProps["sAMAccountName"].Value;
					if (null != userName)
					{
						result.Add(userName.ToString());
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Validates a UserId / Password combination.
		/// </summary>
		/// <param name="userName">The userid.</param>
		/// <param name="password">The password.</param>
		/// <returns>True if the user can be authenticated, False if not.</returns>
		public static bool IsValidUser(string userName, string password)
		{
			bool authenticated = false;
			try
			{
				DirectoryEntry entry = new DirectoryEntry(RootPath, userName, password);

				object nativeObject = entry.NativeObject;
				authenticated = true;
			}
			catch (Exception ex)
			{
				authenticated = false;
				// authenticated = ex.ToString(); 
				// not authenticated  
			}

			return authenticated;
		}

		public static bool IsValidUser(string path, string domain, string userName, string password)
		{
			//string domainAndUsername = string.IsNullOrEmpty(domain) ? userName : domain + @"\" + userName;

			//string rootPathAndDomain = string.IsNullOrEmpty(domain) ? RootPath : RootPath + @"\" + domain;
			//string pathAndIP = path + ip;
			string pathAndDamain = path + domain;

			DirectoryEntry entry = new DirectoryEntry(pathAndDamain, userName, password);

			try
			{
				object obj = entry.NativeObject;

				DirectorySearcher search = new DirectorySearcher(entry);

				search.Filter = "(SAMAccountName=" + userName + ")";
				search.PropertiesToLoad.Add("cn");
				SearchResult result = search.FindOne();

				if (null == result)
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				Exception exception = ex;
				return false;
			}

			return true;
		}

		/// <summary>
		/// Returns a properly configured DirectoryEntry.
		/// </summary>
		/// <returns>A properly configured DirectoryEntry.</returns>
		private static DirectoryEntry GetDirectoryEntry()
		{
			if (string.IsNullOrEmpty(UserName))
			{
				// No Impersonation
				return new DirectoryEntry(RootPath);
			}
			else
			{
				// Impersonation
				return new DirectoryEntry(RootPath, UserName, Password);
			}
		}

		/// <summary>
		/// Returns a properly configured DirectorySearcher.
		/// </summary>
		/// <returns>A properly configured DirectorySearcher.</returns>
		private static DirectorySearcher GetDirectorySearcher()
		{
			return new DirectorySearcher(GetDirectoryEntry());
		}
	}
}
