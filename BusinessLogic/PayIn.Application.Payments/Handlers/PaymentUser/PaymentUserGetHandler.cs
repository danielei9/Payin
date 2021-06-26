using PayIn.Application.Dto.Payments.Arguments.PaymentUser;
using PayIn.Application.Dto.Payments.Results.PaymentUser;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using serV = PayIn.Domain.Payments.PaymentUser;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class PaymentUserGetHandler :
		IQueryBaseHandler<PaymentUserGetArguments, PaymentUserGetResult>
	{
		private readonly IEntityRepository<serV> _Repository;
		private readonly ISessionData _SessionData;

		#region Constructors
		public PaymentUserGetHandler(IEntityRepository<serV> repository, ISessionData sessionData)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;

			if (sessionData == null)
				throw new ArgumentNullException("sessionData");
			_SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<PaymentUserGetResult>> ExecuteAsync(PaymentUserGetArguments arguments)
		{
			var items = (await _Repository.GetAsync());

			var result = items
				.Where(x => x.Id == arguments.Id)
				.Select(x => new PaymentUserGetResult
				{
					Id = x.Id,
					Name = x.Name,
					Login = x.Login,
					State = x.State,
					ConcessionId = x.ConcessionId
				});
			return new ResultBase<PaymentUserGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
