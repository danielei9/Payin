using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Application.Dto.Transport.Results.TransportOperation;
using PayIn.Application.Public.Handlers;
using PayIn.Application.Transport.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Promotions;
using PayIn.Domain.Transport;
using PayIn.Infrastructure.Transport.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Handlers
{
	[XpLog("TransportOperation", "ConfirmAndReadInfo", RelatedId = "OperationId")]
	[XpAnalytics("TransportOperation", "ConfirmAndReadInfo", Response = new[] { "Data[0].Code", "Data[0].Name", "Data[1].Code", "Data[1].Name" })]
	public class TransportOperationConfirmAndReadInfoHandler :
		TransportOperationReadInfoHandler,
		IServiceBaseHandler<TransportOperationConfirmAndReadInfoArguments>
    {
        private const string METHOD_NAME = "TransportOperation_ConfirmAndReadInfo";

        private readonly IEntityRepository<TransportOperation> Repository;
		private TransportOperationConfirmHandler ConfirmHandler;
		private SigapuntService SigapuntService;
		private ISessionData SessionData;

		#region Constructors
		public TransportOperationConfirmAndReadInfoHandler(
			IEntityRepository<TransportOperation> repository,
			ISessionData sessionData,
			EigeService eigeService,
			TescService tescService,
			IMifareClassicHsmService hsm,
			IEntityRepository<GreyList> greyListRepository,
			IEntityRepository<BlackList> blackListRepository,
			IEntityRepository<TransportPrice> priceRepository,
			IEntityRepository<TransportTitle> titleRepository,
			TransportOperationConfirmHandler confirmHandler,
			SigapuntService sigapuntService,
			IEntityRepository<TransportOperation> transportOperationRepository,
			IEntityRepository<TransportOperationTitle> transportOperationTitleRepository,
			IUnitOfWork unitOfWork,
			IEntityRepository<PromoExecution> promotionExecutionsRepository
		) : base(sessionData, eigeService, tescService, sigapuntService, hsm, greyListRepository, blackListRepository, priceRepository, titleRepository, transportOperationTitleRepository, transportOperationRepository, unitOfWork, promotionExecutionsRepository)
		{
			if (repository == null) throw new ArgumentNullException("respository");
			if (confirmHandler == null) throw new ArgumentNullException("confirmHandler");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (sigapuntService == null) throw new ArgumentNullException("sigapuntService");

			Repository = repository;
			SigapuntService = sigapuntService;
			ConfirmHandler = confirmHandler;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportOperationConfirmAndReadInfoArguments arguments)
        {
#if DEBUG
            var watch = Stopwatch.StartNew();
#endif
            var confim = await ConfirmHandler.ExecuteAsync(new TransportOperationConfirmArguments(
				arguments.OperationId,
				arguments.MobileSerial
#if DEBUG || TEST || HOMO
				, arguments.Now
#endif
			));
#if DEBUG
            watch.Stop();
            Debug.WriteLine(METHOD_NAME + " - Confirm: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
            watch = Stopwatch.StartNew();
#endif

			var result = await base.ExecuteAsync(arguments);
#if DEBUG
            watch.Stop();
            Debug.WriteLine(METHOD_NAME + " - ReadInfo: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
            watch = Stopwatch.StartNew();
#endif

            return result;
		}
		#endregion ExecuteAsync
	}
}