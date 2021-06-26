using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class CampaignRepository : PublicRepository<Campaign>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public CampaignRepository(
			ISessionData sessionData,
			IPublicContext context
		)
			: base(context)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<Campaign> CheckHorizontalVisibility(IQueryable<Campaign> that)
		{
			/*that = that
					.Where(x =>
					x.OwnerCampaign.Any(y => y.Concession.State == ConcessionState.Active && y.Concession.Supplier.Login == SessionData.Login) 
					|| x.PayCampaign.Any(y => y.Concession.State == ConcessionState.Active && y.Concession.Supplier.Login == SessionData.Login)
				);*/


			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
