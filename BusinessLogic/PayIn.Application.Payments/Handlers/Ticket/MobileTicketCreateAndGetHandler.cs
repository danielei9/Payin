using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Application.Public.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Payments.Infrastructure;
using PayIn.Domain.Promotions;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Ticket", "SellCreateAndGet")]
	[XpAnalytics("Ticket", "SellCreateAndGet")]
	public class MobileTicketCreateAndGetHandler : IServiceBaseHandler<MobileTicketCreateAndGetArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<TransportConcession> TransportConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<TransportPrice> TransportPriceRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentWorker> PaymentWorkerRepository { get; set; }
		[Dependency] public IEntityRepository<Ticket> Repository { get; set; }
		[Dependency] public IEntityRepository<PaymentMedia> PaymentMediaRepository { get; set; }
		[Dependency] public IEntityRepository<PromoExecution> ExecutionRepository { get; set; }
		[Dependency] public IEntityRepository<Entrance> EntranceRepository { get; set; }
		[Dependency] public IEntityRepository<EntranceType> EntranceTypeRepository { get; set; }
		[Dependency] public IEntityRepository<Event> EventRepository { get; set; }
		[Dependency] public IEntityRepository<Product> ProductRepository { get; set; }
		[Dependency] public IEntityRepository<ControlFormOption> ControlFormOptionRepository { get; set; }
		[Dependency] public IEntityRepository<ControlFormValue> ControlFormValueRepository { get; set; }
		[Dependency] public IEntityRepository<CampaignLine> CampaignLineRepository { get; set; }
		[Dependency] public IEntityRepository<GreyList> GreyListRepository { get; set; }
		[Dependency] public IEntityRepository<Purse> PurseRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }
		[Dependency] public IInternalService InternalService { get; set; }
		[Dependency] public ApiEntranceCreateHandler ApiEntranceCreateHandler { get; set; }
		[Dependency] public MobileMainSynchronizeHandler MobileMainSynchronizeHandler { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobileTicketCreateAndGetArguments arguments)
		{
			var now = DateTime.UtcNow;

			var ticket = await CreateTicketAsync(arguments.Reference, arguments.Uid, arguments.Date, arguments.ConcessionId, arguments.EventId, arguments.Lines, arguments.Payments, arguments.Forms, arguments.LiquidationConcession, arguments.TransportPrice, arguments.Type, SessionData.Email, SessionData.Login, "", arguments.Amount, now, true, false);
			var result = await GetTicketAsync(ticket);

			// Devolver ticket si es gratuito
			if (ticket.Amount == 0)
				return new MobileTicketCreateAndGetResultBase
				{
					//HasPayment = false,
					Data = new[] { result },
					PaymentMedias = null,
					Promotions = null,
				};

			// Devolver ticket al movil
			//var hasPayment = await InternalService.UserHasPaymentAsync();
			var price = arguments.TransportPrice == null ?
				null :
				(await TransportPriceRepository.GetAsync("Title"))
				.Where(x => x.Id == arguments.TransportPrice)
				.FirstOrDefault();

			var promotions = (await ExecutionRepository.GetAsync())
				.Where(x =>
					x.PromoUser.Login == SessionData.Login &&
					x.AppliedDate == null &&
					x.Promotion.PromoLaunchers
						.Where(y => y.Type == PromoLauncherType.Recharge)
						.Any() &&
					x.Promotion.PromoPrices
						.Where(y => y.TransportPriceId == arguments.TransportPrice)
						.Any()
				)
				.Select(z => new
				{
					z.Id,
					z.Promotion.PromoActions.FirstOrDefault().Quantity,
					z.Promotion.PromoActions.FirstOrDefault().Type
				})
				.ToList()
				.Select(z => new MobileTicketCreateAndGetResultBase_Promotion
				{
					Id = z.Id,
					Quantity = z.Quantity,
					Type = z.Type
				});

			var paymentMedias = (await PaymentMediaRepository.GetAsync())
				.Where(x => x.State == PaymentMediaState.Active && x.Login == SessionData.Login)
				.Select(x => new MobilePaymentMediaGetAllResult
				{
					Id = x.Id,
					Title = x.Name,
					Subtitle = x.Type.ToString(),
					VisualOrder = x.VisualOrder,
					NumberHash = x.NumberHash,
					ExpirationMonth = x.ExpirationMonth,
					ExpirationYear = x.ExpirationYear,
					Type = x.Type,
					State = x.State,
					BankEntity = x.BankEntity
				})
				.ToList();

			foreach (var pMedia in paymentMedias)
				if (pMedia.Type == PaymentMediaType.Purse)
				{
					var res = await InternalService.PaymentMediaGetBalanceAsync(pMedia.Id);
					if (res != null)
						pMedia.Balance = res.Balance;
				}

			return new MobileTicketCreateAndGetResultBase
			{
				//HasPayment = hasPayment,
				Data = new[] { result },
				PaymentMedias = paymentMedias,
				Promotions = promotions,
			};
		}
		#endregion ExecuteAsync

		#region CreateTicketAsync
		public async Task<Ticket> CreateTicketAsync(string reference, long? uid, DateTime date, int concessionId, int? eventId, IEnumerable<MobileTicketCreateAndGetArguments_TicketLine> argumentLines, IEnumerable<MobileTicketCreateAndGetArguments_Payment> argumentPayments, IEnumerable<TicketAnswerFormsArguments_Form> forms, int? liquidationConcession, int? transportPrice, TicketType? type, string email, string login, string externalLogin, decimal? amount, DateTime now, bool throwExceptions, bool campaignsAlreadyApplied) => await CreateTicketAsync(reference, uid, date, concessionId, eventId, argumentLines, argumentPayments, forms, liquidationConcession, transportPrice, type, email, login, externalLogin, amount, now, throwExceptions, campaignsAlreadyApplied, true);

		public async Task<Ticket> CreateTicketAsync(string reference, long? uid, DateTime date, int concessionId, int? eventId, IEnumerable<MobileTicketCreateAndGetArguments_TicketLine> argumentLines, IEnumerable<MobileTicketCreateAndGetArguments_Payment> argumentPayments, IEnumerable<TicketAnswerFormsArguments_Form> forms, int? liquidationConcession, int? transportPrice, TicketType? type, string email, string login, string externalLogin, decimal? amount, DateTime now, bool throwExceptions, bool campaignsAlreadyApplied, bool payTicket)
		{
			var concession = (await PaymentConcessionRepository.GetAsync("Concession.Supplier"))
				.Where(x => x.Id == concessionId)
				.FirstOrDefault();
			if (concession == null)
				throw new ArgumentNullException("concessionId");
			if (concession.Concession.State != ConcessionState.Active)
				throw new ArgumentException(TicketResources.CreateNonActiveConcessionException, "concessionId");

			if (eventId != null)
			{
				var event_ = (await EventRepository.GetAsync())
					.Where(x => x.Id == eventId)
					.Select(x => new
					{
						Amount = x.Tickets.Sum(y => (decimal?)y.Amount) ?? 0,
						x.MaxAmountToSpend
					})
					.FirstOrDefault();

				if ((event_ != null) && (event_.MaxAmountToSpend != null) && (event_.Amount + amount > event_.MaxAmountToSpend))
					throw new ApplicationException(TicketResources.ExcedMaxAmountToSpendException);
			}

			var worker = (await PaymentWorkerRepository.GetAsync())
				.Where(x =>
					x.ConcessionId == concessionId &&
					x.Login == SessionData.Login &&
					x.State == WorkerState.Active
				)
				.FirstOrDefault();

			var dateSince = now;
			var dateUntil = now.AddHours(6);

			var lines = await GenerateExtraLinesAsync(argumentLines, uid, login, concessionId, eventId, transportPrice, now, throwExceptions, campaignsAlreadyApplied);

			var ticket = new Ticket
			{
				Date = date.ToUTC(),
				ExternalLogin = externalLogin,
				Amount = amount ?? lines.Sum(x => x.Amount * x.Quantity),
				Reference = reference,
				Concession = concession,
				PaymentWorker = worker ?? null,
				SupplierName = concession.Concession.Supplier.Name,
				TaxName = concession.Concession.Supplier.TaxName,
				TaxNumber = concession.Concession.Supplier.TaxNumber,
				TaxAddress = concession.Concession.Supplier.TaxAddress,
				State =
					type == TicketType.Order ? TicketStateType.Created :
					type == TicketType.Shipment ? TicketStateType.Created :
												  TicketStateType.Active,
				Since = dateSince,
				Until = dateUntil,
				TextUrl = "",
				PdfUrl = "",
				LiquidationConcessionId = concession.LiquidationConcessionId ?? liquidationConcession,
				Type = type.Value,
				EventId = eventId
			};

			// 20190211 XAVI: Antes se comprobaba que el argumentPayments != null
			// Lo he quitado pq si el ticket es 0 debe darse por pagado aunque no haya pagos
			bool payed = (ticket.Amount <= (argumentPayments?.Sum(x => (decimal?)x.Amount) ?? 0));

			if (argumentPayments != null)
				foreach (var item in argumentPayments)
				{
					var payment = new Payment
					{
						Amount = item.Amount ?? ticket.Amount,
						AuthorizationCode = "",
						CommerceCode = "",
						Date = item.Date,
						ExternalLogin = externalLogin,
						State = PaymentState.Active,
						ErrorCode = "",
						ErrorPublic = "",
						ErrorText = "",
						TaxAddress = "",
						TaxName = "",
						TaxNumber = "",
						UserLogin = "",
						UserName = "",
						Uid = item.Uid,
						UidFormat = null,
						Seq = null
					};
					ticket.Payments.Add(payment);
				}

			foreach (var ticketLine in lines)
			{
				ticket.Lines.Add(ticketLine);

				var entranceTypeIdsOfEvent = (await EntranceTypeRepository.GetAsync())
					.Where(x =>
						(x.State == EntranceTypeState.Active) &&
						(x.Event.EntranceTypes
							.Any(y => y.Id == ticketLine.EntranceTypeId)
						)
					)
					.Select(x => x.Id)
					.ToList();
				var entrancesToBuyOfEvent = argumentLines
					.Where(x =>
						(entranceTypeIdsOfEvent.Contains(x.EntranceTypeId ?? 0))
					)
					.Sum(x => x.Quantity);

				await BuyEntranceAsync(ticket, payed, ticketLine, entrancesToBuyOfEvent, forms, uid, now, email, login, payTicket);
				if (uid != null)
				{
					await ApplyRechargeAsync(ticket, payed, ticketLine, uid.Value, now, throwExceptions);

					await ReturnEntrance(ticket, payed, ticketLine, uid.Value, now, throwExceptions);

					if ((!payTicket) && (!payed))
					{
						ticket.State = TicketStateType.Pending;
					}
				}
			}

			await MobileMainSynchronizeHandler.CreateAccountLinesAsync(ticket, concessionId, new List<string>());

			await Repository.AddAsync(ticket);
			await UnitOfWork.SaveAsync();

			return ticket;
		}
		#endregion CreateTicketAsync

		#region GiveEntrancesAsync
		public async Task<Ticket> GiveEntrancesAsync(string reference, long? uid, DateTime date, int concessionId, int? eventId, IEnumerable<MobileTicketCreateAndGetArguments_TicketLine> argumentLines, IEnumerable<MobileTicketCreateAndGetArguments_Payment> argumentPayments, IEnumerable<TicketAnswerFormsArguments_Form> forms, int? liquidationConcession, int? transportPrice, TicketType? type, string email, string login, string externalLogin, decimal? amount, DateTime now, bool throwExceptions, bool campaignsAlreadyApplied)
		{
			var concession = (await PaymentConcessionRepository.GetAsync("Concession.Supplier"))
				.Where(x => x.Id == concessionId)
				.FirstOrDefault();
			if (concession == null)
				throw new ArgumentNullException("concessionId");
			if (concession.Concession.State != ConcessionState.Active)
				throw new ArgumentException(TicketResources.CreateNonActiveConcessionException, "concessionId");

			bool isRecharge = false;
			if (argumentLines != null && argumentLines.Count() == 1 && argumentLines.First().Type == TicketLineType.Recharge)
				isRecharge = true;

			// Comprobar limitación de número de entradas por pulsera, solo si no es una recarga de saldo
			if (uid != null && !isRecharge)
			{
				var entranceTypeBoughtIds = argumentLines
					.Where(x =>
						(x.Type == TicketLineType.Entrance) &&
						(x.EntranceTypeId != null)
					)
					.Select(x => x.EntranceTypeId);

				var eventsToCheckExced = (await EntranceTypeRepository.GetAsync())
					.Where(x => entranceTypeBoughtIds.Contains(x.Id))
					.Select(x => new
					{
						x.Id,
						x.EventId,
						EventName = x.Event.Name,
						x.Event.MaxEntrancesPerCard,
						EntranceCount = x.Entrances
							.Where(y =>
								(y.State == EntranceState.Active) &&
								(y.Uid == uid)
							)
							.Count()
					})
					.GroupBy(x => new { x.EventId, x.EventName, x.MaxEntrancesPerCard })
					.Select(x => new {
						x.Key.EventId,
						x.Key.EventName,
						x.Key.MaxEntrancesPerCard,
						EntranceCount = x
							.Sum(y => y.EntranceCount),
						EntranceTypes = x
							.Select(y => y.Id)
					})
					.ToList();

				foreach (var eventToExced in eventsToCheckExced)
				{
					var boughtEntrancesInCard = argumentLines
							.Where(y =>
								(y.Type == TicketLineType.Entrance) &&
								(y.EntranceTypeId != null) &&
								(eventToExced.EntranceTypes
									.Any(z => z == y.EntranceTypeId))
							)
							.Sum(y => y.Quantity);

					if (eventToExced.MaxEntrancesPerCard < eventToExced.EntranceCount + boughtEntrancesInCard)
						throw new ArgumentException($"El número máximo de entradas por tarjeta para evento {eventToExced.EventName} es de {eventToExced.MaxEntrancesPerCard} y se han intentado comprar {boughtEntrancesInCard} más a las {eventToExced.EntranceCount} ya compradas.");
				}
			}

			var worker = (await PaymentWorkerRepository.GetAsync())
				.Where(x =>
					x.ConcessionId == concessionId &&
					x.Login == SessionData.Login &&
					x.State == WorkerState.Active
				)
				.FirstOrDefault();

			var dateSince = now;
			var dateUntil = now.AddHours(6);

			var lines = await GenerateExtraLinesAsync(argumentLines, uid, login, concessionId, eventId, transportPrice, now, throwExceptions, campaignsAlreadyApplied);

			var ticket = new Ticket
			{
				Date = date.ToUTC(),
				ExternalLogin = externalLogin,
				Amount = amount ?? lines.Sum(x => x.Amount * x.Quantity),
				Reference = reference,
				Concession = concession,
				PaymentWorker = worker ?? null,
				SupplierName = concession.Concession.Supplier.Name,
				TaxName = concession.Concession.Supplier.TaxName,
				TaxNumber = concession.Concession.Supplier.TaxNumber,
				TaxAddress = concession.Concession.Supplier.TaxAddress,
				State =
					type == TicketType.Order ? TicketStateType.Created :
					type == TicketType.Shipment ? TicketStateType.Created :
												  TicketStateType.Active, // TicketType.Ticket
				Since = dateSince,
				Until = dateUntil,
				TextUrl = "",
				PdfUrl = "",
				LiquidationConcessionId = liquidationConcession,
				Type = type.Value,
				EventId = eventId
			};
			bool payed =
				(ticket.Amount <= (argumentPayments.Sum(x => (decimal?)x.Amount) ?? 0)) ||
				(argumentPayments.Any(x => x.Amount == null));

			if (argumentPayments != null)
				foreach (var item in argumentPayments)
				{
					var payment = new Payment
					{
						Amount = item.Amount ?? ticket.Amount,
						AuthorizationCode = "",
						CommerceCode = "",
						Date = item.Date,
						ExternalLogin = externalLogin,
						State = PaymentState.Active,
						ErrorCode = "",
						ErrorPublic = "",
						ErrorText = "",
						TaxAddress = "",
						TaxName = "",
						TaxNumber = "",
						UserLogin = "",
						UserName = "",
						Uid = item.Uid,
						UidFormat = null,
						Seq = null
					};
					ticket.Payments.Add(payment);
				}

			foreach (var ticketLine in lines)
			{
				ticket.Lines.Add(ticketLine);

				var entranceTypeIdsOfEvent = (await EntranceTypeRepository.GetAsync())
					.Where(x =>
						(x.State == EntranceTypeState.Active) &&
						(x.Event.EntranceTypes
							.Any(y => y.Id == ticketLine.EntranceTypeId)
						)
					)
					.Select(x => x.Id)
					.ToList();
				var entrancesToBuyOfEvent = argumentLines
					.Where(x =>
						(entranceTypeIdsOfEvent.Contains(x.EntranceTypeId ?? 0))
					)
					.Sum(x => x.Quantity);

				await BuyEntranceAsync(ticket, payed, ticketLine, entrancesToBuyOfEvent, forms, uid, now, email, login);
				if (uid != null)
					await ApplyRechargeAsync(ticket, payed, ticketLine, uid.Value, now, throwExceptions);
			}

			await Repository.AddAsync(ticket);
			await UnitOfWork.SaveAsync();

			return ticket;
		}
		#endregion GiveEntrancesAsync

		#region GetTicketAsync
		public async Task<MobileTicketCreateAndGetResult> GetTicketAsync(Ticket ticket)
		{
			return await Task.Run(() =>
			{
				var result = new MobileTicketCreateAndGetResult
				{
					Id = ticket.Id,
					Reference = ticket.Reference,
					Amount = ticket.Amount,
					PayedAmount = 0,
					Date = ticket.Date.ToUTC(),
					State = ticket.State,
					CanReturn = (
						(ticket.Concession.Concession.Supplier.Login == SessionData.Login) ||
						(ticket.PaymentWorker?.Login == SessionData.Login)
					),
					SupplierName = ticket.SupplierName,
					SupplierTaxName = ticket.TaxName,
					SupplierAddress = ticket.TaxAddress,
					SupplierNumber = ticket.TaxNumber,
					SupplierPhone = ticket.Concession.Phone,
					WorkerName = (ticket.PaymentWorker != null) ?
						ticket.PaymentWorker.Name :
						"",
					Type = ticket.Type,
					Payments = new List<MobileTicketCreateAndGetResult_Payment>(),
					Lines = ticket.Lines
						.Select(y => new MobileTicketCreateAndGetResult_TicketLine
						{
							Id = y.Id,
							Title = y.Title,
							Amount = y.Amount,
							Quantity = y.Quantity,
							Entrances = y.Entrances
								.Where(z => y.Type == TicketLineType.Entrance)
								.Select(z => new
								{
									z.Id,
									CodeTemplate = z.EntranceType.Event.EntranceSystem.Template,
									CodeTemplateText = z.EntranceType.Event.EntranceSystem.TemplateText,
									CodeType = z.EntranceType.Event.EntranceSystem.Type,
									z.EntranceType.EventId,
									EventCode = z.EntranceType.Event.Code,
									EntranceTypeId = z.EntranceType.Id,
									EntranceTypeCode = "",
									EntranceCode = z.Code
								})
								.Select(z => new MobileTicketCreateAndGetResult_Entrance
								{
									Id = z.Id,
									EntranceTypeId = z.EntranceTypeId,
									EventId = z.EventId,
									Code = z.CodeTemplate.FormatString(z.EventCode ?? 0, z.EntranceTypeCode, z.EntranceCode),
									CodeText = z.CodeTemplateText.FormatString(z.EventCode ?? 0, z.EntranceTypeCode, z.EntranceCode),
									CodeType = z.CodeType
								})
						})
				};

				return result;
			});
		}
		#endregion GetTicketAsync

		#region GetActiveCampaignLinesAsync
		public async Task<IEnumerable<KeyValuePair<CampaignLine, IEnumerable<CampaignCode>>>> GetActiveCampaignLinesAsync(string login, int paymentConcessionId, int? eventId, DateTime now, IEnumerable<long> codes)
		{
			var result = (await CampaignLineRepository.GetAsync(
					"Campaign.TargetEvents",
					"Products",
					"EntranceTypes"
				))
				.Where(x =>
					(x.State == CampaignLineState.Active) &&
					(x.Campaign.Since <= now) &&
					(x.Campaign.Until >= now) &&
					//x.Campaign.NumberOfTimes
					x.Campaign.State == CampaignState.Active &&
					//x.Campaign.TargetConcession &&
					(
						(x.Campaign.TargetSystemCardId == null) ||
						(x.Campaign.TargetSystemCard.Cards.Any(y => y.Users.Any(z => z.Login == login)))
					) &&
					(
						(x.Campaign.TargetEvents.Count() == 0) ||
						(x.Campaign.TargetEvents.Any(y => y.Id == eventId))
					) &&
					(x.Campaign.ConcessionId == paymentConcessionId) &&
					(x.Campaign.Concession.Concession.State == ConcessionState.Active)
				)
				.Select(x => new
				{
					CampaingLine = x,
					Codes = x.Campaign.CampaignCodes
						.Where(y => codes.Contains(y.Code))
				})
				.ToList()
				.Select(x => new KeyValuePair<CampaignLine, IEnumerable<CampaignCode>>(
					x.CampaingLine,
					x.Codes
				));

			var time = new XpTime(now);
			var result2 = result
				.Where(x =>
					time.InPeriod(x.Key.SinceTime, x.Key.UntilTime) // Se ejecuta en memoria
				)
				.ToList();

			return result2;
		}
		#endregion GetActiveCampaignLinesAsync

		#region GenerateExtraLinesAsync
		public async Task<IEnumerable<TicketLine>> GenerateExtraLinesAsync(IEnumerable<MobileTicketCreateAndGetArguments_TicketLine> argumentLines, long? uid, string login, int paymentConcessionId, int? eventId, int? transportPrice, DateTime now, bool throwExceptions, bool campaignsAlreadyApplied)
		{
			var entranceTypeIds = argumentLines
				.Select(y => y.EntranceTypeId);
			var entranceTypes = (await EntranceTypeRepository.GetAsync(
				"CampaignLines.Campaign.TargetSystemCard.Cards.Users"
			))
				.Where(x => entranceTypeIds
					.Contains(x.Id)
				)
				.ToDictionary(x => x.Id);

			var productIds = argumentLines
				.Select(y => y.ProductId);
			var products = (await ProductRepository.GetAsync(
			//"CampaignLines.Campaign.TargetSystemCard.Cards.Users"
			))
				.Where(x => productIds
					.Contains(x.Id)
				)
				.ToDictionary(x => x.Id);

			var codes = argumentLines
				.Where(x =>
					x.CampaignCode != null
				)
				.Select(x =>
					x.CampaignCode.Value
				);

			var lines = new List<TicketLine>();
			if (argumentLines == null)
				return lines;

			// Get active campaign lines
			var campaignLineWithCodes = await GetActiveCampaignLinesAsync(login, paymentConcessionId, eventId, now, codes);

			// Cargar lineas de campaña
			var campaignLineIds = argumentLines
				.Select(x => x.CampaignId)
				.ToList();
			var campaignLineTicket = (await CampaignLineRepository.GetAsync())
				.Where(x => campaignLineIds.Contains(x.Id))
				.ToList();

			// Cargar entradas
			var entranceIds = argumentLines
				.Select(x => x.EntranceId)
				.ToList();
			var entrancesTicket = (await EntranceRepository.GetAsync("EntranceType.Event.EntranceSystem")) // EntranceType necesario para luego comprobar cosas al devolver entradas y devolver las entradas
				.Where(x =>
					(entranceIds.Contains(x.Id)))
				.ToList();

			foreach (var argumentLine in argumentLines)
			{
				if ((argumentLine.ProductId != null) && (argumentLine.EntranceTypeId != null))
					if (throwExceptions)
						throw new ApplicationException("Una linea de ticket no puede estar asociada a una entrada y un producto al mismo tiempo");

				var purse = (await PurseRepository.GetAsync())
					.Where(x => x.Id == argumentLine.PurseId)
					.FirstOrDefault();

				TicketLine line = null;

				#region TicketLine
				{
					var campaignLineTicketLine = campaignLineTicket
						.Where(x => x.Id == argumentLine.CampaignId)
						.FirstOrDefault();
					var entrancesTicketLine = entrancesTicket
						.Where(x => x.Id == argumentLine.EntranceId)
						.ToList();

					// Ticket Line
					line = new TicketLine
					{
						Title = argumentLine.Title.IsNullOrEmpty() ? TicketResources.Several : argumentLine.Title,
						Amount = argumentLine.Amount ?? 0,
						Quantity = argumentLine.Quantity,
						TransportPriceId = transportPrice,
						EntranceTypeId = argumentLine.EntranceTypeId,
						ProductId = argumentLine.ProductId,
						Type = argumentLine.Type,
						CampaignLineId = campaignLineTicketLine?.Id,
						CampaignId = argumentLine.CampaignId ?? campaignLineTicketLine?.CampaignId,
						CampaignCode = argumentLine.CampaignCode,
						Uid = uid,
						Purse = purse,
						Entrances = entrancesTicketLine
					};
					lines.Add(line);
				}
				#endregion TicketLine

				if (argumentLine.Type != TicketLineType.Recharge)
				{
					#region Entrance
					if ((argumentLine.Amount == null) && (argumentLine.EntranceTypeId != null))
					{
						if (!entranceTypes.ContainsKey(argumentLine.EntranceTypeId.Value))
							throw new ArgumentNullException("Lines.EntranceTypeId");
						var entranceType = entranceTypes[argumentLine.EntranceTypeId.Value];
						if (
							(!entranceType.IsVisible) &&
							(entranceType.CampaignLines
								.Any(x => x.Campaign.TargetSystemCard.Cards
									.Any(y => y.Users
										.Any(z => z.Login == SessionData.Login)
									)
								)
							)
						)
							throw new Exception("No se puede comprar un tipo de entrada no visible");

						// XAVI: Comprobar grupos

						// XAVI: Comprobar que funciona bien los descuentos

						// Ticket line
						line.Title = entranceType.Name;
						line.Amount = entranceType.Price;
						line.Type = TicketLineType.Entrance;

						// Extra price
						if (entranceType.ExtraPrice != 0)
							lines.Add(new TicketLine
							{
								Title = "Extra: " + line.Title,
								Amount = entranceType.ExtraPrice,
								Quantity = argumentLine.Quantity,
								TransportPriceId = transportPrice,
								EntranceTypeId = argumentLine.EntranceTypeId,
								ProductId = null,
								Type = TicketLineType.ExtraPrice
							});
					}
					#endregion Entrance

					#region Product
					if ((argumentLine.Amount == null) && (argumentLine.ProductId != null))
					{
						if (!products.ContainsKey(argumentLine.ProductId.Value))
							throw new ArgumentNullException("Lines.ProductId");
						var product = products[argumentLine.ProductId.Value];
						if (
							(!product.IsVisible) &&
							(product.CampaignLines
								.Any(x => x.Campaign.TargetSystemCard.Cards
									.Any(y => y.Users
										.Any(z => z.Login == SessionData.Login)
									)
								)
							)
						)
							throw new Exception("No se puede comprar un producto no visible");
						if (product.Price == null)
							throw new Exception("No se puede comprar un producto sin precio");

						// Ticket line
						line.Title = product.Name;
						line.Amount = product.Price.Value;
						line.Type = TicketLineType.Product;
					}
					#endregion Product

					#region Discount
					if (!campaignsAlreadyApplied)
					{
						var campaignLines = campaignLineWithCodes
							.Where(x =>
								(x.Key.Type == CampaignLineType.DirectPrice) &&
								((argumentLine.ProductId == null) || x.Key.AllProduct || x.Key.Products.Any(y => y.Id == argumentLine.ProductId)) &&
								((argumentLine.EntranceTypeId == null) || x.Key.AllEntranceType || x.Key.EntranceTypes.Any(y => y.Id == argumentLine.EntranceTypeId)) &&
								((argumentLine.CampaignId == null) || (x.Key.CampaignId == argumentLine.CampaignId)) &&
								((argumentLine.CampaignCode == null) || (x.Value.Any(y => y.Code == argumentLine.CampaignCode)))
							)
							.Select(x => new
							{
								x.Key.Id,
								x.Key.Type,
								x.Key.Quantity,
								x.Key.Campaign.Title,
								x.Key.SinceTime,
								x.Key.UntilTime
							})
							.ToList();
						if (campaignLines.Count() > 1)
							throw new ApplicationException("A una linea de ticket solo puede aplicarse una campaña");
						foreach (var campaignLine in campaignLines)
						{
							var discount = 0M;
							if (campaignLine.Type == CampaignLineType.DirectDiscount)
								discount = campaignLine.Quantity;
							else if (campaignLine.Type == CampaignLineType.DirectPercentDiscount)
								discount = line.Amount * campaignLine.Quantity;
							else if (campaignLine.Type == CampaignLineType.DirectPrice)
								discount = line.Amount - campaignLine.Quantity;

							lines.Add(new TicketLine
							{
								Title = campaignLine.Title,
								Amount = -discount,
								Quantity = argumentLine.Quantity,
								TransportPriceId = transportPrice,
								EntranceTypeId = argumentLine.EntranceTypeId,
								ProductId = argumentLine.ProductId,
								Type = TicketLineType.Discount,
								CampaignLineId = campaignLine.Id,
								CampaignLineType = campaignLine.Type,
								CampaignLineQuantity = campaignLine.Quantity
							});
						}
					}
					#endregion Discount
				}
			}

			return lines;
		}
		#endregion GenerateExtraLinesAsync

		#region BuyEntranceAsync
		public async Task BuyEntranceAsync(Ticket ticket, bool payed, TicketLine line, decimal entrancesToBuyOfEvent, IEnumerable<TicketAnswerFormsArguments_Form> forms, long? uid, DateTime now, string email, string login)
		{
			BuyEntranceAsync(ticket, payed, line, entrancesToBuyOfEvent, forms, uid, now, email, login, true);
		}

		public async Task BuyEntranceAsync(Ticket ticket, bool payed, TicketLine line, decimal entrancesToBuyOfEvent, IEnumerable<TicketAnswerFormsArguments_Form> forms, long? uid, DateTime now, string email, string login, bool payTicket)
        {
            if (line.EntranceTypeId == null)
                return;
			if (login.IsNullOrEmpty())
				throw new ArgumentNullException("login");
			if ((line.Type == TicketLineType.ExtraPrice) || (line.Type == TicketLineType.Discount))
				return;
			if (line.Type != TicketLineType.Entrance)
				throw new Exception("El ticket no puede tener un EntranceTypeId y que el tipo no sea ni ExtraPrice ni Entrance");

			// Get entranceType
			var entranceType = (await EntranceTypeRepository.GetAsync(
					"EntranceTypeForms",
					"Event.EntranceSystem"
				))
				.Where(x =>
					x.Id == line.EntranceTypeId &&
					x.State == EntranceTypeState.Active
				)
				.FirstOrDefault();
			if (entranceType == null)
				throw new ArgumentNullException("EntranceTypeId");

			// Check selling dates
			if ((entranceType.SellStart > now) || (entranceType.SellEnd < now))
				throw new ApplicationException(EntranceResources.OutOfBuyablePeriodException.FormatString(entranceType.Name));
			
			// Crear form responses
			var values = new List<ControlFormValue>();
            if (!forms.IsNullOrEmpty())
            {
                var argumentsFormArguments = forms
                .Where(x => entranceType.EntranceTypeForms
                    .Any(y => y.FormId == x.Id)
                )
                .SelectMany(x => x.Arguments);
                foreach (var argumentsFormArgument in argumentsFormArguments)
                {
                    var value = new ControlFormValue
                    {
                        ArgumentId = argumentsFormArgument.Id,
                        Observations = "",
                        Target = ControlFormArgumentTarget.BuyEntrance,
                        ValueNumeric = argumentsFormArgument.ValueNumeric,
                        ValueBool = argumentsFormArgument.ValueBool,
                        ValueDateTime = argumentsFormArgument.ValueDateTime,
                        ValueString = argumentsFormArgument.ValueString
                    };
                    values.Add(value);
                    await ControlFormValueRepository.AddAsync(value);

                    if (!argumentsFormArgument.Options.IsNullOrEmpty())
                    {
                        var argumentsFormOptions = argumentsFormArgument.Options.Select(y => y.Id);
                        value.ValueOptions = (await ControlFormOptionRepository.GetAsync())
                            .Where(x => argumentsFormOptions.Contains(x.Id))
                            .ToList();
                    }
                }
            }
			if (payTicket)
				payed = true;

			// Obtener datos del propietario de la tarjeta de destino
			var taxNumber = SessionData.TaxNumber;
			var taxName = SessionData.TaxName;
			var cardOwner = (await ServiceCardRepository.GetAsync())
				.Where(x => x.Uid == uid)
				.Select(x => new
				{
					taxNumber = x.OwnerUser.VatNumber,
					taxName = x.OwnerUser.Name + " " + x.OwnerUser.LastName
				})
				.FirstOrDefault();
			if (cardOwner != null)
			{
				taxName = (cardOwner.taxName ?? "").Trim();
				taxNumber = cardOwner.taxNumber ?? "";
			}

			// Crear entrances
			await ApiEntranceCreateHandler.CreateEntranceAsync(
				entranceType,
				line,
				line.Quantity, entrancesToBuyOfEvent, line.Amount,
				taxNumber, taxName, email, login,
				uid, payed, now,
				values
			);
        }
		#endregion BuyEntranceAsync

		#region ApplyRechargeAsync
		public async Task<GreyList> ApplyRechargeAsync(Ticket ticket, bool payed, TicketLine line, long? uid, DateTime now, bool throwExceptions)
		{
			if (line.Type != TicketLineType.Recharge)
				return null;

			if (line.Purse == null && line.PurseId != null && line.PurseId > 0)
			{
				var purse = (await PurseRepository.GetAsync())
					.Where(x => x.Id == line.PurseId)
					.FirstOrDefault();
				line.Purse = purse;
			}

			if (line.Purse == null || line.Purse.Slot == null)
			{
				if (throwExceptions)
					throw new ApplicationException("Para recargar un monedero hay que indicar cual");
				else
					return null;
			}
			if ((line.Purse.Slot < 0) || (line.Purse.Slot > 2))
			{
				if (throwExceptions)
					throw new ApplicationException("El sistems solo tiene 3 monederos");
				else
					return null;
			}
			if (!payed)
				return null;
			if (uid == null)
				return null;

			var greyList = new GreyList
			{
				Uid = uid.Value,
				RegistrationDate = now,
				Action = GreyList.ActionType.IncreaseBalance,
				Field =
					line.Purse.Slot == 0 ? "W0B" :
					line.Purse.Slot == 1 ? "W1B" :
					"W2B",
				NewValue = Math.Ceiling(line.Quantity * line.Amount * 100).ToString(),
				Resolved = false,
				ResolutionDate = null,
				Machine = GreyList.MachineType.All,
				Source = GreyList.GreyListSourceType.PayFalles,
				State = GreyList.GreyListStateType.Active,
				SystemCardId = line.Purse.SystemCardId
			};
			await GreyListRepository.AddAsync(greyList);

			return greyList;
		}
		#endregion ApplyRechargeAsync

		#region ApplyRechargeAsync
		public async Task ReturnEntrance(Ticket ticket, bool payed, TicketLine line, long? uid, DateTime now, bool throwExceptions)
		{
			await Task.Run(() =>
			{
				if (line.Type != TicketLineType.Entrance)
					return;
				if ((line.Entrances?.Count() ?? 0) == 0)
					return;
				if (line.Quantity >= 0)
					return;
				if (-1 * line.Quantity != line.Entrances.Count())
				{
					if (throwExceptions)
						throw new ApplicationException("No coincide el número de entradas a devolver con la cuenta de las entradas a devolver.");
					return;
				}

				foreach (var entrance in line.Entrances)
				{
					if (entrance.EntranceType.CheckInStart >= now)
					{
						if (throwExceptions)
							throw new ApplicationException($"No se puede devolver la entrada {entrance.Code} ({entrance.EntranceType.Name}) porque ya están abiertas las puertas para dicha entrada.");
						continue;
					}

					if (entrance.State != EntranceState.Active)
					{
						if (throwExceptions)
							throw new ApplicationException($"No se puede devolver la entrada {entrance.Code} ({entrance.EntranceType.Name}) porque no está activa");
						continue;
					}

					entrance.State = EntranceState.Returned;
				}
			});
		}
		#endregion ApplyRechargeAsync
	}
}
