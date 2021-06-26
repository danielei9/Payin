using PayIn.Application.Dto.Arguments.ServiceConcession;
using PayIn.Application.Dto.Results.ServiceConcession;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceConcessionGetCommerceHandler :
		IQueryBaseHandler<ServiceConcessionGetCommerceArguments, ServiceConcessionGetCommerceResult>
	{
		private readonly IEntityRepository<ServiceConcession> Repository;


		#region Constructors
		public ServiceConcessionGetCommerceHandler(IEntityRepository<ServiceConcession> repository) 
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceConcessionGetCommerceResult>> ExecuteAsync(ServiceConcessionGetCommerceArguments arguments)
		{
				var items = await Repository.GetAsync();
				var result = items
					.Where(x => x.Id.Equals(arguments.Id))
					.Select(x => new ServiceConcessionGetCommerceResult
					{
						Id = x.Id,
						Name = x.Name,
						Type = x.Type,
						MaxWorkers = x.MaxWorkers,
						State = x.State
					});
				return new ResultBase<ServiceConcessionGetCommerceResult> { Data = result };
			}
				#endregion ExecuteAsync
	}
}
