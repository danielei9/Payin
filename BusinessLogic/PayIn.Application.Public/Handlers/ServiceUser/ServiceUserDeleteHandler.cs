using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceUserDeleteHandler :
		IServiceBaseHandler<ServiceUserDeleteArguments>
	{
		[Dependency] public IEntityRepository<ServiceUser> Repository { get; set; }
		[Dependency] public IEntityRepository<GreyList> RepositoryGreyList { get; set; }

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceUserDeleteArguments>.ExecuteAsync(ServiceUserDeleteArguments arguments)
		{
			var serviceUser = (await Repository.GetAsync(arguments.Id));

			serviceUser.State = ServiceUserState.Deleted;

			//Bloquear el usuario del sistema en "seguridad"
			var securityRepository = new PayIn.Infrastructure.Security.SecurityRepository();
			var arg = new PayIn.Application.Dto.Security.Arguments.AccountLockUserArguments { Email = serviceUser.Login };
			var usr = securityRepository.LockUser(arg);

			// Añadir lineas para despersonalizar las tarjetas asignadas
			var now = DateTime.Now.ToUTC();
			if (serviceUser.OnwnerCards != null)
			{
				foreach (ServiceCard serviceCard in serviceUser.OnwnerCards)
				{
					// Añadir linea para anonimizar la tarjeta a la LG
					var createItem = new GreyList
					{
						Uid = serviceCard.Uid,
						RegistrationDate = now,
						Action = PayIn.Domain.Transport.GreyList.ActionType.UnPersonalizeCard,
						Field = "",
						NewValue = "",
						Resolved = false,
						ResolutionDate = null,
						Machine = PayIn.Domain.Transport.GreyList.MachineType.All,
						Source = PayIn.Domain.Transport.GreyList.GreyListSourceType.Payin,
						State = GreyList.GreyListStateType.Active
					};
					await RepositoryGreyList.AddAsync(createItem);
				}
			}
			return serviceUser;
		}
		#endregion ExecuteAsync
	}
}
