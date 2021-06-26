using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentConcessionPurseGetAllHandler :
		IQueryBaseHandler<PaymentConcessionPurseGetAllArguments, PaymentConcessionPurseGetAllResult>
	{
		private readonly IEntityRepository<PaymentConcessionPurse> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public PaymentConcessionPurseGetAllHandler(IEntityRepository<PaymentConcessionPurse> repository,ISessionData sessionData)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
						
			Repository = repository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<PaymentConcessionPurseGetAllResult>> IQueryBaseHandler<PaymentConcessionPurseGetAllArguments, PaymentConcessionPurseGetAllResult>.ExecuteAsync(PaymentConcessionPurseGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync())
			.Where(x => x.PurseId == arguments.Id);

			var result = items
				.Where(x => x.PaymentConcession.Concession.Supplier.Login != SessionData.Login)
			.Select(x => new PaymentConcessionPurseGetAllResult
			{
				Id = x.Id,
				State = x.State,
				ConcessionId = x.PaymentConcessionId,
				Concession = x.PaymentConcession.Concession.Supplier.Name
			});
			return new ResultBase<PaymentConcessionPurseGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
