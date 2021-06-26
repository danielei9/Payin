using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ControlFormAssignRepository : PublicRepository<ControlFormAssign>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ControlFormAssignRepository(
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
		public override IQueryable<ControlFormAssign> CheckHorizontalVisibility(IQueryable<ControlFormAssign> that)
		{
			var result = that
				.Where(x =>
					x.Check.Planning.Item.Concession.Type == Common.ServiceType.Control && 
					x.Check.Planning.Item.Concession.State == Common.ConcessionState.Active && (
						x.Check.Planning.Item.Concession.Supplier.Login == SessionData.Login ||
						x.Check.Planning.Item.Concession.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login)
					)
				)
			;
			return result;
		}
		#endregion CheckHorizontalVisibility
	}
}
