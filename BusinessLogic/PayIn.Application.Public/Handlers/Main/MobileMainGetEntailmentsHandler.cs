using PayIn.Application.Dto.Arguments.Main;
using PayIn.Application.Dto.Results.Main;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers.Main
{
	public class MobileMainGetEntailmentsHandler : 
		IQueryBaseHandler<MainMobileGetEntailmentsArguments, MainMobileGetEntailmentsResult>
	{
		private readonly SessionData SessionData;
		private readonly IEntityRepository<PaymentWorker> PaymentWorkerRepository;
		private readonly IEntityRepository<PaymentUser> PaymentUserRepository;
		private readonly IEntityRepository<PaymentConcessionCampaign> PaymentConcessionCampaignRepository;
		private readonly IEntityRepository<PaymentConcessionPurse> PaymentConcessionPurseRepository;

		#region Contructors
		public MobileMainGetEntailmentsHandler(
			SessionData sessionData,
			IEntityRepository<PaymentWorker> paymentWorkerRepository,
            IEntityRepository<PaymentUser> paymentUserRepository,
			IEntityRepository<PaymentConcessionCampaign> paymentConcessionCampaignRepository,
			IEntityRepository<PaymentConcessionPurse> paymentConcessionPurseRepository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (paymentWorkerRepository == null) throw new ArgumentNullException("paymentWorkerRepository");
			if (paymentUserRepository == null) throw new ArgumentNullException("paymentUserRepository");
			if (paymentConcessionCampaignRepository == null) throw new ArgumentNullException("paymentConcessionCampaignRepository");
			if (paymentConcessionPurseRepository == null) throw new ArgumentNullException("paymentConcessionPurseRepository");

			SessionData = sessionData;
			PaymentWorkerRepository = paymentWorkerRepository;
			PaymentUserRepository = paymentUserRepository;
			PaymentConcessionCampaignRepository = paymentConcessionCampaignRepository;
			PaymentConcessionPurseRepository = paymentConcessionPurseRepository;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<ResultBase<MainMobileGetEntailmentsResult>> ExecuteAsync(MainMobileGetEntailmentsArguments arguments)
		{
			var result = (await PaymentWorkerRepository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login &&
					(x.State == WorkerState.Active || x.State == WorkerState.Pending)
				)
				.Select(x => new MainMobileGetEntailmentsResult
				{
					Id = x.Id,
					ConcessionName = x.Concession.Concession.Name,
					State = x.State
				});

			var paymentUsers = (await PaymentUserRepository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login &&
					(x.State == PaymentUserState.Active || x.State == PaymentUserState.Blocked || x.State == PaymentUserState.Pending)
				)
				.Select(x => new MainMobileGetEntailmentsResultBase.PaymentUser
				{
					Id = x.Id,
					ConcessionName = x.Concession.Concession.Name,
					State = x.State
				});

			var campaigns = (await PaymentConcessionCampaignRepository.GetAsync())
				.Where(x =>
					x.PaymentConcession.Concession.Supplier.Login == SessionData.Login &&
					(x.State == PaymentConcessionCampaignState.Active || x.State == PaymentConcessionCampaignState.Pending)
				)
				.Select(x => new MainMobileGetEntailmentsResultBase.Campaign
				{
					Id = x.Id,
					ConcessionName = x.Campaign.Concession.Concession.Name,
					CampaignName = x.Campaign.Title,
					State = x.State
				});

			var purses = (await PaymentConcessionPurseRepository.GetAsync())
				.Where(x =>
					x.PaymentConcession.Concession.Supplier.Login == SessionData.Login &&
					(x.State == PaymentConcessionPurseState.Active || x.State == PaymentConcessionPurseState.Pending)
				)
				.Select(x => new MainMobileGetEntailmentsResultBase.Purse
				{
					Id = x.Id,
					ConcessionName = x.Purse.Concession.Concession.Name,
					PurseName = x.Purse.Name,
					State = x.State
				});

			return new MainMobileGetEntailmentsResultBase {
				Data = result,
				PaymentUsers = paymentUsers,
				Campaigns = campaigns,
				Purses = purses
			};
		}
		#endregion ExecuteAsync
	}
}
