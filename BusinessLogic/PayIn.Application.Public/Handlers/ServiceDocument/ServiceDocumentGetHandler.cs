using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceDocumentGetHandler : IQueryBaseHandler<ServiceDocumentGetArguments, ServiceDocumentGetResult>
	{
		[Dependency] public IEntityRepository<ServiceDocument> Repository { get; set; }
		
		#region ExecuteAsync
		public async Task<ResultBase<ServiceDocumentGetResult>> ExecuteAsync(ServiceDocumentGetArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id);

			var result = items
				.Select(x => new
				{
					x.Id,
					x.Since,
					x.Until,
					x.Name
				})
				.ToList()
				.Select(x => new ServiceDocumentGetResult
				{
					Id = x.Id,
					Since = x.Since.ToUTC(),
					Until = x.Until.ToUTC(),
					Name = x.Name
				});

			return new ResultBase<ServiceDocumentGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}

