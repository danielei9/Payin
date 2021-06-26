using PayIn.Application.Dto.Payments.Arguments.Promotion;
using PayIn.Application.Dto.Payments.Results.Promotion;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PromotionCheckHandler :
		IQueryBaseHandler<PromotionCheckArguments, PromotionCheckResult>
	{
		private readonly IEntityRepository<Promotion> Repository;
		private readonly IEntityRepository<Ticket> TicketRepository;
		private readonly IEntityRepository<PromoCondition> PromoConditionRepository;


		#region Constructor
		public PromotionCheckHandler(
			IEntityRepository<Promotion> repository,
			IEntityRepository<Ticket> ticketRepository, 
			IEntityRepository<PromoCondition> promoConditionRepository
		)
		{
			if (repository == null) throw new ArgumentNullException("Repository");
			if (ticketRepository == null) throw new ArgumentNullException("TicketRepository");
			if (promoConditionRepository == null) throw new ArgumentNullException("PromoConditionRepository");

			Repository = repository;
			TicketRepository = ticketRepository;
			PromoConditionRepository = promoConditionRepository;
		}

		#endregion Constructor

		#region ExecuteAsync
		public async Task<ResultBase<PromotionCheckResult>> ExecuteAsync(PromotionCheckArguments arguments)
		{
			//Obtención de ticket
			var ticket = await TicketRepository.GetAsync();

			if(ticket == null)
				throw new ArgumentNullException("ticket");

			//Comprobación de Código
			//var promotion = 

			//foreach (item in ) 
			//Si es correcto 
			return null;
		}
		#endregion ExecuteAsync
	}
}
