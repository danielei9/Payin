using PayIn.Application.Dto.Payments.Arguments.TicketTemplate;
using PayIn.Application.Dto.Payments.Results.TicketTemplate;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Application.Payments.Handlers
{
	public class TicketTemplateGetDetailsHandler :
		IQueryBaseHandler<TicketTemplateGetDetailsArguments, TicketTemplateGetDetailsResult>
	{
		private readonly IEntityRepository<TicketTemplate> Repository;

		#region Constructors
		public TicketTemplateGetDetailsHandler(
			IEntityRepository<TicketTemplate> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
				Repository = repository;			
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TicketTemplateGetDetailsResult>> ExecuteAsync(TicketTemplateGetDetailsArguments arguments)
		{
			var ticketTemplate = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.Select(x => new TicketTemplateGetDetailsResult
				 {
					Name = x.Name,			
					RegEx = x.RegEx,
					PreviousTextPosition = x.PreviousTextPosition,
					BackTextPosition = x.BackTextPosition,
					AmountPosition = x.AmountPosition,
					DateFormat = x.DateFormat,
					DatePosition = x.DatePosition,
					DecimalCharDelimiter = x.DecimalCharDelimiter,
					ReferencePosition = x.ReferencePosition,
					TitlePosition = x.TitlePosition,
					WorkerPosition = x.WorkerPosition
				})
				 .FirstOrDefault();
		
			
			return new TicketTemplateGetDetailsResultBase
			{
				Data = new List<TicketTemplateGetDetailsResult>
				{
					new TicketTemplateGetDetailsResult
					{
						Name = ticketTemplate.Name,						
						RegEx = ticketTemplate.RegEx,
						PreviousTextPosition = ticketTemplate.PreviousTextPosition,
						BackTextPosition = ticketTemplate.BackTextPosition,
						AmountPosition = ticketTemplate.AmountPosition,
						DateFormat = ticketTemplate.DateFormat,
						DatePosition = ticketTemplate.DatePosition,
						DecimalCharDelimiter = ticketTemplate.DecimalCharDelimiter,
						ReferencePosition = ticketTemplate.ReferencePosition,
						TitlePosition = ticketTemplate.TitlePosition,
						WorkerPosition = ticketTemplate.WorkerPosition
					}
                }
               
			};
		}
		#endregion ExecuteAsync
	}
}