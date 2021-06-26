using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ControlFormAssignTemplateRepository : PublicRepository<ControlFormAssignTemplate>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ControlFormAssignTemplateRepository(
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
		public override IQueryable<ControlFormAssignTemplate> CheckHorizontalVisibility(IQueryable<ControlFormAssignTemplate> that)
		{
			var result = that
				.Where(x =>
					x.Check.Template.Item.Concession.Type == Common.ServiceType.Control &&
					x.Check.Template.Item.Concession.State == Common.ConcessionState.Active && (
						x.Check.Template.Item.Concession.Supplier.Login == SessionData.Login ||
						x.Check.Template.Item.Concession.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login)
					)
				)
			;
			return result;
		}
		#endregion CheckHorizontalVisibility
	}
}
