using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class ApiAccessControlEntryCreateHandler : IServiceBaseHandler<ApiAccessControlEntryCreateArguments>
	{
		[Dependency] public IEntityRepository<AccessControlEntry> Repository { get; set; }
		[Dependency] public IEntityRepository<AccessControl> RepositoryAccessControl { get; set; }
		[Dependency] public IEntityRepository<AccessControlEntrance> RepositoryAccessControlEntrance { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync

		public async Task<dynamic> ExecuteAsync(ApiAccessControlEntryCreateArguments arguments)
		{
			var enclosure = await RepositoryAccessControl.GetAsync(arguments.AccessControlId);
			var entrance = await RepositoryAccessControlEntrance.GetAsync(arguments.EntranceId);
			var people = arguments.People;

			if (arguments.EntryIn == false)
            {
				people *= -1;
            }

			var currentCapacity = enclosure.CurrentCapacity + people;
			if (currentCapacity < 0 || currentCapacity > enclosure.MaxCapacity)
            {
				throw new ApplicationException("Se ha llegado al limite de personas");
            }

			enclosure.CurrentCapacity = currentCapacity;

			var access = new AccessControlEntry()
			{
				AccessControlEntrance = entrance,
				EntryDateTime = DateTime.UtcNow,
				PeopleEntry = people,
				CapacityAfterEntrance = entrance.AccessControl.CurrentCapacity,
				CreatorLogin = SessionData.Login
			};

            await Repository.AddAsync(access);

			return access;
		}

		#endregion
	}
}
