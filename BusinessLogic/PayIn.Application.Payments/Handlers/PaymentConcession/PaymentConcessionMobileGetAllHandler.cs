using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentConcessionMobileGetAllHandler :
		IQueryBaseHandler<PaymentConcessionMobileGetAllArguments, PaymentConcessionMobileGetAllResult>
	{
		private readonly SessionData SessionData;
		private readonly IEntityRepository<PaymentConcession> PaymentsConcessionRepository;

		#region Contructors
		public PaymentConcessionMobileGetAllHandler(
			SessionData sessionData,
			IEntityRepository<PaymentConcession> paymentsConcessionRepository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (paymentsConcessionRepository == null) throw new ArgumentNullException("paymentsConcessionRepository");
			SessionData = sessionData;
			PaymentsConcessionRepository = paymentsConcessionRepository;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<ResultBase<PaymentConcessionMobileGetAllResult>> ExecuteAsync(PaymentConcessionMobileGetAllArguments arguments)
		{
            var result = (await PaymentsConcessionRepository.GetAsync())
				.Where(x => (
                    (
						x.Concession.Supplier.Login == SessionData.Login &&
						x.Concession.State == ConcessionState.Active
					) ||
					x.PaymentWorkers.Any(y =>
						y.Login == SessionData.Login &&
						y.State == WorkerState.Active
					)
                ))
				.Select(x => new PaymentConcessionMobileGetAllResult
				{
					Id = x.Id,
					Phone = x.Phone,
					Name = x.Concession.Name,
					Address = x.Concession.Supplier.TaxAddress,
					Cif = x.Concession.Supplier.TaxNumber,
				});

			return new ResultBase<PaymentConcessionMobileGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
