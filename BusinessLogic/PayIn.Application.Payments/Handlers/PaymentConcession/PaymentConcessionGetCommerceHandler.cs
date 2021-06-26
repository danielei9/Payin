using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentConcessionGetCommerceHandler :
		IQueryBaseHandler<PaymentConcessionGetCommerceArguments, PaymentConcessionGetCommerceResult>
	{		
		private readonly IEntityRepository<PaymentConcession> Repository;

		#region Constructors
		public PaymentConcessionGetCommerceHandler(IEntityRepository<PaymentConcession> repository) 
		{
			if (repository == null) throw new ArgumentNullException("repository");
						
			Repository = repository;			
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<PaymentConcessionGetCommerceResult>> ExecuteAsync(PaymentConcessionGetCommerceArguments arguments)
		{		
			var result = (await Repository.GetAsync())
				.Where(x => x.ConcessionId == arguments.Id)
				.Select(x => new PaymentConcessionGetCommerceResult
				{
					Id = x.Id,
					// Información fiscal
					TaxNumber = x.Concession.Supplier.TaxNumber,
					TaxName = x.Concession.Supplier.TaxName,
					TaxAddress = x.Concession.Supplier.TaxAddress,
					BankAccountNumber = x.BankAccountNumber,
					FormUrl = x.FormUrl,
					// Información comercial
					Name = x.Concession.Name,
					Phone = x.Phone,
					Address = x.Address,
					Observations = x.Observations,
					// Otros
					PayinCommission = x.PayinCommision,
					LiquidationAmountMin = x.LiquidationAmountMin,
					State = x.Concession.State
				});
			return new ResultBase<PaymentConcessionGetCommerceResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
