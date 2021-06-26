using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceNotificationGetAllHandler :
		IQueryBaseHandler<ServiceNotificationGetAllArguments, ServiceNotificationGetAllResult>
	{
		[Dependency] public IEntityRepository<ServiceNotification> Repository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ServiceNotificationGetAllResult>> ExecuteAsync(ServiceNotificationGetAllArguments arguments)
		{
			if (arguments.Since.Value > arguments.Until.Value)
				return new ResultBase<ServiceNotificationGetAllResult>();

			var until = arguments.Until.AddDays(1);
			var items = (await Repository.GetAsync());
			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x => (
					x.Message.Contains(arguments.Filter) ||
					x.ReferenceClass.Contains(arguments.Filter) ||
					x.ReceiverLogin.Contains(arguments.Filter) ||
					x.SenderLogin.Contains(arguments.Filter)
				));

			if (!arguments.Type.IsNullOrEmpty())
			{
				var tipo = Convert.ToInt32(arguments.Type);
				if (tipo > 0)
					items = items.Where(x => (
						(int) x.Type == tipo
					));

			}

			var result = items
				.Where(x =>
					x.Type == NotificationType.Personalized &&
					x.State != NotificationState.Deleted &&
					(
						x.CreatedAt >= arguments.Since && x.CreatedAt < until
					)
				)
				.Select(x => new
				{
					Id = x.Id,
					State = x.State,
					Type = x.Type,
					ReferenceClass = x.ReferenceClass,
					ReferenceId = x.ReferenceId,
					ReceiverLogin = x.ReceiverLogin,
					SenderLogin = x.SenderLogin,
					CreatedAt = x.CreatedAt
				})
				.OrderByDescending(x => x.CreatedAt)
				.ToList()
				.Select(x => new ServiceNotificationGetAllResult
				{
					Id = x.Id,
					State = x.State,
					Type = x.Type,
					ReferenceClass = x.ReferenceClass,
					ReferenceId = x.ReferenceId,
					ReceiverLogin = x.ReceiverLogin,
					SenderLogin = x.SenderLogin,
					CreatedAt = x.CreatedAt
				})
				.Take(1000)
				.ToList();
			
			return new ResultBase<ServiceNotificationGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}

