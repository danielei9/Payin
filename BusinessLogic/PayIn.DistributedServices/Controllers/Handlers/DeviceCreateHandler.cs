using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using PayIn.Application.Dto.Arguments.Device;
using Xp.Domain;
using PayIn.Domain.Public;
using PayIn.BusinessLogic.Common;

namespace PayIn.DistributedServices.Controllers.Handlers
{
    public class DeviceCreateHandler : IServiceBaseHandler<DeviceMobileCreateArguments>
    {
        private readonly ISessionData SessionData;
        private readonly IEntityRepository<Device> Repository;
		private readonly IEntityRepository<Platform> RepositoryPlatform;

        #region Constructors
        public DeviceCreateHandler(
            IEntityRepository<Device> repository,
			IEntityRepository<Platform> repositoryPlatform,
			ISessionData sessionData
        )
        {
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryPlatform == null) throw new ArgumentNullException("repositoryPlatform");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

            Repository = repository;
			RepositoryPlatform = repositoryPlatform;
			SessionData = sessionData;
        }
        #endregion Constructors

        #region ExecuteAsync
        async Task<dynamic> IServiceBaseHandler<DeviceMobileCreateArguments>.ExecuteAsync(DeviceMobileCreateArguments arguments)
        {
			var now = DateTime.Now;

			var platform = (await RepositoryPlatform.GetAsync())
				.Where(x => x.Type == arguments.Type)
				.FirstOrDefault();

			var devices = (await Repository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login &&
					x.Platform.Type == arguments.Type
				)
				//.ToList()
				;
			foreach (var device in devices)
				await Repository.DeleteAsync(device);

            var item = new Device
            {
				Platform = platform,
                Token = arguments.Token,
				CreatedAt = now,
                Login = SessionData.Login
            };
            await Repository.AddAsync(item);

            return item;
        }
        #endregion ExecuteAsync
    }
}
