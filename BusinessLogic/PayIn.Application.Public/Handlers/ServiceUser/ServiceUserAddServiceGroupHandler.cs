using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class ServiceUserAddServiceGroupHandler : IServiceBaseHandler<ServiceUserAddServiceGroupArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<ServiceConcession> ServiceConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceUser> ServiceUserRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceGroup> ServiceGroupRepository { get; set; }
		[Dependency] public IEntityRepository<GreyList> GreyListRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceUserAddServiceGroupArguments arguments)
		{
			// Cargando concesiones
			var concessionIds = (await ServiceConcessionRepository.GetAsync())
				.Where(x =>
                    (x.Supplier.Login == SessionData.Login) ||
					(x.Supplier.Workers
						.Any(y => y.Login == SessionData.Login)
                    )
				)
				.Select(x => x.Id)
				.ToList();

			// Cargando usuario
			var serviceUser = (await ServiceUserRepository.GetAsync("Groups", "Card"))
				.Where(x =>
					concessionIds.Contains(x.ConcessionId) &&
					x.Id == arguments.ServiceUserId
				)
				.FirstOrDefault();
			if (serviceUser == null)
				throw new ApplicationException("El usuario no está afiliado");

			// Cargando grupo
			var serviceGroup = (await ServiceGroupRepository.GetAsync()) //("Users"))
				.Where(x =>
					concessionIds.Contains(x.Category.ServiceConcessionId) &&
					x.Id == arguments.ServiceGroupId
				)
				.FirstOrDefault();
			if (serviceGroup == null)
				throw new ApplicationException("El grupo no existe");

			if (serviceUser.Groups.Contains(serviceGroup))
				throw new ApplicationException(string.Format("El usuario '{0}' ya está asociado en el grupo '{1}'", serviceUser.Login, serviceGroup.Name));
			
			//serviceGroup.Users.Add(serviceUser);
			serviceUser.Groups.Add(serviceGroup);

			// Añadir linea a LG
			if (serviceUser.Card != null)
			{
				var createItem = new GreyList
				{
					Uid = serviceUser.Card.Uid,
					RegistrationDate = DateTime.Now.ToUTC(),
					Action = GreyList.ActionType.IncreaseUnities,
					Field = "GI",
					NewValue = arguments.ServiceGroupId.ToString(),
					Resolved = false,
					ResolutionDate = null,
					Machine = GreyList.MachineType.All,
					Source = GreyList.GreyListSourceType.PayFalles,
					State = GreyList.GreyListStateType.Active
				};
				await GreyListRepository.AddAsync(createItem);
			}

			return serviceUser;
		}
		#endregion ExecuteAsync
	}
}
