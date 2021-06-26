using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class PaymentConcessionGetHandler :
		IQueryBaseHandler<PaymentConcessionGetArguments, PaymentConcessionGetResult>
	{
		private readonly IEntityRepository<PaymentConcession> Repository;

		#region Constructors
		public PaymentConcessionGetHandler(IEntityRepository<PaymentConcession> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<PaymentConcessionGetResult>> ExecuteAsync(PaymentConcessionGetArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x => x.Id.Equals(arguments.Id))
				.Select(x => new PaymentConcessionGetResult
				{
					Id = x.Id,
					// Información comercial
					Name = x.Concession.Name,
					Phone = x.Phone,
					Address = x.Address,
					Observations = x.Observations,
                    OnlineCartActivated = x.OnlineCartActivated,
					State = x.Concession.State,
					Type = x.Concession.Type,
					PayinCommission =  x.PayinCommision,
					LiquidationAmountMin = x.LiquidationAmountMin,
					TicketTemplateId = x.TicketTemplateId,
					TicketTemplateName = x.TicketTemplate == null ? "" : x.TicketTemplate.Name,
					CanHasPublicEvent = x.CanHasPublicEvent,
					AllowUnsafePayments = x.AllowUnsafePayments,
					OnPayedEmail = x.OnPayedEmail,
					OnPaymentMediaCreatedUrl = x.OnPaymentMediaCreatedUrl,
					OnPaymentMediaErrorCreatedUrl = x.OnPaymentMediaErrorCreatedUrl,
					GroupEntranceTypeByEvent = x.GroupEntranceTypeByEvent,
					PhotoUrl = x.PhotoUrl
	});

			return new ResultBase<PaymentConcessionGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
