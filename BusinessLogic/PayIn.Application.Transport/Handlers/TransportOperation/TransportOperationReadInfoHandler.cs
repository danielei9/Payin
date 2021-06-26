using PayIn.Application.Dto.Results;
using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Application.Transport.Scripts;
using PayIn.Application.Transport.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Promotions;
using PayIn.Domain.Security;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige;
using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using PayIn.Infrastructure.Transport.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Application.Results;
using Xp.Domain;
using Xp.Domain.Transport;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Handlers
{
    [XpLog("TransportOperation", "ReadInfo", RelatedId = "OperationId")]
	[XpAnalytics("TransportOperation", "ReadInfo", Response = new[] { "Data[0].Code", "Data[0].Name", "Data[1].Code", "Data[1].Name" })]
	public class TransportOperationReadInfoHandler :
		IQueryBaseHandler<TransportOperationReadInfoArguments, ServiceCardReadInfoResult>
	{
		private const string METHOD_NAME = "TransportOperation_ReadInfo";

		public class TransportCardReadInfoResultTemp : ServiceCardReadInfoResult
		{
			public int? MaxExternalTransfers { get; set; }
			public int? MaxPeopleInTransfer { get; set; }
		}
		public class RechargeConfig
		{
			public decimal ChangePrice { get; set; }
			public RechargeType RechargeType { get; set; }
			public EigeTituloEnUsoEnum Slot { get; set; }
		}

		private readonly ISessionData SessionData;
		private readonly EigeService EigeService;
		private readonly TescService TescService;
		private readonly IMifareClassicHsmService Hsm;
		private readonly IEntityRepository<GreyList> GreyListRepository;
        private readonly IEntityRepository<TransportCardSupport> TransportCardSupportRepository;
        private readonly IEntityRepository<TransportPrice> PriceRepository;
        private readonly IEntityRepository<TransportTitle> TitleRepository;
		private readonly IEntityRepository<BlackList> BlackListRepository;
		private readonly IEntityRepository<TransportOperation> TransportOperationRepository;
		private readonly IEntityRepository<TransportOperationTitle> TransportOperationTitleRepository;
		private readonly SigapuntService SigapuntService;
		private readonly IUnitOfWork UnitOfWork;
		private readonly IEntityRepository<PromoExecution> PromotionExecutionsRepository;

		#region Constructors
		public TransportOperationReadInfoHandler(
			ISessionData sessionData,
			EigeService eigeService,
			TescService tescService,
			SigapuntService sigapuntService,
			IMifareClassicHsmService hsm,
			IEntityRepository<GreyList> greyListRepository,
			IEntityRepository<BlackList> blackListRepository,
			IEntityRepository<TransportPrice> priceRepository,
			IEntityRepository<TransportTitle> titleRepository,
			IEntityRepository<TransportOperationTitle> transportoperationtitleRepository,
			IEntityRepository<TransportOperation> transportOperationRepository,
			IUnitOfWork unitOfWork,
			IEntityRepository<PromoExecution> promotionExecutionsRepository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (eigeService == null) throw new ArgumentNullException("eigeService");
			if (tescService == null) throw new ArgumentNullException("tescService");
			if (sigapuntService == null) throw new ArgumentNullException("sigapuntService");
			if (hsm == null) throw new ArgumentNullException("hsm");
			if (greyListRepository == null) throw new ArgumentNullException("greyListRepository");
			if (blackListRepository == null) throw new ArgumentNullException("blackListRepository");
			if (priceRepository == null) throw new ArgumentNullException("priceRepository");
			if (titleRepository == null) throw new ArgumentNullException("titleRepository");
			if (transportOperationRepository == null) throw new ArgumentNullException("transportOperationRepository");
			if (transportoperationtitleRepository == null) throw new ArgumentNullException("transportoperationtitleRepository");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (promotionExecutionsRepository == null) throw new ArgumentNullException("promotionExecutionsRepository");

			SessionData = sessionData;
			EigeService = eigeService;
			TescService = tescService;
			SigapuntService = sigapuntService;
			Hsm = hsm;
			GreyListRepository = greyListRepository;
			BlackListRepository = blackListRepository;
			PriceRepository = priceRepository;
			TitleRepository = titleRepository;
			TransportOperationRepository = transportOperationRepository;
			TransportOperationTitleRepository = transportoperationtitleRepository;
			UnitOfWork = unitOfWork;
			PromotionExecutionsRepository = promotionExecutionsRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceCardReadInfoResult>> ExecuteAsync(TransportOperationReadInfoArguments arguments)
		{
#if TEST || DEBUG
			try
			{
#endif

#if DEBUG
				var watch = Stopwatch.StartNew();
#endif
				if (arguments.Script == null)
					throw new ArgumentNullException("script");
				var script = new TransportCardGetReadInfoScript(SessionData.Login, Hsm, arguments.Script);
                script.Card.Uid = arguments.MifareClassicCards.FromHexadecimal();
                var uid = script.Card.Uid.ToInt32().Value;

                #region Obtener operación
                var operation = (await TransportOperationRepository.GetAsync("BlackList", "GreyList"))
					.Where(x =>
						(x.Uid == uid) &&
						(x.OperationType == OperationType.Read)
					)
					.OrderByDescending(x => x.OperationDate)
					.FirstOrDefault();
#if DEBUG
				watch.Stop();
				Debug.WriteLine(METHOD_NAME + " - Obtener operación: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
				watch = Stopwatch.StartNew();
#endif
				#endregion Obtener operación

				var onlyView = !operation.Script.IsNullOrEmpty();
				if (!onlyView)
				{
					#region GreyList
					var greyListUntil = arguments.Now.Add(EigeCard.GreyListResolvedElapsed);
					var greyList = (await GreyListRepository.GetAsync())
						.Where(x =>
							(x.Uid == uid) &&
							(x.State == GreyList.GreyListStateType.Active) &&
							(x.Machine == (x.Machine | GreyList.MachineType.Charge))
						)
						.OrderBy(x => x.OperationNumber)
						.ToList();

                    if (script.Card.Validacion.ListaGris.Value)
                    {
                        // Limpiar lista gris
                        var temp = greyList
                           .Where(x =>
                               x.Resolved &&
                               x.ResolutionDate > greyListUntil
                           );
                        if (!temp.Any())
                        {
                            // Clean
                            operation.GreyListUnmarked = true;
                            script.Card.Validacion.ListaGris = new EigeBool(false);
                        }
                    }
                    else
                    {
                        // Aplicar lista gris
                        var temp = greyList
                            .Where(x =>
                                !x.Resolved &&
                                !x.IsConfirmed
                            );
                        foreach (var item in temp)
                        {
                            // Execute
                            ExecuteLGAsync(script, uid, item);

                            // Mark
                            operation.GreyList.Add(item);
                            script.Card.Validacion.ListaGris = new EigeBool(true);
                        }
                    }
#if DEBUG
					watch.Stop();
					Debug.WriteLine(METHOD_NAME + " - GreyList: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
					watch = Stopwatch.StartNew();
#endif
                    #endregion GreyList

                    #region BlackList
                    var blackListUntil = arguments.Now.Add(EigeCard.BlackListResolvedElapsed);
                    var blackList = (await BlackListRepository.GetAsync())
						.Where(x =>
							(x.Uid == uid) &&
							(x.State == BlackList.BlackListStateType.Active) &&
							(x.Machine == (x.Machine | BlackListMachineType.Charge)) &&
							(!x.Rejection)
						)
						.ToList();

					if (script.Card.Validacion.ListaNegra.Value)
					{
						if (script.Card.Usuario.DesbloqueoListaNegra.Value)
                        {
                            // Limpiar lista negra
							var blackListRecent = blackList
								.Where(x =>
                                    (x.Resolved) &&
                                    (x.ResolutionDate > blackListUntil)
								);
                            if (!blackListRecent.Any())
							{
                                // Clean
								script.Card.Usuario.DesbloqueoListaNegra = new EigeBool(false);
								script.Card.Validacion.ListaNegra = new EigeBool(false);
								operation.BlackListUnmarked = true;
							}
						}
					}
					else
                    {
                        // Aplicar lista negra
                        var temp = blackList
							.Where(x =>
								(!x.Resolved)
							);
						foreach (var item in temp)
						{
                            // Mark
							operation.BlackList.Add(item);
                            script.Card.Validacion.ListaNegra = new EigeBool(true);
                        }
					}
#if DEBUG
					watch.Stop();
					Debug.WriteLine(METHOD_NAME + " - BlackList: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
					watch = Stopwatch.StartNew();
#endif
					#endregion BlackList

					#region Recuperar tarjetas
					// Titulo 1
					if ((script.Card.Sectors[1].Blocks[1].Value.Length == 16) && (script.Card.Sectors[1].Blocks[2].Value.Length == 16) && (!script.Card.Sectors[1].Blocks[1].Value.SequenceEqual(script.Card.Sectors[1].Blocks[2].Value)))
					{
						await BlockRecoveryAsync(uid, script, arguments.Now, script.Card.Sectors[1].Blocks[1], script.Card.Sectors[1].Blocks[2]);
						Debug.WriteLine(METHOD_NAME + " - Recuperando título 1");
					}
					// Titulo 2
					if ((script.Card.Sectors[4].Blocks[0].Value.Length == 16) && (script.Card.Sectors[4].Blocks[1].Value.Length == 16) && (!script.Card.Sectors[4].Blocks[0].Value.SequenceEqual(script.Card.Sectors[4].Blocks[1].Value)))
					{
						await BlockRecoveryAsync(uid, script, arguments.Now, script.Card.Sectors[4].Blocks[0], script.Card.Sectors[4].Blocks[1]);
						Debug.WriteLine(METHOD_NAME + " - Recuperando título 2");
					}
					// Tuin
					if ((script.Card.Sectors[9].Blocks[0].Value.Length == 16) && (script.Card.Sectors[9].Blocks[1].Value.Length == 16) && (!script.Card.Sectors[9].Blocks[0].Value.SequenceEqual(script.Card.Sectors[9].Blocks[1].Value)))
					{
						await BlockRecoveryAsync(uid, script, arguments.Now, script.Card.Sectors[9].Blocks[0], script.Card.Sectors[9].Blocks[1]);
						Debug.WriteLine(METHOD_NAME + " - Recuperando título TuiN");
					}
#if DEBUG
					watch.Stop();
					Debug.WriteLine(METHOD_NAME + " - Recuperar tarjeta: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
					watch = Stopwatch.StartNew();
#endif
					#endregion Recuperar tarjetas
				}

                #region Cargar support
                var transportCardSupport = await EigeService.GetSupportCardAsync(uid, script, arguments.Now);
                #endregion Cargar support

                #region Cargar titulos / monedero / bonus
                var titleCode1 = await EigeService.GetTitleCode1Async(uid, script);
				var titleZone1 = await EigeService.GetTitleZoneName1Async(uid, script);

				var titleCode2 = await EigeService.GetTitleCode2Async(uid, script);
				var titleZone2 = await EigeService.GetTitleZoneName2Async(uid, script);

				var list = new List<TransportCardReadInfoResultTemp>();
				if ((await EigeService.GetTitleActive1Async(uid, script)) == true)
				{
					var title = (await TitleRepository.GetAsync())
						.Where(x => x.Code == titleCode1)
						.FirstOrDefault();
					var item = new TransportCardReadInfoResultTemp
					{
						Code = titleCode1,
						Name = await EigeService.GetTitleName1Async(uid, script),
						OwnerName = await EigeService.GetTitleOwnerName1Async(uid, script),
						Zone = titleZone1,
						Caducity = await EigeService.GetTitleCaducity1Async(uid, script),
						IsRechargable = await EigeService.IsRechargable1Async(uid, script, transportCardSupport, arguments.Now),
						HasTariff = await EigeService.HasTariff1Async(uid, script),
						IsExhausted = await EigeService.GetTitleIsExhausted1Async(uid, script, arguments.Now),
						IsExpired = await EigeService.GetTitleIsExpired1Async(uid, script, arguments.Now),
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
						AmpliationUnits = await EigeService.GetTitleAmpliationUnits1Async(uid, script),
						MeanTransport = title.MeanTransport
					};
					list.Add(item);
				
					//if (arguments.IsRead != null)
					//{
					var operationTitle = new TransportOperationTitle();
					if(!EigeService.IsTuiN(titleCode1)) //No es TuiN
					{
						operationTitle = new TransportOperationTitle
						{
							Caducity = arguments.Now,
							Quantity = (item.HasBalance) ? script.Card.Titulo.SaldoViaje1.Value : -1m,
							Operation = operation,
							Title = title,
							Zone = script.Card.Titulo.ValidezZonal1.Value
						};
					}
					else //Es TuiN
					{
						operationTitle = new TransportOperationTitle
						{
							Caducity = arguments.Now,
							Quantity = (script.Card.TituloTuiN.SaldoViaje_Sign.Value == false) ? -1 * script.Card.TituloTuiN.SaldoViaje_Value.Value : script.Card.TituloTuiN.SaldoViaje_Value.Value,
							Operation = operation,
							Title = title,
							Zone = script.Card.Titulo.ValidezZonal1.Value
						};
					}						
					await TransportOperationTitleRepository.AddAsync(operationTitle);
					//}
				}
				if ((await EigeService.GetTitleActive2Async(uid, script)) == true)
				{
					var title = (await TitleRepository.GetAsync())
						.Where(x => x.Code == titleCode2)
						.FirstOrDefault();

					var item = new TransportCardReadInfoResultTemp
					{
						Code = titleCode2,
						Name = await EigeService.GetTitleName2Async(uid, script),
						OwnerName = await EigeService.GetTitleOwnerName2Async(uid, script),
						Zone = titleZone2,
						Caducity = await EigeService.GetTitleCaducity2Async(uid, script),
						IsRechargable = await EigeService.IsRechargable2Async(uid, script, transportCardSupport, arguments.Now),
						HasTariff = await EigeService.HasTariff2Async(uid, script),
						IsExhausted = await EigeService.GetTitleIsExhausted2Async(uid, script, arguments.Now),
						IsExpired = await EigeService.GetTitleIsExpired2Async(uid, script, arguments.Now),
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
						AmpliationUnits = await EigeService.GetTitleAmpliationUnits2Async(uid, script),
						MeanTransport = title.MeanTransport
					};
					list.Add(item);

					//if (arguments.IsRead != null)
					//{
					var operationTitle = new TransportOperationTitle
					{
						Caducity = arguments.Now,
						Quantity = (item.HasBalance) ? script.Card.Titulo.SaldoViaje2.Value : -1m,
						Operation = operation,
						Title = title,
						Zone = script.Card.Titulo.ValidezZonal2.Value
					};
					await TransportOperationTitleRepository.AddAsync(operationTitle);
				//	}
				}
				if ((await TescService.GetTitleActive1Async(uid, script)) == true)
				{
					var item = new TransportCardReadInfoResultTemp
					{
						//Code = titleCode2,
						Name = "Tarj. Española sin contacto",
						OwnerName = "ITS",
                        //Zone = titleZone2,
                        //Caducity = await EigeService.GetTitleCaducity2Async(uid, script),
                        //IsRechargable = await EigeService.IsRechargable2Async(uid, script),
                        //HasTariff = await EigeService.HasTariff2Async(uid, script),
                        //IsExhausted = await EigeService.GetTitleIsExhausted2Async(uid, script, now),
                        //IsExpired = await EigeService.GetTitleIsExpired2Async(uid, script, now),
                        //MaxExternalTransfers = await EigeService.GetTitleMaxExternalTransfers2Async(uid, script),
                        //MaxPeopleInTransfer = await EigeService.GetTitleMaxPeopleInTransfer2Async(uid, script),
                        //// Balance
                        //HasBalance = await EigeService.GetTitleHasBalance2Async(uid, script),
                        //Balance = await EigeService.GetTitleBalance2Async(uid, script),
                        //BalanceUnits = await EigeService.GetTitleBalanceUnits2Async(uid, script),
                        //Temporal
                        IsTemporal = true,
						ExhaustedDate = (await TescService.GetTitleExhaustedDate1Async(uid, script)).ToUTC(),
						//ActivatedDate = (await EigeService.GetTitleActivatedDate2Async(uid, script)).ToUTC(),
						Ampliation = 0,
						//AmpliationQuantity = await EigeService.GetTitleAmpliationQuantity2Async(uid, script),
						//AmpliationUnits = await EigeService.GetTitleAmpliationUnits2Async(uid, script)
					};
					list.Add(item);

					var title = (await TitleRepository.GetAsync())
						.Where(x => x.Code == titleCode2)
						.FirstOrDefault();

					var operationTitle = new TransportOperationTitle
					{
						Caducity = arguments.Now,
						Quantity = (item.HasBalance) ? script.Card.Titulo.SaldoViaje2.Value : -1m,
						Operation = operation,
						Title = title,
						Zone = script.Card.Titulo.ValidezZonal2.Value
					};
					await TransportOperationTitleRepository.AddAsync(operationTitle);
				}

				if ((await TescService.GetTitleActive2Async(uid, script)) == true)
				{
					var item = new TransportCardReadInfoResultTemp
					{
						//Code = titleCode2,
						Name = "Tarj. Española sin contacto",
						OwnerName = "ITS",
                        //Zone = titleZone2,
                        //Caducity = await EigeService.GetTitleCaducity2Async(uid, script),
                        //IsRechargable = await EigeService.IsRechargable2Async(uid, script),
                        //HasTariff = await EigeService.HasTariff2Async(uid, script),
                        //IsExhausted = await EigeService.GetTitleIsExhausted2Async(uid, script, now),
                        //IsExpired = await EigeService.GetTitleIsExpired2Async(uid, script, now),
                        //MaxExternalTransfers = await EigeService.GetTitleMaxExternalTransfers2Async(uid, script),
                        //MaxPeopleInTransfer = await EigeService.GetTitleMaxPeopleInTransfer2Async(uid, script),
                        //// Balance
                        //HasBalance = await EigeService.GetTitleHasBalance2Async(uid, script),
                        //Balance = await EigeService.GetTitleBalance2Async(uid, script),
                        //BalanceUnits = await EigeService.GetTitleBalanceUnits2Async(uid, script),
                        //Temporal
                        IsTemporal = true,
						ExhaustedDate = (await TescService.GetTitleExhaustedDate2Async(uid, script)).ToUTC(),
						//ActivatedDate = (await EigeService.GetTitleActivatedDate2Async(uid, script)).ToUTC(),
						Ampliation = 0,
						//AmpliationQuantity = await EigeService.GetTitleAmpliationQuantity2Async(uid, script),
						//AmpliationUnits = await EigeService.GetTitleAmpliationUnits2Async(uid, script)
					};
					list.Add(item);

					var title = (await TitleRepository.GetAsync())
						.Where(x => x.Code == titleCode2)
						.FirstOrDefault();

					//if (arguments.IsRead != null)
					//{
					var operationTitle = new TransportOperationTitle
					{
						Caducity = arguments.Now,
						Quantity = (item.HasBalance) ? script.Card.Titulo.SaldoViaje2.Value : -1m,
						Operation = operation,
						Title = title,
						Zone = script.Card.Titulo.ValidezZonal2.Value
					};
					await TransportOperationTitleRepository.AddAsync(operationTitle);
					//}
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
						IsRechargable = await EigeService.IsRechargableMAsync(uid, script, transportCardSupport, arguments.Now),
						HasTariff = await EigeService.HasTariffMAsync(uid, script),
						IsExhausted = await EigeService.GetTitleIsExhaustedMAsync(uid, script, arguments.Now),
						MaxExternalTransfers = await EigeService.GetTitleMaxExternalTransfersMAsync(uid, script),
						MaxPeopleInTransfer = await EigeService.GetTitleMaxPeopleInTransferMAsync(uid, script),
						// Balance
						HasBalance = true,
						Balance = await EigeService.GetTitleBalanceMAsync(uid, script),
                        //BalanceAcumulated = await EigeService.GetTitleBalanceAcumulated1Async(uid, sigapuntScript),
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
						IsRechargable = await EigeService.IsRechargableBAsync(uid, script, transportCardSupport, arguments.Now),
						HasTariff = await EigeService.HasTariffBAsync(uid, script),
						IsExhausted = await EigeService.GetTitleIsExhaustedBAsync(uid, script, arguments.Now),
						MaxExternalTransfers = await EigeService.GetTitleMaxExternalTransfersBAsync(uid, script),
						MaxPeopleInTransfer = await EigeService.GetTitleMaxPeopleInTransferBAsync(uid, script),
						// Balance
						HasBalance = true,
                        Balance = await EigeService.GetTitleBalanceBAsync(uid, script),
                        //BalanceAcumulated = await EigeService.GetTitleBalanceAcumulated1Async(uid, sigapuntScript),
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

#if DEBUG
				watch.Stop();
				Debug.WriteLine(METHOD_NAME + " - Cargar titulos / monedero / bonus: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
				watch = Stopwatch.StartNew();
#endif
				#endregion Cargar titulos / monedero / bonus

				#region Cargar histórico validaciones
				// HistoricoFechaHora
				DateTime? historicoDate1 = await EigeService.GetHistoricoDate1Async(uid, script);
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

				var lastValidation = log.FirstOrDefault();
				var lastValidationTitle = list
					.Where(x =>
						lastValidation != null &&
						x.Code == lastValidation.Code)
					.FirstOrDefault();

				// Ajustar para la Tuin
				var tuinCode =
					EigeService.IsTuiN(script.Card.Titulo.CodigoTitulo1.Value) ? script.Card.Titulo.CodigoTitulo1.Value :
					EigeService.IsTuiN(script.Card.Titulo.CodigoTitulo2.Value) ? script.Card.Titulo.CodigoTitulo2.Value :
					(int?)null;

				if (tuinCode != null)
				{
					ServiceCardReadInfoResult_Log anterior = null;
					foreach (var item in log
						.Where(x => x.Code == tuinCode)
						.OrderBy(x => x.Date)
					)
					{
						if (anterior != null)
						{
							if ((item.TypeName == "Salida") && (anterior.TypeName == "Entrada"))
								item.Quantity -= anterior.Quantity;
						}
						anterior = item;
					}
				}

#if DEBUG
				watch.Stop();
				Debug.WriteLine(METHOD_NAME + " - Cargar histórico validaciones: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
				watch = Stopwatch.StartNew();
#endif
				#endregion Cargar histórico validaciones

				#region Cargar histórico cargas
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
						Date = cargaDate1.ToLTC(),
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
							Date = cargaDate2.ToLTC(),
							TypeName = cargaTypeName2,
							TitleName = cargaTitleName2,
							TitleOwnerName = title != null ? title.OwnerName : "",
							TitleZone = title != null ? title.Zone : (EigeZonaEnum?)null,
							Quantity = cargaQuantity2
						};
				}
				
#if DEBUG
				watch.Stop();
				Debug.WriteLine(METHOD_NAME + " - Cargar histórico cargas: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
				watch = Stopwatch.StartNew();
#endif
				#endregion Cargar histórico cargas

				#region Cargar otros campos
				var titleCodeZone = list
					.Where(x => x.Code != null)
					.Select(x => new {
                        Code = x.Code.Value,
                        x.Zone,
                        ZoneName = x.Zone.ToEnumAlias("")
                    })
					.ToList();
				var titleCode = list
					.Where(x => x.Code != null)
					.Select(x => x.Code.Value)
					.ToList();
				var ownerCard = await EigeService.GetCardOwnerAsync(uid, script);
				var cardType = script.Card.Tarjeta.Tipo == null ? null : (int?)script.Card.Tarjeta.Tipo.Value;
				var cardSubtype = script.Card.Tarjeta.Subtipo == null ? null : (int?)script.Card.Tarjeta.Subtipo.Value;

				var titleActive1 = await EigeService.GetTitleActive1Async(uid, script);
				var titleExhausted1 = await EigeService.GetTitleIsExhausted1Async(uid, script, arguments.Now);
				var titleActive2 = await EigeService.GetTitleActive2Async(uid, script);
				var titleExhausted2 = await EigeService.GetTitleIsExhausted2Async(uid, script, arguments.Now);

				var tarifa1 = script.Card.Titulo.ControlTarifa1.Value;
				var tarifa2 = script.Card.Titulo.ControlTarifa2.Value;

				var cardPrice1 = (await PriceRepository.GetAsync(
					"Title.TransportSimultaneousTitleCompatibility.TransportTitle2",
					"Title.TransportSimultaneousTitleCompatibility2.TransportTitle"
				))
					.Where(x =>
						x.Title.Code == titleCode1 &&
						(!x.Title.HasZone || x.Zone == titleZone1) &&
						x.Version == tarifa1
					)
					.FirstOrDefault();
				var cardPrice2 = (await PriceRepository.GetAsync(
					"Title.TransportSimultaneousTitleCompatibility.TransportTitle2",
					"Title.TransportSimultaneousTitleCompatibility2.TransportTitle"
				))
					.Where(x =>
						x.Title.Code == titleCode2 &&
						(!x.Title.HasZone || x.Zone == titleZone2) &&
						x.Version == tarifa2
					)
					.FirstOrDefault();

#if DEBUG
				watch.Stop();
				Debug.WriteLine(METHOD_NAME + " - Cargar otros campos: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
				watch = Stopwatch.StartNew();
#endif
				#endregion Cargar otros campos

				#region Obtener recargas posibles
				var titlesRecharges2 = (await EigeService.GetRechargesAsync(uid, script, arguments.Now, RechargeType.Recharge, null, null, 1));
				var titlesRecharges = titlesRecharges2 == null ?
					new List<ServiceCardReadInfoResult_RechargeTitle>() :
					titlesRecharges2
						.ToList()
						.GroupBy(x => x.Title)
						.Select(x => new
						{
							x.Key.Id,
							x.Key.Code,
							x.Key.Name,
							OwnerCity = "Valencia",
							x.Key.OwnerName,
							PaymentConcessionId = 1,
							TransportConcession = x.Key.TransportConcessionId,
							x.Key.HasZone,
							Prices = x
								.Select(y => new
								{
									y.Id,
									y.Zone,
									y.Price,
									Config = EigeService.GetRechargeConfig(uid, script, titleActive1, titleExhausted1, cardPrice1, titleActive2, titleExhausted2, cardPrice2, y, arguments.Now)
								}),
                            x.Key.MaxQuantity,
							RechargeMax = (x.Key.MaxQuantity - list
								.Where(y => y.Code == x.Key.Code)
								.Sum(y => y.Balance)
							) / x.Key.Quantity,
							RechargeMin = x.Key.MinCharge,
                            RechargeStep = x.Key.PriceStep,
                            x.Key.Quantity,
                            QuantityInverse = (x.Key.Quantity ?? 0) == 0 ?
                                0 :
                                Math.Round(1 / x.Key.Quantity.Value, 2),
                            AskQuantity = x.Key.AskQuantity(),
							x.Key.MeanTransport
						})
						.Select(x => new ServiceCardReadInfoResult_RechargeTitle
						{
							Id = x.Id,
							Code = x.Code,
							Name = x.Name,
							OwnerCity = x.OwnerCity,
							OwnerName = x.OwnerName,
							PaymentConcessionId = x.PaymentConcessionId,
							TransportConcession = x.TransportConcession,
							Prices = x.Prices
								.Select(y => new ServiceCardReadInfoResult_RechargePrice
								{
									Id = y.Id,
									Zone = x.HasZone ? y.Zone : (x.Code == 96 ? EigeZonaEnum.A : (EigeZonaEnum?)null),
									ZoneName = x.HasZone ? y.Zone.ToEnumAlias("") : "",
									Price = y.Price,
									ChangePrice = y.Config.ChangePrice,
									RechargeType = y.Config.RechargeType,
									Slot = y.Config.Slot
								}),
							MaxQuantity = x.MaxQuantity,
							RechargeMax = x.RechargeMax,
							RechargeMin = x.RechargeMin,
                            RechargeStep = x.RechargeStep,
                            Quantity = x.Quantity,
                            QuantityInverse = x.QuantityInverse,
                            AskQuantity = x.AskQuantity,
							MeanTransport = x.MeanTransport
                        })
#if DEBUG
						.ToList()
#endif // DEBUG
						;
                foreach (var item in list)
                {
                    item.RechargeTitle = titlesRecharges
                        .Where(x =>
                            (x.Code == item.Code) &&
                            (x.Prices
                                .Where(y =>
                                    y.Zone == item.Zone
                                )
                                .Any()
                            )
                        )
                        .FirstOrDefault();
                }

#if DEBUG
				watch.Stop();
				Debug.WriteLine(METHOD_NAME + " - Obtener recargas posibles: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
				watch = Stopwatch.StartNew();
#endif
				#endregion Obtener recargas posibles

				#region Obtener cargas posibles
				var titlesCharges2 = (await EigeService.GetRechargesAsync(uid, script, arguments.Now, RechargeType.Charge, null, null, 1));
				var titlesCharges = titlesCharges2 == null ?
					new List<ServiceCardReadInfoResult_RechargeTitle>() :
					titlesCharges2
						.ToList()
						.GroupBy(x => x.Title)
						.Select(x => new
						{
							x.Key.Id,
							x.Key.Code,
							x.Key.Name,
							OwnerCity = "Valencia",
							x.Key.OwnerName,
							PaymentConcessionId = 1,
							TransportConcession = x.Key.TransportConcessionId,
							x.Key.HasZone,
							Prices = x
								.Select(y => new
								{
									y.Id,
									y.Zone,
									y.Price,
									Config = EigeService.GetRechargeConfig(uid, script, titleActive1, titleExhausted1, cardPrice1, titleActive2, titleExhausted2, cardPrice2, y, arguments.Now)
								}),
                            x.Key.Quantity,
                            QuantityInverse = (x.Key.Quantity ?? 0) == 0 ?
                                0 :
                                Math.Round(1 / x.Key.Quantity.Value, 2),
                            AskQuantity = x.Key.AskQuantity(),
							x.Key.MeanTransport,
							x.Key.MaxQuantity,
							RechargeMax = (x.Key.MaxQuantity - list
								.Where(y => y.Code == x.Key.Code)
								.Sum(y => y.Balance)
							) / x.Key.Quantity,
							RechargeMin = x.Key.MinCharge,
							RechargeStep = x.Key.PriceStep,
						})
						.Select(x => new ServiceCardReadInfoResult_RechargeTitle
						{
							Id = x.Id,
							Code = x.Code,
							Name = x.Name,
							OwnerCity = x.OwnerCity,
							OwnerName = x.OwnerName,
							PaymentConcessionId = x.PaymentConcessionId,
							TransportConcession = x.TransportConcession,
							Prices = x.Prices
								.Select(y => new ServiceCardReadInfoResult_RechargePrice
								{
									Id = y.Id,
									Zone = x.HasZone ? y.Zone : (x.Code == 96 ? EigeZonaEnum.A : (EigeZonaEnum?)null),
									ZoneName = x.HasZone ? y.Zone.ToEnumAlias("") : "",
									Price = y.Price,
									ChangePrice = y.Config == null ? 0 : y.Config.ChangePrice,
									RechargeType = y.Config == null ? 0 : y.Config.RechargeType,
									Slot = y.Config == null ? (EigeTituloEnUsoEnum?)null : y.Config.Slot
								}),
							Quantity = x.Quantity,
                            QuantityInverse = x.QuantityInverse,
                            AskQuantity = x.AskQuantity,
							MeanTransport = x.MeanTransport,
							MaxQuantity = x.MaxQuantity,
							RechargeMax = x.RechargeMax,
							RechargeMin = x.RechargeMin,
							RechargeStep = x.RechargeStep
						})
#if DEBUG
						.ToList()
#endif // DEBUG
						;

#if DEBUG
				watch.Stop();
				Debug.WriteLine(METHOD_NAME + " - Obtener cargas posibles: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
				watch = Stopwatch.StartNew();
#endif
				#endregion Obtener cargas posibles

				#region Obtener promos posibles
				// Buscamos las promos executions instantaneas
				var promotions = (await PromotionExecutionsRepository.GetAsync())
					.Where(x =>
						x.AppliedDate == null &&
						x.PromoUser.Login == SessionData.Login &&
						x.Promotion.StartDate <= arguments.Now &&
						x.Promotion.EndDate >= arguments.Now &&
						x.Promotion.State == PromotionState.Active &&
						x.Promotion.PromoLaunchers
							.Where(y => y.Type == PromoLauncherType.Instant)
							.Any()
					)
					.Select(x => new {
						x.Id,
						x.Promotion.Name,
						x.Promotion.EndDate,
						Concession = x.Promotion.Concession.Concession.Name,
						Actions = x.Promotion.PromoActions
							.Select(y => new ServiceCardReadInfoResult_PromotionAction
							{
								Type = y.Type,
								Quantity = y.Quantity
							}),
						Prices = x.Promotion.PromoPrices
							.Select(y => new ServiceCardReadInfoResult_PromotionPrice
							{
								Id = y.TransportPriceId,
								Code = y.TransportPrice.Title.Code,
								Zone = y.TransportPrice.Zone,
								HasZone = y.TransportPrice.Title.HasZone,
								Name = y.TransportPrice.Title.Name
							})
					})
					.ToList();
				var priceIds = promotions
					.SelectMany(x => x.Prices
						.Select(y => y.Id)
					)
					.Distinct();

				// Buscar las promos recargables
				var titlesPromos2 = (await EigeService.GetRechargesAsync(uid, script, arguments.Now, RechargeType.Charge, null, priceIds, 0));
				var titlesPromos = titlesPromos2 == null ?
					new List<ServiceCardReadInfoResult_RechargeTitle>() :
					titlesPromos2
						.ToList()
						.GroupBy(x => x.Title)
						.Select(x => new
						{
							x.Key.Id,
							x.Key.Code,
							x.Key.Name,
							OwnerCity = "Valencia",
							x.Key.OwnerName,
							PaymentConcessionId = 1,
							TransportConcession = x.Key.TransportConcessionId,
							x.Key.HasZone,
							Prices = x
								.Select(y => new
								{
									y.Id,
									y.Zone,
									y.Price,
									Config = EigeService.GetRechargeConfig(uid, script, titleActive1, titleExhausted1, cardPrice1, titleActive2, titleExhausted2, cardPrice2, y, arguments.Now)
								}),
							x.Key.Quantity,
                            QuantityInverse = (x.Key.Quantity ?? 0) == 0 ?
                                0 :
                                Math.Round(1 / x.Key.Quantity.Value, 2),
                            AskQuantity = x.Key.AskQuantity(),
							x.Key.MeanTransport,
							x.Key.MaxQuantity,
							RechargeMax = (x.Key.MaxQuantity - list
								.Where(y => y.Code == x.Key.Code)
								.Sum(y => y.Balance)
							) / x.Key.Quantity,
							RechargeMin = x.Key.MinCharge,
							RechargeStep = x.Key.PriceStep,
						})
						.Select(x => new ServiceCardReadInfoResult_RechargeTitle
						{
							Id = x.Id,
							Code = x.Code,
							Name = x.Name,
							OwnerCity = x.OwnerCity,
							OwnerName = x.OwnerName,
							PaymentConcessionId = x.PaymentConcessionId,
							TransportConcession = x.TransportConcession,
							Prices = x.Prices
								.Select(y => new ServiceCardReadInfoResult_RechargePrice
								{
									Id = y.Id,
									Zone = x.HasZone ? y.Zone : (x.Code == 96 ? EigeZonaEnum.A : (EigeZonaEnum?)null),
									ZoneName = x.HasZone ? y.Zone.ToEnumAlias("") : "",
									Price = y.Price,
									ChangePrice = y.Config.ChangePrice,
									RechargeType = y.Config.RechargeType,
									Slot = y.Config.Slot
								}),
							Quantity = x.Quantity,
                            QuantityInverse = x.QuantityInverse,
                            AskQuantity = x.AskQuantity,
							MeanTransport = x.MeanTransport,
							MaxQuantity = x.MaxQuantity,
							RechargeMax = x.RechargeMax,
							RechargeMin = x.RechargeMin,
							RechargeStep = x.RechargeStep
						})
#if DEBUG
						.ToList()
#endif // DEBUG
						;

				// Nos quedamos con las promo executions que son recargables 
				var promotions3 = promotions
					.Select(x => new ServiceCardReadInfoResult_Promotion
					{
						Name = x.Name,
						EndDate = x.EndDate,
						Id = x.Id,
						Concession = x.Concession,
						Actions = x.Actions,
						Titles = titlesPromos
							.Select(y => new ServiceCardReadInfoResult_RechargeTitle
							{
								Code = y.Code,
								Id = y.Id,
								MeanTransport = y.MeanTransport,
								Name = y.Name,
								OwnerCity = y.OwnerCity,
								OwnerName = y.OwnerName,
								PaymentConcessionId = y.PaymentConcessionId,
								Prices = y.Prices
									.Where(z => x.Prices
										.Where(a => a.Id == z.Id)
										.Any()
									),
								TransportConcession = y.TransportConcession,
								Quantity = y.Quantity,
								AskQuantity = y.AskQuantity,
								MaxQuantity = y.MaxQuantity,
								RechargeMax = (y.MaxQuantity - list
									.Where(z => z.Code == y.Code)
									.Sum(z => z.Balance)
								) / y.Quantity,
								RechargeMin = y.RechargeMin,
								RechargeStep = y.RechargeStep
							})
							.Where(y => y.Prices.Any())
					})
					.Where(x => x.Titles.Any())
					.ToList();

#if DEBUG
				watch.Stop();
				Debug.WriteLine(METHOD_NAME + " - Obtener cargas posibles: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
				watch = Stopwatch.StartNew();
#endif // DEBUG
				#endregion Obtener cargas posibles

				#region Check revokable
				decimal? revokablePrice = null;
				var lastRechargeOperation = await EigeService.GetOperationToRevoke(uid, script, await TransportOperationRepository.GetAsync(), arguments.Now);
				if (lastRechargeOperation != null && lastRechargeOperation.Price != null)
					revokablePrice = lastRechargeOperation.Price.Price;
				else if (lastRechargeOperation != null && lastRechargeOperation.Price == null)
					revokablePrice = (await PriceRepository.GetAsync())
						.Where(x => x.Id == lastRechargeOperation.TransportPriceId)
						.FirstOrDefault()
						.Price;
#if DEBUG
				watch.Stop();
				Debug.WriteLine(METHOD_NAME + " - Check revokable: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
				watch = Stopwatch.StartNew();
#endif
				#endregion Check revokable

				#region Obtener script
				var resultScript = await script.GetResponseAndRequestAsync();
				var resultKeys = await script.GetKeysEncryptedAsync(Hsm, arguments.CardSystem, resultScript, script.Card.Uid.ToInt32() ?? 0, script.Card.TituloTuiN.VersionClaves.Value);
				var it = new ScriptResult
				{
					Card = new ScriptResult.CardId
					{
						Uid = uid,
						Type = CardType.MIFAREClassic,
						System = CardSystem.Mobilis
					},
					Script = resultScript,
					Keys = resultKeys
				};
#if DEBUG
				watch.Stop();
				Debug.WriteLine(METHOD_NAME + " - Generate script: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
				watch = Stopwatch.StartNew();
#endif
				#endregion Obtener script

				#region Serializar tarjeta				
				var result = new ServiceCardReadInfoResultBase
				{
					// General
					CardId = arguments.MifareClassicCards,
					Owner = await EigeService.GetCardOwnerAsync(uid, script),
					OwnerName = await EigeService.GetCardOwnerNameAsync(uid, script),
					TypeName = await EigeService.GetCardTypeNameAsync(uid, script),
					InBlackList = await EigeService.InBlackListAsync(uid, script),
					ExpiredDate = await EigeService.GetExpiredDateAsync(uid, script), // No hay que invocar a ToUTC() porque ya está en horario local del móvil
					IsExpired = // Show Card Expired (only Eige)
						(await EigeService.IsCardExpiredAsync(uid, script, transportCardSupport, arguments.Now)),
					IsDamaged = await EigeService.IsDamagedAsync(uid, script),
					IsRechargable = (
						(await EigeService.IsRechargable1Async(uid, script, transportCardSupport, arguments.Now)) ||
						(await EigeService.IsRechargable2Async(uid, script, transportCardSupport, arguments.Now)) ||
						(await EigeService.IsRechargableMAsync(uid, script, transportCardSupport, arguments.Now)) ||
						(await EigeService.IsRechargableBAsync(uid, script, transportCardSupport, arguments.Now))
					),
					IsRevokable = lastRechargeOperation == null ? false : true,
					RevokablePrice = revokablePrice,
					LastRechargeOperationId = lastRechargeOperation != null ? lastRechargeOperation.Id : (int?)null,
					HasHourValidity = (await EigeService.HasHourValidity1Async(uid, script)) || (await EigeService.HasHourValidity2Async(uid, script)),
					HasDayValidity = (await EigeService.HasDayValidity1Async(uid, script)) || (await EigeService.HasDayValidity2Async(uid, script)),
					ApproximateValues = false,
					Logs = log
						.Where(x => x != null)
						.OrderByDescending(x => x.Date),
					Charges = charges
						.Where(x => x != null)
						.OrderByDescending(x => x.Date),
					Data = list
						.Select(x => new ServiceCardReadInfoResult
						{
							Code = x.Code,
							Slot = x.Slot,
							Name = x.Name,
							OwnerName = x.OwnerName,
							Zone = x.Zone,
							Caducity = x.Caducity,
							IsRechargable = x.IsRechargable,
							HasTariff = x.HasTariff,
							IsExhausted = x.IsExhausted,
							IsExpired = x.IsExpired,
							// Balance
							HasBalance = x.HasBalance,
							Balance = x.Balance,
							//BalanceAcumulated = x.BalanceAcumulated,
							BalanceUnits = x.BalanceUnits,
                            // Temporal
                            IsTemporal = x.IsTemporal,
							ExhaustedDate = x.ExhaustedDate,
							ActivatedDate = x.ActivatedDate,
							Ampliation = x.Ampliation,
							AmpliationQuantity = x.AmpliationQuantity,
							AmpliationUnits = x.AmpliationUnits,
							ReadAll = true,
							MeanTransport = x.MeanTransport,
                            RechargeTitle = x.RechargeTitle,
                            RechargeImpediments = EigeService.GetRechargeImpediments(x, x.RechargeTitle, arguments.Now)
                        })
                        .ToList(),
					// Personalization
					IsPersonalized = await EigeService.IsPersonalizedAsync(uid, script),
					UserName = await EigeService.GetUserNameAsync(uid, script),
					UserSurname = await EigeService.GetUserSurnameAsync(uid, script),
					UserDni = await EigeService.GetUserDniAsync(uid, script),
					UserCode = await EigeService.GetUserCodeAsync(uid, script),
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
					// Transbordos
					PeopleInTransfer = await EigeService.GetLastValidationPeopleInTransferAsync(uid, script, lastValidation == null ? null : lastValidation.Code),
					MaxPeopleInTransfer = lastValidationTitle == null ? 0 : lastValidationTitle.MaxPeopleInTransfer,
					InternalTransfers = await EigeService.GetLastValidationInternalTransfersAsync(uid, script, lastValidation == null ? null : lastValidation.Code),
					ExternalTransfers = await EigeService.GetLastValidationExternalTransfersAsync(uid, script, lastValidation == null ? null : lastValidation.Code),
					MaxInternalTransfers = await EigeService.GetLastValidationMaxInternalTransfersAsync(uid, script, lastValidation == null ? null : lastValidation.Code),
					MaxExternalTransfers = await EigeService.GetLastValidationMaxExternalTransfersAsync(uid, script, lastValidation == null ? null : lastValidation.Code, lastValidationTitle == null ? 0 : lastValidationTitle.MaxExternalTransfers),
					Mode = EigeService.TransferMode,
					DeviceType = await EigeService.GetDeviceTypeAsync(uid, script),
					// Recargas
					RechargeTitles = titlesRecharges,
					ChargeTitles = titlesCharges,
					Scripts = new[] { it },
					Operation = new ScriptResultBase<ServiceCardReadInfoResult>.ScriptResultOperation
					{
						Type = operation.OperationType,
						Uid = operation.Uid,
						Id = operation.Id
					},
					Promotions = promotions3
				};

#if DEBUG
				watch.Stop();
				Debug.WriteLine(METHOD_NAME + " - Serializar tarjeta: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
				watch = Stopwatch.StartNew();
#endif
				#endregion Serializar tarjeta

				#region Actualizar operación
				if (!onlyView)
				{
					// Paso de ReadInfo
					operation.Script = arguments.Script.ToJson();
					arguments.OperationId = operation.Id;
				}
#if DEBUG
				watch.Stop();
				Debug.WriteLine(METHOD_NAME + " - Actualizar operación: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
				watch = Stopwatch.StartNew();
#endif
                #endregion Actualizar operación
                
                // Poniendo impediments a posteriori
                foreach (var item in result.Data)
                {
                    // Pay[in]
                    if ((SessionData.ClientId != AccountClientId.FgvTsm) && (SessionData.ClientId != AccountClientId.AlacantTsm))
                        item.RechargeImpediments = new List<string>() { ServiceCardReadInfoResult.NotAllowed };
                    // FGV
                    else if ((SessionData.ClientId == AccountClientId.FgvTsm) && (item.OwnerName != "FGV"))
                        item.RechargeImpediments = new List<string>() { ServiceCardReadInfoResult.NotAllowed };
                    // TRAM
                    else if ((SessionData.ClientId == AccountClientId.AlacantTsm) && (item.OwnerName != "TRAM"))
                        item.RechargeImpediments = new List<string>() { ServiceCardReadInfoResult.NotAllowed };
                    // Titulos activos
                    else
                    {
                        if (result.IsExpired)
                            item.RechargeImpediments.Add(ServiceCardReadInfoResult.CardExpired);
                        if (result.InBlackList)
                            item.RechargeImpediments.Add(ServiceCardReadInfoResult.CardInBlackList);
                        if (result.IsDamaged)
                            item.RechargeImpediments.Add(ServiceCardReadInfoResult.CardDamaged);
                    }
                }
                
                // Limpiando datos y bloqueando titulos si hay impediments
                foreach (var item in result.Data)
                {
                    item.IsRechargable &= item.RechargeImpediments.IsNullOrEmpty();
                    if (!item.IsRechargable)
                        item.RechargeTitle = null;
                }

                // Eliminando títulos no antiguos para FGV y TRAM
                result.ChargeTitles = new ServiceCardReadInfoResult_RechargeTitle[0];
                result.RechargeTitles = new ServiceCardReadInfoResult_RechargeTitle[0];
                if (!result.Data
                    .Where(x => x.IsRechargable)
                    .Any()
                )
                    result.IsRechargable = false;

                return result;
#if TEST || DEBUG
			}
			catch (Exception ex)
			{
				throw new ApplicationException("ERROR: " + ex.Message + "\n\n" + ex.StackTrace);
			}
#endif
		}
		#endregion ExecuteAsync

		#region ExecuteLGAsync
		public async void ExecuteLGAsync(TransportCardGetReadInfoScript script, long uid, GreyList item)
		{
			if (item.Action == GreyList.ActionType.ModifyBalance)
			{
				item.OldValue = script.GetValueFromCode(item.Field).GetPropertyValue<byte[]>("Raw").ToHexadecimal();
				script.SetValueFromCode(item.Field, item.NewValue, item.Action, uid);
			}
			else if (item.Action == GreyList.ActionType.DiscountBalance)
			{
				item.OldValue = script.GetValueFromCode(item.Field).GetPropertyValue<byte[]>("Raw").ToHexadecimal();
				await script.AddValueFromCodeAsync(item.Field, -Convert.ToDecimal(item.NewValue));
			}
			else if (item.Action == GreyList.ActionType.IncreaseBalance)
			{
				item.OldValue = script.GetValueFromCode(item.Field).GetPropertyValue<byte[]>("Raw").ToHexadecimal();
				await script.AddValueFromCodeAsync(item.Field, Convert.ToDecimal(item.NewValue));
			}
			else if (item.Action == GreyList.ActionType.DiscountUnities)
			{
				item.OldValue = script.GetValueFromCode(item.Field).GetPropertyValue<byte[]>("Raw").ToHexadecimal();
				await script.AddValueFromCodeAsync(item.Field, -Convert.ToDecimal(item.NewValue));
			}
			else if (item.Action == GreyList.ActionType.IncreaseUnities)
			{
				item.OldValue = script.GetValueFromCode(item.Field).GetPropertyValue<byte[]>("Raw").ToHexadecimal();
				await script.AddValueFromCodeAsync(item.Field, Convert.ToDecimal(item.NewValue));
			}
			else if (item.Action == GreyList.ActionType.ModifyBlock)
			{
				item.OldValue = script.GetValueFromCode(item.Field).GetPropertyValue<byte[]>("Value").ToHexadecimal();
				script.SetValueFromCode(item.Field, item.NewValue, item.Action, item.Uid);
			}
			else if (item.Action == GreyList.ActionType.ModifyExtensionBit)
			{
				item.OldValue = script.GetValueFromCode(item.Field).GetPropertyValue<byte[]>("Raw").ToHexadecimal();
				script.SetValueFromCode(item.Field, item.NewValue, item.Action, uid);
			}
			else if (item.Action == GreyList.ActionType.ModifyField)
			{
				item.OldValue = script.GetValueFromCode(item.Field).GetPropertyValue<byte[]>("Raw").ToHexadecimal();
				script.SetValueFromCode(item.Field, item.NewValue, item.Action, uid);
			}
			else if (item.Action == GreyList.ActionType.ModifyStartDate)
			{
				item.OldValue = script.GetValueFromCode(item.Field).GetPropertyValue<byte[]>("Raw").ToHexadecimal();
				script.SetValueFromCode(item.Field, item.NewValue, item.Action, uid);
			}
		}
		#endregion ExecuteLGAsync

		#region BlockRecovery
		public async Task BlockRecoveryAsync(long uid, TransportCardGetReadInfoScript script, DateTime now, MifareClassicBlock block, MifareClassicBlock blockCopy)
		{
			var operationDamage = (await TransportOperationRepository.GetAsync())
				.Where(x =>
					x.Uid == uid &&
					x.ConfirmationDate == null &&
					x.Script != ""
				)
				.OrderByDescending(x => x.OperationDate)
				.FirstOrDefault();

			var check = await script.Card.CheckAsync(uid, block);
			var copyCheck = await script.Card.CheckAsync(uid, blockCopy);

			if (operationDamage != null)
			{
				if (
					(Enumerable.SequenceEqual(block.Value, blockCopy.Value)) &&
					check &&
					copyCheck
				)
					operationDamage.ConfirmationDate = now;
				else if (!Enumerable.SequenceEqual(block.Value, blockCopy.Value))
				{
					block.Set((byte[])blockCopy.Value.Clone()); // Grabar copia en original

					operationDamage.ConfirmationDate = now;
					operationDamage.DateTimeEventError = now; // ErrorMessage = "Tarjeta recuperada desde el valor anterior";
				}
				else if (!check)
				{
					block.Set((byte[])blockCopy.Value.Clone()); // Grabar copia en original

					// TODO: si monedero actualizar saldo original y copia

					operationDamage.ConfirmationDate = now;
					operationDamage.DateTimeEventError = now; // ErrorMessage = "Tarjeta recuperada desde el valor anterior";
				}
			}
			else if (operationDamage == null)
			{
				if (!copyCheck)
				{
					// Avisar de tarjeta estropeada
					throw new ApplicationException(TransportResources.CardDamage);

					// TODO: RECONSTRUIR
				}
				else if (!Enumerable.SequenceEqual((IEnumerable<byte>)block.Value, (IEnumerable<byte>)blockCopy.Value))
					block.Set((byte[])blockCopy.Value.Clone()); // Copiar datos de copia a original
			}

		}
		#endregion BlockRecovery
	}
}
