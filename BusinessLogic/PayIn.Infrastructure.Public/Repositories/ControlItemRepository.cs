using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ControlItemRepository : PublicRepository<ControlItem>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ControlItemRepository(
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
		public override IQueryable<ControlItem> CheckHorizontalVisibility(IQueryable<ControlItem> that)
		{
			var result = that
				.Where(x =>
					x.Concession.Type == Common.ServiceType.Control && 
					x.Concession.State == Common.ConcessionState.Active && (
						x.Concession.Supplier.Login == SessionData.Login ||
						x.Concession.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login)
					)
				)
			;
			return result;
		}
		#endregion CheckHorizontalVisibility
	}
}
