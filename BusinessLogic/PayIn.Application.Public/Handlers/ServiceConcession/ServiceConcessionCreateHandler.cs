using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.ServiceConcession;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceConcessionCreateHandler :
	IServiceBaseHandler<ServiceConcessionCreateArguments>
	{
		[Dependency] private ISessionData                         SessionData               { get; set; }
		[Dependency] private IEntityRepository<ServiceConcession> Repository                { get; set; }
		[Dependency] private IEntityRepository<ServiceSupplier>   RepositoryServiceSupplier { get; set; }

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceConcessionCreateArguments>.ExecuteAsync(ServiceConcessionCreateArguments arguments)
		{
			var supplier = (await RepositoryServiceSupplier.GetAsync())
				.Where(x => x.Login == SessionData.Login)
				.FirstOrDefault();
			
			var itemServiceConcession = new ServiceConcession
			{
				Name = arguments.Name,
				Supplier = supplier,
				Type = arguments.Type,
				MaxWorkers = arguments.MaxWorkers
			};
			await Repository.AddAsync(itemServiceConcession);

			return itemServiceConcession;
		}
		#endregion ExecuteAsync
	}
}
