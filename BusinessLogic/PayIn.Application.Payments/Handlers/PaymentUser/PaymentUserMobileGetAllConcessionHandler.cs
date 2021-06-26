using PayIn.Application.Dto.Payments.Arguments.PaymentUser;
using PayIn.Application.Dto.Payments.Results.PaymentUser;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
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
	public class PaymentUserMobileGetAllConcessionHandler :
		IQueryBaseHandler<PaymentUserMobileGetAllConcessionArguments, PaymentUserMobileGetAllConcessionResult>
	{
		private readonly IEntityRepository<PaymentUser> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public PaymentUserMobileGetAllConcessionHandler(IEntityRepository<PaymentUser> repository, ISessionData sessionData)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;

			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<PaymentUserMobileGetAllConcessionResult>> IQueryBaseHandler<PaymentUserMobileGetAllConcessionArguments, PaymentUserMobileGetAllConcessionResult>.ExecuteAsync(PaymentUserMobileGetAllConcessionArguments arguments)
		{
			var item = (await Repository.GetAsync())
			.Where(x => x.Login == SessionData.Login && x.State == PaymentUserState.Active && x.Login != x.Concession.Concession.Supplier.Login); 

			var result = item
			.Select(x => new
			{
				Id = x.Id,
				ConcessionId = x.ConcessionId,
				SupplierName = x.Concession.Concession.Supplier.Name,
				ConcessionName = x.Concession.Concession.Name,
				State = x.State
			})
			.Select(x => new PaymentUserMobileGetAllConcessionResult
			{
				Id = x.Id,
				ConcessionId = x.ConcessionId,
				SupplierName = x.SupplierName,
				ConcessionName = x.ConcessionName,
				State = x.State
			});

			return new ResultBase<PaymentUserMobileGetAllConcessionResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
