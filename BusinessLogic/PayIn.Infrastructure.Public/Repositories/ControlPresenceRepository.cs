using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ControlPresenceRepository : PublicRepository<ControlPresence>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ControlPresenceRepository(
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
		public override IQueryable<ControlPresence> CheckHorizontalVisibility(IQueryable<ControlPresence> that)
		{
			var result = that
				.Where(x =>
					(x.TrackSince ?? x.TrackUntil).Item.Concession.Type == Common.ServiceType.Control &&
					(x.TrackSince ?? x.TrackUntil).Item.Concession.State == Common.ConcessionState.Active && (
						(x.TrackSince ?? x.TrackUntil).Item.Concession.Supplier.Login == SessionData.Login ||
						(x.TrackSince ?? x.TrackUntil).Item.Concession.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login)
					)
				)
			;
			return result;
		}
		#endregion CheckHorizontalVisibility
	}
}
