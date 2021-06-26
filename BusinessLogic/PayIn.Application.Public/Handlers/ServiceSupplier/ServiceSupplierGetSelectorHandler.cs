using PayIn.Application.Dto.Arguments.ServiceSupplier;
using PayIn.Application.Dto.Results.ServiceSupplier;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceSupplierGetSelectorHandler :
		IQueryBaseHandler<ServiceSupplierGetSelectorArguments, ServiceSupplierGetSelectorResult>
	{
		private readonly IEntityRepository<ServiceSupplier> _Repository;

		#region Constructors
		public ServiceSupplierGetSelectorHandler(IEntityRepository<ServiceSupplier> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ServiceSupplierGetSelectorResult>> IQueryBaseHandler<ServiceSupplierGetSelectorArguments, ServiceSupplierGetSelectorResult>.ExecuteAsync(ServiceSupplierGetSelectorArguments arguments)
		{
			var items = await _Repository.GetAsync();

			var result = items
				.Where(x => x.Name.Contains(arguments.Filter))
				.Select(x => new ServiceSupplierGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				});

			return new ResultBase<ServiceSupplierGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
