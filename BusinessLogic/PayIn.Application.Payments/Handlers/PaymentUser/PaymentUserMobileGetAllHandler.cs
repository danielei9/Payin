using PayIn.Application.Dto.Payments.Arguments.PaymentUser;
using PayIn.Application.Dto.Payments.Results.PaymentUser;
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
	public class PaymentUserMobileGetAllHandler :
		IQueryBaseHandler<PaymentUserMobileGetAllArguments, PaymentUserMobileGetAllResult>
	{
		private readonly SessionData SessionData;
		private readonly IEntityRepository<PaymentUser> Repository;

		#region Contructors
		public PaymentUserMobileGetAllHandler(
			SessionData sessionData,
			IEntityRepository<PaymentUser> repository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");

			SessionData = sessionData;
			Repository = repository;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<ResultBase<PaymentUserMobileGetAllResult>> ExecuteAsync(PaymentUserMobileGetAllArguments arguments)
		{
            var result = (await Repository.GetAsync())
				.Where(x => 
					x.Login == SessionData.Login && 
					(x.State == PaymentUserState.Active || x.State == PaymentUserState.Blocked || x.State == PaymentUserState.Pending)
				)
				.Select(x => new PaymentUserMobileGetAllResult
                {
					Id = x.Id,
                    ConcessionName = x.Concession.Concession.Name,
                    State = x.State
				})
				.Skip(arguments.Skip)
				.Take(arguments.Top)
				.ToList();



			return new ResultBase<PaymentUserMobileGetAllResult> { Data = result };
		}
        #endregion ExecuteAsync
    }
}
