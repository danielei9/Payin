using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Domain.Public;
using Microsoft.Practices.Unity;

namespace PayIn.Application.Public.Handlers
{
    public class EventGetSelectorHandler :
        IQueryBaseHandler<EventGetSelectorArguments, EventGetSelectorResult>
    {
		[Dependency] public IEntityRepository<SystemCardMember> SystemCardMemberRepository { get; set; }
		[Dependency] public IEntityRepository<Event> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<EventGetSelectorResult>> ExecuteAsync(EventGetSelectorArguments arguments)
        {
            var items = (await Repository.GetAsync());
			var systemCardMembers = (await SystemCardMemberRepository.GetAsync());

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						x.Name.Contains(arguments.Filter)
					);

			var result = items
				.Where(x =>
						// Eventos mios
						(x.PaymentConcession.Concession.Supplier.Login == SessionData.Login) ||
						// Eventos de las empresas de mi sistema de tarjetas
						(systemCardMembers
								.Any(y =>
									y.Login == x.PaymentConcession.Concession.Supplier.Login &&
									y.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login
									)
					   ))
			    .ToList()
				.Select(x => new EventGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				});

            return new ResultBase<EventGetSelectorResult> { Data = result };
        }
		#endregion ExecuteAsync
	}
}