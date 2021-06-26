using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ControlTrackItemRepository : PublicRepository<ControlTrackItem>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ControlTrackItemRepository(
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
		public override IQueryable<ControlTrackItem> CheckHorizontalVisibility(IQueryable<ControlTrackItem> that)
		{
			var result = that
				.Where(x =>
					x.Track.Item.Concession.Type == Common.ServiceType.Control &&
					x.Track.Item.Concession.State == Common.ConcessionState.Active && (
						x.Track.Item.Concession.Supplier.Login == SessionData.Login ||
						x.Track.Item.Concession.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login)
					)
				)
			;
			return result;
		}
		#endregion CheckHorizontalVisibility
	}
}
