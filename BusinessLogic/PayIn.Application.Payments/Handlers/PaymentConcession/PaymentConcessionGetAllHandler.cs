using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class PaymentConcessionGetAllHandler :
        IQueryBaseHandler<PaymentConcessionGetAllArguments, PaymentConcessionGetAllResult>
    {
        [Dependency] public IEntityRepository<PaymentConcession> Repository { get; set; }
        [Dependency] public IEntityRepository<SystemCardMember> SystemCardMemberRepository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<PaymentConcessionGetAllResult>> ExecuteAsync(PaymentConcessionGetAllArguments arguments)
		{
            var systemCardMembers = (await SystemCardMemberRepository.GetAsync());

            var items = (await Repository.GetAsync());
			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						x.Concession.Name.Contains(arguments.Filter)
					);

			var result = items
				.Where(x =>
                    systemCardMembers
                        .Where(y =>
                            (y.Login == x.Concession.Supplier.Login) &&
                            (y.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login) ||
                            (y.SystemCard.ConcessionOwner.Supplier.Workers
								.Where(z => z.Login == SessionData.Login)
								.Any()
                            )
                        )
                        .Any()
                )
                .Select(x => new PaymentConcessionGetAllResult
                {
                    Id = x.Id,
                    // Información comercial
                    Name = x.Concession.Name,
                    Phone = x.Phone,
					Email = x.Concession.Supplier.Login					
                });

            return new ResultBase<PaymentConcessionGetAllResult> { Data = result };
        }
        #endregion ExecuteAsync
    }
}
