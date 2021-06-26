using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ProductUpdateHandler :
		IServiceBaseHandler<ProductUpdateArguments>
	{
		private readonly IEntityRepository<Product> Repository;

		#region Constructors
		public ProductUpdateHandler(
			IEntityRepository<Product> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ProductUpdateArguments arguments)
		{
			var product = (await Repository.GetAsync())
				.Where(x =>
					x.Id == arguments.Id
				)
				.FirstOrDefault();

            product.FamilyId = arguments.FamilyId;
			product.SellableInTpv = arguments.SellableInTpv;
			product.SellableInWeb = arguments.SellableInWeb;
			product.Name = arguments.Name;
            product.Code = arguments.Code;
			product.Description = arguments.Description;
			product.IsVisible = arguments.IsVisible;
			product.Visibility = arguments.Visibility;

			if(arguments.ToConsult == true)
				product.Price = null;
			else product.Price = arguments.Price;

			return product;
		}
		#endregion ExecuteAsync
	}
}
