using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ControlFormValueRepository : PublicRepository<ControlFormValue>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ControlFormValueRepository(
			ISessionData sessionData, 
			IPublicContext context
		)
			: base(context)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Contructors

		//#region CheckHorizontalVisibility
		//public override IQueryable<ControlFormValue> CheckHorizontalVisibility(IQueryable<ControlFormValue> that)
		//{
			//	var result = that
			//		.Where(x =>
			//			(x.Assign.Check.Planning.Item.Concession.Type == Common.ServiceType.Control || x.Assign.Check.Planning.Item.Concession.Type == Common.ServiceType.Charge) &&
			//			x.Assign.Check.Planning.Item.Concession.State == Common.ConcessionState.Active && (
			//				x.Assign.Check.Planning.Item.Concession.Supplier.Login == SessionData.Login ||
			//				x.Assign.Check.Planning.Item.Concession.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login)
			//			)
			//		)
			//	;
			//	return result;
		//}
		//#endregion CheckHorizontalVisibility
	}
}
