using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Security.Arguments;
using PayIn.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Security;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceUserUnsubscribeHandler :
		IServiceBaseHandler<ServiceUserUnsubscribeArguments>
	{
		[Dependency] public IEntityRepository<ServiceUser> Repository { get; set; }
		[Dependency] public ServiceCardLockHandler serviceCardLockHandler { get; set; }
		
		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceUserUnsubscribeArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id, "Card", "OnwnerCards"));

			item.State = ServiceUserState.Unsubscribed;

			//Bloquear el usuario del sistema en "seguridad"
			var securityRepository = new SecurityRepository();
			var arg = new AccountLockUserArguments {Email = item.Login};
			var usr = securityRepository.LockUser(arg);

			// Ejecutar el proceso de bloquear todas sus tarjetas
			if (item.Card != null)
			{
				ServiceCardLockArguments lockArguments = new ServiceCardLockArguments(item.Card.Id, item.Card.Uid);
				await serviceCardLockHandler.ExecuteAsync(lockArguments);

				if (item.OnwnerCards != null)
				{
					foreach (ServiceCard card in item.OnwnerCards)
					{
						if (card.Id != item.Card.Id)
						{
							lockArguments = new ServiceCardLockArguments(card.Id, card.Uid);
							await serviceCardLockHandler.ExecuteAsync(lockArguments);
						}

					}
				}
			}
			
			return item.Id;
		}
		#endregion ExecuteAsync
	}
}
