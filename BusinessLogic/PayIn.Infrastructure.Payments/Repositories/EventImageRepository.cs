using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments.Repositories
{
	public class EventImageRepository : PublicRepository<EventImage>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public EventImageRepository(
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
		public override IQueryable<EventImage> CheckHorizontalVisibility(IQueryable<EventImage> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
