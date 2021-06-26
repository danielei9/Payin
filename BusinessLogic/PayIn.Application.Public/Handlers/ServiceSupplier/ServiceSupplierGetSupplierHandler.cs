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
	public class ServiceSupplierGetSupplierHandler :
		IQueryBaseHandler<ServiceSupplierGetSupplierArguments, ServiceSupplierGetSupplierResult>
	{
		private readonly IEntityRepository<ServiceSupplier> Repository;

		#region Constructors
		public ServiceSupplierGetSupplierHandler(IEntityRepository<ServiceSupplier> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ServiceSupplierGetSupplierResult>> IQueryBaseHandler<ServiceSupplierGetSupplierArguments, ServiceSupplierGetSupplierResult>.ExecuteAsync(ServiceSupplierGetSupplierArguments arguments)
		{
			var items = await Repository.GetAsync();
			var result = items
				.Where(x => x.Id.Equals(arguments.Id))
				.Select(x => new ServiceSupplierGetSupplierResult
				{
					Id = x.Id,
					Login = x.Login,
					Name = x.Name,
					TaxName = x.TaxName,
					TaxNumber = x.TaxNumber,
					TaxAddress = x.TaxAddress
				})
				.ToList()
				;				

			return new ResultBase<ServiceSupplierGetSupplierResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
