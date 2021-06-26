using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Application.Dto.Transport.Results.TransportOperation;
using PayIn.Application.Transport.Scripts;
using PayIn.Application.Transport.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Payments.Infrastructure;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige;
using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using PayIn.Domain.Transport.MifareClassic.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Application.Results;
using Xp.Domain;
using Xp.Domain.Transport;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Public.Handlers
{
	[XpLog("TransportOperation", "Revoke", RelatedId = "OperationId")]
	[XpAnalytics("TransportOperation", "Revoke")]
	public class TransportOperationRevokeHandler :
		IQueryBaseHandler<TransportOperationRevokeArguments, TransportOperationRevokeResult>
	{
		private readonly ISessionData SessionData;
		private readonly EigeService EigeService;
		private readonly IMifareClassicHsmService Hsm;
		private readonly IEntityRepository<TransportTitle> TitleRepository;
		private readonly IUnitOfWork UnitOfWork;
		private readonly IEntityRepository<TransportOperation> TransportOperationRepository;
		private readonly IEntityRepository<Log> LogRepository;
		private readonly IEntityRepository<TransportPrice> TransportPriceRepository;	
		private readonly IEntityRepository<Ticket> TicketRepository;
		private readonly IEntityRepository<Payment> PaymentRepository;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;
		private readonly IInternalService InternalService;

		#region Constructors
		public TransportOperationRevokeHandler(
			ISessionData sessionData,
			EigeService eigeService,
			IMifareClassicHsmService hsm,
			IEntityRepository<TransportTitle> titleRepository,
			IUnitOfWork unitOfWork,
			IEntityRepository<TransportOperation> transportOperationRepository,
			IEntityRepository<Log> logRepository,
			IEntityRepository<TransportPrice> transportPriceRepository,			
			IEntityRepository<Ticket> ticketRepository,
			IEntityRepository<Payment> paymentRepository,
			ServiceNotificationCreateHandler serviceNotificationCreate,
			IInternalService internalService
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (eigeService == null) throw new ArgumentNullException("eigeService");
			if (hsm == null) throw new ArgumentNullException("hsm");
			if (titleRepository == null) throw new ArgumentNullException("titleRepository");
			if (transportOperationRepository == null) throw new ArgumentNullException("transportOperationRepository");
			if (logRepository == null) throw new ArgumentNullException("logrepository");
			if (transportPriceRepository == null) throw new ArgumentNullException("transportPriceRepository");
			if (ticketRepository == null) throw new ArgumentNullException("ticketRepository");
			if (paymentRepository == null) throw new ArgumentNullException("paymentRepository");

			SessionData = sessionData;
			EigeService = eigeService;
			Hsm = hsm;
			TitleRepository = titleRepository;
			TransportOperationRepository = transportOperationRepository;
			UnitOfWork = unitOfWork;
			LogRepository = logRepository;
			TransportPriceRepository = transportPriceRepository;
			TicketRepository = ticketRepository;
			PaymentRepository = paymentRepository;
			ServiceNotificationCreate = serviceNotificationCreate;
			InternalService = internalService;
	}
	#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TransportOperationRevokeResult>> ExecuteAsync(TransportOperationRevokeArguments arguments)
		{
			var uid = arguments.CardId.FromHexadecimal().ToInt32().Value;

			if (arguments.Script == null)
				throw new ArgumentNullException("script");
			var script = new TransportCardGetReadInfoScript(SessionData.Login, Hsm, arguments.Script);

			// Get operation
			var operationToRevoke = await EigeService.GetOperationToRevoke(uid, script, await TransportOperationRepository.GetAsync("Price.Title"), arguments.Now);
			if (operationToRevoke == null)
				throw new ApplicationException(TransportResources.RevokeException);
			if (operationToRevoke.Id != arguments.OperationId)
				throw new ApplicationException(TransportResources.RevokeException);

			var operationScript = (await TransportOperationRepository.GetAsync())
				  .Where(x => x.Id > operationToRevoke.Id
				  && x.OperationType == OperationType.Read).FirstOrDefault();

			// Get recharge log
			var lastRecharge = (await LogRepository.GetAsync("Arguments"))
				.Where(x =>
					(x.RelatedId == operationToRevoke.Id) &&
					(x.RelatedClass == "TransportOperation") &&
					(x.RelatedMethod == "Recharge")
				)
				.OrderByDescending(x => x.DateTime)
				.FirstOrDefault();
			if (lastRecharge == null)
				throw new ApplicationException(TransportResources.RevokeException);

			// Get previous script
			var previousScript = lastRecharge.Arguments
				.Where(x => x.Name == "Script")
				.Select(x => x.Value)
				.FirstOrDefault()
				.FromJson();
			if (previousScript == null)
				throw new ApplicationException(TransportResources.RevokeException);

			var slotToReturn =
				(await EigeService.GetTitleActive1Async(uid, script)) && ((await EigeService.GetTitleCode1Async(uid, script)) == lastRecharge.Arguments.Where(x => x.Name == "Code").Select(x => Convert.ToInt32(x.Value)).FirstOrDefault()) ? EigeTituloEnUsoEnum.Titulo1 :
				(await EigeService.GetTitleActive2Async(uid, script)) && ((await EigeService.GetTitleCode2Async(uid, script)) == lastRecharge.Arguments.Where(x => x.Name == "Code").Select(x => Convert.ToInt32(x.Value)).FirstOrDefault()) ? EigeTituloEnUsoEnum.Titulo2 :
				(EigeTituloEnUsoEnum?)null;
			var code =
				(slotToReturn == EigeTituloEnUsoEnum.Titulo1) ? script.Card.Titulo.CodigoTitulo1.Value :
				(slotToReturn == EigeTituloEnUsoEnum.Titulo2) ? script.Card.Titulo.CodigoTitulo2.Value :
				(int?)null;
			var zone =
				(slotToReturn == EigeTituloEnUsoEnum.Titulo1) ? script.Card.Titulo.ValidezZonal1.Value :
				(slotToReturn == EigeTituloEnUsoEnum.Titulo2) ? script.Card.Titulo.ValidezZonal2.Value :
				(EigeZonaEnum?)null;
			var tarifa =
				(slotToReturn == EigeTituloEnUsoEnum.Titulo1) ? script.Card.Titulo.ControlTarifa1.Value :
				(slotToReturn == EigeTituloEnUsoEnum.Titulo2) ? script.Card.Titulo.ControlTarifa2.Value :
				(int?)null;

			// Anular el título
			if (previousScript.Count > 0)
			{
				if (slotToReturn == EigeTituloEnUsoEnum.Titulo2)
				{
					foreach (var i in previousScript)
					{
						if (i.operation == MifareOperationType.ReadBlock && i.sector == 4 && i.block == 0)
						{
							string oldValue = i.data.ToObject<string>();
							script.Card.Sectors[4].Blocks[0].Set(oldValue.FromHexadecimal());
							break;
						}
					}
				}
				else if (slotToReturn == EigeTituloEnUsoEnum.Titulo1)
				{
					foreach (var i in previousScript)
					{
						if (i.operation == MifareOperationType.ReadBlock && i.sector == 1 && i.block == 1)
						{
							string oldValue = i.data.ToObject<string>();
							script.Card.Sectors[1].Blocks[1].Set(oldValue.FromHexadecimal());
							break;
						}
					}
				}
			}
			else
			{
				if (slotToReturn == EigeTituloEnUsoEnum.Titulo2)
				{
					script.Card.Titulo.CodigoTitulo2 = new EigeInt16(0);
					script.Card.Titulo.SaldoViaje2 = new EigeInt8(0);
					script.Card.Titulo.TitulosActivos = new GenericEnum<EigeTitulosActivosEnum>(new byte[] { (byte)EigeTitulosActivosEnum.Titulo1 }, 4);
					script.Card.Titulo.TituloEnAmpliacion2 = new EigeBool(false);
					script.Card.Titulo.FechaValidez2 = new EigeDateTime(Convert.ToDateTime("01/01/2000 00:00"));
				}
				else if (slotToReturn == EigeTituloEnUsoEnum.Titulo1)
				{
					script.Card.Titulo.CodigoTitulo1 = new EigeInt16(0);
					script.Card.Titulo.SaldoViaje1 = new EigeInt8(0);
					script.Card.Titulo.TitulosActivos = new GenericEnum<EigeTitulosActivosEnum>(new byte[] { (byte)EigeTitulosActivosEnum.Titulo2 }, 4);
					script.Card.Titulo.TituloEnAmpliacion1 = new EigeBool(false);
					script.Card.Titulo.FechaValidez1 = new EigeDateTime(Convert.ToDateTime("01/01/2000 00:00"));
				}
			}

			//6.- Modificar el histórico de Carga/Recarga
			var lastCharge = script.Card.Carga.PosicionUltima;
			if (lastCharge.Value == EigePosicionUltimaCargaEnum.Carga2)
			{
				// Indice
				script.Card.Carga.PosicionUltima = new GenericEnum<EigePosicionUltimaCargaEnum>(new byte[] { (byte)EigePosicionUltimaCargaEnum.Carga1 }, 1);

				// Log
				script.Card.Carga.CodigoTitulo1 = new EigeInt16((int)code);
				script.Card.Carga.Empresa1 = new EigeInt8(EigeService.PayinCode);
				script.Card.Carga.Expendedor1 = new EigeInt24(
					arguments.Imei.IsNullOrEmpty() ? 0 :
					arguments.Imei.Length <= 7 ? Convert.ToInt32(arguments.Imei) :
					Convert.ToInt32(arguments.Imei.Substring(arguments.Imei.Length - 7, 7)));
				script.Card.Carga.Fecha1 = new EigeDate(arguments.Now);
				script.Card.Carga.FormaPago1 = new GenericEnum<EigeFormaPagoEnum>(new byte[] { (byte)EigeFormaPagoEnum.Movil }, 4); // Móvil
				script.Card.Carga.Importe1 = new EigeCurrency(operationToRevoke.Price.Price, 18);
				script.Card.Carga.TipoEquipo1 = new GenericEnum<EigeTipoEquipoCargaEnum>(new byte[] { 0x00 }, 2); // Falta Móvil
				script.Card.Carga.TipoOperacion1_Opcion = new GenericEnum<EigeTipoOperacionCarga_OpcionEnum>(new byte[] { (byte) (
						slotToReturn == EigeTituloEnUsoEnum.Titulo1 ? EigeTipoOperacionCarga_OpcionEnum.Titulo1 :
						slotToReturn == EigeTituloEnUsoEnum.Titulo2 ? EigeTipoOperacionCarga_OpcionEnum.Titulo2 :
						0
					)}, 8);
				script.Card.Carga.TipoOperacion1_Operacion = new GenericEnum<EigeTipoOperacionCarga_OperacionEnum>(new byte[] { (byte)EigeTipoOperacionCarga_OperacionEnum.Anulacion }, 8);
			}
			else
			{
				script.Card.Carga.PosicionUltima = new GenericEnum<EigePosicionUltimaCargaEnum>(new byte[] { (byte)EigePosicionUltimaCargaEnum.Carga2 }, 1);

				// Log
				script.Card.Carga.CodigoTitulo2 = new EigeInt16((int)code);
				script.Card.Carga.Empresa2 = new EigeInt8(EigeService.PayinCode);
				script.Card.Carga.Expendedor2 = new EigeInt24(
					arguments.Imei.IsNullOrEmpty() ? 0 :
					arguments.Imei.Length <= 7 ? Convert.ToInt32(arguments.Imei) :
					Convert.ToInt32(arguments.Imei.Substring(arguments.Imei.Length - 7, 7)));
				script.Card.Carga.Fecha2 = new EigeDate(arguments.Now);
				script.Card.Carga.FormaPago2 = new GenericEnum<EigeFormaPagoEnum>(new byte[] { (byte)EigeFormaPagoEnum.Movil }, 4);
				script.Card.Carga.Importe2 = new EigeCurrency(operationToRevoke.Price.Price, 18);
				script.Card.Carga.TipoEquipo2 = new GenericEnum<EigeTipoEquipoCargaEnum>(new byte[] { 0x00 }, 2); // Falta Móvil
				script.Card.Carga.TipoOperacion2_Opcion = new GenericEnum<EigeTipoOperacionCarga_OpcionEnum>(new byte[] { (byte) (
						slotToReturn == EigeTituloEnUsoEnum.Titulo1 ? EigeTipoOperacionCarga_OpcionEnum.Titulo1 :
						slotToReturn == EigeTituloEnUsoEnum.Titulo2 ? EigeTipoOperacionCarga_OpcionEnum.Titulo2 :
						0
					)}, 8);
				script.Card.Carga.TipoOperacion2_Operacion = new GenericEnum<EigeTipoOperacionCarga_OperacionEnum>(new byte[] { (byte)EigeTipoOperacionCarga_OperacionEnum.Anulacion }, 8);
			}

			// Copia datos
			script.Card.Sectors[1].Blocks[2].Set(script.Card.Sectors[1].Blocks[1].Value);
			script.Card.Sectors[4].Blocks[1].Set(script.Card.Sectors[4].Blocks[0].Value);

			await WriteAsync(script, script, uid, code.Value);

			//Creando operación
			var price = (await TransportPriceRepository.GetAsync())
				.Where(x => 
					x.Zone == zone &&
					x.Version == tarifa &&
					x.State == TransportPriceState.Active &&
					x.Title.Code == code &&
					x.Title.State == TransportTitleState.Active
				)
				.FirstOrDefault();

			var scriptSerialize = Newtonsoft.Json.JsonConvert.SerializeObject(script);
			var deviceSerialize = Newtonsoft.Json.JsonConvert.SerializeObject(arguments.Device);

			var operation = new TransportOperation
			{
				OperationDate = arguments.Now.ToUTC(),
				OperationType = OperationType.Revoke,
				Uid = arguments.CardNumber,
                Ticket = operationToRevoke?.Ticket,
                Price = price,
				Login = SessionData.Login,
				Script = scriptSerialize,
				Device = deviceSerialize
			};
			await TransportOperationRepository.AddAsync(operation);
			await UnitOfWork.SaveAsync();
			arguments.OperationId = operation.Id;

			var resultScript = await script.GetResponseAndRequestAsync();
			var item = new ScriptResult
			{
				Card = new ScriptResult.CardId
				{
					Uid = uid,
					Type = arguments.CardType,
					System = CardSystem.Mobilis
				},
				Script = resultScript,
				Keys = await script.GetKeysEncryptedAsync(Hsm, arguments.CardSystem, resultScript, script.Card.Uid.ToInt64() ?? 0, script.Card.TituloTuiN.VersionClaves.Value),
			};

			// TODO: Arreglar el tema de pagos en la devolucion, ahora se virtualiza

			//Crear la devolución del ticket
			//var ticket = (await TicketRepository.GetAsync())
			//	.Where(x => x.Id == lastOperation.TicketId)
			//	.FirstOrDefault();

			//var getPayment = (await PaymentRepository.GetAsync("Ticket.Concession.Concession.Supplier"))
			//	.Where(x =>
			//		x.Ticket.Id == ticket.Id &&
			//		x.State == PaymentState.Active &&
			//		!x.RefundTo.Any(y => y.State == PaymentState.Active)
			//	)
			//	.FirstOrDefault();


			//var refund = new Payment
			//{
			//	AuthorizationCode = "",
			//  MerchantCode = "",
			//	Amount = -getPayment.Amount,
			//	Date = now.ToUTC(),
			//	Order = getPayment.Order,
			//	Payin = 0,
			//	PaymentMediaId = getPayment.PaymentMediaId,
			//	ErrorCode = "",
			//	ErrorText = "",
			//	ErrorPublic = "",
			//	State = PaymentState.Pending,
			//	TaxAddress = getPayment.TaxAddress,
			//	TaxName = getPayment.TaxName,
			//	TaxNumber = getPayment.TaxNumber,
			//	TicketId = getPayment.TicketId,
			//	UserLogin = getPayment.UserLogin,
			//	UserName = getPayment.UserName,
			//	RefundFrom = getPayment
			//};
			//await PaymentRepository.AddAsync(refund);
			//await UnitOfWork.SaveAsync();

			// Ejecutar devolución
			//var refundResponse = await InternalService.PaymentMediaRefundAsync(
			//	pin: arguments.Pin ?? "1234",
			//	publicPaymentMediaId: refund.PaymentMediaId,
			//	publicTicketId: refund.TicketId,
			//	publicPaymentId: refund.Id,
			//	order: getPayment.Order.Value,
			//	amount: getPayment.Amount
			//);

			// Actualizar datos devolución
			//refund.AuthorizationCode = refundResponse.AuthorizationCode;
			//refund.MarchantCode
			//refund.Order = refundResponse.OrderId;
			//if (refundResponse.IsError)
			//{
			//	refund.ErrorCode = refundResponse.ErrorCode;
			//	refund.ErrorText = refundResponse.ErrorText;

			//	refund.State = PaymentState.Error;
			//	await UnitOfWork.SaveAsync();

			//	throw new ApplicationException(ServiceNotificationResources.GatewayError.FormatString(
			//		refundResponse.ErrorCode,
			//		refundResponse.ErrorText
			//	));
			//}
			//else
			//{
			//	refund.State = PaymentState.Active;

			//	await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
			//		type: NotificationType.RefundSucceed,
			//		message: PaymentResources.RefundedPushMessage.FormatString(getPayment.Amount, getPayment.Ticket.Concession.Concession.Supplier.Name),
			//		referenceId: getPayment.TicketId,
			//		referenceClass: "Ticket",
			//		senderLogin: getPayment.Ticket.Concession.Concession.Supplier.Login,
			//		receiverLogin: getPayment.UserLogin
			//	));
			//}

			return new ScriptResultBase<TransportOperationRevokeResult>
			{
				Scripts = new[] { item },
				Operation = new ScriptResultBase<TransportOperationRevokeResult>.ScriptResultOperation
				{
					Type = operation.OperationType,
					Uid = operation.Uid,
					Id = operation.Id
				}
			};
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


			// Datos variables Titulo1
			await script.WriteAsync(responseScript.Response, 1, 1, uid);
			// Datos variables Titulo2
			//await script.WriteAsync(responseScript.Response, 4, 0, uid); // Ya guardado como datos fijos titulo 2


			// Copia de datos Titulo1
			await script.WriteAsync(responseScript.Response, 1, 2, uid);
			// Copia de datos Titulo2
			await script.WriteAsync(responseScript.Response, 4, 1, uid);
			// Copia Monedero
			await script.WriteAsync(responseScript.Response, 6, 1, uid);
			// Copia Bonus
			await script.WriteAsync(responseScript.Response, 5, 1, uid);

			// Cobro y pedir pase tarjeta
			// Hecho antes

			// Datos Generales Titulos
			await script.WriteAsync(responseScript.Response, 1, 0, uid);
			//await script.WriteAsync(responseScript.Response, 1, 1, uid); // Ya guardado como datos variables titulo 1


			// CORRECTO!!!


			// Encabezado Carga / Recarga
			await script.WriteAsync(responseScript.Response, 2, 0, uid);


			// Historico Carga / Recarga
			//await script.WriteAsync(responseScript.Response, 2, 0, uid); // Ya guardado como encabezado carga / recarga
			await script.WriteAsync(responseScript.Response, 3, 1, uid);


			// VALIDO!!!
		}
		#endregion WriteAsync
	}
}