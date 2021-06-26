using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class MobileServiceCardGetAllHandler :
        IQueryBaseHandler<MobileServiceCardGetAllArguments, MobileServiceCardGetAllResult>
    {
        [Dependency] public IEntityRepository<BlackList> BlackListRepository { get; set; }
        [Dependency] public IEntityRepository<Purse> PurseRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> Repository { get; set; }
		[Dependency] public IEntityRepository<GreyList> GreyListRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<MobileServiceCardGetAllResult>> ExecuteAsync(MobileServiceCardGetAllArguments arguments)
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
			var blackLists = (await BlackListRepository.GetAsync());
			var roles = SessionData.Roles;

			var items = (await Repository.GetAsync());
			if (!arguments.Filter.IsNullOrEmpty())
			{
				items = items
					.Where(x =>
						x.UidText.Contains(arguments.Filter) ||
						x.OwnerUser.Name.Contains(arguments.Filter) ||
						x.OwnerUser.LastName.Contains(arguments.Filter) ||
						x.Alias.Contains(arguments.Filter)
					);
			}

			if (SessionData.Login.IsNullOrEmpty())
			{
				return new MobileServiceCardGetAllResultBase
				{
					Data = new List<MobileServiceCardGetAllResult>(),
					OwnerCount = 0,
					LinkedCount = 0
				};
			}

			var result = items
				.Where(x =>
                    (
						(x.State == ServiceCardState.Emited) ||
						(x.State == ServiceCardState.Active)
					) &&
                    (
                        (x.OwnerUser.Login == SessionData.Login) ||             // Soy dueño de la tarjeta, pri, sec o anonima
                        (x.LinkedUsers                                          // Es una tarjeta vinculada conmigo (p. ej. la de mi hijo)
                            .Any(y => y.Login == SessionData.Login)
                        )
                    )
                )
                .Select(x => new
                {
                    x.Id,
                    x.Uid,
                    x.UidText,
                    Relation =
                        (x.OwnerLogin == SessionData.Login || x.OwnerUser.Login == SessionData.Login) ?
                            MobileServiceCardGetAllResult.RelationType.Owner :
                            MobileServiceCardGetAllResult.RelationType.Linked,
                    Type =
						(x.State != ServiceCardState.Emited) ? MobileServiceCardGetAllResult.ResultType.NotEmitted :
						(x.OwnerUserId == null) ? MobileServiceCardGetAllResult.ResultType.Anonymous :
                        (x.Users.Any()) ? MobileServiceCardGetAllResult.ResultType.Principal :
                            MobileServiceCardGetAllResult.ResultType.Secondary,
                    x.OwnerUser.Name,
                    x.OwnerUser.LastName,
                    x.Alias,
					//LastSeq = x.LastSeq, // LastXAVI
					LastSeq = x.Operations
								.Max(z => z.Seq) ?? 0,
					//LastBalance = //(x.OwnerLogin == SessionData.Login || x.OwnerUser.Login == SessionData.Login) ?
                        //(decimal?) null :
                        //0, x.LastBalance, // LastXAVI
					LastBalance = x.Operations
						.Where(z =>
							z.Seq == x.Operations
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
					x.SystemCard.PhotoUrl,
					SystemCardName = x.SystemCard.Name
				})
				.OrderBy(x => new
				{
					x.Type,
					x.Name,
					x.LastName
				})
				.ToList()
				.Select(x => new MobileServiceCardGetAllResult
				{
					Id = x.Id,
					Uid = x.Uid,
					UidText = x.UidText,
					Relation = x.Relation,
					Type = x.Type,
					Name = x.Name,
					LastName = x.LastName,
					Alias = x.Alias,
					LastSeq = x.LastSeq,
					LastBalance = x.LastBalance,
					PendingBalance = x.PendingBalances
						.Sum(z => (decimal?)Convert.ToInt32(z) / 100) ?? 0,
					InBlackList = x.InBlackList,
					PhotoUrl = x.PhotoUrl,
					SystemCardName = x.SystemCardName
				});

			return new MobileServiceCardGetAllResultBase
			{
				Data = result.ToList(),
                OwnerCount = result.Count(x => x.Relation == MobileServiceCardGetAllResult.RelationType.Owner),
                LinkedCount = result.Count(x => x.Relation == MobileServiceCardGetAllResult.RelationType.Linked)
			};
		}
		#endregion ExecuteAsync
	}
}
