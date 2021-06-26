using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceDocumentGetAllHandler :
		IQueryBaseHandler<ServiceDocumentGetAllArguments, ServiceDocumentGetAllResult>
	{
		[Dependency] public IEntityRepository<ServiceDocument> Repository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ServiceDocumentGetAllResult>> ExecuteAsync(ServiceDocumentGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
					(x.State == ServiceDocumentState.Active)
				);
			if (!arguments.Filter.IsNullOrEmpty())
			{
				items = items
					.Where(x =>
						(x.Name.Contains(arguments.Filter))
					);
			}

			var result = items
				.Select(x => new
				{
					x.Id,
					x.Since,
					x.Until,
					x.Name,
					x.Url
				})
				.OrderByDescending(x => x.Since)
				.ToList()
				.Select(x => new ServiceDocumentGetAllResult
				{
					Id = x.Id,
					Since = x.Since.ToUTC(),
					Until = x.Until.ToUTC(),
					Name = x.Name,
					Url = x.Url
				});

			return new ResultBase<ServiceDocumentGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}

