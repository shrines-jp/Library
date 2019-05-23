namespace ActiveDirectoryHelper
{
	/*
	 * Developed 2004-2010 Zeta Software GmbH.
	 * http://www.zeta-test.com
	 * 
	 * Author: uwe.keim@gmail.com - http://twitter.com/UweKeim
	 */

	#region Using directives.
	// ----------------------------------------------------------------------
	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Class to configure an instance of the active directory class.
	/// </summary>
	public class ActiveDirectoryConfiguration
	{
		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Configures the LDAP connection string for accessing the LDAP
		/// server.
		/// <example><c>MyServer</c></example>
		/// </summary>
		/// <value>The LDAP server.</value>
		public string LdapServer { get; set; }

		/// <summary>
		/// Configures the LDAP connection base DN (distinguished name)
		/// string for accessing the LDAP server.
		/// <example><c>DC=duernau,DC=zeta-software,DC=de</c></example>
		/// </summary>
		/// <value>The LDAP base DN.</value>
		public string LdapBaseDN { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [do impersonate].
		/// </summary>
		/// <value><c>true</c> if [do impersonate]; otherwise, <c>false</c>.</value>
		public bool DoImpersonate { get; set; }

		/// <summary>
		/// Gets or sets the name of the impersonation user.
		/// </summary>
		/// <value>The name of the impersonation user.</value>
		public string ImpersonationUserName { get; set; }

		/// <summary>
		/// Gets or sets the impersonation password.
		/// </summary>
		/// <value>The impersonation password.</value>
		public string ImpersonationPassword { get; set; }

		/// <summary>
		/// Gets or sets the name of the impersonation domain.
		/// </summary>
		/// <value>The name of the impersonation domain.</value>
		public string ImpersonationDomainName { get; set; }

		/// <summary>
		/// Gets or sets the dir entry user name prefix.
		/// </summary>
		/// <value>The dir entry user name prefix.</value>
		public string DirEntryUserNamePrefix { get; set; }

		/// <summary>
		/// Gets or sets the dir entry user name suffix.
		/// </summary>
		/// <value>The dir entry user name suffix.</value>
		public string DirEntryUserNameSuffix { get; set; }

		/// <summary>
		/// Gets or sets the name of the LDAP user.
		/// </summary>
		/// <value>The name of the LDAP user.</value>
		public string LdapUserName { get; set; }

		/// <summary>
		/// Gets or sets the LDAP password.
		/// </summary>
		/// <value>The LDAP password.</value>
		public string LdapPassword { get; set; }

		/// <summary>
		/// To get over the 1000-result-entries limit,
		/// set this to TRUE. Makes the processing slower, though.
		/// </summary>
		/// <value><c>true</c> if [use sliced queries]; otherwise, <c>false</c>.</value>
		public bool UseSlicedQueries { get; set; }

		/// <summary>
		/// Enables caching for reading items.
		/// </summary>
		/// <value><c>true</c> if [allow for caching]; otherwise, <c>false</c>.</value>
		public bool AllowForCaching
		{
			get
			{
				return _allowForCaching;
			}
			set
			{
				_allowForCaching = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private bool _allowForCaching = true;

		public ActiveDirectoryConfiguration()
		{
			UseSlicedQueries = false;
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}