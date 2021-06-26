using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Application.Dto.Transport.Results.TransportOperation;
using PayIn.Application.Transport.Scripts;
using PayIn.Application.Transport.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Promotions;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige;
using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using PayIn.Domain.Transport.MifareClassic.Operations;
using System;
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
	[XpLog("TransportOperation", "Recharge", RelatedId = "OperationId")]
	[XpAnalytics("TransportOperation", "Recharge")]
	public class TransportOperationRechargeHandler : TransportOperationInternal,
		IServiceBaseHandler<TransportOperationRechargeArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<TransportOperationTitle> TransportOperationTitleRepository;
		private readonly IEntityRepository<PromoExecution> PromoExecutionRepository;
		private readonly IEntityRepository<PromoUser> PromoUserRepository;
		private readonly IEntityRepository<Promotion> PromotionRepository;
		private readonly IEntityRepository<TransportTitle> TitleRepository;
		private readonly IEntityRepository<Ticket> TicketRepository;
		private readonly IEntityRepository<PaymentConcession> PaymentConcessionRepository;
		private readonly IEntityRepository<TransportOperation> TransportOperationRepository;
		private readonly IEntityRepository<TransportCardSupportTitleCompatibility> TransportCardRepository;
		private readonly IEntityRepository<TransportSimultaneousTitleCompatibility> TransportSimoultaneousRepository;
		private readonly IUnitOfWork UnitOfWork;
		private readonly IMifareClassicHsmService Hsm;
		private readonly TransportOperationReadInfoHandler TransportOperatinReadInfoRepository;

		#region Constructors
		public TransportOperationRechargeHandler(
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
			TransportOperationReadInfoHandler transportOperatinReadInfoRepository
		)
			: base(eigeService, priceRepository, blackListRepository, greyListRepository)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (transportOperationTitleRepository == null) throw new ArgumentNullException("transportOperationTitleRepository");
			if (titleRepository == null) throw new ArgumentNullException("titleRepository");
			if (ticketRepository == null) throw new ArgumentNullException("ticketRepository");
			if (promoExecutionRepository == null) throw new ArgumentNullException("promoExecutionRepository");
			if (promoUserRepository == null) throw new ArgumentNullException("promoUserRepository");
			if (promotionRepository == null) throw new ArgumentNullException("promotionRepository");
			if (paymentConcessionRepository == null) throw new ArgumentNullException("paymentConcessionRepository");
			if (transportOperationRepository == null) throw new ArgumentNullException("transportOperationRepository");
			if (transportSimoultaneousRepository == null) throw new ArgumentNullException("transportSimoultaneousRepository");
			if (transportCardRepository == null) throw new ArgumentNullException("transportCardRepository");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (hsm == null) throw new ArgumentNullException("hsm");
			if (transportOperatinReadInfoRepository == null) throw new ArgumentNullException("transportOperatinReadInfoRepository");

			SessionData = sessionData;
			TransportOperationTitleRepository = transportOperationTitleRepository;
			TitleRepository = titleRepository;
			TicketRepository = ticketRepository;
			PaymentConcessionRepository = paymentConcessionRepository;
			TransportOperationRepository = transportOperationRepository;
			TransportSimoultaneousRepository = transportSimoultaneousRepository;
			TransportCardRepository = transportCardRepository;
			UnitOfWork = unitOfWork;
			Hsm = hsm;
			TransportOperatinReadInfoRepository = transportOperatinReadInfoRepository;
			PromoExecutionRepository = promoExecutionRepository;
			PromotionRepository = promotionRepository;
			PromoUserRepository = promoUserRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TransportOperationRechargeArguments arguments)
		{
#if TEST || DEBUG
			try
			{
#endif
				var uid = arguments.CardId.FromHexadecimal().ToInt32().Value;
				var script = new TransportCardGetReadInfoScript(SessionData.Login, Hsm, arguments.Script);
				script.Card.Uid = uid.ToHexadecimalBE().FromHexadecimal();
				var scriptPreviousSerialize = Newtonsoft.Json.JsonConvert.SerializeObject(script);
				var now = DateTime.Now.ToUTC();
				var fechaValidez1 = new EigeDateTime(null);
				var fechaValidez2 = new EigeDateTime(null);
				int? titlecodetoerase1 = null;
				int? titlecodetoerase2 = null;

				var titleCode1 = await EigeService.GetTitleCode1Async(uid, script);
				var titleZone1 = await EigeService.GetTitleZoneName1Async(uid, script);
				var titleTarifa1 = script.Card.Titulo.ControlTarifa1.Value;
				var titleActive1 = await EigeService.GetTitleActive1Async(uid, script);
				var titleIsExhausted1 = await EigeService.GetTitleIsExhausted1Async(uid, script, arguments.Now);
				var titleBalance1 = await EigeService.GetTitleBalance1Async(uid, script);
				var titleDateTime1 = script.Card.Titulo.FechaValidez1.Value;
				var titlePrice1 = (await PriceRepository.GetAsync("Title.TransportSimultaneousTitleCompatibility.TransportTitle2", "Title.TransportSimultaneousTitleCompatibility2.TransportTitle"))
					.Where(x =>
						x.Title.Code == titleCode1 &&
						(!x.Title.HasZone || x.Zone == titleZone1) &&
						x.Version == titleTarifa1
					)
					.FirstOrDefault();
				var titleCode2 = await EigeService.GetTitleCode2Async(uid, script);
				var titleZone2 = await EigeService.GetTitleZoneName2Async(uid, script);
				var titleTarifa2 = script.Card.Titulo.ControlTarifa2.Value;
				var titleActive2 = await EigeService.GetTitleActive2Async(uid, script);
				var titleIsExhausted2 = await EigeService.GetTitleIsExhausted2Async(uid, script, arguments.Now);
				var titleBalance2 = await EigeService.GetTitleBalance2Async(uid, script);
				var titleDateTime2 = script.Card.Titulo.FechaValidez2.Value;
				var titlePrice2 = (await PriceRepository.GetAsync("Title.TransportSimultaneousTitleCompatibility.TransportTitle2", "Title.TransportSimultaneousTitleCompatibility2.TransportTitle"))
					.Where(x =>
						x.Title.Code == titleCode2 &&
						(!x.Title.HasZone || x.Zone == titleZone2) &&
						x.Version == titleTarifa2
					)
					.FirstOrDefault();
				var hasInstant = (arguments.Promotions.Where(x => x.Launcher == Common.PromoLauncherType.Instant).FirstOrDefault() != null) ? true : false;

				// Obtener el precio
				var price = (await GetRechargesAsync(uid, script, arguments.Now, arguments.Code, new[] { arguments.PriceId }, 1, checkMaxBalance: false));
				if (price == null)
					throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargableInCard);
				if (price.Id != arguments.PriceId)
					throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargable);

				// Obtener la configuración de precio
				var config = await GetRechargeConfigAsync(uid, script, arguments.Now, price);
				if (config == null)
					throw new ApplicationException(TransportCardRechargeResources.ExceptionCompatibility);

				// 1.- Comprobación LN
				if (await InBlackListAsync(uid, script))
					throw new ApplicationException(TransportCardRechargeResources.ExceptionInBlackList);

				// 2.- Comprobación LG
				if (await InGreyListAsync(uid, script))
					throw new ApplicationException(TransportCardRechargeResources.ExceptionInGreyList);

				// 3.- Comprobación fecha caducidad de tarjeta
				if (
					(price.Title.OwnerCode == EigeService.EigeCode) &&
					(script.Card.Tarjeta.CodigoEntorno.Value == EigeCodigoEntornoTarjetaEnum.Valencia) &&
					(script.Card.Tarjeta.EmpresaPropietaria.Value == EigeService.EigeCode)
				)
				{
					if (arguments.Now >= (await EigeService.GetExpiredDateAsync(uid, script)))
						throw new ApplicationException(TransportCardRechargeResources.ExceptionExpiration);
					if (script.Card.Titulo.TituloEnAmpliacion1.Value && script.Card.Titulo.FechaValidez1.Value > now)
						throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargable);
				}

				// 4.- Comprobación límite superior de viajes
				// Se comprueba después de recarga

				// 5.- Comprobación que no tiene domiciliación bancaria (solo recarga)
				if (
					(arguments.RechargeType == RechargeType.Recharge) &&
					(script.Card.Usuario.TieneDomiciliacionBancaria.Value)
				)
					throw new ApplicationException(TransportCardRechargeResources.ExceptionBank);

				// 6.- Comprobar el ticket
				var ticket = (await TicketRepository.GetAsync())
						.Where(x => x.Id == arguments.TicketId)
						.ToList()
						.LastOrDefault();
				if (hasInstant)
				{
					// Si tiene promociones instantaneas no se ha de comprobar si el tiquet es de 0€
				}
				else if (config.Slot == EigeTituloEnUsoEnum.Titulo1)
				{
					if (ticket == null)
						throw new ArgumentNullException("ticketId");

					if (!titleActive1)
					{
						if ((price.Price != ticket.Amount))// && (EigeService.IsTuiN(titleCode1)))
							throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargable);
					}
					else if (titlePrice1 == null)
					{
						if (price.Price != ticket.Amount)
							throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargable);
					}
					else if (config.RechargeType == RechargeType.Replace)
					{
						if (price.Price != ticket.Amount)
							throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargable);
					}
					else if ((titlePrice1.Title.Quantity > 0) && (ticket.Amount > 0)) // && !EigeService.IsTuiN(arguments.Code)) //bono
					{
						if (config.RechargeType == RechargeType.Exchange)//canje de título (AT-BT) - devuelve el importe del título anterior
						{
							if (price.Price != ticket.Amount - config.ChangePrice)
								throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargable);
						}
                        // Chapuza
						//else if (
						//	((price.Price * arguments.Quantity + (price.Price - titlePrice1.Price) * script.Card.Titulo.SaldoViaje1.Value / 10) != ticket.Amount) && 
						//	(config.RechargeType != RechargeType.RechargeExpiredPrice)
						//)
						//	throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargable);
					}
					else if ((titlePrice1.Title.TemporalUnit > 0 && titlePrice1.Title.Quantity == 0)) //&& !EigeService.IsTuiN(arguments.Code)) // abono
					{
						if (config.RechargeType == RechargeType.Exchange)
						{
							if (price.Price != ticket.Amount - config.ChangePrice)
								throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargable);
						}
						else
						{
							if (price.Price != ticket.Amount)
								throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargable);
						}
					}
				}
				else if (config.Slot == EigeTituloEnUsoEnum.Titulo2)
				{
					if (ticket == null)
						throw new ArgumentNullException("ticketId");

					if (!titleActive2)
					{
						if (price.Price != ticket.Amount)
							throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargable);
					}
					else if (titlePrice2 == null)
					{
						if (price.Price != ticket.Amount)
							throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargable);
					}
					else if (config.RechargeType == RechargeType.Replace)
					{
						if (price.Price != ticket.Amount)
							throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargable);
					}
					else if ((titlePrice2.Title.Quantity > 0) && (ticket.Amount > 0)) // bono
					{
						if ((price.Price + (price.Price - titlePrice2.Price) * script.Card.Titulo.SaldoViaje2.Value / 10) != ticket.Amount && config.RechargeType != RechargeType.RechargeExpiredPrice)
							throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargable);
					}
					else if (titlePrice2.Title.TemporalUnit > 0 && titlePrice2.Title.Quantity == 0) // abono
					{
						if (config.RechargeType == RechargeType.Exchange)
						{
							if (price.Price != ticket.Amount - config.ChangePrice)
								throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargable);
						}
						else
						{
							if (price.Price != ticket.Amount)
								throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargable);
						}
					}
				}

				// 7.-Comprobamos si hay alguna promoción
				var promotionIds = arguments.Promotions
					.Select(x => x.Id);
				var promotions = (await PromoExecutionRepository.GetAsync("Promotion.PromoActions", "PromoUser"))
					.Where(x =>
						promotionIds.Contains(x.Id) &&
						x.AppliedDate == null &&
						x.PromoUser.Login == SessionData.Login &&
						x.Promotion.StartDate <= arguments.Now &&
						x.Promotion.EndDate >= arguments.Now &&
						x.Promotion.State == Common.PromotionState.Active &&
						x.Promotion.PromoLaunchers
							.Where(y => y.Type == Common.PromoLauncherType.Instant)
							.Any() &&
						//x.Promotion.PromoPrices
							//.Where(y =>
							//	(
							//		y.TransportPrice.Title.Code == titleCode1 &&
							//		y.TransportPrice.Zone == titleZone1
							//	) ||
							//	(
							//		y.TransportPrice.Title.Code == titleCode2 &&
							//		y.TransportPrice.Zone == titleZone2
							//	)
							//)
							//.Any() &&
						x.Promotion.PromoPrices
							.Select(y => y.TransportPriceId == price.Id)
							.FirstOrDefault()
						)
						.ToList();
				var promotionTravels = promotions
					.Sum(x => (int?)(x.Promotion.PromoActions
						.Sum(y => (int?)y.Quantity) ?? 0)
					) ?? 0;

				// Carga				
				if (
					(config.RechargeType == RechargeType.Charge) ||
					(config.RechargeType == RechargeType.Replace) ||
					(config.RechargeType == RechargeType.Exchange)
				)
				{
					if (config.Slot == EigeTituloEnUsoEnum.Titulo1)
					{
						titlecodetoerase2 = script.Card.Titulo.CodigoTitulo2.Value;
						// //Tuin
						// if (EigeService.IsTuiN(titleCode1) || EigeService.IsTuiN(arguments.Code))
						// {
						// 	//ToDo - Algunos datos inventados - Verificar cuales son necesarios
						// 	script.Card.TituloTuiN.SaldoViaje_Value = new EigeCurrency(Convert.ToInt32(EigeService.SaldoTuiN(0, ticket, price.Title.Quantity)), 20);
						// 	script.Card.TituloTuiN.UltimaValidacionTipo = new EigeBytes(new Byte[] { 0x00 }, 4);
						// 	script.Card.TituloTuiN.UltimaValidacionEstacion = new EigeInt8(0);
						// 	script.Card.TituloTuiN.UltimaValidacionFechaHora = new EigeDateTime(null);
						// 	script.Card.TituloTuiN.NumeroPersonasViajando = new EigeInt8(0);
						// 	script.Card.TituloTuiN.Temporalidad = new GenericEnum<EigeTipoTemporalidad_TuiNEnum>(new Byte[] { (titleCode1 == 1271) ? Convert.ToByte(4) : Convert.ToByte(5) }, 4);
						// 	script.Card.TituloTuiN.VersionClaves = new EigeBytes(new Byte[] { 0x00 }, 3);
						// 	script.Card.TituloTuiN.Tiene8Historicos = new EigeBool(false);
						// 	script.Card.TituloTuiN.IndiceTransaccion = new EigeInt8(0);
						// 	script.Card.TituloTuiN.PersonasDeInicio = new EigeBytes(new Byte[] { 0x00 }, 6);
						// 	script.Card.TituloTuiN.CobradoPrimerViajero = new EigeCurrency(new Byte[] { 0x00 }, 12);
						// 	script.Card.TituloTuiN.BonificadoPrimerViajero = new EigeCurrency(new Byte[] { 0x00 }, 12);
						// 	script.Card.TituloTuiN.ContadorTransbordosInternos = new EigeInt8(0);
						// 	script.Card.TituloTuiN.TiempoConsumido = new EigeInt8(0);
						// 	script.Card.TituloTuiN.EstacionInicioViaje = new EigeInt8(0);
						// 	script.Card.TituloTuiN.TipoTarifa = new EigeInt8(0);
						// 	script.Card.TituloTuiN.Cobrado = new EigeCurrency(0, 14);
						// 	script.Card.TituloTuiN.ConsumoAcumulado = new EigeCurrency(0, 16);
						// 	script.Card.TituloTuiN.Sentido = new EigeBytes(new Byte[] { 0x00 }, 2);
						// 	script.Card.TituloTuiN.SaldoViaje_Sign = new EigeBool(true);

						// 	script.Card.Titulo.TipoUnidadesValidezTemporal1 = new GenericEnum<EigeTipoUnidadesValidezTemporalEnum>(new Byte[] { Convert.ToByte(3) }, 4);
						// 	script.Card.Titulo.NumeroUnidadesValidezTemporal1 = new EigeInt8(255);
						// 	script.Card.Validacion.MaxTransbordosInternos = new EigeInt8(2);
						// 	script.Card.Titulo.CodigoTitulo1 = new EigeInt16(arguments.Code);
						// }
						// else
						// {
							if (titleActive2 && titleIsExhausted2)
							{
								script.Card.Titulo.CodigoTitulo2 = new EigeInt16(0);
								script.Card.Titulo.ValidezHoraria2 = new GenericEnum<TituloValidezHorariaEnum>(new byte[] { 0x00 }, 4);
								script.Card.Titulo.ValidezDiaria2 = new GenericEnum<TituloValidezDiariaEnum>(new byte[] { 0x00 }, 4);
								script.Card.Titulo.TipoTarifa2 = new GenericEnum<EigeTituloTipoTarifaEnum>(new byte[] { 0x00 }, 4);
								script.Card.Titulo.ControlTarifa2 = new EigeInt8(0);
								script.Card.Titulo.EmpresaPropietaria2 = new EigeInt8(0);
								script.Card.Titulo.TipoInformacion2 = new GenericEnum<EigeTicketTipoInformacionEnum>(new byte[] { 0x00 }, 2);
								script.Card.Titulo.ValidezSextaZona2 = new EigeBool(false);
								script.Card.Titulo.AmbitoOperadores2 = new GenericEnum<EigeTituloAmbitoOperadoresEnum>(new byte[] { 0x00, 0x00, 0x00, 0x00 }, 32);
								script.Card.Titulo.TieneValidezOperador2 = new GenericEnum<EigeValidezTitulosEnum>(new byte[] { 0x00 }, 1);
								script.Card.Titulo.ValidezOperador2 = new EigeBytes(new byte[] { 0x00, 0x00 }, 10);
								script.Card.Titulo.ValidezZonal2 = new GenericEnum<EigeZonaEnum>(new byte[] { 0x00 }, 5);
								script.Card.Titulo.TipoUnidadesValidezTemporal2 = new GenericEnum<EigeTipoUnidadesValidezTemporalEnum>(new byte[] { 0x00 }, 2);
								script.Card.Titulo.NumeroUnidadesValidezTemporal2 = new EigeInt8(0);
								script.Card.Titulo.MaxTransbordosExternos2 = new EigeInt8(0);
								script.Card.Titulo.MaxPersonasEnTransbordo2 = new EigeInt8(0);
								script.Card.Titulo.MaxTiempoTransbordo2 = new EigeInt8(0);
								script.Card.Titulo.UsoDelCampoSaldoViajes2 = new EigeBool(false);
								script.Card.Titulo.PrioridadTitulo2 = new EigeInt8(0);
								// Variables
								script.Card.Titulo.TituloEnAmpliacion2 = new EigeBool(false);
								script.Card.Titulo.FechaValidez2 = new EigeDateTime(null);
								script.Card.Titulo.SaldoViaje2 = new EigeInt8(0);

								script.Card.Titulo.TitulosActivos = new GenericEnum<EigeTitulosActivosEnum>(new byte[] { (byte)(script.Card.Titulo.TitulosActivos.Value & ~EigeTitulosActivosEnum.Titulo2) }, 4);
							}

							// Fijos
							script.Card.Titulo.CodigoTitulo1 = new EigeInt16(price.Title.Code);
							script.Card.Titulo.ValidezHoraria1 = new GenericEnum<TituloValidezHorariaEnum>(new byte[] { (byte)TituloValidezHorariaEnum.SinDistincion }, 4);
							script.Card.Titulo.ValidezDiaria1 = new GenericEnum<TituloValidezDiariaEnum>(new byte[] { (byte)TituloValidezDiariaEnum.SinRestriccion }, 4);
							script.Card.Titulo.TipoTarifa1 = new GenericEnum<EigeTituloTipoTarifaEnum>(new byte[] { (byte)(await EigeService.IsYoungCardAsync(uid, script) ? 1 : 0) }, 4);
							script.Card.Titulo.ControlTarifa1 = new EigeInt8(price.Version);
							script.Card.Titulo.EmpresaPropietaria1 = new EigeInt8(price.Title.OwnerCode);
							script.Card.Titulo.TipoInformacion1 = new GenericEnum<EigeTicketTipoInformacionEnum>(new byte[] { (byte)(0) }, 1);
							script.Card.Titulo.ValidezSextaZona1 = new EigeBool(false);
							script.Card.Titulo.AmbitoOperadores1 = new GenericEnum<EigeTituloAmbitoOperadoresEnum>(new EigeInt64(price.OperatorContext ?? 0).Raw, 32);
							script.Card.Titulo.TieneValidezOperador1 = new GenericEnum<EigeValidezTitulosEnum>(new byte[] { (byte)(price.Title.ValidityBit ?? 0) }, 1);
							script.Card.Titulo.ValidezOperador1 = new EigeBytes(new EigeInt16(price.Title.TableIndex ?? 0).Raw, 10);
							script.Card.Titulo.ValidezZonal1 = new GenericEnum<EigeZonaEnum>(new byte[] { (byte)(price.Zone ?? 0) }, 5);
							script.Card.Titulo.TipoUnidadesValidezTemporal1 = new GenericEnum<EigeTipoUnidadesValidezTemporalEnum>(new byte[] { (byte)(price.Title.TemporalType ?? 0) }, 2);
							script.Card.Titulo.NumeroUnidadesValidezTemporal1 = new EigeInt8(price.Title.TemporalUnit ?? 255);
							script.Card.Titulo.MaxTransbordosExternos1 = new EigeInt8(price.Title.MaxExternalChanges ?? 0);
							script.Card.Titulo.MaxPersonasEnTransbordo1 = new EigeInt8(price.Title.MaxPeopleChanges ?? 0);
							script.Card.Titulo.MaxTiempoTransbordo1 = new EigeInt8(price.MaxTimeChanges ?? 0);
							script.Card.Titulo.UsoDelCampoSaldoViajes1 = new EigeBool(price.Title.Quantity != null && price.Title.Quantity > 0);
							script.Card.Titulo.PrioridadTitulo1 = new EigeInt8(price.Title.Priority ?? 0);
							// Variables
							script.Card.Titulo.TituloEnAmpliacion1 = new EigeBool(false);
							script.Card.Titulo.FechaValidez1 = new EigeDateTime(null);
							script.Card.Titulo.SaldoViaje1 = new EigeInt8(Convert.ToInt32((price.Title.Quantity * arguments.Quantity + promotionTravels) ?? 0));

						script.Card.Titulo.TitulosActivos = new GenericEnum<EigeTitulosActivosEnum>(new byte[] { (byte)(script.Card.Titulo.TitulosActivos.Value | EigeTitulosActivosEnum.Titulo1) }, 4);
						//}
					}
					if (config.Slot == EigeTituloEnUsoEnum.Titulo2)
					{
						titlecodetoerase1 = script.Card.Titulo.CodigoTitulo1.Value;

						if (titleActive1 && titleIsExhausted1)
						{
							script.Card.Titulo.CodigoTitulo1 = new EigeInt16(0);
							script.Card.Titulo.ValidezHoraria1 = new GenericEnum<TituloValidezHorariaEnum>(new byte[] { 0x00 }, 4);
							script.Card.Titulo.ValidezDiaria1 = new GenericEnum<TituloValidezDiariaEnum>(new byte[] { 0x00 }, 4);
							script.Card.Titulo.TipoTarifa1 = new GenericEnum<EigeTituloTipoTarifaEnum>(new byte[] { 0x00 }, 4);
							script.Card.Titulo.ControlTarifa1 = new EigeInt8(0);
							script.Card.Titulo.EmpresaPropietaria1 = new EigeInt8(0);
							script.Card.Titulo.TipoInformacion1 = new GenericEnum<EigeTicketTipoInformacionEnum>(new byte[] { 0x00 }, 2);
							script.Card.Titulo.ValidezSextaZona1 = new EigeBool(false);
							script.Card.Titulo.AmbitoOperadores1 = new GenericEnum<EigeTituloAmbitoOperadoresEnum>(new byte[] { 0x00, 0x00, 0x00, 0x00 }, 32);
							script.Card.Titulo.TieneValidezOperador1 = new GenericEnum<EigeValidezTitulosEnum>(new byte[] { 0x00 }, 1);
							script.Card.Titulo.ValidezOperador1 = new EigeBytes(new byte[] { 0x00, 0x00 }, 10);
							script.Card.Titulo.ValidezZonal1 = new GenericEnum<EigeZonaEnum>(new byte[] { 0x00 }, 5);
							script.Card.Titulo.TipoUnidadesValidezTemporal1 = new GenericEnum<EigeTipoUnidadesValidezTemporalEnum>(new byte[] { 0x00 }, 2);
							script.Card.Titulo.NumeroUnidadesValidezTemporal1 = new EigeInt8(0);
							script.Card.Titulo.MaxTransbordosExternos1 = new EigeInt8(0);
							script.Card.Titulo.MaxPersonasEnTransbordo1 = new EigeInt8(0);
							script.Card.Titulo.MaxTiempoTransbordo1 = new EigeInt8(0);
							script.Card.Titulo.UsoDelCampoSaldoViajes1 = new EigeBool(false);
							script.Card.Titulo.PrioridadTitulo1 = new EigeInt8(0);
							// Variables
							script.Card.Titulo.TituloEnAmpliacion1 = new EigeBool(false);
							script.Card.Titulo.FechaValidez1 = new EigeDateTime(null);
							script.Card.Titulo.SaldoViaje1 = new EigeInt8(0);

							script.Card.Titulo.TitulosActivos = new GenericEnum<EigeTitulosActivosEnum>(new byte[] { (byte)(script.Card.Titulo.TitulosActivos.Value & ~EigeTitulosActivosEnum.Titulo1) }, 4);
						}

						// Fijos
						script.Card.Titulo.CodigoTitulo2 = new EigeInt16(price.Title.Code);
						script.Card.Titulo.ValidezHoraria2 = new GenericEnum<TituloValidezHorariaEnum>(new byte[] { (byte)TituloValidezHorariaEnum.SinDistincion }, 4);
						script.Card.Titulo.ValidezDiaria2 = new GenericEnum<TituloValidezDiariaEnum>(new byte[] { (byte)TituloValidezDiariaEnum.SinRestriccion }, 4);
						script.Card.Titulo.TipoTarifa2 = new GenericEnum<EigeTituloTipoTarifaEnum>(new byte[] { (byte)(await EigeService.IsYoungCardAsync(uid, script) ? 1 : 0) }, 4);
						script.Card.Titulo.ControlTarifa2 = new EigeInt8(price.Version);
						script.Card.Titulo.EmpresaPropietaria2 = new EigeInt8(price.Title.OwnerCode);
						script.Card.Titulo.TipoInformacion2 = new GenericEnum<EigeTicketTipoInformacionEnum>(new byte[] { (byte)(0) }, 1);
						script.Card.Titulo.ValidezSextaZona2 = new EigeBool(false);
						script.Card.Titulo.AmbitoOperadores2 = new GenericEnum<EigeTituloAmbitoOperadoresEnum>(new EigeInt64(price.OperatorContext ?? 0).Raw, 32);
						script.Card.Titulo.TieneValidezOperador2 = new GenericEnum<EigeValidezTitulosEnum>(new byte[] { (byte)(price.Title.ValidityBit ?? 0) }, 1);
						script.Card.Titulo.ValidezOperador2 = new EigeBytes(new EigeInt16(price.Title.TableIndex ?? 0).Raw, 10);
						script.Card.Titulo.ValidezZonal2 = new GenericEnum<EigeZonaEnum>(new byte[] { (byte)(price.Zone ?? 0) }, 5);
						script.Card.Titulo.TipoUnidadesValidezTemporal2 = new GenericEnum<EigeTipoUnidadesValidezTemporalEnum>(new byte[] { (byte)(price.Title.TemporalType ?? 0) }, 2);
						script.Card.Titulo.NumeroUnidadesValidezTemporal2 = new EigeInt8(price.Title.TemporalUnit ?? 255);
						script.Card.Titulo.MaxTransbordosExternos2 = new EigeInt8(price.Title.MaxExternalChanges ?? 0);
						script.Card.Titulo.MaxPersonasEnTransbordo2 = new EigeInt8(price.Title.MaxPeopleChanges ?? 0);
						script.Card.Titulo.MaxTiempoTransbordo2 = new EigeInt8(price.MaxTimeChanges ?? 0);
						script.Card.Titulo.UsoDelCampoSaldoViajes2 = new EigeBool(price.Title.Quantity != null && price.Title.Quantity > 0);
						script.Card.Titulo.PrioridadTitulo2 = new EigeInt8(price.Title.Priority ?? 0);
						// Variables
						script.Card.Titulo.TituloEnAmpliacion2 = new EigeBool(false);
						script.Card.Titulo.FechaValidez2 = new EigeDateTime(null);
						script.Card.Titulo.SaldoViaje2 = new EigeInt8(Convert.ToInt32((price.Title.Quantity * arguments.Quantity + promotionTravels) ?? 0));

						script.Card.Titulo.TitulosActivos = new GenericEnum<EigeTitulosActivosEnum>(new byte[] { (byte)(script.Card.Titulo.TitulosActivos.Value | EigeTitulosActivosEnum.Titulo2) }, 4);
					}
				}
				//Recarga
				else
				{
					if (config.Slot == EigeTituloEnUsoEnum.Titulo1)
					{
						if ((price.Title.Quantity != null) && (price.Title.Quantity != 0))
						{
							var saldo = (
								(config.RechargeType == RechargeType.RechargeExpiredPrice) ||
								(config.RechargeType == RechargeType.Charge) ||
								(config.RechargeType == RechargeType.Replace)
							) ?
								// Caso de que se pierden los billetes antiguos
								0M :
								titleBalance1.Value;
							var newSaldo = Math.Round(saldo + (arguments.Quantity ?? 0) * (price.Title.Quantity ?? 1) + promotionTravels, 2);

							if (EigeService.IsTuiN(titleCode1))
							{
								script.Card.TituloTuiN.SaldoViaje_Value = new EigeCurrency(newSaldo * (newSaldo < 0 ? -1 : 1), 20);
								script.Card.TituloTuiN.SaldoViaje_Sign = new EigeBool(newSaldo < 0);
							}
							else
								script.Card.Titulo.SaldoViaje1 = new EigeInt8(Convert.ToInt32(newSaldo));
						}
						if ((price.Title.TemporalUnit != null) && (price.Title.TemporalUnit != 0))
						{
							var exhaustedDate = await EigeService.GetTitleExhaustedDate1Async(uid, script);

							if ((exhaustedDate != null) && (exhaustedDate.Value < arguments.Now))
								script.Card.Titulo.FechaValidez1 = new EigeDateTime(null);
							else if (!script.Card.Titulo.TituloEnAmpliacion1.Value)
							{
								script.Card.Titulo.TituloEnAmpliacion1 = new EigeBool(true);
								script.Card.Titulo.FechaValidez1 = new EigeDateTime(exhaustedDate);
								fechaValidez1 = new EigeDateTime(exhaustedDate); // Se utiliza en la notificación a Sigapunt
								script.Card.Titulo.SaldoViaje1 = new EigeInt8(0);
							}
							else
								throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargable);
						}
					}
					else if (config.Slot == EigeTituloEnUsoEnum.Titulo2)
					{
						if ((price.Title.Quantity != null) && (price.Title.Quantity != 0))
						{
							var saldo = (
								(config.RechargeType == RechargeType.RechargeExpiredPrice) ||
								(config.RechargeType == RechargeType.Charge) ||
								(config.RechargeType == RechargeType.Replace)
							) ?
								// Caso de que se pierden los billetes antiguos
								0M :
								titleBalance2.Value;
							var newSaldo = Math.Round(saldo + (arguments.Quantity ?? 0) * (price.Title.Quantity ?? 1) + promotionTravels, 2);
                            
							script.Card.Titulo.SaldoViaje2 = new EigeInt8(Convert.ToInt32(newSaldo));
						}
						if ((price.Title.TemporalUnit != null) && (price.Title.TemporalUnit != 0))
						{
							var exhaustedDate2 = await EigeService.GetTitleExhaustedDate2Async(uid, script);

							if ((exhaustedDate2 != null) && (exhaustedDate2.Value < arguments.Now))
								script.Card.Titulo.FechaValidez2 = new EigeDateTime(null);
							else if (!script.Card.Titulo.TituloEnAmpliacion2.Value)
							{
								script.Card.Titulo.TituloEnAmpliacion2 = new EigeBool(true);
								script.Card.Titulo.FechaValidez2 = new EigeDateTime(exhaustedDate2);
								fechaValidez2 = new EigeDateTime(exhaustedDate2); // Se utiliza en la notificación a Sigapunt
								script.Card.Titulo.SaldoViaje2 = new EigeInt8(0);
							}
							else
								throw new ApplicationException(TransportCardRechargeResources.ExceptionNotRechargable);
						}
					}
				}

				// Si la tarjeta no estaba activa, modificamos la fecha de caducidad
				if (script.Card.Tarjeta.Caducidad.Value == EigeDateTime.Zero.Value)
					script.Card.Tarjeta.Caducidad = new EigeDate(arguments.Now.AddYears(5));

				// TODO: Falta tener en cuenta los descuentos
				var equivalentPrice = (
						price.Price * arguments.Quantity + // Precio del titulo
						promotionTravels * price.Price / price.Title.Quantity // Precio equivalente de las promociones
					) ?? 0M;

                int? expendedor = null;
                if (ticket != null)
                {
                    int temp = 0;
                    if (int.TryParse(ticket.ExternalLogin, out temp))
                        expendedor = temp;
                }

                // Actualizar log recargas
                if (script.Card.Carga.PosicionUltima.Value == EigePosicionUltimaCargaEnum.Carga2)
				{
					// Indice
					script.Card.Carga.PosicionUltima = new GenericEnum<EigePosicionUltimaCargaEnum>(new byte[] { (byte)EigePosicionUltimaCargaEnum.Carga1 }, 1);
					// Log
					script.Card.Carga.CodigoTitulo1 = new EigeInt16(arguments.Code);
					script.Card.Carga.Empresa1 = new EigeInt8(EigeService.PayinCode);
					script.Card.Carga.Expendedor1 = new EigeInt24(expendedor ?? 0);
					script.Card.Carga.Fecha1 = new EigeDate(arguments.Now);
					script.Card.Carga.FormaPago1 = new GenericEnum<EigeFormaPagoEnum>(new byte[] { (byte)EigeFormaPagoEnum.Movil }, 4); // Móvil
					script.Card.Carga.Importe1 = new EigeCurrency(equivalentPrice, 18);
					script.Card.Carga.TipoEquipo1 = new GenericEnum<EigeTipoEquipoCargaEnum>(new byte[] { 0x03 }, 2);
					script.Card.Carga.TipoOperacion1_Opcion = new GenericEnum<EigeTipoOperacionCarga_OpcionEnum>(new byte[] { (byte) (
						config.Slot == EigeTituloEnUsoEnum.Titulo1 ? EigeTipoOperacionCarga_OpcionEnum.Titulo1 :
						config.Slot == EigeTituloEnUsoEnum.Titulo2 ? EigeTipoOperacionCarga_OpcionEnum.Titulo2 :
						0
					)}, 8);
					script.Card.Carga.TipoOperacion1_Operacion = new GenericEnum<EigeTipoOperacionCarga_OperacionEnum>(new byte[] { (byte)EigeTipoOperacionCarga_OperacionEnum.Recarga }, 8);
				}
				else
				{
					// Indice
					script.Card.Carga.PosicionUltima = new GenericEnum<EigePosicionUltimaCargaEnum>(new byte[] { (byte)EigePosicionUltimaCargaEnum.Carga2 }, 1);
					// Log
					script.Card.Carga.CodigoTitulo2 = new EigeInt16(arguments.Code);
					script.Card.Carga.Empresa2 = new EigeInt8(EigeService.PayinCode);
					script.Card.Carga.Expendedor2 = new EigeInt24(expendedor ?? 0);
                    script.Card.Carga.Fecha2 = new EigeDate(arguments.Now);
					script.Card.Carga.FormaPago2 = new GenericEnum<EigeFormaPagoEnum>(new byte[] { (byte)EigeFormaPagoEnum.Movil }, 4);
					script.Card.Carga.Importe2 = new EigeCurrency(equivalentPrice, 18);
					script.Card.Carga.TipoEquipo2 = new GenericEnum<EigeTipoEquipoCargaEnum>(new byte[] { 0x03 }, 2);
					script.Card.Carga.TipoOperacion2_Opcion = new GenericEnum<EigeTipoOperacionCarga_OpcionEnum>(new byte[] { (byte) (
						config.Slot == EigeTituloEnUsoEnum.Titulo1 ? EigeTipoOperacionCarga_OpcionEnum.Titulo1 :
						config.Slot == EigeTituloEnUsoEnum.Titulo2 ? EigeTipoOperacionCarga_OpcionEnum.Titulo2 :
						0
					)}, 8);
					script.Card.Carga.TipoOperacion2_Operacion = new GenericEnum<EigeTipoOperacionCarga_OperacionEnum>(new byte[] { (byte)EigeTipoOperacionCarga_OperacionEnum.Recarga }, 8);
				}

				// Actualizar tarifa / zona / ambito
				if (config.Slot == EigeTituloEnUsoEnum.Titulo1)
				{
					script.Card.Titulo.ControlTarifa1 = new EigeInt8(price.Version);
					if (price.Title.HasZone)
					{
						script.Card.Titulo.ValidezZonal1 = new GenericEnum<EigeZonaEnum>(new EigeInt8((int)price.Zone.Value).Raw, 5);
						script.Card.Titulo.MaxTiempoTransbordo1 = new EigeInt8(price.MaxTimeChanges ?? 0);
						script.Card.Titulo.AmbitoOperadores1 = new GenericEnum<EigeTituloAmbitoOperadoresEnum>(new EigeInt64(price.OperatorContext ?? 0).Raw, 32);
					}
				}
				else if (config.Slot == EigeTituloEnUsoEnum.Titulo2)
				{
					script.Card.Titulo.ControlTarifa2 = new EigeInt8(price.Version);
					if (price.Title.HasZone)
					{
						script.Card.Titulo.ValidezZonal2 = new GenericEnum<EigeZonaEnum>(new EigeInt8((int)price.Zone.Value).Raw, 5);
						script.Card.Titulo.MaxTiempoTransbordo2 = new EigeInt8(price.MaxTimeChanges ?? 0);
						script.Card.Titulo.AmbitoOperadores2 = new GenericEnum<EigeTituloAmbitoOperadoresEnum>(new EigeInt64(price.OperatorContext ?? 0).Raw, 32);
					}
				}

				// 4.- Comprobación límite superior de viajes
				if (
					(config.Slot == EigeTituloEnUsoEnum.Titulo1) &&
					(await EigeService.GetTitleHasBalance1Async(uid, script)) &&
					(price.Title.MaxQuantity < (await EigeService.GetTitleBalance1Async(uid, script)))
				)
					throw new ApplicationException(TransportCardRechargeResources.ExceptionQuantity.FormatString(price.Title.MaxQuantity));
				else if (
					(config.Slot == EigeTituloEnUsoEnum.Titulo2) &&
					(await EigeService.GetTitleHasBalance2Async(uid, script)) &&
					(price.Title.MaxQuantity < (await EigeService.GetTitleBalance2Async(uid, script)))
				)
					throw new ApplicationException(TransportCardRechargeResources.ExceptionQuantity.FormatString(price.Title.MaxQuantity));

				// Escribir script en orden
				await WriteAsync(script, script, uid, arguments.Code);

				var scriptSerialize = Newtonsoft.Json.JsonConvert.SerializeObject(script);
				var deviceSerialize = Newtonsoft.Json.JsonConvert.SerializeObject(arguments.Device);

				// Crear operation
				var operation = new TransportOperation
				{
					OperationDate = arguments.Now.ToUTC(),
					OperationType = OperationType.Recharge,
					Slot = config.Slot,
					RechargeType = (config != null) ? config.RechargeType : arguments.RechargeType,
					Uid = arguments.CardNumber,
					Price = price,
					Login = SessionData.Login,
					Ticket = ticket,
					Script = scriptSerialize,
					ScriptPrevious = scriptPreviousSerialize,
					Device = deviceSerialize,
					DateTimeValue =
						config.Slot == EigeTituloEnUsoEnum.Titulo1 ? fechaValidez1.Value :
						fechaValidez2.Value,
					DateTimeValueOld =
						config.Slot == EigeTituloEnUsoEnum.Titulo1 ? titleDateTime1 : titleDateTime2,
					QuantityValue =
						config.Slot == EigeTituloEnUsoEnum.Titulo1 ? await EigeService.GetTitleBalance1Async(uid, script) :
						await EigeService.GetTitleBalance2Async(uid, script), //contiene el saldo final de la carga/recarga
					QuantityValueOld =
						config.Slot == EigeTituloEnUsoEnum.Titulo1 ? titleBalance1 :
						titleBalance2,
					QuantityType =
						config.Slot == EigeTituloEnUsoEnum.Titulo1 ? await EigeService.GetTitleBalanceUnits1Async(uid, script) :
						await EigeService.GetTitleBalanceUnits2Async(uid, script),
					MobilisAmpliationBit =
						config.Slot == EigeTituloEnUsoEnum.Titulo1 ? script.Card.Titulo.TituloEnAmpliacion1.Value :
						script.Card.Titulo.TituloEnAmpliacion2.Value,
					MobilisEnvironment = script.Card.Tarjeta.CodigoEntorno.Value
				};
				await TransportOperationRepository.AddAsync(operation);
				await UnitOfWork.SaveAsync();
				arguments.OperationId = operation.Id;

				var title = (await TitleRepository.GetAsync());				
				var operationTitle = new TransportOperationTitle();

				if (!EigeService.IsTuiN(arguments.Code) || !EigeService.IsTuiN(script.Card.Titulo.CodigoTitulo1.Value)) //No es TuiN
				{
					operationTitle = new TransportOperationTitle
					{
						Caducity = now,
						Quantity = (config.Slot == EigeTituloEnUsoEnum.Titulo2) ? script.Card.Titulo.SaldoViaje2.Value: script.Card.Titulo.SaldoViaje1.Value,
						Operation = operation,
						Title = (config.Slot == EigeTituloEnUsoEnum.Titulo2) ? title.Where(x=> x.Code == script.Card.Titulo.CodigoTitulo2.Value).FirstOrDefault() : title.Where(x=> x.Code == script.Card.Titulo.CodigoTitulo1.Value).FirstOrDefault(),
						Zone = (config.Slot == EigeTituloEnUsoEnum.Titulo2) ? script.Card.Titulo.ValidezZonal2.Value : script.Card.Titulo.ValidezZonal1.Value
					};
				}
				else //Es TuiN
				{
					operationTitle = new TransportOperationTitle
					{
						Caducity = now,
						Quantity = (script.Card.TituloTuiN.SaldoViaje_Sign.Value == false) ? -1 * script.Card.TituloTuiN.SaldoViaje_Value.Value : script.Card.TituloTuiN.SaldoViaje_Value.Value,
						Operation = operation,
						Title = title.Where(x => x.Code == script.Card.Titulo.CodigoTitulo1.Value).FirstOrDefault(),
						Zone = script.Card.Titulo.ValidezZonal1.Value
					};
				}
				await TransportOperationTitleRepository.AddAsync(operationTitle);
				
				// Marcar promoción como ejecutada
				foreach (var execution in promotions)
				{
					execution.Imei = Convert.ToInt64(arguments.Imei);
					execution.TransportOperationId = operation.Id;
					execution.AppliedDate = now;
					execution.Uid = uid;

					if (execution.PromoUserId == null)
					{
						var promoUser = new PromoUser
						{
							Attemps = 1,
							LastChance = now,
							Login = SessionData.Login,
							NextChance = now.AddMinutes(1)
						};
						await PromoUserRepository.AddAsync(promoUser);
						await UnitOfWork.SaveAsync();

						execution.PromoUserId = promoUser.Id;
					}						
					else						
						execution.PromoUser.Login = SessionData.Email;
			
					await UnitOfWork.SaveAsync();
				}

				var resultScript = await script.GetResponseAndRequestAsync();
				var item = new ScriptResult
				{
					Card = new ScriptResult.CardId
					{
						Uid = uid,
						Type = arguments.CardType,
						System = arguments.CardSystem
                    },
					Script = resultScript,
					Keys = await script.GetKeysEncryptedAsync(Hsm, arguments.CardSystem, resultScript, script.Card.Uid.ToInt32() ?? 0, script.Card.TituloTuiN.VersionClaves.Value)
				};

				return new ScriptResultBase<TransportOperationGetReadInfoResult>
				{
					Scripts = new[] { item },
					Operation = new ScriptResultBase<TransportOperationGetReadInfoResult>.ScriptResultOperation
					{
						Type = operation.OperationType,
						Id = operation.Id,
                        Card = new ScriptResultBase<TransportOperationGetReadInfoResult>.ScriptResultOperationCard
                        {
                            Uid = operation.Uid.Value
                        }
					}
				};
