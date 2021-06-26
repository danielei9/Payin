using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceGroupGetSelectorHandler :
		IQueryBaseHandler<ServiceGroupGetSelectorArguments, ServiceGroupGetSelectorResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<ServiceConcession> RepositoryServiceConcession { get; set; }
		[Dependency] public IEntityRepository<ServiceGroup> Repository { get; set; }
		
		#region ExecuteAsync
		public async Task<ResultBase<ServiceGroupGetSelectorResult>> ExecuteAsync(ServiceGroupGetSelectorArguments arguments)
		{
			// Cargando concesiones
			var concessionIds = (await RepositoryServiceConcession.GetAsync())
				.Where(x => x.Supplier.Login == SessionData.Login ||
							x.Supplier.Workers
								.Any(y => y.Login == SessionData.Login)
				)
				.Select(x => x.Id)
				.ToList();


			var result = (await Repository.GetAsync())
				.Where(x => 
					x.Name.Contains(arguments.Filter) &&
					concessionIds.Contains(x.Category.ServiceConcessionId)
				)
				.GroupBy(y => y.Name)
				.Select(z => z.FirstOrDefault())			
				.ToList()
				.Select (x => new ServiceGroupGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				 });

			return new ResultBase<ServiceGroupGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
