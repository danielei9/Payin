using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ProductVisibilityHandler : 
		IServiceBaseHandler<ProductVisibilityArguments>
	{
		private readonly IEntityRepository<Product> Repository;

		#region Constructors
		public ProductVisibilityHandler(IEntityRepository<Product> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ProductVisibilityArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			item.Visibility = arguments.Visibility;

			return item;
		}
		#endregion ExecuteAsync
	}
}
