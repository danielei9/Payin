using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ControlTrackRepository : PublicRepository<ControlTrack>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ControlTrackRepository(
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
		public override IQueryable<ControlTrack> CheckHorizontalVisibility(IQueryable<ControlTrack> that)
		{
			var result = that
				.Where(x =>
					x.Item.Concession.Type == Common.ServiceType.Control &&
					x.Item.Concession.State == Common.ConcessionState.Active && (
						x.Item.Concession.Supplier.Login == SessionData.Login ||
						x.Item.Concession.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login)
					)
				)
			;
			return result;
		}
		#endregion CheckHorizontalVisibility
	}
}
