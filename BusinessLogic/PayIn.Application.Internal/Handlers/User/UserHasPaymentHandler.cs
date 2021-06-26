using PayIn.Application.Dto.Internal.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Internal.Handlers
{
	public class UserHasPaymentHandler :
		IServiceBaseHandler<UserHasPaymentArguments>
	{
		private readonly SessionData SessionData;
		private readonly IEntityRepository<User> Repository;

		#region Construtors
		public UserHasPaymentHandler(
			SessionData sessionData,
			IEntityRepository<User> repository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			SessionData = sessionData;
			Repository = repository;
		}
		#endregion Construtors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(UserHasPaymentArguments arguments)
		{
			return (await Repository.GetAsync())
				.Where(x => x.Login == SessionData.Login)
				.Any();
		}
		#endregion ExecuteAsync
	}
}
