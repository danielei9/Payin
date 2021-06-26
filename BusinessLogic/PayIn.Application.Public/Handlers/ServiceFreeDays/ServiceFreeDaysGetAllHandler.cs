using PayIn.Application.Dto.Arguments.ServiceFreeDays;
using PayIn.Application.Dto.Results.ServiceFreeDays;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceFreeDaysGetAllHandler :
		IQueryBaseHandler<ServiceFreeDaysGetAllArguments, ServiceFreeDaysGetAllByConcessionResult>
	{
		private readonly IEntityRepository<ServiceFreeDays> _Repository;

		#region Constructors
		public ServiceFreeDaysGetAllHandler(IEntityRepository<ServiceFreeDays> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ServiceFreeDaysGetAllByConcessionResult>> IQueryBaseHandler<ServiceFreeDaysGetAllArguments, ServiceFreeDaysGetAllByConcessionResult>.ExecuteAsync(ServiceFreeDaysGetAllArguments arguments)
		{
			var items = await _Repository.GetAsync();
			var result = items
				.Select(x => new ServiceFreeDaysGetAllByConcessionResult
				{
					Id = x.Id,
					ConcessionId = x.ConcessionId,
					ConcessionName = x.Concession.Name,
					Name = x.Name,
					From = x.From,
					Until = x.Until
				});

			return new ResultBase<ServiceFreeDaysGetAllByConcessionResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
