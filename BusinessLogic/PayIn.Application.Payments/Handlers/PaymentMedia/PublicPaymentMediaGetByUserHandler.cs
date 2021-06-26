using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PublicPaymentMediaGetByUserHandler :
		IQueryBaseHandler<PublicPaymentMediaGetByUserArguments, MobilePaymentMediaGetAllResult>
	{
		[Dependency] public IEntityRepository<PaymentMedia> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobilePaymentMediaGetAllResult>> ExecuteAsync(PublicPaymentMediaGetByUserArguments arguments)
		{
			var now = DateTime.Now;

			var result = await ExecuteInternalAsync(now, arguments.Login);

			return new ResultBase<MobilePaymentMediaGetAllResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync

		#region ExecuteInternalAsync
		public async Task<IEnumerable<MobilePaymentMediaGetAllResult>> ExecuteInternalAsync(DateTime now, string login)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
					x.Login == login &&
					(
						x.PaymentConcession.Concession.Supplier.Login == SessionData.Login ||
						x.PaymentConcession.PaymentWorkers
							.Any(y => y.Login == SessionData.Login)
					) &&
					x.Type == PaymentMediaType.WebCard &&
					(
						x.State == PaymentMediaState.Active ||
						x.State == PaymentMediaState.Pending
					)
				)
				.Select(x => new MobilePaymentMediaGetAllResult
				{
					Id              = x.Id,
					Title           = x.Name,
					Subtitle        = x.Type.ToString(),
					VisualOrder     = x.VisualOrder,
					NumberHash      = x.NumberHash,
					ExpirationMonth = x.ExpirationMonth,
					ExpirationYear  = x.ExpirationYear,
					Type            = x.Type,
					State           = x.State,
					BankEntity      = x.BankEntity,
					Image           = x.Purse.Image,
					Login           = x.Login,
					Balance         = 0
				})
#if DEBUG
				.ToList()
#endif // DEBUG
				;

			return items;
		}
		#endregion ExecuteInternalAsync
	}
}
