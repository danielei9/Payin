using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Common;
using PayIn.Domain.Payments.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers
{
	public class MobilePaymentMediaGetByPaymentConcessionHandler :
		IQueryBaseHandler<MobilePaymentMediaGetByPaymentConcessionArguments, MobilePaymentMediaGetAllResult>
	{
		[Dependency] public IInternalService InternalService { get; set; }
		[Dependency] public MobilePaymentMediaGetAllHandler MobilePaymentMediaGetAllHandler { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<MobilePaymentMediaGetAllResult>> ExecuteAsync(MobilePaymentMediaGetByPaymentConcessionArguments arguments)
		{
			var now = DateTime.UtcNow;

			var items = (await MobilePaymentMediaGetAllHandler.GetPaymentMediasAsync(now, arguments.PaymentConcessionId))
				.ToList(); // Necesario porque sino al modificar el valor de Balance Linq no lo hace correctamente al usar otra copia.
			foreach (var pMedia in items)
			{
				var res = await InternalService.PaymentMediaGetBalanceAsync(pMedia.Id);
				if (res != null)
					pMedia.Balance = res.Balance;
			}
			var cards = items
				.Where(x => (x.Type == PaymentMediaType.WebCard)) //No sea monedero
				.Select(x => new MobilePaymentMediaGetAllResult
				{
					Id = x.Id,
					Title = x.Title,
					Subtitle = x.Type.ToString(),
					VisualOrder = x.VisualOrder,
					NumberHash = x.NumberHash,
					ExpirationMonth = x.ExpirationMonth,
					ExpirationYear = x.ExpirationYear,
					Type = x.Type,
					State = x.State,
					BankEntity = x.BankEntity,
					Balance = x.Balance,
					Image = x.Image
				});

			var purses = items
			.Where(x => (x.Type == PaymentMediaType.Purse) && x.Balance != 0) //Sea monedero y  con valor distinto de 0
			.Select(x => new MobilePaymentMediaGetAllResult_Purse
			{
				Id = x.Id,
				Title = x.Title,
				Subtitle = x.Type.ToString(),
				VisualOrder = x.VisualOrder,
				NumberHash = x.NumberHash,
				ExpirationMonth = x.ExpirationMonth,
				ExpirationYear = x.ExpirationYear,
				Type = x.Type,
				State = x.State,
				BankEntity = x.BankEntity,
				Balance = x.Balance,
				Image = x.Image
			});

			return new MobilePaymentMediaGetAllResultBase
			{
				Data = cards,
				Purses = purses
			};
		}
		#endregion ExecuteAsync
	}
}