#if TEST || DEBUG
			}
			catch (Exception ex)
			{
				throw ex; // new ApplicationException("ERROR: " + ex.Message + "\n\n" + ex.StackTrace);
			}
#endif
		}

		private decimal CanjeUpgrade(EigeTipoUnidadesValidezTemporalEnum type, EigeTipoUnidadesSaldoEnum unities, DateTime? fechaFin, decimal price, int number)
		{
			DateTime now = DateTime.Now;
			decimal result = 0;

			if (type == EigeTipoUnidadesValidezTemporalEnum.Dias)
			{
				result = (decimal)(fechaFin - now).Value.TotalDays * (price / 30);
			}
			else if (unities == EigeTipoUnidadesSaldoEnum.Viajes)
			{
				result = number * (price / 10);
			}

			return result;
		}
		#endregion ExecuteAsync

		#region WriteAsync
		public async Task WriteAsync(TransportCardGetReadInfoScript script, MifareClassicScript<EigeCard> responseScript, long uid, int code)
		{
			// Datos fijos Titulo1
			await script.WriteAsync(responseScript.Response, 3, 0, uid);
			// Datos fijos Titulo2
			await script.WriteAsync(responseScript.Response, 4, 0, uid);
			// Datos fijos Monedero
			await script.WriteAsync(responseScript.Response, 6, 0, uid);
			// Datos fijos Bonus
			await script.WriteAsync(responseScript.Response, 5, 0, uid);
			// Datos TuiN
			await script.WriteAsync(responseScript.Response, 9, 0, uid);

			// Datos variables Titulo1
			await script.WriteAsync(responseScript.Response, 1, 1, uid);
			// Datos variables Titulo2
			//await script.WriteAsync(responseScript.Response, 4, 0, uid); // Ya guardado como datos fijos titulo 2
			//Datos variables TuiN
			await script.WriteAsync(responseScript.Response, 9, 2, uid);

			// Copia de datos Titulo1
			await script.WriteAsync(responseScript.Response, 1, 2, uid);
			// Copia de datos Titulo2
			await script.WriteAsync(responseScript.Response, 4, 1, uid);
			// Copia Monedero
			await script.WriteAsync(responseScript.Response, 6, 1, uid);
			// Copia Bonus
			await script.WriteAsync(responseScript.Response, 5, 1, uid);
			// Copia de datos TuiN
			await script.WriteAsync(responseScript.Response, 9, 1, uid);

			// Datos Generales Titulos
			await script.WriteAsync(responseScript.Response, 1, 0, uid);
            //await script.WriteAsync(responseScript.Response, 1, 1, uid); // Ya guardado como datos variables titulo 1

            // Mark success
            responseScript.Response.Add(MifareSuccessOperation.Create());

            // Encabezado Carga / Recarga
            await script.WriteAsync(responseScript.Response, 2, 0, uid);
            
			// Historico Carga / Recarga
			//await script.WriteAsync(responseScript.Response, 2, 0, uid); // Ya guardado como encabezado carga / recarga
			await script.WriteAsync(responseScript.Response, 3, 1, uid);


			// VALIDO!!!
		}
		#endregion WriteAsync

		#region GetExpiryDate
		private EigeDateTime GetExpiryDate(EigeDateTime initialDate, TransportPrice price)
		{
			if (initialDate == null)
				return new EigeDateTime(null);

			var initialDateTime = initialDate.Value ?? new DateTime(0);
			var unities = price.Title.TemporalUnit;
			var temporalUnities = (int?)price.Title.TemporalType;

			if (unities == null || temporalUnities == null)
				return new EigeDateTime(null);

			switch (temporalUnities)
			{
				case 0:
					return new EigeDateTime(initialDateTime.AddYears((int)unities));
				case 1:
					return new EigeDateTime(initialDateTime.AddHours((int)unities));
				case 2:
					return new EigeDateTime(initialDateTime.AddDays((int)unities));
				case 3:
					return new EigeDateTime(initialDateTime.AddMonths((int)unities));
			}
			return new EigeDateTime(null);
		}
		#endregion GetExpiryDate
	}
}