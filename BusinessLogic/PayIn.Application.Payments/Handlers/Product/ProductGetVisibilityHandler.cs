using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ProductGetVisibilityHandler :
		IQueryBaseHandler<ProductGetVisibilityArguments, ProductGetVisibilityResult>
	{
		private readonly IEntityRepository<Product> Repository;
		
		#region Constructors
		public ProductGetVisibilityHandler(
			IEntityRepository<Product> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ProductGetVisibilityResult>> ExecuteAsync(ProductGetVisibilityArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id);

			var result = item
				.Select(x => new ProductGetVisibilityResult
				{
					Id = x.Id,
					Visibility = x.Visibility,
				 });

			return new ResultBase<ProductGetVisibilityResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
