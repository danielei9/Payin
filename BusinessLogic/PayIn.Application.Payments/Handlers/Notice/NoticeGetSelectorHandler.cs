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
    public class NoticeGetSelectorHandler :
        IQueryBaseHandler<NoticeGetSelectorArguments, NoticeGetSelectorResult>
    {
		//[Dependency] public IEntityRepository<SystemCardMember> SystemCardMemberRepository { get; set; }
		[Dependency] public IEntityRepository<Notice> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<NoticeGetSelectorResult>> ExecuteAsync(NoticeGetSelectorArguments arguments)
        {
            var items = (await Repository.GetAsync());
			//var systemCardMembers = (await SystemCardMemberRepository.GetAsync());

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						x.Name.Contains(arguments.Filter)
					);

			var result = items
				.Where(x =>
					// Noticias mías
					x.PaymentConcession.Concession.Supplier.Login == SessionData.Login
				)
			    .ToList()
				.Select(x => new NoticeGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				});

            return new ResultBase<NoticeGetSelectorResult> { Data = result };
        }
		#endregion ExecuteAsync
	}
}