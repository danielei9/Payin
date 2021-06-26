using PayIn.Application.Dto.Arguments.ServiceCheckPoint;
using PayIn.Application.Dto.Results.ServiceCheckPoint;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceCheckPointGetItemChecksHandler :
		IQueryBaseHandler<ServiceCheckPointGetItemChecksArguments, ServiceCheckPointGetItemChecksResult>
	{
		private readonly IEntityRepository<ServiceCheckPoint> _Repository;

		#region Constructors
		public ServiceCheckPointGetItemChecksHandler(IEntityRepository<ServiceCheckPoint> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceCheckPointGetItemChecksResult>> ExecuteAsync(ServiceCheckPointGetItemChecksArguments arguments)
		{
			var items = await _Repository.GetAsync();

			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x => (
					x.Name.Contains(arguments.Filter) ||
					((x.Tag != null) && (x.Tag.Reference.Contains(arguments.Filter))) ||
					((x.Supplier != null) && (x.Supplier.Name.Contains(arguments.Filter)))
				));

			var result = items
				.Where(x => x.ItemId == arguments.Id)
				.Select(x => new ServiceCheckPointGetItemChecksResult
				{
					Id = x.Id,
					Latitude = x.Latitude,
					Longitude = x.Longitude,
					Name = x.Name,
					Reference = x.Tag != null ? x.Tag.Reference : "",
					Type = x.Type,					
					SupplierName = x.Supplier.Name,
					FormsCount = x.FormAssigns.Count()
				})
				.OrderBy(x => new { x.Reference });

			return new ResultBase<ServiceCheckPointGetItemChecksResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
