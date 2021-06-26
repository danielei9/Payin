using PayIn.Application.Dto.Payments.Arguments.TicketTemplate;
using PayIn.Application.Dto.Payments.Results.TicketTemplate;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class TicketTemplateGetAllHandler :
		IQueryBaseHandler<TicketTemplateGetAllArguments, TicketTemplateGetAllResult>
	{
		private readonly IEntityRepository<TicketTemplate> Repository;

		#region Constructors
		public TicketTemplateGetAllHandler(
			IEntityRepository<TicketTemplate> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TicketTemplateGetAllResult>> ExecuteAsync(TicketTemplateGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync());			
			
			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x => (
					x.Name.Contains(arguments.Filter))
				);

			var result = items
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					ConcessionsCount = x.Concessions.Count(),
					IsGeneric = x.IsGeneric		
				})
				.OrderByDescending(x => x.Name)
				.ToList()
				.Select(x => new TicketTemplateGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					ConcessionsCount = x.ConcessionsCount,
					IsGeneric = x.IsGeneric
						
				})
				.ToList();

			return new ResultBase<TicketTemplateGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
 }
