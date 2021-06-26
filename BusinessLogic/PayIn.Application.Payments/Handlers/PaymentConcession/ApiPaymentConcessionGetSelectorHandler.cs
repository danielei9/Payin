using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ApiPaymentConcessionGetSelectorHandler :
		IQueryBaseHandler<ApiPaymentConcessionGetSelectorArguments, ApiPaymentConcessionGetSelectorResult>
	{
		[Dependency] public ISessionData SessionData { get; set;}
		[Dependency] public IEntityRepository<ServiceConcession> Repository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ApiPaymentConcessionGetSelectorResult>> ExecuteAsync(ApiPaymentConcessionGetSelectorArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
					(
						x.Supplier.Login == SessionData.Login ||
						x.Supplier.Workers
							.Any(y => y.Login == SessionData.Login)
					) &&
					x.State == ConcessionState.Active &&
                    x.Type == ServiceType.Charge
                );
			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x =>
					x.Name.Contains(arguments.Filter)
				);

			var result = items
				.Select(x => new ApiPaymentConcessionGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				});

			return new ResultBase<ApiPaymentConcessionGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
