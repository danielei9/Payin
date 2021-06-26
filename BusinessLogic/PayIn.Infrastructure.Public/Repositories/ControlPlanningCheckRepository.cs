using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ControlPlanningCheckRepository : PublicRepository<ControlPlanningCheck>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ControlPlanningCheckRepository(
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
		public override IQueryable<ControlPlanningCheck> CheckHorizontalVisibility(IQueryable<ControlPlanningCheck> that)
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
