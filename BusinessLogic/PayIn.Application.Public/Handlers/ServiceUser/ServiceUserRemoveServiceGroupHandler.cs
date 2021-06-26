using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class ServiceUserRemoveServiceGroupHandler : IServiceBaseHandler<ServiceUserRemoveServiceGroupArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<ServiceUser> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceGroup> ServiceGroupRepository { get; set; }
		[Dependency] public IEntityRepository<GreyList> RepositoryGreyList { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceUserRemoveServiceGroupArguments arguments)
		{
            var serviceUser = (await Repository.GetAsync(arguments.UserId, "Groups", "Card"));
            if (serviceUser == null)
				throw new ArgumentNullException("ServiceUserId");

			var serviceGroup = (await ServiceGroupRepository.GetAsync(arguments.GroupId));
			serviceUser.Groups.Remove(serviceGroup);

			// Añadir linea para remover el grupo a la LG
			if (serviceUser.Card != null)
			{
				var createItem = new GreyList
				{
					Uid = serviceUser.Card.Uid,
					RegistrationDate = DateTime.Now.ToUTC(),
					Action = GreyList.ActionType.DiscountUnities,
					Field = "GI",
					NewValue = arguments.GroupId.ToString(),
					Resolved = false,
					ResolutionDate = null,
					Machine = GreyList.MachineType.All,
					Source = GreyList.GreyListSourceType.PayFalles,
					State = GreyList.GreyListStateType.Active
				};
				await RepositoryGreyList.AddAsync(createItem);
			}

			return null;
		}
		#endregion ExecuteAsync
	}
}
