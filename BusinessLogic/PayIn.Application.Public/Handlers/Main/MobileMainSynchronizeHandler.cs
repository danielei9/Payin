using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
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
using Xp.Domain.Transport;

namespace PayIn.Application.Public.Handlers
{
	[XpLog("Main", "Synchronize")]
    public class MobileMainSynchronizeHandler :
		IServiceBaseHandler<MobileMainSynchronizeArguments>
	{
		[Dependency] public SessionData SessionData { get; set; }
        [Dependency] public IUnitOfWork UnitOfWork { get; set; }
        [Dependency] public IEntityRepository<ServiceUser> ServiceUserRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }
		[Dependency] public IEntityRepository<Event> EventRepository { get; set; }
		[Dependency] public IEntityRepository<Notice> NoticeRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<Ticket> TicketRepository { get; set; }
        [Dependency] public IEntityRepository<EntranceType> EntranceTypeRepository { get; set; }
        [Dependency] public IEntityRepository<Product> ProductRepository { get; set; }
		[Dependency] public IEntityRepository<Campaign> CampaignRepository { get; set; }
		[Dependency] public IEntityRepository<CampaignLine> CampaignLineRepository { get; set; }
		[Dependency] public IEntityRepository<ControlForm> ControlFormRepository { get; set; }
        [Dependency] public IEntityRepository<Entrance> EntranceRepository { get; set; }
        [Dependency] public IEntityRepository<ServiceCategory> ServiceCategoryRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceGroup> ServiceGroupRepository { get; set; }
		[Dependency] public IEntityRepository<BlackList> BlackListRepository { get; set; }
        [Dependency] public IEntityRepository<GreyList> GreyListRepository { get; set; }
		[Dependency] public IEntityRepository<TransportOperation> TransportOperationRepository { get; set; }
		[Dependency] public IEntityRepository<Purse> PurseRepository { get; set; }
		[Dependency] public IEntityRepository<PurseValue> PurseValueRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceOperation> ServiceOperationRepository { get; set; }
		[Dependency] public IEntityRepository<Translation> TranslationRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobileMainSynchronizeArguments arguments)
		{
            var now = DateTime.UtcNow;
            var errors = new List<string>();
			var serviceUsersAll = (await ServiceUserRepository.GetAsync());

			var purses = (await PurseRepository.GetAsync())
				.Where(x =>
					x.State == PurseState.Active
				);
			var rechargeGreyList = (await GreyListRepository.GetAsync())
				.Where(x =>
					!x.Resolved &&
					x.ResolutionDate == null &&
					x.Field == "W1B"
				);
			var translations = (await TranslationRepository.GetAsync())
				.Where(y =>
					(y.Language == arguments.Language)
				);

			var whereIWork = (await PaymentConcessionRepository.GetAsync())
                .Where(x =>
                    (x.Concession.State == ConcessionState.Active) &&
                    (x.PaymentWorkers.Any(y => y.Login == SessionData.Login))
                );

            if (arguments.Tickets != null)
            {
                // Limpiar datos
                foreach (var ticket in arguments.Tickets)
                    if (ticket.EventId == 0)
                        ticket.EventId = null;

                // Actualizar info
                foreach (var ticket in arguments.Tickets)
                {
                    if (
                        (ticket.Type == TicketType.Ticket) &&
                        (
                            ticket.Lines.Any(x => x.Title == "Devolución tarjeta") ||
                            ticket.Lines.Any(x => x.Title == "Recarga")
                        )
                    )
                        ticket.Type = TicketType.Recharge;
                }
            }

            var paymentConcession = (await PaymentConcessionRepository.GetAsync())
                .Where(x =>
                    (x.Concession.State == ConcessionState.Active) &&
                    (x.Id == arguments.ConcessionId)
                )
                .Select(x => new
                {
                    x.Id,
                    SupplierName = x.Concession.Supplier.Name,
                    x.Concession.Supplier.TaxNumber,
                    x.Concession.Supplier.TaxName,
                    x.Concession.Supplier.TaxAddress,
                    Worker = x.PaymentWorkers
                        .Where(y =>
                            (y.State == WorkerState.Active) &&
                            (y.Login == SessionData.Login)
                        )
                        .Select(y => new
                        {
                            y.Id
                        })
                        .FirstOrDefault(),
                        x.LiquidationConcessionId
                })
                .FirstOrDefault();
            if (paymentConcession == null)
                throw new ArgumentNullException("concessionId");
            
            #region Get campaigns
            var campaigns = (await CampaignRepository.GetAsync(
                    "TargetEvents",
                    "CampaignLines.Products",
                    "CampaignLines.EntranceTypes",
					"CampaignLines." + nameof(CampaignLine.ServiceUsers),
					"CampaignLines." + nameof(CampaignLine.ServiceGroups)
				))
                .Where(y =>
                    (y.Concession.Id == paymentConcession.Id) &&
                    (y.Since <= now) &&
                    (y.Until >= now) &&
                    (y.State == CampaignState.Active) &&
                    (
                        (y.TargetSystemCardId == null) ||
                        (y.TargetSystemCard.Cards
                            .Any(z => z.Users
                                .Any(a => a.Login == SessionData.Login)
                            )
                        )
                    )
                )
                .ToList();
			#endregion Get campaigns

			#region Get generatedCampaignLines
			var generatedCampaignLines = (await CampaignLineRepository.GetAsync())
				.Where(x =>
					(x.State == CampaignLineState.Active) &&
					(x.Campaign.State == CampaignState.Active) &&
					(x.Campaign.Since <= now) &&
					(x.Campaign.Until >= now) &&
					(x.Campaign.ConcessionId == paymentConcession.Id) &&
					(
						(x.Campaign.TargetSystemCardId == null) ||
						(x.Campaign.TargetSystemCard.ConcessionOwner.Supplier.Login == SessionData.Login) ||
						(x.Campaign.TargetSystemCard.ConcessionOwner.Supplier.Workers
							.Any(y => y.Login == SessionData.Login)
						) ||
						(x.Campaign.TargetSystemCard.Cards
							.Any(z => z.Users
								.Any(a => a.Login == SessionData.Login)
							)
						)
					)
				)
				.Select(y => new
				{
					y.Id,
					y.Max,
					y.Min,
					y.Quantity,
					y.Type,
					y.SinceTime,
					y.UntilTime,
					y.CampaignId,
					CampaignTitle = y.Campaign.Title,
					CampaignSince = y.Campaign.Since,
					CampaignUntil = y.Campaign.Until,
					y.AllProduct,
					y.AllEntranceType,
					Products = y.Products
						.Select(z => z.Id),
					Families = y.ProductFamilies
						.Select(z => z.Id),
					EntranceTypes = y.EntranceTypes
						.Select(z => z.Id),
					Users = y.ServiceUsers
						.Select(z => new MobileMainSynchronizeResult_CampaignLine_User
						{
							Id = z.ServiceUser.Id
						}),
					Groups = y.ServiceGroups
						.Select(z => new MobileMainSynchronizeResult_CampaignLine_Group
						{
							Id = z.ServiceGroup.Id
						})
				});
			#endregion Get generatedCampaignLines

			#region Get entranceTypes in tickets
			List<Tuple<EntranceType, int>> entranceTypesInTickets = null;
			if (arguments.Tickets != null) {
				var entranceTypesInTicketsIds = arguments?.Tickets
                .SelectMany(x => x.Lines
                    .Select(y => y.EntranceTypeId ?? 0)
                )
                .Where(x => x != 0)
                .ToList();
            entranceTypesInTickets = (await EntranceTypeRepository.GetAsync(
                "CampaignLines.Campaign.TargetSystemCard.Cards.Users",
                "EntranceTypeForms"
            //"Event.EntranceSystem"
            ))
                .Where(x =>
                    (x.State == EntranceTypeState.Active) &&
                    (entranceTypesInTicketsIds
                        .Contains(x.Id)
                    )
                )
                .Select(x => new
                {
                    item = x,
                    entranceCount = x.Entrances
                        .Where(y =>
                            y.State == EntranceState.Active ||
                            y.State == EntranceState.Pending
                        )
                        .Count()//,
                                //x.Entrances
                                //    .Where(y =>
                                //        y.State == EntranceState.Active ||
                                //        y.State == EntranceState.Pending
                                //    )
                                //    .Max(y => x.Code)
                })
                .ToList()
                .Select(x => Tuple.Create(
                    x.item,
                    x.entranceCount
                ))
                .ToList();
			}
			#endregion Get entranceTypes in tickets

			#region Get products in tickets
			List<Product> productsInTickets = null;
			if (arguments.Tickets != null)
			{
				var productsInTicketsIds = arguments.Tickets
				.SelectMany(x => x.Lines
					.Select(y => y.ProductId ?? 0)
				)
				.Where(x => x != 0)
				.ToList();
				productsInTickets = (await ProductRepository.GetAsync(
				//"CampaignLines.Campaign.TargetSystemCard.Cards.Users"
				))
					.Where(x => productsInTicketsIds
						.Contains(x.Id)
					)
					.ToList();
			}
			#endregion Get products in tickets

			#region Get Forms in tickets
			List<ControlForm> formsInTickets = null;
			if (arguments.Tickets != null)
			{
				var formsInTicketsIds = arguments.Tickets
				.Where(x => x.Forms != null)
				.SelectMany(x => x.Forms
					.Select(y => y.Id)
				)
				.ToList();
				formsInTickets = (await ControlFormRepository.GetAsync(
					"Arguments.Options"
				))
					.Where(x => formsInTicketsIds
						.Contains(x.Id)
					)
					.ToList();
			}
			#endregion Get Forms

			ServiceOperation operation = null;

            #region Crear ServiceCardEmitions / Ticket / ServiceCardReturned
            {
				var nextEmisions = arguments.ServiceCardEmitions
					.OrderBy(x => x.LastSeq) as IEnumerable<MobileMainSynchronizeArguments_ServiceCardEmition>;
				var nextReturns = arguments.ServiceCardReturned
					.OrderBy(x => x.Seq) as IEnumerable<MobileMainSynchronizeArguments_ServiceCardReturned>;
				var nextTickets = arguments.Tickets
					.OrderBy(x => x.Seq) as IEnumerable<MobileMainSynchronizeArguments_Ticket>;

				while ((nextEmisions.Any()) || (nextReturns.Any()) || (nextTickets.Any()))
				{
					var nextEmision = nextEmisions
						.FirstOrDefault();
					var nextReturn = nextReturns
						.FirstOrDefault();
					var nextTicket = nextTickets
						.FirstOrDefault();

					var nextEmisionSeq = nextEmision?.LastSeq ?? int.MaxValue;
					var nextReturnSeq = nextReturn?.Seq ?? int.MaxValue;
					var nextTicketSeq = nextTicket?.Seq ?? int.MaxValue;

					// Ticket siempre antes de la devolucion
					if ((nextEmisionSeq <= nextReturnSeq) && (nextEmisionSeq <= nextTicketSeq))
					{
						// Emision
						operation = await AddEmisionAsync(operation, nextEmision, arguments.SystemCardId, errors, now);

						// Next
						nextEmisions = nextEmisions
							.Skip(1);
					}
					else if ((nextTicketSeq <= nextEmisionSeq) && (nextTicketSeq <= nextReturnSeq))
					{
						// Emision
						//operation = await AddTicketAsync(operation, nextTicket, arguments.SystemCardId, campaigns, paymentConcession, entranceTypesInTickets, productsInTickets, errors, now);

						// Esto es una chapuza, ya que vamos a subir las lecturas como tickets pero se ha de sacar fuera
						if (nextTicket.Type == TicketType.Reading)
							operation = await AddRead(nextTicket, arguments.SystemCardId);
						else
						{
							#region Crear Tickets
							var ticketArg = nextTicket;

							var ticketCampaigns = campaigns
								.Where(x =>
									(x.TargetEvents.Count() == 0) ||
									(x.TargetEvents.Any(y =>
										(y.Id == ticketArg.EventId)
									))
								)
								.ToList();

							var ticketForms = formsInTickets
								.Where(x =>
									ticketArg.Forms
										.Where(y => y.Id == x.Id)
										.Any()
								)
								.ToList();

							var paymentConcessionTicket = paymentConcession;
							if ((ticketArg.PaymentConcessionId != null) && (paymentConcession.Id != ticketArg.PaymentConcessionId))
								paymentConcessionTicket = (await PaymentConcessionRepository.GetAsync())
									.Where(x =>
										(x.Concession.State == ConcessionState.Active) &&
										(x.Id == ticketArg.PaymentConcessionId)
									)
									.Select(x => new
									{
										x.Id,
										SupplierName = x.Concession.Supplier.Name,
										x.Concession.Supplier.TaxNumber,
										x.Concession.Supplier.TaxName,
										x.Concession.Supplier.TaxAddress,
										Worker = x.PaymentWorkers
											.Where(y =>
												(y.State == WorkerState.Active) &&
												(y.Login == SessionData.Login)
											)
											.Select(y => new
											{
												y.Id
											})
											.FirstOrDefault(),
										x.LiquidationConcessionId
									})
									.FirstOrDefault() ?? paymentConcessionTicket;
							if (paymentConcessionTicket == null)
								throw new ArgumentNullException("tickets.concessionId");

							var ticket = await CreateTicketsAsync(
								paymentConcessionTicket.Id, paymentConcessionTicket.SupplierName,
								paymentConcessionTicket.TaxNumber, paymentConcessionTicket.TaxName, paymentConcessionTicket.TaxAddress,
								ticketArg.Reference, ticketArg.Date,
								paymentConcessionTicket.Worker?.Id, ticketArg.EventId,
								ticketArg.Lines, ticketArg.Payments, ticketArg.Forms, ticketArg.Wallets,
								entranceTypesInTickets, productsInTickets, ticketCampaigns, ticketForms,
								paymentConcessionTicket.LiquidationConcessionId, ticketArg.TransportPrice, ticketArg.Type,
								ticketArg.Login.IsNullOrEmpty() ? SessionData.Email : ticketArg.Login,
								ticketArg.Login.IsNullOrEmpty() ? SessionData.Login : ticketArg.Login,
								ticketArg.ExternalLogin, null, now, errors);

							var balance = ticketArg.Wallets
								.Sum(x => x.Balance);

							// Lines
							long? uidLine = null;
							foreach (var lineArg in ticketArg.Lines)
								uidLine = uidLine ?? lineArg.Uid;

							// Payments
							long? uidPayment = null;
							foreach (var paymentArg in ticketArg.Payments)
							{
								var serviceCardPayment = (await ServiceCardRepository.GetAsync("ServiceCardBatch"))
									.Where(x =>
										x.SystemCardId == arguments.SystemCardId &&
										x.Uid == paymentArg.Uid
									)
									.FirstOrDefault();

								// Entiendo que esto es temporal para no dejar fuera la app que ahora hay en Mallorca
								uidPayment = uidPayment ?? paymentArg.Uid;

								foreach (var payment in ticket.Payments)
								{
									payment.Uid = paymentArg.Uid;
									payment.Seq = ticketArg.Seq;
								}

								if (serviceCardPayment == null)
									continue;

								foreach (var payment in ticket.Payments)
									payment.UidFormat = serviceCardPayment.ServiceCardBatch.UidFormat;
							}

							var serviceCard = (await ServiceCardRepository.GetAsync("OwnerUser.Groups"))
								.Where(x =>
									(x.SystemCardId == arguments.SystemCardId) &&
									(x.Uid == (ticketArg.Uid ?? uidLine ?? uidPayment))
								)
								.FirstOrDefault();
							if (serviceCard != null)
							{
								// Event
								var event_ = (ticketArg.EventId != null) ?
									await EventRepository.GetAsync(ticketArg.EventId.Value) :
									null;

								// Operation
								operation = await CreateServiceOperationAsync(
									operation,
									ticketArg,
									serviceCard,
									event_
								);
								ticket.ServiceOperations.Add(operation);
							}
							#endregion Crear Tickets
						}

						// Next
						nextTickets = nextTickets
							.Skip(1);
					}
					else if ((nextReturnSeq <= nextEmisionSeq) && (nextReturnSeq <= nextTicketSeq))
                    {
                        // Return
                        operation = await AddReturnAsync(operation, nextReturn, arguments.SystemCardId, errors, now);

						// Next
						nextReturns = nextReturns
							.Skip(1);
					}
                }
            }
			#endregion Crear ServiceCardEmitions / Ticket / ServiceCardReturned

			#region Crear GreyList
			if (arguments.GreyList != null)
            {
				foreach (var greyListResolution in arguments.GreyList)
				{
					var greyListToResolve = (await GreyListRepository.GetAsync(greyListResolution.Id));
					if (greyListToResolve == null)
					{
						errors.Add($"Aplicar Lista Gris : No se puede Aplicar la lista gris a {greyListResolution.CardUid} - porque no existente");
						continue;
					}

					greyListToResolve.ResolutionDate = greyListResolution.ResolutionDate;
					greyListToResolve.Resolved = true;
                }
            }
            #endregion Crear GreyList

            #region Crear BlackListUpdates
            if (arguments.BlackList != null)
            {
                foreach (var blackListUpdate in arguments.BlackList)
                {
                    var blackList2 = (await BlackListRepository.GetAsync())
                        .Where(x =>
                            (x.Uid == blackListUpdate.Uid) &&
                            (x.State == BlackList.BlackListStateType.Active) &&
                            (x.SystemCardId == arguments.SystemCardId) &&
                            (!x.Resolved)
                        )
                        .FirstOrDefault();

                    if (blackListUpdate.Resolved)
                    {
                        // Desbloquear
                        if (blackList2 == null)
                            errors.Add($"Desbloquear : No se puede desbloquear la tarjeta {blackListUpdate.Uid} - porque no está bloqueada");
                        else
                        {
                            blackList2.Resolved = true;
                            blackList2.ResolutionDate = now;
                            blackList2.IsConfirmed = true;
                        }
                    }
                    else
                    {
                        // Bloquear
                        if (blackList2 != null)
                            errors.Add($"Bloquear : No se puede bloquear la tarjeta {blackListUpdate.Uid} - porque ya está bloqueada");
                        else
                        {
                            blackList2 = new BlackList
                            {
                                Uid = blackListUpdate.Uid,
                                RegistrationDate = blackListUpdate.RegistrationDate,
                                Machine = BlackListMachineType.All,
                                Resolved = false,
                                ResolutionDate = null,
                                Concession = 0,
                                Service = BlackListServiceType.Rejection,
                                IsConfirmed = false,
                                Source =
                                    SessionData.ClientId == AccountClientId.AndroidVilamarxantNative ? BlackList.BlackListSourceType.PayVilamarxant :
                                    SessionData.ClientId == AccountClientId.AndroidFallesNative ? BlackList.BlackListSourceType.PayFalles :
                                    BlackList.BlackListSourceType.Payin,
                                State = BlackList.BlackListStateType.Active,
                                SystemCardId = arguments.SystemCardId
                            };
                            await BlackListRepository.AddAsync(blackList2);
                        }
                    }
                }
            }
            #endregion Crear BlackListUpdates

            await UnitOfWork.SaveAsync();

			#region ServiceUsers
			var serviceUsers2 = serviceUsersAll
				.Where(x =>
					(
						x.Concession.Supplier.Login == SessionData.Login ||
						x.Concession.Supplier.Workers
							.Any(y => y.Login == SessionData.Login)
					) &&
					(x.Concession.State == ConcessionState.Active) &&
					(x.State == ServiceUserState.Active)
				)
				.Select(x => new
				{
					x.Id,
					x.Name,
					x.LastName,
					HasPrimaryCard = x.CardId != null,
					x.CardId,
					CardUid = (long?)x.Card.Uid,
					CardAlias = x.Card.Alias,
					Groups = x.Groups
						.Select(y => new MobileMainSynchronizeResult_ServiceGroup
						{
							Id = y.Id
						}),
					OwnerCards = x.OnwnerCards
						.Select(y => new
						{
							y.Id,
							y.Uid,
							y.UidText,
							y.ServiceCardBatch.UidFormat,
							y.Alias,
							//y.LastSeq, // LastXAVI
							LastSeq = y.Operations
								.Max(z => z.Seq) ?? 0,
							//y.LastBalance // LastXAVI
							LastBalance = y.Operations
								.Where(z => z.Seq == y.Operations
									.Max(a => a.Seq) &&
									!y.Operations
										.Any(b =>
											b.Seq == z.Seq &&
											b.Id > z.Id
										)
								)
								.Sum(z => purses
									.Where(a => a.SystemCardId == y.SystemCardId)
									.Sum(a => a.PurseValues
										.Where(b => b.ServiceOperationId == z.Id)
										.Sum(b => (decimal?)b.Amount)
									)
								) ?? 0,
							PendingBalances = rechargeGreyList
								.Where(z => z.Uid == y.Uid)
								.Select(z => z.NewValue)
								.ToList() // Si no se pone en la consulta después da error de metodo no soportado
						})
				})
				.ToList();
			var serviceUsers = serviceUsers2
				.Select(x => new MobileMainSynchronizeResult_ServiceUser
                {
                    Id = x.Id,
                    Name = x.Name,
                    LastName = x.LastName,
                    HasPrimaryCard = x.HasPrimaryCard,
                    CardId = x.CardId,
                    CardUid = x.CardUid,
                    CardAlias = x.CardAlias,
                    Groups = x.Groups,
                    OwnerCards = x.OwnerCards
                        .Select(y => new MobileMainSynchronizeResult_ServiceCardEmited
                        {
                            Id = y.Id,
                            Uid = y.Uid,
                            UidText = y.UidText,
                            Alias = y.Alias,
                            LastSeq = y.LastSeq, // LastXAVI
							LastBalance = y.LastBalance, // LastXAVI
							PendingBalance = y.PendingBalances
								.Sum(z =>
									(decimal?)Convert.ToInt32(z)/100
								) ?? 0m
						})
                });
            #endregion ServiceUsers

            #region Get ServiceCardsNotEmited
            var serviceCardsNotEmited = (await ServiceCardRepository.GetAsync())
                .Where(x =>
                    (
                        x.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login ||
                        x.SystemCard.ConcessionOwner.Supplier.Workers
                            .Any(y => y.Login == SessionData.Login)
                    ) &&
                    (x.SystemCard.ConcessionOwner.State == ConcessionState.Active) &&
                    (x.State == ServiceCardState.Active)
                )
                .Select(x => new MobileMainSynchronizeResult_ServiceCardNotEmited
                {
                    Id = x.Id,
                    Uid = x.Uid,
					//LastSeq = x.LastSeq // LastXAVI
					LastSeq = x.Operations
						.Max(y => y.Seq) ?? 0
				})
                .ToList();
			#endregion Get ServiceCardsNotEmited

			#region Get ServiceCardsEmited
			var serviceCardsEmited = (await ServiceCardRepository.GetAsync())
				.Where(x =>
					(
						x.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login ||
						x.SystemCard.ConcessionOwner.Supplier.Workers
							.Any(y => y.Login == SessionData.Login)
					) &&
					(x.SystemCard.ConcessionOwner.State == ConcessionState.Active) &&
					(x.State == ServiceCardState.Emited)
				)
				.Select(x => new
				{
					x.Id,
					x.Uid,
					x.Alias,
					x.ServiceCardBatch.UidFormat,
					x.OwnerUserId,
					//x.LastSeq, // LastXAVI
					LastSeq = x.Operations
								.Max(z => z.Seq) ?? 0,
					//x.LastBalance // LastXAVI
					LastBalance = x.Operations
						.Where(z => z.Seq == x.Operations
							.Max(a => a.Seq) &&
							!x.Operations
								.Any(b =>
									b.Seq == z.Seq &&
									b.Id > z.Id
								)
						)
						.Sum(z => purses
							.Where(a => a.SystemCardId == x.SystemCardId)
							.Sum(a => a.PurseValues
								.Where(b => b.ServiceOperationId == z.Id)
								.Sum(b => (decimal?)b.Amount)
							)
						) ?? 0,
					PendingBalances = rechargeGreyList
						.Where(z => z.Uid == x.Uid)
						.Select(z => z.NewValue)
						.ToList() // Si no se pone en la consulta después da error de metodo no soportado
				})
				.ToList()
				.Select(x => new MobileMainSynchronizeResult_ServiceCardEmited
				{
					Id = x.Id,
					Uid = x.Uid,
					UidText = ServiceCard.GetUidText(x.Uid, x.UidFormat),
					Alias = x.Alias,
					OwnerUserId = x.OwnerUserId,
					LastSeq = x.LastSeq, // LastXAVI
					LastBalance = x.LastBalance, // LastXAVI
					PendingBalance = x.PendingBalances
						.Sum(y => (decimal?)Convert.ToInt32(y)/100) ?? 0
				});
            #endregion Get ServiceCardsEmited

			#region Categorias
			var categories = (await ServiceCategoryRepository.GetAsync())
				.Where(x =>
					x.ServiceConcession.Supplier.Login == SessionData.Login ||
					x.ServiceConcession.Supplier.Workers
						.Any(y => y.Login == SessionData.Login)
				)
				.Select(x => new
				{
					x.Id,
					x.Name,
					x.AllMembersInSomeGroup,
					x.AMemberInOnlyOneGroup,
					x.AskWhenEmit,
					x.DefaultGroupWhenEmitId,
					Groups = x.Groups
						.Select(y => new
						{
							y.Id,
							y.Name,
							Products = y.Products
								.Select(z => z.ProductId),
							EntranceTypes = y.EntranceTypes
								.Select(z => z.EntranceTypeId)
						})
				})
				.ToList();
			#endregion Categorias

			var events = new List<MobileMainSynchronizeResult_Event>();
			#region Events
			if (
				// Apps ad-hoc
				(SessionData.ClientId == AccountClientId.AndroidVilamarxantNative) ||
				(SessionData.ClientId == AccountClientId.AndroidFinestratNative) ||
				(SessionData.ClientId == AccountClientId.AndroidFallesNative) ||
				(SessionData.ClientId == AccountClientId.FiraValenciaApp) ||
				// App pro
				(SessionData.ClientId == AccountClientId.CashlessProApp)
			)
			{
				events = (await EventRepository.GetAsync())
					.Where(x =>
						(
							// Mis eventos
							x.PaymentConcession.Concession.Supplier.Login == SessionData.Login ||
							x.PaymentConcession.Concession.Supplier.Workers
								.Any(y => y.Login == SessionData.Login) ||
							// Eventos del ST al que pertenezco
							x.PaymentConcession.Concession.SystemCardOwners
								.Any(y => y.SystemCardMembers
									.Any(z => z.Login == SessionData.Login)
								) ||
							x.PaymentConcession.Concession.SystemCardOwners
								.Any(y => y.SystemCardMembers
									.Any(z =>
										(z.State == SystemCardMemberState.Active) &&
										(whereIWork
											.Any(a =>
												(a.Concession.Supplier.Login == z.Login)
											)
										)
									)
								)
						) &&
						(x.PaymentConcession.Concession.State == ConcessionState.Active) &&
						(x.State == EventState.Active) &&
						(
							(x.CheckInStart <= now) ||
							// Apps ad-hoc
							(SessionData.ClientId == AccountClientId.AndroidVilamarxantNative) ||
							(SessionData.ClientId == AccountClientId.AndroidFallesNative) ||
							(SessionData.ClientId == AccountClientId.FiraValenciaApp)
						)&&
						(x.CheckInEnd >= now)// &&
						//(x.IsVisible)
					)
					.Select(x => new
					{
						x.Id,
						x.PhotoUrl,
						x.Name,
						x.ShortDescription,
						x.Description,
						x.Capacity,
						x.CheckInStart,
						x.CheckInEnd,
						x.EventStart,
						x.EventEnd,
						x.MaxAmountToSpend,
						Entrances = x.EntranceTypes
							.Where(y =>
								(y.State == EntranceTypeState.Active) &&
								(y.CheckInStart <= now) &&
								(y.CheckInEnd >= now)
							)
							.SelectMany(y => y.Entrances
								.Where(z =>
									(z.State == EntranceState.Active)
								)
								.Select(z => new
								{
									z.Id,
									z.Code,
									Uid =
										z.Uid ??
										serviceUsersAll
											.Where(a =>
												(a.Login == z.Login) &&
												(a.State == ServiceUserState.Active)
											)
											.Select(a => (long?)a.Card.Uid)
											.FirstOrDefault(),
									z.VatNumber,
									z.UserName,
									z.LastName,
									EntranceTypeName = y.Name,
									CheckInStart = x.CheckInStart <= y.CheckInStart ? y.CheckInStart : x.CheckInStart,
									CheckInEnd = x.CheckInEnd >= y.CheckInEnd ? y.CheckInEnd : x.CheckInEnd
								})
							)
					})
					.ToList()
					.Select(x => new MobileMainSynchronizeResult_Event
					{
						Id = x.Id,
						PhotoUrl = x.PhotoUrl,
						Name = x.Name,
						ShortDescription = x.ShortDescription,
						Description = x.Description,
						Capacity = x.Capacity,
						CheckInStart = x.CheckInStart.ToUTC(),
						CheckInEnd = x.CheckInEnd.ToUTC(),
						IsOnline = x.MaxAmountToSpend != null,
						Entrances = x.Entrances
							.Select(y => new MobileMainSynchronizeResult_ValidationEntrance
							{
								Id = y.Id,
								Code = y.Code,
								Uid = y.Uid,
								VatNumber = y.VatNumber,
								UserName = y.UserName,
								LastName = y.LastName,
								EntranceTypeName = y.EntranceTypeName,
								CheckInStart = ((XpDateTime)y.CheckInStart.ToUTC()) ?? x.EventStart.ToUTC(),
								CheckInEnd = ((XpDateTime)y.CheckInEnd.ToUTC()) ?? x.EventEnd.ToUTC()
							})
					})
					.ToList()
					;
			}
			#endregion Events

			var pages = new List<MobileMainSynchronizeResult_Page>();
			#region Pages
			if (
				// Apps ad-hoc
				(SessionData.ClientId == "") ||
				(SessionData.ClientId == AccountClientId.AndroidVilamarxantNative) ||
				(SessionData.ClientId == AccountClientId.AndroidFinestratNative) ||
				(SessionData.ClientId == AccountClientId.AndroidFallesNative) ||
				(SessionData.ClientId == AccountClientId.FiraValenciaApp)
			)
			{
				var pageDictionary = (await NoticeRepository.GetAsync())
					.Where(x =>
						(x.PaymentConcession.Concession.State == ConcessionState.Active) &&
						(x.PaymentConcessionId == arguments.ConcessionId) &&
						(x.State == NoticeState.Active) &&
						(x.Type == NoticeType.Page) &&
						(x.Start <= now) &&
						(x.End >= now) &&
						(x.IsVisible)
					)
					.Select(x => new
					{
						x.SuperNoticeId,
						x.Id,
						x.PhotoUrl,
						Name = translations
							.Where(y => y.NoticeNameId == x.Id)
							.Select(y => y.Text)
							.FirstOrDefault() ?? x.Name,
						Description = translations
							.Where(y => y.NoticeDescriptionId == x.Id)
							.Select(y => y.Text)
							.FirstOrDefault() ?? x.Description,
						ShortDescription = translations
							.Where(y => y.NoticeShortDescriptionId == x.Id)
							.Select(y => y.Text)
							.FirstOrDefault() ?? x.ShortDescription,
						Place = translations
							.Where(y => y.NoticeShortDescriptionId == x.Id)
							.Select(y => y.Text)
							.FirstOrDefault() ?? x.Place,
						x.Longitude,
						x.Latitude,
						x.Start,
						Poi = new PoiResult
						{
							Name = translations
									.Where(y => y.NoticeNameId == x.Id)
									.Select(z => z.Text)
									.FirstOrDefault() ??
									x.Name,
							Longitude = x.Longitude ?? 0,
							Latitude = x.Latitude ?? 0
						}
					})
					.ToDictionary(
						x => x.Id,
						x => new MobileMainSynchronizeResult_Page
						{
							SuperNoticeId = x.SuperNoticeId,
							Id = x.Id,
							PhotoUrl = x.PhotoUrl,
							Name = x.Name,
							ShortDescription = x.ShortDescription,
							Place = x.Place,
							Description = x.Description,
							Longitude = x.Longitude,
							Latitude = x.Latitude,
							Start = x.Start,
							Pois = new List<PoiResult> { x.Poi }
						}
					);

				// Buscando subnoticias
				foreach (var page in pageDictionary)
					if (page.Value.SuperNoticeId != null)
					{
						// Inheritance
						var parent = pageDictionary[page.Value.SuperNoticeId.Value];
						if (parent != null)
							parent.SubNotices.Add(page.Value);

						// Pois
						// Page tiene todos los pois suyos y de sus hijos
						var temp = parent;
						while (temp != null)
						{
							// Copiar pois de page a temp
							foreach (var poi in page.Value.Pois)
								temp.Pois.Add(poi);

							// Get parent de temp
							temp = temp.SuperNoticeId != null ?
								pageDictionary[temp.SuperNoticeId.Value] :
								null;
						}
					}

				// Nos quedamos con las raices
				pages = pageDictionary.Values
					.Where(x => x.SuperNoticeId == null)
					.ToList();
			}
			#endregion Pages

			#region Products
			var products = (await ProductRepository.GetAsync())
				.Where(x =>
					(x.State == ProductState.Active) &&
                    (x.IsVisible) &&
					(x.SellableInTpv) &&
					(x.PaymentConcession.Concession.State == Common.ConcessionState.Active) &&
					(
						x.PaymentConcession.Concession.Supplier.Login == SessionData.Login ||
						x.PaymentConcession.PaymentWorkers.Any(y => y.Login == SessionData.Login)
					)
				)
				.Select(x => new
				{
					x.Id,
					x.Name,
					x.PhotoUrl,
					x.Price,
					ConcessionId = x.PaymentConcessionId,
					ConcessionName = x.PaymentConcession.Concession.Name,
					CampaignLines = generatedCampaignLines
						.Where(y =>
							(y.Products.Any(z => z == x.Id)) ||
							(y.AllProduct) ||
							(y.Families.Any(z => z == x.FamilyId)) ||
							(y.Families.Any(z => z == x.Family.SuperFamilyId)) ||
							(y.Families.Any(z => z == x.Family.SuperFamily.SuperFamilyId)) ||
							(y.Families.Any(z => z == x.Family.SuperFamily.SuperFamily.SuperFamilyId)) ||
							(y.Families.Any(z => z == x.Family.SuperFamily.SuperFamily.SuperFamily.SuperFamilyId))
						)
						.ToList()
				})
				.ToList()
				.Select(x => new MobileMainSynchronizeResult_Product
				{
					Id = x.Id,
					Name = x.Name,
					PhotoUrl = x.PhotoUrl,
					Price = x.Price,
					ConcessionId = x.ConcessionId,
					ConcessionName = x.ConcessionName,
					CampaignLines = x.CampaignLines
						.Select(y => new MobileMainSynchronizeResult_CampaignLine
						{
							Id = y.Id,
							Max = y.Max,
							Min = y.Min,
							Quantity = y.Quantity,
							Type = y.Type,
							SinceTime = y.SinceTime,
							UntilTime = y.UntilTime,
							CampaignId = y.CampaignId,
							CampaignTitle = y.CampaignTitle,
							CampaignSince = y.CampaignSince.ToUTC(),
							CampaignUntil = y.CampaignUntil.ToUTC(),
							Users = y.Users,
							Groups = y.Groups
						})
				})
				.ToList(); // Se tiene que hacer un tolist para calcular el listado y que el bucle posterior modifique la instancia de la colección ya creada.
			foreach (var product in products)
			{
				product.Groups = categories
					.SelectMany(x => x.Groups
						.Where(y => y.Products.Contains(product.Id))
						.Select(y => new MobileMainSynchronizeResult_ServiceGroupId
						{
							Id = y.Id
						})
					);
			}
			#endregion Products

			#region BlackList
			var blackList = (await BlackListRepository.GetAsync())
                .Where(x =>
                    (x.State == BlackList.BlackListStateType.Active) &&
                    (!x.Resolved) &&
                    (x.SystemCardId == arguments.SystemCardId) //&&
                    //(x.Source == BlackList.BlackListSourceType.PayFalles)
                )
                .Select(x => new
                {
                    x.Id,
                    x.Uid,
                    x.Rejection,
                    x.Resolved,
                    x.ResolutionDate,
                    x.RegistrationDate
                })
                .ToList()
                .Select(x => new MobileMainSynchronizeResult_BlackList
                {
                    Id = x.Id,
                    Uid = x.Uid,
                    Rejection = x.Rejection,
                    Resolved = x.Resolved,
                    ResolutionDate = x.ResolutionDate.ToUTC(),
                    RegistrationDate = x.RegistrationDate.ToUTC()
                });
            #endregion BlackList

            #region GreyList
            var greyList = (await GreyListRepository.GetAsync())
                .Where(x =>
                    (x.State == GreyList.GreyListStateType.Active) &&
                    (x.Source == GreyList.GreyListSourceType.PayFalles) &&
                    (!x.Resolved)
                )
                .Select(x => new
                {
                    x.Id,
                    x.Uid,
                    x.ResolutionDate,
                    Action = (int)x.Action,
                    x.Field,
                    x.NewValue,
                    x.ObjectId
                })
                .ToList()
                .Select(x => new MobileMainSynchronizeResult_GreyList
                {
                    Id = x.Id,
                    CardUid = x.Uid,
                    ResolutionDate = x.ResolutionDate.ToUTC(),
                    Action = (int)x.Action,
                    Field = x.Field,
                    NewValue = x.NewValue,
                    ObjectId = x.ObjectId
                });
            #endregion GreyList

            #region ConfigCard
            List<MobileMainSynchronizeResult_ConfigCard> configCards = new List<MobileMainSynchronizeResult_ConfigCard>();
            if (SessionData.ClientId == AccountClientId.CashlessProApp)
            {
				var wallets = purses
					.Where(x =>
						(x.SystemCardId == arguments.SystemCardId) &&
						(x.Slot != null)
					)
					.Select(x => new MobileMainSynchronizeResult_ConfigWallet
					{
						Id = x.Id,
						Slot = x.Slot.Value,
						ConcessionId = x.ConcessionId,
						Name = x.Name,
						IsPayin = x.IsPayin,
						IsRechargable = x.IsRechargable
					})
					.ToList();
				configCards.Add(new MobileMainSynchronizeResult_ConfigCard
                {
                    EmitPrimary = true,
                    EmitSecondary = true,
                    EmitAnonymous = true,
                    Wallets = wallets
                });
            }
            #endregion ConfigCard

            return new MobileMainSynchronizeResultBase
            {
                ServiceUsers = serviceUsers,
                ServiceCardsNotEmited = serviceCardsNotEmited,
                ServiceCardsEmited = serviceCardsEmited,
                Events = events, // Checkin events
				Pages = pages,
				Products = products,
                Categories = categories
                    .Select(x => new MobileMainSynchronizeResult_ServiceCategory
                    {
                        Id = x.Id,
                        Name = x.Name,
						AllMembersInSomeGroup = x.AllMembersInSomeGroup,
						AMemberInOnlyOneGroup = x.AMemberInOnlyOneGroup,
						AskWhenEmit = x.AskWhenEmit,
						DefaultGroupWhenEmitId = x.DefaultGroupWhenEmitId,
						Groups = x.Groups
							.Select(y => new MobileMainSynchronizeResult_ServiceGroup
							{
								Id = y.Id,
								Name = y.Name
							})
                    }),
                BlackList = blackList,
                GreyList = greyList,
                Errors = errors,
                ConfigCard = configCards.FirstOrDefault(),
                ConfigCards = configCards
            };
		}
		#endregion ExecuteAsync

		#region CreateTicketAsync
		public async Task<Ticket> CreateTicketsAsync(
			int paymentConcessionId, string supplierName, 
            string taxNumber, string taxName, string taxAddress, 
            string reference, DateTime date,
            int? workerId, int? eventId,
            IEnumerable<MobileMainSynchronizeArguments_TicketLine> argumentLines, IEnumerable<MobileMainSynchronizeArguments_Payment> argumentPayments,
            IEnumerable<MobileMainSynchronizeArguments_Form> argumentForms, IEnumerable<MobileMainSynchronizeArguments_Wallet> argumentWallets,
            IEnumerable<Tuple<EntranceType, int>> entranceTypes, IEnumerable<Product> products, IEnumerable<Campaign> campaigns, IEnumerable<ControlForm> forms,
            int? liquidationConcessionId, int? transportPrice, TicketType type, string email, string login, string externalLogin, decimal? amount, DateTime now, List<string> errors)
        {
            var dateSince = now;
            var dateUntil = now.AddHours(6);

			// Get uid & card
			var uid =
				argumentLines
					.Select(x => x.Uid)
					.Where(x => x != null)
					.FirstOrDefault() ??
				argumentPayments
					.Select(x => x.Uid)
					.Where(x => x != null)
					.FirstOrDefault();
			var card = (await ServiceCardRepository.GetAsync(nameof(ServiceCard.OwnerUser) + "." + nameof(ServiceUser.Groups)))
				.Where(x => 
					x.Uid == uid &&
					(
						x.State != ServiceCardState.Deleted &&
						x.State != ServiceCardState.Destroyed &&
						x.State != ServiceCardState.Returned
					)
				)
				.FirstOrDefault();

			var lines = await CreateExtraLinesAsync(paymentConcessionId, argumentLines,
                entranceTypes, products, campaigns,
                login, card, eventId, transportPrice, now
            );

            // Create ticket
            var ticket = new Ticket(
                paymentConcessionId: paymentConcessionId,
                supplierName: supplierName,
                taxNumber: taxNumber,
                taxName: taxName,
                taxAddress: taxAddress,
                date: date,
                amount: amount ?? lines.Sum(x => x.Amount * x.Quantity),
                since: dateSince,
                until: dateUntil,
                type: type,
                paymentWorkerId: workerId,
                
                externalLogin: externalLogin,
                reference: reference,

                liquidationConcessionId: liquidationConcessionId,
                eventId: eventId,

                observations: argumentWallets?.ToJson() ?? ""
            );
            await TicketRepository.AddAsync(ticket);

            // Crear operación
			var operationTypes = lines
                .Select(x => new
                {
                    OperationType =
                        x.Type == TicketLineType.Recharge ? OperationType.Recharge : // Recarga
                        x.Type == TicketLineType.Product ? OperationType.Purchaise : // TPV
                        x.Type == TicketLineType.Buy ? OperationType.Unemit : // Devolver pulsera
                        OperationType.Read,
                    RechargeType =
                        x.Type == TicketLineType.Recharge ? RechargeType.Recharge :
                        //x.Type == TicketLineType.Product ? RechargeType.Purchaise :
                        (RechargeType?)null
                })
                .FirstOrDefault();
            var operation = await CreateOperation(ticket, uid, operationTypes?.OperationType ?? OperationType.Read, operationTypes?.RechargeType, date, date, login, "");

            // Create payments
            if (argumentPayments != null)
                foreach (var item in argumentPayments)
                {
                    var payment = new Payment(
                        ticket,
                        item.Amount, 0, item.Date,
                        externalLogin: externalLogin,
                        uid: item.Uid
                    );
                    ticket.Payments.Add(payment);
                }

            // Create lines
            foreach (var ticketLine in lines)
            {
                ticket.Lines.Add(ticketLine);

                await CreateEntrancesAsync(argumentForms, ticket, ticketLine, entranceTypes, forms, now, email, login);
            }

            await CreateAccountLinesAsync(ticket, paymentConcessionId, errors);

            return ticket;
        }
		#endregion CreateTicketAsync

		#region CreateServiceOperationAsync
		public async Task<ServiceOperation> CreateServiceOperationAsync(ServiceOperation operation, MobileMainSynchronizeArguments_Ticket ticket, ServiceCard serviceCard, Event event_)
		{
			if ((operation == null) || (operation.Seq != ticket.Seq))
			{
				var type = ServiceOperationType.Other;
				if (ticket.Type == TicketType.Recharge)
				{
					if (ticket.Lines.Any(x => x.Amount < 0))
						type = ServiceOperationType.PurseReturned;
					else
						type = ServiceOperationType.PurseRecharged;
				}
				else if (ticket.Type == TicketType.Ticket)
				{
					if (ticket.Lines.Any(x => x.Type == TicketLineType.Product && x.Amount < 0))
						type = ServiceOperationType.ProductReturned;
					else if (ticket.Lines.Any(x => x.Type == TicketLineType.Product))
						type = ServiceOperationType.ProductBought;
					else if (ticket.Lines.Any(x => x.Type == TicketLineType.Entrance && x.Amount < 0))
						type = ServiceOperationType.EntranceReturned;
					else if (ticket.Lines.Any(x => x.Type == TicketLineType.Entrance))
						type = ServiceOperationType.EntranceBought;
					else if (ticket.Lines.Any(x => x.Amount < 0))
						type = ServiceOperationType.TpvReturned;
					else if (ticket.Type == TicketType.Reading)
						type = ServiceOperationType.CardRead;
					else
						type = ServiceOperationType.TpvBought;
				}
				else if(ticket.Type == TicketType.Reading)
					type = ServiceOperationType.CardRead;

				operation = new ServiceOperation(
					ticket.Date,
					type,
					serviceCard,
					seq: ticket.Seq,
					eSeq: ticket.ESeq
				);
				await ServiceOperationRepository.AddAsync(operation);

                if (event_ != null)
                    event_.Operations.Add(operation);
            }

			// En el caso de devoluciones de tarjeta el ticket completa la devolución
			//if (operation.Type == ServiceOperationType.CardReturned)
			{
				foreach (var purseInfo in ticket.Wallets)
				{
					var purse = (await PurseRepository.GetAsync())
						.Where(x =>
							x.State == PurseState.Active &&
							x.SystemCardId == serviceCard.SystemCardId &&
							x.Slot == purseInfo.Slot
						)
						.FirstOrDefault();

					if (purse != null)
					{
						var purseValue = new PurseValue(
							purseInfo.Balance,
							purse,
							operation,
							slot: purseInfo.Slot
						);
						await PurseValueRepository.AddAsync(purseValue);
					}
				}
			}

			return operation;
		}
		public async Task<ServiceOperation> CreateServiceOperationAsync(ServiceOperation operation, MobileMainSynchronizeArguments_ServiceCardEmition emition, ServiceCard serviceCard, Event event_, DateTime now)
		{
			if ((operation == null) || (operation.Seq != emition.LastSeq))
			{
				operation = new ServiceOperation(
					emition.Date ?? now,
					ServiceOperationType.CardEmited,
					serviceCard,
					emition.LastSeq,
					emition.LastSeq
				);
				await ServiceOperationRepository.AddAsync(operation);

                if (event_ != null)
                    event_.Operations.Add(operation);
			}

			return operation;
		}
		public async Task<ServiceOperation> CreateServiceOperationAsync(ServiceOperation operation, MobileMainSynchronizeArguments_ServiceCardReturned returned, ServiceCard serviceCard, Event event_, DateTime now)
		{
			if ((operation == null) || (operation.Seq != returned.Seq))
			{
				operation = new ServiceOperation(
					returned.Date ?? now,
					ServiceOperationType.CardReturned,
					serviceCard,
					returned.Seq,
					returned.ESeq
				);
				await ServiceOperationRepository.AddAsync(operation);

                if (event_ != null)
                    event_.Operations.Add(operation);
            }

			return operation;
		}
		#endregion CreateServiceOperationAsync

		#region CreateOperation
		public async Task<TransportOperation> CreateOperation(Ticket ticket, long? uid, OperationType operationType, RechargeType? rechargeType, DateTime operationDate, DateTime confirmationDate, string login, string device)
		{
			var operation = new TransportOperation(
                ticket,
                uid,
                operationType,
                rechargeType,
                operationDate,
                confirmationDate,
                login,
                device
            );
            await TransportOperationRepository.AddAsync(operation);

            return operation;
        }
        #endregion CreateOperation

        #region CreateExtraLinesAsync
        public async Task<IEnumerable<TicketLine>> CreateExtraLinesAsync(int paymentConcessionId, IEnumerable<MobileMainSynchronizeArguments_TicketLine> argumentLines,
            IEnumerable<Tuple<EntranceType, int>> entranceTypes, IEnumerable<Product> products, IEnumerable<Campaign> campaigns, 
            string login, ServiceCard card, int? eventId, int? transportPriceId, DateTime now)
        {
            return await Task.Run(() =>
            {
                var timeNow = new XpTime(now);

                var lines = new List<TicketLine>();
                if (argumentLines == null)
                    return lines;

                foreach (var argumentLine in argumentLines)
                {
                    if ((argumentLine.ProductId != null) && (argumentLine.EntranceTypeId != null))
                        throw new ApplicationException("Una linea de ticket no puede estar asociada a una entrada y un producto al mismo tiempo");

                    #region Create TicketLine
                    var line = new TicketLine
                    {
                        Title = argumentLine.Title.IsNullOrEmpty() ? TicketResources.Several : argumentLine.Title,
                        Amount = argumentLine.Amount ?? 0,
                        Quantity = argumentLine.Quantity,
                        TransportPriceId = transportPriceId,
                        EntranceTypeId = argumentLine.EntranceTypeId,
                        ProductId = argumentLine.ProductId,
                        Type = argumentLine.Type,
                        Uid = argumentLine.Uid
                    };
                    lines.Add(line);
                    #endregion Create TicketLine

                    if (argumentLine.EntranceTypeId != null)
                    {
                        line.Type = TicketLineType.Entrance;

                        #region Manage Entrance
                        var entranceType = entranceTypes
                            .Where(x =>
                                x.Item1.Id == argumentLine.EntranceTypeId
                            )
                            .FirstOrDefault();
                        if (entranceType == null)
                            throw new ArgumentNullException("Lines.EntranceTypeId");

                        if (
                            (!entranceType.Item1.IsVisible) &&
                            (entranceType.Item1.CampaignLines
                                .Any(x => x.Campaign.TargetSystemCard.Cards
                                    .Any(y => y.Users
                                        .Any(z => z.Login == SessionData.Login)
                                    )
                                )
                            )
                        )
                            throw new Exception("No se puede comprar un tipo de entrada no visible");
                        
                        if (argumentLine.Amount == null)
                        {
                            line.Title = entranceType.Item1.Name;
                            line.Amount = entranceType.Item1.Price;

                            // Extra price
                            if (entranceType.Item1.ExtraPrice != 0)
                                lines.Add(new TicketLine
                                {
                                    Title = "Extra: " + line.Title,
                                    Amount = entranceType.Item1.ExtraPrice,
                                    Quantity = argumentLine.Quantity,
                                    TransportPriceId = transportPriceId,
                                    EntranceTypeId = argumentLine.EntranceTypeId,
                                    ProductId = null,
                                    Type = TicketLineType.ExtraPrice
                                });
                        }
                        #endregion Manage Entrance
                    }

                    if (argumentLine.ProductId != null)
                    {
                        line.Type = TicketLineType.Product;

                        #region Manage Product
                        if (argumentLine.Amount == null)
                        {
                            var product = products
                            .Where(x =>
                                x.Id == argumentLine.ProductId
                            )
                            .FirstOrDefault();
                            if (product == null)
                                throw new ArgumentNullException("Lines.ProductId");

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
                        }
                        #endregion Manage Product
                    }

					#region Assign Discount
					foreach (var campaign in campaigns)
                    {
                        var campaignLines = campaign.CampaignLines
                            .Where(x =>
                                (x.State == CampaignLineState.Active) &&
                                (timeNow.InPeriod(x.SinceTime, x.UntilTime)) &&
                                (
									x.Products.Any(y => y.Id == argumentLine.ProductId) ||
									x.EntranceTypes.Any(y => y.Id == argumentLine.EntranceTypeId)
								) &&
								(
									x.ServiceUsers.Any(y => y.ServiceUserId == card.OwnerUserId) ||
									x.ServiceGroups.Any(y => card.OwnerUser.Groups.Any(z => z.Id == y.ServiceGroupId))
								)
                            )
                            .Select(x => new
                            {
                                x.Id,
                                x.Type,
                                x.Quantity,
                                x.Campaign.Title,
                                x.SinceTime,
                                x.UntilTime
                            })
                            .ToList();
                        //if (campaignLines.Count() > 1)
                        //    throw new ApplicationException("A una linea de ticket solo puede aplicarse una campaña");

                        foreach (var campaignLine in campaignLines)
                        {
                            var discount =
                                campaignLine.Type == CampaignLineType.DirectPrice ? line.Amount - campaignLine.Quantity :
                                campaignLine.Type == CampaignLineType.DirectDiscount ? campaignLine.Quantity :
                                campaignLine.Type == CampaignLineType.DirectPercentDiscount ? line.Amount * campaignLine.Quantity / 100 :
                                0M;

							if (discount != 0)
							{
								//lines.Add(new TicketLine
								//{
								//    Title = campaignLine.Title,
								//    Amount = -discount,
								//    Quantity = argumentLine.Quantity,
								//    TransportPriceId = transportPriceId,
								//    EntranceTypeId = argumentLine.EntranceTypeId,
								//    ProductId = argumentLine.ProductId,
								//    Type = TicketLineType.Discount,
								//    CampaignLineId = campaignLine.Id,
								//    CampaignLineType = campaignLine.Type,
								//    CampaignLineQuantity = campaignLine.Quantity
								//});
								line.Amount = line.Amount - discount;
								if (line.Amount < 0)
									line.Amount = 0;
							}
						}
                    }
                    #endregion Assign Discount
                }

                return lines;
            });
        }
        #endregion CreateExtraLinesAsync

        #region CreateEntrancesAsync
        public async Task CreateEntrancesAsync(
            IEnumerable<MobileMainSynchronizeArguments_Form> argumentForms,
            Ticket ticket, TicketLine line, IEnumerable<Tuple<EntranceType, int>> entranceTypes, IEnumerable<ControlForm> forms,
            DateTime now, string email, string login
        )
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
            var entranceType = entranceTypes
                .Where(x =>
                    x.Item1.Id == line.EntranceTypeId
                )
                .FirstOrDefault();
            if (entranceType == null)
                throw new ArgumentNullException("Lines.EntranceTypeId");
            if ((entranceType.Item1.SellStart > now) || (entranceType.Item1.SellEnd < now))
                throw new ApplicationException(EntranceResources.OutOfBuyablePeriodException.FormatString(entranceType.Item1.Name));

            // Check entrance count
            if (entranceType.Item2 + line.Quantity > entranceType.Item1.MaxEntrance)
                // You can't take {0} '{1}' entrances.You exceed maximum number of available entrances. You can only buy {2} entrances of this type.
                throw new ApplicationException(EntranceResources.MaxEntrancesExceedException.FormatString(
                    entranceType.Item2,
                    entranceType.Item1.Name,
                    entranceType.Item1.MaxEntrance
                ));

            // Crear form responses
            var values = new List<ControlFormValue>();
            if (!argumentForms.IsNullOrEmpty())
            {
                foreach(var argumentForm in argumentForms)
                {
                    var form = forms
                        .Where(x => x.Id == argumentForm.Id)
                        .FirstOrDefault();

                    foreach (var argumentFormArgument in argumentForm.Arguments)
                    {
                        var formArgument = form.Arguments
                            .Where(x => x.Id == argumentFormArgument.Id)
                            .FirstOrDefault();

                        var value = new ControlFormValue
                        {
                            ArgumentId = argumentFormArgument.Id,
                            Observations = "",
                            Target = ControlFormArgumentTarget.BuyEntrance,
                            ValueNumeric = argumentFormArgument.ValueNumeric,
                            ValueBool = argumentFormArgument.ValueBool,
                            ValueDateTime = argumentFormArgument.ValueDateTime,
                            ValueString = argumentFormArgument.ValueString
                        };
                        values.Add(value);

                        foreach (var argumentFormOption in formArgument.Options)
                        {
                            var formOption = formArgument.Options
                                .Where(x => x.Id == argumentFormOption.Id)
                                .FirstOrDefault();

                            formOption.Values.Add(value);
                        }
                    }
                }
            }

            // Crear entrances
            for (int i = 0; i < line.Quantity; i++)
            {
                var code = await GenerateCodeAsync(ticket, entranceType.Item1, i);

                var entrance = new Entrance(
                    code: code,
                    state: ticket.Amount == 0 ? EntranceState.Active : EntranceState.Pending,
                    sendingCount: 0,
                    now: now,
                    vatNumber: SessionData.TaxNumber,
                    userName: SessionData.TaxName,
                    lastName: "",
					uid: line.Uid,
                    email: email,
                    login: login,
                    entranceType: entranceType.Item1,
					ticketLine: line,
                    formValues: values
                        .Select(x => new EntranceFormValue { FormValueId = x.Id })
                        .ToList()
                );
                line.Entrances.Add(entrance);
            }
        }
        #endregion CreateEntrancesAsync

        #region CreateAccountLines
        /// <summary>
        /// Crear AccountLines
        /// </summary>
        /// <param name="ticket">Necesita: Lines, Payments</param>
        /// <param name="paymentConcessionId"></param>
        /// <returns></returns>
        public async Task CreateAccountLinesAsync(Ticket ticket, int paymentConcessionId, List<string> errors)
        {
            var serviceCardRepository = (await ServiceCardRepository.GetAsync())
                .Where(x =>
                    (x.State == ServiceCardState.Active) ||
                    (x.State == ServiceCardState.Emited)
                 );

            var linesPerType = ticket.Lines
                .GroupBy(x => x.Type);
            foreach (var linePerType in linesPerType)
            {
                if (linePerType.Key == TicketLineType.Recharge)
                {
                    var linesPerUid = ticket.Lines
                        .Where(x => x.Type == TicketLineType.Recharge)
                        .Select(x => new
                        {
                            x.Uid,
                            Amount = x.Quantity * x.Amount,
                            UidFormat = serviceCardRepository
                                .Where(y => y.Uid == x.Uid)
                                .Select(y => (UidFormat?)y.ServiceCardBatch.UidFormat)
                                .FirstOrDefault()
                        })
                        .GroupBy(x => new { x.Uid, x.UidFormat });
                    
                    foreach (var linePerUid in linesPerUid)
                    {
                        var concessionOwnerIds = (await PaymentConcessionRepository.GetAsync())
                           .Where(x =>
                               serviceCardRepository
                                   .Any(y =>
                                       y.Uid == linePerUid.Key.Uid &&
                                       y.SystemCard.ConcessionOwnerId == x.ConcessionId &&
                                       y.SystemCard.SystemCardMembers
                                           .Any(z =>
                                               z.Login == SessionData.Login
                                           )
                                   )
                           )
                           .Select(x => (int?)x.Id)
                           .ToList();
                        if (concessionOwnerIds.Count() == 0)
                            errors.Add
                            //throw new ApplicationException
                            (
                                "Esta tarjeta no está añadida ningún sistema de tarjetas"
                            );
                        if (concessionOwnerIds.Count() > 1)
                            errors.Add
                            //throw new ApplicationException
                            (
                                "Esta tarjeta está añadida a varios sistemas de tarjetas: {0}"
                                    .FormatString(
                                        concessionOwnerIds
                                            .Select(x => x.ToString())
                                            .JoinString(",")
                                    )
                            );
                        var concessionOwnerId = concessionOwnerIds
                            .FirstOrDefault();

                        var accountLine = AccountLine.CreateServiceCard(
                            "Recarga",
                            -1 * linePerUid.Sum(x => x.Amount),
                            concessionOwnerId ?? paymentConcessionId,
                            linePerUid.Key.Uid.Value,
                            linePerUid.Key.UidFormat ?? UidFormat.Numeric

                        );
                        ticket.AccountLines.Add(accountLine);
                    }
                }
                else if (linePerType.Key == TicketLineType.ReturnCard)
                {
                    var linesPerUid = ticket.Lines
                        .Where(x => x.Type == TicketLineType.ReturnCard)
                        .Select(x => new
                        {
                            x.Uid,
                            Amount = x.Quantity * x.Amount,
                            UidFormat = serviceCardRepository
                                .Where(y => y.Uid == x.Uid)
                                .Select(y => (UidFormat?)y.ServiceCardBatch.UidFormat)
                                .FirstOrDefault()
                        })
                        .GroupBy(x => new { x.Uid, x.UidFormat });
                    foreach (var linePerUid in linesPerUid)
                    {
                        var concessionOwnerIds = (await PaymentConcessionRepository.GetAsync())
                           .Where(x =>
                               serviceCardRepository
                                   .Any(y =>
                                       y.Uid == linePerUid.Key.Uid &&
                                       y.SystemCard.ConcessionOwnerId == x.ConcessionId &&
                                       y.SystemCard.SystemCardMembers
                                           .Any(z =>
                                               z.Login == SessionData.Login
                                           )
                                   )
                           )
                           .Select(x => (int?)x.Id)
                           .ToList();
                        if (concessionOwnerIds.Count() == 0)
                            errors.Add
                            //throw new ApplicationException
                            (
                                "Esta tarjeta no está añadida a ningún sistema de tarjetas"
                                    .FormatString(
                                        concessionOwnerIds
                                            .Select(x => x.ToString())
                                            .JoinString(",")
                                    )
                            );
                        if (concessionOwnerIds.Count() > 1)
                            errors.Add
                            //throw new ApplicationException
                            (
                                "Esta tarjeta está añadida a varios sistemas de tarjetas: {0}"
                                    .FormatString(
                                        concessionOwnerIds
                                            .Select(x => x.ToString())
                                            .JoinString(",")
                                    )
                            );
                        var concessionOwnerId = concessionOwnerIds
                            .FirstOrDefault();
                        
                        var accountLine = AccountLine.CreateServiceCard(
                            "Devolución tarjeta",
                            -1 * linePerUid.Sum(x => x.Amount),
                            concessionOwnerId ?? paymentConcessionId,
                            linePerUid.Key.Uid.Value,
                            linePerUid.Key.UidFormat ?? UidFormat.Numeric
                        );
                        ticket.AccountLines.Add(accountLine);
                    }
                }
                else if (linePerType.Key == TicketLineType.Buy)
                {
                    // Compra Productos
                    var accountLine = AccountLine.CreateProducts(
                        "Compra",
                        -1 * linePerType.Sum(x => x.Quantity * x.Amount),
                        paymentConcessionId
                    );
                    ticket.AccountLines.Add(accountLine);
                }
                else if (linePerType.Key == TicketLineType.Product)
                {
                    // Compra Productos
                    var accountLine = AccountLine.CreateProducts(
                        "Productos",
                        -1 * linePerType.Sum(x => x.Quantity * x.Amount),
                        paymentConcessionId
                    );
                    ticket.AccountLines.Add(accountLine);
                }
                else if (linePerType.Key == TicketLineType.Entrance)
                {
                    // Compra Entradas
                    var accountLine = AccountLine.CreateEntrances(
                        "Entradas",
                        -1 * linePerType.Sum(x => x.Quantity * x.Amount),
                        paymentConcessionId
                    );
                    ticket.AccountLines.Add(accountLine);
                }
                else
                {
                    // Compra Otros
                    var accountLine = AccountLine.Create(
                        "Otros: " + linePerType.Select(x => x.Title).FirstOrDefault(),
                        -1 * linePerType.Sum(x => x.Quantity * x.Amount),
                        paymentConcessionId,
                        null,
                        null
                    );
                    ticket.AccountLines.Add(accountLine);
                }
            }

            // Create accountLines from payments
            var paymentsGrouped = ticket.Payments
                .GroupBy(x => new { x.Uid, x.UidFormat });
            foreach (var paymentGrouped in paymentsGrouped)
            {
                if (paymentGrouped.Key.Uid != null)
                {
                    var paymentGroupedAmount = paymentGrouped.Sum(x => (decimal?)x.Amount) ?? 0;

                    var concessionOwnerIds = (await PaymentConcessionRepository.GetAsync())
                        .Where(x =>
                            serviceCardRepository
                                .Any(y =>
                                    y.Uid == paymentGrouped.Key.Uid &&
                                    y.SystemCard.ConcessionOwnerId == x.ConcessionId &&
                                    y.SystemCard.SystemCardMembers
                                        .Any(z =>
                                            z.Login == SessionData.Login
                                        )
                                )
                        )
                        .Select(x => (int?)x.Id)
                        .ToList();
                    if (concessionOwnerIds.Count() == 0)
                        errors.Add
                         //throw new ApplicationException
                        (
                            "Esta tarjeta no está añadida a ningún sistema de tarjetas"
                        );
                    if (concessionOwnerIds.Count() > 1)
                        errors.Add
                        //throw new ApplicationException
                        (
                            "Esta tarjeta está añadida a varios sistemas de tarjetas: {0}"
                                .FormatString(
                                    concessionOwnerIds
                                        .Select(x => x.ToString())
                                        .JoinString(",")
                                )
                        );
                    var concessionOwnerId = concessionOwnerIds
                        .FirstOrDefault();

                    var accountLine = AccountLine.CreateServiceCard(
                        paymentGroupedAmount > 0 ?
                            "Pago con tarjeta" :
                            "Ingreso en tarjeta",
                        paymentGroupedAmount,
                        concessionOwnerId ?? paymentConcessionId,
                        paymentGrouped.Key.Uid.Value,
                        paymentGrouped.Key.UidFormat ?? UidFormat.Numeric
                    );
                    ticket.AccountLines.Add(accountLine);
                }
                else
                {
                    var accountLine = AccountLine.CreateCash(
                        paymentGrouped.Sum(x => (decimal?)x.Amount) > 0 ?
                            "Ingreso metálico" :
                            "Abono metálico",
                        paymentGrouped.Sum(x => (decimal?)x.Amount) ?? 0,
                        paymentConcessionId
                    );
                    ticket.AccountLines.Add(accountLine);
                }
            }
        }
        #endregion CreateAccountLines

        #region GenerateCodeAsync
        private static Random RandomGenerator = new Random();
        public async Task<long> GenerateCodeAsync(Ticket ticket, EntranceType entranceType, int countEntrances)
        {
            /*if (entranceType.RangeMin != null)
            {
                // Rango
                var previousCode = 0; (await EntranceRepository.GetAsync())
                     .Where(x =>
                         x.EntranceTypeId == entranceType.Id &&
                         x.State == EntranceState.Active
                     )
                     .Select(x => (long?)x.Code)
                     .Max() ?? (entranceType.RangeMin.Value - 1);
                if (previousCode + 1 + countEntrances > entranceType.RangeMax)
                    throw new ApplicationException("Rango completo!!!");

                return previousCode + 1 + countEntrances;
            }*/

            // Aleatorio
            var code = RandomGenerator.Next(0, int.MaxValue);
            while (
                (
                    ticket.Lines
                        .Any(y => y.Entrances
                            .Any(x => x.Code == code)
                        )
                ) || (
                    (await EntranceRepository.GetAsync())
                        .Any(x =>
                            x.EntranceTypeId == x.EntranceType.Id &&
                            x.Code == code
                        )
                )
            )
            {
                code = RandomGenerator.Next(0, int.MaxValue);
            }

            return code;
        }
        #endregion GenerateCodeAsync

        #region AddEmisionAsync
        public async Task<ServiceOperation> AddEmisionAsync(ServiceOperation operation, MobileMainSynchronizeArguments_ServiceCardEmition serviceCardEmition, int? systemCardId, List<string> errors, DateTime now)
        {
            var serviceCard = (await ServiceCardRepository.GetAsync(serviceCardEmition.Id ?? 0, "Groups"));
            if (serviceCard == null)
                serviceCard = (await ServiceCardRepository.GetAsync("Groups"))
                    .Where(x =>
                        x.SystemCardId == systemCardId &&
                        x.Uid == serviceCardEmition.CardUid
                    )
                    .FirstOrDefault();

            if (serviceCard == null)
                errors.Add($"Emitir tarjeta : No se puede emitir la tarjeta {serviceCardEmition.CardUid} - porque no existe");
            else
            {
                if (serviceCard.State == ServiceCardState.Destroyed)
                    errors.Add($"Emitir tarjeta: No se puede emitira la tarjeta {serviceCardEmition.CardUid} porque está bloqueada");
                if (serviceCard.State == ServiceCardState.Emited)
                    errors.Add($"Emitir tarjeta: No se puede emitira la tarjeta {serviceCardEmition.CardUid} porque ya está emitida");

                // Data
                serviceCard.State = ServiceCardState.Emited;
                serviceCard.Alias = serviceCardEmition.Alias;
                serviceCard.OwnerUserId = serviceCardEmition.OwnerUserId;

                // Personalized
                if (serviceCardEmition.OwnerUserId != null)
                {
                    var serviceUser = (await ServiceUserRepository.GetAsync(serviceCardEmition.OwnerUserId.Value));
                    if (serviceUser == null)
                        errors.Add($"Emitir tarjeta: No se puede emitira la tarjeta {serviceCardEmition.CardUid} porque el usuario {serviceCardEmition.OwnerUserId} no existe");
                    if (serviceUser.State != ServiceUserState.Active)
                        errors.Add($"Emitir tarjeta: No se puede emitira la tarjeta {serviceCardEmition.CardUid} porque el usuario {serviceCardEmition.OwnerUserId} no existe");

                    if (serviceCardEmition.IsPrincipal)
                        // Principal
                        serviceUser.Card = serviceCard;
                    else
                        // Secondary
                        serviceUser.Card = null;
                }

                // Groups
                if (serviceCardEmition.Groups != null)
                {
                    var servicegroupIds = serviceCardEmition.Groups
                        .Select(x => x.Id);
                    var servicegroups = (await ServiceGroupRepository.GetAsync())
                        .Where(x => servicegroupIds.Contains(x.Id));
                    serviceCard.Groups.Clear();
                    foreach (var group in servicegroups)
                        serviceCard.Groups.Add(group);
                }

                // Event
                var event_ = (serviceCardEmition.EventId != null) ?
                    await EventRepository.GetAsync(serviceCardEmition.EventId.Value) :
                    null;

                // Operation
                operation = await CreateServiceOperationAsync(
                    operation,
                    serviceCardEmition,
                    serviceCard,
                    event_,
                    now
                );

                var greyListNotResolved = (await GreyListRepository.GetAsync())
                    .Where(x =>
                        (x.State == GreyList.GreyListStateType.Active) &&
                        (!x.Resolved) &&
                        (x.Uid == serviceCardEmition.CardUid)
                    );
                foreach (var item in greyListNotResolved)
                    item.Resolved = true;
            }

            return operation;
        }
        #endregion AddEmisionAsync

        #region AddReturnAsync
        public async Task<ServiceOperation> AddReturnAsync(ServiceOperation operation, MobileMainSynchronizeArguments_ServiceCardReturned serviceCardReturned, int? systemCardId, List<string> errors, DateTime now)
        {
            var serviceCard = (await ServiceCardRepository.GetAsync(serviceCardReturned.Id ?? 0, "Groups"));
            if (serviceCard == null)
                serviceCard = (await ServiceCardRepository.GetAsync("Groups"))
                    .Where(x =>
                        x.SystemCardId == systemCardId &&
                        x.Uid == serviceCardReturned.CardUid
                    )
                    .FirstOrDefault();
            if (serviceCard == null)
                errors.Add($"Devolver tarjeta : No se puede devolver la tarjeta {serviceCardReturned.CardUid} - porque no existe");
            else
            {
                if (serviceCard.State == ServiceCardState.Destroyed)
                    errors.Add($"Devolver tarjeta: No se puede devolver la tarjeta {serviceCardReturned.CardUid} porque está bloqueada");
                if (serviceCard.State == ServiceCardState.Active)
                    errors.Add($"Devolver tarjeta: No se puede devolver la tarjeta {serviceCardReturned.CardUid} porque no está emitida");

                // Data
                serviceCard.State = ServiceCardState.Active;
                serviceCard.Alias = ""; // serviceCardReturned.Alias;
                serviceCard.OwnerUserId = null; // serviceCardReturned.OwnerUserId;
				serviceCard.Groups.Clear();

                // Event
                var event_ = (serviceCardReturned.EventId != null) ?
                    await EventRepository.GetAsync(serviceCardReturned.EventId.Value) :
                    null;

                // Operation
                operation = await CreateServiceOperationAsync(
                    operation,
                    serviceCardReturned,
                    serviceCard,
                    event_,
                    now
                );

                var greyListNotResolved = (await GreyListRepository.GetAsync())
                    .Where(x =>
                        (x.State == GreyList.GreyListStateType.Active) &&
                        (!x.Resolved) &&
                        (x.Uid == serviceCardReturned.CardUid)
                    );
                foreach (var item in greyListNotResolved)
                    item.Resolved = true;
            }

            return operation;
        }
		#endregion AddReturnAsync

		#region AddTicketAsync
		public async Task<ServiceOperation> AddTicketAsync(ServiceOperation operation, MobileMainSynchronizeArguments_Ticket ticketArg, List<Campaign> campaigns, List<ControlForm> formsInTickets, object paymentConcession, List<Tuple<EntranceType, int>> entranceTypesInTickets, List<Product> productsInTickets, int? systemCardId, List<string> errors, DateTime now)
		{
			return null;
		}
		#endregion AddTicketAsync

		#region AddRead
		private async Task<ServiceOperation> AddRead(MobileMainSynchronizeArguments_Ticket ticketArguments, int? systemCardId)
		{
			var uid = ticketArguments.Uid ?? ticketArguments.Lines
				.Select(x => x.Uid)
				.FirstOrDefault();

			var serviceCard = (await ServiceCardRepository.GetAsync())
				.Where(x =>
					x.SystemCardId == systemCardId &&
					x.Uid == uid
				)
				.FirstOrDefault();

			var amount = ticketArguments.Wallets
				.Sum(x => x.Balance);
			var purseValues = (await PurseValueRepository.GetAsync());
			var operation = (await ServiceOperationRepository.GetAsync())
				.Where(x =>
					(x.Card.Uid == uid) &&
					(x.Seq == ticketArguments.Seq) &&
					(amount == purseValues
						.Where(y => y.ServiceOperationId == x.Id)
						.Sum(y => y.Amount)
					)
				)
				.FirstOrDefault();
			if (operation != null)
				return operation;

			// Operation
			operation = await CreateServiceOperationAsync(
				operation,
				ticketArguments,
				serviceCard,
				null
			);
			await ServiceOperationRepository.AddAsync(operation);

			return operation;
		}
		#endregion AddRead
	}
}