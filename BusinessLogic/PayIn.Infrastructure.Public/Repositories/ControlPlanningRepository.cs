using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ControlPlanningRepository : PublicRepository<ControlPlanning>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ControlPlanningRepository(
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
		public override IQueryable<ControlPlanning> CheckHorizontalVisibility(IQueryable<ControlPlanning> that)
		{
			if (SessionData.Roles.Contains(AccountRoles.Operator))
				that = that
					.Where(x =>
						x.Item.Concession.Type == Common.ServiceType.Control &&
						x.Item.Concession.State == Common.ConcessionState.Active && 
						(
							x.Item.Concession.Supplier.Login == SessionData.Login ||
							x.Item.Concession.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login)
						)
					);
			else
				that = that
					.Where(x =>
						x.Item.Concession.Type == Common.ServiceType.Control &&
						x.Item.Concession.State == Common.ConcessionState.Active && 
						(
							x.Item.Concession.Supplier.Login == SessionData.Login ||
							x.Worker.Login == SessionData.Login
						)
					);

			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
