using Microsoft.Practices.Unity;
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
	public class ServiceSupplierGetHandler :
		IQueryBaseHandler<ServiceSupplierGetArguments, ServiceSupplierGetResult>
	{
		[Dependency] public IEntityRepository<ServiceSupplier> Repository { get; set; }

		#region ExecuteAsync
		async Task<ResultBase<ServiceSupplierGetResult>> IQueryBaseHandler<ServiceSupplierGetArguments, ServiceSupplierGetResult>.ExecuteAsync(ServiceSupplierGetArguments arguments)
		{
			var items = await Repository.GetAsync();
			var result = items
				.Where(x => x.Id.Equals(arguments.Id))
				.Select(x => new ServiceSupplierGetResult
				{
					Login = x.Login,
					Name = x.Name,
					TaxName = x.TaxName,
					TaxNumber = x.TaxNumber,
					TaxAddress = x.TaxAddress
				})
				.ToList()
				;				

			return new ResultBase<ServiceSupplierGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
