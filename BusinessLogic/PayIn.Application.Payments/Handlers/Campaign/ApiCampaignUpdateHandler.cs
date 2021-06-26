using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ApiCampaignUpdateHandler : IServiceBaseHandler<ApiCampaignUpdateArguments>
	{
		private readonly IEntityRepository<Campaign> Repository;
		private readonly IEntityRepository<PaymentConcession> ConcessionRepository;
		private readonly IEntityRepository<PaymentConcessionCampaign> PcCamapignRepository;
		private readonly IUnitOfWork UnitOfWork;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;
		private readonly ISessionData SessionData;

		#region Constructors
		public ApiCampaignUpdateHandler(
			IUnitOfWork unitOfWork, 
			IEntityRepository<Campaign> repository, 
			IEntityRepository<PaymentConcession> concessionRepository, 
			IEntityRepository<PaymentConcessionCampaign> pcCampaignRepository, 
			ISessionData sessionData, 
			ServiceNotificationCreateHandler serviceNotificationCreate
		)
		{
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (repository == null) throw new ArgumentNullException("repository");
			if (concessionRepository == null) throw new ArgumentNullException("paymentRepository");
			if (pcCampaignRepository == null) throw new ArgumentNullException("paymentConcessionCampaignRepository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");

			UnitOfWork = unitOfWork;
			Repository = repository;
			ConcessionRepository = concessionRepository;
			PcCamapignRepository = pcCampaignRepository;
			SessionData = sessionData;
			ServiceNotificationCreate = serviceNotificationCreate;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ApiCampaignUpdateArguments arguments)
		{
			var now = new XpDate(DateTime.Now);

			if (arguments.Until != XpDate.MaxValue && arguments.Since.Value >= arguments.Until.Value)
				throw new ApplicationException(CampaignResources.UntilPreviousThanSinceException);
			if ((arguments.NumberOfTimes != null) && (arguments.NumberOfTimes <= 0))
				throw new ApplicationException(CampaignResources.NullNumberOfTimes);

			var campaign = await Repository.GetAsync(arguments.Id);
			if (campaign == null)
				throw new ArgumentNullException("id");

			campaign.Since = arguments.Since.Value;
			campaign.Until = arguments.Until.Value;
			campaign.Title = arguments.Title;
			campaign.Description = arguments.Description;
			campaign.NumberOfTimes = arguments.NumberOfTimes;
			campaign.TargetSystemCardId = arguments.TargetSystemCardId;
			campaign.TargetConcessionId = arguments.TargetConcessionId;

			return campaign;
		}
		#endregion ExecuteAsync
	}
}
