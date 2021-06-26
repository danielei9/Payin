using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceIncidenceUpdateHandler :
		IServiceBaseHandler<ServiceIncidenceUpdateArguments>
	{
		[Dependency] public IEntityRepository<ServiceIncidence> ServiceIncidenceRepository { get; set; }

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceIncidenceUpdateArguments>.ExecuteAsync(ServiceIncidenceUpdateArguments arguments)
		{
			var serviceIncidence = (await ServiceIncidenceRepository.GetAsync())
				.Where(x =>
						x.Id == arguments.Id
				)
				.FirstOrDefault();

			if (serviceIncidence == null)
				throw new ArgumentException("serviceIncidenceId");

			serviceIncidence.InternalObservations = arguments.InternalObservations ?? "";
			serviceIncidence.State = arguments.State;

			return serviceIncidence;
		}
		#endregion ExecuteAsync
	}
}
