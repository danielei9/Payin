using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class ApiAccessControlGetAllHandler : 
		IQueryBaseHandler<ApiAccessControlGetAllArguments, ApiAccessControlGetAllResult>
	{
		[Dependency] public IEntityRepository<AccessControl> Repository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync

		public async Task<ResultBase<ApiAccessControlGetAllResult>> ExecuteAsync(ApiAccessControlGetAllArguments arguments)
		{
			var accesses = (await Repository.GetAsync())
				.Where(x =>
					(
						(x.PaymentConcession.Concession.Supplier.Login == SessionData.Login) ||
						(x.PaymentConcession.Concession.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login))
					)
				);

			if (!arguments.Filter.IsNullOrEmpty())
				accesses = accesses
					.Where(x =>
						x.Name.Contains(arguments.Filter)
					);

			var result = accesses
				.Select(x => new
				{
					x.Id,
					Name = x.Name.Replace("\"", "'"),
					x.CurrentCapacity,
					x.MaxCapacity,
				})
                .OrderBy(x => x.Name)
				.ToList()
				.Select(x => new ApiAccessControlGetAllResult
				{
					Id = x.Id,
					Name = x.Name,
					CurrentCapacity = x.CurrentCapacity,
					MaxCapacity = x.MaxCapacity
				});

			return new ResultBase<ApiAccessControlGetAllResult> { Data = result }; ;
		}

		#endregion
	}
}
