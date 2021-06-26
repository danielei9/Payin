using PayIn.Application.Dto.Queries;
using PayIn.Application.Dto.Results;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class PaymentMediaGetAllHandler :
		IQueryBaseHandler<PaymentMediaGetAllQuery, PaymentMediaGetAllResult>
	{
		public readonly IEntityRepository<PayIn.Domain.Public.PaymentMedia> _PaymentMediaRepository;

		#region Contructors
		public PaymentMediaGetAllHandler(IEntityRepository<PayIn.Domain.Public.PaymentMedia> paymentMediaRepository)
		{
			if (paymentMediaRepository == null)
				throw new ArgumentNullException("paymentMediaRepository");
			_PaymentMediaRepository = paymentMediaRepository;
		}
		#endregion Contructors

		#region ExecuteAsync
		async Task<ResultBase<PaymentMediaGetAllResult>> IQueryBaseHandler<PaymentMediaGetAllQuery, PaymentMediaGetAllResult>.ExecuteAsync(PaymentMediaGetAllQuery command)
		{
			var paymentMedias = await _PaymentMediaRepository.GetAsync();

			var result = paymentMedias
				.OrderBy(x => x.VisualOrder)
				.Select(x => new PaymentMediaGetAllResult
				{
					Id = x.Id,
					Title = x.Name,
					Subtitle = x.Type.ToString(),
					VisualOrder = x.VisualOrder
				});

			return new ResultBase<PaymentMediaGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
