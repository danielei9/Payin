using PayIn.BusinessLogic.Common;
using PayIn.Common;
using System;

namespace Payin.Test.Tpv
{
	public class SessionData : ISessionData
	{
		public string token;

		#region ApplicationName
		public string ApplicationName
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		#endregion ApplicationName

		#region Email
		public string Email
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		#endregion Email

		#region Login
		public string Login
		{
			get
			{
				return "tpv@pay-in.es";
			}
		}
		#endregion Login

		#region Name
		public string Name
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		#endregion Name

		#region Roles
		public string[] Roles
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		#endregion Roles

		#region ClientId
		public string ClientId
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		#endregion ClientId

		#region TaxAddress
		public string TaxAddress
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		#endregion TaxAddress

		#region TaxName
		public string TaxName
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		#endregion TaxName

		#region TaxNumber
		public string TaxNumber
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		#endregion TaxNumber

		#region Token
		public string Token
		{
			get
			{
				return token;
            }
			set
			{
				token = value;
			}
		}
        #endregion Token

        #region Language
        public LanguageEnum? Language
        {
            get
            {
                return LanguageEnum.Spanish;
            }
        }
        #endregion Language
    }
}
