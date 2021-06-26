using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Application.Payments.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ApiCampaignGetAllHandler : IQueryBaseHandler<ApiCampaignGetAllArguments, ApiCampaignGetAllResult>
	{
		[Dependency] public IEntityRepository<Campaign> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public PaymentConcessionGetSelectorHandler PaymentConcessionGetSelectorHandler { get; set; }

		#region ExecuteAsync
		async Task<ResultBase<ApiCampaignGetAllResult>> IQueryBaseHandler<ApiCampaignGetAllArguments, ApiCampaignGetAllResult>.ExecuteAsync(ApiCampaignGetAllArguments arguments)
		{
			var now = new XpDate(DateTime.Now);

			var paymentConcessions = (await PaymentConcessionGetSelectorHandler.ExecuteInternalAsync(""))
				.ToList();
			var paymentConcession =
				paymentConcessions.Where(x => x.Id == arguments.PaymentConcessionId).FirstOrDefault() ??
				paymentConcessions.FirstOrDefault();

			var items = (await Repository.GetAsync())
				.Where(x =>
					(x.State != CampaignState.Deleted) &&
					(x.ConcessionId == paymentConcession.Id) &&
					(
						(x.Concession.Concession.Supplier.Login == SessionData.Login) ||
						(x.Concession.PaymentWorkers.Any(y => y.Login == SessionData.Login)) ||
						(x.PaymentConcessionCampaigns.Any(y => y.PaymentConcession.Concession.Supplier.Login == SessionData.Login && y.State == PaymentConcessionCampaignState.Active))
					)
				);

			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x =>
					x.Title.Contains(arguments.Filter) ||
					x.Description.Contains(arguments.Filter)
				);

			var result = items
			.Select(x => new 
			{
				Id = x.Id,
				Title = x.Title,
				State = x.State,
				Description = x.Description,
				Since = x.Since,
				Until = x.Until,
				NumberOfTimes = x.NumberOfTimes,
				Active = x.Since <= now && x.Until >= now,
				NumberPaymentConcessions = (x.PaymentConcessionCampaigns.Count()-1),
				NumberActivePaymentConcessions = (x.PaymentConcessionCampaigns.Where(y => y.State== PaymentConcessionCampaignState.Active).Count() - 1),
				IsSupplier =
					x.Concession.Concession.Supplier.Login == SessionData.Login ||
					x.Concession.PaymentWorkers.Any(y => y.Login == SessionData.Login),
				CampaignLines = x.CampaignLines
					.Where(y =>
						y.State != CampaignLineState.Deleted
					)
					.Count(),
               events = x.TargetEvents.Count()
               })
			.OrderByDescending(x => x.Since)
			.ToList()
			.Select(x => new ApiCampaignGetAllResult
			{
				Id = x.Id,
				Title = x.Title,
				State = x.State,
				Description = x.Description,
				Since = x.Since,
				Until = x.Until != XpDate.MaxValue ? x.Until : (DateTime?)null,
				NumberOfTimes = x.NumberOfTimes != int.MaxValue ? x.NumberOfTimes : (int?)null,
				Active = x.Active,
				NumberPaymentConcessions = x.NumberPaymentConcessions,
				NumberActivePaymentConcessions = x.NumberActivePaymentConcessions,
				IsSupplier = x.IsSupplier,
				CampaignLines = x.CampaignLines,
                events= x.events
			});

			return new ApiCampaignGetAllResultBase
			{
				Data = result,
				PaymentConcessionId = paymentConcession?.Id,
				PaymentConcessionName = paymentConcession?.Value ?? "",
				PaymentConcessions = paymentConcessions
			};
		}
		#endregion ExecuteAsync
	}
}
