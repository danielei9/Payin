using PayIn.Application.Dto.Internal.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Internal.Handlers
{
	[XpLog("User", "UpdatePin")]
	public class UserUpdatePinHandler :
		IServiceBaseHandler<UserUpdatePinArguments>
	{
		private readonly SessionData SessionData;
		private readonly IEntityRepository<User> Repository;

		#region Construtors
		public UserUpdatePinHandler(
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
		public async Task<dynamic> ExecuteAsync(UserUpdatePinArguments arguments)
		{
			if (arguments.Pin != arguments.ConfirmPin)
				throw new ArgumentException(UserResources.ConfirmPin, "confirmPin");

			var pinHash = arguments.OldPin.ToHash();

			var item = (await Repository.GetAsync())
				.Where(x => x.Login == SessionData.Login)
				.FirstOrDefault();
			if (item == null)
				throw new ArgumentNullException("login");
			if (item.Pin != arguments.OldPin.ToHash())
			{
				throw new ArgumentException(UserResources.IncorrectPin, "oldPin");
			}
			item.Pin = arguments.Pin.ToHash();

			return item;
		}
		#endregion ExecuteAsync
	}
}
