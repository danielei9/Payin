using PayIn.Application.Dto.Arguments;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceUserSubscribeHandler :
		IServiceBaseHandler<ServiceUserSubscribeArguments>
	{
		private readonly IEntityRepository<ServiceUser> Repository;

		#region Constructors
		public ServiceUserSubscribeHandler(IEntityRepository<ServiceUser> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceUserSubscribeArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			item.State = ServiceUserState.Active;

			//Desbloquear el usuario del sistema en "seguridad"
			var securityRepository = new Infrastructure.Security.SecurityRepository();
			var arg = new Dto.Security.Arguments.AccountUnlockUserByEmailArguments { Email = item.Login };
			var usr = securityRepository.UnlockUserByEmail(arg);

			return item;
		}
		#endregion ExecuteAsync
	}
}
