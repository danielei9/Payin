using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	class ControlTemplateCheckRepository : PublicRepository<ControlTemplateCheck>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ControlTemplateCheckRepository(
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
		public override IQueryable<ControlTemplateCheck> CheckHorizontalVisibility(IQueryable<ControlTemplateCheck> that)
		{
			var result = that
				.Where(x =>
					x.Template.Item.Concession.Type == Common.ServiceType.Control &&
					x.Template.Item.Concession.State == Common.ConcessionState.Active && (
						x.Template.Item.Concession.Supplier.Login == SessionData.Login ||
						x.Template.Item.Concession.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login)
					)
				)
			;
			return result;
		}
		#endregion CheckHorizontalVisibility
	}
}
