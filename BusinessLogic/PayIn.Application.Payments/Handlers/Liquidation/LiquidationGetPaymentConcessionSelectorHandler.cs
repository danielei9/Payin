using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System.Linq;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Domain.Security;

namespace PayIn.Application.Public.Handlers
{
	public class LiquidationGetPaymentConcessionSelectorHandler :
		IQueryBaseHandler<LiquidationGetPaymentConcessionSelectorArguments, LiquidationGetPaymentConcessionSelectorResult>
	{
		[Dependency] public IEntityRepository<SystemCardMember> SystemCardMemberRepository { get; set; }
		[Dependency] public IEntityRepository<Domain.Payments.PaymentConcession> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<LiquidationGetPaymentConcessionSelectorResult>> ExecuteAsync(LiquidationGetPaymentConcessionSelectorArguments arguments)
		{
			var systemCardMembers = (await SystemCardMemberRepository.GetAsync());
			var items = await Repository.GetAsync("Concession");

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						x.Concession.Name.Contains(arguments.Filter)
					);


			var result = items
				.Where(x =>
					(SessionData.Roles.Contains(AccountRoles.Superadministrator)) ||
					// Eventos mios
					(x.Concession.Supplier.Login == SessionData.Login) ||
					// Eventos de las empresas de mi sistema de tarjetas
					(systemCardMembers
						.Any(y =>
							y.Login == x.Concession.Supplier.Login &&
							y.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login
						)
					) ||
					// Eventos donde trabajo
					(x.PaymentWorkers
						.Any(y => y.Login == SessionData.Login)
					)
				)
				.ToList()
				.Select(x => new LiquidationGetPaymentConcessionSelectorResult
				{
					Id = x.Id,
					Value = x.Concession.Name
				});

			return new ResultBase<LiquidationGetPaymentConcessionSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
