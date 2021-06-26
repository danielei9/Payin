using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ApiCampaignCreateHandler : IServiceBaseHandler<ApiCampaignCreateArguments>
	{
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<Campaign> Repository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> ConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcessionCampaign> PaymentConcessionCampaignRepository { get; set; }
		[Dependency] public IEntityRepository<CampaignLine> CampaignLineRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ApiCampaignCreateArguments arguments)
		{
			var now = new XpDate(DateTime.Now);
			
			if (arguments.Until != XpDate.MaxValue && arguments.Since.Value >= arguments.Until.Value)
				throw new ApplicationException(CampaignResources.UntilPreviousThanSinceException);
			if (arguments.Until != XpDate.MaxValue && arguments.Until.Value <= now)
				throw new ApplicationException(CampaignResources.NullNumberOfTimes);
			if (arguments.NumberOfTimes <= 0)
				throw new ApplicationException(CampaignResources.UntilInvalidDateException);
			//if (arguments.Since.Value <= now)
			//	throw new ApplicationException(CampaignResources.SinceInvalidDateException);

			var paymentConcession = (await PaymentConcessionRepository.GetAsync())
				.Where(x =>
					(x.Id == arguments.PaymentConcessionId) &&
					(x.Concession.State == ConcessionState.Active) &&
					(
						(x.Concession.Supplier.Login == SessionData.Login) ||
						(x.PaymentWorkers
							.Any(y => y.Login == SessionData.Login)
						)
					)
				)
				.FirstOrDefault();
			if (paymentConcession == null)
				throw new ArgumentException("paymentConcessionId");

			var campaign = new Campaign
			{
				Title = arguments.Title,
				Description = arguments.Description,
				Since = arguments.Since.Value,
				Until = arguments.Until.Value,
				NumberOfTimes = arguments.NumberOfTimes,
				Concession = paymentConcession,
				State = CampaignState.Active,
				TargetSystemCardId = arguments.TargetSystemCardId,
				TargetConcessionId = arguments.TargetConcessionId
			};
			await Repository.AddAsync(campaign);

			var vinculate = new PaymentConcessionCampaign
			{
				State = PaymentConcessionCampaignState.Active,
				PaymentConcession = paymentConcession,
				Campaign = campaign
			};
			await PaymentConcessionCampaignRepository.AddAsync(vinculate);

			return campaign;
		}
		#endregion ExecuteAsync
	}
}

