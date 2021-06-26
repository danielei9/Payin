using PayIn.Application.Dto.Arguments.ServiceTag;
using PayIn.Application.Dto.Results.ServiceTag;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceTagGetSelectorHandler :
		IQueryBaseHandler<ServiceTagGetSelectorArguments, ServiceTagGetSelectorResult>
	{
		private readonly IEntityRepository<ServiceTag> _Repository;

		#region Constructors
		public ServiceTagGetSelectorHandler(IEntityRepository<ServiceTag> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceTagGetSelectorResult>> ExecuteAsync(ServiceTagGetSelectorArguments arguments)
		{
			var items = await _Repository.GetAsync();

			var result = items
				.Where(x =>
					x.Reference.Contains(arguments.Filter)
				)
				.Select(x => new ServiceTagGetSelectorResult
				{
					Id = x.Id,
					Value = x.Reference
				})
				.ToList();

			return new ResultBase<ServiceTagGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
