using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Application.Payments.Handlers;
using PayIn.Application.Transport.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Promotions;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Handlers
{
	[XpLog("TransportOperation", "RechargeAndPay", RelatedId = "OperationId")]
	[XpAnalytics("TransportOperation", "RechargeAndPay")]
	public class TransportOperationRechargeAndPayHandler : TransportOperationRechargeHandler,
		IServiceBaseHandler<TransportOperationRechargeAndPayArguments>
	{
		private readonly MobileTicketPayV3Handler TicketMobilePayV3Handler;

		#region Constructors
		public TransportOperationRechargeAndPayHandler(
			ISessionData sessionData,
			EigeService eigeService,
			IEntityRepository<TransportOperationTitle> transportOperationTitleRepository,
			IEntityRepository<PromoExecution> promoExecutionRepository,
			IEntityRepository<PromoUser> promoUserRepository,
			IEntityRepository<Promotion> promotionRepository,
			IEntityRepository<TransportTitle> titleRepository,
			IEntityRepository<Ticket> ticketRepository,
			IEntityRepository<PaymentConcession> paymentConcessionRepository,
			IEntityRepository<TransportPrice> priceRepository,
			IEntityRepository<BlackList> blackListRepository,
			IEntityRepository<GreyList> greyListRepository,
			IEntityRepository<TransportOperation> transportOperationRepository,
			IEntityRepository<TransportSimultaneousTitleCompatibility> transportSimoultaneousRepository,
			IEntityRepository<TransportCardSupportTitleCompatibility> transportCardRepository,
			IUnitOfWork unitOfWork,
			IMifareClassicHsmService hsm,
			TransportOperationReadInfoHandler transportOperatinReadInfoRepository,
			MobileTicketPayV3Handler ticketMobilePayV3Handler
		)
			: base(sessionData, eigeService, transportOperationTitleRepository, promoExecutionRepository, promoUserRepository, promotionRepository, titleRepository, ticketRepository, paymentConcessionRepository, priceRepository, blackListRepository, greyListRepository,
				  transportOperationRepository, transportSimoultaneousRepository, transportCardRepository, unitOfWork, hsm, transportOperatinReadInfoRepository
				  )
		{
			if (ticketMobilePayV3Handler == null) throw new ArgumentNullException("ticketMobilePayV3Handler");
			TicketMobilePayV3Handler = ticketMobilePayV3Handler;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportOperationRechargeAndPayArguments arguments)
		{
#if TEST || DEBUG
			try
			{
#endif
				// Pay ticket
				var payArguments = new MobileTicketPayV3Arguments(
					arguments.TicketId,
					arguments.PaymentMedias.Select(x => new MobileTicketPayV3Arguments_PaymentMedia()
					{
						Id = x.Id,
						Balance = x.Balance,
						Order = x.Order
					}),
					arguments.Promotions.Select(x => new MobileTicketPayV3Arguments_Promotion()
					{
						Id = x.Id,
						Quantity = x.Quantity,
						Type = x.Type
					}),
					arguments.Pin,
					arguments.DeviceManufacturer,
					arguments.DeviceModel,
					arguments.DeviceName,
					arguments.DeviceSerial,
					arguments.DeviceId,
					arguments.DeviceOperator,
					arguments.DeviceImei,
					arguments.DeviceMac,
					arguments.OperatorSim,
					arguments.OperatorMobile
				);
				var payResult = await TicketMobilePayV3Handler.ExecuteAsync(payArguments);

				// Recharge
				var rechargeResult = await base.ExecuteAsync(arguments);

				return rechargeResult;
#if TEST || DEBUG
			}
			catch (Exception ex)
			{
				throw ex; // new ApplicationException("ERROR: " + ex.Message + "\n\n" + ex.StackTrace);
			}
#endif
		}
		#endregion ExecuteAsync
	}
}