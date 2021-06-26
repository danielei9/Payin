using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ControlPlanningItemRepository : PublicRepository<ControlPlanningItem>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ControlPlanningItemRepository(
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
		public override IQueryable<ControlPlanningItem> CheckHorizontalVisibility(IQueryable<ControlPlanningItem> that)
		{
			var result = that
				.Where(x =>
					x.Planning.Item.Concession.Type == Common.ServiceType.Control && 
					x.Planning.Item.Concession.State == Common.ConcessionState.Active && (
						x.Planning.Item.Concession.Supplier.Login == SessionData.Login ||
						x.Planning.Item.Concession.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login)
					)
				)
			;
			return result;
		}
		#endregion CheckHorizontalVisibility
	}
}
