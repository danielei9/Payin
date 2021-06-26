using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class NotificationSetGetAllHandler :
		IQueryBaseHandler<NotificationSetGetAllArguments, NotificationSetGetAllResult>
	{
		[Dependency] private ISessionData                      SessionData { get; set; }
		//[Dependency] private IEntityRepository<NotificationSet> Repository { get; set; }
		[Dependency] private IEntityRepository<ServiceNotification> Repository  { get; set; }

		#region ExecuteAsync
		async Task<ResultBase<NotificationSetGetAllResult>> IQueryBaseHandler<NotificationSetGetAllArguments, NotificationSetGetAllResult>.ExecuteAsync(NotificationSetGetAllArguments arguments)
		{
			var items = await Repository.GetAsync();
			//items = items
			//	.Where(x =>
			//		x.PaymentConcession.Concession.Supplier.Login == SessionData.Login ||
			//		x.PaymentConcession.Concession.Supplier.Workers
			//			.Any(y => y.Login == SessionData.Login)
			//	);

			//items = items
			//	.Where(x => x.EventId == arguments.EventId);

	
			var result = items
				.Select(x => new NotificationSetGetAllResult
				{
					Id = x.Id,
					Message = "" // DESCOMENTAR LINEA Y PONER x.Message EN LUGAR DE ""
				})
				.ToList();

			return new ResultBase<NotificationSetGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
