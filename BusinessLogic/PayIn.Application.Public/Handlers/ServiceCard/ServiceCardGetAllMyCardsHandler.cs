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
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceCardGetAllMyCardsHandler :
        IQueryBaseHandler<ServiceCardGetAllMyCardsArguments, ServiceCardGetAllMyCardsResult>
	{
		[Dependency] public IEntityRepository<BlackList> BlackListRepository { get; set; }
		[Dependency] public IEntityRepository<GreyList> GreyListRepository { get; set; }
		[Dependency] public IEntityRepository<Purse> PurseRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> Repository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<ServiceCardGetAllMyCardsResult>> ExecuteAsync(ServiceCardGetAllMyCardsArguments arguments)
		{
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
			var roles = SessionData.Roles;

            var blackLists = (await BlackListRepository.GetAsync());

			var result = (await Repository.GetAsync())
                .Where(x =>                                                     // SOLO DEBE CARGAR MIS PULSERAS O VINCULADAS A MI
					(x.State != ServiceCardState.Deleted) &&
                    (
						(x.OwnerUser.Login == SessionData.Login) ||				// Soy dueño de la tarjeta, pri, sec o anonima
						(x.LinkedUsers										// Es una tarjeta vinculada conmigo (p. ej. la de mi hijo)
							.Any(y => y.Login == SessionData.Login)
						)
					)
				)
                .Select(x => new
				{
                    x.Id,
                    x.Uid,
                    x.UidText,
					BatchName = x.ServiceCardBatch.Name,
					x.State,
                    Type =
                        (x.State != ServiceCardState.Emited) ? ServiceCardGetAllMyCardsResult.ResultType.NotEmitted :
                        (x.OwnerUserId == null) ? ServiceCardGetAllMyCardsResult.ResultType.Anonymous :
                        (x.Users.Any()) ? ServiceCardGetAllMyCardsResult.ResultType.Principal :
							ServiceCardGetAllMyCardsResult.ResultType.Secondary,
                    x.OwnerUser.Name,
                    x.OwnerUser.LastName,
                    x.Alias,
					//LastSeq = x.LastSeq, // LastXAVI
					LastSeq = x.Operations
								.Max(z => z.Seq) ?? 0,
					//LastBalance = x.LastBalance, // LastXAVI
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
						.ToList(), // Si no se pone en la consulta después da error de metodo no soportado
					InBlackList = blackLists
                        .Where(y =>
                            (y.Uid == x.Uid) &&
                            (y.SystemCardId == x.SystemCardId) &&
                            (y.State == BlackList.BlackListStateType.Active) &&
                            (!y.Resolved)
                        )
                        .Any(),
					Relation =
						x.OwnerUser.Login == SessionData.Login ?
                            ServiceCardRelationType.IAmCardOwner :
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
								) ? ServiceCardRelationType.IAmSystemCardManager :
                                    ServiceCardRelationType.IAmVinculatedUser,
					CardType =
						(x.State != ServiceCardState.Emited) ? ServiceCardGetAllMyCardsResult.ResultType.NotEmitted :
						(x.OwnerUserId == null) ? ServiceCardGetAllMyCardsResult.ResultType.Anonymous :
						(x.Users
							.Any(y => y.CardId == x.Id)
						) ? ServiceCardGetAllMyCardsResult.ResultType.Principal :
							ServiceCardGetAllMyCardsResult.ResultType.Secondary,
					x.SystemCard.PhotoUrl,
					SystemCardName = x.SystemCard.Name
				})
				.ToList()
				.Select(x => new ServiceCardGetAllMyCardsResult
				{
					Id = x.Id,
					Uid = x.Uid,
					UidText = x.UidText,
					BatchName = x.BatchName,
					State = x.State,
					Type = x.Type,
					Name = x.Name,
					LastName = x.LastName,
					Alias = x.Alias,
					LastSeq = x.LastSeq,
					LastBalance = x.LastBalance,
					PendingBalance = x.PendingBalances
						.Sum(z => (decimal?)Convert.ToInt32(z) / 100) ?? 0,
					InBlackList = x.InBlackList,
					Relation = x.Relation,
					CardType = x.CardType,
					PhotoUrl = x.PhotoUrl,
					SystemCardName = x.SystemCardName
				});

            if (!arguments.Filter.IsNullOrEmpty())
            {
                result = result
                    .Where(x =>
                        (x.Uid.ToString().Contains(arguments.Filter)) ||
                        (x.UidText.Contains(arguments.Filter)) ||
						(x.BatchName.Contains(arguments.Filter)) ||
						(x.Name + " " + x.LastName).Contains(arguments.Filter) ||
                        (x.Alias.Contains(arguments.Filter))
                    );
            }

			var res = result
				.Where(x =>
					x.CardType != ServiceCardGetAllMyCardsResult.ResultType.NotEmitted
				)
				.OrderBy(x =>
					x.Relation == ServiceCardRelationType.IAmCardOwner ? 1 :
					x.Relation == ServiceCardRelationType.IAmSystemCardManager ? 2 : 3)
				.ThenBy(x => x.Name)
				.ThenBy(x => x.LastName)
				.ToList();

			return new ResultBase<ServiceCardGetAllMyCardsResult> { Data = res };
		}
		#endregion ExecuteAsync
	}
}
