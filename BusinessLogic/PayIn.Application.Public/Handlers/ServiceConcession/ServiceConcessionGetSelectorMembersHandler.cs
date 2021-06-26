using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.ServiceConcession;
using PayIn.Application.Dto.Results.ServiceConcession;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class ServiceConcessionGetSelectorMembersHandler :
		IQueryBaseHandler<ServiceConcessionGetSelectorMembersArguments, ServiceConcessionGetSelectorMembersResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<ServiceConcession> Repository { get; set; }
		[Dependency] public IEntityRepository<SystemCardMember> SystemCardMemberRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ServiceConcessionGetSelectorMembersResult>> ExecuteAsync(ServiceConcessionGetSelectorMembersArguments arguments)
		{
			var items = (await Repository.GetAsync());
			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						x.Name.Contains(arguments.Filter)
					);

			if (!SessionData.Roles.Contains(AccountRoles.Superadministrator))
			{
				var myPaymentConcessions = (await PaymentConcessionRepository.GetAsync())
					.Where(x =>
						(x.Concession.State == ConcessionState.Active) &&
						(
							(x.Concession.Supplier.Login == SessionData.Login) ||
							(x.PaymentWorkers
								.Any(y =>
									(y.State == WorkerState.Active) &&
									(y.Login == SessionData.Login)
								)
							) 
						)
					);

				var mySystemCardMembers = (await SystemCardMemberRepository.GetAsync())
					.Where(x =>
						myPaymentConcessions
							.Any(y =>
								x.SystemCard.ConcessionOwnerId == y.ConcessionId
							)
					);

				items = items
					.Where(x =>
						mySystemCardMembers
							.Any(y =>
								x.Supplier.Login == y.Login
							)
					);
			}
			
			var result = items
				.Select(x => new ServiceConcessionGetSelectorMembersResult
				{
					Id = x.Id,
					Value = x.Name
				});

			return new ResultBase<ServiceConcessionGetSelectorMembersResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
