using PayIn.Application.Dto.Payments.Arguments.Purse;
using PayIn.Application.Dto.Payments.Results.Purse;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Common;
using PayIn.Common.Security;
using PayIn.Domain.Payments;
using PayIn.Domain.Payments.Infrastructure;

namespace PayIn.Application.Public.Handlers
{
	public class PurseGetUsersHandler :
		IQueryBaseHandler<PurseGetUsersArguments, PurseGetUsersResult>
	{
		private readonly IEntityRepository<PaymentMedia> PublicRepository;
		private readonly SessionData SessionData;
		private readonly IInternalService InternalService;

		#region Constructors
		public PurseGetUsersHandler(IEntityRepository<PaymentMedia> publicRepository, IInternalService internalService,SessionData sessionData)
		{
			if (publicRepository == null)throw new ArgumentNullException("publicRepository");
			if (internalService == null) throw new ArgumentNullException("internalService");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			PublicRepository = publicRepository;
			InternalService = internalService;
			SessionData = sessionData;

		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<PurseGetUsersResult>> ExecuteAsync(PurseGetUsersArguments arguments)
		{

			var items = (await PublicRepository.GetAsync())
				.Where(x => x.Type == PaymentMediaType.Purse && x.PurseId == arguments.Id && x.Login != SessionData.Login)
				.Select(x => new
				{
					Id = x.Id,
					Login = x.Login
				})
				.Select(x => new PurseGetUsersResult
				{
					Id = x.Id,
					Login = x.Login					
				})
			.ToList();

			foreach (var pMedia in items)
			{
				var res = await InternalService.PaymentMediaGetBalanceToRefundAsync(pMedia.Id);
				if (res != null)
					pMedia.Amount = res.Balance;
			}			

			return new ResultBase<PurseGetUsersResult> { Data = items };
		}
		#endregion ExecuteAsync
	}
}
