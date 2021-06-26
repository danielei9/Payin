using PayIn.Application.Dto.Payments.Arguments.TicketTemplate;
using PayIn.Application.Dto.Payments.Results.TicketTemplate;
using PayIn.Domain.Public;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.BusinessLogic.Common;

namespace PayIn.Application.Public.Handlers
{
	public class TicketTemplateGetSelectorHandler :
		IQueryBaseHandler<TicketTemplateGetSelectorArguments, TicketTemplateGetSelectorResult>
	{
		private readonly IEntityRepository<TicketTemplate> Repository;

		#region Constructors
		public TicketTemplateGetSelectorHandler(IEntityRepository<TicketTemplate> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TicketTemplateGetSelectorResult>> ExecuteAsync(TicketTemplateGetSelectorArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x => x.Name.Contains(arguments.Filter))
				.Select(x => new TicketTemplateGetSelectorResult {
					Id = x.Id,
					Value = x.Name
				});

			return new ResultBase<TicketTemplateGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
