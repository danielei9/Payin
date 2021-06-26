using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class ServiceCardGetHandler :
        IQueryBaseHandler<ServiceCardGetArguments, ServiceCardGetResult>
    {
		[Dependency] public IEntityRepository<ServiceCard> Repository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<PurseValue> PurseValueRepository { get; set; }
		[Dependency] public IEntityRepository<Entrance> EntranceRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }
		[Dependency] public IEntityRepository<BlackList> BlackListRepository { get; set; }
		[Dependency] public IEntityRepository<GreyList> GreyListRepository { get; set; }
		[Dependency] public MobileServiceCardGetHandler MobileServiceCardGetHandler { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<ServiceCardGetResult>> ExecuteAsync(ServiceCardGetArguments arguments)
		{
            var now = DateTime.UtcNow;
			var roles = SessionData.Roles;
			var rechargeGreyList = (await GreyListRepository.GetAsync())
				.Where(x =>
					!x.Resolved &&
					x.ResolutionDate == null &&
					(x.Field == "W1B" || x.Field == "W2B" || x.Field == "W3B")
				)
				.Select(x => new
				{
					x.Uid,
					//PurseSlot = x.Field == "W1B" ? 1 :
					//			x.Field == "W2B" ? 2 : 3,
					x.Field,
					DateTime = x.RegistrationDate,
					Amount = x.NewValue
				});

			// El valor de estas variables los obtendremos de la configuración asignada en el paymentConcession
			// Donde aparece esta variable en el Where deberemos poner la condición para obtener si es TRUE o FALSE
			bool canSellBalance = true;
			bool canGiveBalance = true;
			bool canBuyBalance = true;
			bool canSellEntrance = true;
			bool canGiveEntrance = true;
			bool canBuyEntrance = true;
			
			var paymentConcessions = await PaymentConcessionRepository.GetAsync();
			var purseValues = (await PurseValueRepository.GetAsync());

			var serviceCards = (await ServiceCardRepository.GetAsync("OwnerUser", "SystemCard.ConcessionOwner.Supplier"))
				.Where(x => x.Id == arguments.Id);

			var card = serviceCards.FirstOrDefault(); // ¿Cómo puedo obtenerlo sin hacer llamada a la BD?
			if (card == null)
				throw new ArgumentException("cardId");

			var iAmOwner = (
				card.OwnerLogin == SessionData.Login ||
				card.OwnerUser?.Login == SessionData.Login
			);
			var iAmSystemOwner = (
				card.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login
			);

			var entrances_ = (await EntranceRepository.GetAsync("Event", "EntranceType"))
                .Where(x =>
					(serviceCards .Any(y => y.Uid == x.Uid)) &&
					(x.State != EntranceState.Deleted) &&
					(x.State != EntranceState.Pending) &&
					(
						(iAmOwner) ||
						(iAmSystemOwner) ||
						(x.Login == SessionData.Login)
					)
				);

            var entrances = (await MobileServiceCardGetHandler.GetEntrancesAsync(entrances_, now))
                .Select(x => new ServiceCardGetResult_Entrance
                {
                    Id = x.Id,
                    Name = x.Name,
                    LastName = x.LastName,
                    Code = x.Code,
                    EntranceTypeName = x.EntranceTypeName,
                    EventName = x.EventName,
					EventStart = x.EventStart,
                    Amount = x.Amount,
					Timestamp = x.Timestamp,
					State = x.State,
					Finished = x.Finished
                })
				.OrderBy(x => x.Finished)
				.ThenByDescending(x => x.EventName)
				.ThenByDescending(x => x.EventStart)
				.ThenByDescending(x => x.Timestamp);

			var sessionUserType = (await Repository.GetAsync())
				.Where(x =>
					(x.Id == arguments.Id) &&
					(x.State != ServiceCardState.Deleted) &&
					(
						(roles.Contains(AccountRoles.Superadministrator)) ||    // Soy el superadministrador
						(x.Concession.Supplier.Login == SessionData.Login) ||   // Soy el dueño de la concesión
						(x.Concession.Supplier.Workers                          // Soy un trabajador de la concesión
							.Any(y => y.Login == SessionData.Login)
						) ||
						x.Concession.SystemCardOwners                           // Soy el dueño del sistema de tarjetas
							.Any(y => y.SystemCardMembers
								.Any(z =>
									z.Login == x.Concession.Supplier.Login &&
								  	z.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login
								)
							) ||
						(x.OwnerUser.Login == SessionData.Login) ||             // Soy dueño de la tarjeta, pri, sec o anonima
						(x.LinkedUsers                                      // Es una tarjeta vinculada conmigo (p. ej. la de mi hijo)
							.Any(y => y.Login == SessionData.Login)
						)
					)
				)
				.Select(x => new 
				{
					IAmSeller =
					(
						(roles.Contains(AccountRoles.Superadministrator)) ||        // Soy el superadministrador
						(x.Concession.Supplier.Login == SessionData.Login) ||       // Soy el dueño de la concesión
						(x.Concession.Supplier.Workers                              // Soy un trabajador de la concesión
							.Any(y => y.Login == SessionData.Login)
						) ||
						x.Concession.SystemCardOwners.Any(y => y.SystemCardMembers  // Soy el dueño del sistema de tarjetas
							.Any(z =>
								z.Login == x.Concession.Supplier.Login &&
								z.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login
							)
						)
					),
					IAmBuyer =
					(
						x.OwnerUser.Login == SessionData.Login ||
						x.LinkedUsers.Any(y => y.Login == SessionData.Login) // Es una tarjeta vinculada conmigo (p. ej. la de mi hijo)
					),
				})
			.FirstOrDefault();

			var blackLists = await BlackListRepository.GetAsync();

			var result = (await Repository.GetAsync())
				.Where(x =>
					(x.Id == arguments.Id) &&
					(x.State != ServiceCardState.Deleted) &&
					(
						(roles.Contains(AccountRoles.Superadministrator)) ||    // Soy el superadministrador
						(x.Concession.Supplier.Login == SessionData.Login) ||   // Soy el dueño de la concesión
						(x.Concession.Supplier.Workers                          // Soy un trabajador de la concesión
							.Any(y => y.Login == SessionData.Login)
						) ||
						x.Concession.SystemCardOwners                           // Soy el dueño del sistema de tarjetas
							.Any(y => y.SystemCardMembers
								.Any(z =>
									z.Login == x.Concession.Supplier.Login &&
								  	z.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login
								)
							) ||
						(x.OwnerUser.Login == SessionData.Login) ||             // Soy dueño de la tarjeta, pri, sec o anonima
						(x.LinkedUsers                                      // Es una tarjeta vinculada conmigo (p. ej. la de mi hijo)
							.Any(y => y.Login == SessionData.Login)
						)
					)
				)
				.Select(x => new
				{
					InBlackList = blackLists
						.Where(y =>
							serviceCards.Any(z =>
								y.Uid == z.Uid &&
								y.SystemCardId == z.SystemCardId
							) &&
							(y.State == BlackList.BlackListStateType.Active) &&
							(!y.Resolved)
						)
						.Any(),
					x.State,
					x.Id,
					PaymentConcessionId = paymentConcessions
						.Where(y => y.ConcessionId == x.ConcessionId)
						.Select(y => y.Id)
						.FirstOrDefault(),
					SystemCardName = x.SystemCard.Name,
					x.Uid,
					CardId = x.UidText,
					CardType =
						(x.State != ServiceCardState.Emited) ? ServiceCardGetResult.ResultType.NotEmitted :
						(x.OwnerUserId == null) ? ServiceCardGetResult.ResultType.Anonymous :
						(x.Users
							.Any(y => y.CardId == x.Id)
						) ? ServiceCardGetResult.ResultType.Principal :
							ServiceCardGetResult.ResultType.Secondary,
					UserPhoto = x.SystemCard.PhotoUrl,
					UserName = x.OwnerUser.Name,
					UserSurname = x.OwnerUser.LastName,
					x.Alias,
					PurseValues = purseValues
						.Where(y =>
							y.ServiceOperation.CardId == x.Id &&
							//y.ServiceOperation.Seq == x.Operations.Max(z => z.Seq)
							y.ServiceOperation.Id == x.Operations.Max(z => z.Id)
						)
						.OrderBy(y => y.Slot)
						.Select(y => new ServiceCardGetResult_PurseValue
						{
							Slot = y.Slot,
							Name = y.Purse.Name,
							Amount = y.Amount
						})
						.ToList(),
					Groups = x.Groups
						.OrderBy(y => y.Name)
						.Select(y => new ServiceCardGetResult_Group
						{
							Name = y.Name
						}),
					Operations = x.Operations
						.Where(y => y.Date >= new DateTime(2019, 01, 01))
						.OrderByDescending(y => y.Seq)
						.ThenByDescending(y => y.Type == ServiceOperationType.CardRead ? 1 : 0)
						.ThenByDescending(y => y.Id)
						.Select(y => new
						{
							y.Id,
							y.Date,
							y.Type,
							y.Seq,
							y.ESeq,
							PurseValues = purseValues
								.Where(z => z.ServiceOperationId == y.Id)
								.OrderBy(z => z.Purse.Name)
								.Select(z => new
								{
									z.Purse.Name,
									z.Amount
								})
								.ToList()
						}),
					Relation = 
						(roles.Contains(AccountRoles.Superadministrator)) ||		// Soy el superadministrador
						(x.Concession.Supplier.Login == SessionData.Login) ||		// Soy el dueño de la concesión
						(x.Concession.Supplier.Workers								// Soy un trabajador de la concesión
							.Any(y => y.Login == SessionData.Login)
						) ||
						x.Concession.SystemCardOwners.Any(y => y.SystemCardMembers	// Soy el dueño del sistema de tarjetas
							.Any(z =>
								z.Login == x.Concession.Supplier.Login &&
								z.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login
							)
						) ? ServiceCardGetResult.RelationType.IAmSystemCardManager :
							x.OwnerUser.Login == SessionData.Login ? 
								ServiceCardGetResult.RelationType.IAmCardOwner : 
								ServiceCardGetResult.RelationType.IAmVinculatedUser,
					CanSellBalance = (canSellBalance && sessionUserType.IAmSeller),
					CanGiveBalance = (canGiveBalance && sessionUserType.IAmSeller),
					CanBuyBalance = (canBuyBalance && sessionUserType.IAmBuyer),
					CanSellEntrance = (canSellEntrance && sessionUserType.IAmSeller),
					CanGiveEntrance = (canGiveEntrance && sessionUserType.IAmSeller),
					CanBuyEntrance = (canBuyEntrance && sessionUserType.IAmBuyer)
				})
				.ToList();

			var res = result
				.Select(x => new ServiceCardGetResult
				{
					Id = x.Id,
					PaymentConcessionId = x.PaymentConcessionId,
					SystemCardName = x.SystemCardName,
					Uid = x.Uid,
					CardId = x.CardId,
					CardType = x.CardType,
					UserPhoto = x.UserPhoto,
					UserName = x.UserName,
					UserSurname = x.UserSurname,
					Alias = x.Alias,
					PurseValues = x.PurseValues
						.Select(y => new ServiceCardGetResult_PurseValue
						{
							Name = y.Name,
							Amount = y.Amount,
							PendingRecharges = rechargeGreyList
								.Where(z =>
									z.Uid == x.Uid &&
									//z.PurseSlot == y.Slot
									z.Field == "W" + y.Slot + "B"
								)
								.Select(z => new
								{
									DateTime = z.DateTime,
									Amount = z.Amount
								})
								.ToList()
								.Select(z => new ServiceCardGetResult_PurseValue.PendingRecharge
								{
									DateTime = z.DateTime.ToUTC(),
									Amount = Convert.ToInt32(z.Amount) / 100m
								})
						}),
					Groups = x.Groups,
					Entrances = entrances,
					Relation = x.Relation,
					Operations = x.Operations
						.Select(y => new ServiceCardGetResult_Operation
						{
							Id = y.Id,
							Date = y.Date,
							Seq = y.Seq,
							ESeq = y.ESeq,
							TypeName = y.Type.ToEnumAlias(","),
							PurseValues = y.PurseValues
							.Select(z => new ServiceCardGetResult_PurseValue
							{
								Name = z.Name,
								Amount = z.Amount
							})
						}),
					CanSellBalance = x.CanSellBalance,
					CanGiveBalance = x.CanGiveBalance,
					CanBuyBalance = x.CanBuyBalance,
					CanSellEntrance = x.CanSellEntrance,
					CanGiveEntrance = x.CanGiveEntrance,
					CanBuyEntrance = x.CanBuyEntrance,
					InBlackList = x.InBlackList,
					State = x.State
				});

			var rst = res.ToList();
           
			return new ResultBase<ServiceCardGetResult> { Data = rst };
		}
		#endregion ExecuteAsync
	}
}
