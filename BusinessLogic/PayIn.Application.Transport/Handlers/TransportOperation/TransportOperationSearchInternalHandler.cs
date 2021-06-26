using PayIn.Application.Dto.Results;
using PayIn.Application.Dto.Transport.Arguments.TransportOperation.Internal;
using PayIn.Application.Transport.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Infrastructure.Transport;
using PayIn.Infrastructure.Transport.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Domain.Transport;
using Xp.Infrastructure;

namespace PayIn.Application.Public.Handlers
{
	public class TransportCardSearchInternalHandler :
		IQueryBaseHandler<TransportOperationSearchInternalArguments, ServiceCardReadInfoResult>
	{
		public class TransportCardReadInfoResultTemp : ServiceCardReadInfoResult
		{
			public int? MaxExternalTransfers { get; set; }
			public int? MaxPeopleInTransfer { get; set; }
		}

		public readonly ISessionData SessionData;
		public readonly EigeService EigeService;
		public readonly SigapuntService SigapuntService;
		public readonly EmtService EmtService;
		public readonly FgvService FgvService;
		public readonly EmailService EmailService;
		public readonly IEntityRepository<TransportOperation> TransportOperationRepository;
		public readonly IEntityRepository<TransportTitle> TransportTitleRepository;

		#region Constructors
		public TransportCardSearchInternalHandler(
			ISessionData sessionData,
			EigeService eigeService,
			SigapuntService sigapuntService,
			EmtService emtService,
			FgvService fgvService,
			EmailService emailService,
			IEntityRepository<TransportOperation> transportOperationRepository,
			IEntityRepository<TransportTitle> transportTitleRepository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (eigeService == null) throw new ArgumentNullException("eigeService");
			if (sigapuntService == null) throw new ArgumentNullException("sigapuntService");
			if (emtService == null) throw new ArgumentNullException("emtService");
			if (fgvService == null) throw new ArgumentNullException("fgvService");
			if (emailService == null) throw new ArgumentNullException("emailService");
			if (transportOperationRepository == null) throw new ArgumentNullException("transportOperationRepository");
			if (transportTitleRepository == null) throw new ArgumentNullException("transportTitleRepository");

			SessionData = sessionData;
			EigeService = eigeService;
			SigapuntService = sigapuntService;
			EmtService = emtService;
			FgvService = fgvService;
			EmailService = emailService;
			TransportOperationRepository = transportOperationRepository;
			TransportTitleRepository = transportTitleRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceCardReadInfoResult>> ExecuteAsync(TransportOperationSearchInternalArguments arguments)
		{
#if TEST
			try
			{
#endif
			var now = DateTime.Now;
			var uid = arguments.Cards.FromHexadecimal().ToInt32().Value;

			// Obtain Scripts
			Exception exception = null;
			SigapuntScript sigapuntScript = null;
			try
			{
				sigapuntScript = await SigapuntService.GetData(uid);
			}
			catch (CardNotFoundException) { }
			catch (ScrapFormatException ex)
			{
				await EmailService.SendAsync("info@pay-in.es", "Scrap error de " + ex.Owner + " del usuario " + SessionData.Login, ex.Message);
				exception = ex;
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			EmtScript emtScript = null;
			try
			{
				emtScript = await EmtService.GetData(uid);
			}
			catch (CardNotFoundException) { }
			catch (ScrapFormatException ex)
			{
				await EmailService.SendAsync("info@pay-in.es", "Scrap error de " + ex.Owner + " del usuario " + SessionData.Login, ex.Message);
				exception = ex;
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			FgvScript fgvScript = null;
			try
			{
				fgvScript = await FgvService.GetData(uid);
			}
			catch (CardNotFoundException) { }
			catch (ScrapFormatException ex)
			{
				await EmailService.SendAsync("info@pay-in.es", "Scrap error de " + ex.Owner + " del usuario " + SessionData.Login, ex.Message);
				exception = ex;
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			// Save for log XAVI
			if (
				(sigapuntScript == null) &&
				(emtScript == null) &&
				(fgvScript == null)
			)
			{
				if (exception is ScrapExpiredPriceException)
					throw exception;
				throw new ArgumentException("Tarjeta no encontrada", exception);
			}

			arguments.Script =
				(sigapuntScript != null ? sigapuntScript.Data : new Dictionary<string, string>())
				.Union(emtScript != null ? emtScript.Data : new Dictionary<string, string>())
				.Union(fgvScript != null ? fgvScript.Data : new Dictionary<string, string>())
				.ToDictionary(x => x.Key, x => x.Value);

			var transportOperation = new TransportOperation
			{
				OperationDate = now.ToUTC(),
				OperationType = OperationType.Search,
				Uid = arguments.Uids,
				Login = SessionData.Login
			};
			await TransportOperationRepository.AddAsync(transportOperation);

            #region Cargar support
            var transportCardSupport = await EigeService.GetSupportCardAsync(uid, sigapuntScript, now);
            #endregion Cargar support

            #region Cargar titulos / monedero / bonus
            var list = new List<TransportCardReadInfoResultTemp>();
			var unknownTitles = new List<TransportCardReadInfoResultTemp>();

			if (sigapuntScript != null)
			{
				if (
					((await EigeService.GetTitleActive1Async(uid, sigapuntScript)) == true) &&
					((await EigeService.GetTitleCode1Async(uid, sigapuntScript)) != null)
				)
				{
					var item = new TransportCardReadInfoResultTemp
					{
						Code = await EigeService.GetTitleCode1Async(uid, sigapuntScript),
						Name = await EigeService.GetTitleName1Async(uid, sigapuntScript),
						OwnerName = await EigeService.GetTitleOwnerName1Async(uid, sigapuntScript, fgvScript, emtScript),
						Zone = await EigeService.GetTitleZoneName1Async(uid, sigapuntScript),
						Caducity = await EigeService.GetTitleCaducity1Async(uid, sigapuntScript),
						IsRechargable = await EigeService.IsRechargable1Async(uid, sigapuntScript),
						HasTariff = await EigeService.HasTariff1Async(uid, sigapuntScript),
						IsExhausted = await EigeService.GetTitleIsExhausted1Async(uid, sigapuntScript, now),
						IsExpired = await EigeService.GetTitleIsExpired1Async(uid, sigapuntScript, now),
						// MaxExternalTransfers = await EigeService.GetTitleMaxExternalTransfers1Async(uid, script),
						// MaxPeopleInTransfer = await EigeService.GetTitleMaxPeopleInTransfer1Async(uid, script),*/
						// Balance
						HasBalance = false, //await EigeService.GetTitleHasBalance1Async(uid, script),
						Balance = await EigeService.GetTitleBalance1Async(uid, sigapuntScript),
						//BalanceAcumulated = await EigeService.GetTitleBalanceAcumulated1Async(uid, sigapuntScript),
						BalanceUnits = await EigeService.GetTitleBalanceUnits1Async(uid, sigapuntScript),
						// Temporal
						IsTemporal = await EigeService.GetTitleIsTemporal1Async(uid, sigapuntScript),
						ExhaustedDate = (await EigeService.GetTitleExhaustedDate1Async(uid, sigapuntScript)),
						ActivatedDate = (await EigeService.GetTitleActivatedDate1Async(uid, sigapuntScript)),
						Ampliation = await EigeService.GetTitleAmpliation1Async(uid, sigapuntScript),
						AmpliationQuantity = await EigeService.GetTitleAmpliationQuantity1Async(uid, sigapuntScript),
						AmpliationUnits = await EigeService.GetTitleAmpliationUnits1Async(uid, sigapuntScript),
						ReadAll = false
					};
					list.Add(item);
					if ((await EigeService.GetTitleCode1Async(uid, sigapuntScript)) == null)
						unknownTitles.Add(item);

					transportOperation.OperationTitles.Add(new TransportOperationTitle
					{
						Title = (await TransportTitleRepository.GetAsync())
						.Where(x =>
							x.Code == item.Code.Value
						)
						.FirstOrDefault(),
						Zone = item.Zone,
						Quantity = item.Balance,
						Caducity = item.Caducity != null ? item.Caducity.Value : (DateTime?)null
					});
				}
				if (
					((await EigeService.GetTitleActive2Async(uid, sigapuntScript)) == true) &&
					((await EigeService.GetTitleCode2Async(uid, sigapuntScript)) != null)
				)
				{
					var item = new TransportCardReadInfoResultTemp
					{
						Code = await EigeService.GetTitleCode2Async(uid, sigapuntScript),
						Name = await EigeService.GetTitleName2Async(uid, sigapuntScript),
						OwnerName = await EigeService.GetTitleOwnerName2Async(uid, sigapuntScript, fgvScript, emtScript),
						Zone = await EigeService.GetTitleZoneName2Async(uid, sigapuntScript),
						Caducity = await EigeService.GetTitleCaducity2Async(uid, sigapuntScript),
						IsRechargable = await EigeService.IsRechargable2Async(uid, sigapuntScript),
						HasTariff = await EigeService.HasTariff2Async(uid, sigapuntScript),
						IsExhausted = await EigeService.GetTitleIsExhausted2Async(uid, sigapuntScript, now),
						IsExpired = await EigeService.GetTitleIsExpired2Async(uid, sigapuntScript, now),
						// MaxExternalTransfers = await EigeService.GetTitleMaxExternalTransfers2Async(uid, script),
						// MaxPeopleInTransfer = await EigeService.GetTitleMaxPeopleInTransfer2Async(uid, script),
						// Balance
						HasBalance = false, //await EigeService.GetTitleHasBalance2Async(uid, script),
						Balance = await EigeService.GetTitleBalance2Async(uid, sigapuntScript),
						//BalanceAcumulated = await EigeService.GetTitleBalanceAcumulated1Async(uid, sigapuntScript),
						BalanceUnits = await EigeService.GetTitleBalanceUnits2Async(uid, sigapuntScript),
						// Temporal
						IsTemporal = await EigeService.GetTitleIsTemporal2Async(uid, sigapuntScript),
						ExhaustedDate = (await EigeService.GetTitleExhaustedDate2Async(uid, sigapuntScript)),
						ActivatedDate = (await EigeService.GetTitleActivatedDate2Async(uid, sigapuntScript)),
						Ampliation = await EigeService.GetTitleAmpliation2Async(uid, sigapuntScript),
						AmpliationQuantity = await EigeService.GetTitleAmpliationQuantity2Async(uid, sigapuntScript),
						AmpliationUnits = await EigeService.GetTitleAmpliationUnits2Async(uid, sigapuntScript),
						ReadAll = false
					};
					list.Add(item);
					if ((await EigeService.GetTitleCode2Async(uid, sigapuntScript)) == null)
						unknownTitles.Add(item);

					transportOperation.OperationTitles.Add(new TransportOperationTitle
					{
						Title = (await TransportTitleRepository.GetAsync())
						.Where(x =>
							x.Code == item.Code.Value
						)
						.FirstOrDefault(),
						Zone = item.Zone,
						Quantity = item.Balance,
						Caducity = item.Caducity != null ? item.Caducity.Value : (DateTime?)null
					});
				}
				if ((await EigeService.GetTitleActiveMAsync(uid, sigapuntScript)) == true)
				{
					var item = new TransportCardReadInfoResultTemp
					{
						Code = await EigeService.GetTitleCodeMAsync(uid, sigapuntScript),
						Name = await EigeService.GetTitleNameMAsync(uid, sigapuntScript),
						OwnerName = await EigeService.GetTitleOwnerNameMAsync(uid, sigapuntScript, fgvScript, emtScript),
						Zone = await EigeService.GetTitleZoneNameMAsync(uid, sigapuntScript),
						Caducity = await EigeService.GetTitleCaducityMAsync(uid, sigapuntScript),
						IsRechargable = await EigeService.IsRechargableMAsync(uid, sigapuntScript),
						HasTariff = await EigeService.HasTariffMAsync(uid, sigapuntScript),
						IsExhausted = await EigeService.GetTitleIsExhaustedMAsync(uid, sigapuntScript, now),
						IsExpired = await EigeService.GetTitleIsExpiredMAsync(uid, sigapuntScript, now),
						// MaxExternalTransfers = await EigeService.GetTitleMaxExternalTransfersMAsync(uid, script),
						// MaxPeopleInTransfer = await EigeService.GetTitleMaxPeopleInTransferMAsync(uid, script),
						// Balance
						HasBalance = true, //await EigeService.GetTitleHasBalanceMAsync(uid, script),
						Balance = await EigeService.GetTitleBalanceMAsync(uid, sigapuntScript),
						//BalanceAcumulated = await EigeService.GetTitleBalanceAcumulated1Async(uid, sigapuntScript),
						BalanceUnits = await EigeService.GetTitleBalanceUnitsMAsync(uid, sigapuntScript),
						// Temporal
						IsTemporal = await EigeService.GetTitleIsTemporalMAsync(uid, sigapuntScript),
						ExhaustedDate = (await EigeService.GetTitleExhaustedDateMAsync(uid, sigapuntScript)),
						ActivatedDate = (await EigeService.GetTitleActivatedDateMAsync(uid, sigapuntScript)),
						Ampliation = await EigeService.GetTitleAmpliationMAsync(uid, sigapuntScript),
						AmpliationQuantity = await EigeService.GetTitleAmpliationQuantityMAsync(uid, sigapuntScript),
						AmpliationUnits = await EigeService.GetTitleAmpliationUnitsMAsync(uid, sigapuntScript),
						ReadAll = false
					};
					list.Add(item);

					//transportOperation.OperationTitles.Add(new TransportOperationTitle
					//{
					//	Title = (await TransportTitleRepository.GetAsync())
					//	.Where(x =>
					//		x.Code == item.Code.Value
					//	)
					//	.FirstOrDefault(),
					//	Zone = item.Zone,
					//	Quantity = item.Balance,
					//	Caducity = item.Caducity != null ? item.Caducity.Value : (DateTime?)null
					//});
				}
				if ((await EigeService.GetTitleActiveBAsync(uid, sigapuntScript)) == true)
				{
					var item = new TransportCardReadInfoResultTemp
					{
						Code = await EigeService.GetTitleCodeBAsync(uid, sigapuntScript),
						Name = await EigeService.GetTitleNameBAsync(uid, sigapuntScript),
						OwnerName = await EigeService.GetTitleOwnerNameBAsync(uid, sigapuntScript, fgvScript, emtScript),
						Zone = await EigeService.GetTitleZoneNameBAsync(uid, sigapuntScript),
						Caducity = await EigeService.GetTitleCaducityBAsync(uid, sigapuntScript),
						IsRechargable = await EigeService.IsRechargableBAsync(uid, sigapuntScript),
						HasTariff = await EigeService.HasTariffBAsync(uid, sigapuntScript),
						IsExhausted = await EigeService.GetTitleIsExhaustedBAsync(uid, sigapuntScript, now),
						IsExpired = await EigeService.GetTitleIsExpiredBAsync(uid, sigapuntScript, now),
						// MaxExternalTransfers = await EigeService.GetTitleMaxExternalTransfersBAsync(uid, script),
						// MaxPeopleInTransfer = await EigeService.GetTitleMaxPeopleInTransferBAsync(uid, script),
						// Balance
						HasBalance = true, //await EigeService.GetTitleHasBalanceBAsync(uid, script),
						Balance = await EigeService.GetTitleBalanceBAsync(uid, sigapuntScript),
						//BalanceAcumulated = await EigeService.GetTitleBalanceAcumulated1Async(uid, sigapuntScript),
						BalanceUnits = await EigeService.GetTitleBalanceUnitsBAsync(uid, sigapuntScript),
						// Temporal
						IsTemporal = await EigeService.GetTitleIsTemporalBAsync(uid, sigapuntScript),
						ExhaustedDate = (await EigeService.GetTitleExhaustedDateBAsync(uid, sigapuntScript)),
						ActivatedDate = (await EigeService.GetTitleActivatedDateBAsync(uid, sigapuntScript)),
						Ampliation = await EigeService.GetTitleAmpliationBAsync(uid, sigapuntScript),
						AmpliationQuantity = await EigeService.GetTitleAmpliationQuantityBAsync(uid, sigapuntScript),
						AmpliationUnits = await EigeService.GetTitleAmpliationUnitsBAsync(uid, sigapuntScript),
						ReadAll = false
					};
					list.Add(item);

					//transportOperation.OperationTitles.Add(new TransportOperationTitle
					//{
					//	Title = (await TransportTitleRepository.GetAsync())
					//	.Where(x =>
					//		x.Code == item.Code.Value
					//	)
					//	.FirstOrDefault(),
					//	Zone = item.Zone,
					//	Quantity = item.Balance,
					//	Caducity = item.Caducity != null ? item.Caducity.Value : (DateTime?)null
					//});
				}
			}
			if (emtScript != null)
			{
				for (int i = 0; i < await EmtService.GetTitleCountAsync(uid, emtScript); i++)
				{
					var item = new TransportCardReadInfoResultTemp
					{
						Code = Convert.ToInt32(await EmtService.GetTitleCodeAsync(uid, emtScript,i)),
						Name = await EmtService.GetTitleNameAsync(uid, emtScript, i),
						OwnerName = await EigeService.GetTitleOwnerNameAsync(uid, emtScript),
						Zone = await EmtService.GetTitleZoneNameAsync(uid, emtScript, i),
						//Caducity = await EigeService.GetTitleCaducityBAsync(uid, Script),
						IsRechargable = false, // await EigeService.IsRechargableBAsync(uid, Script),
						HasTariff = false, // await EigeService.HasTariffBAsync(uid, Script),
						IsExhausted = await EmtService.GetTitleIsExhaustedAsync(uid, emtScript, i, now),
						IsExpired = await EmtService.GetTitleIsExpiredAsync(uid, emtScript, i, now),
						//// MaxExternalTransfers = await EigeService.GetTitleMaxExternalTransfersBAsync(uid, script),
						//// MaxPeopleInTransfer = await EigeService.GetTitleMaxPeopleInTransferBAsync(uid, script),
						//// Balance
						HasBalance = await EmtService.GetTitleHasBalanceAsync(uid, emtScript, i),
						Balance = await EmtService.GetTitleBalanceAsync(uid, emtScript, i),
						//BalanceAcumulated = await EigeService.GetTitleBalanceAcumulated1Async(uid, sigapuntScript),
						BalanceUnits = await EmtService.GetTitleBalanceUnitsAsync(uid, emtScript, i),
						//// Temporal
						IsTemporal = await EmtService.GetTitleIsTemporalAsync(uid, emtScript, i),
						//ExhaustedDate = (await EigeService.GetTitleExhaustedDateBAsync(uid, Script)).ToUTC(),
						//ActivatedDate = (await EigeService.GetTitleActivatedDate1Async(uid, emtScript)).ToUTC(),
						//Ampliation = await EigeService.GetTitleAmpliationBAsync(uid, Script),
						//AmpliationQuantity = await EigeService.GetTitleAmpliationQuantityBAsync(uid, Script),
						//AmpliationUnits = await EigeService.GetTitleAmpliationUnitsBAsync(uid, Script),
						ReadAll = false
					};

					// Clear list
					var titleRepeated = list
						.Where(x =>
							x.Name == item.Name &&
							x.Zone == item.Zone
						)
						.FirstOrDefault();
					if (titleRepeated != null)
						list.Remove(titleRepeated);
					else if (unknownTitles.Count() > 0)
					{
						list.Remove(unknownTitles[0]);
						unknownTitles.RemoveAt(0);
					}

					// Add to list
					list.Add(item);

					transportOperation.OperationTitles.Add(new TransportOperationTitle
					{
						Title = (await TransportTitleRepository.GetAsync())
						.Where(x =>
							x.Code == item.Code.Value
						)
						.FirstOrDefault(),
						Zone = item.Zone,
						Quantity = item.Balance,
						Caducity = item.Caducity != null ? item.Caducity.Value : (DateTime?)null
					});
				}
			}
			if (fgvScript != null)
			{
				for (int i = 0; i < await FgvService.GetTitleCountAsync(uid, fgvScript); i++)
				{
					var item = new TransportCardReadInfoResultTemp
					{
						Code = await FgvService.GetTitleCodeAsync(uid, fgvScript, i),
						Name = await FgvService.GetTitleNameAsync(uid, fgvScript, i),
						OwnerName = await EigeService.GetTitleOwnerNameAsync(uid, fgvScript),
						Zone = await FgvService.GetTitleZoneNameAsync(uid, fgvScript, i),
						//Caducity = await EigeService.GetTitleCaducityBAsync(uid, Script),
						IsRechargable = false, // await EigeService.IsRechargableBAsync(uid, Script),
						HasTariff = false, // await EigeService.HasTariffBAsync(uid, Script),
						IsExhausted = await FgvService.GetTitleIsExhaustedAsync(uid, fgvScript, i, now),
						IsExpired = await FgvService.GetTitleIsExpiredAsync(uid, fgvScript, i, now),
						//// MaxExternalTransfers = await EigeService.GetTitleMaxExternalTransfersBAsync(uid, script),
						//// MaxPeopleInTransfer = await EigeService.GetTitleMaxPeopleInTransferBAsync(uid, script),
						//// Balance
						HasBalance = await FgvService.GetTitleHasBalanceAsync(uid, fgvScript, i),
						Balance = await FgvService.GetTitleBalanceAsync(uid, fgvScript, i),
						BalanceAcumulated = await FgvService.GetTitleBalanceAcumulatedAsync(uid, fgvScript, i),
						BalanceUnits = await FgvService.GetTitleBalanceUnitsAsync(uid, fgvScript, i),
						//// Temporal
						IsTemporal = await FgvService.GetTitleIsTemporalAsync(uid, fgvScript, i),
						ExhaustedDate = await FgvService.GetTitleExhaustedDateAsync(uid, fgvScript, i),
						//ActivatedDate = (await EigeService.GetTitleActivatedDate1Async(uid, emtScript)).ToUTC(),
						//Ampliation = await EigeService.GetTitleAmpliationBAsync(uid, Script),
						//AmpliationQuantity = await EigeService.GetTitleAmpliationQuantityBAsync(uid, Script),
						//AmpliationUnits = await EigeService.GetTitleAmpliationUnitsBAsync(uid, Script),
						ReadAll = false
					};

					// Clear List
					var titleRepeated = list
						.Where(x =>
							x.Name == item.Name &&
							x.Zone == item.Zone
						)
						.FirstOrDefault();
					if (titleRepeated != null)
						list.Remove(titleRepeated);
					else if (unknownTitles.Count() > 0)
					{
						list.Remove(unknownTitles[0]);
						unknownTitles.RemoveAt(0);
					}

					// Add to list
					list.Add(item);

					if (item.Code != null) 
						transportOperation.OperationTitles.Add(new TransportOperationTitle
						{
							Title = (await TransportTitleRepository.GetAsync())
							.Where(x =>
								x.Code == item.Code
							)
							.ToList()
							.FirstOrDefault(),
							Zone = item.Zone,
							Quantity = item.Balance,
							Caducity = item.Caducity != null ? item.Caducity.Value : (DateTime?)null
						});
				}
			}
			#endregion Cargar titulos / monedero / bonus
            
			// Serializar tarjeta 
			return new ServiceCardReadInfoResultBase
			{
				CardId = arguments.Cards,
				Owner =
					sigapuntScript != null ? await EigeService.GetCardOwnerAsync(uid, sigapuntScript) :
					null,
				OwnerName = await EigeService.GetCardOwnerNameAsync(uid, sigapuntScript, fgvScript, emtScript),
				TypeName =
					sigapuntScript != null ? await EigeService.GetCardTypeNameAsync(uid, sigapuntScript) :
					"Tarj. leida de servidor",
				InBlackList = false, // await EigeService.InBlackListAsync(uid, script),
				ExpiredDate =
					sigapuntScript != null ? await EigeService.GetExpiredDateAsync(uid, sigapuntScript) :
					null, // No hay que invocar a ToUTC() porque ya está en horario local del móvil
				IsExpired =
					sigapuntScript != null ? await EigeService.IsCardExpiredAsync(uid, sigapuntScript, transportCardSupport, now) :
					false,
				IsRechargable =
					sigapuntScript != null ? (await EigeService.IsRechargable1Async(uid, sigapuntScript)) || (await EigeService.IsRechargable2Async(uid, sigapuntScript)) || (await EigeService.IsRechargableMAsync(uid, sigapuntScript)) || (await EigeService.IsRechargableBAsync(uid, sigapuntScript)) :
					false,
				HasHourValidity = false, //hasHourValidity,
				HasDayValidity = false, //hasDayValidity,
				ApproximateValues = true,
				//Logs = default(IEnumerable<TransportCardReadInfoResultBase.Log>);
				//Charges = default(IEnumerable<TransportCardReadInfoResultBase.Log>);
				Data = list,
				IsPersonalized =
					sigapuntScript != null ? await EigeService.IsPersonalizedAsync(uid, sigapuntScript) :
					false,
				IsDamaged = false, //await EigeService.IsDamagedAsync(uid, script),
				UserName =
					sigapuntScript != null ? await EigeService.GetUserNameAsync(uid, sigapuntScript) :
					"",
				UserSurname =
					sigapuntScript != null ? await EigeService.GetUserSurnameAsync(uid, sigapuntScript) :
					"",
				UserDni =
					sigapuntScript != null ? await EigeService.GetUserDniAsync(uid, sigapuntScript) :
					"",
				UserCode =
					sigapuntScript != null ? await EigeService.GetUserCodeAsync(uid, sigapuntScript) :
					null,
				LastValidationDate = null, //await EigeService.GetLastValidationDateAsync(uid, script),
				LastValidationTypeName = "", //await EigeService.GetLastValidationTypeNameAsync(uid, script),
				LastValidationPlace = "", // await EigeService.GetLastValidationPlaceAsync(uid, script),
				LastValidationOperator = "", //await EigeService.GetLastValidationOperatorAsync(uid, script),
				LastValidationZone = null, //lastValidation == null ? null : lastValidation.Zone,
				LastValidationTitleName = "", //lastValidation == null ? "" : lastValidation.TitleName,
				LastValidationTitleOwnerName = "", // lastValidation == null ? "" : lastValidation.TitleOwnerName,
				LastValidationTitleZone = null, //lastValidation == null ? null : lastValidation.TitleZone,
				PeopleTraveling = 0, //await EigeService.GetLastValidationPeopleTravelingAsync(uid, script),
				PeopleInTransfer = 0, //await EigeService.GetLastValidationPeopleInTransferAsync(uid, script),
				MaxPeopleInTransfer = 0, //lastValidationTitle == null ? 0 : lastValidationTitle.MaxPeopleInTransfer,
				InternalTransfers = 0, //await EigeService.GetLastValidationInternalTransfersAsync(uid, script),
				ExternalTransfers = 0, //await EigeService.GetLastValidationExternalTransfersAsync(uid, script),
				MaxInternalTransfers = 0, //await EigeService.GetLastValidationMaxInternalTransfersAsync(uid, script),
				MaxExternalTransfers = 0, //lastValidationTitle == null ? 0 : lastValidationTitle.MaxExternalTransfers,
				Mode = EigeService.TransferMode,
				DeviceType = (EigeFechaPersonalizacion_DispositivoEnum?)null // await EigeService.GetDeviceTypeAsync(uid, script)
			};
#if TEST
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message + "\n\n" + ex.StackTrace);
			}
#endif
		}
		#endregion ExecuteAsync
	}
}
