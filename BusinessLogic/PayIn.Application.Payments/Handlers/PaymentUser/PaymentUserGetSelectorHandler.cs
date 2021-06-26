using PayIn.Application.Dto.Payments.Arguments.PaymentUser;
using PayIn.Application.Dto.Payments.Results.PaymentUser;
using PayIn.Domain.Public;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.BusinessLogic.Common;

namespace PayIn.Application.Public.Handlers
{
	public class PaymentUserGetSelectorHandler :
		IQueryBaseHandler<PaymentUserGetSelectorArguments, PaymentUserGetSelectorResult>
	{
		private readonly IEntityRepository<PaymentUser> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public PaymentUserGetSelectorHandler(
			IEntityRepository<PaymentUser> repository, 
			ISessionData sessionData
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			Repository = repository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<PaymentUserGetSelectorResult>> ExecuteAsync(PaymentUserGetSelectorArguments arguments)
		{
			var items = (await Repository.GetAsync())
			.Where(x => x.Concession.Concession.Supplier.Login == SessionData.Login && x.State != Common.PaymentUserState.Deleted && x.State != Common.PaymentUserState.Blocked);

			var result = items
				.Where(x => x.Name.Contains(arguments.Filter))
				.Select(x => new PaymentUserGetSelectorResult
				{
					Id = x.Id,
					Name = x.Name,
					Login = x.Login
				});

			return new ResultBase<PaymentUserGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
