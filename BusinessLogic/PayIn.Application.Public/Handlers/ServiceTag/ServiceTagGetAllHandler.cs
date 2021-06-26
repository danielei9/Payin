using PayIn.Application.Dto.Arguments.ServiceTag;
using PayIn.Application.Dto.Results.ServiceTag;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceTagGetAllHandler :
		IQueryBaseHandler<ServiceTagGetAllArguments, ServiceTagGetAllResult>
	{
		private readonly IEntityRepository<ServiceTag> _Repository;

		#region Constructors
		public ServiceTagGetAllHandler(IEntityRepository<ServiceTag> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceTagGetAllResult>> ExecuteAsync(ServiceTagGetAllArguments arguments)
		{
			var items = await _Repository.GetAsync();

			if (!arguments.Filter.IsNullOrEmpty())
				items = items.Where(x => (
					(x.Reference.Contains(arguments.Filter)) ||
					((x.Supplier != null) && (x.Supplier.Name.Contains(arguments.Filter)))
				));

			var result = items
				.Select(x => new
				{
					Id = x.Id,
					Reference = x.Reference,
					Type = x.Type,
					SupplierId = x.SupplierId,
					SupplierName = x.Supplier != null ? x.Supplier.Name : ""
				})
				.OrderBy(x => new
				{
					x.SupplierName,
					x.Reference
				})
				.ToList()
				.Select(x => new ServiceTagGetAllResult
				{
					Id = x.Id,
					Reference = x.Reference,
					Type = x.Type,
					TypeLabel = x.Type.ToEnumAlias(),
					SupplierId = x.SupplierId,
					SupplierName = x.SupplierName
				});

			return new ResultBase<ServiceTagGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
