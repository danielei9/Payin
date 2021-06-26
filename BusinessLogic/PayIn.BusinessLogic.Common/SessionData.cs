using System;
using System.Linq;
using System.Security.Claims;
using Xp.Common;
using System.Collections.Generic;
using PayIn.Common;

namespace PayIn.BusinessLogic.Common
{
	public class SessionData : ISessionData
	{
		#region ApplicationName
		public string ApplicationName
		{
			get { return "PayIn"; }
		}
		#endregion ApplicationName

		#region Login
		private string _Login = null;
		public string Login
		{
			get
			{
				if (_Login == null)
				{
					if (Identity == null)
						_Login = "";
					else if (Identity.Name.IsNullOrEmpty())
						_Login = "";
					else
						_Login = Identity.Name;
				}
				return _Login;
			}
		}
		#endregion Login

		#region Name
		private string _Name;
		public string Name
		{
			get
			{
				if (_Name == null)
					_Name = GetClaim(XpClaimTypes.Name);

				return _Name;
			}
		}
		#endregion Name

		#region Email
		private string _Email;
		public string Email
		{
			get
			{
				if (_Email == null)
					_Email = GetClaim(XpClaimTypes.Email);
				return _Email;
			}
		}
		#endregion Email

		#region Roles
		private string[] _Roles;
		public string[] Roles
		{
			get
			{
				if (_Roles == null)
					_Roles = GetClaim(ClaimTypes.Role).SplitString(",");

				return _Roles;
			}
		}
		#endregion Roles
		
		#region ClientId
		private string _ClientId;
		public string ClientId
		{
			get
			{
				if (_ClientId == null)
					_ClientId = GetClaim(XpClaimTypes.ClientId);
				return _ClientId;
			}
		}
		#endregion ClientId
		
		#region TaxNumber
		private string _TaxNumber;
		public string TaxNumber
		{
			get
			{
				if (_TaxNumber == null)
					_TaxNumber = GetClaim(XpClaimTypes.TaxNumber);

				return _TaxNumber;
			}
		}
		#endregion TaxNumber

		#region TaxName
		private string _TaxName;
		public string TaxName
		{
			get
			{
				if (_TaxName == null)
					_TaxName = GetClaim(XpClaimTypes.TaxName);

				return _TaxName;
			}
		}
		#endregion TaxName

		#region TaxAddress
		private string _TaxAddress;
		public string TaxAddress
		{
			get
			{
				if (_TaxAddress == null)
					_TaxAddress = GetClaim(XpClaimTypes.TaxAddress);

				return _TaxAddress;
			}
		}
		#endregion TaxAddress

		#region Uri
		public string _Uri;
		public string Uri
		{
			get
			{
				//var result = Thread.GetData(Thread.GetNamedDataSlot("XpUri")) as string;
				//return result;
				if (_Uri == null)
					_Uri = GetClaim(XpClaimTypes.Uri);

				return _Uri;
			}
		}
		#endregion Uri

		#region Token
		private string _Token;
		public string Token
		{
			get
			{
				//var result = Thread.GetData(Thread.GetNamedDataSlot("XpToken")) as string;
				//return result;
				if (_Token == null)
					_Token = GetClaim(XpClaimTypes.Token);

				return _Token;
			}
		}
		#endregion Token

		#region Identity
		private ClaimsIdentity _Identity = null;
		public ClaimsIdentity Identity
		{
			get
			{
				if (_Identity == null)
					_Identity = System.Threading.Thread.CurrentPrincipal.Identity as ClaimsIdentity;

				return _Identity;
			}
		}
        #endregion Identity

        #region GetClaim
        public string GetClaim(string claimName)
		{
			if (Identity == null)
				return "";

			var result = Identity.Claims
				.Where(x => x.Type == claimName)
				.Select(x => x.Value)
				.JoinString(",");

			return result;
		}
		#endregion GetClaim

		#region Language
		public LanguageEnum? Language => LanguageEnum.Spanish;
        #endregion Language
    }
}
 