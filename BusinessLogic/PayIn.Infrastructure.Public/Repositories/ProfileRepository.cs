using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ProfileRepository : PublicRepository<Profile>
	{
		public readonly ISessionData SessionData;

		#region Constructors
		public ProfileRepository(
			ISessionData sessionData, 
			IPublicContext context
		)
			: base(context)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors

		#region CheckHorizontalVisibility
		public override IQueryable<Profile> CheckHorizontalVisibility(IQueryable<Profile> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
