using PayIn.Application.Dto.Internal.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Common.Exceptions;
using Xp.Domain;

namespace PayIn.Application.Internal.Handlers
{
	[XpLog("User", "Create")]
	public class UserCreateHandler :
		IServiceBaseHandler<UserCreateArguments>
	{
		private readonly SessionData SessionData;
		private readonly IEntityRepository<User> Repository;

		#region Construtors
		public UserCreateHandler(
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
		public async Task<dynamic> ExecuteAsync(UserCreateArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Login == SessionData.Login)
				.FirstOrDefault();
			if (item != null)
				throw new XpEntityAlreadyExistsException(UserResources.User);

			item = new User {
				Login = SessionData.Login,
				Pin = arguments.Pin.ToHash()
			};
			await Repository.AddAsync(item);

			return item;
		}
		#endregion ExecuteAsync
	}
}
