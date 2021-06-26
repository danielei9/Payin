using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Domain.Public;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class ServiceGroupCreateHandler : IServiceBaseHandler<ServiceGroupCreateArguments>
	{
		[Dependency] public IEntityRepository<ServiceGroup> Repository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ServiceGroupCreateArguments arguments)
		{
			var serviceGroup = new ServiceGroup
			{
				Name = arguments.Name,
				CategoryId = arguments.ServiceCategoryId
			};
			await Repository.AddAsync(serviceGroup);

			return serviceGroup;
		}
		#endregion ExecuteAsync
	}
}
