using PayIn.Application.Dto.Arguments.Device;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.DistributedServices.Controllers.Handlers
{
	public class DeviceCreateHandler : IServiceBaseHandler<DeviceCreateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<Device> Repository;
		private readonly IEntityRepository<Platform> RepositoryPlatform;
		private readonly IPushService PushService;

		#region Constructors
		public DeviceCreateHandler(
			ISessionData sessionData,
			IEntityRepository<Device> repository,
			IEntityRepository<Platform> repositoryPlatform,
			IPushService pushService
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryPlatform == null) throw new ArgumentNullException("repositoryPlatform");
			if (pushService == null) throw new ArgumentNullException("pushService");

			SessionData = sessionData;
			Repository = repository;
			RepositoryPlatform = repositoryPlatform;
			PushService = pushService;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(DeviceCreateArguments arguments)
		{
			var now = DateTime.Now;

			var item = (await Repository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login &&
					x.Platform.Type == arguments.Type)
				.FirstOrDefault();

			if (item == null)
			{
				var platform = (await RepositoryPlatform.GetAsync())
					.Where(x => x.Type == arguments.Type)
					.FirstOrDefault();

				item = new Device
				{
					Platform = platform,
					Token = arguments.Token,
					CreatedAt = now,
					Login = SessionData.Login
				};
				await Repository.AddAsync(item);
			}
			else
				item.Token = arguments.Token;

			var badItems = (await Repository.GetAsync())
				.Where(x =>
					x.Login != SessionData.Login &&
					x.Token == arguments.Token)
				.ToList();

			foreach (var badItem in badItems) {
				await Repository.DeleteAsync(badItem);
			}

			return item;
		}
		#endregion ExecuteAsync
	}
}
