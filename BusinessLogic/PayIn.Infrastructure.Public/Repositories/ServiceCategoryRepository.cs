using PayIn.Infrastructure.Public.Db;
using PayIn.Domain.Public;
using System.Linq;
using PayIn.BusinessLogic.Common;
using System;

namespace PayIn.Infrastructure.Public.Repositories
{
	class ServiceCategoryRepository : PublicRepository<ServiceCategory>
	{
		//public ISessionData SessionData { get; set; }

		#region Constructors
		public ServiceCategoryRepository (
			IPublicContext context
			//ISessionData sessionData
		)
			: base(context)
		{
			//if (sessionData == null) throw new ArgumentNullException("sessionData");

			//SessionData = sessionData;
		}
		#endregion Constructors

		//#region CheckHorizontalVisibility
		//public override IQueryable<ServiceCategory> CheckHorizontalVisibility(IQueryable<ServiceCategory> that)
		//{
		//	return that;
		//		.Where(x =>
		//			x.Concession.Type == Common.ServiceType.Control &&
		//			x.Concession.State == Common.ConcessionState.Active &&
		//			(
		//				x.Concession.Supplier.Login == SessionData.Login ||
		//				x.Concession.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login)
		//			)
		//		);
		//}
		//#endregion CheckHorizontalVisibility
	}
}
