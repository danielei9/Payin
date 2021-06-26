using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.ServiceConcession;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Application.Dto.Results.ServiceConcession;
using PayIn.Application.Payments.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceConcessionGetAllHandler :
		IQueryBaseHandler<ServiceConcessionGetAllArguments, ServiceConcessionGetAllResult>
	{
		[Dependency] public IEntityRepository<ServiceConcession>	Repository							{ get; set; }
		[Dependency] public ISessionData							SessionData							{ get; set; }


		#region ExecuteAsync
		async Task<ResultBase<ServiceConcessionGetAllResult>> IQueryBaseHandler<ServiceConcessionGetAllArguments, ServiceConcessionGetAllResult>.ExecuteAsync(ServiceConcessionGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x => x.Supplier.Login == SessionData.Login);

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x => 
						x.Name.Contains(arguments.Filter)
					);

			var result = items
				.Select(x => new ServiceConcessionGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					SupplierId = x.Id,
					Zones = x.Zones.Select(y => new ServiceConcessionGetAllResult.Zone
					{
						Id = y.Id,
						Name = y.Name,
						CancelationAmount = y.CancelationAmount
					})
				});

			return new ResultBase<ServiceConcessionGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
