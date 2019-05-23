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
	using System;
	using System.Diagnostics;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Summarizes group information from Active Directory.
	/// <see href="ms-help://MS.MSDNQTR.2002JUL.1033/netdir/ad/group_object_user_interface_mapping.htm"/>
	/// </summary>
	[DebuggerDisplay( @"Name = {Name}, SAM name = {SamName}" )]
	public class ADGroupInfo
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="owner">The owner.</param>
		protected internal ADGroupInfo(
			ActiveDirectory owner )
		{
			_owner = owner;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Guid
		/// </summary>
		/// <value>The GUID.</value>
		public Guid Guid { get; internal set; }

		/// <summary>
		/// SAM name
		/// </summary>
		/// <value>The name of the sam.</value>
		public string SamName { get; internal set; }

		/// <summary>
		/// Common name
		/// </summary>
		/// <value>The CN.</value>
		public string CN
		{
			get
			{
				return _cn;
			}
			internal set
			{
				_cn = value;
			}
		}

		/// <summary>
		/// Name
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// E-mail address
		/// </summary>
		/// <value>The E mail.</value>
		public string EMail { get; set; }

		/// <summary>
		/// Scope
		/// </summary>
		/// <value>The scope.</value>
		public string Scope { get; set; }

		/// <summary>
		/// Type
		/// </summary>
		/// <value>The type.</value>
		public int Type { get; internal set; }

		/// <summary>
		/// Description
		/// </summary>
		/// <value>The description.</value>
		public string Description { get; set; }

		/// <summary>
		/// Returns a list of the direct(!) groups that a group belongs to.
		/// </summary>
		/// <value>The parent groups.</value>
		public ADGroupInfo[] ParentGroups
		{
			get
			{
				if ( _parentGroups == null )
				{
					_parentGroups = _owner.GetGroupParentGroups( this );
				}

				return _parentGroups;
			}
		}

		/// <summary>
		/// Returns a list of the direct(!) groups that belong to this group.
		/// </summary>
		/// <value>The child groups.</value>
		public ADGroupInfo[] ChildGroups
		{
			get
			{
				if ( _childGroups == null )
				{
					_childGroups = _owner.GetGroupChildGroups( this );
				}

				return _childGroups;
			}
		}

		/// <summary>
		/// Returns a list of the direct(!) groups that a group belongs to.
		/// </summary>
		/// <value>The child users.</value>
		public ADUserInfo[] ChildUsers
		{
			get
			{
				if ( _childUsers == null )
				{
					_childUsers = _owner.GetGroupChildUsers( this );
				}

				return _childUsers;
			}
		}

		/// <summary>
		/// Gets or sets the DN.
		/// </summary>
		/// <value>The DN.</value>
		protected internal string DN { get; set; }

		/// <summary>
		/// The owning parent object.
		/// </summary>
		/// <value>The active directory.</value>
		public ActiveDirectory ActiveDirectory
		{
			get
			{
				return _owner;
			}
		}

		/// <summary>
		/// Gets or sets the primary group token.
		/// </summary>
		/// <value>The primary group token.</value>
		public int PrimaryGroupToken { get; set; }

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private string _cn;

		/// <summary>
		/// Cached.
		/// </summary>
		private ADGroupInfo[] _parentGroups;

		/// <summary>
		/// Cached.
		/// </summary>
		private ADGroupInfo[] _childGroups;

		/// <summary>
		/// Cached.
		/// </summary>
		private ADUserInfo[] _childUsers;

		private readonly ActiveDirectory _owner;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}