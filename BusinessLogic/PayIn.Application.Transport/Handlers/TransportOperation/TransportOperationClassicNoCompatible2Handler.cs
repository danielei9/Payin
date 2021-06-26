using PayIn.Application.Dto.Results;
using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Application.Transport.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using PayIn.Infrastructure.Transport.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Application.Public.Handlers
{
	[XpLog("TransportOperation", "ClassicNoCompatible", RelatedId = "Uids")]
	[XpAnalytics("TransportOperation", "ClassicNoCompatible", Response = new[] { "Data[0].Code", "Data[0].Name", "Data[1].Code", "Data[1].Name" })]
	public class TransportOperationClassicNoCompatible2Handler : TransportCardSearchInternalHandler,
		IQueryBaseHandler<TransportOperationClassicNoCompatible2Arguments, ServiceCardReadInfoResult>
	{
		private readonly IEntityRepository<PaymentConcession> PaymentConcessionRepository;
        private readonly IEntityRepository<SystemCardMember> SystemCardMembersRepository;
        private readonly IEntityRepository<Campaign> CampaignRepository;

		#region Constructors
		public TransportOperationClassicNoCompatible2Handler(
			ISessionData sessionData,
			EigeService eigeService,
			SigapuntService sigapuntService,
			EmtService emtService,
			FgvService fgvService,
			EmailService emailService,
			IEntityRepository<TransportOperation> transportOperationRepository,
			IEntityRepository<TransportTitle> transportTitleRepository,
			IEntityRepository<PaymentConcession> paymentConcessionrepository,
			IEntityRepository<Campaign> campaignRepository
		)
			: base(sessionData, eigeService, sigapuntService, emtService, fgvService, emailService, transportOperationRepository, transportTitleRepository)
		{
			if (paymentConcessionrepository == null) throw new ArgumentNullException("repository");
			if (campaignRepository == null) throw new ArgumentNullException("campaignRepository");

			PaymentConcessionRepository = paymentConcessionrepository;
			CampaignRepository = campaignRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceCardReadInfoResult>> ExecuteAsync(TransportOperationClassicNoCompatible2Arguments arguments)
		{
			var serviceCard = await ServiceCardAsync(arguments);
			if (serviceCard != null)
				return serviceCard;

			return await base.ExecuteAsync(arguments);
		}
		#endregion ExecuteAsync

		#region ServiceCardAsync
		public async Task<ResultBase<ServiceCardReadInfoResult>> ServiceCardAsync(TransportOperationClassicNoCompatible2Arguments arguments)
		{
			var uid = arguments.Cards.FromHexadecimal().ToInt32().Value;
            var systemCardMembers = (await SystemCardMembersRepository.GetAsync());

            var card = (await PaymentConcessionRepository.GetAsync())
				.Where(x =>
					x.Concession.Supplier.Login == SessionData.Login &&
					x.Concession.State == Common.ConcessionState.Active
				)
				.SelectMany(x =>
                    systemCardMembers
                    .Where(y => y.Login == x.Concession.Supplier.Login)
					.SelectMany(y =>
                        y.SystemCard.Cards
						.Where(z =>
							z.State == Common.ServiceCardState.Active &&
							z.Uid == uid
						)
						.Select(a => new {
							a.Id,
							OwnerUserName = a.OwnerUser.Name,
							OwnerUserLastName = a.OwnerUser.LastName,
							OwnerUserVatNumber = a.OwnerUser.VatNumber,
							OwnerUserPhoto = a.OwnerUser.Photo,
							Concessions = a.Users
								.Select(b => new ServiceCardReadInfoResult_Concession
								{
									Id = b.ConcessionId,
									Name = b.Concession.Name,
									Address = b.Concession.Supplier.TaxAddress,
									VatNumber = b.Concession.Supplier.TaxNumber,
									Email = b.Concession.Supplier.Login
								})
						})
					)
				)
				.FirstOrDefault();
			if (card == null)
				return null;

			var promotions = (await CampaignRepository.GetAsync())
				.Where(x => x.State == Common.CampaignState.Active)
				.Select(x => new
				{
					x.Id,
					Name = x.Title,
					Image = x.PhotoUrl,
					EndDate = x.Until,
					Concession = x.Concession.Concession.Name
				})
				.ToList()
				.Select(a => new ServiceCardReadInfoResult_Promotion
				{
					Id = a.Id,
					Name = a.Name,
					Image = a.Image,
					EndDate = a.EndDate.ToUTC(),
					Concession = a.Concession
				});

			var result = new ServiceCardReadInfoResultBase
				{
					OwnerName = card.OwnerUserName,
					UserName = card.OwnerUserName,
					UserDni = card.OwnerUserVatNumber,
					UserSurname = card.OwnerUserLastName,
					UserPhoto = card.OwnerUserPhoto,
					Concessions = card.Concessions,
					Promotions = promotions
				};

			return result;
		}
		#endregion ServiceCardAsync
	}
}
