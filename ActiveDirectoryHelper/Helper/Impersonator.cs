namespace ActiveDirectoryHelper.Helper
{
	/*
	 * Developed 2004-2010 Zeta Software GmbH.
	 * http://www.zeta-test.com
	 * 
	 * Author: uwe.keim@gmail.com - http://twitter.com/UweKeim
	 */

	#region Using directives.
	// ----------------------------------------------------------------------
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Runtime.InteropServices;
	using System.Security;
	using System.Security.Principal;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Impersonation of a user. Allows to execute code under another
	/// user context.
	/// The account that instantiates the Impersonator class
	/// needs to have the 'Act as part of operating system' privilege set.
	/// </summary>
	/// <remarks>
	/// This class is based on the information in the Microsoft knowledge base
	/// article http://support.microsoft.com/default.aspx?scid=kb;en-us;Q306158
	/// Encapsulate an instance into a using-directive like e.g.:
	/// ...
	/// using ( new Impersonator( "myUsername", "myDomainname", "myPassword" ) )
	/// {
	/// ...
	/// [code that executes under the new context]
	/// ...
	/// }
	/// ...
	/// </remarks>
	public class Impersonator :
		IDisposable
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Initializes a new instance of the <see cref="Impersonator"/> class.
		/// </summary>
		public Impersonator()
		{
		}

		/// <summary>
		/// Constructor. Starts the impersonation with the given credentials.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		public Impersonator(
			string userName,
			string domainName,
			string password,
			ProfileBehaviour profileBehaviour )
		{
			impersonateValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive,
				profileBehaviour );
		}

		/// <summary>
		/// Constructor. Starts the impersonation with the given credentials.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		public Impersonator(
			string userName,
			string domainName,
			string password,
			LoginType loginType,
			ProfileBehaviour profileBehaviour )
		{
			impersonateValidUser(
				userName,
				domainName,
				password,
				loginType,
				profileBehaviour );
		}

		/// <summary>
		/// Constructor. Starts the impersonation with the given credentials.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		public Impersonator(
			string userName,
			string domainName,
			SecureString password,
			ProfileBehaviour profileBehaviour )
		{
			impersonateValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive,
				profileBehaviour );
		}

		/// <summary>
		/// Constructor. Starts the impersonation with the given credentials.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		public Impersonator(
			string userName,
			string domainName,
			SecureString password,
			LoginType loginType,
			ProfileBehaviour profileBehaviour )
		{
			impersonateValidUser(
				userName,
				domainName,
				password,
				loginType,
				profileBehaviour );
		}

		/// <summary>
		/// Constructor. Starts the impersonation with the given credentials.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		public Impersonator(
			string userName,
			string domainName,
			string password )
		{
			impersonateValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive,
				ProfileBehaviour.DontLoad );
		}

		/// <summary>
		/// Constructor. Starts the impersonation with the given credentials.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		public Impersonator(
			string userName,
			string domainName,
			string password,
			LoginType loginType )
		{
			impersonateValidUser(
				userName,
				domainName,
				password,
				loginType,
				ProfileBehaviour.DontLoad );
		}

		/// <summary>
		/// Constructor. Starts the impersonation with the given credentials.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		public Impersonator(
			string userName,
			string domainName,
			SecureString password )
		{
			impersonateValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive,
				ProfileBehaviour.DontLoad );
		}

		/// <summary>
		/// Constructor. Starts the impersonation with the given credentials.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		public Impersonator(
			string userName,
			string domainName,
			SecureString password,
			LoginType loginType )
		{
			impersonateValidUser(
				userName,
				domainName,
				password,
				loginType,
				ProfileBehaviour.DontLoad );
		}

		/// <summary>
		/// Starts the impersonation with the given credentials.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		public void Impersonate(
			string userName,
			string domainName,
			string password,
			ProfileBehaviour profileBehaviour )
		{
			impersonateValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive,
				profileBehaviour );
		}

		/// <summary>
		/// Starts the impersonation with the given credentials.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		public void Impersonate(
			string userName,
			string domainName,
			string password,
			LoginType loginType,
			ProfileBehaviour profileBehaviour )
		{
			impersonateValidUser(
				userName,
				domainName,
				password,
				loginType,
				profileBehaviour );
		}

		/// <summary>
		/// Starts the impersonation with the given credentials.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		public void Impersonate(
			string userName,
			string domainName,
			SecureString password,
			ProfileBehaviour profileBehaviour )
		{
			impersonateValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive,
				profileBehaviour );
		}

		/// <summary>
		/// Starts the impersonation with the given credentials.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		public void Impersonate(
			string userName,
			string domainName,
			SecureString password,
			LoginType loginType,
			ProfileBehaviour profileBehaviour )
		{
			impersonateValidUser(
				userName,
				domainName,
				password,
				loginType,
				profileBehaviour );
		}

		/// <summary>
		/// Starts the impersonation with the given credentials.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		public void Impersonate(
			string userName,
			string domainName,
			string password )
		{
			impersonateValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive,
				ProfileBehaviour.DontLoad );
		}

		/// <summary>
		/// Starts the impersonation with the given credentials.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		public void Impersonate(
			string userName,
			string domainName,
			string password,
			LoginType loginType )
		{
			impersonateValidUser(
				userName,
				domainName,
				password,
				loginType,
				ProfileBehaviour.DontLoad );
		}

		/// <summary>
		/// Starts the impersonation with the given credentials.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		public void Impersonate(
			string userName,
			string domainName,
			SecureString password )
		{
			impersonateValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive,
				ProfileBehaviour.DontLoad );
		}

		/// <summary>
		/// Starts the impersonation with the given credentials.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		public void Impersonate(
			string userName,
			string domainName,
			SecureString password,
			LoginType loginType )
		{
			impersonateValidUser(
				userName,
				domainName,
				password,
				loginType,
				ProfileBehaviour.DontLoad );
		}

		/// <summary>
		/// Undoes the impersonation. Safe to call even if not yet
		/// impersonized.
		/// </summary>
		public void Undo()
		{
			undoImpersonation();
		}

		// ------------------------------------------------------------------
		#endregion

		#region IDisposable member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Performs application-defined tasks associated with freeing,
		/// releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			undoImpersonation();
			GC.SuppressFinalize( this );
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="Impersonator"/> is reclaimed by garbage collection.
		/// </summary>
		~Impersonator()
		{
			undoImpersonation();
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public static methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Static method to check whether user can log in.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		public static bool CanLogIn(
			string userName,
			string domainName,
			string password )
		{
			return canLogInValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive );
		}

		/// <summary>
		/// Static method to check whether user can log in.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		public static bool CanLogIn(
			string userName,
			string domainName,
			string password,
			LoginType loginType )
		{
			return canLogInValidUser(
				userName,
				domainName,
				password,
				loginType );
		}

		/// <summary>
		/// Static method to check whether user can log in.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		public static bool CanLogIn(
			string userName,
			string domainName,
			SecureString password )
		{
			return canLogInValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive );
		}

		/// <summary>
		/// Static method to check whether user can log in.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		public static bool CanLogIn(
			string userName,
			string domainName,
			SecureString password,
			LoginType loginType )
		{
			return canLogInValidUser(
				userName,
				domainName,
				password,
				loginType );
		}

		/// <summary>
		/// Static method to check whether user can log in.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="exception">The exception.</param>
		/// <returns>
		/// 	<c>true</c> if this instance [can log in] the specified user name; otherwise, <c>false</c>.
		/// </returns>
		public static bool CanLogIn(
			string userName,
			string domainName,
			string password,
			out Exception exception )
		{
			return canLogInValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive,
				out exception );
		}

		/// <summary>
		/// Static method to check whether user can log in.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		/// <param name="exception">The exception.</param>
		/// <returns>
		/// 	<c>true</c> if this instance [can log in] the specified user name; otherwise, <c>false</c>.
		/// </returns>
		public static bool CanLogIn(
			string userName,
			string domainName,
			string password,
			LoginType loginType,
			out Exception exception )
		{
			return canLogInValidUser(
				userName,
				domainName,
				password,
				loginType,
				out exception );
		}

		/// <summary>
		/// Static method to check whether user can log in.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="exception">The exception.</param>
		/// <returns>
		/// 	<c>true</c> if this instance [can log in] the specified user name; otherwise, <c>false</c>.
		/// </returns>
		public static bool CanLogIn(
			string userName,
			string domainName,
			SecureString password,
			out Exception exception )
		{
			return canLogInValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive,
				out exception );
		}

		/// <summary>
		/// Static method to check whether user can log in.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		/// <param name="exception">The exception.</param>
		/// <returns>
		/// 	<c>true</c> if this instance [can log in] the specified user name; otherwise, <c>false</c>.
		/// </returns>
		public static bool CanLogIn(
			string userName,
			string domainName,
			SecureString password,
			LoginType loginType,
			out Exception exception )
		{
			return canLogInValidUser(
				userName,
				domainName,
				password,
				loginType,
				out exception );
		}

		/// <summary>
		/// Static method to check whether user can impersonate.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		public static bool CanImpersonate(
			string userName,
			string domainName,
			string password,
			ProfileBehaviour profileBehaviour )
		{
			return canImpersonateValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive,
				profileBehaviour );
		}

		/// <summary>
		/// Static method to check whether user can impersonate.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		public static bool CanImpersonate(
			string userName,
			string domainName,
			string password,
			LoginType loginType,
			ProfileBehaviour profileBehaviour )
		{
			return canImpersonateValidUser(
				userName,
				domainName,
				password,
				loginType,
				profileBehaviour );
		}

		/// <summary>
		/// Static method to check whether user can impersonate.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		public static bool CanImpersonate(
			string userName,
			string domainName,
			SecureString password,
			ProfileBehaviour profileBehaviour )
		{
			return canImpersonateValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive,
				profileBehaviour );
		}

		/// <summary>
		/// Static method to check whether user can impersonate.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		public static bool CanImpersonate(
			string userName,
			string domainName,
			SecureString password,
			LoginType loginType,
			ProfileBehaviour profileBehaviour )
		{
			return canImpersonateValidUser(
				userName,
				domainName,
				password,
				loginType,
				profileBehaviour );
		}

		/// <summary>
		/// Static method to check whether user can impersonate.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		public static bool CanImpersonate(
			string userName,
			string domainName,
			string password )
		{
			return canImpersonateValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive,
				ProfileBehaviour.DontLoad );
		}

		/// <summary>
		/// Static method to check whether user can impersonate.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		public static bool CanImpersonate(
			string userName,
			string domainName,
			string password,
			LoginType loginType )
		{
			return canImpersonateValidUser(
				userName,
				domainName,
				password,
				loginType,
				ProfileBehaviour.DontLoad );
		}

		/// <summary>
		/// Static method to check whether user can impersonate.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		public static bool CanImpersonate(
			string userName,
			string domainName,
			SecureString password )
		{
			return canImpersonateValidUser(
				userName,
				domainName,
				password,
				LoginType.Interactive,
				ProfileBehaviour.DontLoad );
		}

		/// <summary>
		/// Static method to check whether user can impersonate.
		/// The account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">The login type.</param>
		public static bool CanImpersonate(
			string userName,
			string domainName,
			SecureString password,
			LoginType loginType )
		{
			return canImpersonateValidUser(
				userName,
				domainName,
				password,
				loginType,
				ProfileBehaviour.DontLoad );
		}

		// ------------------------------------------------------------------
		#endregion

		#region P/Invoke.
		// ------------------------------------------------------------------

		/// <summary>
		/// Logs the user on.
		/// </summary>
		/// <param name="lpszUserName">Name of the LPSZ user.</param>
		/// <param name="lpszDomain">The LPSZ domain.</param>
		/// <param name="lpszPassword">The LPSZ password.</param>
		/// <param name="dwLogonType">Type of the dw logon.</param>
		/// <param name="dwLogonProvider">The dw logon provider.</param>
		/// <param name="phToken">The ph token.</param>
		/// <returns></returns>
		[DllImport( @"advapi32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		private static extern int LogonUser(
			string lpszUserName,
			string lpszDomain,
			string lpszPassword,
			int dwLogonType,
			int dwLogonProvider,
			ref IntPtr phToken );

		/// <summary>
		/// Logons the user2.
		/// </summary>
		/// <param name="lpszUserName">Name of the LPSZ user.</param>
		/// <param name="lpszDomain">The LPSZ domain.</param>
		/// <param name="Password">The password.</param>
		/// <param name="dwLogonType">Type of the dw logon.</param>
		/// <param name="dwLogonProvider">The dw logon provider.</param>
		/// <param name="phToken">The ph token.</param>
		/// <returns></returns>
		[DllImport( @"advapi32.dll", EntryPoint = @"LogonUser", CharSet = CharSet.Unicode, SetLastError = true )]
		private static extern int LogonUser2(
			string lpszUserName,
			string lpszDomain,
			IntPtr Password,
			int dwLogonType,
			int dwLogonProvider,
			ref IntPtr phToken );

		/// <summary>
		/// Duplicates the token.
		/// </summary>
		/// <param name="hToken">The h token.</param>
		/// <param name="impersonationLevel">The impersonation level.</param>
		/// <param name="hNewToken">The h new token.</param>
		/// <returns></returns>
		[DllImport( @"advapi32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		private static extern int DuplicateToken(
			IntPtr hToken,
			int impersonationLevel,
			ref IntPtr hNewToken );

		/// <summary>
		/// Reverts to self.
		/// </summary>
		/// <returns></returns>
		[DllImport( @"advapi32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		private static extern bool RevertToSelf();

		/// <summary>
		/// Closes the handle.
		/// </summary>
		/// <param name="handle">The handle.</param>
		/// <returns></returns>
		[DllImport( @"kernel32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		private static extern bool CloseHandle(
			IntPtr handle );

		/// <summary>
		/// 
		/// </summary>
		private const int LOGON32_PROVIDER_DEFAULT = 0;

		/// <summary>
		/// Loads the user profile.
		/// </summary>
		/// <param name="hToken">The h token.</param>
		/// <param name="lpProfileInfo">The lp profile info.</param>
		/// <returns></returns>
		[DllImport( @"userenv.dll", SetLastError = true, CharSet = CharSet.Auto )]
		private static extern bool LoadUserProfile(
			IntPtr hToken,
			ref PROFILEINFO lpProfileInfo );

		/// <summary>
		/// Unloads the user profile.
		/// </summary>
		/// <param name="hToken">The h token.</param>
		/// <param name="hProfile">The h profile.</param>
		/// <returns></returns>
		[DllImport( @"userenv.dll", SetLastError = true, CharSet = CharSet.Auto )]
		private static extern bool UnloadUserProfile(
			IntPtr hToken,
			IntPtr hProfile );

		/// <summary>
		/// Profile information structure.
		/// </summary>
		[StructLayout( LayoutKind.Sequential )]
		private struct PROFILEINFO
		{
			public int dwSize;
			public int dwFlags;
			[MarshalAs( UnmanagedType.LPTStr )]
			public String lpUserName;
			[MarshalAs( UnmanagedType.LPTStr )]
			public String lpProfilePath;
			[MarshalAs( UnmanagedType.LPTStr )]
			public String lpDefaultPath;
			[MarshalAs( UnmanagedType.LPTStr )]
			public String lpServerName;
			[MarshalAs( UnmanagedType.LPTStr )]
			public String lpPolicyPath;
			public IntPtr hProfile;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Does the actual impersonation.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">Type of the login.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		private void impersonateValidUser(
			string userName,
			string domainName,
			string password,
			LoginType loginType,
			ProfileBehaviour profileBehaviour )
		{
			Exception exception;

			impersonateValidUser(
				userName,
				domainName,
				password,
				loginType,
				profileBehaviour,
				out exception );

			if ( exception != null )
			{
				throw exception;
			}
		}

		/// <summary>
		/// Does the actual check for impersonation.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">Type of the login.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		private static bool canImpersonateValidUser(
			string userName,
			string domainName,
			string password,
			LoginType loginType,
			ProfileBehaviour profileBehaviour )
		{
			using ( var impersonator = new Impersonator() )
			{
				Exception exception;

				impersonator.impersonateValidUser(
					userName,
					domainName,
					password,
					loginType,
					profileBehaviour,
					out exception );

				return exception == null;
			}
		}

		/// <summary>
		/// Does the actual impersonation.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">Type of the login.</param>
		/// <returns>
		/// 	<c>true</c> if this instance can log in the specified user name; otherwise, <c>false</c>.
		/// </returns>
		private static bool canLogInValidUser(
			string userName,
			string domainName,
			string password,
			LoginType loginType )
		{
			Exception exception;

			return canLogInValidUser(
				userName,
				domainName,
				password,
				loginType,
				out exception );
		}

		/// <summary>
		/// Does the actual impersonation.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">Type of the login.</param>
		/// <param name="exception">The exception.</param>
		/// <returns>
		/// 	<c>true</c> if this instance can log in the specified user name; otherwise, <c>false</c>.
		/// </returns>
		private static bool canLogInValidUser(
			string userName,
			string domainName,
			string password,
			LoginType loginType,
			out Exception exception )
		{
			Trace.TraceInformation(
				string.Format(
					@"[Impersonation] About to check for login as domain '{0}', user '{1}'.",
					domainName,
					userName ) );

			exception = null;

			if ( domainName != null && domainName.Length <= 0 )
			{
				domainName = null;
			}

			var token = IntPtr.Zero;

			try
			{
				if ( LogonUser(
					userName,
					domainName,
					password,
					(int)loginType,
					LOGON32_PROVIDER_DEFAULT,
					ref token ) == 0 )
				{
					var le = Marshal.GetLastWin32Error();
					exception = new Win32Exception( le );
				}
			}
			finally
			{
				if ( token != IntPtr.Zero )
				{
					CloseHandle( token );
				}
			}

			if ( exception == null )
			{
				Trace.TraceInformation(
					string.Format(
						@"[Impersonation] Successfully check for login as domain '{0}', user '{1}'.",
						domainName,
						userName ) );

				return true;
			}
			else
			{
				Trace.TraceError(
					string.Format(
						@"[Impersonation] Error check for login as domain '{0}', user '{1}'.",
						domainName,
						userName ),
					exception );

				return false;
			}
		}

		/// <summary>
		/// Does the actual impersonation.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">Type of the login.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		/// <param name="exception">The exception.</param>
		private void impersonateValidUser(
			string userName,
			string domainName,
			string password,
			LoginType loginType,
			ProfileBehaviour profileBehaviour,
			out Exception exception )
		{
			Trace.TraceInformation(
				string.Format(
				@"[Impersonation] About to impersonate as domain '{0}', user '{1}'.",
				domainName,
				userName ) );

			exception = null;

			if ( domainName != null && domainName.Length <= 0 )
			{
				domainName = null;
			}

			var token = IntPtr.Zero;

			try
			{
				if ( RevertToSelf() )
				{
					if ( LogonUser(
						userName,
						domainName,
						password,
						(int)loginType,
						LOGON32_PROVIDER_DEFAULT,
						ref token ) != 0 )
					{
						if ( DuplicateToken( token, 2, ref _impersonationToken ) != 0 )
						{
							checkLoadProfile( profileBehaviour );

							var tempWindowsIdentity =
								new WindowsIdentity( _impersonationToken );
							_impersonationContext =
								tempWindowsIdentity.Impersonate();
						}
						else
						{
							var le = Marshal.GetLastWin32Error();
							exception = new Win32Exception( le );
						}
					}
					else
					{
						var le = Marshal.GetLastWin32Error();
						exception = new Win32Exception( le );
					}
				}
				else
				{
					var le = Marshal.GetLastWin32Error();
					exception = new Win32Exception( le );
				}
			}
			finally
			{
				if ( token != IntPtr.Zero )
				{
					CloseHandle( token );
				}
			}

			if ( exception == null )
			{
				Trace.TraceInformation(
					string.Format(
					@"[Impersonation] Successfully impersonated as domain '{0}', user '{1}'.",
					domainName,
					userName ) );
			}
			else
			{
				Trace.TraceError(
					string.Format(
						@"[Impersonation] Error impersonating as domain '{0}', user '{1}'.",
						domainName,
						userName ),
					exception );
			}
		}

		/// <summary>
		/// Does the actual check for impersonation.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">Type of the login.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		private static bool canImpersonateValidUser(
			string userName,
			string domainName,
			SecureString password,
			LoginType loginType,
			ProfileBehaviour profileBehaviour )
		{
			using ( var impersonator = new Impersonator() )
			{
				Exception exception;

				impersonator.impersonateValidUser(
					userName,
					domainName,
					password,
					loginType,
					profileBehaviour,
					out exception );

				return exception == null;
			}
		}

		/// <summary>
		/// Does the actual impersonation.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">Type of the login.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		private void impersonateValidUser(
			string userName,
			string domainName,
			SecureString password,
			LoginType loginType,
			ProfileBehaviour profileBehaviour )
		{
			Exception exception;

			impersonateValidUser(
				userName,
				domainName,
				password,
				loginType,
				profileBehaviour,
				out exception );

			if ( exception != null )
			{
				throw exception;
			}
		}

		/// <summary>
		/// Does the actual impersonation.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">Type of the login.</param>
		/// <param name="profileBehaviour">The profile behaviour.</param>
		/// <param name="exception">The exception.</param>
		private void impersonateValidUser(
			string userName,
			string domainName,
			SecureString password,
			LoginType loginType,
			ProfileBehaviour profileBehaviour,
			out Exception exception )
		{
			Trace.TraceInformation(
				string.Format(
					@"[Impersonation] About to impersonate as domain '{0}', user '{1}'.",
					domainName,
					userName ) );

			exception = null;

			if ( domainName != null && domainName.Length <= 0 )
			{
				domainName = null;
			}

			var token = IntPtr.Zero;
			var passwordPtr = IntPtr.Zero;

			try
			{
				if ( RevertToSelf() )
				{
					// Marshal the SecureString to unmanaged memory.
					passwordPtr =
						Marshal.SecureStringToGlobalAllocUnicode( password );

					if ( LogonUser2(
						userName,
						domainName,
						passwordPtr,
						(int)loginType,
						LOGON32_PROVIDER_DEFAULT,
						ref token ) != 0 )
					{
						if ( DuplicateToken( token, 2, ref _impersonationToken ) != 0 )
						{
							checkLoadProfile( profileBehaviour );

							var tempWindowsIdentity =
								new WindowsIdentity( _impersonationToken );
							_impersonationContext =
								tempWindowsIdentity.Impersonate();
						}
						else
						{
							int le = Marshal.GetLastWin32Error();
							exception = new Win32Exception( le );
						}
					}
					else
					{
						int le = Marshal.GetLastWin32Error();
						exception = new Win32Exception( le );
					}
				}
				else
				{
					int le = Marshal.GetLastWin32Error();
					exception = new Win32Exception( le );
				}
			}
			finally
			{
				if ( token != IntPtr.Zero )
				{
					CloseHandle( token );
				}

				// Zero-out and free the unmanaged string reference.
				Marshal.ZeroFreeGlobalAllocUnicode( passwordPtr );
			}

			if ( exception == null )
			{
				Trace.TraceInformation(
					string.Format(
						@"[Impersonation] Successfully impersonated as domain '{0}', user '{1}'.",
						domainName,
						userName ) );
			}
			else
			{
				Trace.TraceError(
					string.Format(
						@"[Impersonation] Error impersonating as domain '{0}', user '{1}'.",
						domainName,
						userName ),
					exception );
			}
		}

		/// <summary>
		/// Does the actual check for login.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">Type of the login.</param>
		/// <returns>
		/// 	<c>true</c> if this instance can log in the specified user name; otherwise, <c>false</c>.
		/// </returns>
		private static bool canLogInValidUser(
			string userName,
			string domainName,
			SecureString password,
			LoginType loginType )
		{
			Exception exception;

			return canLogInValidUser(
				userName,
				domainName,
				password,
				loginType,
				out exception );
		}

		/// <summary>
		/// Does the actual check for login.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		/// <param name="loginType">Type of the login.</param>
		/// <param name="exception">The exception.</param>
		/// <returns>
		/// 	<c>true</c> if this instance can log in the specified user name; otherwise, <c>false</c>.
		/// </returns>
		private static bool canLogInValidUser(
			string userName,
			string domainName,
			SecureString password,
			LoginType loginType,
			out Exception exception )
		{
			Trace.TraceInformation(
				string.Format(
					@"[Impersonation] About to check for login as domain '{0}', user '{1}'.",
					domainName,
					userName ) );

			exception = null;

			if ( domainName != null && domainName.Length <= 0 )
			{
				domainName = null;
			}

			IntPtr token = IntPtr.Zero;
			IntPtr passwordPtr = IntPtr.Zero;

			try
			{
				// Marshal the SecureString to unmanaged memory.
				passwordPtr =
					Marshal.SecureStringToGlobalAllocUnicode( password );

				if ( LogonUser2(
					userName,
					domainName,
					passwordPtr,
					(int)loginType,
					LOGON32_PROVIDER_DEFAULT,
					ref token ) == 0 )
				{
					int le = Marshal.GetLastWin32Error();
					exception = new Win32Exception( le );
				}
			}
			finally
			{
				if ( token != IntPtr.Zero )
				{
					CloseHandle( token );
				}

				// Zero-out and free the unmanaged string reference.
				Marshal.ZeroFreeGlobalAllocUnicode( passwordPtr );
			}

			if ( exception == null )
			{
				Trace.TraceInformation(
					string.Format(
						@"[Impersonation] Successfully check for login as domain '{0}', user '{1}'.",
						domainName,
						userName ) );

				return true;
			}
			else
			{
				Trace.TraceError(
					string.Format(
						@"[Impersonation] Error check for login as domain '{0}', user '{1}'.",
						domainName,
						userName ),
					exception );

				return false;
			}
		}

		/// <summary>
		/// Checks and loads the load profile.
		/// </summary>
		private void checkLoadProfile(
			ProfileBehaviour profileBehaviour )
		{
			if ( profileBehaviour == ProfileBehaviour.Load )
			{
				_profileInfo = new PROFILEINFO();
				_profileInfo.dwSize = Marshal.SizeOf( _profileInfo );
				_profileInfo.lpUserName = WindowsIdentity.GetCurrent().Name;

				if ( LoadUserProfile( _impersonationToken, ref _profileInfo ) )
				{
					_profileBehaviour = profileBehaviour;
				}
				else
				{
					int le = Marshal.GetLastWin32Error();
					throw new Win32Exception( le );
				}
			}
		}

		/// <summary>
		/// Reverts the impersonation.
		/// </summary>
		private void undoImpersonation()
		{
			if ( _impersonationContext != null )
			{
				Trace.TraceInformation(
					string.Format(
					@"[Impersonation] About to undo impersonation." ) );

				try
				{
					_impersonationContext.Undo();
					_impersonationContext = null;
				}
				catch ( Exception x )
				{
					Trace.TraceError(
						string.Format(
						@"[Impersonation] Error undoing impersonation." ),
						x );

					throw;
				}

				Trace.TraceInformation(
					string.Format(
					@"[Impersonation] Successfully undone impersonation." ) );
			}

			// --

			if ( _profileBehaviour == ProfileBehaviour.Load )
			{
				Trace.TraceInformation(
					string.Format(
					@"[Impersonation] About to unload user profile." ) );

				try
				{
					if ( !UnloadUserProfile( _impersonationToken, _profileInfo.hProfile ) )
					{
						int le = Marshal.GetLastWin32Error();
						throw new Win32Exception( le );
					}

					_profileBehaviour = ProfileBehaviour.DontLoad;
				}
				catch ( Exception x )
				{
					Trace.TraceError(
						string.Format(
						@"[Impersonation] Error unloading user profile." ),
						x );

					throw;
				}
			}

			if ( _impersonationToken != IntPtr.Zero )
			{
				CloseHandle( _impersonationToken );
				_impersonationToken = IntPtr.Zero;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private WindowsImpersonationContext _impersonationContext;

		private ProfileBehaviour _profileBehaviour = ProfileBehaviour.DontLoad;
		private PROFILEINFO _profileInfo;
		private IntPtr _impersonationToken = IntPtr.Zero;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// How to log in the user.
	/// </summary>
	public enum LoginType
	{
		#region Enum members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Interactive. This is the default.
		/// </summary>
		Interactive = 2,

		/// <summary>
		/// 
		/// </summary>
		Batch = 4,

		/// <summary>
		/// 
		/// </summary>
		Network = 3,

		/// <summary>
		/// 
		/// </summary>
		NetworkClearText = 0,

		/// <summary>
		/// 
		/// </summary>
		Service = 5,

		/// <summary>
		/// 
		/// </summary>
		Unlock = 7,

		/// <summary>
		/// 
		/// </summary>
		NewCredentials = 9

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// How to deal with the user's profile.
	/// </summary>
	/// <remarks>
	/// 2008-05-21, suggested and implemented by Tim Daplyn 
	/// (TDaplyn@MedcomSoft.com).
	/// </remarks>
	public enum ProfileBehaviour
	{
		#region Enum members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Do not load the user's profile. This is the default behaviour.
		/// </summary>
		DontLoad,

		/// <summary>
		/// Load the user's profile.
		/// </summary>
		Load

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}