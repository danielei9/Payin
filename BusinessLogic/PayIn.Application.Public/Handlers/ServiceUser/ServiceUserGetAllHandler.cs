using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using PayIn.Domain.Transport;
using PayIn.Infrastructure.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceUserGetAllHandler : 
		IQueryBaseHandler<ServiceUserGetAllArguments, ServiceUserGetAllResult>
	{
		[Dependency] public IEntityRepository<ServiceUser> Repository { get; set; }
        [Dependency] public IEntityRepository<ServiceUserLink> ServiceUserLinkRepository { get; set; }
        [Dependency] public IEntityRepository<BlackList> BlackListRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public SecurityRepository SecurityRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ServiceUserGetAllResult>> ExecuteAsync(ServiceUserGetAllArguments arguments)
		{
			var paymentConcessions = (await PaymentConcessionRepository.GetAsync("Concession"))
				.Where(x =>
					x.Concession.Supplier.Login == SessionData.Login ||
					x.Concession.Supplier.Workers
						.Any(y => y.Login == SessionData.Login)
				);
			if (arguments.PaymentConcessionId == null)
				arguments.PaymentConcessionId = paymentConcessions.FirstOrDefault()?.Id;
			var paymentConcession = paymentConcessions.FirstOrDefault();

            var blackLists = (await BlackListRepository.GetAsync());

			var serviceUserLinks = (await ServiceUserLinkRepository.GetAsync())
				.Where(x =>
					x.Card.ConcessionId == paymentConcession.ConcessionId
				);

			var items = (await Repository.GetAsync())
				.Where(x =>
					(
						(x.Concession.Supplier.Login == SessionData.Login) ||
						(
							(SessionData.Roles.Contains(AccountRoles.PaymentWorkerCash)) &&
							(paymentConcessions
								.Any(y => y.ConcessionId == x.ConcessionId)
							)
						)
					)
				);

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						(x.Name + " " + x.LastName).Contains(arguments.Filter) ||
						(x.Login).Contains(arguments.Filter) ||
						(x.Card.UidText.Contains(arguments.Filter)) ||
						(x.VatNumber.Contains(arguments.Filter)) ||
						(x.Code.Contains(arguments.Filter)) ||
						(x.Card.Concession.Name.Contains(arguments.Filter)) ||
						(x.OnwnerCards.Where(y =>
							y.Uid.ToString().Contains(arguments.Filter) ||
							y.Concession.Name.Contains(arguments.Filter)
						).Any())
					);

			if (arguments.PaymentConcessionId != null)
			{
				var concessionId = paymentConcessions
					.Where(x => x.Id == arguments.PaymentConcessionId)
					.Select(x => x.ConcessionId)
					.FirstOrDefault();

				items = items
					.Where(x => x.ConcessionId == concessionId);
			}

			var result = items
				.Select(x => new ServiceUserGetAllResult
				{
					Id = x.Id,
					Code = x.Code,
					Name = x.Name,
					Login = x.Login,
					LastName = x.LastName,
					VatNumber = x.VatNumber,
					State = x.State,
					ServiceGroupsCount = x.Groups.Count(),
					Cards =
						(
							(
								x.OnwnerCards
									.Where(y =>
										//(y.Id != x.CardId) &&
										(y.ConcessionId == x.ConcessionId) &&
										(y.State != ServiceCardState.Deleted)
									)
									.Select(y => new ServiceUserGetAllResult.Card
									{
										Id = y.Id,
										//Uid = y.Uid,
										UidText = y.UidText,
										//LastSeq = y.LastSeq,
										State = y.Users.Count() == 0 ? ServiceCardState.Active : y.State,
										Type =
											//(x.State != ServiceCardState.Emited) ? MobileServiceCardGetAllResult.ResultType.NotEmitted :
											(y.OwnerUserId == null) ? ServiceUserGetAllResult.Card.ResultType.Anonymous :
											(y.Users.Any()) ? ServiceUserGetAllResult.Card.ResultType.Principal :
												ServiceUserGetAllResult.Card.ResultType.Secondary,
										OwnerName = ((y.OwnerUser.Name + " " + y.OwnerUser.LastName) ?? "").Trim(),
										//ConcessionName = y.ConcessionId == x.ConcessionId ? "" : y.Concession.Name,
										Alias = y.Alias ?? "",
										BlackListCount = blackLists
											.Where(z =>
												(z.Uid == y.Uid) &&
												(z.SystemCardId == y.SystemCardId) &&
												(z.State == BlackList.BlackListStateType.Active) &&
												(!z.Resolved)
											)
											.Count()
									})
								).Union(
									serviceUserLinks
										.Where(y => y.Login == x.Login)
										.Select(y => new ServiceUserGetAllResult.Card
										{
											Id = y.Card.Id,
											//Uid = y.Card.Uid,
											UidText = y.Card.UidText,
											//LastSeq = y.LastSeq,
											State = y.Card.Users.Count() == 0 ? ServiceCardState.Active : y.Card.State,
											Type = ServiceUserGetAllResult.Card.ResultType.Linked,
											OwnerName = (y.Card.OwnerUser.Name + " " + y.Card.OwnerUser.LastName) ?? "",
											//ConcessionName = y.ConcessionId == x.ConcessionId ? "" : y.Concession.Name,
											Alias = y.Card.Alias ?? "",
											BlackListCount = blackLists
											.Where(z =>
												(z.Uid == y.Card.Uid) &&
												(z.SystemCardId == y.Card.SystemCardId) &&
												(z.State == BlackList.BlackListStateType.Active) &&
												(!z.Resolved)
											)
											.Count()
										})
								)
						)
				})
				.ToList();

			foreach(var item in result)
			{
				var user = await SecurityRepository.GetUserAsync(item.Login);
				item.IsRegistered = user != null;
				item.IsEmailConfirmed = user?.EmailConfirmed ?? false;
			}

			return new ServiceUserGetAllResultBase
			{
				Data = result,
				PaymentConcessionId = paymentConcession.Id,
				PaymentConcessionName = paymentConcession.Concession.Name ?? "",
				PaymentConcessions = paymentConcessions
					.Select(x => new SelectorResult
					{
						Id = x.Id,
						Value = x.Concession.Name ?? ""
					})
			};

		}
		#endregion ExecuteAsync
	}
}
