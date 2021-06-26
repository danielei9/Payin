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
	public class DeviceMobileDeleteHandler : IServiceBaseHandler<DeviceMobileDeleteArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<Device> Repository;

		#region Constructors
		public DeviceMobileDeleteHandler(
			ISessionData sessionData,
			IEntityRepository<Device> repository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");

			SessionData = sessionData;
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(DeviceMobileDeleteArguments arguments)
		{
			var now = DateTime.Now;

			var items = (await Repository.GetAsync())
				.Where(x =>
					x.Login == SessionData.Login &&
					x.Platform.Type == arguments.Type);
			foreach(var item in items)
				await Repository.DeleteAsync(item);
			
			return null;
		}
		#endregion ExecuteAsync
	}
}
