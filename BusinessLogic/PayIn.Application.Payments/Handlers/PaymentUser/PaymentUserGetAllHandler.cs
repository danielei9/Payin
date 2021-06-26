using PayIn.Application.Dto.Payments.Arguments.PaymentUser;
using PayIn.Application.Dto.Payments.Results.PaymentUser;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class PaymentUserGetAllHandler :
		IQueryBaseHandler<PaymentUserGetAllArguments, PaymentUserGetAllResult>
	{
		private readonly IEntityRepository<PaymentUser> Repository;
		private readonly ISessionData SessionData;


		#region Constructors
		public PaymentUserGetAllHandler(IEntityRepository<PaymentUser> repository, ISessionData sessionData)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<PaymentUserGetAllResult>> ExecuteAsync(PaymentUserGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync())
			.Where(x => x.Concession.Concession.Supplier.Login == SessionData.Login && x.State != Common.PaymentUserState.Deleted && x.State != Common.PaymentUserState.Blocked);

			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x => (
					x.Name.Contains(arguments.Filter) ||
					x.Login.Contains(arguments.Filter)
				));

			var result = items
			.Select(x => new PaymentUserGetAllResult
			{
				Id = x.Id,
				State = x.State,
				Login = x.Login,
				Name = x.Name
			});
			return new ResultBase<PaymentUserGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
