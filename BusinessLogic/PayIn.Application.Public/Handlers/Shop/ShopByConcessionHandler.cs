using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Arguments.Shop;
using PayIn.Application.Dto.Results;
using PayIn.Application.Dto.Results.Shop;
using PayIn.Application.Public.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handler.Shop
{
	[XpLog("Shop", "ByConcession")] 
	public class ShopByConcessionHandler :
        IQueryBaseHandler<ShopByConcessionArguments, ShopByConcessionResult>
    {
		[Dependency] public IEntityRepository<Event> Repository { get; set; }
		[Dependency] public IEntityRepository<PurseValue> PurseValueRepository { get; set; }
		[Dependency] public IEntityRepository<Product> ProductRepository { get; set; }
		[Dependency] public IEntityRepository<Entrance> EntranceRepository { get; set; }
		[Dependency] public IEntityRepository<EntranceType> EntranceTypeRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceUser> ServiceUserRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<Ticket> TicketRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceOperation> ServiceOperationRepository { get; set; }
		[Dependency] public SecurityRepository SecurityRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public MobileServiceCardGetAllHandler MobileServiceCardGetAllHandler { get; set; }
        [Dependency] public IEntityRepository<Purse> PurseRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceDocument> ServiceDocumentRepository { get; set; }
		[Dependency] public IEntityRepository<SystemCard> SystemCardRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ShopByConcessionResult>> ExecuteAsync(ShopByConcessionArguments arguments)
        {
			var times = new StringBuilder();
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			var now = DateTime.UtcNow;

            var purseId = (await PurseRepository.GetAsync())
                .Where(y =>
                    y.ConcessionId == arguments.PaymentConcessionId &&
                    y.IsRechargable &&
                    !y.IsPayin
                )
                .Select(y => y.Id)
                .FirstOrDefault();
            if (purseId <= 0)
                throw new ApplicationException("Monedero inválido");

			var mobileServiceCards = (await MobileServiceCardGetAllHandler.ExecuteAsync(new MobileServiceCardGetAllArguments(""))).Data;
			var selectedServiceCard = mobileServiceCards
				.Where(x => x.Id == arguments.ServiceCardId)
				.FirstOrDefault();
			if (selectedServiceCard == null)
			{
				selectedServiceCard = mobileServiceCards
					.Where(x => x.Type == MobileServiceCardGetAllResult.ResultType.Principal)
					.FirstOrDefault();
			}
			if (selectedServiceCard == null)
			{
				selectedServiceCard = mobileServiceCards
					.FirstOrDefault();
			}

            var linkedCardSelected = selectedServiceCard.Relation == MobileServiceCardGetAllResult.RelationType.Linked;
			var entrances = new List<ShopByConcessionResultBase.Entrance>();
			var tickets = new List<ShopByConcessionResultBase.Ticket>();
			var serviceOperations = new List<ServiceOperation>();
			var events = new List<ShopByConcessionResult>();
			var products = new List<ShopByConcessionResultBase.Product>();
			var serviceDocuments = new List<ShopByConcessionResultBase.ServiceDocument>();

			times.AppendFormat("ShopGetEntranceTypesByConcession Inicializando: {0}\n", stopwatch.Elapsed);

			var ownerUser = "";
			var cardUidText = "";
;
			//var balance = 0m;
			bool inBlackList = false;
			if (selectedServiceCard != null)
			{
				// InBlackList
				inBlackList = selectedServiceCard.InBlackList;

				// Name
				ownerUser = selectedServiceCard.Name + " " + selectedServiceCard.LastName;
				// Uid
				var uid = selectedServiceCard.Uid;
				cardUidText = selectedServiceCard.UidText;
				if (selectedServiceCard.Alias != "")
					cardUidText += " (" + selectedServiceCard.Alias + ")";
				
				times.AppendFormat("ShopGetEntranceTypesByConcession Buscando UID: {0}\n", stopwatch.Elapsed);

				entrances = (await EntranceRepository.GetAsync())
					.Where(x =>
						x.Uid == uid &&
						x.State == EntranceState.Active &&
						x.Event.EventEnd > now &&
						x.TicketLine.Ticket.State == TicketStateType.Active
					)
					.GroupBy(x => new
					{
						x.EntranceTypeId
					})
					.Select(x => new ShopByConcessionResultBase.Entrance
					{
						Quantity = x.Count(),
						EventId = x.FirstOrDefault().EventId,
						EventName = x.FirstOrDefault().Event.Name,
						EntranceTypeName = x.FirstOrDefault().EntranceType.Name
					})
					.ToList(); // XAVI

				times.AppendFormat("ShopGetEntranceTypesByConcession Entradas: {0}\n", stopwatch.Elapsed);

				var maxESeq = (await ServiceOperationRepository.GetAsync())
					.Where(x =>
						x.Card.Uid == uid
					)
					.Max(x => x.ESeq) ?? 0;
				
				var tck = (await TicketRepository.GetAsync())
					.Where(x =>
						x.Type != TicketType.Reading &&
						x.ServiceOperations
						.Any(y =>
							y.Card.Uid == uid &&
							y.Seq >= maxESeq
						//serviceOperationIds.Contains(y.Id)
						)
					//x.PaymentUser.Login == SessionData.Login // XAVI
					)
					.OrderByDescending(x => x.Date)
					.Take(30)
					.ToList();

				tickets = tck
					.Select(x => new ShopByConcessionResultBase.Ticket
					{
						Id = x.Id,
						DateTime = x.Date.ToUTC(),
						Type = x.Type,
						TypeName = (x.Type == TicketType.NotPayable ? "No pagadero" :
									x.Type == TicketType.Order ? "Pedido" :
									x.Type == TicketType.Recharge ? "Recarga" :
									x.Type == TicketType.Shipment ? "Transporte" :
									x.Type == TicketType.Ticket ? "Compra" :
									// Lectura no puede llegar
									""),
						Amount = x.Amount,
						State = x.State,
						StateName = (x.State == TicketStateType.Active ? "Pagado" :
									 x.State == TicketStateType.Cancelled ? "Cancelado" :
									 x.State == TicketStateType.TimedOut ? "Tiempo exc." :
									 x.State == TicketStateType.Preactive ? "Prepagado" :
									 x.State == TicketStateType.Pending ? "Pendiente" :
									 x.State == TicketStateType.Prepared ? "Preparado" :
									 x.State == TicketStateType.Created ? "Creado" :
									 ""),
						EntranceTypes = x.Lines
							.Where(y => y.EntranceType != null)
							.Select(y => new ShopByConcessionResultBase.EntranceType
							{
								EntranceTypeName = y.EntranceType.Name,
								EventName = y.EntranceType.Event.Name,
								Quantity = y.Quantity
							}),
                        ProductLines = x.Lines
                            .Where(y => y.Type == TicketLineType.Product)
                            .Select(y => new ShopByConcessionResultBase.ProductLine
                            {
                                ProductName = y.Title ?? "(producto sin nombre)",
                                Price = y.Amount,
                                Quantity = y.Quantity
                            })
                    })
					.ToList(); // XAVI

				times.AppendFormat("ShopGetEntranceTypesByConcession Tickets: {0}\n", stopwatch.Elapsed);

				var entranceTypes = await EntranceTypeRepository.GetAsync();
				var users = await ServiceUserRepository.GetAsync();
				events = (await Repository.GetAsync())
					.Where(x =>
						x.State == EventState.Active &&
						x.PaymentConcessionId == arguments.PaymentConcessionId &&
						x.PaymentConcession.Concession.State == ConcessionState.Active &&
						x.EntranceTypes.Any() &&
						x.EventEnd > now &&
						x.IsVisible &&
						(
							x.Visibility == EventVisibility.Public ||
							x.Visibility == EventVisibility.Members
						)
					)
					.OrderBy(x => new
					{
						x.EventStart,
						x.Name
					})
					.Select(x => new
					{
						x.Id,
						EventName = x.Name,
						EventDescription = x.Description,
						EventShortDescription = x.ShortDescription,
						EventConditions = x.Conditions,
						x.PhotoUrl,
						x.PhotoMenuUrl,
						x.Place,
						Date = x.EventStart,
						ConcessionId = x.PaymentConcessionId,
						ConcessionName = x.PaymentConcession.Concession.Name,
						ConcessionPhotoUrl = users
							.Where(y => y.Login == x.PaymentConcession.Concession.Supplier.Login)
							.Select(y => y.Photo)
							.FirstOrDefault(),
						EntranceTypes = entranceTypes
							.Where(y =>
								(y.EventId == x.Id) &&
								(y.State == EntranceTypeState.Active) &&
								(y.SellStart < now) &&
								(y.SellEnd > now) &&
								(
									(y.Visibility == EntranceTypeVisibility.Public) ||
									(y.Visibility == EntranceTypeVisibility.Internal) ||
									(
										(y.Visibility == EntranceTypeVisibility.Members) &&
										(x.PaymentConcession.Concession.ServiceUsers.Any(z => z.Login == SessionData.Login))
									)
								)
							)
							.OrderBy(y => y.Name)
							.Select(y => new ShopEventGetEntranceTypesResult
							{
								Id = y.Id,
								EntranceName = y.Name,
								//EntranceConditions = y.EntranceConditions,
								EntrancePrice = y.Price,
								EntranceExtraPrice = y.ExtraPrice,
								Quantity = 0,
								EntranceSoldByType = y.Event.Entrances // ¿La operación lenta?
										.Where(z =>
											z.Uid == uid &&
											z.TicketLine.Ticket.State == TicketStateType.Active &&
											z.TicketLine.EntranceType.Id == y.Id)
										.Count(),
								EntranceMaxEntrancesPerCard = (y.MaxEntrance > x.MaxEntrancesPerCard ? x.MaxEntrancesPerCard : y.MaxEntrance)
							})
							.ToList()
					})
					.ToList()// Tarda 4 segundos
					.Where(x => x.EntranceTypes.Count() > 0)
					.Select(x => new ShopByConcessionResult
					{
						EventId = x.Id,
						EventName = x.EventName,
						EventDescription = x.EventDescription,
						EventShortDescription = x.EventShortDescription,
						EventConditions = x.EventConditions,
                        ShowConditions = false,
						EventPhotoUrl = x.PhotoUrl,
						EventPlace = x.Place,
						EventDate = x.Date == XpDateTime.MinValue ?
							(DateTime?)null :
							x.Date.ToUTC(),
						EntranceTypes = x.EntranceTypes
					})
					.ToList(); // XAVI

				times.AppendFormat("ShopGetEntranceTypesByConcession Tipos de entradas: {0}\n", stopwatch.Elapsed);

				products = (await ProductRepository.GetAsync())
					.OrderBy(x => x.Name)
					.Where(x =>
						x.State == ProductState.Active &&
						x.Visibility == ProductVisibility.Public &&
						x.IsVisible &&
						x.SellableInWeb &&
						x.PaymentConcessionId == arguments.PaymentConcessionId &&
						x.PaymentConcession.Concession.State == ConcessionState.Active
					)
					.Select(x => new ShopByConcessionResultBase.Product
					{
						Quantity = 0,
						Price = x.Price ?? 0,
						ProductId = x.Id,
						PhotoUrl = x.PhotoUrl,
						ProductName = x.Name
					})
					.ToList(); // XAVI

				times.AppendFormat("ShopGetEntranceTypesByConcession Productos: {0}\n", stopwatch.Elapsed);
			}

			var entrancesList = entrances.ToList(); // Al hacerlo, va bien. Antes de hacerlo, a veces

			var supplier = (await PaymentConcessionRepository.GetAsync())
				.Where(x => x.Id == arguments.PaymentConcessionId)
				.Select(x => new
				{
					x.ConcessionId,
					x.Concession.Supplier.Login,
					x.Concession.Name,
					PhotoUrl = x.PhotoUrl == "" ? "/app/ticket.png" : x.PhotoUrl
				})
				.FirstOrDefault();
			if (supplier == null)
				throw new ApplicationException("Compañia suministradora no especificada o inválida");

			var systemCardId = (await SystemCardRepository.GetAsync())
				.Where(x => x.ConcessionOwnerId == supplier.ConcessionId)
				.Select(x => x.Id)
				.FirstOrDefault();

			serviceDocuments = (await ServiceDocumentRepository.GetAsync())
				.Where(x =>
					x.SystemCardId == systemCardId && 
					x.State == ServiceDocumentState.Active &&
					x.IsVisible == true &&
					(x.Visibility == ServiceDocumentVisibility.Members || x.Visibility == ServiceDocumentVisibility.Public) &&
					(now >= x.Since) && (now <= x.Until)
				)
				.Select(x => new
				{
					x.Name,
					x.Url
				})
				.ToList()
				.Select(x => new ShopByConcessionResultBase.ServiceDocument
				{
					ServiceDocumentName = x.Name,
					Url = x.Url
				})
				.ToList();

			times.AppendFormat("ShopGetEntranceTypesByConcession Docuemnts: {0}\n", stopwatch.Elapsed);

			var userSecurity = await SecurityRepository.GetUserAsync(supplier.Login); // Tarda 0.4 segundos
			var logoUrl = userSecurity.PhotoUrl + (userSecurity.PhotoUrl == "" ? "" : "?now=" + DateTime.Now.ToString());
			
			times.AppendFormat("ShopGetEntranceTypesByConcession Provedor: {0}\n", stopwatch.Elapsed);

			var rst = new ShopByConcessionResultBase
			{
				Data = events,
				ConcessionName = supplier.Name,
				ConcessionPhotoUrl = supplier.PhotoUrl, // Poner aquí la que venga del campo que debe crear Xavi
				ConcessionLogoUrl = logoUrl,
				ServiceCards = mobileServiceCards
					.Select(x => new SelectorResult
					{
						Id = x.Id,
						Value = (x.Relation== MobileServiceCardGetAllResult.RelationType.Linked ? "" : "") + x.UidText + " - " + x.Alias + " (" + x.Name + " " + x.LastName + ")",
					}),
				ServiceCardId = selectedServiceCard?.Id ?? 0,
				ServiceCardName = selectedServiceCard?.UidText + " - " + selectedServiceCard?.Alias + " (" + selectedServiceCard?.Name + " " + selectedServiceCard?.LastName + ")",
                IsLinkedCard = linkedCardSelected,
                InBlackList = inBlackList,
				OwnerName = ownerUser,
				CardUid = cardUidText,
				LastBalance = selectedServiceCard?.LastBalance ?? 0, //  balance, // LastXAVI
				PendingBalance = selectedServiceCard?.PendingBalance ?? 0, //  balance, // LastXAVI
				Entrances = entrancesList,
				Products = products,
				Tickets = tickets,
                PurseId = purseId,
				ServiceDocuments = serviceDocuments
			};

			stopwatch.Stop();
			times.AppendFormat("ShopGetEntranceTypesByConcession Initialization: {0}\n", stopwatch.Elapsed);

			return rst;
        }
        #endregion ExecuteAsync
    }
}
