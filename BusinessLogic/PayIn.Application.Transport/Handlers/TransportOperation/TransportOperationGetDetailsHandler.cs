using PayIn.Application.Dto.Results;
using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Application.Dto.Transport.Results.TransportOperation;
using PayIn.Application.Transport.Scripts;
using PayIn.Application.Transport.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige;
using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.MifareClassic.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Domain.Transport;
using Xp.Domain.Transport.MifareClassic;
using static PayIn.Application.Public.Handlers.TransportCardSearchInternalHandler;

namespace PayIn.Application.Public.Handlers
{
	public class TransportOperationGetDetailsHandler :
		IQueryBaseHandler<TransportOperationGetDetailsArguments, TransportOperationGetDetailsResult>
	{

		public class Device
		{
			public string Model { get; set; }
			public string Platform { get; set; }
			public string Uuid { get; set; }
			public string Version { get; set; }
			public string Manufacturer { get; set; }
			public string Serial { get; set; }
			public string Operator { get; set; }
			public string Imei { get; set; }
			public string Mac { get; set; }
		}
		private readonly IEntityRepository<Log> LogRepository;
		private readonly IEntityRepository<TransportTitle> TitleRepository;
		private readonly IEntityRepository<GreyList> GreyListRepository;
		private readonly IEntityRepository<Ticket> TicketRepository;
		private readonly EigeService EigeService;
		private readonly SessionData SessionData;
		private readonly IMifareClassicHsmService Hsm;
		private readonly IEntityRepository<TransportOperation> TransportOperationRepository;

		#region Constructors
		public TransportOperationGetDetailsHandler(
			IEntityRepository<Log> logRepository,
			IEntityRepository<TransportTitle> titleRepository,
			IEntityRepository<GreyList> greyListRepository,
			IEntityRepository<Ticket> ticketRepository,
		    EigeService eigeService,
		    SessionData sessionData,
			IMifareClassicHsmService hsm,
			IEntityRepository<TransportOperation> transportOperationRepository
		)
		{
			if (logRepository == null) throw new ArgumentNullException("logRepository");
			if (titleRepository == null) throw new ArgumentNullException("titleRepository");
			if (greyListRepository == null) throw new ArgumentNullException("greyListRepository");
			if (ticketRepository == null) throw new ArgumentNullException("ticketRepository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (hsm == null) throw new ArgumentNullException("hsm");
			if (eigeService == null) throw new ArgumentNullException("eigeService");
			if (transportOperationRepository == null) throw new ArgumentNullException("transportOperationRepository");


			LogRepository = logRepository;
			TitleRepository = titleRepository;
			TicketRepository = ticketRepository;
			GreyListRepository = greyListRepository;
			EigeService = eigeService;
			SessionData = sessionData;
			Hsm = hsm;
			TransportOperationRepository = transportOperationRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TransportOperationGetDetailsResult>> ExecuteAsync(TransportOperationGetDetailsArguments arguments)
		{
			var items = (await TransportOperationRepository.GetAsync())
			.Where(x =>
				(x.Id == arguments.Id || x.Uid == arguments.Id) &&
				x.Script != "" && x.Error == "" && x.OperationType == OperationType.Read
			);

			long uid = (long)items.FirstOrDefault().Uid;

			var now = DateTime.Now.ToUTC();

			var greyList = (await GreyListRepository.GetAsync())
				.Where(x => x.Uid == uid && x.State == GreyList.GreyListStateType.Active)
				.OrderByDescending(x => x.RegistrationDate)
				.Select(x => new TransportOperationGetDetailsResultBase.GreyLists
				{
					Id = x.Id,
					RegistrationDate = x.RegistrationDate,
					Action = x.Action,
					Field = x.Field,
					NewValue = x.NewValue,
					Resolved = x.Resolved,
					ResolutionDate = x.ResolutionDate,
					Source = x.Source,
					OperationNumber = x.OperationNumber,
					State = x.State
				})
				.AsEnumerable();

			var result = items
				.Select(x => new
				{
					Date = x.OperationDate,
					x.Id,
					x.Uid,
					Action = x.OperationType,
					ScriptRequest = x.Script
				})
				.Where(x => x.Id == items.OrderByDescending(y => y.Id).FirstOrDefault().Id)
				.OrderByDescending(x => x.Date)
				.ToList()
				.Select(x => new
				{
					x.Date,
					x.Id,
					x.Uid,
					x.Action,
					ScriptRequestReadInfo = GetScriptReadInfo(x.Action, x.ScriptRequest),
					ScriptRequestSearch = GetScriptSearch(x.Action, x.ScriptRequest)
				})
				.FirstOrDefault();

			var script = new TransportCardGetReadInfoScript(SessionData.Login, Hsm, result.ScriptRequestSearch);

            #region Cargar support
            var transportCardSupport = await EigeService.GetSupportCardAsync(uid, script, now);
            #endregion Cargar support

            var codigo1 = await EigeService.GetTitleNameAsync((int)await EigeService.GetTitleCode1Async(uid, script));
			var codigo2 = await EigeService.GetTitleNameAsync((int)await EigeService.GetTitleCode2Async(uid, script));

			#region Cargar titulos / monedero / bonus			
			var titleCode1 = await EigeService.GetTitleCode1Async(uid, script);
			var titleZone1 = await EigeService.GetTitleZoneName1Async(uid, script);

			var titleCode2 = await EigeService.GetTitleCode2Async(uid, script);
			var titleZone2 = await EigeService.GetTitleZoneName2Async(uid, script);

			var list = new List<TransportCardReadInfoResultTemp>();
			if ((await EigeService.GetTitleActive1Async(uid, script)) == true)
			{
				var item = new TransportCardReadInfoResultTemp
				{
					Code = titleCode1,
					Name = await EigeService.GetTitleName1Async(uid, script),
					OwnerName = await EigeService.GetTitleOwnerName1Async(uid, script),
					Zone = titleZone1,
					Caducity = await EigeService.GetTitleCaducity1Async(uid, script),
					IsRechargable = await EigeService.IsRechargable1Async(uid, script, transportCardSupport, now),
					HasTariff = await EigeService.HasTariff1Async(uid, script),
					IsExhausted = await EigeService.GetTitleIsExhausted1Async(uid, script, now),
					IsExpired = await EigeService.GetTitleIsExpired1Async(uid, script, now),
					MaxExternalTransfers = await EigeService.GetTitleMaxExternalTransfers1Async(uid, script),
					MaxPeopleInTransfer = await EigeService.GetTitleMaxPeopleInTransfer1Async(uid, script),
					// Balance
					HasBalance = await EigeService.GetTitleHasBalance1Async(uid, script),
					Balance = await EigeService.GetTitleBalance1Async(uid, script),
					//BalanceAcumulated = await EigeService.GetTitleBalanceAcumulated1Async(uid, sigapuntScript),
					BalanceUnits = await EigeService.GetTitleBalanceUnits1Async(uid, script),
					//Temporal
					IsTemporal = await EigeService.GetTitleIsTemporal1Async(uid, script),
					ExhaustedDate = (await EigeService.GetTitleExhaustedDate1Async(uid, script)).ToUTC(),
					ActivatedDate = (await EigeService.GetTitleActivatedDate1Async(uid, script)).ToUTC(),
					Ampliation = await EigeService.GetTitleAmpliation1Async(uid, script),
					AmpliationQuantity = await EigeService.GetTitleAmpliationQuantity1Async(uid, script),
					AmpliationUnits = await EigeService.GetTitleAmpliationUnits1Async(uid, script)
				};
				list.Add(item);
			}
			if ((await EigeService.GetTitleActive2Async(uid, script)) == true)
			{
				var item = new TransportCardReadInfoResultTemp
				{
					Code = titleCode2,
					Name = await EigeService.GetTitleName2Async(uid, script),
					OwnerName = await EigeService.GetTitleOwnerName2Async(uid, script),
					Zone = titleZone2,
					Caducity = await EigeService.GetTitleCaducity2Async(uid, script),
					IsRechargable = await EigeService.IsRechargable2Async(uid, script, transportCardSupport, now),
					HasTariff = await EigeService.HasTariff2Async(uid, script),
					IsExhausted = await EigeService.GetTitleIsExhausted2Async(uid, script, now),
					MaxExternalTransfers = await EigeService.GetTitleMaxExternalTransfers2Async(uid, script),
					MaxPeopleInTransfer = await EigeService.GetTitleMaxPeopleInTransfer2Async(uid, script),
					// Balance
					HasBalance = await EigeService.GetTitleHasBalance2Async(uid, script),
					Balance = await EigeService.GetTitleBalance2Async(uid, script),
					//BalanceAcumulated = await EigeService.GetTitleBalanceAcumulated1Async(uid, sigapuntScript),
					BalanceUnits = await EigeService.GetTitleBalanceUnits2Async(uid, script),
					//Temporal
					IsTemporal = await EigeService.GetTitleIsTemporal2Async(uid, script),
					ExhaustedDate = (await EigeService.GetTitleExhaustedDate2Async(uid, script)).ToUTC(),
					ActivatedDate = (await EigeService.GetTitleActivatedDate2Async(uid, script)).ToUTC(),
					Ampliation = await EigeService.GetTitleAmpliation2Async(uid, script),
					AmpliationQuantity = await EigeService.GetTitleAmpliationQuantity2Async(uid, script),
					AmpliationUnits = await EigeService.GetTitleAmpliationUnits2Async(uid, script)
				};
				list.Add(item);
			}
			if ((await EigeService.GetTitleActiveMAsync(uid, script)) == true)
			{
				var item = new TransportCardReadInfoResultTemp
				{
					Code = await EigeService.GetTitleCodeMAsync(uid, script),
					Name = await EigeService.GetTitleNameMAsync(uid, script),
					OwnerName = await EigeService.GetTitleOwnerNameMAsync(uid, script),
					Zone = await EigeService.GetTitleZoneNameMAsync(uid, script),
					Caducity = await EigeService.GetTitleCaducityMAsync(uid, script),
					IsRechargable = await EigeService.IsRechargableMAsync(uid, script, transportCardSupport, now),
					HasTariff = await EigeService.HasTariffMAsync(uid, script),
					IsExhausted = await EigeService.GetTitleIsExhaustedMAsync(uid, script, now),
					MaxExternalTransfers = await EigeService.GetTitleMaxExternalTransfersMAsync(uid, script),
					MaxPeopleInTransfer = await EigeService.GetTitleMaxPeopleInTransferMAsync(uid, script),
					// Balance
					HasBalance = true,
					Balance = await EigeService.GetTitleBalanceMAsync(uid, script),
					BalanceUnits = await EigeService.GetTitleBalanceUnitsMAsync(uid, script),
					// Temporal
					IsTemporal = await EigeService.GetTitleIsTemporalMAsync(uid, script),
					ExhaustedDate = (await EigeService.GetTitleExhaustedDateMAsync(uid, script)).ToUTC(),
					ActivatedDate = (await EigeService.GetTitleActivatedDateMAsync(uid, script)).ToUTC(),
					Ampliation = await EigeService.GetTitleAmpliationMAsync(uid, script),
					AmpliationQuantity = await EigeService.GetTitleAmpliationQuantityMAsync(uid, script),
					AmpliationUnits = await EigeService.GetTitleAmpliationUnitsMAsync(uid, script)
				};
				list.Add(item);
			}
			if ((await EigeService.GetTitleActiveBAsync(uid, script)) == true)
			{
				var item = new TransportCardReadInfoResultTemp
				{
					Code = await EigeService.GetTitleCodeBAsync(uid, script),
					Name = await EigeService.GetTitleNameBAsync(uid, script),
					OwnerName = await EigeService.GetTitleOwnerNameBAsync(uid, script),
					Zone = await EigeService.GetTitleZoneNameBAsync(uid, script),
					Caducity = await EigeService.GetTitleCaducityBAsync(uid, script),
					IsRechargable = await EigeService.IsRechargableBAsync(uid, script, transportCardSupport, now),
					HasTariff = await EigeService.HasTariffBAsync(uid, script),
					IsExhausted = await EigeService.GetTitleIsExhaustedBAsync(uid, script, now),
					MaxExternalTransfers = await EigeService.GetTitleMaxExternalTransfersBAsync(uid, script),
					MaxPeopleInTransfer = await EigeService.GetTitleMaxPeopleInTransferBAsync(uid, script),
					// Balance
					HasBalance = true,
					Balance = await EigeService.GetTitleBalanceBAsync(uid, script),
					BalanceUnits = await EigeService.GetTitleBalanceUnitsBAsync(uid, script),
					// Temporal
					IsTemporal = await EigeService.GetTitleIsTemporalBAsync(uid, script),
					ExhaustedDate = (await EigeService.GetTitleExhaustedDateBAsync(uid, script)).ToUTC(),
					ActivatedDate = (await EigeService.GetTitleActivatedDateBAsync(uid, script)).ToUTC(),
					Ampliation = await EigeService.GetTitleAmpliationBAsync(uid, script),
					AmpliationQuantity = await EigeService.GetTitleAmpliationQuantityBAsync(uid, script),
					AmpliationUnits = await EigeService.GetTitleAmpliationUnitsBAsync(uid, script)
				};
				list.Add(item);
			}
			#endregion Cargar titulos / monedero / bonus	

			#region Cargar Historico Validaciones
			// HistoricoFechaHora
			var historicoDate1 = await EigeService.GetHistoricoDate1Async(uid, script);
			var historicoDate2 = await EigeService.GetHistoricoDate2Async(uid, script);
			var historicoDate3 = await EigeService.GetHistoricoDate3Async(uid, script);
			var historicoDate4 = await EigeService.GetHistoricoDate4Async(uid, script);
			var historicoDate5 = await EigeService.GetHistoricoDate5Async(uid, script);
			var historicoDate6 = await EigeService.GetHistoricoDate6Async(uid, script);
			var historicoDate7 = await EigeService.GetHistoricoDate7Async(uid, script);
			var historicoDate8 = await EigeService.GetHistoricoDate8Async(uid, script);

			// HistoricoTyeName
			var historicoTypeName1 = await EigeService.GetHistoricoTypeName1Async(uid, script);
			var historicoTypeName2 = await EigeService.GetHistoricoTypeName2Async(uid, script);
			var historicoTypeName3 = await EigeService.GetHistoricoTypeName3Async(uid, script);
			var historicoTypeName4 = await EigeService.GetHistoricoTypeName4Async(uid, script);
			var historicoTypeName5 = await EigeService.GetHistoricoTypeName5Async(uid, script);
			var historicoTypeName6 = await EigeService.GetHistoricoTypeName6Async(uid, script);
			var historicoTypeName7 = await EigeService.GetHistoricoTypeName7Async(uid, script);
			var historicoTypeName8 = await EigeService.GetHistoricoTypeName8Async(uid, script);

			// HistoricoCode
			var historicoCode1 = await EigeService.GetHistoricoCode1Async(uid, script);
			var historicoCode2 = await EigeService.GetHistoricoCode2Async(uid, script);
			var historicoCode3 = await EigeService.GetHistoricoCode3Async(uid, script);
			var historicoCode4 = await EigeService.GetHistoricoCode4Async(uid, script);
			var historicoCode5 = await EigeService.GetHistoricoCode5Async(uid, script);
			var historicoCode6 = await EigeService.GetHistoricoCode6Async(uid, script);
			var historicoCode7 = await EigeService.GetHistoricoCode7Async(uid, script);
			var historicoCode8 = await EigeService.GetHistoricoCode8Async(uid, script);

			// HistoricoZone
			var historicoZone1 = await EigeService.GetHistoricoZone1Async(uid, script);
			var historicoZone2 = await EigeService.GetHistoricoZone2Async(uid, script);
			var historicoZone3 = await EigeService.GetHistoricoZone3Async(uid, script);
			var historicoZone4 = await EigeService.GetHistoricoZone4Async(uid, script);
			var historicoZone5 = await EigeService.GetHistoricoZone5Async(uid, script);
			var historicoZone6 = await EigeService.GetHistoricoZone6Async(uid, script);
			var historicoZone7 = await EigeService.GetHistoricoZone7Async(uid, script);
			var historicoZone8 = await EigeService.GetHistoricoZone8Async(uid, script);

			// HistoricoPlace
			var historicoPlace1 = await EigeService.GetHistoricoPlace1Async(uid, script);
			var historicoPlace2 = await EigeService.GetHistoricoPlace2Async(uid, script);
			var historicoPlace3 = await EigeService.GetHistoricoPlace3Async(uid, script);
			var historicoPlace4 = await EigeService.GetHistoricoPlace4Async(uid, script);
			var historicoPlace5 = await EigeService.GetHistoricoPlace5Async(uid, script);
			var historicoPlace6 = await EigeService.GetHistoricoPlace6Async(uid, script);
			var historicoPlace7 = await EigeService.GetHistoricoPlace7Async(uid, script);
			var historicoPlace8 = await EigeService.GetHistoricoPlace8Async(uid, script);

			// HistoricoPlace
			var historicoOperator1 = await EigeService.GetHistoricoOperator1Async(uid, script);
			var historicoOperator2 = await EigeService.GetHistoricoOperator2Async(uid, script);
			var historicoOperator3 = await EigeService.GetHistoricoOperator3Async(uid, script);
			var historicoOperator4 = await EigeService.GetHistoricoOperator4Async(uid, script);
			var historicoOperator5 = await EigeService.GetHistoricoOperator5Async(uid, script);
			var historicoOperator6 = await EigeService.GetHistoricoOperator6Async(uid, script);
			var historicoOperator7 = await EigeService.GetHistoricoOperator7Async(uid, script);
			var historicoOperator8 = await EigeService.GetHistoricoOperator8Async(uid, script);

			var last = await EigeService.GetHistoricoIndiceAsync(uid, script);
			var numHistoricos = (await EigeService.GetHistoricoTiene8Async(uid, script) == true) ? 8 : 4;
			var log = new ServiceCardReadInfoResult_Log[numHistoricos];
			if (numHistoricos > 0)
			{
				var title = list
					.Where(x => x.Code == script.Card.Historico.CodigoTitulo1.Value)
					.FirstOrDefault();

				// Historico1
				log[0] = (historicoDate1 == null) ? null : new ServiceCardReadInfoResult_Log
				{
					Date = historicoDate1,
					TypeName = historicoTypeName1,
					TitleName = await EigeService.GetHistoricoName1Async(uid, script),
					TitleOwnerName = title != null ? title.OwnerName : "",
					TitleZone = title != null ? title.Zone : (EigeZonaEnum?)null,
					Code = historicoCode1,
					Zone = historicoZone1,
					Quantity = await EigeService.GetHistoricoQuantity1Async(uid, script),
					QuantityUnits = await EigeService.GetHistoricoQuantityUnits1Async(uid, script),
					HasBalance = title != null ? title.HasBalance : true,
					Place = historicoPlace1,
					Operator = historicoOperator1
				};
			}
			if (numHistoricos > 1)
			{
				var title = list
					.Where(x => x.Code == script.Card.Historico.CodigoTitulo2.Value)
					.FirstOrDefault();

				// Historico2
				log[1] = (historicoDate2 == null) ? null : new ServiceCardReadInfoResult_Log
				{
					Date = historicoDate2,
					TypeName = historicoTypeName2,
					TitleName = await EigeService.GetHistoricoName2Async(uid, script),
					TitleOwnerName = title != null ? title.OwnerName : "",
					TitleZone = title != null ? title.Zone : (EigeZonaEnum?)null,
					Code = historicoCode2,
					Zone = historicoZone2,
					Quantity = await EigeService.GetHistoricoQuantity2Async(uid, script),
					QuantityUnits = await EigeService.GetHistoricoQuantityUnits2Async(uid, script),
					HasBalance = title != null ? title.HasBalance : true,
					Place = historicoPlace2,
					Operator = historicoOperator2
				};
			}
			if (numHistoricos > 2)
			{
				var title = list
					.Where(x => x.Code == script.Card.Historico.CodigoTitulo3.Value)
					.FirstOrDefault();

				// Historico3
				log[2] = (historicoDate3 == null) ? null : new ServiceCardReadInfoResult_Log
				{
					Date = historicoDate3,
					TypeName = historicoTypeName3,
					TitleName = await EigeService.GetHistoricoName3Async(uid, script),
					TitleOwnerName = title != null ? title.OwnerName : "",
					TitleZone = title != null ? title.Zone : (EigeZonaEnum?)null,
					Code = historicoCode3,
					Zone = historicoZone3,
					Quantity = await EigeService.GetHistoricoQuantity3Async(uid, script),
					QuantityUnits = await EigeService.GetHistoricoQuantityUnits3Async(uid, script),
					HasBalance = title != null ? title.HasBalance : true,
					Place = historicoPlace3,
					Operator = historicoOperator3
				};
			}
			if (numHistoricos > 3)
			{
				var title = list
					.Where(x => x.Code == script.Card.Historico.CodigoTitulo4.Value)
					.FirstOrDefault();

				// Historico4
				log[3] = (historicoDate4 == null) ? null : new ServiceCardReadInfoResult_Log
				{
					Date = historicoDate4,
					TypeName = historicoTypeName4,
					TitleName = await EigeService.GetHistoricoName4Async(uid, script),
					TitleOwnerName = title != null ? title.OwnerName : "",
					TitleZone = title != null ? title.Zone : (EigeZonaEnum?)null,
					Code = historicoCode4,
					Zone = historicoZone4,
					Quantity = await EigeService.GetHistoricoQuantity4Async(uid, script),
					QuantityUnits = await EigeService.GetHistoricoQuantityUnits4Async(uid, script),
					HasBalance = title != null ? title.HasBalance : true,
					Place = historicoPlace4,
					Operator = historicoOperator4
				};
			}
			if (numHistoricos > 4)
			{
				var title = list
					.Where(x => x.Code == script.Card.Historico.CodigoTitulo5.Value)
					.FirstOrDefault();

				// Historico5
				log[4] = (historicoDate5 == null) ? null : new ServiceCardReadInfoResult_Log
				{
					Date = historicoDate5,
					TypeName = historicoTypeName5,
					TitleName = await EigeService.GetHistoricoName5Async(uid, script),
					TitleOwnerName = title != null ? title.OwnerName : "",
					TitleZone = title != null ? title.Zone : (EigeZonaEnum?)null,
					Code = historicoCode5,
					Zone = historicoZone5,
					Quantity = await EigeService.GetHistoricoQuantity5Async(uid, script),
					QuantityUnits = await EigeService.GetHistoricoQuantityUnits5Async(uid, script),
					HasBalance = title != null ? title.HasBalance : true,
					Place = historicoPlace5,
					Operator = historicoOperator5
				};
			}
			if (numHistoricos > 5)
			{
				var title = list
					.Where(x => x.Code == script.Card.Historico.CodigoTitulo6.Value)
					.FirstOrDefault();

				// Historico6
				log[5] = (historicoDate6 == null) ? null : new ServiceCardReadInfoResult_Log
				{
					Date = historicoDate6,
					TypeName = historicoTypeName6,
					TitleName = await EigeService.GetHistoricoName6Async(uid, script),
					TitleOwnerName = title != null ? title.OwnerName : "",
					TitleZone = title != null ? title.Zone : (EigeZonaEnum?)null,
					Code = historicoCode6,
					Zone = historicoZone6,
					Quantity = await EigeService.GetHistoricoQuantity6Async(uid, script),
					QuantityUnits = await EigeService.GetHistoricoQuantityUnits6Async(uid, script),
					HasBalance = title != null ? title.HasBalance : true,
					Place = historicoPlace6,
					Operator = historicoOperator6
				};
			}
			if (numHistoricos > 6)
			{
				var title = list
					.Where(x => x.Code == script.Card.Historico.CodigoTitulo7.Value)
					.FirstOrDefault();

				// Historico7
				log[6] = (historicoDate7 == null) ? null : new ServiceCardReadInfoResult_Log
				{
					Date = historicoDate7,
					TypeName = historicoTypeName7,
					TitleName = await EigeService.GetHistoricoName7Async(uid, script),
					TitleOwnerName = title != null ? title.OwnerName : "",
					TitleZone = title != null ? title.Zone : (EigeZonaEnum?)null,
					Code = historicoCode7,
					Zone = historicoZone7,
					Quantity = await EigeService.GetHistoricoQuantity7Async(uid, script),
					QuantityUnits = await EigeService.GetHistoricoQuantityUnits7Async(uid, script),
					HasBalance = title != null ? title.HasBalance : true,
					Place = historicoPlace7,
					Operator = historicoOperator7
				};
			}
			if (numHistoricos > 7)
			{
				var title = list
					.Where(x => x.Code == script.Card.Historico.CodigoTitulo8.Value)
					.FirstOrDefault();

				// Historico8
				log[7] = (historicoDate8 == null) ? null : new ServiceCardReadInfoResult_Log
				{
					Date = historicoDate8,
					TypeName = historicoTypeName8,
					TitleName = await EigeService.GetHistoricoName8Async(uid, script),
					TitleOwnerName = title != null ? title.OwnerName : "",
					TitleZone = title != null ? title.Zone : (EigeZonaEnum?)null,
					Code = historicoCode8,
					Zone = historicoZone8,
					Quantity = await EigeService.GetHistoricoQuantity8Async(uid, script),
					QuantityUnits = await EigeService.GetHistoricoQuantityUnits8Async(uid, script),
					HasBalance = title != null ? title.HasBalance : true,
					Place = historicoPlace8,
					Operator = historicoOperator8
				};
			}

			// Limpiar y ordenar
			log = log
				.Where(x => x != null)
				.OrderByDescending(x => x.Date)
				.ToArray();
			var historical = log.Take(4);
			#endregion Cargar Historico Validaciones

			var lastValidation = log.FirstOrDefault();
			var lastValidationTitle = list
				.Where(x =>
					lastValidation != null &&
					x.Code == lastValidation.Code)
				.FirstOrDefault();

			#region Cargar Historico Cargas
			// CargaDate
			var cargaDate1 = await EigeService.GetCargaDate1Async(uid, script);
			var cargaDate2 = await EigeService.GetCargaDate2Async(uid, script);

			// CargaFecha
			var cargaTypeName1 = await EigeService.GetCargaTypeName1Async(uid, script);
			var cargaTypeName2 = await EigeService.GetCargaTypeName2Async(uid, script);

			// CargaFecha
			var cargaTitleName1 = await EigeService.GetCargaTitleName1Async(uid, script);
			var cargaTitleName2 = await EigeService.GetCargaTitleName2Async(uid, script);

			// CargaQuantity
			var cargaQuantity1 = await EigeService.GetCargaQuantity1Async(uid, script);
			var cargaQuantity2 = await EigeService.GetCargaQuantity2Async(uid, script);

			var lastCharge = script.Card.Carga.PosicionUltima;
			var charges = new ServiceCardReadInfoResult_Charge[2];

			// Carga1
			{
				var title = list
					.Where(x => x.Code == script.Card.Carga.CodigoTitulo1.Value)
					.FirstOrDefault();

				charges[0] = (cargaDate1 == null) ? null : new ServiceCardReadInfoResult_Charge
				{
					Date = cargaDate1, // No hay que invocar a ToUTC() porque ya está en horario local del móvil
					TypeName = cargaTypeName1,
					TitleName = cargaTitleName1,
					TitleOwnerName = title != null ? title.OwnerName : "",
					TitleZone = title != null ? title.Zone : (EigeZonaEnum?)null,
					Quantity = cargaQuantity1
				};
			}

			// Carga2
			{
				var title = list
					.Where(x => x.Code == script.Card.Carga.CodigoTitulo2.Value)
					.FirstOrDefault();

				charges[1] =
					(cargaDate2 == null) ? null : new ServiceCardReadInfoResult_Charge
					{
						Date = cargaDate2, // No hay que invocar a ToUTC() porque ya está en horario local del móvil
						TypeName = cargaTypeName2,
						TitleName = cargaTitleName2,
						TitleOwnerName = title != null ? title.OwnerName : "",
						TitleZone = title != null ? title.Zone : (EigeZonaEnum?)null,
						Quantity = cargaQuantity2
					};
			}
			#endregion Cargar Historico Cargas		

			var json = (items.FirstOrDefault().Device == null || items.FirstOrDefault().Device == "") ? null : items.FirstOrDefault().Device.FromJson();
			var device = new List<TransportOperationGetDetailsHandler.Device>();

			#region Device
			if (json != null)
			{
				var element = new TransportOperationGetDetailsHandler.Device
				{
					Model = json.model,
					Platform = json.platform,
					Uuid = json.uuid,
					Version = json.version,
					Manufacturer = json.manufacturer,
					Serial = json.serial,
					Operator = json.Operator,
					Imei = json.imei,
					Mac = json.mac
				};
				device.Add(element);
			}
			#endregion

			
			return new TransportOperationGetDetailsResultBase
			{
				Id = result.Id,
				Uid = result.Uid,
				Date = result.Date,
				Action = result.Action,			
				Hexadecimal = result.ScriptRequestReadInfo == null ? "" : GetScriptString(result.ScriptRequestReadInfo),	
				Device = device,			
				Data = new TransportOperationGetDetailsResult[] {
					new TransportOperationGetDetailsResult {
						Values = new{
							script.Card.Carga,
							script.Card.Emision,
							script.Card.Historico,
							script.Card.Inspeccion,
							script.Card.Personalizacion,
							script.Card.Produccion,
							script.Card.Tarjeta,
							script.Card.Titulo,
							script.Card.Usuario,
							script.Card.Validacion
						},
						TuiN = result.ScriptRequestReadInfo == null ? null : script.Card.TituloTuiN,
						Historical = historical,						
						GreyList = greyList,
						Titulo1 =  codigo1,
						Titulo2 =  codigo2,
						Owner = await EigeService.GetCardOwnerAsync(uid, script),
						OwnerName = await EigeService.GetCardOwnerNameAsync(uid, script),
						TypeName = await EigeService.GetCardTypeNameAsync(uid, script),
						InBlackList = await EigeService.InBlackListAsync(uid, script),
						ExpiredDate = await EigeService.GetExpiredDateAsync(uid, script), // No hay que invocar a ToUTC() porque ya está en horario local del móvil
						IsExpired = await EigeService.IsCardExpiredAsync(uid, script, transportCardSupport, now),
						IsDamaged = await EigeService.IsDamagedAsync(uid, script),
						IsRechargable =
                            (await EigeService.IsRechargable1Async(uid, script, transportCardSupport, now)) ||
                            (await EigeService.IsRechargable2Async(uid, script, transportCardSupport, now)) ||
                            (await EigeService.IsRechargableMAsync(uid, script, transportCardSupport, now)) ||
                            (await EigeService.IsRechargableBAsync(uid, script, transportCardSupport, now)),
						HasHourValidity = (await EigeService.HasHourValidity1Async(uid, script)) || (await EigeService.HasHourValidity2Async(uid, script)),
					    HasDayValidity = (await EigeService.HasDayValidity1Async(uid, script)) || (await EigeService.HasDayValidity2Async(uid, script)),
						// Personalization
						IsPersonalized = await EigeService.IsPersonalizedAsync(uid, script),
						UserName = await EigeService.GetUserNameAsync(uid, script),
						UserSurname = await EigeService.GetUserSurnameAsync(uid, script),
						UserDni = await EigeService.GetUserDniAsync(uid, script),
						UserCode = await EigeService.GetUserCodeAsync(uid, script),							
						Mode = EigeService.TransferMode,
						DeviceType = await EigeService.GetDeviceTypeAsync(uid, script),
					    // LastValidation
						PeopleTraveling = await EigeService.GetLastValidationPeopleTravelingAsync(uid, script, lastValidation == null ? null : lastValidation.Code),
						LastValidationDate = await EigeService.GetLastValidationDateAsync(uid, script, lastValidation == null ? null : lastValidation.Code),
						LastValidationTypeName = await EigeService.GetLastValidationTypeNameAsync(uid, script, lastValidation == null ? null : lastValidation.Code),
						LastValidationPlace = await EigeService.GetLastValidationPlaceAsync(uid, script, lastValidation == null ? null : lastValidation.Code),
						LastValidationOperator = await EigeService.GetLastValidationOperatorAsync(uid, script),
						LastValidationZone = lastValidation == null ? null : lastValidation.Zone,
						LastValidationTitleName = lastValidation == null ? "" : lastValidation.TitleName,
						LastValidationTitleOwnerName = lastValidation == null ? "" : lastValidation.TitleOwnerName,
						LastValidationTitleZone = lastValidation == null ? null : lastValidation.TitleZone,
						Charges = charges,
						ScriptRequest = result.ScriptRequestReadInfo == null ? "" : GetScriptString(result.ScriptRequestReadInfo)
					}
				}
			};
		}
		#endregion ExecuteAsync

		#region GetScriptReadInfo
		private MifareClassicScript<EigeCard> GetScriptReadInfo(OperationType relatedMethod, string scriptRequest)
		{
			if (relatedMethod != OperationType.Read)
				return null;

			var arguments = scriptRequest.FromJson();
			List<MifareOperationResultArguments> args = arguments.ToObject<List<MifareOperationResultArguments>>();
			var card = new EigeCard(SessionData.Login, Hsm);
			var script = new MifareClassicScript<EigeCard>(card);			
			script.Load(args);			
			return script;
		}
		#endregion GetScriptReadInfo
		
		#region GetScriptSearch
		private List<MifareOperationResultArguments> GetScriptSearch(OperationType relatedMethod, string scriptRequest)
		{
			if (relatedMethod != OperationType.Read)
				return null;

			var arguments = scriptRequest.FromJson();
			List<MifareOperationResultArguments> args = arguments.ToObject<List<MifareOperationResultArguments>>();			
			return args;
		}
		#endregion GetScriptSearch

		#region GetScriptString
		private string GetScriptString(MifareClassicScript<EigeCard> script)
		{
			var result = script.Card.Sectors
				.SelectMany(x => x.Blocks)
				.Select(x => x.Value == null ? "" : x.Value.ToHexadecimal())
				.ToList();

			for (int i = 0; i < 16; i++)
				result[i * 4 + 3] = "-----------Sector " + i + "-----------";

			return result
				.JoinString("\n");
		}
		#endregion GetScriptString
	}
}
