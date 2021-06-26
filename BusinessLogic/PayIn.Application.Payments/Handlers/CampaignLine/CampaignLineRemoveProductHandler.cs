using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class CampaignLineRemoveProductHandler :
		IServiceBaseHandler<CampaignLineRemoveProductArguments>
	{
		private readonly IEntityRepository<CampaignLine> Repository;
		private readonly IEntityRepository<Product> ProductRepository;

		#region Constructors
		public CampaignLineRemoveProductHandler(
			IEntityRepository<CampaignLine> repository,
			IEntityRepository<Product> productRepository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (productRepository == null) throw new ArgumentNullException("productRepository");

			Repository = repository;
			ProductRepository = productRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(CampaignLineRemoveProductArguments arguments)
		{
			var product = (await ProductRepository.GetAsync(arguments.ProductId));
			if (product == null)
				throw new ArgumentNullException("ProductId");

			var item = (await Repository.GetAsync(arguments.Id, "Products"));
			if (item == null)
				throw new ArgumentNullException("Id");

			item.Products.Remove(product);

			return item;
		}
		#endregion ExecuteAsync
	}
}
